using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace TwainLib {
    public enum TwainCommand {
        Not = -1,
        Null = 0,
        TransferReady = 1,
        CloseRequest = 2,
        CloseOk = 3,
        DeviceEvent = 4
    }




    public class Twain {
        private const short CountryUSA = 1;
        private const short LanguageUSA = 13;

        public Twain() {
            appid = new TwIdentity();
            appid.Id = IntPtr.Zero;
            appid.Version.MajorNum = 1;
            appid.Version.MinorNum = 1;
            appid.Version.Language = LanguageUSA;
            appid.Version.Country = CountryUSA;
            appid.Version.Info = "Hack 1";
            appid.ProtocolMajor = TwProtocol.Major;
            appid.ProtocolMinor = TwProtocol.Minor;
            appid.SupportedGroups = (int)(TwDG.Image | TwDG.Control);
            appid.Manufacturer = "NETMaster";
            appid.ProductFamily = "Freeware";
            appid.ProductName = "Hack";

            srcds = new TwIdentity();
            srcds.Id = IntPtr.Zero;

            evtmsg.EventPtr = Marshal.AllocHGlobal(Marshal.SizeOf(winmsg));

            hwnd = (notifier = new Notifier(this)).Handle;
        }

        ~Twain() {
            Marshal.FreeHGlobal(evtmsg.EventPtr);
        }




        public void Init() {
            Finish();
            TwRC rc = DSMparent(appid, IntPtr.Zero, TwDG.Control, TwDAT.Parent, TwMSG.OpenDSM, ref hwnd);
            if (rc == TwRC.Success) {
                rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetDefault, srcds);
                if (rc == TwRC.Success) {
                }
                else {
                    rc = DSMparent(appid, IntPtr.Zero, TwDG.Control, TwDAT.Parent, TwMSG.CloseDSM, ref hwnd);
                }
            }
        }

        public void Select() {
            TwRC rc;
            CloseSrc();
            if (appid.Id == IntPtr.Zero) {
                Init();
                if (appid.Id == IntPtr.Zero)
                    return;
            }
            rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.UserSelect, srcds);
        }


        public bool Acquire() {
            TwRC rc;
            CloseSrc();
            if (appid.Id == IntPtr.Zero) {
                Init();
                if (appid.Id == IntPtr.Zero)
                    return false;
            }
            rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.OpenDS, srcds);
            if (rc != TwRC.Success)
                return false;

            notifier.Start();

            TwCapability cap = new TwCapability(TwCap.XferCount, -1);
            rc = DScap(appid, srcds, TwDG.Control, TwDAT.Capability, TwMSG.Set, cap);
            if (rc != TwRC.Success) {
                CloseSrc();
                return false;
            }

            TwUserInterface guif = new TwUserInterface();
            guif.ShowUI = 1;
            guif.ModalUI = 1;
            guif.ParentHand = hwnd;
            rc = DSuserif(appid, srcds, TwDG.Control, TwDAT.UserInterface, TwMSG.EnableDS, guif);
            if (rc != TwRC.Success) {
                CloseSrc();
                return false;
            }

            return true;
        }


        public void TransferPictures() {
            if (srcds.Id == IntPtr.Zero)
                return;

            TwRC rc;
            IntPtr hbitmap = IntPtr.Zero;
            TwPendingXfers pxfr = new TwPendingXfers();

            try {
                do {
                    pxfr.Count = 0;
                    hbitmap = IntPtr.Zero;

                    TwImageInfo iinf = new TwImageInfo();
                    rc = DSiinf(appid, srcds, TwDG.Image, TwDAT.ImageInfo, TwMSG.Get, iinf);
                    if (rc != TwRC.Success) {
                        CloseSrc();
                        return;
                    }

                    rc = DSixfer(appid, srcds, TwDG.Image, TwDAT.ImageNativeXfer, TwMSG.Get, ref hbitmap);
                    if (rc != TwRC.XferDone) {
                        CloseSrc();
                        return;
                    }

                    rc = DSpxfer(appid, srcds, TwDG.Control, TwDAT.PendingXfers, TwMSG.EndXfer, pxfr);
                    if (rc != TwRC.Success) {
                        CloseSrc();
                        return;
                    }

                    IntPtr bmpptr = GlobalLock(hbitmap);
                    BITMAPINFOHEADER bmi = new BITMAPINFOHEADER();
                    Marshal.PtrToStructure(bmpptr, bmi);

                    if (bmi.biSizeImage == 0)
                        bmi.biSizeImage = ((((bmi.biWidth * bmi.biBitCount) + 31) & ~31) >> 3) * bmi.biHeight;

                    BITMAPFILEHEADER bfh = new BITMAPFILEHEADER();
                    int cbHdr1 = Marshal.SizeOf(bfh);
                    int cbHdr2 = bmi.biSize + 4 * bmi.biClrUsed;
                    int cbBody = bmi.biSizeImage;

                    byte[] bin = new byte[cbHdr1 + cbHdr2 + cbBody];
                    Marshal.Copy(bmpptr, bin, cbHdr1, cbHdr2 + cbBody);

                    GlobalUnlock(bmpptr);

                    MemoryStream os = new MemoryStream(bin);
                    BinaryWriter wr = new BinaryWriter(os);
                    wr.Write((ushort)0x4D42);
                    wr.Write((uint)(cbHdr1 + cbHdr2 + cbBody));
                    wr.Write((ushort)0);
                    wr.Write((ushort)0);
                    wr.Write((uint)(cbHdr1 + cbHdr2));

                    os.Position = 0;

                    if (ImageAvail != null)
                        ImageAvail(this, new ImageAvailEventArgs(new Bitmap(os)));
                }
                while (pxfr.Count != 0);

            }
            finally {
                rc = DSpxfer(appid, srcds, TwDG.Control, TwDAT.PendingXfers, TwMSG.Reset, pxfr);
            }
            return;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        struct BITMAPFILEHEADER {
            public ushort bfType;
            public uint bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public uint bfOffBits;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal class BITMAPINFOHEADER {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }

        public TwainCommand PassMessage(ref Message m) {
            if (srcds.Id == IntPtr.Zero)
                return TwainCommand.Not;

            int pos = GetMessagePos();

            winmsg.hwnd = m.HWnd;
            winmsg.message = m.Msg;
            winmsg.wParam = m.WParam;
            winmsg.lParam = m.LParam;
            winmsg.time = GetMessageTime();
            winmsg.x = (short)pos;
            winmsg.y = (short)(pos >> 16);

            Marshal.StructureToPtr(winmsg, evtmsg.EventPtr, false);
            evtmsg.Message = 0;
            TwRC rc = DSevent(appid, srcds, TwDG.Control, TwDAT.Event, TwMSG.ProcessEvent, ref evtmsg);
            if (rc == TwRC.NotDSEvent)
                return TwainCommand.Not;
            if (evtmsg.Message == (short)TwMSG.XFerReady)
                return TwainCommand.TransferReady;
            if (evtmsg.Message == (short)TwMSG.CloseDSReq)
                return TwainCommand.CloseRequest;
            if (evtmsg.Message == (short)TwMSG.CloseDSOK)
                return TwainCommand.CloseOk;
            if (evtmsg.Message == (short)TwMSG.DeviceEvent)
                return TwainCommand.DeviceEvent;

            return TwainCommand.Null;
        }

        public void CloseSrc() {
            TwRC rc;
            if (srcds.Id != IntPtr.Zero) {
                TwUserInterface guif = new TwUserInterface();
                rc = DSuserif(appid, srcds, TwDG.Control, TwDAT.UserInterface, TwMSG.DisableDS, guif);
                rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.CloseDS, srcds);
            }
        }

        public void Finish() {
            TwRC rc;
            CloseSrc();
            if (appid.Id != IntPtr.Zero)
                rc = DSMparent(appid, IntPtr.Zero, TwDG.Control, TwDAT.Parent, TwMSG.CloseDSM, ref hwnd);
            appid.Id = IntPtr.Zero;
        }

        private IntPtr hwnd;
        private TwIdentity appid;
        private TwIdentity srcds;
        private TwEvent evtmsg;
        private WINMSG winmsg;



        // ------ DSM entry point DAT_ variants:
        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSMparent([In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, ref IntPtr refptr);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSMident([In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwIdentity idds);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSMstatus([In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwStatus dsmstat);


        // ------ DSM entry point DAT_ variants to DS:
        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSuserif([In, Out] TwIdentity origin, [In, Out] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, TwUserInterface guif);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSevent([In, Out] TwIdentity origin, [In, Out] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, ref TwEvent evt);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSstatus([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwStatus dsmstat);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DScap([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwCapability capa);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSiinf([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwImageInfo imginf);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSixfer([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, ref IntPtr hbitmap);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSpxfer([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwPendingXfers pxfr);


        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalAlloc(int flags, int size);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalLock(IntPtr handle);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern bool GlobalUnlock(IntPtr handle);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalFree(IntPtr handle);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int GetMessagePos();
        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int GetMessageTime();


        [DllImport("gdi32.dll", ExactSpelling = true)]
        private static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateDC(string szdriver, string szdevice, string szoutput, IntPtr devmode);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        private static extern bool DeleteDC(IntPtr hdc);




        public static int ScreenBitDepth {
            get {
                IntPtr screenDC = CreateDC("DISPLAY", null, null, IntPtr.Zero);
                int bitDepth = GetDeviceCaps(screenDC, 12);
                bitDepth *= GetDeviceCaps(screenDC, 14);
                DeleteDC(screenDC);
                return bitDepth;
            }
        }


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        internal struct WINMSG {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int x;
            public int y;
        }


        class Notifier : Form {
            Twain tw;
            bool hasDone = false;

            public Notifier(Twain tw) {
                this.tw = tw;
            }

            internal protected void Start() {
                hasDone = false;
            }

            protected void EndingScan() {
                if (tw.EndingScan != null && !hasDone) {
                    tw.EndingScan(tw, new EventArgs());
                    hasDone = true;
                }
            }

            protected override void WndProc(ref Message m) {
                TwainCommand cmd = tw.PassMessage(ref m);
                if (cmd == TwainCommand.Not || cmd == TwainCommand.Null) {
                    base.WndProc(ref m);
                    return;
                }

                switch (cmd) {
                    case TwainCommand.CloseRequest: {
                            EndingScan();
                            tw.CloseSrc();
                            break;
                        }
                    case TwainCommand.CloseOk: {
                            EndingScan();
                            tw.CloseSrc();
                            break;
                        }
                    case TwainCommand.DeviceEvent: {
                            Console.Write("");
                            break;
                        }
                    case TwainCommand.TransferReady: {
                            tw.TransferPictures();
                            EndingScan();
                            tw.CloseSrc();

                            break;
                        }
                }
            }
        }

        Notifier notifier;

        public event EventHandler<ImageAvailEventArgs> ImageAvail;
        public event EventHandler EndingScan;

    } // class Twain

    public class ImageAvailEventArgs : EventArgs {
        Bitmap _Pic;
        public Bitmap Pic { get { return _Pic; } }

        public ImageAvailEventArgs(Bitmap pic) {
            _Pic = pic;
        }
    }
}
