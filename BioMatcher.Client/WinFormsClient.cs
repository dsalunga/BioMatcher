using Microsoft.AspNet.SignalR.Client;
using System;
using System.Net.Http;
using System.Windows.Forms;
using System.Configuration;

using BioMatcher;
using WCMS.Common.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using BioMatcher.ServiceAdapter;

namespace BioMatcher.Client
{
    /// <summary>
    /// SignalR client hosted in a WinForms application. The client
    /// lets the user pick a user name, connect to the server asynchronously
    /// to not block the UI thread, and send chat messages to all connected 
    /// clients whether they are hosted in WinForms, WPF, or a web application.
    /// </summary>
    public partial class WinFormsClient : Form
    {
        public event EventHandler OnMatch;

        private Queue<MatchRequest> queue = new Queue<MatchRequest>();
        private Stopwatch queueTimer = new Stopwatch();
        private TimeSpan totalElapsed = new TimeSpan();
        private String UserName { get; set; }

        private BioMatcherClient client = new BioMatcherClient();

        internal WinFormsClient()
        {
            InitializeComponent();
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            //HubProxy.Invoke("Send", UserName, TextBoxMessage.Text);
            //TextBoxMessage.Text = String.Empty;
            //TextBoxMessage.Focus();
        }



        void WinFormsClient_OnMatch(object sender, EventArgs e)
        {
            if (queue.Count > 0)
            {
                RichTextBoxConsole.AppendText(Environment.NewLine + String.Format("{0}", "Request sent"));
                client.IdentifyAsync(queue.Dequeue());
            }
            else
            {
                if (queueTimer.IsRunning)
                {
                    queueTimer.Stop();
                    RichTextBoxConsole.AppendText("Queue Combined: " + queueTimer.Elapsed);
                    RichTextBoxConsole.AppendText(Environment.NewLine + "Queue Total: " + totalElapsed + Environment.NewLine);

                    queueTimer.Reset();
                }
            }
        }

        /// <summary>
        /// If the server is stopped, the connection will time out after 30 seconds (default), and the 
        /// Closed event will fire.
        /// </summary>
        private void Connection_Closed()
        {
            //Deactivate chat UI; show login UI. 
            this.Invoke((Action)(() => ChatPanel.Visible = false));
            //this.Invoke((Action)(() => ButtonSend.Enabled = false));
            this.Invoke((Action)(() => StatusText.Text = "You have been disconnected."));
            this.Invoke((Action)(() => SignInPanel.Visible = true));
        }

        private void SignInButton_Click(object sender, EventArgs e)
        {
            MatchManager.StartupPath = Application.StartupPath;
            //FingerprintMatcher.BenchmarkMode = false;
            RichTextBoxConsole.AppendText("BenchmarkMode: " + MatchManager.BenchmarkMode + Environment.NewLine);

            //Connect to server (use async method to avoid blocking UI thread)
            StatusText.Visible = true;
            StatusText.Text = "Connecting to server...";

            //client.EnableLocaleCache = false;
            client.LocaleId = DataHelper.GetInt32(ConfigurationManager.AppSettings["LocaleId"]);
            client.Initialize();
            client.EnableLocaleCache = false;

            client.OnMatchComplete += Client_OnMatchComplete;
            OnMatch += WinFormsClient_OnMatch;

            if (client.ClientMode == ClientModes.SignalR)
            {
                client.Connection.Closed += Connection_Closed;

                //Handle incoming event from server: use Invoke to write to console from SignalR's thread

                //client.OnIdentifyComplete((result) =>
                //{
                //    this.Invoke((Action)(() =>
                //    {
                //        if (result.Extra.Contains("Run time:"))
                //        {
                //            var elapsed = DateTime.Now - result.RequestDate;

                //            RichTextBoxConsole.AppendText(String.Format("{0}: {1}" + Environment.NewLine, "", result.Extra) +
                //                String.Format("Elapsed: {0}" + Environment.NewLine + Environment.NewLine, elapsed));

                //            totalElapsed += elapsed;
                //            EventHandler handler = OnMatch;
                //            if (handler != null)
                //                handler(this, null);
                //        }
                //        else
                //        {
                //            RichTextBoxConsole.AppendText(Environment.NewLine + String.Format("{0}: {1}" + Environment.NewLine, "", result.Extra));
                //        }
                //    }));
                //});

                client.HubProxy.On<DateTime, string>("addMessage", (date, message) =>
                {
                    this.Invoke((Action)(() =>
                    {
                        RichTextBoxConsole.AppendText(Environment.NewLine + String.Format("{0}" + Environment.NewLine, message));
                    }));
                });

                RichTextBoxConsole.AppendText("Connected to server at " + client.ServerURI + Environment.NewLine);
            }
            else
            {
                RichTextBoxConsole.AppendText("Ready." + Environment.NewLine);
            }

            //Activate UI
            SignInPanel.Visible = false;
            ChatPanel.Visible = true;
            //ButtonSend.Enabled = true;
            //TextBoxMessage.Focus();
        }

        void Client_OnMatchComplete(object sender, MatchResult e)
        {
            this.Invoke((Action)(() =>
            {
                var extra = e.Extra;
                if (string.IsNullOrEmpty(extra))
                    extra = string.Format("Found: {0}, MemberId: {1}, Elapsed: {2}", e.Found, e.MemberId, DateTime.Now - e.RequestDate);

                if (queue.Count > 0) //e.Extra != null && e.Extra.Contains("Run time:"))
                {
                    var elapsed = DateTime.Now - e.RequestDate;

                    RichTextBoxConsole.AppendText(String.Format("{0}" + Environment.NewLine, extra) +
                        String.Format("Elapsed: {0}" + Environment.NewLine, elapsed));

                    totalElapsed += elapsed;
                    EventHandler handler = OnMatch;
                    if (handler != null)
                        handler(this, null);
                }
                else
                {
                    RichTextBoxConsole.AppendText(Environment.NewLine + String.Format("{0}", extra));
                }
            }));
        }

        private void WinFormsClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Dispose();
        }

        private void cmdSendTasks_Click(object sender, EventArgs e)
        {
            var request = new MatchRequest(MatchManager.Cache.Fingerprints[MatchManager.LocaleId][1]); //LastFingerprint);
            request.ExtraRefCode = 123;
            RichTextBoxConsole.AppendText(Environment.NewLine + String.Format("{0}", "Request sent"));
            client.IdentifyAsync(request);
        }

        private void cmdSendQueue_Click(object sender, EventArgs e)
        {
            var count = Convert.ToInt32(txtQueue.Text.Trim());

            for (int i = 0; i < count; i++)
            {
                var request = new MatchRequest(MatchManager.Cache.Fingerprints[MatchManager.LocaleId][1]);
                request.ExtraRefCode = 321;
                queue.Enqueue(request); //.LastFingerprint));
            }

            totalElapsed = new TimeSpan();
            queueTimer.Restart();
            RichTextBoxConsole.AppendText(Environment.NewLine + String.Format("{0}", "Request sent"));
            client.IdentifyAsync(queue.Dequeue());
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            client.UpdateCache();
        }

        private void cmdGetStatus_Click(object sender, EventArgs e)
        {
            RichTextBoxConsole.AppendText(Environment.NewLine + client.GetCacheStatus());
        }
    }
}
