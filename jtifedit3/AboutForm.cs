using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using jtifedit3.Utils;
using jtifedit3.Properties;

namespace jtifedit3 {
    public partial class AboutForm : Form {
        public AboutForm() {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e) {
            tbVer.Text += Application.ProductVersion;

            foreach (var credit in Credits.List()) {
                var ll = new LinkLabel {
                    Text = $"{credit.name}\r\n{credit.license}",
                    LinkArea = new LinkArea(0, credit.name.Length),
                    Padding = new Padding(18, 0, 0, 0),
                    AutoSize = true,
                    Image = Resources.PlayHS,
                    ImageAlign = ContentAlignment.MiddleLeft,
                };
                ll.LinkClicked += llhp_LinkClicked;
                ttUrl.SetToolTip(ll, credit.url);
                flpTechs.Controls.Add(ll);
                flpTechs.SetFlowBreak(ll, true);
            }
        }

        private void llhp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try {
                Process.Start(ttUrl.GetToolTip((Control)sender));
            }
            catch (Exception) {
                MessageBox.Show(this, "表示に失敗しました。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}