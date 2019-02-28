using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace jtifedit3.Utils {
    /// <summary>
    /// https://www.codeproject.com/Tips/792316/WIA-Scanner-in-Csharp-Windows-Forms
    /// </summary>
    class WIAScanner {
        const string wiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";

        public static List<Bitmap> AcquireImages(string deviceId) {
            List<Bitmap> bitmaps = new List<Bitmap>();
            WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
            using (new ComObjectAutoRelease(wiaCommonDialog)) {
                var device = FindDeviceFromDeviceId(deviceId);
                if (device != null) {
                    var items = wiaCommonDialog.ShowSelectItems(device, SingleSelect: false);
                    if (items != null) {
                        using (new ComObjectAutoRelease(items)) {
                            for (int itemIndex = 1; itemIndex <= items.Count; itemIndex++) {
                                var item = items[itemIndex];
                                using (new ComObjectAutoRelease(item)) {
                                    WIA.ImageFile imageFile = (WIA.ImageFile)wiaCommonDialog.ShowTransfer(item, wiaFormatBMP);
                                    using (new ComObjectAutoRelease(imageFile)) {
                                        // save to temp file
                                        string fileName = Path.GetTempFileName();
                                        File.Delete(fileName);
                                        imageFile.SaveFile(fileName);
                                        // add file to output list
                                        bitmaps.Add(new Bitmap(fileName));
                                    }
                                }
                            }
                        }
                    }
                }
                return bitmaps;
            }
        }

        private static WIA.Device FindDeviceFromDeviceId(string deviceId) {
            WIA.DeviceManager manager = new WIA.DeviceManager();
            using (new ComObjectAutoRelease(manager)) {
                foreach (WIA.DeviceInfo info in manager.DeviceInfos) {
                    using (new ComObjectAutoRelease(info)) {
                        if (info.DeviceID == deviceId) {
                            // connect to scanner
                            return info.Connect();
                        }
                    }
                }
                return null;
            }
        }

        public static string SelectDevice() {
            WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
            using (new ComObjectAutoRelease(wiaCommonDialog)) {
                var device = wiaCommonDialog.ShowSelectDevice(WIA.WiaDeviceType.UnspecifiedDeviceType, true, false);
                if (device != null) {
                    using (new ComObjectAutoRelease(device)) {
                        return device.DeviceID;
                    }
                }
                return null;
            }
        }

        class ComObjectAutoRelease : IDisposable {
            private readonly object it;

            public ComObjectAutoRelease(object it) {
                this.it = it;
            }

            public void Dispose() {
                Marshal.ReleaseComObject(it);
            }
        }
    }
}
