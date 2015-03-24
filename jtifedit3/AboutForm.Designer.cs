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
            this.llvs = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.llfx = new System.Windows.Forms.LinkLabel();
            this.llfi = new System.Windows.Forms.LinkLabel();
            this.llnsis = new System.Windows.Forms.LinkLabel();
            this.tbVer = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.flpTechs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // bClose
            // 
            this.bClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bClose.Location = new System.Drawing.Point(12, 251);
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
            this.groupBox1.Size = new System.Drawing.Size(422, 166);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "利用技術をご紹介いたします：";
            // 
            // flpTechs
            // 
            this.flpTechs.Controls.Add(this.llvs);
            this.flpTechs.Controls.Add(this.llfx);
            this.flpTechs.Controls.Add(this.llfi);
            this.flpTechs.Controls.Add(this.llnsis);
            this.flpTechs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTechs.Location = new System.Drawing.Point(3, 15);
            this.flpTechs.Name = "flpTechs";
            this.flpTechs.Size = new System.Drawing.Size(416, 148);
            this.flpTechs.TabIndex = 0;
            // 
            // llvs
            // 
            this.llvs.AutoSize = true;
            this.llvs.Image = global::jtifedit3.Properties.Resources.PlayHS;
            this.llvs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.llvs.LinkArea = new System.Windows.Forms.LinkArea(0, 28);
            this.llvs.Location = new System.Drawing.Point(3, 0);
            this.llvs.Name = "llvs";
            this.llvs.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.llvs.Size = new System.Drawing.Size(311, 29);
            this.llvs.TabIndex = 0;
            this.llvs.TabStop = true;
            this.llvs.Text = "Microsoft Visual Studio 2005を使用して開発しています。\r\nWYSIWYG手法による迅速な開発が期待できます。";
            this.ttUrl.SetToolTip(this.llvs, "http://www.microsoft.com/japan/msdn/vstudio/2005/");
            this.llvs.UseCompatibleTextRendering = true;
            this.llvs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llhp_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::jtifedit3.Properties.Resources._1;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // llfx
            // 
            this.llfx.AutoSize = true;
            this.llfx.Image = global::jtifedit3.Properties.Resources.PlayHS;
            this.llfx.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.llfx.LinkArea = new System.Windows.Forms.LinkArea(0, 36);
            this.llfx.Location = new System.Drawing.Point(3, 29);
            this.llfx.Name = "llfx";
            this.llfx.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.llfx.Size = new System.Drawing.Size(347, 29);
            this.llfx.TabIndex = 1;
            this.llfx.TabStop = true;
            this.llfx.Text = "Microsoft .NET Framework Version 2.0を使用しています。\r\nランタイムライブラリの支援により、開発工数の削減が期待できます。";
            this.ttUrl.SetToolTip(this.llfx, "http://www.microsoft.com/downloads/ja-jp/details.aspx?FamilyID=0856eacb-4362-4b0d" +
                    "-8edd-aab15c5e04f5");
            this.llfx.UseCompatibleTextRendering = true;
            this.llfx.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llhp_LinkClicked);
            // 
            // llfi
            // 
            this.llfi.AutoSize = true;
            this.llfi.Image = global::jtifedit3.Properties.Resources.PlayHS;
            this.llfi.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.llfi.LinkArea = new System.Windows.Forms.LinkArea(0, 17);
            this.llfi.Location = new System.Drawing.Point(3, 58);
            this.llfi.Name = "llfi";
            this.llfi.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.llfi.Size = new System.Drawing.Size(273, 29);
            this.llfi.TabIndex = 2;
            this.llfi.TabStop = true;
            this.llfi.Text = "FreeImage libraryを使用しています。\r\nTIF/TIFFファイルの操作に利用させて頂いています。";
            this.ttUrl.SetToolTip(this.llfi, "http://freeimage.sourceforge.net/");
            this.llfi.UseCompatibleTextRendering = true;
            this.llfi.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llhp_LinkClicked);
            // 
            // llnsis
            // 
            this.llnsis.AutoSize = true;
            this.llnsis.Image = global::jtifedit3.Properties.Resources.PlayHS;
            this.llnsis.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.llnsis.LinkArea = new System.Windows.Forms.LinkArea(0, 41);
            this.llnsis.Location = new System.Drawing.Point(3, 87);
            this.llnsis.Name = "llnsis";
            this.llnsis.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.llnsis.Size = new System.Drawing.Size(328, 29);
            this.llnsis.TabIndex = 3;
            this.llnsis.TabStop = true;
            this.llnsis.Text = "NSIS (Nullsoft Scriptable Install System)を使用しています。\r\nセットアップの作成・運用に利用させて頂いています。";
            this.ttUrl.SetToolTip(this.llnsis, "http://nsis.sourceforge.net/Main_Page");
            this.llnsis.UseCompatibleTextRendering = true;
            this.llnsis.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llhp_LinkClicked);
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
            this.ClientSize = new System.Drawing.Size(446, 286);
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
            this.flpTechs.ResumeLayout(false);
            this.flpTechs.PerformLayout();
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
        private System.Windows.Forms.LinkLabel llvs;
        private System.Windows.Forms.LinkLabel llfx;
        private System.Windows.Forms.LinkLabel llfi;
        private System.Windows.Forms.LinkLabel llnsis;
        private System.Windows.Forms.TextBox tbVer;
    }
}