namespace jtifedit3 {
    partial class FR3Form {
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.numw = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numh = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.curw = new System.Windows.Forms.Label();
            this.curh = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.currc = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numh)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.currc);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.curh);
            this.panel1.Controls.Add(this.curw);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.numh);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.numw);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 194);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 92);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "幅";
            // 
            // numw
            // 
            this.numw.Location = new System.Drawing.Point(35, 3);
            this.numw.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.numw.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numw.Name = "numw";
            this.numw.Size = new System.Drawing.Size(75, 19);
            this.numw.TabIndex = 1;
            this.numw.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numw.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numw.ValueChanged += new System.EventHandler(this.numw_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "高さ";
            // 
            // numh
            // 
            this.numh.Location = new System.Drawing.Point(35, 28);
            this.numh.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.numh.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numh.Name = "numh";
            this.numh.Size = new System.Drawing.Size(75, 19);
            this.numh.TabIndex = 3;
            this.numh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numh.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numh.ValueChanged += new System.EventHandler(this.numh_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(144, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "幅：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "高さ：";
            // 
            // curw
            // 
            this.curw.AutoSize = true;
            this.curw.Location = new System.Drawing.Point(197, 5);
            this.curw.Name = "curw";
            this.curw.Size = new System.Drawing.Size(9, 12);
            this.curw.TabIndex = 6;
            this.curw.Text = "..";
            // 
            // curh
            // 
            this.curh.AutoSize = true;
            this.curh.Location = new System.Drawing.Point(197, 30);
            this.curh.Name = "curh";
            this.curh.Size = new System.Drawing.Size(9, 12);
            this.curh.TabIndex = 7;
            this.curh.Text = "..";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(292, 194);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            this.panel2.Resize += new System.EventHandler(this.panel2_Resize);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "Result";
            // 
            // currc
            // 
            this.currc.AutoSize = true;
            this.currc.Location = new System.Drawing.Point(75, 69);
            this.currc.Name = "currc";
            this.currc.Size = new System.Drawing.Size(35, 12);
            this.currc.TabIndex = 9;
            this.currc.Text = "label6";
            // 
            // FR3Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 286);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FR3Form";
            this.Text = "FR3Form";
            this.Load += new System.EventHandler(this.FR3Form_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numh)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown numh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numw;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label curh;
        private System.Windows.Forms.Label curw;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label currc;
        private System.Windows.Forms.Label label5;
    }
}