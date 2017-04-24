namespace jtifedit3 {
    partial class TextForm {
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
            this.cbFill = new System.Windows.Forms.CheckBox();
            this.bBackColor = new System.Windows.Forms.Button();
            this.bReadTemplate = new System.Windows.Forms.Button();
            this.bSaveTemplate = new System.Windows.Forms.Button();
            this.lMetrics = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bSave = new System.Windows.Forms.Button();
            this.bForeColor = new System.Windows.Forms.Button();
            this.tbBody = new System.Windows.Forms.TextBox();
            this.bU = new System.Windows.Forms.CheckBox();
            this.bI = new System.Windows.Forms.CheckBox();
            this.bB = new System.Windows.Forms.CheckBox();
            this.cbSize = new System.Windows.Forms.ComboBox();
            this.cbFonts = new System.Windows.Forms.ComboBox();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.ofdTempl = new System.Windows.Forms.OpenFileDialog();
            this.sfdTempl = new System.Windows.Forms.SaveFileDialog();
            this.panel2 = new jtifedit3.DoubleBufferedPanel();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbFill);
            this.panel1.Controls.Add(this.bBackColor);
            this.panel1.Controls.Add(this.bReadTemplate);
            this.panel1.Controls.Add(this.bSaveTemplate);
            this.panel1.Controls.Add(this.lMetrics);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.bSave);
            this.panel1.Controls.Add(this.bForeColor);
            this.panel1.Controls.Add(this.tbBody);
            this.panel1.Controls.Add(this.bU);
            this.panel1.Controls.Add(this.bI);
            this.panel1.Controls.Add(this.bB);
            this.panel1.Controls.Add(this.cbSize);
            this.panel1.Controls.Add(this.cbFonts);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(970, 114);
            this.panel1.TabIndex = 0;
            // 
            // cbFill
            // 
            this.cbFill.AutoSize = true;
            this.cbFill.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource, "fill", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbFill.Location = new System.Drawing.Point(749, 82);
            this.cbFill.Name = "cbFill";
            this.cbFill.Size = new System.Drawing.Size(66, 25);
            this.cbFill.TabIndex = 12;
            this.cbFill.Text = "塗る";
            this.cbFill.UseVisualStyleBackColor = true;
            this.cbFill.CheckedChanged += new System.EventHandler(this.cbFill_CheckedChanged);
            // 
            // bBackColor
            // 
            this.bBackColor.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", this.bindingSource, "backColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bBackColor.Location = new System.Drawing.Point(749, 5);
            this.bBackColor.Name = "bBackColor";
            this.bBackColor.Size = new System.Drawing.Size(54, 71);
            this.bBackColor.TabIndex = 11;
            this.bBackColor.Text = "背景";
            this.bBackColor.UseVisualStyleBackColor = true;
            this.bBackColor.Click += new System.EventHandler(this.bBackColor_Click);
            // 
            // bReadTemplate
            // 
            this.bReadTemplate.Image = global::jtifedit3.Properties.Resources.openHS;
            this.bReadTemplate.Location = new System.Drawing.Point(12, 5);
            this.bReadTemplate.Name = "bReadTemplate";
            this.bReadTemplate.Size = new System.Drawing.Size(61, 102);
            this.bReadTemplate.TabIndex = 0;
            this.bReadTemplate.Text = "テンプレ\r\n読込";
            this.bReadTemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bReadTemplate.UseVisualStyleBackColor = true;
            this.bReadTemplate.Click += new System.EventHandler(this.bReadTemplate_Click);
            // 
            // bSaveTemplate
            // 
            this.bSaveTemplate.Image = global::jtifedit3.Properties.Resources.saveHS;
            this.bSaveTemplate.Location = new System.Drawing.Point(79, 5);
            this.bSaveTemplate.Name = "bSaveTemplate";
            this.bSaveTemplate.Size = new System.Drawing.Size(61, 102);
            this.bSaveTemplate.TabIndex = 1;
            this.bSaveTemplate.Text = "テンプレ\r\n保存";
            this.bSaveTemplate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bSaveTemplate.UseVisualStyleBackColor = true;
            this.bSaveTemplate.Click += new System.EventHandler(this.bSaveTemplate_Click);
            // 
            // lMetrics
            // 
            this.lMetrics.AutoSize = true;
            this.lMetrics.Location = new System.Drawing.Point(208, 86);
            this.lMetrics.Name = "lMetrics";
            this.lMetrics.Size = new System.Drawing.Size(22, 21);
            this.lMetrics.TabIndex = 8;
            this.lMetrics.Text = "...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(146, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 21);
            this.label1.TabIndex = 7;
            this.label1.Text = "寸法：";
            // 
            // bSave
            // 
            this.bSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bSave.Image = global::jtifedit3.Properties.Resources.Pen;
            this.bSave.Location = new System.Drawing.Point(830, 5);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(116, 102);
            this.bSave.TabIndex = 13;
            this.bSave.Text = "書き込んで閉じる";
            this.bSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bForeColor
            // 
            this.bForeColor.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", this.bindingSource, "foreColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bForeColor.Location = new System.Drawing.Point(689, 5);
            this.bForeColor.Name = "bForeColor";
            this.bForeColor.Size = new System.Drawing.Size(54, 102);
            this.bForeColor.TabIndex = 10;
            this.bForeColor.Text = "文字";
            this.bForeColor.UseVisualStyleBackColor = true;
            this.bForeColor.Click += new System.EventHandler(this.bForeColor_Click);
            // 
            // tbBody
            // 
            this.tbBody.AcceptsReturn = true;
            this.tbBody.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "body", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbBody.Location = new System.Drawing.Point(396, 5);
            this.tbBody.Multiline = true;
            this.tbBody.Name = "tbBody";
            this.tbBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbBody.Size = new System.Drawing.Size(287, 102);
            this.tbBody.TabIndex = 9;
            this.tbBody.Text = "テキストを入力";
            this.tbBody.TextChanged += new System.EventHandler(this.tbBody_TextChanged);
            // 
            // bU
            // 
            this.bU.AutoSize = true;
            this.bU.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource, "underline", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bU.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.bU.Location = new System.Drawing.Point(316, 37);
            this.bU.Name = "bU";
            this.bU.Size = new System.Drawing.Size(71, 25);
            this.bU.TabIndex = 6;
            this.bU.Text = "下線";
            this.bU.UseVisualStyleBackColor = true;
            this.bU.CheckedChanged += new System.EventHandler(this.bB_CheckedChanged);
            // 
            // bI
            // 
            this.bI.AutoSize = true;
            this.bI.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource, "italic", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bI.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.bI.Location = new System.Drawing.Point(239, 61);
            this.bI.Name = "bI";
            this.bI.Size = new System.Drawing.Size(71, 25);
            this.bI.TabIndex = 5;
            this.bI.Text = "斜体";
            this.bI.UseVisualStyleBackColor = true;
            this.bI.CheckedChanged += new System.EventHandler(this.bB_CheckedChanged);
            // 
            // bB
            // 
            this.bB.AutoSize = true;
            this.bB.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource, "bold", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bB.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.bB.Location = new System.Drawing.Point(239, 37);
            this.bB.Name = "bB";
            this.bB.Size = new System.Drawing.Size(73, 25);
            this.bB.TabIndex = 4;
            this.bB.Text = "太字";
            this.bB.UseVisualStyleBackColor = true;
            this.bB.CheckedChanged += new System.EventHandler(this.bB_CheckedChanged);
            // 
            // cbSize
            // 
            this.cbSize.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "fontSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbSize.FormattingEnabled = true;
            this.cbSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "20",
            "22",
            "24",
            "26",
            "28",
            "36",
            "48",
            "72",
            "100",
            "150",
            "200",
            "250",
            "300",
            "350",
            "400",
            "450",
            "500"});
            this.cbSize.Location = new System.Drawing.Point(162, 44);
            this.cbSize.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cbSize.Name = "cbSize";
            this.cbSize.Size = new System.Drawing.Size(68, 29);
            this.cbSize.TabIndex = 3;
            this.cbSize.Text = "12";
            this.cbSize.TextChanged += new System.EventHandler(this.cbSize_TextChanged);
            // 
            // cbFonts
            // 
            this.cbFonts.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "fontFamily", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbFonts.FormattingEnabled = true;
            this.cbFonts.Location = new System.Drawing.Point(162, 5);
            this.cbFonts.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cbFonts.Name = "cbFonts";
            this.cbFonts.Size = new System.Drawing.Size(225, 29);
            this.cbFonts.TabIndex = 2;
            this.cbFonts.TextChanged += new System.EventHandler(this.cbFonts_TextChanged);
            // 
            // timerUpdate
            // 
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // ofdTempl
            // 
            this.ofdTempl.DefaultExt = "xml";
            this.ofdTempl.Filter = "*.xml|*.xml";
            // 
            // sfdTempl
            // 
            this.sfdTempl.DefaultExt = "xml";
            this.sfdTempl.Filter = "*.xml|*.xml";
            // 
            // panel2
            // 
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 114);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(970, 438);
            this.panel2.TabIndex = 2;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseMove);
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseDown);
            this.panel2.Resize += new System.EventHandler(this.panel2_Resize);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel2_MouseUp);
            // 
            // bindingSource
            // 
            this.bindingSource.AllowNew = false;
            this.bindingSource.DataSource = typeof(jtifedit3.TextTemplate);
            // 
            // TextForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(970, 552);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "TextForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "文字描画";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TextForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbSize;
        private System.Windows.Forms.ComboBox cbFonts;
        private System.Windows.Forms.CheckBox bU;
        private System.Windows.Forms.CheckBox bI;
        private System.Windows.Forms.CheckBox bB;
        private System.Windows.Forms.TextBox tbBody;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.Button bForeColor;
        private DoubleBufferedPanel panel2;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lMetrics;
        private System.Windows.Forms.Button bSaveTemplate;
        private System.Windows.Forms.Button bReadTemplate;
        private System.Windows.Forms.OpenFileDialog ofdTempl;
        private System.Windows.Forms.SaveFileDialog sfdTempl;
        private System.Windows.Forms.BindingSource bindingSource;
        private System.Windows.Forms.Button bBackColor;
        private System.Windows.Forms.CheckBox cbFill;

    }
}