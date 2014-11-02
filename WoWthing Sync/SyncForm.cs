using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WoWthing_Sync
{
    public partial class SyncForm : Form
    {
        private const string DEFAULT_HOST = "https://www.wowthing.org";

        private bool loaded = false;
        private INI ini;
        private string username, password, folder, host;

        public SyncForm()
        {
            InitializeComponent();
        }
        
        public void LogText(string text)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            textLog.Text = textLog.Text + "[" + timestamp + "] " + text + "\r\n";
        }

        private void LoadINI()
        {
            this.username = this.ini.ReadValue("general", "username");
            this.textUsername.Text = this.username;
            this.password = this.ini.ReadValue("general", "password");
            this.textPassword.Text = this.password;
            this.folder = this.ini.ReadValue("general", "folder");
            this.textFolder.Text = this.folder;
            this.host = this.ini.ReadValue("general", "host");
            if (this.host == "")
            {
                this.host = DEFAULT_HOST;
                this.ini.WriteValue("general", "host", this.host);
            }

            this.loaded = true;
        }

        private void SaveINI()
        {
            if (this.loaded)
            {
                this.ini.WriteValue("general", "username", this.textUsername.Text);
                this.ini.WriteValue("general", "password", this.textPassword.Text);
                this.ini.WriteValue("general", "folder", this.textFolder.Text);
                this.ini.WriteValue("general", "host", this.host);
            }
        }

        private void SyncForm_Load(object sender, EventArgs e)
        {
            LogText("WoWthing Sync started");

            var iniPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "wowthing_sync.ini");
            LogText("Using " + iniPath);
            
            this.ini = new INI(iniPath);
            this.LoadINI();
        }

        private void textUsername_TextChanged(object sender, EventArgs e)
        {
            this.SaveINI();
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {
            this.SaveINI();
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textFolder.Text = fbd.SelectedPath;
                this.SaveINI();
            }
        }
    }
}
