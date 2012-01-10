using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace jtifedit3 {
    public partial class EdForm : Form {
        public EdForm(string fp) {
            this.fp = fp;

            InitializeComponent();
        }

        string fp = null;

        private void bReplace_Click(object sender, EventArgs e) {
            this.DialogResult = ((Button)sender).DialogResult;
            Close();
        }

        private void bRepaint_Click(object sender, EventArgs e) {
            this.DialogResult = ((Button)sender).DialogResult;
            Close();
        }

        private void bCancel_Click(object sender, EventArgs e) {
            this.DialogResult = ((Button)sender).DialogResult;
            Close();
        }

        private void EdForm_Load(object sender, EventArgs e) {

        }

        private void EdForm_Activated(object sender, EventArgs e) {
            try {
                pbPv.Image = new Bitmap(new MemoryStream(File.ReadAllBytes(fp)));
            }
            catch (Exception) {
                pbPv.Image = null;
            }
        }
    }
}