using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using jtifedit3.Properties;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace jtifedit3 {
    public partial class TextForm : Form {
        public TextForm() {
            InitializeComponent();

            bindingSource.DataSource = new TextTemplate();

            String dir = Program.TemplateDir;
            String fpxml = Path.Combine(dir, "初期値.xml");
            if (File.Exists(fpxml)) {
                try {
                    using (FileStream fs = File.OpenRead(fpxml)) {
                        bindingSource.DataSource = (TextTemplate)new XmlSerializer(typeof(TextTemplate)).Deserialize(fs);
                    }
                }
                catch (Exception err) {
                    Debug.WriteLine("& " + err);
                }
            }
        }

        Size sizeImage = new Size(1, 1);
        Bitmap bitmapSource;

        public void SetPic(Bitmap bitmap) {
            bitmapSource = bitmap;
            sizeImage = bitmapSource.Size;
        }

        private void TextForm_Load(object sender, EventArgs e) {
            foreach (FontFamily font in FontFamily.Families) {
                cbFonts.Items.Add(font.Name);
            }

            tbBody.Focus();
            tbBody.SelectAll();

            penDash = new Pen(Color.Gray);
            penDash.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            panel2_Resize(sender, e);
        }

        Pen penDash;

        private void bSave_Click(object sender, EventArgs e) {
            Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e) {
            Graphics cv = e.Graphics;
            Composite(cv, scale, true, bitmapSource.HorizontalResolution);
        }

        public void Composite(Graphics cv, float scale, bool toControl, float dpi) {
            int fontSize;
            if (!int.TryParse(cbSize.Text, out fontSize)) {
                fontSize = 12;
            }
            using (Font font1 = new Font(cbFonts.Text,
                Math.Max(1, fontSize * scale),
                (bB.Checked ? FontStyle.Bold : 0) | (bI.Checked ? FontStyle.Italic : 0) | (bU.Checked ? FontStyle.Underline : 0)
                ))
            using (Brush brush = new SolidBrush(bForeColor.BackColor)) {
                SizeF sizeText = cv.MeasureString(tbBody.Text, font1);

                Point pointDraw = Pic2View(textPos + textDeltaPos, scale, toControl);

                Rectangle rcFill = new Rectangle(pointDraw, sizeText.ToSize());
                if (cbFill.Checked) {
                    cv.FillRectangle(new SolidBrush(bBackColor.BackColor), rcFill);
                }
                cv.DrawString(tbBody.Text, font1, brush, pointDraw);

                if (toControl) {
                    cv.DrawRectangle(penDash, rcFill);

                    lMetrics.Text = String.Format("{0:0.0}mm x {1:0.0}mm"
                        , sizeText.Width / scale / dpi * 25.4f
                        , sizeText.Height / scale / dpi * 25.4f
                        );
                }
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

        Point Pic2View(Point pt) {
            return new Point(
                (int)(rcPlace.X + pt.X * scale),
                (int)(rcPlace.Y + pt.Y * scale)
                );
        }

        Point textPos {
            get { return ((TextTemplate)bindingSource.Current).textPos; }
            set { ((TextTemplate)bindingSource.Current).textPos = value; }
        }
        Size textDeltaPos = Size.Empty;

        private void timerUpdate_Tick(object sender, EventArgs e) {
            timerUpdate.Stop();
            panel2.Refresh();
        }

        private void tbBody_TextChanged(object sender, EventArgs e) {
            TriggerUpdate();
        }

        private void TriggerUpdate() {
            timerUpdate.Stop();
            timerUpdate.Start();
        }

        private void cbFonts_TextChanged(object sender, EventArgs e) {
            TriggerUpdate();
        }

        private void cbSize_TextChanged(object sender, EventArgs e) {
            TriggerUpdate();
        }

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

        private void bB_CheckedChanged(object sender, EventArgs e) {
            TriggerUpdate();
        }

        private void bForeColor_Click(object sender, EventArgs e) {
            colorDialog.Color = bForeColor.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                bForeColor.BackColor = colorDialog.Color;
                TriggerUpdate();
            }
        }

        Point pointDown, pointMove;
        bool isDown = false;

        private void panel2_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                pointDown = pointMove = e.Location;
                textDeltaPos = Size.Empty;
                isDown = true;
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

        private void bSaveTemplate_Click(object sender, EventArgs e) {
            sfdTempl.InitialDirectory = Program.TemplateDir;
            if (sfdTempl.FileName.Length == 0) {
                sfdTempl.FileName = tbBody.Text.Split('\r', '\n', '\t')[0] + ".xml";
            }
            if (sfdTempl.ShowDialog(this) == DialogResult.OK) {
                try {
                    bindingSource.EndEdit();
                    using (FileStream fs = File.Create(sfdTempl.FileName)) {
                        new XmlSerializer(typeof(TextTemplate)).Serialize(fs, bindingSource.DataSource);
                    }
                }
                catch (Exception err) {
                    MessageBox.Show(this, "書き込みに失敗しました。\n\n" + err);
                }
            }
        }

        private void bReadTemplate_Click(object sender, EventArgs e) {
            ofdTempl.InitialDirectory = Program.TemplateDir;
            if (ofdTempl.ShowDialog(this) == DialogResult.OK) {
                LoadTemplateFrom(ofdTempl.FileName);
                TriggerUpdate();
            }
        }

        public void LoadTemplateFrom(string fp) {
            try {
                bindingSource.CancelEdit();
                using (FileStream fs = File.OpenRead(fp)) {
                    bindingSource.DataSource = (TextTemplate)new XmlSerializer(typeof(TextTemplate)).Deserialize(fs);
                }
                bindingSource.ResetCurrentItem();
                sfdTempl.FileName = fp;
            }
            catch (Exception err) {
                MessageBox.Show(this, "読み込みに失敗しました。\n\n" + err);
            }
        }

        private void bBackColor_Click(object sender, EventArgs e) {
            colorDialog.Color = bBackColor.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
                bBackColor.BackColor = colorDialog.Color;
                cbFill.Checked = true;
                TriggerUpdate();
            }
        }

        private void cbFill_CheckedChanged(object sender, EventArgs e) {
            TriggerUpdate();
        }
    }

    [XmlRoot]
    public class TextTemplate : INotifyPropertyChanged {
        string _fontFamily = "ＭＳ ゴシック";
        [XmlAttribute]
        public string fontFamily {
            get { return _fontFamily; }
            set { _fontFamily = value; Fire("fontFamily"); }
        }

        string _fontSize = "72";
        [XmlAttribute]
        public string fontSize {
            get { return _fontSize; }
            set { _fontSize = value; Fire("fontSize"); }
        }

        bool _bold;
        [XmlAttribute]
        public bool bold {
            get { return _bold; }
            set { _bold = value; Fire("bold"); }
        }

        bool _italic;
        [XmlAttribute]
        public bool italic {
            get { return _italic; }
            set { _italic = value; Fire("italic"); }
        }

        bool _underline;
        [XmlAttribute]
        public bool underline {
            get { return _underline; }
            set { _underline = value; Fire("underline"); }
        }

        string _body = "テキストを入力";
        [XmlElement]
        public string body {
            get { return _body.Replace("\r\n", "\n").Replace("\n", "\r\n"); }
            set { _body = value; Fire("body"); }
        }

        Color _foreColor = Color.Black;
        [XmlIgnore]
        public Color foreColor {
            get { return _foreColor; }
            set { _foreColor = value; Fire("foreColor"); Fire("foreColorRaw"); }
        }

        [XmlAttribute("foreColor")]
        public int foreColorRaw {
            get { return _foreColor.ToArgb(); }
            set { _foreColor = Color.FromArgb(value); Fire("foreColor"); Fire("foreColorRaw"); }
        }

        Color _backColor = Color.White;
        [XmlIgnore]
        public Color backColor {
            get { return _backColor; }
            set { _backColor = value; Fire("backColor"); Fire("backColorRaw"); }
        }

        [XmlAttribute("backColor")]
        public int backColorRaw {
            get { return _backColor.ToArgb(); }
            set { _backColor = Color.FromArgb(value); Fire("backColor"); Fire("backColorRaw"); }
        }

        bool _fill;
        [XmlAttribute]
        public bool fill {
            get { return _fill; }
            set { _fill = value; Fire("fill"); }
        }

        Point _textPos;
        [XmlIgnore]
        public Point textPos {
            get { return _textPos; }
            set { _textPos = value; Fire("textPos"); Fire("textPosRaw"); }
        }

        [XmlAttribute("textPos")]
        public String textPosRaw {
            get { return new PointConverter().ConvertToString(_textPos); }
            set { _textPos = (Point)new PointConverter().ConvertFromString(value); Fire("textPos"); Fire("textPosRaw"); }
        }

        private void Fire(string prop) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        #region INotifyPropertyChanged メンバ

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}