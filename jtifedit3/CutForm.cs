using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace jtifedit3 {
    public partial class CutForm : Form {
        public CutForm() {
            InitializeComponent();

            penDash = new Pen(Color.Gray);
            penDash.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        }

        private void CutForm_Load(object sender, EventArgs e) {
            panel2_Resize(sender, e);
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

        private void bCopyToFile_Click(object sender, EventArgs e) {
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

            if (sfdPic.ShowDialog(this) == DialogResult.OK) {
                try {
                    using (Bitmap picSave = new Bitmap(rcPaste.Width, rcPaste.Height)) {
                        using (Graphics cv = Graphics.FromImage(picSave)) {
                            cv.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                            cv.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                            cv.DrawImage(bitmapSource, new Rectangle(Point.Empty, picSave.Size), rcPaste, GraphicsUnit.Pixel);
                        }
                        picSave.SetResolution(bitmapSource.HorizontalResolution, bitmapSource.VerticalResolution);
                        switch (Path.GetExtension(sfdPic.FileName).ToLowerInvariant()) {
                            case ".png":
                            default:
                                picSave.Save(sfdPic.FileName, ImageFormat.Png);
                                break;
                            case ".jpg":
                            case ".jpeg":
                                picSave.Save(sfdPic.FileName, ImageFormat.Jpeg);
                                break;
                            case ".bmp":
                                picSave.Save(sfdPic.FileName, ImageFormat.Bmp);
                                break;
                            case ".gif":
                                picSave.Save(sfdPic.FileName, ImageFormat.Gif);
                                break;
                        }
                    }
                    //lSaved.Text = sfdPic.FileName;
                }
                catch (Exception err) {
                    MessageBox.Show(this, "保存に失敗しました。\n\n" + err);
                }
            }
        }

        private void bExplorer_Click(object sender, EventArgs e) {
            if (File.Exists(sfdPic.FileName)) {
                try {
                    Process.Start("explorer.exe", "/e,/select,\"" + sfdPic.FileName + "\"");
                }
                catch (Exception err) {
                    MessageBox.Show(this, "エクスプローラーの起動に失敗しました。\n\n" + err);
                }
            }
        }

        private void bPaint_Click(object sender, EventArgs e) {
            if (File.Exists(sfdPic.FileName)) {
                try {
                    Process.Start("mspaint.exe", "\"" + sfdPic.FileName + "\"");
                }
                catch (Exception err) {
                    MessageBox.Show(this, "ペイントの起動に失敗しました。\n\n" + err);
                }
            }
        }

    }
}