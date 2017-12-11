using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WCMS.Common.Utilities;
using BioMatcher;
using System.Diagnostics;
using System.Threading;

using System.Configuration;
using BioMatcher.ServiceAdapter;

namespace BioMatcher.Server
{
    public partial class ServerConsole : Form
    {
        private IDisposable SignalR { get; set; }
        private string ServerURI = "http://localhost:8080";

        public bool BenchmarkMode { get; set; }
        public int CacheSize { get; set; }
        public bool Verbose { get; set; }

        internal ServerConsole()
        {
            InitializeComponent();

            var key = ConfigurationManager.AppSettings["BioMatcher.ServerURI"];
            if (!string.IsNullOrEmpty(key))
                ServerURI = key;

            //key = ConfigurationManager.AppSettings["ThreadSet"];
            //if (!string.IsNullOrEmpty(key))
            //    threadSet = Convert.ToInt32(key);
            //WriteToConsole("ThreadSet: " + threadSet);
        }

        /// <summary>
        /// Calls the StartServer method with Task.Run to not
        /// block the UI thread. 
        /// </summary>
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            TriggerStartServer();
        }

        private void TriggerStartServer()
        {
            LogMessage("Starting server...");
            ButtonStart.Enabled = false;
            Task.Run(() => StartServer());

            //key = ConfigurationManager.AppSettings["ThreadCount"];
            //if (!string.IsNullOrEmpty(key))
            //    threadCount = Convert.ToInt32(key);
            //WriteToConsole("ThreadCount: " + threadCount);

            var key = ConfigurationManager.AppSettings["BioMatcher.CacheSize"];
            if (!string.IsNullOrEmpty(key))
                CacheSize = Convert.ToInt32(key);

            key = ConfigurationManager.AppSettings["BioMatcher.BenchmarkMode"];
            if (!string.IsNullOrEmpty(key))
                BenchmarkMode = DataHelper.GetBool(key); // Convert.ToInt32(key);
            LogMessage("BenchmarkMode: " + BenchmarkMode);

            MatchManager.Mode = MatcherModes.ServerFullCache;
            MatchManager.StartupPath = Application.StartupPath;
            MatchManager.BenchmarkMode = BenchmarkMode;
            MatchManager.CacheSize = CacheSize;
            var cacheStatus = MatchManager.Initialize();
            LogMessage(cacheStatus);

            LogMessage("CacheSize: " + (BenchmarkMode ? CacheSize : MatchManager.Cache.TotalFingerprints));
        }

        /// <summary>
        /// Stops the server and closes the form. Restart functionality omitted
        /// for clarity.
        /// </summary>
        private void ButtonStop_Click(object sender, EventArgs e)
        {
            //SignalR will be disposed in the FormClosing event
            Close();
        }


        private void WinFormsServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SignalR != null)
            {
                SignalR.Dispose();
            }
        }

        private void chkBreakOnMatch_CheckedChanged(object sender, EventArgs e)
        {
            MatchManager.BreakOnMatch = chkBreakOnMatch.Checked;
        }

        /// <summary>
        /// Starts the server and checks for error thrown when another server is already 
        /// running. This method is called asynchronously from Button_Start.
        /// </summary>
        private void StartServer()
        {
            try
            {
                SignalR = WebApp.Start(ServerURI);
            }
            catch (TargetInvocationException)
            {
                LogMessage("Server failed to start. A server is already running on " + ServerURI);
                //Re-enable button to let user try to start server again
                this.Invoke((Action)(() => ButtonStart.Enabled = true));
                return;
            }
            this.Invoke((Action)(() => ButtonStop.Enabled = true));
            LogMessage("Server started at " + ServerURI);
        }

        /// <summary>
        /// This method adds a line to the RichTextBoxConsole control, using Invoke if used
        /// from a SignalR hub thread rather than the UI thread.
        /// </summary>
        /// <param name="message"></param>
        internal void LogMessage(String message)
        {
            if (RichTextBoxConsole.InvokeRequired)
            {
                this.Invoke((Action)(() =>
                    LogMessage(message)
                ));
                return;
            }
            RichTextBoxConsole.AppendText(message + Environment.NewLine);
        }

        public MatchResult Identify(MatchRequest request)
        {
            return MatchManager.Identify(request);
        }

        public void UpdateCache(int localeId = -1, bool fullUpdate = false)
        {
            MatchManager.UpdateCache(localeId, fullUpdate);
        }

        private void ServerConsole_Load(object sender, EventArgs e)
        {
            TriggerStartServer();
        }

        private void cmdUpdateCache_Click(object sender, EventArgs e)
        {
            MatchManager.UpdateCache(-1, false);
            LogMessage("Cache Updated.");
        }

        private void chkVerbose_CheckedChanged(object sender, EventArgs e)
        {
            Verbose = chkVerbose.Checked;
        }
    }
}
