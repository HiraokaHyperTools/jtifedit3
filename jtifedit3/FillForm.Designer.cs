namespace jtifedit3 {
    partial class FillForm {
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.bSave = new System.Windows.Forms.Button();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.bFillBlack = new System.Windows.Forms.Button();
            this.bFillWhite = new System.Windows.Forms.Button();
            this.bFillWith = new System.Windows.Forms.Button();
            this.panel2 = new jtifedit3.DoubleBufferedPanel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bFillWith);
            this.panel1.Controls.Add(this.bFillBlack);
            this.panel1.Controls.Add(this.bFillWhite);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.bSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(889, 72);
            this.panel1.TabIndex = 2;
            // 
            // bSave
            // 
            this.bSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bSave.Location = new System.Drawing.Point(765, 3);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(119, 63);
            this.bSave.TabIndex = 4;
            this.bSave.Text = "書き込んで閉じる";
            this.bSave.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "マウスで範囲選択して、";
            // 
            // bFillBlack
            // 
            this.bFillBlack.Image = global::jtifedit3.Properties.Resources.FillBlack;
            this.bFillBlack.Location = new System.Drawing.Point(347, 3);
            this.bFillBlack.Name = "bFillBlack";
            this.bFillBlack.Size = new System.Drawing.Size(126, 63);
            this.bFillBlack.TabIndex = 2;
            this.bFillBlack.Text = "黒塗り";
            this.bFillBlack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bFillBlack.UseVisualStyleBackColor = true;
            this.bFillBlack.Click += new System.EventHandler(this.bFillBlack_Click);
            // 
            // bFillWhite
            // 
            this.bFillWhite.Image = global::jtifedit3.Properties.Resources.FillWhite;
            this.bFillWhite.Location = new System.Drawing.Point(215, 3);
            this.bFillWhite.Name = "bFillWhite";
            this.bFillWhite.Size = new System.Drawing.Size(126, 63);
            this.bFillWhite.TabIndex = 1;
            this.bFillWhite.Text = "白塗り";
            this.bFillWhite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bFillWhite.UseVisualStyleBackColor = true;
            this.bFillWhite.Click += new System.EventHandler(this.bFillWhite_Click);
            // 
            // bFillWith
            // 
            this.bFillWith.Image = global::jtifedit3.Properties.Resources.RecolorPictureHS;
            this.bFillWith.Location = new System.Drawing.Point(512, 3);
            this.bFillWith.Name = "bFillWith";
            this.bFillWith.Size = new System.Drawing.Size(160, 63);
            this.bFillWith.TabIndex = 3;
            this.bFillWith.Text = "指定色で塗る";
            this.bFillWith.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bFillWith.UseVisualStyleBackColor = true;
            this.bFillWith.Click += new System.EventHandler(this.bFillWith_Click);
            // 
            // panel2
            // 
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(889, 578);
            this.panel2.TabIndex = 3;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseMove);
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            this.panel2.Resize += new System.EventHandler(this.panel2_Resize);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseUp);
            // 
            // FillForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(889, 578);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "FillForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "塗りつぶし";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FillForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Timer timerUpdate;
        private DoubleBufferedPanel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bFillWhite;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button bFillBlack;
        private System.Windows.Forms.Button bFillWith;
    }
}