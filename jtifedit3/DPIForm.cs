using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Diagnostics;

namespace jtifedit3 {
    public partial class DPIForm : Form {
        Size pix;
        uint rx, ry;

        public DPIForm(Size pix, uint rx, uint ry) {
            this.pix = pix;
            this.rx = rx;
            this.ry = ry;
            InitializeComponent();
            numrx.Value = rx;
            numry.Value = ry;
        }

        private void numrx_ValueChanged(object sender, EventArgs e) {
            if (!Object.ReferenceEquals(tabControl1.SelectedTab, tabPage1)) return;

            pbNewPixel.Image = r.MakePixel(numrx.Value, numry.Value, pix.Width, pix.Height, pbNewPixel.ClientRectangle);
            pbNewPaper.Image = r.MakePaper(numrx.Value, numry.Value, pix.Width, pix.Height, pbNewPaper.ClientRectangle);
        }

        private void cbNewDPI_TextChanged(object sender, EventArgs e) {
            if (!Object.ReferenceEquals(tabControl1.SelectedTab, tabPage2)) return;

            decimal dpi;
            if (!decimal.TryParse(cbNewDPI.Text, out dpi)) return;

            int npx = (int)((pix.Width * dpi) / rx);
            int npy = (int)((pix.Height * dpi) / ry);

            pbNewPixel.Image = r.MakePixel(dpi, dpi, npx, npy, pbNewPixel.ClientRectangle);
            pbNewPaper.Image = r.MakePaper(dpi, dpi, npx, npy, pbNewPaper.ClientRectangle);
        }

        RUt r = new RUt();

        class RUt {
            Font fontH = new Font("Arial", 15, FontStyle.Regular, GraphicsUnit. Pixel, 1, false);
            Font fontV = new Font("Arial", 15, FontStyle.Regular, GraphicsUnit.Pixel, 1, true);
            StringFormat sfL = new StringFormat();
            StringFormat sfT = new StringFormat();
            StringFormat sfR = new StringFormat();
            StringFormat sfB = new StringFormat();

            public RUt() {
                sfL.Alignment = StringAlignment.Center;
                sfL.LineAlignment = StringAlignment.Near;

                sfR.Alignment = StringAlignment.Center;
                sfR.LineAlignment = StringAlignment.Far;

                sfT.Alignment = StringAlignment.Center;
                sfT.LineAlignment = StringAlignment.Near;
                sfT.FormatFlags |= StringFormatFlags.DirectionVertical;

                sfB.Alignment = StringAlignment.Center;
                sfB.LineAlignment = StringAlignment.Far;
                sfB.FormatFlags |= StringFormatFlags.DirectionVertical;
            }

            const int m = 5;

            public Bitmap MakePixel(decimal rx, decimal ry, int cx, int cy, Rectangle rc) {
                Bitmap pic = new Bitmap(rc.Width, rc.Height);
                using (Graphics cv = Graphics.FromImage(pic)) {
                    float fx = (float)(cx);
                    float fy = (float)(cy);
                    rc.Inflate(-m, -m);
                    RectangleF rc1 = FitRect3.FitF(rc, new SizeF(fx, fy));
                    rc1.Width--;
                    rc1.Height--;
                    cv.FillRectangle(Brushes.WhiteSmoke, rc1);
                    cv.DrawRectangle(Pens.Gray, Rectangle.Truncate(rc1));

                    cv.DrawString(String.Format("{0}px", cx), fontH, Brushes.Blue, new PointF((rc1.X + rc1.Right) / 2, rc1.Y + 2), sfL);
                    cv.DrawString(String.Format("{0}px", cy), fontV, Brushes.Blue, new PointF(rc1.X + 2, (rc1.Y + rc1.Bottom) / 2), sfT);
                }
                return pic;
            }

            public Bitmap MakePaper(decimal rx, decimal ry, int cx, int cy, Rectangle rc) {
                Bitmap pic = new Bitmap(rc.Width, rc.Height);
                using (Graphics cv = Graphics.FromImage(pic)) {
                    cv.Clear(Color.FromKnownColor(KnownColor.Control));
                    float fx = (float)(cx / rx);
                    float fy = (float)(cy / ry);
                    rc.Inflate(-m, -m);
                    RectangleF rc1 = FitRect3.FitF(rc, new SizeF(fx, fy));
                    rc1.Width--;
                    rc1.Height--;
                    cv.FillRectangle(Brushes.WhiteSmoke, rc1);
                    cv.DrawRectangle(Pens.Gray, Rectangle.Truncate(rc1));

                    cv.DrawString(String.Format("{0}dpi", rx), fontH, Brushes.Blue, new PointF((rc1.X + rc1.Right) / 2, rc1.Y + 2), sfL);
                    cv.DrawString(String.Format("{0}dpi", ry), fontV, Brushes.Blue, new PointF(rc1.X + 2, (rc1.Y + rc1.Bottom) / 2), sfT);

                    cv.DrawString(String.Format("{0:0.0}mm", cx / rx * 25.4m), fontH, Brushes.Green, new PointF((rc1.X + rc1.Right) / 2, rc1.Bottom - 2), sfR);
                    cv.DrawString(String.Format("{0:0.0}mm", cy / ry * 25.4m), fontV, Brushes.Green, new PointF(rc1.Right - 2, (rc1.Y + rc1.Bottom) / 2), sfB);
                }
                return pic;
            }
        }

        private void DPIForm_Load(object sender, EventArgs e) {
            numrx_ValueChanged(sender, e);

            pbOrgPixel.Image = r.MakePixel(rx, ry, pix.Width, pix.Height, pbOrgPixel.ClientRectangle);
            pbOrgPaper.Image = r.MakePaper(rx, ry, pix.Width, pix.Height, pbOrgPaper.ClientRectangle);

            cbNewDPI.Text = "300";
        }

        class PUt {
            public static PaperSize Guess(float xmm, float ymm) {
                xmm *= 0.1f;
                ymm *= 0.1f;
                float X = Math.Min(xmm, ymm);
                float Y = Math.Max(xmm, ymm);

                //MessageBox.Show(String.Format("dpi({0},{1}) cxcy({2},{3}) mm({4},{5})", xDPI, yDPI, cx, cy, xmm, ymm));

                { 	// B5
                    float D = 1, Px = 18.2f, Py = 25.72f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B5", 717, 1012);
                }
                {	// A4
                    float D = 1, Px = 21.00f, Py = 29.70f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("A4", 827, 1169);
                }
                {	// B4
                    float D = 1, Px = 25.72f, Py = 36.41f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B4", 1012, 1433);
                }
                { 	// A3
                    float D = 1, Px = 29.7f, Py = 42.0f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("A3", 1169, 1654);
                }
                { 	// B3
                    float D = 1, Px = 36.41f, Py = 51.51f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B3", 1433, 2028);
                }
                { 	// A2
                    float D = 1, Px = 42.00f, Py = 59.40f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("A2", 1654, 2339);
                }
                { 	// B2
                    float D = 1, Px = 51.51f, Py = 72.81f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B2", 2028, 2867);
                }
                { 	// A1
                    float D = 1, Px = 59.41f, Py = 84.10f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("A1", 2339, 3311);
                }
                { 	// B1
                    float D = 1, Px = 72.81f, Py = 103.01f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B1", 2867, 4056);
                }
                { 	// A0
                    float D = 1, Px = 84.10f, Py = 118.89f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("A0", 3311, 4680);
                }
                { 	// B0
                    float D = 1, Px = 103.01f, Py = 145.59f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B0", 4056, 5732);
                }

                return new PaperSize("‚í‚©‚è‚Ü‚¹‚ñ", (int)(xmm / 2.54f * 100), (int)(ymm / 2.54f * 100));
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e) {
            if (e.Action == TabControlAction.Selected) {
                numrx_ValueChanged(sender, e);
                cbNewDPI_TextChanged(sender, e);
            }
        }

        public IDeform Res = null;

        private void bSameDPI_Click(object sender, EventArgs e) {
            if (false) {
            }
            else if (Object.ReferenceEquals(tabControl1.SelectedTab, tabPage1)) {
                ForceDPI p = new ForceDPI();
                p.rx = (uint)numrx.Value;
                p.ry = (uint)numry.Value;

                Res = p;
            }
            else if (Object.ReferenceEquals(tabControl1.SelectedTab, tabPage2)) {
                SameDPI p = new SameDPI();
                if (!decimal.TryParse(cbNewDPI.Text, out p.dpi)) {
                    cbNewDPI.Select();
                    DialogResult = DialogResult.None;
                    return;
                }
                p.npx = (int)((pix.Width * p.dpi) / rx);
                p.npy = (int)((pix.Height * p.dpi) / ry);
                Res = p;
            }
            Close();
        }

        public class ForceDPI : IDeform {
            public uint rx, ry;
        }
        public class SameDPI : IDeform {
            public decimal dpi;
            public int npx, npy;
        }
        public interface IDeform {
        }
    }
}