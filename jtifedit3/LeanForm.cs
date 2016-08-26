using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FreeImageAPI;

namespace jtifedit3 {
    public partial class LeanForm : Form {
        FIBITMAP dibSmall;

        public LeanForm(FIBITMAP dib) {
            Size size = FitRect3.Fit(new Rectangle(0, 0, 1000, 1000), new Size((int)FreeImage.GetWidth(dib), (int)FreeImage.GetHeight(dib))).Size;

            FIBITMAP dibRescale = FreeImage.Rescale(dib, size.Width, size.Height, FREE_IMAGE_FILTER.FILTER_BILINEAR);
            try {
                dibSmall = FreeImage.ConvertTo24Bits(dibRescale);
            }
            finally {
                FreeImage.Unload(dibRescale);
            }

            InitializeComponent();
        }

        private void LeanForm_Load(object sender, EventArgs e) {
            Upd();
        }

        /// <summary>
        /// ‰E 0
        /// ‰º 270
        /// </summary>
        double rot = 0;

        internal double newrot = 0;

        private void Upd() {
            if (newrot == 0) {
                panel1.BackgroundImage = FreeImage.GetBitmap(dibSmall);
                return;
            }
            float ox = FreeImage.GetWidth(dibSmall) / 2;
            float oy = FreeImage.GetHeight(dibSmall) / 2;
            FIBITMAP dib2 = FreeImage.RotateEx(dibSmall, (double)newrot, 0, 0, ox, oy, true);
            try {
                panel1.BackgroundImage = FreeImage.GetBitmap(dib2);
            }
            finally {
                FreeImage.Unload(dib2);
            }
        }

        Point pt0, pt1;
        bool drawing = false;

        private void pb_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                newrot = 0;
                Upd();

                pt0 = pt1 = e.Location;
                drawing = true;
                panel1.Refresh();
            }
        }

        private void pb_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                pt1 = e.Location;

                CalcRot();
                drawing = true;
                panel1.Refresh();
            }
        }

        private void CalcRot() {
            float fx = (pt1.X - pt0.X);
            float fy = (pt1.Y - pt0.Y);
            float len = (float)Math.Sqrt(fx * fx + fy * fy);
            if (fx == 0) {
                rot = (fy > 0) ? 90 : 270;
            }
            else {
                rot = Math.Acos(fx / len) / Math.PI * 180;
                rot = (fy > 0) ? 360 - rot : rot;
            }

            if (rot <= 45) {
                newrot = -rot;
            }
            else if (rot <= 135) {
                newrot = -(rot - 90);
            }
            else if (rot <= 225) {
                newrot = -(rot - 180);
            }
            else if (rot <= 315) {
                newrot = -(rot - 270);
            }
            else {
                newrot = 360 - rot;
            }

            lCur.Text = String.Format("ü‚ÌŠp“x {0:0.0}‹A•â³ {1:0.0}‹", rot, newrot);
            lCur.Update();
        }

        const int R = 5;

        private void pb_Paint(object sender, PaintEventArgs e) {
            if (drawing) {
                Graphics cv = e.Graphics;

                cv.DrawLine(Pens.Blue, pt0, pt1);

                RectangleF rc0 = RectangleF.FromLTRB(pt0.X - R, pt0.Y - R, pt0.X + R, pt0.Y + R);
                cv.DrawEllipse(Pens.Blue, rc0);

                RectangleF rc1 = RectangleF.FromLTRB(pt1.X - R, pt1.Y - R, pt1.X + R, pt1.Y + R);
                cv.DrawEllipse(Pens.Blue, rc1);
            }
        }

        private void pb_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                pt1 = e.Location;

                CalcRot();
                drawing = false;
                panel1.Invalidate();

                Upd();
            }
        }

        private void LeanForm_FormClosed(object sender, FormClosedEventArgs e) {
            FreeImage.UnloadEx(ref dibSmall);
        }

        private void bCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void bReplace_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Yes;
            Close();
        }
    }
}