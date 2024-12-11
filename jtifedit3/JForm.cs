using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using FreeImageAPI;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Drawing.Printing;
using jtifedit3.Properties;
using jtifedit3.Utils;
using TwainDotNet;

namespace jtifedit3 {
    public partial class JForm : Form {
        String fp = null;
        String fpCurrent = null;

        public JForm(String fp) {
            InitializeComponent();

            this.fp = fp;

            if (!Settings.Default.RestoreBounds.IsEmpty) {
                WindowState = FormWindowState.Normal;
                Bounds = Settings.Default.RestoreBounds;
                WindowState = FormWindowState.Maximized;
            }
        }

        bool isModified = false;

        private void bNew_Click(object sender, EventArgs e) {
            Newf();
        }

        enum TState {
            Yes, No, Cancel,
        }

        TState TrySave() {
            if (!Modified) return TState.Yes;

            switch (MessageBox.Show(this, "変更されています。保存しますか。", Path.GetFileName(Currentfp), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)) {
                case DialogResult.Yes:
                    if (Savef(Currentfp))
                        return TState.Yes;
                    return TState.Cancel;
                case DialogResult.No:
                    return TState.No;
                default:
                    return TState.Cancel;
            }
        }

        TState TrySave2() {
            if (!Modified) return TState.Yes;

            switch (MessageBox.Show(this, "先に保存しますか。", Path.GetFileName(Currentfp), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                case DialogResult.Yes:
                    if (Savef(Currentfp))
                        return TState.Yes;
                    return TState.Cancel;
                case DialogResult.No:
                default:
                    return TState.No;
            }
        }

        private void Newf() {
            switch (TrySave()) {
                case TState.Yes:
                case TState.No:
                    tv.Picts.Clear();
                    Currentfp = null;
                    Modified = false;
                    ExifCut = false;
                    ReadOnly = false;
                    tlpDPIWarn.Visible = false;
                    break;
            }
        }

        String TTLTemp = String.Empty;

        bool exifCut = false;

        bool ExifCut {
            get {
                return exifCut;
            }
            set {
                tlpExifCut.Visible = exifCut = value;
            }
        }

        bool isReadOnly = false;

        bool ReadOnly {
            get {
                return isReadOnly;
            }
            set {
                isReadOnly = value;
                UpdateTTL();
            }
        }

        bool Modified {
            get {
                return isModified;
            }
            set {
                isModified = value;
                UpdateTTL();
            }
        }

        String Currentfp {
            get {
                return fpCurrent;
            }
            set {
                fpCurrent = value;
                fpatt = (!String.IsNullOrEmpty(value) && File.Exists(value)) ? File.GetAttributes(value) : FileAttributes.Normal;
                UpdateTTL();
            }
        }

        FileAttributes fpatt = FileAttributes.Normal;

        private void UpdateTTL() {
            this.Text = (fpCurrent != null)
                ? Path.GetFileName(fpCurrent) + (Modified ? "*" : "") + " -- " + TTLTemp
                : TTLTemp + (Modified ? " *" : "")
                ;
            if (isReadOnly) this.Text += " [読取専用]";
        }

        String PrintTitle {
            get {
                if (fpCurrent != null)
                    return Path.GetFileNameWithoutExtension(fpCurrent);
                return Path.GetFileNameWithoutExtension(TTLTemp);
            }
        }

        private void JForm_Load(object sender, EventArgs e) {
            {
                Control[] ary = new Control[] { tstop, tsvis };
                Control.ControlCollection col = tsc.TopToolStripPanel.Controls;
                col.Clear();
                col.AddRange(ary);
            }

            tv.Picts = new BindingList<TvPict>();
            tv.Picts.ListChanged += new ListChangedEventHandler(Picts_ListChanged);

            TTLTemp = this.Text;

            if (fp != null) Openf(fp);

            tscRate.SelectedIndex = 0;
        }

        bool isX64 = IntPtr.Size == 8;

        void Picts_ListChanged(object sender, ListChangedEventArgs e) {
            Modified = true;
        }

        private void bOpenf_Click(object sender, EventArgs e) {
            switch (TrySave()) {
                case TState.Yes:
                case TState.No:
                    if (ofdPict.ShowDialog(this) != DialogResult.OK) return;

                    Openf(ofdPict.FileName);
                    break;
            }
        }

        class TFUt {
            internal static String FilterUnsafeTIFFTags(String fp, Tempfp tempfp) {
                TCUt tcut = new TCUt(fp);
                if (tcut.Test() > 0) {
                    String fpOpen = tempfp.GetTempFileName();
                    File.Copy(fp, fpOpen, true);

                    using (FileStream fs = File.Open(fpOpen, FileMode.Open, FileAccess.ReadWrite, FileShare.Read)) {
                        new TC.TIFCut().Cut(fs);
                    }
                    fp = fpOpen;
                }
                return fp;
            }
        }

        void Openf(String fp) {
            String fpOpen = fp;

            TCUt tcut = new TCUt(fp);
            if (tcut.Test() > 0) {
                ExifCut = true;

                lExifCutWhat.Text = tcut.DisposalTags;

                fpOpen = tempfp.GetTempFileName();
                File.Copy(fp, fpOpen, true);
                File.SetAttributes(fpOpen, File.GetAttributes(fpOpen) & ~FileAttributes.ReadOnly);

                using (FileStream fs = File.Open(fpOpen, FileMode.Open, FileAccess.ReadWrite, FileShare.Read)) {
                    new TC.TIFCut().Cut(fs);
                }
            }
            else {
                ExifCut = false;
            }

            ReadOnly = 0 != (File.GetAttributes(fp) & FileAttributes.ReadOnly);

            FREE_IMAGE_FORMAT fmt = FREE_IMAGE_FORMAT.FIF_TIFF;
            FIMULTIBITMAP tif = FreeImage.OpenMultiBitmapEx(fpOpen, ref fmt, FREE_IMAGE_LOAD_FLAGS.DEFAULT, false, true, false);
            try {
                tv.Picts.Clear();
                Currentfp = null;
                int cnt = FreeImage.GetPageCount(tif);
                for (int i = 0; i < cnt; i++) {
                    FIBITMAP fib = FreeImage.LockPage(tif, i);
                    try {
                        tv.Picts.Add(new TvPict(FreeImage.Clone(fib)));
                    }
                    finally {
                        FreeImage.UnlockPage(tif, fib, false);
                    }
                }
                CheckXYRes();
                Currentfp = fp;
                Modified = false;
            }
            finally {
                FreeImage.CloseMultiBitmapEx(ref tif);
            }
        }

        private void CheckXYRes() {
            tlpDPIWarn.Visible = TestXYRes();
        }

        bool TestXYRes() {
            bool any = false;
            foreach (TvPict t in tv.Picts) {
                FIBITMAP fib = t.Picture;
                uint h = FreeImage.GetResolutionX(fib);
                uint v = FreeImage.GetResolutionY(fib);
                any = h != 0 && v != 0 && h != v;
            }
            return any;
        }

        private void tv_SelChanged(object sender, EventArgs e) {
            int c = tv.SelCount;
            if (c == 0) {
                tssl.Text = "Ready";
            }
            else if (c == 1) {
                tssl.Text = String.Format("ページ{0:#,##0}を選択。", 1 + tv.SelFirst);
            }
            else {
                tssl.Text = String.Format("ページ{0:#,##0}～{1:#,##0}を選択。(ページ数{2:#,##0})", 1 + tv.SelFirst, 1 + tv.SelLast, c);
            }

            String mets = "";
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast; x++) {
                FIBITMAP dib = tv.Picts[x].Picture;
                if (mets.Length != 0)
                    mets += ", ";
                mets += UtPS.GuessPageSize(dib, true) ?? "?";
            }
            if (mets.Length != 0) tssl.Text += " [" + mets + "]";

            if (c == 0) {
                pvw.Pic = null;
            }
            else {
                pvw.Pic = tv.Picts[tv.Sel2].DefTumbGen.GetThumbnail(Size.Empty);
            }
        }

        class UtPS {
            class P1 {
                public int cx, cy;
                public String a;

                public P1(String a, int cx, int cy) {
                    this.a = a;
                    this.cx = cx;
                    this.cy = cy;
                }
            }

            static P1[] sizes = new P1[] {
                new P1("4A0",1682,2378),
                new P1("2A0",1189,1682),
                new P1("A0", 841,1189),
                new P1("A1", 594, 841),
                new P1("A2", 420, 594),
                new P1("A3", 297, 420),
                new P1("A4", 210, 297),
                new P1("A5", 148, 210),
                new P1("A6", 105, 148),

                new P1("JIS B3", 364, 515),
                new P1("JIS B4", 257, 364),
                new P1("JIS B5", 182, 257),

                new P1("ISO B3", 353, 500),
                new P1("ISO B4", 250, 353),
                new P1("ISO B5", 176, 250),

                new P1("C3", 324, 458),
                new P1("C4", 229, 324),
                new P1("C5", 162, 229),

                new P1("はがき",  99, 148),

                new P1("L",  89, 127),
                new P1("2L",  127, 178),

                new P1("Letter", 216, 279),
                new P1("Folio", 210, 330),
            };

            public static String GuessPageSize(FIBITMAP dib, bool usemm) {
                uint dpix = FreeImage.GetResolutionX(dib);
                uint dpiy = FreeImage.GetResolutionY(dib);
                uint cx = FreeImage.GetWidth(dib);
                uint cy = FreeImage.GetHeight(dib);

                if (cx > 0 && cy > 0 && dpix > 0 && dpiy > 0) {
                    float lx = cx / (float)dpix * 25.4f;
                    float ly = cy / (float)dpiy * 25.4f;

                    foreach (P1 p1 in sizes) {
                        if (Isit(lx, ly, p1.cx, p1.cy)) return p1.a + "縦";
                        if (Isit(lx, ly, p1.cy, p1.cx)) return p1.a + "横";
                    }

                    if (usemm) return String.Format("{0:0} x {1:0}", lx, ly);
                }

                return null;
            }

            static bool Isit(float lx, float ly, float tx, float ty) {
                float f = 2;
                return Math.Abs(lx - tx) < f && Math.Abs(ly - ty) < f;
            }
        }

        private void tscRate_SelectedIndexChanged(object sender, EventArgs e) {
            switch (tscRate.SelectedIndex) {
                case 0: //"頁全体"
                    pvw.FitCnf = new FitCnf(SizeSpec.FWH);
                    break;
                case 1: //"頁幅"
                    pvw.FitCnf = new FitCnf(SizeSpec.FW);
                    break;
                default:
                    SetRate(tscRate.Text);
                    break;
            }
        }

        private void SetRate(String text) {
            Match M = Regex.Match(text, "^(\\d+)");
            int percent;
            if (M.Success && int.TryParse(M.Groups[1].Value, out percent) && 1 <= percent && percent <= 1600) {
                pvw.FitCnf = new FitCnf(percent / 100.0f);
            }
            else {
                MessageBox.Show(this, "1 から 1600 までの数値を入れてください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void tscRate_Validating(object sender, CancelEventArgs e) {
            if (tscRate.SelectedIndex < 0) {
                SetRate(tscRate.Text);
            }
        }

        private void tscRate_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                SetRate(tscRate.Text);
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void bzoomIn_Click(object sender, EventArgs e) {
            if (pvw.Pic != null)
                pvw.FitCnf = new FitCnf(pvw.ActualRate * 1.3f);
        }

        private void bzoomOut_Click(object sender, EventArgs e) {
            if (pvw.Pic != null)
                pvw.FitCnf = new FitCnf(pvw.ActualRate / 1.3f);
        }

        private void preViewer1_FitCnfChanged(object sender, EventArgs e) {
            FitCnf fc = pvw.FitCnf;
            switch (fc.SizeSpec) {
                case SizeSpec.FWH: {
                        int i = 0;
                        if (tscRate.SelectedIndex != i)
                            tscRate.SelectedIndex = i;
                        break;
                    }
                case SizeSpec.FW: {
                        int i = 1;
                        if (tscRate.SelectedIndex != i)
                            tscRate.SelectedIndex = i;
                        break;
                    }
                case SizeSpec.ResoRate: {
                        tscRate.Text = String.Format("{0:0} %", fc.Rate * 100);
                        break;
                    }
            }
        }

        private void bShowPreView_Click(object sender, EventArgs e) {
            vsc.Panel2Collapsed = !bShowPreView.Checked;
        }

        private void tv_PictDrag(object sender, EventArgs e) {
            DataObject dat = new DataObject();
            dat.SetData("PId", Process.GetCurrentProcess().Id);
            dat.SetData("SelFirst", tv.SelFirst);
            dat.SetData("SelLast", tv.SelLast);

            for (int x = tv.SelFirst; x <= tv.SelLast; x++) {
                using (MemoryStream os = new MemoryStream()) {
                    FreeImage.SaveToStream(tv.Picts[x].Picture, os, FREE_IMAGE_FORMAT.FIF_PNG);
                    dat.SetData("#" + x, os.ToArray());
                }
            }

            tv.DoDragDrop(dat, DragDropEffects.Scroll | DragDropEffects.Copy | DragDropEffects.Move);

            if (dat.GetDataPresent("Pasted") && (int)dat.GetData("Pasted") == 1) {
                int iSelLast = tv.SelLast;
                int iSelFirst = tv.SelFirst;

                for (int x = iSelLast; x >= iSelFirst; x--) {
                    tv.Picts.RemoveAt(x);
                }
            }
        }

        private void tv_QueryContinueDrag(object sender, QueryContinueDragEventArgs e) {
            if (e.EscapePressed)
                e.Action = DragAction.Cancel;
        }

        private void tv_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent("PId") && e.Data.GetDataPresent("SelFirst") && e.Data.GetDataPresent("SelLast")) {
                e.Effect = e.AllowedEffect & ((0 != (e.KeyState & 8)) ? DragDropEffects.Copy | DragDropEffects.Move : DragDropEffects.Move);
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = e.AllowedEffect & DragDropEffects.Copy;
            }
            else {
                e.Effect = DragDropEffects.None;
            }
        }

        private void tv_DragOver(object sender, DragEventArgs e) {
            tv_DragEnter(sender, e);

            int W = 20, M = 20;

            Point pos = Point.Empty - new Size(tv.AutoScrollPosition);

            Point pt = tv.PointToClient(new Point(e.X, e.Y));

            tv.InsertMark = tv.SuggestInsertMark(pt);

            if (pt.X < W) {
                pos.X -= M;
            }
            else if (tv.Width - W <= pt.X) {
                pos.X += M;
            }
            if (pt.Y < W) {
                pos.Y -= M;
            }
            else if (tv.Height - W <= pt.Y) {
                pos.Y += M;
            }

            tv.AutoScrollPosition = (pos);
        }

        Tempfp tempfp = new Tempfp();

        class Tempfp {
            List<string> fps = new List<string>();

            public String GetTempFileName() {
                String s = Path.GetTempFileName();
                lock (this) fps.Add(s);
                return s;
            }

            public void Cleanup() {
                lock (this) {
                    foreach (String fp in fps)
                        try {
                            File.Delete(fp);
                        }
                        catch (Exception) {

                        }
                    fps.Clear();
                }
            }
        }

        private void tv_DragDrop(object sender, DragEventArgs e) {
            TvInsertMark iMark = tv.InsertMark;
            bool forceAppend = (sender == bAppend);
            try {
                int iAt = forceAppend
                    ? tv.Picts.Count
                    : (iMark.Item + ((0 != (iMark.Location & (TvInsertMarkLocation.Right | TvInsertMarkLocation.Bottom))) ? 1 : 0))
                    ;

                bool isCopy = 0 != (e.Effect & DragDropEffects.Copy);

                int cntAdded = 0;

                String[] alfp = e.Data.GetData(DataFormats.FileDrop) as String[];
                if (alfp != null) {
                    DialogResult res = DialogResult.Yes; // insert there
                    if (tv.Picts.Count == 0)
                        if (forceAppend)
                            res = DialogResult.Yes; // insert (append) there
                        else
                            using (OpenWayForm form = new OpenWayForm(false))
                                res = form.ShowDialog();
                    if (res == DialogResult.OK) {
                        QueueOpenit(alfp);
                    }
                    else if (res != DialogResult.Cancel) {
                        bool fAppend = res == DialogResult.Retry;
                        bool fInsertAfter = res == DialogResult.Yes;

                        if (!fInsertAfter) {
                            iAt = tv.Picts.Count;
                        }

                        foreach (String fp in alfp) {
                            String fpOpen = TFUt.FilterUnsafeTIFFTags(fp, tempfp);
                            FREE_IMAGE_FORMAT fmt = FREE_IMAGE_FORMAT.FIF_UNKNOWN;
                            fmt = FreeImage.GetFileType(fpOpen, 0);
                            switch (fmt) {
                                case FREE_IMAGE_FORMAT.FIF_TIFF:
                                case FREE_IMAGE_FORMAT.FIF_MNG:
                                case FREE_IMAGE_FORMAT.FIF_GIF: {
                                        FIMULTIBITMAP tif = FreeImage.OpenMultiBitmapEx(fpOpen, ref fmt, FREE_IMAGE_LOAD_FLAGS.DEFAULT, false, true, false);
                                        try {
                                            int cnt = FreeImage.GetPageCount(tif);
                                            for (int i = 0; i < cnt; i++) {
                                                FIBITMAP fib = FreeImage.LockPage(tif, i);
                                                if (!fib.IsNull) {
                                                    try {
                                                        tv.Picts.Insert(iAt, new TvPict(FreeImage.Clone(fib)));
                                                        iAt++;
                                                        cntAdded++;
                                                    }
                                                    finally {
                                                        FreeImage.UnlockPage(tif, fib, false);
                                                    }
                                                }
                                            }
                                        }
                                        finally {
                                            FreeImage.CloseMultiBitmapEx(ref tif);
                                        }
                                        break;
                                    }
                                default: {
                                        FIBITMAP tif = FreeImage.Load(fmt, fpOpen, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
                                        try {
                                            if (!tif.IsNull) {
                                                tv.Picts.Insert(iAt, new TvPict(FreeImage.Clone(tif)));
                                                iAt++;
                                                cntAdded++;
                                            }
                                        }
                                        finally {
                                            FreeImage.UnloadEx(ref tif);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                    if (cntAdded != 0) {
                        CheckXYRes();
                    }
                    return;
                }
                else {
                    int iPId = (int)e.Data.GetData("PId");
                    int iSelFirst = (int)e.Data.GetData("SelFirst");
                    int iSelLast = (int)e.Data.GetData("SelLast");

                    int cnt = iSelLast - iSelFirst + 1;

                    if (iPId == Process.GetCurrentProcess().Id) {
                        List<TvPict> al = new List<TvPict>();
                        for (int x = iSelFirst; x <= iSelLast; x++) al.Add(tv.Picts[x]);

                        if (isCopy) {
                            for (int x = 0; x < al.Count; x++) {
                                tv.Picts.Insert(iAt + x, al[x].Clone());
                                cntAdded++;
                            }
                        }
                        else {
                            if (iSelFirst <= iAt && iAt <= iSelLast + 1) {

                            }
                            else {
                                for (int x = iSelLast; x >= iSelFirst; x--) {
                                    tv.Picts.RemoveAt(x);
                                }
                                if (iAt > iSelFirst)
                                    iAt -= cnt;
                                for (int x = 0; x < al.Count; x++) {
                                    tv.Picts.Insert(iAt + x, al[x]);
                                    cntAdded++;
                                }
                            }
                        }
                    }
                    else {
                        for (int x = 0; x < cnt; x++) {
                            tv.Picts.Insert(iAt + x, new TvPict(CPUt.GetPic(e.Data, iSelFirst + x)));
                            cntAdded++;
                        }
                        if (!isCopy) {
                            try {
                                e.Data.SetData("Pasted", (int)1);
                            }
                            catch (Exception) { }
                        }
                    }
                }

                if (cntAdded != 0) {
                    CheckXYRes();
                }
                if (forceAppend && cntAdded != 0) {
                    MessageBox.Show(this, cntAdded + "ページ、最後に" + (isCopy ? "追加" : "移動") + "しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            finally {
                tv.InsertMark = new TvInsertMark();
            }
        }

        class CPUt {
            internal static FIBITMAP GetPic(IDataObject dat, int iSel) {
                byte[] bin = dat.GetData("#" + iSel) as byte[];
                if (bin == null) throw new NotSupportedException();

                FREE_IMAGE_FORMAT fif = FREE_IMAGE_FORMAT.FIF_PNG;
                return FreeImage.LoadFromStream(new MemoryStream(bin, false), ref fif);
            }
        }

        private void tv_DragLeave(object sender, EventArgs e) {
            tv.InsertMark = new TvInsertMark();
        }

        private void tv_MouseMove(object sender, MouseEventArgs e) {
        }

        private void bDelp_Click(object sender, EventArgs e) {
            if (tv.SelCount == 0) return;
            if (MessageBox.Show(this, "選択した頁を削除します。", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;

            int cx = tv.SelCount;
            for (int x = tv.SelLast; cx > 0; x--, cx--) {
                tv.Picts.RemoveAt(x);
            }
        }

        private void bRotLeft_Click(object sender, EventArgs e) {
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast; x++) {
                tv.Picts[x].Rotate(sender == bRotRight);
                tv.Picts.ResetItem(x);
            }
        }

        class TCUt {
            String fp;

            public TCUt(String fp) {
                this.fp = fp;
            }

            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();

            public int Test() {
                if (!File.Exists(fp))
                    return -1;
                try {
                    dict.Clear();
                    using (FileStream fs = File.OpenRead(fp)) {
                        jtifedit2.TIF.Reader reader = new jtifedit2.TIF.Reader(fs);
                        jtifedit2.TIF.Entry[] al;
                        while (null != (al = reader.Read())) {
                            foreach (jtifedit2.TIF.Entry e in al) {
                                // from http://www.awaresystems.be/imaging/tiff/tifftags/private.html
                                switch (e.tag) {
#if false
                                    case 0x8649: dict["Adobe Photoshop"] = null; break;
                                    case 0x935C: dict["Adobe Photoshop"] = null; break;
                                    case 0xC612: dict["DNG"] = null; break;
                                    case 0xC613: dict["DNG"] = null; break;
                                    case 0xC614: dict["DNG"] = null; break;
                                    case 0xC615: dict["DNG"] = null; break;
                                    case 0xC616: dict["DNG"] = null; break;
                                    case 0xC617: dict["DNG"] = null; break;
                                    case 0xC618: dict["DNG"] = null; break;
                                    case 0xC619: dict["DNG"] = null; break;
                                    case 0xC61A: dict["DNG"] = null; break;
                                    case 0xC61B: dict["DNG"] = null; break;
                                    case 0xC61C: dict["DNG"] = null; break;
                                    case 0xC61D: dict["DNG"] = null; break;
                                    case 0xC61E: dict["DNG"] = null; break;
                                    case 0xC61F: dict["DNG"] = null; break;
                                    case 0xC620: dict["DNG"] = null; break;
                                    case 0xC621: dict["DNG"] = null; break;
                                    case 0xC622: dict["DNG"] = null; break;
                                    case 0xC623: dict["DNG"] = null; break;
                                    case 0xC624: dict["DNG"] = null; break;
                                    case 0xC625: dict["DNG"] = null; break;
                                    case 0xC626: dict["DNG"] = null; break;
                                    case 0xC627: dict["DNG"] = null; break;
                                    case 0xC628: dict["DNG"] = null; break;
                                    case 0xC629: dict["DNG"] = null; break;
                                    case 0xC62A: dict["DNG"] = null; break;
                                    case 0xC62B: dict["DNG"] = null; break;
                                    case 0xC62C: dict["DNG"] = null; break;
                                    case 0xC62D: dict["DNG"] = null; break;
                                    case 0xC62E: dict["DNG"] = null; break;
                                    case 0xC62F: dict["DNG"] = null; break;
                                    case 0xC630: dict["DNG"] = null; break;
                                    case 0xC631: dict["DNG"] = null; break;
                                    case 0xC632: dict["DNG"] = null; break;
                                    case 0xC634: dict["DNG"] = null; break;
                                    case 0xC635: dict["DNG"] = null; break;
                                    case 0xC65A: dict["DNG"] = null; break;
                                    case 0xC65B: dict["DNG"] = null; break;
                                    case 0xC65C: dict["DNG"] = null; break;
                                    case 0xA480: dict["GDAL"] = null; break;
                                    case 0xA481: dict["GDAL"] = null; break;
                                    case 0x830E: dict["GeoTIFF"] = null; break;
                                    case 0x8480: dict["GeoTIFF"] = null; break;
                                    case 0x8482: dict["GeoTIFF"] = null; break;
                                    case 0x85D8: dict["GeoTIFF"] = null; break;
                                    case 0x87AF: dict["GeoTIFF"] = null; break;
                                    case 0x87B0: dict["GeoTIFF"] = null; break;
                                    case 0x87B1: dict["GeoTIFF"] = null; break;
                                    case 0x885C: dict["HylaFAX"] = null; break;
                                    case 0x885D: dict["HylaFAX"] = null; break;
                                    case 0x885E: dict["HylaFAX"] = null; break;
                                    case 0x80A4: dict["Imaging"] = null; break;
                                    case 0x847E: dict["Intergraph Application"] = null; break;
                                    case 0x847F: dict["Intergraph Application"] = null; break;
                                    case 0x83BB: dict["IPTC"] = null; break;
                                    case 0x82A5: dict["Molecular Dynamics GEL"] = null; break;
                                    case 0x82A6: dict["Molecular Dynamics GEL"] = null; break;
                                    case 0x82A7: dict["Molecular Dynamics GEL"] = null; break;
                                    case 0x82A8: dict["Molecular Dynamics GEL"] = null; break;
                                    case 0x82A9: dict["Molecular Dynamics GEL"] = null; break;
                                    case 0x82AA: dict["Molecular Dynamics GEL"] = null; break;
                                    case 0x82AB: dict["Molecular Dynamics GEL"] = null; break;
                                    case 0x82AC: dict["Molecular Dynamics GEL"] = null; break;
                                    case 0xC427: dict["Oce scanning"] = null; break;
                                    case 0xC428: dict["Oce scanning"] = null; break;
                                    case 0xC429: dict["Oce scanning"] = null; break;
                                    case 0xC42A: dict["Oce scanning"] = null; break;
                                    case 0xC660: dict["Sketchbook Pro"] = null; break;
#endif
                                    case 0x8769: dict["Exif IFD"] = null; break;
                                    case 0x8825: dict["GPS IFD"] = null; break;
                                    case 0x8773: dict["ICC Profile"] = null; break;
                                    case 0xA005: dict["Interoperability IFD"] = null; break;
                                }
                            }
                        }
                        return dict.Count;
                    }
                }
                catch (Exception) {
                    return -1;
                }
            }

            public string DisposalTags {
                get {
                    return string.Join(", ", new List<string>(dict.Keys).ToArray());
                }
            }

            public bool ConfirmTagsDisposal(IWin32Window parent, bool save) {
                String s = "";
                foreach (String kw in dict.Keys) s += "- " + kw + "\n";

                if (dict.Count != 0) {
                    if (MessageBox.Show(parent, "安全の為、次の情報は削除してから編集します。\n\n" + s + "\n続行しますか?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                        return false;
                }
                return true;
            }
        }

        private void bSave_Click(object sender, EventArgs e) {
            Savef(Currentfp);
        }

        private bool Savef(String fp) {
            if (fp == null || ReadOnly || (0 != (File.GetAttributes(fp) & FileAttributes.ReadOnly))) {
                sfdPict.FileName = Currentfp;
            _Retry:
                if (sfdPict.ShowDialog(this) != DialogResult.OK)
                    return false;
                if (File.Exists(sfdPict.FileName) && 0 != (File.GetAttributes(sfdPict.FileName) & FileAttributes.ReadOnly)) {
                    switch (MessageBox.Show(this, "指定したファイルは読取専用です。書き込みできません。\n\n書き込みできるように、先に読取専用を解除しますか?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)) {
                        case DialogResult.Yes:
                            try {
                                File.SetAttributes(sfdPict.FileName, File.GetAttributes(sfdPict.FileName) & ~FileAttributes.ReadOnly);
                            }
                            catch (Exception) {
                                MessageBox.Show(this, "失敗しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                goto _Retry;
                            }
                            MessageBox.Show(this, "完了しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case DialogResult.No:
                            goto _Retry;
                        case DialogResult.Cancel:
                        default:
                            MessageBox.Show(this, "保存を中止しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                    }
                }
                fp = sfdPict.FileName;
            }

            FileAttributes att = fpatt;

            String fpTmp = tempfp.GetTempFileName() + ".tif";

            FIMULTIBITMAP tif = FreeImage.OpenMultiBitmap(FREE_IMAGE_FORMAT.FIF_TIFF, fpTmp, true, false, false, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            try {
                for (int x = 0; x < tv.Picts.Count; x++) {
                    FIBITMAP dib = tv.Picts[x].Picture;
                    FREE_IMAGE_COLOR_TYPE fict = FreeImage.GetColorType(dib);
                    if (fict == FREE_IMAGE_COLOR_TYPE.FIC_MINISBLACK && FreeImage.GetBPP(dib) == 1) {
                        FreeImage.Invert(dib);
                        try {
                            Palette P = FreeImage.GetPaletteEx(dib);
                            RGBQUAD t0 = P.GetValue(0);
                            RGBQUAD t1 = P.GetValue(1);
                            P.SetValue(t0, 1);
                            P.SetValue(t1, 0);
                        }
                        catch (NullReferenceException) {

                        }
                    }
                }
                for (int x = 0; x < tv.Picts.Count; x++) {
                    FIBITMAP dib = tv.Picts[x].Picture;

                    FreeImage.AppendPage(tif, dib);
                }
                if (!FreeImage.CloseMultiBitmapEx(ref tif, FREE_IMAGE_SAVE_FLAGS.DEFAULT)) {
                    tif.SetNull();
                    MessageBox.Show(this, "保存に失敗しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                while (true) {
                    try {
                        if (File.Exists(fp))
                            File.SetAttributes(fp, FileAttributes.Normal);
                        File.Copy(fpTmp, fp, true);
                        break;
                    }
                    catch (Exception err) {
                        DialogResult res = MessageBox.Show(this, "保存に失敗しました。\n\n" + err.Message, Application.ProductName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                        if (res == DialogResult.Cancel) return false;
                    }
                }

                File.SetAttributes(fp, att & (FileAttributes.System | FileAttributes.Hidden) | FileAttributes.Archive);

                Currentfp = fp;
                Modified = false;
                ReadOnly = false;
            }
            finally {
                FreeImage.CloseMultiBitmap(tif, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
            }

            return true;
        }

        private void ss_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {

        }

        private void bAbout_Click(object sender, EventArgs e) {
            using (AboutForm form = new AboutForm())
                form.ShowDialog();
        }

        private void bSaveas_Click(object sender, EventArgs e) {
            Savef(null);
        }

        private void bMail_Click(object sender, EventArgs e) {
            if (tv.SelCount == 0) {
                MessageBox.Show(this, "メール送信できるページが有りません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            String fpTmp = tempfp.GetTempFileName() + ".tif";

            FIMULTIBITMAP tif = FreeImage.OpenMultiBitmap(FREE_IMAGE_FORMAT.FIF_TIFF, fpTmp, true, false, true, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            try {
                for (int x = tv.SelFirst; x <= tv.SelLast; x++) {
                    FIBITMAP dib = tv.Picts[x].Picture;

                    FREE_IMAGE_COLOR_TYPE fict = FreeImage.GetColorType(dib);
                    if (fict == FREE_IMAGE_COLOR_TYPE.FIC_MINISBLACK) {
                        FreeImage.Invert(dib);
                        try {
                            Palette P = FreeImage.GetPaletteEx(dib);
                            RGBQUAD t0 = P.GetValue(0);
                            RGBQUAD t1 = P.GetValue(1);
                            P.SetValue(t0, 1);
                            P.SetValue(t1, 0);
                        }
                        catch (NullReferenceException) {

                        }
                    }
                }
                for (int x = tv.SelFirst; x <= tv.SelLast; x++) {
                    FIBITMAP dib = tv.Picts[x].Picture;

                    FreeImage.AppendPage(tif, dib);
                }
            }
            finally {
                FreeImage.CloseMultiBitmap(tif, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
            }

            Process.Start(Path.Combine(Application.StartupPath, "MAPISendMailSa.exe"), " \"" + fpTmp + "\"");
        }

        private void JForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                switch (TrySave()) {
                    case TState.Yes:
                    case TState.No:
                        break;
                    case TState.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void JForm_DragEnter(object sender, DragEventArgs e) {
            e.Effect = e.AllowedEffect & (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None);
        }

        private void JForm_DragDrop(object sender, DragEventArgs e) {
            String[] alfp = e.Data.GetData(DataFormats.FileDrop) as String[];
            if (alfp != null) {
                DialogResult res = DialogResult.None;
                foreach (String fp in alfp) {
                    if (File.Exists(fp)) {
                        if (Currentfp == null)
                            res = DialogResult.OK;
                        else
                            using (OpenWayForm form = new OpenWayForm(false)) {
                                res = form.ShowDialog();
                            }
                        break;
                    }
                }

                if (false) { }
                else if (res == DialogResult.OK) // Load it
                    QueueOpenit(alfp);
                else if (res == DialogResult.Retry)// Append
                    SynchronizationContext.Current.Post(delegate {
                        using (WIPPanel wip = new WIPPanel(this)) {
                            foreach (String fp in alfp) {
                                if (File.Exists(fp)) {
                                    String fpOpen = TFUt.FilterUnsafeTIFFTags(fp, tempfp);
                                    FREE_IMAGE_FORMAT fmt = FREE_IMAGE_FORMAT.FIF_UNKNOWN;
                                    FIMULTIBITMAP tif = FreeImage.OpenMultiBitmapEx(fpOpen, ref fmt, FREE_IMAGE_LOAD_FLAGS.DEFAULT, false, true, false);
                                    try {
                                        int cnt = FreeImage.GetPageCount(tif);
                                        for (int i = 0; i < cnt; i++) {
                                            FIBITMAP fib = FreeImage.LockPage(tif, i);
                                            try {
                                                tv.Picts.Add(new TvPict(FreeImage.Clone(fib)));
                                            }
                                            finally {
                                                FreeImage.UnlockPage(tif, fib, false);
                                            }
                                        }
                                    }
                                    finally {
                                        FreeImage.CloseMultiBitmapEx(ref tif);
                                    }
                                }
                            }
                        }
                    }, null);
            }
        }

        private void QueueOpenit(string[] alfp) {
            SynchronizationContext.Current.Post(delegate {
                foreach (String fp in alfp) {
                    if (File.Exists(fp)) {
                        switch (TrySave()) {
                            case TState.Yes:
                            case TState.No:
                                Openf(fp);
                                break;
                        }
                        return;
                    }
                }
            }, null);
        }

        private void bMailContents_Click(object sender, EventArgs e) {
            switch (TrySave2()) {
                case TState.Yes: {
                        if (tv.SelCount == 0) {
                            MessageBox.Show(this, "メール送信できるページが有りません。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        else {
                            Process.Start(Path.Combine(Application.StartupPath, "MAPISendMailSa.exe"), " \"" + Currentfp + "\"");
                        }
                        break;
                    }
                default: {
                        MessageBox.Show(this, "送信するには、先に保存してください。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
            }
        }

        private void JForm_FormClosed(object sender, FormClosedEventArgs e) {
            tempfp.Cleanup();

            Settings.Default.RestoreBounds = Bounds;
            Settings.Default.Save();
        }

        private void bAppend_Click(object sender, EventArgs e) {
            if (ofdAppend.ShowDialog(this) == DialogResult.OK) {
                String[] alfp = ofdAppend.FileNames;
                using (WIPPanel wip = new WIPPanel(this)) {
                    foreach (String fp in alfp) {
                        if (File.Exists(fp)) {
                            String fpOpen = TFUt.FilterUnsafeTIFFTags(fp, tempfp);
                            FREE_IMAGE_FORMAT fmt = FREE_IMAGE_FORMAT.FIF_UNKNOWN;
                            FIMULTIBITMAP tif = FreeImage.OpenMultiBitmapEx(fpOpen, ref fmt, FREE_IMAGE_LOAD_FLAGS.DEFAULT, false, true, false);
                            try {
                                int cnt = FreeImage.GetPageCount(tif);
                                for (int i = 0; i < cnt; i++) {
                                    FIBITMAP fib = FreeImage.LockPage(tif, i);
                                    try {
                                        tv.Picts.Add(new TvPict(FreeImage.Clone(fib)));
                                    }
                                    finally {
                                        FreeImage.UnlockPage(tif, fib, false);
                                    }
                                }
                            }
                            finally {
                                FreeImage.CloseMultiBitmapEx(ref tif);
                            }
                        }
                    }
                }
            }
        }

        private void bNega_Click(object sender, EventArgs e) {
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast; x++) {
                tv.Picts[x].Nega();
                tv.Picts.ResetItem(x);
            }
        }

        class PFUt {
            public String Dir { get { return Environment.ExpandEnvironmentVariables("%APPDATA%\\jtifedit3\\history"); } }

            public String Next(String fext) {
                String dir = Dir;
                Directory.CreateDirectory(dir);
                for (int t = 1; ; t++) {
                    String fp = dir + "\\" + DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分 ") + " " + t.ToString("0000") + fext;
                    if (File.Exists(fp)) continue;

                    try {
                        DateTime dt0 = DateTime.Now.AddMonths(-3); // 3 カ月で削除
                        foreach (FileInfo fi in new DirectoryInfo(dir).GetFiles()) {
                            if (fi.LastWriteTime < dt0) fi.Delete();
                        }
                    }
                    catch (Exception) {
                        // 削除失敗
                    }
                    return fp;
                }
            }
        }

        PFUt pfut = new PFUt();

        private void bMSPaint_Click(object sender, EventArgs e) {
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast;) {
                FIBITMAP dib = tv.Picts[x].Picture;
                bool isMono = false;// (FreeImage.GetBPP(dib) == 1);
                String fp = pfut.Next(isMono ? ".bmp" : ".png");
                uint rx = FreeImage.GetResolutionX(dib);
                uint ry = FreeImage.GetResolutionY(dib);
                if (isMono)
                    FreeImage.Save(FREE_IMAGE_FORMAT.FIF_BMP, dib, fp, FREE_IMAGE_SAVE_FLAGS.BMP_SAVE_RLE);
                else
                    FreeImage.Save(FREE_IMAGE_FORMAT.FIF_PNG, dib, fp, FREE_IMAGE_SAVE_FLAGS.PNG_Z_DEFAULT_COMPRESSION);
                using (EdForm form = new EdForm(fp)) {
                    while (true) {
                        try {
                            Process.Start("mspaint.exe", " \"" + fp + "\"");
                        }
                        catch (Exception) {
                            MessageBox.Show(this, "失敗しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        switch (form.ShowDialog()) {
                            case DialogResult.OK: {
                                    // ペイントの不具合で、モノクロ BMP は常にモノクロ BMP で保存される?
                                    FIBITMAP dibNew = isMono
                                        ? FreeImage.Load(FREE_IMAGE_FORMAT.FIF_BMP, fp, FREE_IMAGE_LOAD_FLAGS.DEFAULT)
                                        : FreeImage.Load(FREE_IMAGE_FORMAT.FIF_PNG, fp, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
                                    FreeImage.SetResolutionX(dibNew, rx);
                                    FreeImage.SetResolutionY(dibNew, ry);
                                    tv.Picts[x].Picture = dibNew;
                                    tv.Picts.ResetItem(x);
                                    goto _Break;
                                }
                            case DialogResult.Retry:
                                continue;
                            default:
                                return;
                        }
                    _Break:;
                        break;
                    }
                }
                break;
            }
        }

        private void llHideExifCut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
        }

        private void bHideExifCut_Click(object sender, EventArgs e) {
            tlpExifCut.Hide();
        }

        class PUt {
            public static PaperSize Guess(Bitmap pic) {
                float xDPI = pic.HorizontalResolution;
                float yDPI = pic.VerticalResolution;
                int cx = pic.Width;
                int cy = pic.Height;

                float xmm = cx / xDPI * 2.560344827586206896551724137931f;
                float ymm = cy / yDPI * 2.560344827586206896551724137931f;

                float X = Math.Min(xmm, ymm);
                float Y = Math.Max(xmm, ymm);

                //MessageBox.Show(String.Format("dpi({0},{1}) cxcy({2},{3}) mm({4},{5})", xDPI, yDPI, cx, cy, xmm, ymm));

                { 	// B5
                    float D = 2, Px = 18.2f, Py = 25.72f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B5", 717, 1012);
                }
                {	// A4
                    float D = 2, Px = 21.00f, Py = 29.70f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("A4", 827, 1169);
                }
                {	// B4
                    float D = 2, Px = 25.72f, Py = 36.41f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B4", 1012, 1433);
                }
                { 	// A3
                    float D = 2, Px = 29.7f, Py = 42.0f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("A3", 1169, 1654);
                }
                { 	// B3
                    float D = 2, Px = 36.41f, Py = 51.51f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B3", 1433, 2028);
                }
                { 	// A2
                    float D = 2, Px = 42.00f, Py = 59.40f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("A2", 1654, 2339);
                }
                { 	// B2
                    float D = 2, Px = 51.51f, Py = 72.81f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B2", 2028, 2867);
                }
                { 	// A1
                    float D = 2, Px = 59.41f, Py = 84.10f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("A1", 2339, 3311);
                }
                { 	// B1
                    float D = 2, Px = 72.81f, Py = 103.01f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B1", 2867, 4056);
                }
                { 	// A0
                    float D = 2, Px = 84.10f, Py = 118.89f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("A0", 3311, 4680);
                }
                { 	// B0
                    float D = 2, Px = 103.01f, Py = 145.59f;
                    if (Px - D <= X && X <= Px + D && Py - D <= Y && Y <= Py + D)
                        return new PaperSize("B0", 4056, 5732);
                }

                return new PaperSize("A4", 827, 1169);
            }

            internal static PaperSize FindOrGuess(
                Bitmap pic,
                PrinterSettings.PaperSizeCollection paperSizes
            ) {
                float xDPI = pic.HorizontalResolution;
                float yDPI = pic.VerticalResolution;
                int cx = pic.Width;
                int cy = pic.Height;

                int a = (int)(cx / xDPI * 100);
                int b = (int)(cy / yDPI * 100);

                var width = Math.Min(a, b);
                var height = Math.Max(a, b);

                var wDiff = width * 0.01;
                var hDiff = height * 0.01;

                foreach (PaperSize paperSize in paperSizes) {
                    if (true
                        && Math.Abs(paperSize.Width - width) < wDiff
                        && Math.Abs(paperSize.Height - height) < hDiff
                    ) {
                        return paperSize;
                    }
                }

                return Guess(pic);
            }
        }

        class PUt2 {
            internal static bool IsLandscape(Bitmap pic) {
                return pic.Width * pic.HorizontalResolution > pic.Height * pic.VerticalResolution;
            }
        }

        class PUt5 {
            internal static SizeF SizePortlait(TvPict pict) {
                FIBITMAP fib = pict.Picture;
                uint cx = FreeImage.GetWidth(fib);
                uint cy = FreeImage.GetHeight(fib);
                uint rx = FreeImage.GetResolutionX(fib);
                uint ry = FreeImage.GetResolutionY(fib);
                float vx = (float)cx / (float)rx;
                float vy = (float)cy / (float)ry;
                float inchX = Math.Min(vx, vy); // 短辺
                float inchY = Math.Max(vx, vy); // 長辺
                return new SizeF(inchX, inchY);
            }

            internal static bool ShouldCut(TvPict e1, TvPict e2) {
                SizeF s1 = SizePortlait(e1);
                SizeF s2 = SizePortlait(e2);
                return (Math.Abs(s1.Width - s2.Width) > 0.5f || Math.Abs(s1.Height - s2.Height) > 0.5f);
            }
        }

        private void bPrint_Click(object sender, EventArgs e) {
            if (TestXYRes()) {
                switch (MessageBox.Show(this, "縦と横の解像度が異なる画像が有ります。印刷に問題が出るかもしれません。\n\n先に修正しますか。", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)) {
                    case DialogResult.Yes:
                        FixDPI();
                        CheckXYRes();
                        break;
                    case DialogResult.No:
                        break;
                    default:
                        return;
                }
            }

            pGo.PrinterSettings.MinimumPage = 1;
            pGo.PrinterSettings.MaximumPage = tv.Picts.Count;

            if (ReferenceEquals(sender, bPrintAll)) {
                pGo.PrinterSettings.FromPage = pGo.PrinterSettings.MinimumPage;
                pGo.PrinterSettings.ToPage = pGo.PrinterSettings.MaximumPage;
                pGo.PrinterSettings.PrintRange = System.Drawing.Printing.PrintRange.AllPages;
            }
            else {
                pGo.PrinterSettings.FromPage = 1 + tv.SelFirst;
                pGo.PrinterSettings.ToPage = 1 + tv.SelLast;
                pGo.PrinterSettings.PrintRange = System.Drawing.Printing.PrintRange.SomePages;
            }

            if (pGo.PrinterSettings.FromPage < 1 || tv.Picts.Count < 1) {
                return;
            }

            if (pGo.ShowDialog(this) != DialogResult.OK)
                return;

            pGo.PrinterSettings.Collate = true; // PUt5.GetCollate(pGo.PrinterSettings);

            pis.Clear();

            int y0 = pGo.PrinterSettings.FromPage;
            int y1 = pGo.PrinterSettings.ToPage;
            if (pGo.PrinterSettings.PrintRange == PrintRange.AllPages) {
                y0 = pGo.PrinterSettings.MinimumPage;
                y1 = pGo.PrinterSettings.MaximumPage;
            }
            for (; y0 <= y1; y0++) {
                PrintInfo pi = new PrintInfo();
                pi.CurPage = y0;
                pis.Add(pi);
            }

            if (Settings.Default.PrintPaper == PaperKind.Custom) {
                // 混載して印刷
                for (int y = 0, cy = pis.Count; y + 1 < cy; y++) {
                    pis[y].CutHere = PUt5.ShouldCut(tv.Picts[pis[y].CurPage - 1], tv.Picts[pis[y + 1].CurPage - 1]);
                }
            }

            iPi = 0;
            while (iPi < pis.Count) {
                isPrinted = false;
                pDocGo.DocumentName = PrintTitle + " p." + pis[iPi].CurPage.ToString("0000");
                pDocGo.Print();
                if (!isPrinted) break;
            }
        }

        List<PrintInfo> pis = new List<PrintInfo>();
        int iPi = 0;
        Bitmap picFrm;
        bool isPrinted = false;

        class PrintInfo {
            public int CurPage;
            public bool CutHere;
        }

        class PUt3 {
            internal static PaperSize Find(PrinterSettings.PaperSizeCollection paperSizeCollection, PaperKind paperKind) {
                foreach (PaperSize paperSize in paperSizeCollection) {
                    if (paperSize.Kind == paperKind)
                        return paperSize;
                }
                return new PaperSize();
            }
        }

        private void pDocGo_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e) {
            PrintInfo pi = pis[iPi];
            TvPict pict = tv.Picts[pi.CurPage - 1];
            FIBITMAP dib = pict.Picture;
            picFrm = FreeImage.GetBitmap(dib);

            e.PageSettings.Margins = Settings.Default.PrintMargins; // new Margins(20, 20, 20, 20); // 5mm
            e.PageSettings.PaperSize = Settings.Default.PrintPaper == PaperKind.Custom
                ? PUt.FindOrGuess(picFrm, e.PageSettings.PrinterSettings.PaperSizes) // 混載
                : PUt3.Find(e.PageSettings.PrinterSettings.PaperSizes, Settings.Default.PrintPaper); // 統一サイズ
            e.PageSettings.Landscape = false; // 常に縦
        }

        private void pDocGo_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
            e.HasMorePages = iPi + 1 < pis.Count && !pis[iPi].CutHere;
            Graphics cv = e.Graphics;
            SizeF byReso = new SizeF(picFrm.Width / picFrm.HorizontalResolution, picFrm.Height / picFrm.VerticalResolution);
            bool doRotl = (byReso.Width > byReso.Height);
            cv.PageUnit = GraphicsUnit.Inch;
            if (!doRotl) {
                cv.DrawImage(picFrm, FitRect3.FitF(cv.VisibleClipBounds, byReso));
            }
            else {
                RectangleF rcHandl = FitRect3.FitF(cv.VisibleClipBounds, new SizeF(byReso.Height, byReso.Width));
                cv.DrawImage(picFrm, new PointF[] { new PointF(rcHandl.Left, rcHandl.Bottom), new PointF(rcHandl.Left, rcHandl.Top), new PointF(rcHandl.Right, rcHandl.Bottom) });
            }
            iPi++;
            if (picFrm != null) {
                picFrm.Dispose();
                picFrm = null;
            }
            isPrinted = true;
        }

        private void pDocGo_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e) {

        }

        private void bPageSetting_Click(object sender, EventArgs e) {
            using (PageSetForm form = new PageSetForm())
                form.ShowDialog(this);
        }

        private void bNewDPI_Click(object sender, EventArgs e) {
            uint rx = 0, ry = 0;
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast; x++) {
                FIBITMAP dib = tv.Picts[x].Picture;
                if (x == tv.SelFirst) {
                    rx = FreeImage.GetResolutionX(dib);
                    ry = FreeImage.GetResolutionY(dib);

                    using (DPIForm form = new DPIForm(new Size(Convert.ToInt32(FreeImage.GetWidth(dib)), Convert.ToInt32(FreeImage.GetHeight(dib))), rx, ry)) {
                        if (form.ShowDialog() != DialogResult.OK)
                            return;

                        if (form.Res is DPIForm.ForceDPI) {
                            DPIForm.ForceDPI p = (DPIForm.ForceDPI)form.Res;

                            tv.Picts[x].SetDPI(p.rx, p.ry);
                            tv.Picts.ResetItem(x);
                        }
                        else if (form.Res is DPIForm.SameDPI) {
                            DPIForm.SameDPI p = (DPIForm.SameDPI)form.Res;

                            FIBITMAP frm = tv.Picts[x].Picture;
                            uint bpp = FreeImage.GetBPP(frm);
                            FIBITMAP fib = FreeImage.Rescale(frm, p.npx, p.npy, FREE_IMAGE_FILTER.FILTER_BOX);
                            FIBITMAP fibNew = RestoreBPP(ref fib, bpp);
                            uint dpi = (uint)p.dpi;
                            tv.Picts[x].Picture = fibNew;
                            tv.Picts[x].SetDPI(dpi, dpi);
                            tv.Picts.ResetItem(x);
                        }
                        break;
                    }
                }
            }
        }

        private void bHideDPIWarn_Click(object sender, EventArgs e) {
            tlpDPIWarn.Hide();
        }

        private void llFixDPI_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            FixDPI();

            CheckXYRes();
        }

        private void FixDPI() {
            foreach (TvPict t in tv.Picts) {
                FIBITMAP frm = t.Picture;
                uint h = FreeImage.GetResolutionX(frm);
                uint v = FreeImage.GetResolutionY(frm);
                if (h != 0 && v != 0 && h != v) {
                    uint min = Math.Min(h, v);
                    uint max = Math.Max(h, v);
                    uint new1 = 0;
                    if (false) { }
                    else if (max == 203 && min == 97) new1 = 200;
                    else if (max == 203 && min == 98) new1 = 200;
                    else if (max == 203 && min == 196) new1 = 200;
                    else if (max == 391 && min == 203) new1 = 400;
                    else if (max == 392 && min == 203) new1 = 400;
                    else if (max <= 200) new1 = 200;
                    else if (max <= 300) new1 = 300;
                    else if (max <= 400) new1 = 400;
                    else if (max <= 500) new1 = 500;
                    else if (max <= 600) new1 = 600;
                    else new1 = max;

                    if (new1 != 0) {
                        uint bpp = FreeImage.GetBPP(frm);
                        uint dpi = (uint)new1;
                        uint rx = h;
                        uint ry = v;
                        int npx = (int)((FreeImage.GetWidth(frm) * dpi) / rx);
                        int npy = (int)((FreeImage.GetHeight(frm) * dpi) / ry);
                        FIBITMAP fib = FreeImage.Rescale(frm, npx, npy, FREE_IMAGE_FILTER.FILTER_BOX);
                        FIBITMAP fibNew = RestoreBPP(ref fib, bpp);
                        t.Picture = fibNew;
                        t.SetDPI(dpi, dpi);
                        tv.Picts.ResetItem(tv.Picts.IndexOf(t));
                    }
                }
            }
        }


        private void Add1(int cx, int cy) {
            int y = tv.SelFirst + 1;
            if (y < 0) y = 0;
            FIBITMAP fib = FreeImage.AllocateEx(cx, cy, 1, new RGBQUAD(Color.White), FREE_IMAGE_COLOR_OPTIONS.FICO_DEFAULT, new RGBQUAD[] {
                new RGBQUAD(Color.Black),
                new RGBQUAD(Color.White),
            });
            FreeImage.SetResolutionX(fib, 300);
            FreeImage.SetResolutionY(fib, 300);
            tv.Picts.Insert(y, new TvPict(fib));
            tv.SSel = y;
        }

        private void bA3P300_Click(object sender, EventArgs e) { Add1(3504, 4960); }
        private void bA4P300_Click(object sender, EventArgs e) { Add1(2480, 3504); }

        private void bB4P300_Click(object sender, EventArgs e) { Add1(3024, 4288); }
        private void bB5P300_Click(object sender, EventArgs e) { Add1(2144, 3024); }

        private void bJapanesePostCard300_Click(object sender, EventArgs e) { Add1(1168, 1744); }

        private void bHist_Click(object sender, EventArgs e) {
            String dir = pfut.Dir;
            Directory.CreateDirectory(dir);

            try {
                Process.Start(dir);
            }
            catch (Exception err) {
                MessageBox.Show(this, "フォルダを表示できません。", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void bTIFI_Click(object sender, EventArgs e) {
            if (Currentfp == null) return;

            StringWriter wr = new StringWriter();

            using (FileStream fs = File.OpenRead(Currentfp)) {
                CTIF tif = new CTIF();
                tif.Read(fs);

                int y = 0;
                foreach (CTIF.P1 p1 in tif.Pages) {
                    ++y;
                    String fmt = "";
                    // http://www.awaresystems.be/imaging/tiff/tifftags/compression.html
                    switch (p1.Compression) {
                        case 1: fmt = "No compression"; break;
                        case 2: fmt = "CCITT modified Huffman RLE"; break;
                        case 32773: fmt = "PackBits compression, aka Macintosh RLE"; break;
                        case 3: fmt = "CCITT Group 3 fax encoding"; break;
                        case 4: fmt = "CCITT Group 4 fax encoding"; break;
                        case 5: fmt = "LZW"; break;
                        case 6: fmt = "JPEG ('old-style' JPEG)"; break;
                        case 7: fmt = "JPEG ('new-style' JPEG)"; break;
                        case 8: fmt = "Deflate ('Adobe-style')"; break;
                        case 9: fmt = "T.85"; break;
                        case 10: fmt = "T.43"; break;
                    }
                    String BitsPerSample = "";
                    if (p1.BitsPerSample != null)
                        foreach (int v in p1.BitsPerSample)
                            BitsPerSample += " " + v;
                    wr.WriteLine("p.{0} = {1} ({2}) {7}x{8} {3}x{4} BitsPerSample='{5}' SamplesPerPixel={6}"
                        , y
                        , fmt
                        , p1.Compression
                        , p1.HorizontalResolution
                        , p1.VerticalResolution
                        , BitsPerSample.Trim()
                        , p1.SamplesPerPixel
                        , p1.Width
                        , p1.Height
                        );
                }
            }

            MessageBox.Show("" + wr);
        }

        private void bScan_Click(object sender, EventArgs e) {

        }

        private void bSelScanner_Click(object sender, EventArgs e) {
            if (isX64) {
                MBox.Show(this, messages.UseTwain32, Text, icon: MessageBoxIcon.Exclamation);
            }
            else {
                if (!PrepareTwain()) {
                    return;
                }

                tw.SelectSource();
                tw = null;
            }
        }

        Twain tw = null;

        bool insertMode = false;

        void ScanTwain() {
            if (isX64) {
                MBox.Show(this, messages.UseTwain32, Text, icon: MessageBoxIcon.Exclamation);
            }
            else {
                if (!PrepareTwain()) {
                    return;
                }

                tw.StartScanning(new ScanSettings {
                    ShowTwainUI = true,
                    ShowProgressIndicatorUI = true,
                    ShouldTransferAllPages = true,
                    Rotation = new RotationSettings {
                    },
                });
            }
        }

        private bool PrepareTwain() {
            if (tw != null) {
                return false;
            }

            tw = new Twain(new WinFormsWindowMessageHook(this));
            tw.TransferImage += tw_ImageAvail;
            tw.ScanningComplete += tw_EndingScan;

            return true;
        }

        private void bScanInsert_Click(object sender, EventArgs e) {
            insertMode = true;
            ScanTwain();
        }

        private void bScanAppend_Click(object sender, EventArgs e) {
            insertMode = false;
            ScanTwain();
        }

        void tw_EndingScan(object sender, ScanningCompleteEventArgs e) {
            tw = null;
        }

        void tw_ImageAvail(object sender, TransferImageEventArgs e) {
            if (insertMode) {
                FIBITMAP fib = FreeImage_CreateFromBitmap(e.Image);
                tv.Picts.Insert(Math.Max(0, tv.SelFirst), new TvPict(fib));
            }
            else {
                FIBITMAP fib = FreeImage_CreateFromBitmap(e.Image);
                tv.Picts.Add(new TvPict(fib));
            }
        }

        private void bFreeRot_Click(object sender, EventArgs e) {
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast;) {
                FIBITMAP dib = tv.Picts[x].Picture;
                using (LeanForm form = new LeanForm(dib)) {
                    if (form.ShowDialog(this) == DialogResult.Yes) {
                        using (WIPPanel wip = new WIPPanel(this)) {
                            FIBITMAP dib2 = FreeImage.ConvertTo32Bits(dib);
                            try {
                                FreeImage.Invert(dib2);
                                float ox = FreeImage.GetWidth(dib2) / 2;
                                float oy = FreeImage.GetHeight(dib2) / 2;
                                FIBITMAP dib3 = FreeImage.RotateEx(dib2, form.newrot, 0, 0, ox, oy, true);
                                FreeImage.Invert(dib3);
                                FreeImage.UnloadEx(ref dib2);
                                try {
                                    FIBITMAP dibFin = RestoreBPP(ref dib3, FreeImage.GetBPP(dib));
                                    tv.Picts[x].Picture = dibFin;
                                    tv.Picts.ResetItem(x);
                                }
                                finally {
                                    FreeImage.UnloadEx(ref dib3);
                                }
                            }
                            finally {
                                FreeImage.UnloadEx(ref dib2);
                            }
                        }
                    }
                }
                break;
            }
        }

        private FIBITMAP RestoreBPP(ref FIBITMAP dibReleaseAfterConversion, uint bpp) {
            FIBITMAP dibFin;
            switch (bpp) {
                case 1:
                    dibFin = FreeImage.ConvertColorDepth(dibReleaseAfterConversion, FREE_IMAGE_COLOR_DEPTH.FICD_01_BPP | FREE_IMAGE_COLOR_DEPTH.FICD_FORCE_GREYSCALE);
                    break;
                case 4:
                    dibFin = FreeImage.ConvertColorDepth(dibReleaseAfterConversion, FREE_IMAGE_COLOR_DEPTH.FICD_04_BPP);
                    break;
                case 8:
                    dibFin = FreeImage.ConvertColorDepth(dibReleaseAfterConversion, FREE_IMAGE_COLOR_DEPTH.FICD_08_BPP);
                    break;
                case 15:
                case 16:
                case 24:
                default:
                    dibFin = FreeImage.ConvertColorDepth(dibReleaseAfterConversion, FREE_IMAGE_COLOR_DEPTH.FICD_24_BPP);
                    break;
                case 32:
                    dibFin = FreeImage.ConvertColorDepth(dibReleaseAfterConversion, FREE_IMAGE_COLOR_DEPTH.FICD_32_BPP);
                    break;
            }
            if (dibReleaseAfterConversion == dibFin) {
                dibReleaseAfterConversion.SetNull();
            }
            return dibFin;
        }

        private void bWriteText_Click(object sender, EventArgs e) {
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast;) {
                using (TextForm form = new TextForm()) {
                    Bitmap bitmapSource = tv.Picts[x].GetBitmapCopy();
                    form.SetPic(bitmapSource);
                    if (e is EventArgsWithFile) {
                        form.LoadTemplateFrom(((EventArgsWithFile)e).fp);
                    }
                    if (form.ShowDialog() == DialogResult.OK) {
                        tv.Picts[x].Picture = FreeImage_CreateFromBitmap(form.CompositeTo(bitmapSource));
                        tv.Picts.ResetItem(x);
                    }
                }
                break;
            }
        }

        private FIBITMAP FreeImage_CreateFromBitmap(Bitmap bitmap) {
            FIBITMAP dib = FreeImage.CreateFromBitmap(bitmap);
            FreeImage.SetResolutionX(dib, (uint)bitmap.HorizontalResolution);
            FreeImage.SetResolutionY(dib, (uint)bitmap.VerticalResolution);
            return dib;
        }

        private void bWriteText_DropDownOpening(object sender, EventArgs e) {
            while (bWriteText.DropDownItems.Count >= 2) {
                bWriteText.DropDownItems.RemoveAt(1);
            }

            String[] files = Directory.GetFiles(Program.TemplateDir, "*.xml");
            if (files.Length > 0) {
                bWriteText.DropDownItems.Add(new ToolStripSeparator());
                foreach (String fp in files) {
                    String thisfp = fp;
                    bWriteText.DropDownItems.Add(Path.GetFileNameWithoutExtension(fp), null, delegate {
                        EventArgsWithFile ee = new EventArgsWithFile();
                        ee.fp = thisfp;
                        bWriteText_Click(this, ee);
                    });
                }
            }
        }

        class EventArgsWithFile : EventArgs {
            public String fp;
        }

        private void bWriteImage_ButtonClick(object sender, EventArgs e) {
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast;) {
                if (ofdImport.ShowDialog(this) == DialogResult.OK) {
                    writeImage(x, ofdImport.FileName);
                }
                break;
            }
        }

        private void writeImage(int x, string fp) {
            using (PastePicForm form = new PastePicForm()) {
                Bitmap bitmapSource = tv.Picts[x].GetBitmapCopy();
                form.SetPic(bitmapSource);
                form.ImportPic(fp);
                if (form.ShowDialog() == DialogResult.OK) {
                    tv.Picts[x].Picture = FreeImage_CreateFromBitmap(form.CompositeTo(bitmapSource));
                    tv.Picts.ResetItem(x);
                }
            }
        }

        private void bWriteImageNew_Click(object sender, EventArgs e) {
            bWriteImage_ButtonClick(sender, e);
        }

        private void bWriteImage_DropDownOpening(object sender, EventArgs e) {
#if false
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast; ) {
                int thisx = x;
                while (bWriteImage.DropDownItems.Count >= 2) {
                    bWriteImage.DropDownItems.RemoveAt(1);
                }

                String[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
                if (files.Length > 0) {
                    bWriteImage.DropDownItems.Add(new ToolStripSeparator());
                    foreach (String fp in files) {
                        if ("|.bmp|.gif|.jpg|.jpeg|.png.|.emf|.wmf|".IndexOf("|" + Path.GetExtension(fp).ToLowerInvariant() + "|") < 0) {
                            continue;
                        }
                        String thisfp = fp;
                        bWriteImage.DropDownItems.Add(Path.GetFileNameWithoutExtension(fp), null, delegate {
                            writeImage(thisx, thisfp);
                        });
                    }
                }

                break;
            }
#endif
        }

        private void bCutPic_Click(object sender, EventArgs e) {
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast;) {
                using (CutForm form = new CutForm()) {
                    Bitmap bitmapSource = tv.Picts[x].GetBitmapCopy();
                    form.SetPic(bitmapSource);
                    if (form.ShowDialog() == DialogResult.OK) {

                    }
                }
                break;
            }
        }

        private void bFillPic_Click(object sender, EventArgs e) {
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast;) {
                using (FillForm form = new FillForm()) {
                    Bitmap bitmapSource = tv.Picts[x].GetBitmapCopy();
                    form.SetPic(bitmapSource);
                    if (form.ShowDialog() == DialogResult.OK) {
                        tv.Picts[x].Picture = FreeImage_CreateFromBitmap(form.CompositeTo(bitmapSource));
                        tv.Picts.ResetItem(x);
                    }
                }
                break;
            }
        }

        private void bSelectWiaDevice_Click(object sender, EventArgs e) {
            Settings.Default.SelectedWiaDeviceId = WIAScanner.SelectDevice();
            Settings.Default.Save();
        }

        private void bInsertWia_Click(object sender, EventArgs e) {
            try {
                if (string.IsNullOrEmpty(Settings.Default.SelectedWiaDeviceId)) {
                    Settings.Default.SelectedWiaDeviceId = WIAScanner.SelectDevice();
                    if (string.IsNullOrEmpty(Settings.Default.SelectedWiaDeviceId)) {
                        return;
                    }
                }
                var bitmaps = WIAScanner.AcquireImages(Settings.Default.SelectedWiaDeviceId);
                foreach (var bitmap in bitmaps) {
                    FIBITMAP fib = FreeImage_CreateFromBitmap(bitmap);
                    tv.Picts.Insert(Math.Max(0, tv.SelFirst), new TvPict(fib));
                }
            }
            catch (Exception ex) {
                MessageBox.Show(this, "エラーが発生しました: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void bAppendWia_Click(object sender, EventArgs e) {
            try {
                if (string.IsNullOrEmpty(Settings.Default.SelectedWiaDeviceId)) {
                    Settings.Default.SelectedWiaDeviceId = WIAScanner.SelectDevice();
                    if (string.IsNullOrEmpty(Settings.Default.SelectedWiaDeviceId)) {
                        return;
                    }
                }
                var bitmaps = WIAScanner.AcquireImages(Settings.Default.SelectedWiaDeviceId);
                foreach (var bitmap in bitmaps) {
                    FIBITMAP fib = FreeImage_CreateFromBitmap(bitmap);
                    tv.Picts.Add(new TvPict(fib));
                }
            }
            catch (Exception ex) {
                MessageBox.Show(this, "エラーが発生しました: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}