using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Timers;
using System.Net.Http;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace WoWthing_Sync
{
    public partial class SyncForm : Form
    {
#if DEBUG
        private const string UPLOAD_HOST = "http://192.168.25.10:8002/";
#else
        private const string UPLOAD_HOST = "https://www.wowthing.org/";
#endif

        private bool isPaused = true;
        private bool isUploading = false;

        private Dictionary<string, DateTime> changedFiles = new Dictionary<string, DateTime>();
        private List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();
        private System.Timers.Timer _timer = new System.Timers.Timer(500);
        private readonly TimeSpan waitInterval = TimeSpan.FromMilliseconds(2500);

        public SyncForm()
        {
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = String.Format("WoWthing Sync {0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        private void SyncForm_Load(object sender, EventArgs e)
        {
            Log("WoWthing Sync started");

            this.LoadSettings();
            this.Pause();

            // Auto-start if the Start button is enabled
            if (this.btnStart.Enabled == true)
            {
                this.btnStart.PerformClick();
            }

            // Timer setup
            this._timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            this._timer.SynchronizingObject = this;
            this._timer.Enabled = true;
        }

        delegate void LogCallback(string text, string[] args);
        private void Log(string text, params string[] args)
        {
            // Invoke magic if accessing from another thread
            if (this.textLog.InvokeRequired)
            {
                LogCallback ltc = new LogCallback(Log);
                this.Invoke(ltc, new object[] { text, args });
            }
            else
            {
                if (args.Length > 0)
                    text = String.Format(text, args);
                textLog.AppendText(String.Format("[{0}] {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), text));
            }
        }

        private void LogDebug(string text, params string[] args)
        {
#if DEBUG
            Log(text, args);
#endif
        }

        private void Pause()
        {
            this.btnStart.Text = "Start";
            this.btnStart.Enabled = (this.textUsername.Text != "" && this.textPassword.Text != "" && this.textFolder.Text != "");

            // Disable the watchers
            foreach (FileSystemWatcher fsw in this.watchers)
            {
                fsw.EnableRaisingEvents = false;
            }
            this.watchers = new List<FileSystemWatcher>();

            this.isPaused = true;
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
                    fsw.Changed += new FileSystemEventHandler(this.Watcher_FileChanged);
                    // Begin watching
                    fsw.EnableRaisingEvents = true;

                    this.watchers.Add(fsw);
                    Log("Watching {0}", dirs[i]);
                }
            }

            // Update button
            this.textStatus.Text = "ACTIVE";
            this.btnStart.Text = "Pause";

            this.isPaused = false;
        }

        private void LoadSettings()
        {
            // Check if an upgrade is required
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

            // Restore window size and position
            if (Properties.Settings.Default.WindowH > 0)
            {
                this.Height = (int)Properties.Settings.Default.WindowH;
            }
            this.Left = (int)Properties.Settings.Default.WindowX;
            this.Top = (int)Properties.Settings.Default.WindowY;

            // Restore our settings
            this.textUsername.Text = Properties.Settings.Default.Username;
            this.textPassword.Text = Properties.Settings.Default.Password;
            this.textFolder.Text = Properties.Settings.Default.WatchFolder;
        }

        private byte[] ReadAllGZip(string filePath)
        {
            using (FileStream src = System.IO.File.OpenRead(filePath))
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                    {
                        src.CopyTo(gzip);
                    }
                    return memory.ToArray();
                }
            }
        }

        private void Upload(string filePath)
        {
            isUploading = true;

            //byte[] fileData = File.ReadAllBytes(filePath);
            byte[] fileData;
            try
            {
                fileData = ReadAllGZip(filePath);
            }
            catch (Exception ex)
            {
                Log("EXCEPTION: {0}", ex.Message);

                lock (changedFiles)
                {
                    changedFiles[filePath] = DateTime.Now;
                }
                return;
            }

            HttpContent fileContent = new ByteArrayContent(fileData);
            
            var client = new HttpClient();
            client.BaseAddress = new Uri(UPLOAD_HOST);
            client.DefaultRequestHeaders.Add("User-Agent", "WoWthing Sync");
            client.Timeout = new System.TimeSpan(0, 0, 20);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(this.textUsername.Text), "username");
            formData.Add(new StringContent(this.textPassword.Text), "password");
            formData.Add(new StringContent("yup"), "compressed");
            formData.Add(fileContent, "lua_file", "WoWthing_Collector.lua");

            Log("Uploading {0}...", filePath);
            client.PostAsync("/api/upload/", formData).ContinueWith(
                (postTask) =>
                {
                    try
                    {
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            Log("Upload successful.");
                        }
                        else
                        {
                            var errorMessage = result.Content.ReadAsStringAsync().Result;
                            Log("Upload failed: {0}", errorMessage);
                        }
                    }
                    catch (AggregateException aex)
                    {
                        foreach (Exception ex in aex.InnerExceptions)
                        {
                            Log("EXCEPTION: {0}", ex.Message);
                        }
                    }

                    isUploading = false;
                }
            );
        }

        #region Events
        // Save our settings when the window closes
        private void SyncForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.WindowX = this.Left;
            Properties.Settings.Default.WindowY = this.Top;
            Properties.Settings.Default.WindowH = this.Height;
            Properties.Settings.Default.Save();
        }

        private void textUsername_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Username = textUsername.Text;
            Properties.Settings.Default.Save();
            Pause();
        }

        private void textPassword_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Password = textPassword.Text;
            Properties.Settings.Default.Save();
            Pause();
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textFolder.Text = fbd.SelectedPath;
                Properties.Settings.Default.WatchFolder = textFolder.Text;
                Properties.Settings.Default.Save();
                Pause();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.isPaused)
            {
                this.Start();
            }
            else
            {
                this.Pause();
            }
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

        private void Watcher_FileChanged(object source, FileSystemEventArgs e)
        {
            lock (changedFiles)
            {
                LogDebug("File changed: {0}", e.FullPath);
                changedFiles[e.FullPath] = DateTime.Now;
            }
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!isUploading)
            {
                lock (changedFiles)
                {
                    foreach (var kvp in changedFiles.Where(x => DateTime.Now - x.Value > waitInterval))
                    {
                        changedFiles.Remove(kvp.Key);
                        Upload(kvp.Key);
                        break;
                    }
                }
            }
        }
        #endregion
    }
}
