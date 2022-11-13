using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WoWthing_Sync
{
    public partial class SyncForm : Form
    {
        private const string STARTUP_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

#if DEBUG
        private const string STARTUP_NAME = "WoWthingDebug";
        private const string UPLOAD_HOST = "https://localhost:55501/";
#else
        private const string STARTUP_NAME = "WoWthingRelease";
        private const string UPLOAD_HOST = "https://wowthing.org/";
#endif

        private bool isPaused = true;
        private bool isUploading = false;

        private List<string> watchedPaths = new List<string>();
        private readonly Dictionary<string, DateTime> lastUpdated = new Dictionary<string, DateTime>();
        private readonly Dictionary<string, DateTime> changedFiles = new Dictionary<string, DateTime>();
        private readonly HttpClient client = new HttpClient();
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(500);
        private readonly TimeSpan waitInterval = TimeSpan.FromMilliseconds(2000);
        private int counter = 0;

        public SyncForm()
        {
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = string.Format("WoWthing Sync {0}.{1}.{2}", version.Major, version.Minor, version.Build);
        }

        private void SyncForm_Load(object sender, EventArgs e)
        {
            Log("WoWthing Sync started");

            LoadSettings();
            Pause();

            // Auto-start if the Start button is enabled
            if (btnStart.Enabled)
            {
                btnStart.PerformClick();
            }

            // HttpClient setup
            client.BaseAddress = new Uri(UPLOAD_HOST);
            client.DefaultRequestHeaders.Add("User-Agent", "WoWthing Sync");
            client.Timeout = new System.TimeSpan(0, 0, 20);

            // Timer setup
            _timer.Elapsed += _timer_Elapsed;
            _timer.SynchronizingObject = this;
            _timer.Enabled = true;
        }

        private delegate void LogCallback(string text, string[] args);

        private void Log(string text, params string[] args)
        {
            // Invoke magic if accessing from another thread
            if (textLog.InvokeRequired)
            {
                LogCallback ltc = new LogCallback(Log);
                Invoke(ltc, new object[] { text, args });
            }
            else
            {
                if (args.Length > 0)
                {
                    text = String.Format(text, args);
                }
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
            Log("Paused");

            btnStart.Text = "Start";
            btnStart.Enabled = (textApiKey.Text != "" && textFolder.Text != "");
            textStatus.Text = "PAUSED";

            Icon = notifyIcon.Icon = Properties.Resources.PausedIcon;

            isPaused = true;
        }

        private void Start()
        {
            watchedPaths = new List<string>();

            // Get a list of account directories
            string wtfPath = Path.Combine(textFolder.Text, @"WTF\Account");
            string[] dirs = Directory.GetDirectories(wtfPath);

            for (int i = 0; i < dirs.Length; i++)
            {
                // Check to see if our Lua file exists
                string svPath = Path.Combine(dirs[i], "SavedVariables");
                if (Directory.Exists(svPath))
                {
                    string luaPath = Path.Combine(svPath, "WoWthing_Collector.lua");
                    if (File.Exists(luaPath))
                    {
                        lastUpdated[luaPath] = File.GetLastWriteTimeUtc(luaPath);
                    }

                    watchedPaths.Add(luaPath);
                    Log("Watching {0}", dirs[i]);
                }
            }

            // Update button
            textStatus.Text = "ACTIVE";
            btnStart.Text = "Pause";

            isPaused = false;

            Icon = notifyIcon.Icon = Properties.Resources.ActiveIcon;

            // Minimize on startup
            if (checkStartMinimized.Checked)
            {
                this.WindowState = FormWindowState.Minimized;
            }
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
            if (Properties.Settings.Default.WindowH < 100 ||
                Properties.Settings.Default.WindowX < 0 ||
                Properties.Settings.Default.WindowY < 0)
            {
                if (Properties.Settings.Default.WindowH < 100)
                {
                    Properties.Settings.Default.WindowH = 400;
                }

                if (Properties.Settings.Default.WindowX < 0)
                {
                    Properties.Settings.Default.WindowX = 0;
                }

                if (Properties.Settings.Default.WindowY < 0)
                {
                    Properties.Settings.Default.WindowY = 0;
                }

                Properties.Settings.Default.Save();
            }

            Height = (int)Properties.Settings.Default.WindowH;
            Left = (int)Properties.Settings.Default.WindowX;
            Top = (int)Properties.Settings.Default.WindowY;

            // Restore our settings
            textApiKey.Text = Properties.Settings.Default.ApiKey;
            textFolder.Text = Properties.Settings.Default.WatchFolder;
            checkStartMinimized.Checked = Properties.Settings.Default.StartMinimized;
            checkStartOnStartup.Checked = Properties.Settings.Default.StartOnStartup;
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

            var upload = new ApiUpload
            {
                ApiKey = textApiKey.Text,
                LuaFile = File.ReadAllText(filePath, Encoding.UTF8),
            };
            var json = JsonConvert.SerializeObject(upload, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
            });

            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            var content = new CompressedContent(new StringContent(json, Encoding.UTF8, "application/json"));

            Log("Uploading {0}...", filePath);
            client.PostAsync("/api/upload/", content).ContinueWith(
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
                            if (ex.InnerException != null)
                            {
                                Log("  - {0}", ex.InnerException.Message);
                            }
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
            Properties.Settings.Default.WindowX = Left;
            Properties.Settings.Default.WindowY = Top;
            Properties.Settings.Default.WindowH = Height;
            Properties.Settings.Default.Save();
        }

        private void textApiKey_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ApiKey = textApiKey.Text;
            Properties.Settings.Default.Save();
            Pause();
        }

        private void checkStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartMinimized = checkStartMinimized.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkStartOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartOnStartup = checkStartOnStartup.Checked;
            Properties.Settings.Default.Save();

            RegistryKey key = Registry.CurrentUser.OpenSubKey(STARTUP_KEY, true);
            if (checkStartOnStartup.Checked)
            {
                key.SetValue(STARTUP_NAME, Application.ExecutablePath.ToString());
            }
            else
            {
                var existing = key.GetValue(STARTUP_NAME);
                if (existing != null)
                {
                    key.DeleteValue(STARTUP_NAME);
                }
            } 
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (!string.IsNullOrWhiteSpace(textFolder.Text) && Directory.Exists(textFolder.Text))
            {
                fbd.SelectedPath = textFolder.Text;
            }

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string retailFolder = Path.Combine(fbd.SelectedPath, "_retail_");
                textFolder.Text = Directory.Exists(retailFolder) ? retailFolder : fbd.SelectedPath;
                Properties.Settings.Default.WatchFolder = textFolder.Text;
                Properties.Settings.Default.Save();
                Pause();
            }
        }

        private void btnManualUpload_Click(object sender, EventArgs e)
        {
            if (!isUploading)
            {
                foreach (string luaPath in watchedPaths)
                {
                    if (File.Exists(luaPath))
                    {
                        Upload(luaPath);
                    }
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (isPaused)
            {
                Start();
            }
            else
            {
                Pause();
            }
        }

        private void SyncForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(3000);
                ShowInTaskbar = false;
                Hide();
            }
            else if (WindowState == FormWindowState.Normal)
            {
                notifyIcon.Visible = false;
                ShowInTaskbar = true;
                Show();
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!isUploading)
            {
                lock (changedFiles)
                {
                    counter = (counter + 1) % 4;
                    if (counter == 0)
                    {
                        foreach (string luaPath in watchedPaths)
                        {
                            if (File.Exists(luaPath))
                            {
                                DateTime newMtime = File.GetLastWriteTimeUtc(luaPath);
                                lastUpdated.TryGetValue(luaPath, out DateTime oldMtime);

                                if (newMtime > oldMtime)
                                {
                                    changedFiles[luaPath] = DateTime.Now;
                                    lastUpdated[luaPath] = newMtime;
                                }
                            }
                        }
                    }

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

    public class ApiUpload
    {
        public string ApiKey { get; set; }
        public string LuaFile { get; set; }
    }
}
