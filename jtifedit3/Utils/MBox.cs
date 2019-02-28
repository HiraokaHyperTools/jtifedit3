using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jtifedit3.Utils {
    class MBox {
        public static void Show(
            IWin32Window owner,
            string text,
            string caption,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.Information
        ) {
            MessageBox.Show(owner, text, caption, buttons, icon);
        }
    }
}
