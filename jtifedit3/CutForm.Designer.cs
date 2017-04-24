namespace jtifedit3 {
    partial class CutForm {
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
            this.bCopyToFile = new System.Windows.Forms.Button();
            this.sfdPic = new System.Windows.Forms.SaveFileDialog();
            this.panel2 = new jtifedit3.DoubleBufferedPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.bCopyToFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(902, 72);
            this.panel1.TabIndex = 1;
            // 
            // bCopyToFile
            // 
            this.bCopyToFile.Image = global::jtifedit3.Properties.Resources.ExportFile_16x;
            this.bCopyToFile.Location = new System.Drawing.Point(223, 3);
            this.bCopyToFile.Name = "bCopyToFile";
            this.bCopyToFile.Size = new System.Drawing.Size(206, 63);
            this.bCopyToFile.TabIndex = 13;
            this.bCopyToFile.Text = "ファイルへ切り出し";
            this.bCopyToFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bCopyToFile.UseVisualStyleBackColor = true;
            this.bCopyToFile.Click += new System.EventHandler(this.bCopyToFile_Click);
            // 
            // sfdPic
            // 
            this.sfdPic.DefaultExt = "png";
            this.sfdPic.Filter = "*.png|*.png|*.jpg|*.jpg|*.bmp|*.bmp";
            // 
            // panel2
            // 
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 72);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(902, 413);
            this.panel2.TabIndex = 2;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseMove);
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            this.panel2.Resize += new System.EventHandler(this.panel2_Resize);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 21);
            this.label2.TabIndex = 16;
            this.label2.Text = "画像をドラッグで選択し、";
            // 
            // CutForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(902, 485);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "CutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "画像の切り出し";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.CutForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DoubleBufferedPanel panel2;
        private System.Windows.Forms.Button bCopyToFile;
        private System.Windows.Forms.SaveFileDialog sfdPic;
        private System.Windows.Forms.Label label2;
    }
}