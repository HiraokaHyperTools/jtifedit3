namespace jtifedit3 {
    partial class AboutForm {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.bClose = new System.Windows.Forms.Button();
            this.llhp = new System.Windows.Forms.LinkLabel();
            this.ttUrl = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flpTechs = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbVer = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // bClose
            // 
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bClose.Location = new System.Drawing.Point(12, 369);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(150, 23);
            this.bClose.TabIndex = 4;
            this.bClose.Text = "閉じる";
            this.bClose.UseVisualStyleBackColor = true;
            // 
            // llhp
            // 
            this.llhp.AutoSize = true;
            this.llhp.LinkArea = new System.Windows.Forms.LinkArea(21, 10);
            this.llhp.Location = new System.Drawing.Point(12, 59);
            this.llhp.Name = "llhp";
            this.llhp.Size = new System.Drawing.Size(428, 17);
            this.llhp.TabIndex = 2;
            this.llhp.TabStop = true;
            this.llhp.Text = "オープンソースプロジェクトへの貢献として、枚岡合金工具株式会社が開発いたしました。";
            this.ttUrl.SetToolTip(this.llhp, "http://www.digitaldolphins.jp");
            this.llhp.UseCompatibleTextRendering = true;
            this.llhp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llhp_LinkClicked);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flpTechs);
            this.groupBox1.Location = new System.Drawing.Point(12, 79);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 284);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "利用技術をご紹介いたします：";
            // 
            // flpTechs
            // 
            this.flpTechs.AutoScroll = true;
            this.flpTechs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTechs.Location = new System.Drawing.Point(3, 15);
            this.flpTechs.Name = "flpTechs";
            this.flpTechs.Size = new System.Drawing.Size(416, 266);
            this.flpTechs.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::jtifedit3.Properties.Resources.App;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // tbVer
            // 
            this.tbVer.BackColor = System.Drawing.SystemColors.Control;
            this.tbVer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbVer.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tbVer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbVer.Location = new System.Drawing.Point(50, 32);
            this.tbVer.Name = "tbVer";
            this.tbVer.Size = new System.Drawing.Size(381, 12);
            this.tbVer.TabIndex = 9;
            this.tbVer.Text = "J TIFF Editor 3  Version ";
            // 
            // AboutForm
            // 
            this.AcceptButton = this.bClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bClose;
            this.ClientSize = new System.Drawing.Size(446, 404);
            this.Controls.Add(this.tbVer);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.llhp);
            this.Controls.Add(this.bClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "J TIFF Editor 3 について";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.LinkLabel llhp;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolTip ttUrl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flpTechs;
        private System.Windows.Forms.TextBox tbVer;
    }
}