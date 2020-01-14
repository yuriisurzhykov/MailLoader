namespace Order1
{
    partial class CreateConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateConnection));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.password_box = new System.Windows.Forms.TextBox();
            this.db_name = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.user_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.server_box = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // password_box
            // 
            resources.ApplyResources(this.password_box, "password_box");
            this.password_box.Name = "password_box";
            // 
            // db_name
            // 
            resources.ApplyResources(this.db_name, "db_name");
            this.db_name.Name = "db_name";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // user_name
            // 
            resources.ApplyResources(this.user_name, "user_name");
            this.user_name.Name = "user_name";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // server_box
            // 
            resources.ApplyResources(this.server_box, "server_box");
            this.server_box.Name = "server_box";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // CreateConnection
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.server_box);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.user_name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.db_name);
            this.Controls.Add(this.password_box);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CreateConnection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox password_box;
        private System.Windows.Forms.TextBox db_name;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox user_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox server_box;
        private System.Windows.Forms.Label label4;
    }
}