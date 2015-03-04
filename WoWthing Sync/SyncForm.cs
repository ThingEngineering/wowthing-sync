using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Timers;
using System.Net.Http;
using System.Threading.Tasks;

namespace WoWthing_Sync
{
    public partial class SyncForm : Form
    {
        private const string DEFAULT_HOST = "https://www.wowthing.org/";

        private INI ini;
        private bool loaded = false, paused = true;
        private string host, changedFile;

        private List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
        private System.Timers.Timer timer = new System.Timers.Timer(500);

        public SyncForm()
        {
            InitializeComponent();
        }

        private void SyncForm_Load(object sender, EventArgs e)
        {
            LogText("WoWthing Sync started");

            var iniPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "wowthing_sync.ini");
            LogText("Using " + iniPath);
            
            // Timer setup
            this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            this.timer.SynchronizingObject = this;

            this.ini = new INI(iniPath);
            this.LoadINI();
            this.Pause();

            // Auto-start if the Start button is enabled
            if (this.btnStart.Enabled == true)
            {
                this.btnStart.PerformClick();
            }
        }

        private void textUsername_TextChanged(object sender, EventArgs e)
        {
            this.Pause();
            this.SaveINI();
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {
            this.Pause();
            this.SaveINI();
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textFolder.Text = fbd.SelectedPath;
                this.Pause();
                this.SaveINI();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.paused)
            {
                this.Start();
            }
            else
            {
                this.Pause();
            }
        }

        delegate void LogTextCallback(string text);
        private void LogText(string text)
        {
            // Invoke magic if accessing from another thread
            if (this.textLog.InvokeRequired)
            {
                LogTextCallback ltc = new LogTextCallback(LogText);
                this.Invoke(ltc, new object[] { text });
            }
            else
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                textLog.AppendText("[" + timestamp + "] " + text + "\r\n");
            }
        }

        private void Pause()
        {
            this.btnStart.Text = "Start";
            if (this.textUsername.Text != "" &&
                this.textPassword.Text != "" &&
                this.textFolder.Text != "")
            {
                this.btnStart.Enabled = true;
            }
            else
            {
                this.btnStart.Enabled = false;
            }

            // Disable the watchers
            foreach (FileSystemWatcher fsw in this.watchers)
            {
                fsw.EnableRaisingEvents = false;
            }
            this.watchers = new List<FileSystemWatcher>();

            this.paused = true;
        }

        private void Start()
        {
            // Get a list of account directories
            string wtfPath = Path.Combine(this.textFolder.Text, @"WTF\Account");
            string[] dirs = Directory.GetDirectories(wtfPath);
            
            for (int i = 0; i < dirs.Length; i++)
            {
                // Check to see if our Lua file exists
                string svPath = Path.Combine(dirs[i], "SavedVariables");
                string luaPath = Path.Combine(svPath, "WoWthing_Collector.lua");
                if (File.Exists(luaPath))
                {
                    FileSystemWatcher fsw = new FileSystemWatcher(svPath);
                    fsw.Filter = "WoWthing_Collector.lua";
                    // Watch for changes in LastWrite times
                    fsw.NotifyFilter = NotifyFilters.LastWrite;
                    // Add event handler
                    fsw.Changed += new FileSystemEventHandler(this.FileChanged);
                    // Begin watching
                    fsw.EnableRaisingEvents = true;

                    this.watchers.Add(fsw);
                    LogText("Watching " + dirs[i]);
                }
            }

            // Update button
            this.textStatus.Text = "ACTIVE";
            this.btnStart.Text = "Pause";

            this.paused = false;
        }

        private void FileChanged(object source, FileSystemEventArgs e)
        {
            //LogText("FileChanged event: " + e.FullPath);
            this.changedFile = e.FullPath;
            // Activate the timer
            this.timer.Start();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //LogText("TimerEvent: " + this.changedFile);
            this.timer.Stop();

            this.Upload(this.changedFile);
        }

        private void LoadINI()
        {
            this.textUsername.Text = this.ini.ReadValue("general", "username");
            this.textPassword.Text = this.ini.ReadValue("general", "password");
            this.textFolder.Text = this.ini.ReadValue("general", "folder");
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

        private void Upload(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);

            HttpContent fileContent = new ByteArrayContent(fileData);
            
            var client = new HttpClient();
            client.BaseAddress = new Uri(this.host);
            client.DefaultRequestHeaders.Add("User-Agent", "WoWthing Sync");
            client.Timeout = new System.TimeSpan(0, 0, 10);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(this.textUsername.Text), "username");
            formData.Add(new StringContent(this.textPassword.Text), "password");
            formData.Add(fileContent, "lua_file", "WoWthing_Collector.lua");

            LogText("Uploading " + this.changedFile);
            client.PostAsync("api/upload/", formData).ContinueWith(
                (postTask) =>
                {
                    try
                    {
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            LogText("Upload successful.");
                        }
                        else
                        {
                            var errorMessage = result.Content.ReadAsStringAsync().Result;
                            LogText("Upload failed: " + errorMessage);
                        }
                    }
                    catch (AggregateException aex)
                    {
                        foreach (Exception ex in aex.InnerExceptions)
                        {
                            LogText("EXCEPTION: " + ex.Message);
                        }
                    }
                }
            );
        }

        private void SyncForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.notifyIcon.Visible = true;
                this.notifyIcon.ShowBalloonTip(3000);
                this.ShowInTaskbar = false;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                this.ShowInTaskbar = true;
                this.notifyIcon.Visible = false;
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
