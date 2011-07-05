using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace jtifedit3 {
    public partial class FR3Form : Form {
        public FR3Form() {
            InitializeComponent();
        }

        private void numw_ValueChanged(object sender, EventArgs e) {
            Recalc();
        }

        private void numh_ValueChanged(object sender, EventArgs e) {
            Recalc();
        }

        Rectangle rc = Rectangle.Empty;

        private void Recalc() {
            rc = FitRect3.Fit(panel2.ClientRectangle, new Size((int)numw.Value, (int)numh.Value));
            panel2.Invalidate();

            Size siz = panel2.ClientSize;
            curw.Text = siz.Width.ToString();
            curh.Text = siz.Height.ToString();

            currc.Text = rc.ToString();
        }

        private void panel2_Resize(object sender, EventArgs e) {
            Recalc();
        }

        private void FR3Form_Load(object sender, EventArgs e) {
            Recalc();
        }

        private void panel2_Paint(object sender, PaintEventArgs e) {
            using (HatchBrush br = new HatchBrush(HatchStyle.Weave, Color.Blue, Color.Transparent))
                e.Graphics.FillRectangle(br, rc);
            e.Graphics.DrawRectangle(Pens.Blue, rc);
        }
    }
}