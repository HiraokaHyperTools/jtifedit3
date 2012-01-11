using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using jtifedit3.Properties;
using System.Drawing.Printing;

namespace jtifedit3 {
    public partial class PageSetForm : Form {
        public PageSetForm() {
            InitializeComponent();
        }

        Margins MM0 { get { return new Margins(0, 0, 0, 0); } }
        Margins MM5 { get { return new Margins(20, 20, 20, 20); } }
        Margins MM10 { get { return new Margins(39, 39, 39, 39); } }

        private void PageSetForm_Load(object sender, EventArgs e) {
            {
                List<KeyValuePair<string, Margins>> al = new List<KeyValuePair<string, Margins>>();
                al.Add(new KeyValuePair<string, Margins>("0mm", MM0));
                al.Add(new KeyValuePair<string, Margins>("5mm", MM5));
                al.Add(new KeyValuePair<string, Margins>("10mm", MM10));

                cbMargins.ValueMember = "Value";
                cbMargins.DisplayMember = "Key";
                cbMargins.DataSource = al;

                cbMargins.SelectedValue = Settings.Default.PrintMargins;

                if (cbMargins.SelectedValue == null)
                    cbMargins.Text = (string)new MarginsConverter().ConvertTo(Settings.Default.PrintMargins, typeof(string));
            }

            {
                List<KeyValuePair<string, PaperKind>> al = new List<KeyValuePair<string, PaperKind>>();
                al.Add(new KeyValuePair<string, PaperKind>("ç¨ç⁄ÇµÇƒàÛç¸", PaperKind.Custom));
                al.Add(new KeyValuePair<string, PaperKind>("A4Ç…ìùàÍ", PaperKind.A4));
                al.Add(new KeyValuePair<string, PaperKind>("B4Ç…ìùàÍ", PaperKind.B4));
                al.Add(new KeyValuePair<string, PaperKind>("A3Ç…ìùàÍ", PaperKind.A3));

                cbMix.ValueMember = "Value";
                cbMix.DisplayMember = "Key";
                cbMix.DataSource = al;

                cbMix.SelectedValue = Settings.Default.PrintPaper;

                if (cbMix.SelectedValue == null)
                    cbMix.SelectedValue = PaperKind.Custom;
            }
        }

        private void bOk_Click(object sender, EventArgs e) {
            Settings.Default.PrintMargins = (cbMargins.SelectedValue == null)
                ? (Margins)new MarginsConverter().ConvertFrom(cbMargins.Text)
                : (Margins)cbMargins.SelectedValue;

            Settings.Default.PrintPaper = (PaperKind)cbMix.SelectedValue;

            Settings.Default.Save();

            Close();
        }
    }
}