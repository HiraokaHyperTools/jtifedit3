using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace jtifedit3 {
    static class Program {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(String[] args) {
            String fp = null;
            foreach (String a in args) {
                if (fp == null && File.Exists(a)) {
                    fp = a;
                }
            }

            Environment.SetEnvironmentVariable(
                "PATH",
                Environment.GetEnvironmentVariable("PATH").TrimEnd(';')
                + ";"
                + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, (IntPtr.Size == 4) ? "x32" : "x64")
                );

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new PageSetForm());
            Application.Run(new JForm(fp));
            //Application.Run(new FR3Form());
        }

        public static String TemplateDir {
            get {
                String dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "jtifedit3\\templates");
                Directory.CreateDirectory(dir);
                return dir;
            }
        }
    }
}