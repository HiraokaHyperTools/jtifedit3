using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace jtifedit3 {
    public partial class FillForm : Form {
        public FillForm() {
            InitializeComponent();

            penDash = new Pen(Color.Gray);
            penDash.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        Size sizeImage = new Size(1, 1);
        Bitmap bitmapSource;

        public void SetPic(Bitmap bitmap) {
            bitmapSource = bitmap;
            sizeImage = bitmapSource.Size;
        }

        Pen penDash;

        Rectangle rcPlace;
        float scale;
        Bitmap bitmapNow;

        private void panel2_Resize(object sender, EventArgs e) {
            rcPlace = FitRect3.ZoomFit(panel2.ClientRectangle, sizeImage);
            scale = rcPlace.Width / (float)sizeImage.Width;
            bitmapNow = new Bitmap(Math.Max(1, rcPlace.Width), Math.Max(1, rcPlace.Height));
            using (Graphics cv = Graphics.FromImage(bitmapNow)) {
                cv.DrawImage(bitmapSource, new Rectangle(0, 0, bitmapNow.Width, bitmapNow.Height));
            }
            panel2.BackgroundImage = bitmapNow;
        }

        private void panel2_Paint(object sender, PaintEventArgs e) {
            Graphics cv = e.Graphics;
            Composite(cv, scale, true, bitmapSource.HorizontalResolution);

            if (isDown) {
                Point pointDraw1 = Pic2View(textPos, scale);
                Point pointDraw2 = Pic2View(textPos + textDeltaPos, scale);

                Rectangle rcPaste = Rectangle.FromLTRB(
                    Math.Min(pointDraw1.X, pointDraw2.X),
                    Math.Min(pointDraw1.Y, pointDraw2.Y),
                    Math.Max(pointDraw1.X, pointDraw2.X),
                    Math.Max(pointDraw1.Y, pointDraw2.Y)
                    );

                cv.DrawRectangle(penDash, rcPaste);
            }
        }

        private Rectangle Pic2View(Rectangle rc, float scale, bool toControl) {
            Point pt0 = Pic2View(rc.Location, scale, toControl);
            Point pt1 = Pic2View(new Point(rc.Right, rc.Bottom), scale, toControl);
            return Rectangle.FromLTRB(pt0.X, pt0.Y, pt1.X, pt1.Y);
        }

        Point Pic2View(Point pt, float scale, bool toControl) {
            return new Point(
                (int)((toControl ? rcPlace.X : 0) + pt.X * scale),
                (int)((toControl ? rcPlace.Y : 0) + pt.Y * scale)
                );
        }

        Point Pic2View(Point pt, float scale) {
            return new Point(
                (int)(rcPlace.X + pt.X * scale),
                (int)(rcPlace.Y + pt.Y * scale)
                );
        }

        Point textPos = Point.Empty;
        Size textDeltaPos = Size.Empty;

        bool isDown = false;
        Point pointDown, pointMove;

        private void panel2_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                isDown = true;
                pointDown = pointMove = e.Location;
                textPos = new Point(
                    (int)((pointDown.X - rcPlace.X) / scale),
                    (int)((pointDown.Y - rcPlace.Y) / scale)
                    );
                textDeltaPos = Size.Empty;
            }
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && isDown) {
                pointMove = e.Location;
                textDeltaPos = new Size(
                    (int)((pointMove.X - pointDown.X) / scale),
                    (int)((pointMove.Y - pointDown.Y) / scale)
                    );
                panel2.Invalidate();
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && isDown) {
                isDown = false;
            }
        }

        private void FillForm_Load(object sender, EventArgs e) {
            panel2_Resize(sender, e);
        }

        List<FillInfo> fillInfos = new List<FillInfo>();

        class FillInfo {
            public Rectangle rcFill;
            public Color fillColor;
        }

        public void Composite(Graphics cv, float scale, bool toControl, float dpi) {
            foreach (FillInfo fi in fillInfos) {
                using (Brush brush = new SolidBrush(fi.fillColor)) {
                    cv.FillRectangle(brush, Pic2View(fi.rcFill, scale, toControl));
                }
            }
        }

        public Bitmap CompositeTo(Bitmap bitmapSource) {
            Bitmap bitmapNew = new Bitmap(bitmapSource.Width, bitmapSource.Height, Util.SuggestTrueColorOrGreater(bitmapSource.PixelFormat));
            using (Graphics cv = Graphics.FromImage(bitmapNew)) {
                cv.DrawImageUnscaledAndClipped(bitmapSource, new Rectangle(Point.Empty, bitmapNew.Size));
                Composite(cv, 1, false, bitmapSource.HorizontalResolution);
            }
            bitmapNew.SetResolution(bitmapSource.HorizontalResolution, bitmapSource.VerticalResolution);
            return bitmapNew;
        }

        class Util {
            internal static PixelFormat SuggestTrueColorOrGreater(PixelFormat pixelFormat) {
                switch (pixelFormat) {
                    case PixelFormat.Format32bppArgb:
                    case PixelFormat.Format32bppPArgb:
                    case PixelFormat.Format32bppRgb:
                        return pixelFormat;
                    default:
                        return PixelFormat.Format24bppRgb;
                }
            }
        }

        private void bFillWhite_Click(object sender, EventArgs e) {
            FillWith(Color.White);
        }

        private void FillWith(Color fillColor) {
            Point pointDraw1 = textPos;
            Point pointDraw2 = textPos + textDeltaPos;

            Rectangle rcPaste = Rectangle.FromLTRB(
                Math.Min(pointDraw1.X, pointDraw2.X),
                Math.Min(pointDraw1.Y, pointDraw2.Y),
                Math.Max(pointDraw1.X, pointDraw2.X),
                Math.Max(pointDraw1.Y, pointDraw2.Y)
                );

            if (rcPaste.Width < 1 || rcPaste.Height < 1) {
                return;
            }

            FillInfo fi = new FillInfo();
            fi.rcFill = rcPaste;
            fi.fillColor = fillColor;
            fillInfos.Add(fi);

            textPos = Point.Empty;
            textDeltaPos = Size.Empty;

            panel2.Invalidate();
        }

        private void bFillWith_Click(object sender, EventArgs e) {
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                FillWith(colorDialog.Color);
            }
        }

        private void bFillBlack_Click(object sender, EventArgs e) {
            FillWith(Color.Black);
        }


    }
}