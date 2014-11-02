namespace WoWthing_Sync
{
    partial class SyncForm
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
            this.textLog = new System.Windows.Forms.TextBox();
            this.textFolder = new System.Windows.Forms.TextBox();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.textUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textLog
            // 
            this.textLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textLog.Location = new System.Drawing.Point(12, 105);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ReadOnly = true;
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(460, 145);
            this.textLog.TabIndex = 4;
            this.textLog.TabStop = false;
            // 
            // textFolder
            // 
            this.textFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textFolder.BackColor = System.Drawing.SystemColors.Control;
            this.textFolder.Location = new System.Drawing.Point(231, 22);
            this.textFolder.Name = "textFolder";
            this.textFolder.ReadOnly = true;
            this.textFolder.Size = new System.Drawing.Size(223, 23);
            this.textFolder.TabIndex = 1;
            this.textFolder.TabStop = false;
            // 
            // btnChooseFolder
            // 
            this.btnChooseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseFolder.Location = new System.Drawing.Point(231, 52);
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.Size = new System.Drawing.Size(223, 23);
            this.btnChooseFolder.TabIndex = 3;
            this.btnChooseFolder.Text = "Select Folder...";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.btnChooseFolder_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnChooseFolder);
            this.groupBox2.Controls.Add(this.textFolder);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textPassword);
            this.groupBox2.Controls.Add(this.textUsername);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(460, 87);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(84, 51);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(116, 23);
            this.textPassword.TabIndex = 2;
            this.textPassword.UseSystemPasswordChar = true;
            this.textPassword.TextChanged += new System.EventHandler(this.textPassword_TextChanged);
            // 
            // textUsername
            // 
            this.textUsername.Location = new System.Drawing.Point(84, 22);
            this.textUsername.Name = "textUsername";
            this.textUsername.Size = new System.Drawing.Size(116, 23);
            this.textUsername.TabIndex = 1;
            this.textUsername.TextChanged += new System.EventHandler(this.textUsername_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // SyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 262);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textLog);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "SyncForm";
            this.Text = "WoWthing Sync";
            this.Load += new System.EventHandler(this.SyncForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textLog;
        private System.Windows.Forms.TextBox textFolder;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.TextBox textUsername;
        private System.Windows.Forms.Label label1;
    }
}

