using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace jtifedit3 {
    public partial class DoubleBufferPanel : UserControl {
        public DoubleBufferPanel() {
            InitializeComponent();
        }

        private void DoubleBufferPanel_Load(object sender, EventArgs e) {
            DoubleBuffered = true;
        }
    }
}
