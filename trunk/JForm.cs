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
                    Savef(Currentfp);
                    return TState.Yes;
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
                    Savef(Currentfp);
                    return TState.Yes;
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
                    break;
            }
        }

        String TTLTemp = String.Empty;

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
                UpdateTTL();
            }
        }

        private void UpdateTTL() {
            this.Text = (fpCurrent != null)
                ? Path.GetFileName(fpCurrent) + (Modified ? "*" : "") + " -- " + TTLTemp
                : TTLTemp + (Modified ? " *" : "")
                ;
        }

        private void JForm_Load(object sender, EventArgs e) {
            tv.Picts = new BindingList<TvPict>();
            tv.Picts.ListChanged += new ListChangedEventHandler(Picts_ListChanged);

            TTLTemp = this.Text;

            tstop.Location = Point.Empty;
            tsvis.Location = new Point(0, tstop.Height);

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

        void Openf(String fp) {
            String fpOpen = fp;

            TCUt tcut = new TCUt(fp);
            if (tcut.Test() > 0) {
                if (!tcut.ConfirmTagsDisposal(this, false))
                    return;

                fpOpen = Path.GetTempFileName();
                File.Copy(fp, fpOpen, true);

                using (FileStream fs = File.Open(fpOpen, FileMode.Open, FileAccess.ReadWrite, FileShare.Read)) {
                    new TC.TIFCut().Cut(fs);
                }
            }

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

        private void tv_DragDrop(object sender, DragEventArgs e) {
            TvInsertMark iMark = tv.InsertMark;
            try {
                int iAt = iMark.Item + ((0 != (iMark.Location & (TvInsertMarkLocation.Right | TvInsertMarkLocation.Bottom))) ? 1 : 0);

                bool isCopy = 0 != (e.Effect & DragDropEffects.Copy);

                String[] alfp = e.Data.GetData(DataFormats.FileDrop) as String[];
                if (alfp != null) {
                    DialogResult res;
                    using (OpenWayForm form = new OpenWayForm(true))
                        res = form.ShowDialog();
                    if (res != DialogResult.Cancel) {
                        bool fAppend = res == DialogResult.Retry;
                        bool fInsertAfter = res == DialogResult.Yes;

                        if (!fInsertAfter) {
                            iAt = tv.Picts.Count;
                        }

                        foreach (String fp in alfp) {
                            FREE_IMAGE_FORMAT fmt = FREE_IMAGE_FORMAT.FIF_UNKNOWN;
                            FIMULTIBITMAP tif = FreeImage.OpenMultiBitmapEx(fp, ref fmt, FREE_IMAGE_LOAD_FLAGS.DEFAULT, false, true, false);
                            try {
                                int cnt = FreeImage.GetPageCount(tif);
                                for (int i = 0; i < cnt; i++) {
                                    FIBITMAP fib = FreeImage.LockPage(tif, i);
                                    try {
                                        tv.Picts.Insert(iAt, new TvPict(FreeImage.Clone(fib)));
                                        iAt++;
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
                                }
                            }
                        }
                    }
                    else {
                        for (int x = 0; x < cnt; x++) {
                            tv.Picts.Insert(iAt + x, new TvPict(CPUt.GetPic(e.Data, iSelFirst + x)));
                        }
                        if (!isCopy) {
                            e.Data.SetData("Pasted", (int)1);
                        }
                    }
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

            public bool ConfirmTagsDisposal(IWin32Window parent, bool save) {
                String s = "";
                foreach (String kw in dict.Keys) s += "- " + kw + "\n";

                if (dict.Count != 0) {
                    if (MessageBox.Show(parent, (save ? "上書き保存しますと、他のソフトウェアで追加した情報を失う可能性が有ります。" : "続行しますと、他のソフトウェアで追加した情報を安全の為に削除致します。") + "情報：\n\n" + s + "\n" + "それでも、続行しますか。", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                        return false;
                }
                return true;
            }
        }

        private void bSave_Click(object sender, EventArgs e) {
            if (Currentfp != null) {
                TCUt tcut = new TCUt(Currentfp);
                if (tcut.Test() > 0 && !tcut.ConfirmTagsDisposal(this, true))
                    return;
            }
            Savef(Currentfp);
        }

        private void Savef(String fp) {
            if (fp == null) {
                sfdPict.FileName = Currentfp;
                if (sfdPict.ShowDialog(this) != DialogResult.OK)
                    return;
                fp = sfdPict.FileName;
            }

            String fpbak = Path.ChangeExtension(fp, ".bak");
            if (File.Exists(fpbak))
                File.Delete(fpbak);
            if (File.Exists(fp))
                File.Move(fp, fpbak);

            FIMULTIBITMAP tif = FreeImage.OpenMultiBitmap(FREE_IMAGE_FORMAT.FIF_TIFF, fp, true, false, false, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            try {
                for (int x = 0; x < tv.Picts.Count; x++) {
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
                for (int x = 0; x < tv.Picts.Count; x++) {
                    FIBITMAP dib = tv.Picts[x].Picture;

                    FreeImage.AppendPage(tif, dib);
                }
                Currentfp = fp;
                Modified = false;

                if (File.Exists(fpbak))
                    File.Delete(fpbak);
            }
            finally {
                FreeImage.CloseMultiBitmap(tif, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
            }
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

            String fpTmp = Path.GetTempFileName() + ".tif";

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
                        using (OpenWayForm form = new OpenWayForm(false)) {
                            res = form.ShowDialog();
                        }
                        break;
                    }
                }

                if (false) { }
                else if (res == DialogResult.OK) // Load it
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
                else if (res == DialogResult.Retry)// Append
                    SynchronizationContext.Current.Post(delegate {
                        using (WIPPanel wip = new WIPPanel(this)) {
                            foreach (String fp in alfp) {
                                if (File.Exists(fp)) {
                                    FREE_IMAGE_FORMAT fmt = FREE_IMAGE_FORMAT.FIF_UNKNOWN;
                                    FIMULTIBITMAP tif = FreeImage.OpenMultiBitmapEx(fp, ref fmt, FREE_IMAGE_LOAD_FLAGS.DEFAULT, false, true, false);
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
    }
}