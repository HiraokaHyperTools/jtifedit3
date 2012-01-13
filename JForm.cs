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

namespace jtifedit3 {
    public partial class JForm : Form {
        String fp = null;
        String fpCurrent = null;

        public JForm(String fp) {
            InitializeComponent();

            this.fp = fp;
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
            tv.Picts = new BindingList<TvPict>();
            tv.Picts.ListChanged += new ListChangedEventHandler(Picts_ListChanged);

            TTLTemp = this.Text;

            if (fp != null) Openf(fp);

            tscRate.SelectedIndex = 0;
        }

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
                Currentfp = fp;
                Modified = false;
            }
            finally {
                FreeImage.CloseMultiBitmapEx(ref tif);
            }
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

            if (c == 0) {
                pvw.Pic = null;
            }
            else {
                pvw.Pic = tv.Picts[tv.Sel2].DefTumbGen.GetThumbnail(Size.Empty);
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
                            FIMULTIBITMAP tif = FreeImage.OpenMultiBitmapEx(fpOpen, ref fmt, FREE_IMAGE_LOAD_FLAGS.DEFAULT, false, true, false);
                            try {
                                int cnt = FreeImage.GetPageCount(tif);
                                for (int i = 0; i < cnt; i++) {
                                    FIBITMAP fib = FreeImage.LockPage(tif, i);
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
                            finally {
                                FreeImage.CloseMultiBitmapEx(ref tif);
                            }
                        }
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

            String fpbak = Path.ChangeExtension(fp, ".bak");

            try {
                File.Copy(fp, fpbak, true);
            }
            catch (Exception) {

            }

            bool fSamefp = (fp == Currentfp);
            FileAttributes att = fSamefp ? fpatt : FileAttributes.Normal;

            FIMULTIBITMAP tif = FreeImage.OpenMultiBitmap(FREE_IMAGE_FORMAT.FIF_TIFF, fp, true, false, false, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
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
                Currentfp = fp;
                Modified = false;
                ReadOnly = false;

                if (File.Exists(fpbak)) {
                    try {
                        File.Delete(fpbak);
                    }
                    catch (Exception) {

                    }
                }
            }
            finally {
                FreeImage.CloseMultiBitmap(tif, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
            }

            if (fSamefp) File.SetAttributes(fp, att & (FileAttributes.System | FileAttributes.Hidden) | FileAttributes.Archive);

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

        private void bMSPaint_Click(object sender, EventArgs e) {
            for (int x = tv.SelFirst; 0 <= x && x <= tv.SelLast; ) {
                String fp = Path.GetTempFileName() + ".tif";
                FIBITMAP dib = tv.Picts[x].Picture;
                uint rx = FreeImage.GetResolutionX(dib);
                uint ry = FreeImage.GetResolutionY(dib);
                FreeImage.Save(FREE_IMAGE_FORMAT.FIF_TIFF, dib, fp, (FreeImage.GetBPP(dib) == 1) ? FREE_IMAGE_SAVE_FLAGS.TIFF_CCITTFAX4 : FREE_IMAGE_SAVE_FLAGS.TIFF_LZW);
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
                                    FIBITMAP dibNew = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_TIFF, fp, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
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
                    _Break: ;
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
        }

        class PUt2 {
            internal static bool IsLandscape(Bitmap pic) {
                return pic.Width * pic.HorizontalResolution > pic.Height * pic.VerticalResolution;
            }
        }

        private void bPrint_Click(object sender, EventArgs e) {
            pGo.PrinterSettings.FromPage = 1 + tv.SelFirst;
            pGo.PrinterSettings.ToPage = 1 + tv.SelLast;
            pGo.PrinterSettings.MinimumPage = 1;
            pGo.PrinterSettings.MaximumPage = tv.Picts.Count;
            pGo.PrinterSettings.PrintRange = System.Drawing.Printing.PrintRange.SomePages;

            if (pGo.PrinterSettings.FromPage < 1)
                return;

            if (pGo.ShowDialog(this) != DialogResult.OK)
                return;

            pGo.PrinterSettings.Collate = true; // PUt5.GetCollate(pGo.PrinterSettings);

            stat = new PrintStat();

            if (pGo.PrinterSettings.PrintRange == PrintRange.AllPages) {
                stat.CurPage =
                stat.FromPage = pGo.PrinterSettings.MinimumPage;
                stat.ToPage = pGo.PrinterSettings.MaximumPage;
            }
            else {
                stat.CurPage =
                stat.FromPage = pGo.PrinterSettings.FromPage;
                stat.ToPage = pGo.PrinterSettings.ToPage;
            }

            pDocGo.DocumentName = PrintTitle;
            pDocGo.Print();
        }

        PrintStat stat = null;

        class PrintStat {
            public int CurPage, FromPage, ToPage;
            public Bitmap picFrm;

            public bool HasMorePage {
                get {
                    return CurPage < ToPage;
                }
            }
            public void Next() {
                ++CurPage;
                if (picFrm != null)
                    picFrm.Dispose();
            }
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
            TvPict pict = tv.Picts[stat.CurPage - 1];
            FIBITMAP dib = pict.Picture;
            stat.picFrm = FreeImage.GetBitmap(dib);

            e.PageSettings.Margins = Settings.Default.PrintMargins; // new Margins(20, 20, 20, 20); // 5mm
            e.PageSettings.PaperSize = Settings.Default.PrintPaper == PaperKind.Custom
                ? PUt.Guess(stat.picFrm) // 混載
                : PUt3.Find(e.PageSettings.PrinterSettings.PaperSizes, Settings.Default.PrintPaper); // 統一サイズ
            e.PageSettings.Landscape = PUt2.IsLandscape(stat.picFrm); // 横?
        }

        private void pDocGo_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
            e.HasMorePages = stat.HasMorePage;
            Graphics cv = e.Graphics;
            SizeF byReso = new SizeF(stat.picFrm.Width / stat.picFrm.HorizontalResolution, stat.picFrm.Height / stat.picFrm.VerticalResolution);
            cv.PageUnit = GraphicsUnit.Inch;
            cv.DrawImage(stat.picFrm, FitRect3.FitF(cv.VisibleClipBounds, byReso));
            stat.Next();
        }

        private void pDocGo_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e) {

        }

        private void bPageSetting_Click(object sender, EventArgs e) {
            using (PageSetForm form = new PageSetForm())
                form.ShowDialog(this);
        }
    }
}