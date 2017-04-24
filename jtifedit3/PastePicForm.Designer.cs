namespace jtifedit3 {
    partial class PastePicForm {
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
            this.bLarger = new System.Windows.Forms.Button();
            this.bSmaller = new System.Windows.Forms.Button();
            this.panel2 = new jtifedit3.DoubleBufferedPanel();
            this.b25 = new System.Windows.Forms.Button();
            this.b50 = new System.Windows.Forms.Button();
            this.b100 = new System.Windows.Forms.Button();
            this.b150 = new System.Windows.Forms.Button();
            this.b200 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.b200);
            this.panel1.Controls.Add(this.b150);
            this.panel1.Controls.Add(this.b100);
            this.panel1.Controls.Add(this.b50);
            this.panel1.Controls.Add(this.b25);
            this.panel1.Controls.Add(this.bSmaller);
            this.panel1.Controls.Add(this.bLarger);
            this.panel1.Controls.Add(this.bSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(992, 72);
            this.panel1.TabIndex = 0;
            // 
            // bSave
            // 
            this.bSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bSave.Location = new System.Drawing.Point(765, 3);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(119, 63);
            this.bSave.TabIndex = 12;
            this.bSave.Text = "書き込んで閉じる";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // timerUpdate
            // 
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // bLarger
            // 
            this.bLarger.Location = new System.Drawing.Point(3, 3);
            this.bLarger.Name = "bLarger";
            this.bLarger.Size = new System.Drawing.Size(86, 63);
            this.bLarger.TabIndex = 13;
            this.bLarger.Text = "拡大";
            this.bLarger.UseVisualStyleBackColor = true;
            this.bLarger.Click += new System.EventHandler(this.bLarger_Click);
            // 
            // bSmaller
            // 
            this.bSmaller.Location = new System.Drawing.Point(95, 3);
            this.bSmaller.Name = "bSmaller";
            this.bSmaller.Size = new System.Drawing.Size(86, 63);
            this.bSmaller.TabIndex = 14;
            this.bSmaller.Text = "縮小";
            this.bSmaller.UseVisualStyleBackColor = true;
            this.bSmaller.Click += new System.EventHandler(this.bSmaller_Click);
            // 
            // panel2
            // 
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 72);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(992, 490);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseMove);
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            this.panel2.Resize += new System.EventHandler(this.panel2_Resize);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseUp);
            // 
            // b25
            // 
            this.b25.Location = new System.Drawing.Point(221, 3);
            this.b25.Name = "b25";
            this.b25.Size = new System.Drawing.Size(53, 63);
            this.b25.TabIndex = 15;
            this.b25.Text = "25%";
            this.b25.UseVisualStyleBackColor = true;
            this.b25.Click += new System.EventHandler(this.b100_Click);
            // 
            // b50
            // 
            this.b50.Location = new System.Drawing.Point(280, 3);
            this.b50.Name = "b50";
            this.b50.Size = new System.Drawing.Size(53, 63);
            this.b50.TabIndex = 16;
            this.b50.Text = "50%";
            this.b50.UseVisualStyleBackColor = true;
            this.b50.Click += new System.EventHandler(this.b100_Click);
            // 
            // b100
            // 
            this.b100.Location = new System.Drawing.Point(339, 3);
            this.b100.Name = "b100";
            this.b100.Size = new System.Drawing.Size(64, 63);
            this.b100.TabIndex = 17;
            this.b100.Text = "100%";
            this.b100.UseVisualStyleBackColor = true;
            this.b100.Click += new System.EventHandler(this.b100_Click);
            // 
            // b150
            // 
            this.b150.Location = new System.Drawing.Point(409, 3);
            this.b150.Name = "b150";
            this.b150.Size = new System.Drawing.Size(64, 63);
            this.b150.TabIndex = 18;
            this.b150.Text = "150%";
            this.b150.UseVisualStyleBackColor = true;
            this.b150.Click += new System.EventHandler(this.b100_Click);
            // 
            // b200
            // 
            this.b200.Location = new System.Drawing.Point(479, 3);
            this.b200.Name = "b200";
            this.b200.Size = new System.Drawing.Size(64, 63);
            this.b200.TabIndex = 19;
            this.b200.Text = "200%";
            this.b200.UseVisualStyleBackColor = true;
            this.b200.Click += new System.EventHandler(this.b100_Click);
            // 
            // PastePicForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(992, 562);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "PastePicForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "画像を貼り付け";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PastePicForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DoubleBufferedPanel panel2;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bSmaller;
        private System.Windows.Forms.Button bLarger;
        private System.Windows.Forms.Button b25;
        private System.Windows.Forms.Button b100;
        private System.Windows.Forms.Button b50;
        private System.Windows.Forms.Button b200;
        private System.Windows.Forms.Button b150;
    }
}