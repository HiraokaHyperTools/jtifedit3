using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace jtifedit3 {
    public partial class AboutForm : Form {
        public AboutForm() {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e) {
            tbVer.Text += Application.ProductVersion;
        }

        private void llhp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            try {
                Process.Start(ttUrl.GetToolTip((Control)sender));
            }
            catch (Exception) {
                MessageBox.Show(this, "ï\é¶Ç…é∏îsÇµÇ‹ÇµÇΩÅB", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}