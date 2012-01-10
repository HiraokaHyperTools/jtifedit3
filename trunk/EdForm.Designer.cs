namespace jtifedit3 {
    partial class EdForm {
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
            this.label2 = new System.Windows.Forms.Label();
            this.bReplace = new System.Windows.Forms.Button();
            this.pbPv = new System.Windows.Forms.PictureBox();
            this.bRepaint = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbPv)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "編集した画像です：";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 398);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "処理を選択してください：";
            // 
            // bReplace
            // 
            this.bReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bReplace.AutoSize = true;
            this.bReplace.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bReplace.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bReplace.Image = global::jtifedit3.Properties.Resources.saveHS1;
            this.bReplace.Location = new System.Drawing.Point(12, 413);
            this.bReplace.Name = "bReplace";
            this.bReplace.Size = new System.Drawing.Size(86, 50);
            this.bReplace.TabIndex = 2;
            this.bReplace.Text = "\r\n取り込みします";
            this.bReplace.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bReplace.UseVisualStyleBackColor = true;
            this.bReplace.Click += new System.EventHandler(this.bReplace_Click);
            // 
            // pbPv
            // 
            this.pbPv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbPv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbPv.Location = new System.Drawing.Point(12, 24);
            this.pbPv.Name = "pbPv";
            this.pbPv.Size = new System.Drawing.Size(606, 371);
            this.pbPv.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPv.TabIndex = 1;
            this.pbPv.TabStop = false;
            // 
            // bRepaint
            // 
            this.bRepaint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRepaint.AutoSize = true;
            this.bRepaint.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bRepaint.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.bRepaint.Image = global::jtifedit3.Properties.Resources.ColorHS;
            this.bRepaint.Location = new System.Drawing.Point(104, 413);
            this.bRepaint.Name = "bRepaint";
            this.bRepaint.Size = new System.Drawing.Size(160, 50);
            this.bRepaint.TabIndex = 3;
            this.bRepaint.Text = "\r\nもう一度、ペイントを起動します";
            this.bRepaint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bRepaint.UseVisualStyleBackColor = true;
            this.bRepaint.Click += new System.EventHandler(this.bRepaint_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCancel.AutoSize = true;
            this.bCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Image = global::jtifedit3.Properties.Resources.DeleteHS;
            this.bCancel.Location = new System.Drawing.Point(309, 413);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(62, 50);
            this.bCancel.TabIndex = 4;
            this.bCancel.Text = "\r\nキャンセル";
            this.bCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // EdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 475);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bRepaint);
            this.Controls.Add(this.bReplace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pbPv);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(300, 202);
            this.Name = "EdForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ペイントで編集";
            this.Load += new System.EventHandler(this.EdForm_Load);
            this.Activated += new System.EventHandler(this.EdForm_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.pbPv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbPv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bReplace;
        private System.Windows.Forms.Button bRepaint;
        private System.Windows.Forms.Button bCancel;
    }
}