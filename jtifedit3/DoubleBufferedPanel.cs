using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jtifedit3 {
    public class DoubleBufferedPanel : Panel {
        public DoubleBufferedPanel() {
            DoubleBuffered = true;
        }
    }
}
