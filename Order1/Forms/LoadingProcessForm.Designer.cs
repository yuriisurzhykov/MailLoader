namespace Order1.Forms
{
    partial class LoadingProcessForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.processTextBox = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // processTextBox
            // 
            this.processTextBox.AutoSize = true;
            this.processTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.processTextBox.Location = new System.Drawing.Point(60, 76);
            this.processTextBox.Name = "processTextBox";
            this.processTextBox.Size = new System.Drawing.Size(64, 16);
            this.processTextBox.TabIndex = 14;
            this.processTextBox.Text = "Процесс";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(63, 112);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(300, 91);
            this.textBox1.TabIndex = 13;
            this.textBox1.Text = "NEXTVAL(\'s_mail\'), 12, 23\r\nNEXTVAL(\'s_mail\'), 12, 23";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MingLiU_HKSCS-ExtB", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(154, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 19);
            this.label1.TabIndex = 12;
            this.label1.Text = "Процесс импорта";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(42, 209);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(335, 32);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 11;
            this.progressBar1.Value = 53;
            // 
            // LoadingProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 283);
            this.Controls.Add(this.processTextBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Name = "LoadingProcessForm";
            this.Text = "LoadingProcessForm";
            this.Load += new System.EventHandler(this.LoadingProcessForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label processTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ProgressBar progressBar1;
    }
}