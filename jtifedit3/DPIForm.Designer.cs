namespace jtifedit3 {
    partial class DPIForm {
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
            this.bForceDPI = new System.Windows.Forms.Button();
            this.numry = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numrx = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pbOrgPixel = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pbOrgPaper = new System.Windows.Forms.PictureBox();
            this.pbNewPaper = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pbNewPixel = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tlp = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bSameDPI = new System.Windows.Forms.Button();
            this.cbNewDPI = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numrx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOrgPixel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOrgPaper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewPaper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewPixel)).BeginInit();
            this.tlp.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bForceDPI
            // 
            this.bForceDPI.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bForceDPI.Location = new System.Drawing.Point(6, 72);
            this.bForceDPI.Name = "bForceDPI";
            this.bForceDPI.Size = new System.Drawing.Size(75, 46);
            this.bForceDPI.TabIndex = 4;
            this.bForceDPI.Text = "設定";
            this.bForceDPI.UseVisualStyleBackColor = true;
            this.bForceDPI.Click += new System.EventHandler(this.bSameDPI_Click);
            // 
            // numry
            // 
            this.numry.Location = new System.Drawing.Point(81, 18);
            this.numry.Maximum = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            this.numry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numry.Name = "numry";
            this.numry.Size = new System.Drawing.Size(68, 19);
            this.numry.TabIndex = 3;
            this.numry.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numry.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numry.ValueChanged += new System.EventHandler(this.numrx_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "縦";
            // 
            // numrx
            // 
            this.numrx.Location = new System.Drawing.Point(8, 18);
            this.numrx.Maximum = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            this.numrx.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numrx.Name = "numrx";
            this.numrx.Size = new System.Drawing.Size(68, 19);
            this.numrx.TabIndex = 1;
            this.numrx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numrx.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numrx.ValueChanged += new System.EventHandler(this.numrx_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "横";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(120, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "現状";
            // 
            // pbOrgPixel
            // 
            this.pbOrgPixel.Location = new System.Drawing.Point(60, 15);
            this.pbOrgPixel.Name = "pbOrgPixel";
            this.pbOrgPixel.Size = new System.Drawing.Size(150, 150);
            this.pbOrgPixel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbOrgPixel.TabIndex = 5;
            this.pbOrgPixel.TabStop = false;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "画像の形";
            // 
            // pbOrgPaper
            // 
            this.pbOrgPaper.Location = new System.Drawing.Point(60, 171);
            this.pbOrgPaper.Name = "pbOrgPaper";
            this.pbOrgPaper.Size = new System.Drawing.Size(150, 150);
            this.pbOrgPaper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbOrgPaper.TabIndex = 7;
            this.pbOrgPaper.TabStop = false;
            // 
            // pbNewPaper
            // 
            this.pbNewPaper.Location = new System.Drawing.Point(236, 171);
            this.pbNewPaper.Name = "pbNewPaper";
            this.pbNewPaper.Size = new System.Drawing.Size(150, 150);
            this.pbNewPaper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbNewPaper.TabIndex = 9;
            this.pbNewPaper.TabStop = false;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 240);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "用紙の形";
            // 
            // pbNewPixel
            // 
            this.pbNewPixel.Location = new System.Drawing.Point(236, 15);
            this.pbNewPixel.Name = "pbNewPixel";
            this.pbNewPixel.Size = new System.Drawing.Size(150, 150);
            this.pbNewPixel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbNewPixel.TabIndex = 12;
            this.pbNewPixel.TabStop = false;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(281, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 12);
            this.label11.TabIndex = 11;
            this.label11.Text = "変形した後";
            // 
            // tlp
            // 
            this.tlp.AutoSize = true;
            this.tlp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlp.ColumnCount = 4;
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlp.Controls.Add(this.label6, 1, 0);
            this.tlp.Controls.Add(this.pbNewPixel, 3, 1);
            this.tlp.Controls.Add(this.label8, 0, 2);
            this.tlp.Controls.Add(this.pbNewPaper, 3, 2);
            this.tlp.Controls.Add(this.label7, 0, 1);
            this.tlp.Controls.Add(this.label11, 3, 0);
            this.tlp.Controls.Add(this.pbOrgPixel, 1, 1);
            this.tlp.Controls.Add(this.pbOrgPaper, 1, 2);
            this.tlp.Controls.Add(this.label3, 2, 1);
            this.tlp.Controls.Add(this.label4, 2, 2);
            this.tlp.Location = new System.Drawing.Point(12, 12);
            this.tlp.Name = "tlp";
            this.tlp.RowCount = 3;
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlp.Size = new System.Drawing.Size(389, 324);
            this.tlp.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "⇒";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(216, 240);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "⇒";
            // 
            // bSameDPI
            // 
            this.bSameDPI.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bSameDPI.Location = new System.Drawing.Point(6, 72);
            this.bSameDPI.Name = "bSameDPI";
            this.bSameDPI.Size = new System.Drawing.Size(75, 46);
            this.bSameDPI.TabIndex = 5;
            this.bSameDPI.Text = "設定";
            this.bSameDPI.UseVisualStyleBackColor = true;
            this.bSameDPI.Click += new System.EventHandler(this.bSameDPI_Click);
            // 
            // cbNewDPI
            // 
            this.cbNewDPI.FormattingEnabled = true;
            this.cbNewDPI.Items.AddRange(new object[] {
            "200",
            "300",
            "600"});
            this.cbNewDPI.Location = new System.Drawing.Point(8, 18);
            this.cbNewDPI.Name = "cbNewDPI";
            this.cbNewDPI.Size = new System.Drawing.Size(70, 20);
            this.cbNewDPI.TabIndex = 1;
            this.cbNewDPI.TextChanged += new System.EventHandler(this.cbNewDPI_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "新しいDPI数：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 350);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(389, 150);
            this.tabControl1.TabIndex = 15;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.bForceDPI);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.numry);
            this.tabPage1.Controls.Add(this.numrx);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(381, 124);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DPI数だけを変更したい";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.bSameDPI);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.cbNewDPI);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(389, 124);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "縦横のDPIが等しい画像にする";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(420, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 120);
            this.label5.TabIndex = 16;
            this.label5.Text = "参考サイズ(mm)：\r\nA0=841×1189\r\nA1=594×841\r\nA2=420×594\r\nA3=297×420\r\nA4=210×297\r\nA5=148×2" +
                "10\r\n\r\nJIS B4=257×364\r\nJIS B5=182×257";
            // 
            // DPIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 507);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tlp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DPIForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DPI変更";
            this.Load += new System.EventHandler(this.DPIForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numrx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOrgPixel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOrgPaper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewPaper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewPixel)).EndInit();
            this.tlp.ResumeLayout(false);
            this.tlp.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown numry;
        public System.Windows.Forms.NumericUpDown numrx;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pbOrgPixel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pbOrgPaper;
        private System.Windows.Forms.PictureBox pbNewPaper;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pbNewPixel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tlp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button bForceDPI;
        private System.Windows.Forms.Button bSameDPI;
        public System.Windows.Forms.ComboBox cbNewDPI;
        public System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
    }
}