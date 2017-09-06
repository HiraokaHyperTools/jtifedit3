using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using jtifedit3.Properties;
using System.Xml.Serialization;
using System.Drawing.Imaging;

namespace jtifedit3 {
    public partial class PastePicForm : Form {
        public PastePicForm() {
            InitializeComponent();

            penDash = new Pen(Color.Gray);
            penDash.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        Size sizeImage = new Size(1, 1);
        Bitmap bitmapSource;
        Image imagePaste;
        float scalePaste = 1;

        public void SetPic(Bitmap bitmap) {
            bitmapSource = bitmap;
            sizeImage = bitmapSource.Size;
        }

        public void ImportPic(string fp) {
            imagePaste = new Bitmap(fp);
            textPos = new Point(bitmapSource.Width / 2, bitmapSource.Height / 2);
        }

        private void PastePicForm_Load(object sender, EventArgs e) {
            panel2_Resize(sender, e);
        }

        private void bSave_Click(object sender, EventArgs e) {
            Close();
        }

        Pen penDash;

        Rectangle rcPlace;
        float scale;
        Bitmap bitmapNow;

        Bitmap transparentedBitmap;
        Color newTransparentColor;

        Bitmap getTransparentedBitmap() {
            if (transparentedBitmap == null) {
                if (newTransparentColor != Color.Empty) {
                    transparentedBitmap = new Bitmap(imagePaste);
                    transparentedBitmap.MakeTransparent(newTransparentColor);
                }
                else {
                    transparentedBitmap = (Bitmap)imagePaste;
                }
            }
            return transparentedBitmap;
        }

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
        }

        public void Composite(Graphics cv, float scale, bool toControl, float dpi) {
            Point pointDraw = toControl ? Pic2View(textPos + textDeltaPos, scale) : Mult(textPos + textDeltaPos, scale);

            Rectangle rcPaste;
            int cx = (int)(imagePaste.Width * scalePaste * scale);
            int cy = (int)(imagePaste.Height * scalePaste * scale);
            rcPaste = new Rectangle(
                (int)(pointDraw.X - cx / 2),
                (int)(pointDraw.Y - cy / 2),
                cx,
                cy
                );
            cv.DrawImage(getTransparentedBitmap(), rcPaste);

            if (toControl) {
                cv.DrawRectangle(penDash, rcPaste);
            }
        }

        private Point Mult(Point point, float scale) {
            return new Point((int)(point.X * scale), (int)(point.Y * scale));
        }

        Point Pic2View(Point pt, float scale) {
            return new Point(
                (int)(rcPlace.X + pt.X * scale),
                (int)(rcPlace.Y + pt.Y * scale)
                );
        }

        Point textPos = Point.Empty;
        Size textDeltaPos = Size.Empty;

        private void timerUpdate_Tick(object sender, EventArgs e) {
            timerUpdate.Stop();
            panel2.Refresh();
        }

        private void TriggerUpdate() {
            timerUpdate.Stop();
            timerUpdate.Start();
        }

        private void bLarger_Click(object sender, EventArgs e) {
            multScale(1.1f);
        }

        private void multScale(float mult) {
            scalePaste *= mult;
            panel2.Invalidate();
        }

        private void bSmaller_Click(object sender, EventArgs e) {
            multScale(1 / 1.1f);

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

        public Bitmap CompositeTo(Bitmap bitmapSource) {
            Bitmap bitmapNew = new Bitmap(bitmapSource.Width, bitmapSource.Height, Util.SuggestTrueColorOrGreater(bitmapSource.PixelFormat));
            using (Graphics cv = Graphics.FromImage(bitmapNew)) {
                cv.DrawImageUnscaledAndClipped(bitmapSource, new Rectangle(Point.Empty, bitmapNew.Size));
                Composite(cv, 1, false, bitmapSource.HorizontalResolution);
            }
            bitmapNew.SetResolution(bitmapSource.HorizontalResolution, bitmapSource.VerticalResolution);
            return bitmapNew;
        }

        bool isDown = false;
        Point pointDown, pointMove;

        private void panel2_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                isDown = true;
                pointDown = pointMove = e.Location;
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
                textPos += textDeltaPos;
                textDeltaPos = Size.Empty;
                isDown = false;
            }
        }

        private void b100_Click(object sender, EventArgs e) {
            scalePaste = float.Parse(((Button)sender).Text.Split('%')[0]) / 100;
            panel2.Invalidate();
        }

        private void bOpaque_Click(object sender, EventArgs e) {
            transparentedBitmap = null;
            newTransparentColor = Color.Empty;
            panel2.Invalidate();
        }

        private void bTransparent_Click(object sender, EventArgs e) {
            transparentedBitmap = null;
            newTransparentColor = Color.White;
            panel2.Invalidate();
        }

    }
}