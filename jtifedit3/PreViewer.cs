using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace jtifedit3 {
    public partial class PreViewer : UserControl {
        public PreViewer() {
            InitializeComponent();
        }

        Bitmap pic = null;

        public Bitmap Pic {
            get { return pic; }
            set {
                if (pic != value) {
                    pic = value;

                    Recalc();
                    Invalidate();

                    if (PicChanged != null)
                        PicChanged(this, new EventArgs());
                }
            }
        }

        public event EventHandler PicChanged;

        private void Recalc() {
            if (pic == null) {
                sizeDisp = Size.Empty;
                sizePic = SizeF.Empty;
                return;
            }
            using (Graphics cv = CreateGraphics()) {
                float rx = cv.DpiX;
                float ry = cv.DpiY;
                float px = pic.HorizontalResolution;
                float py = pic.VerticalResolution;
                sizePic = new SizeF(
                    rx / px * pic.Width,
                    ry / py * pic.Height
                    );
                FitCnf fc = fitcnf;
                switch (fc.SizeSpec) {
                    case SizeSpec.ResoRate: {
                            sizeDisp = new SizeF(
                                sizePic.Width * fc.Rate,
                                sizePic.Height * fc.Rate
                            );
                            break;
                        }
                    case SizeSpec.FW: {
                            Size client = ClientSize;
                            if (client.Width < sizePic.Width) {
                                // 画面より大きい→縮小
                                sizeDisp = new SizeF(client.Width, sizePic.Height / sizePic.Width * client.Width);
                            }
                            else {
                                // 同等・小さい→そのまま
                                sizeDisp = sizePic;
                            }
                            break;
                        }
                    case SizeSpec.FWH:
                        sizeDisp = FitRect3.Fit(ClientRectangle, pic.Size).Size;
                        break;
                }

                AutoScrollMinSize = Size.Round(sizeDisp);
            }
        }

        SizeF sizePic = SizeF.Empty;

        public float ActualRate {
            get {
                if (sizePic.IsEmpty)
                    return 1;
                return sizeDisp.Width / sizePic.Width;
            }
        }

        SizeF sizeDisp = SizeF.Empty;

        private void PreViewer_Load(object sender, EventArgs e) {
            DoubleBuffered = true;
        }

        private void PreViewer_Paint(object sender, PaintEventArgs e) {
            Graphics cv = e.Graphics;
            if (DesignMode) {
                Rectangle rc = ClientRectangle;
                rc.Inflate(-1, -1);
                using (Pen pen = new Pen(this.ForeColor)) {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    cv.DrawRectangle(pen, rc);
                }
                rc.Inflate(-1, -1);
                using (HatchBrush br = new HatchBrush(HatchStyle.Wave, this.ForeColor, Color.Transparent)) {
                    cv.FillRectangle(br, rc);
                }

                return;
            }

            if (pic != null) {
                cv.InterpolationMode = InterpolationMode.High;

                cv.DrawImage(pic, RUt.Centerize(this.ClientSize, new Rectangle(AutoScrollPosition, Size.Round(sizeDisp))), new Rectangle(Point.Empty, pic.Size), GraphicsUnit.Pixel);

                cv.InterpolationMode = InterpolationMode.Default;
            }
        }

        class RUt {
            internal static Rectangle Centerize(Size clientSize, Rectangle rcDest) {
                if (rcDest.Width < clientSize.Width) {
                    rcDest.Offset((clientSize.Width - rcDest.Width) / 2, 0);
                }
                if (rcDest.Height < clientSize.Height) {
                    rcDest.Offset(0, (clientSize.Height - rcDest.Height) / 2);
                }
                return rcDest;
            }
        }

        FitCnf fitcnf = new FitCnf(SizeSpec.FWH);

        public FitCnf FitCnf {
            get { return fitcnf; }
            set {
                if (fitcnf != value) {
                    fitcnf = value;

                    Recalc();
                    Invalidate();

                    if (FitCnfChanged != null)
                        FitCnfChanged(this, new EventArgs());
                }
            }
        }

        public event EventHandler FitCnfChanged;

        private void PreViewer_Resize(object sender, EventArgs e) {
            Recalc();
            Invalidate();
        }

        Point pt0;
        Point pt1;
        bool bandVisible = false;
        int bandGripping = -1;
        Point mpt0, mpt1;

        private void PreViewer_MouseDown(object sender, MouseEventArgs e) {
            if (useBand_) {
                if (e.Button == MouseButtons.Left) {
                    Point npt0 = new Point(Math.Min(pt0.X, pt1.X), Math.Min(pt0.Y, pt1.Y));
                    Point npt1 = new Point(Math.Max(pt0.X, pt1.X), Math.Max(pt0.Y, pt1.Y));
                    pt0 = npt0;
                    pt1 = npt1;

                    bandGripping = -1;
                    if (bandVisible) {
                        bool outLeft = pt0.X > e.X;
                        bool outTop = pt0.Y > e.Y;
                        bool outRight = pt1.X < e.X;
                        bool outBottom = pt1.Y < e.Y;
                        if (outLeft) {
                            if (outTop) {
                                bandGripping = 0;
                            }
                            else if (outBottom) {
                                bandGripping = 8;
                            }
                            else {
                                bandGripping = 4;
                            }
                        }
                        else if (outRight) {
                            if (outTop) {
                                bandGripping = 2;
                            }
                            else if (outBottom) {
                                bandGripping = 10;
                            }
                            else {
                                bandGripping = 6;
                            }
                        }
                        else if (outTop) {
                            bandGripping = 1;
                        }
                        else if (outBottom) {
                            bandGripping = 9;
                        }
                        else {
                            bandGripping = 5;
                        }
                    }

                    FlushBand();
                    if (-1 == bandGripping) {
                        pt0 = pt1 = e.Location;
                    }
                    mpt0 = mpt1 = e.Location;
                }
            }
        }

        private void PreViewer_MouseMove(object sender, MouseEventArgs e) {
            if (useBand_) {
                if (e.Button == MouseButtons.Left) {
                    FlushBand();
                    if (bandGripping == -1) {
                        pt1 = e.Location;
                    }
                    else {
                        UpdateBandLocation(mpt1, e.Location);
                    }
                    mpt1 = e.Location;
                    bandVisible = true;

                    using (Graphics cv = CreateGraphics()) {
                        Pen pen = new Pen(Color.LimeGreen);
                        pen.DashStyle = DashStyle.Dash;
                        cv.DrawRectangle(pen, RectUtil.FromEx(pt0, pt1, 0, 0));
                        bands = RectUtil.GetBandRects(RectUtil.From(pt0, pt1), bandWidth);
                        cv.FillRectangles(Brushes.LimeGreen, bands);
                        cv.DrawRectangles(Pens.Black, bands);
                    }
                }
            }
        }

        const int bandWidth = 3;

        Rectangle[] bands;

        private void UpdateBandLocation(Point pta, Point ptb) {
            int dx = ptb.X - pta.X;
            int dy = ptb.Y - pta.Y;
            switch (bandGripping) {
                case 0:
                    pt0.X += dx;
                    pt0.Y += dy;
                    break;
                case 1:
                    pt0.Y += dy;
                    break;
                case 2:
                    pt1.X += dx;
                    pt0.Y += dy;
                    break;

                case 4:
                    pt0.X += dx;
                    break;
                case 5:
                    pt0.X += dx;
                    pt0.Y += dy;
                    pt1.X += dx;
                    pt1.Y += dy;
                    break;
                case 6:
                    pt1.X += dx;
                    break;

                case 8:
                    pt0.X += dx;
                    pt1.Y += dy;
                    break;
                case 9:
                    pt1.Y += dy;
                    break;
                case 10:
                    pt1.X += dx;
                    pt1.Y += dy;
                    break;
            }
        }

        private void FlushBand() {
            if (bandVisible) {
                Invalidate(RectUtil.FromEx(pt0, pt1, bandWidth, bandWidth + 1));
                bandVisible = false;
                Update();
            }
        }

        class RectUtil {
            internal static Rectangle From(Point pt0, Point pt1) {
                return Rectangle.FromLTRB(
                    Math.Min(pt0.X, pt1.X), Math.Min(pt0.Y, pt1.Y),
                    Math.Max(pt0.X, pt1.X), Math.Max(pt0.Y, pt1.Y)
                    );
            }

            internal static Rectangle FromEx(Point pt0, Point pt1, int marginTL, int marginRB) {
                return Rectangle.FromLTRB(
                    Math.Min(pt0.X, pt1.X) - marginTL, Math.Min(pt0.Y, pt1.Y) - marginTL,
                    Math.Max(pt0.X, pt1.X) + marginRB, Math.Max(pt0.Y, pt1.Y) + marginRB
                    );
            }

            internal static Rectangle[] GetBandRects(Rectangle rectBase, int margin) {
                Rectangle[] rects = new Rectangle[12];
                int x0 = rectBase.X;
                int x2 = rectBase.Right;
                int x1 = (x0 + x2) / 2;
                int y0 = rectBase.Y;
                int y2 = rectBase.Bottom;
                int y1 = (y0 + y2) / 2;
                int step = margin * 10;

                // A B C -
                // D   E -
                // F G H -

                rects[0] = new Rectangle(x0 - margin, y0 - margin, margin * 2, margin * 2);
                if (x2 - x0 > step) {
                    rects[1] = new Rectangle(x1 - margin, y0 - margin, margin * 2, margin * 2);
                }
                rects[2] = new Rectangle(x2 - margin, y0 - margin, margin * 2, margin * 2);
                //
                if (y2 - y0 > step) {
                    rects[4] = new Rectangle(x0 - margin, y1 - margin, margin * 2, margin * 2);

                    rects[6] = new Rectangle(x2 - margin, y1 - margin, margin * 2, margin * 2);
                }
                //
                rects[8] = new Rectangle(x0 - margin, y2 - margin, margin * 2, margin * 2);
                if (x2 - x0 > step) {
                    rects[9] = new Rectangle(x1 - margin, y2 - margin, margin * 2, margin * 2);
                }
                rects[10] = new Rectangle(x2 - margin, y2 - margin, margin * 2, margin * 2);

                return rects;
            }
        }

        private void PreViewer_MouseUp(object sender, MouseEventArgs e) {
            if (useBand_) {
                if (e.Button == MouseButtons.Left) {
                    if (bandVisible) {
                        if (BandChanged != null) {
                            BandChanged(this, e);
                        }
                    }
                }
            }
        }

        public event EventHandler BandChanged;

        public Rectangle Band {
            get {
                return RectUtil.From(pt0, pt1);
            }
            set {
                pt0 = new Point(value.X, value.Y);
                pt1 = new Point(value.Right, value.Bottom);
                Invalidate();
            }
        }

        bool useBand_ = false;
        [DefaultValue(false)]
        public bool UseBand {
            get { return useBand_; }
            set { useBand_ = value; }
        }
    }

    public enum SizeSpec {
        ResoRate, // 解像度に対する倍率(1.0f -> 100%)
        FW, // 幅フィット
        FWH, // 頁フィット
    }

    public struct FitCnf : IComparable<FitCnf> {
        float rate;
        SizeSpec sizeSpec;

        public FitCnf(float rate) {
            this.rate = rate;
            this.sizeSpec = SizeSpec.ResoRate;
        }

        public FitCnf(SizeSpec spec) {
            this.rate = 1;
            this.sizeSpec = spec;
        }

        public float Rate { get { return rate; } }
        public SizeSpec SizeSpec { get { return sizeSpec; } }

        public override bool Equals(object obj) { return (obj is FitCnf) ? CompareTo((FitCnf)obj) == 0 : base.Equals(obj); }
        public override int GetHashCode() { return Convert.ToInt32(sizeSpec) ^ BitConverter.ToInt32(BitConverter.GetBytes(rate), 0); }

        public static bool operator ==(FitCnf x, FitCnf y) { return x.CompareTo(y) == 0; }
        public static bool operator !=(FitCnf x, FitCnf y) { return x.CompareTo(y) != 0; }

        #region IComparable<FitCnf> メンバ

        public int CompareTo(FitCnf other) {
            int t;
            t = sizeSpec.CompareTo(other.sizeSpec); if (t != 0) return t;
            t = rate.CompareTo(other.rate); if (t != 0) return t;
            return t;
        }

        #endregion
    }
}
