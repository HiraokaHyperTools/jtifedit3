namespace jtifedit3 {
    partial class LeanForm {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.lCur = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.bReplace = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new jtifedit3.DoubleBufferPanel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "図中の線をマウスでドラッグして、傾き状態を指定してください。";
            // 
            // lCur
            // 
            this.lCur.AutoSize = true;
            this.lCur.Location = new System.Drawing.Point(360, 9);
            this.lCur.Name = "lCur";
            this.lCur.Size = new System.Drawing.Size(11, 12);
            this.lCur.TabIndex = 2;
            this.lCur.Text = "...";
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCancel.AutoSize = true;
            this.bCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Image = global::jtifedit3.Properties.Resources.DeleteHS;
            this.bCancel.Location = new System.Drawing.Point(309, 470);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(62, 50);
            this.bCancel.TabIndex = 7;
            this.bCancel.Text = "\r\nキャンセル";
            this.bCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bReplace
            // 
            this.bReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bReplace.AutoSize = true;
            this.bReplace.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bReplace.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bReplace.Image = global::jtifedit3.Properties.Resources.saveHS1;
            this.bReplace.Location = new System.Drawing.Point(12, 470);
            this.bReplace.Name = "bReplace";
            this.bReplace.Size = new System.Drawing.Size(86, 50);
            this.bReplace.TabIndex = 6;
            this.bReplace.Text = "\r\n取り込みします";
            this.bReplace.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bReplace.UseVisualStyleBackColor = true;
            this.bReplace.Click += new System.EventHandler(this.bReplace_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 455);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "処理を選択してください：";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(12, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(608, 414);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.pb_Paint);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pb_MouseMove);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pb_MouseDown);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pb_MouseUp);
            // 
            // LeanForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(632, 532);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bReplace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lCur);
            this.Controls.Add(this.label1);
            this.Name = "LeanForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "傾き補正";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.LeanForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LeanForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lCur;
        private DoubleBufferPanel panel1;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bReplace;
        private System.Windows.Forms.Label label2;

    }
}