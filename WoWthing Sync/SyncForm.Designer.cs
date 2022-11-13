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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncForm));
            this.textLog = new System.Windows.Forms.TextBox();
            this.textFolder = new System.Windows.Forms.TextBox();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkStartOnStartup = new System.Windows.Forms.CheckBox();
            this.checkStartMinimized = new System.Windows.Forms.CheckBox();
            this.textApiKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnManualUpload = new System.Windows.Forms.Button();
            this.textStatus = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textLog
            // 
            this.textLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textLog.Location = new System.Drawing.Point(10, 198);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ReadOnly = true;
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(393, 186);
            this.textLog.TabIndex = 4;
            this.textLog.TabStop = false;
            // 
            // textFolder
            // 
            this.textFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textFolder.BackColor = System.Drawing.SystemColors.Control;
            this.textFolder.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textFolder.Location = new System.Drawing.Point(122, 20);
            this.textFolder.Name = "textFolder";
            this.textFolder.ReadOnly = true;
            this.textFolder.Size = new System.Drawing.Size(263, 25);
            this.textFolder.TabIndex = 1;
            this.textFolder.TabStop = false;
            // 
            // btnChooseFolder
            // 
            this.btnChooseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseFolder.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseFolder.Location = new System.Drawing.Point(5, 19);
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.Size = new System.Drawing.Size(109, 26);
            this.btnChooseFolder.TabIndex = 3;
            this.btnChooseFolder.Text = "Select Folder...";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.btnChooseFolder_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkStartOnStartup);
            this.groupBox2.Controls.Add(this.checkStartMinimized);
            this.groupBox2.Controls.Add(this.btnChooseFolder);
            this.groupBox2.Controls.Add(this.textFolder);
            this.groupBox2.Controls.Add(this.textApiKey);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(10, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(394, 123);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // checkStartOnStartup
            // 
            this.checkStartOnStartup.AutoSize = true;
            this.checkStartOnStartup.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkStartOnStartup.Location = new System.Drawing.Point(204, 91);
            this.checkStartOnStartup.Name = "checkStartOnStartup";
            this.checkStartOnStartup.Size = new System.Drawing.Size(181, 21);
            this.checkStartOnStartup.TabIndex = 5;
            this.checkStartOnStartup.Text = "Start when Windows starts";
            this.checkStartOnStartup.UseVisualStyleBackColor = true;
            this.checkStartOnStartup.CheckedChanged += new System.EventHandler(this.checkStartOnStartup_CheckedChanged);
            // 
            // checkStartMinimized
            // 
            this.checkStartMinimized.AutoSize = true;
            this.checkStartMinimized.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkStartMinimized.Location = new System.Drawing.Point(66, 91);
            this.checkStartMinimized.Name = "checkStartMinimized";
            this.checkStartMinimized.Size = new System.Drawing.Size(117, 21);
            this.checkStartMinimized.TabIndex = 4;
            this.checkStartMinimized.Text = "Start minimized";
            this.checkStartMinimized.UseVisualStyleBackColor = true;
            this.checkStartMinimized.CheckedChanged += new System.EventHandler(this.checkStartMinimized_CheckedChanged);
            // 
            // textApiKey
            // 
            this.textApiKey.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textApiKey.Location = new System.Drawing.Point(122, 50);
            this.textApiKey.Name = "textApiKey";
            this.textApiKey.PasswordChar = '*';
            this.textApiKey.Size = new System.Drawing.Size(263, 25);
            this.textApiKey.TabIndex = 1;
            this.textApiKey.TextChanged += new System.EventHandler(this.textApiKey_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(63, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "API Key";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnManualUpload);
            this.groupBox1.Controls.Add(this.textStatus);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Location = new System.Drawing.Point(10, 139);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(393, 53);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // btnManualUpload
            // 
            this.btnManualUpload.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManualUpload.Location = new System.Drawing.Point(260, 19);
            this.btnManualUpload.Name = "btnManualUpload";
            this.btnManualUpload.Size = new System.Drawing.Size(127, 26);
            this.btnManualUpload.TabIndex = 1;
            this.btnManualUpload.Text = "Manual Upload";
            this.btnManualUpload.UseVisualStyleBackColor = true;
            this.btnManualUpload.Click += new System.EventHandler(this.btnManualUpload_Click);
            // 
            // textStatus
            // 
            this.textStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textStatus.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textStatus.Location = new System.Drawing.Point(6, 19);
            this.textStatus.Name = "textStatus";
            this.textStatus.ReadOnly = true;
            this.textStatus.Size = new System.Drawing.Size(70, 26);
            this.textStatus.TabIndex = 0;
            this.textStatus.Text = "PAUSED";
            this.textStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(82, 19);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(89, 26);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipText = "WoWthing Sync has minimised to the system tray";
            this.notifyIcon.Icon = Properties.Resources.PausedIcon;
            this.notifyIcon.Text = "WoWthing Sync";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // SyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 394);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textLog);
            this.Icon = Properties.Resources.PausedIcon;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(431, 872);
            this.MinimumSize = new System.Drawing.Size(431, 352);
            this.Name = "SyncForm";
            this.Text = "WoWthing Sync";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SyncForm_FormClosing);
            this.Load += new System.EventHandler(this.SyncForm_Load);
            this.Resize += new System.EventHandler(this.SyncForm_Resize);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textLog;
        private System.Windows.Forms.TextBox textFolder;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textApiKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox textStatus;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnManualUpload;
        private System.Windows.Forms.CheckBox checkStartOnStartup;
        private System.Windows.Forms.CheckBox checkStartMinimized;
    }
}

