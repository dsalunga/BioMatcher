using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using BioMatcher;
using System.Configuration;
using WCMS.Common.Utilities;
using BioMatcher.ServiceAdapter;
using System.IO;

namespace BioMatcher.BenchMark
{
    public partial class FormMain : Form
    {
        //private static IFingerprintMatcher util;
        private int cacheSize = 18000;
        private bool benchmarkMode = true;
        private List<MemberFingerprint> seeds = new List<MemberFingerprint>();

        public FormMain()
        {
            InitializeComponent();

            txtThreads.Text = MatchManager.ProcessorCores.ToString();
        }

        private void cmdInitialize_Click(object sender, EventArgs e)
        {
            //var threadCount = 2;

            cacheSize = Convert.ToInt32(txtCount.Text);

            txtStatus.Text += "\r\nCacheSize: " + cacheSize;
            txtStatus.Text += "\r\nBenchmarkMode: " + benchmarkMode;

            MatchManager.StartupPath = Application.StartupPath;
            MatchManager.BenchmarkMode = benchmarkMode;
            MatchManager.CacheSize = cacheSize;
            MatchManager.SaveTempalates = chkSave.Checked;
            var result = MatchManager.Initialize();

            txtStatus.Text += "\r\n" + result;
            txtStatus.Text += "\r\nMatch Index: " + MatchManager.Cache.RandomSampleIndex;
            if (!benchmarkMode)
                txtStatus.Text += "\r\nActual Cache: " + MatchManager.Cache.TotalFingerprints;

            txtStatus.Text += "\r\n**Matcher Initialized Successfull**";
            cmdBenchmarkTask.Enabled = true;
            cmdBenchmarkSeed.Enabled = true;

            MatchManager.BenchmarkMode = true;
        }

        private void cmdBenchmarkTask_Click(object sender, EventArgs e)
        {
            var result = MatchManager.Identify(new MatchRequest(MatchManager.Cache.RandomSample));
            txtStatus.Text += "\r\n\r\n" + result.Extra;
        }

        private void chkBreakOnMarch_CheckedChanged(object sender, EventArgs e)
        {
            MatchManager.BreakOnMatch = chkBreakOnMarch.Checked;
        }

        private void cmdVerify_Click(object sender, EventArgs e)
        {
            /*var pfcount = util.FingerPrints.Count;
            var toMatch = util.FingerPrints[util.BenchmarkMode ? FingerprintUtil.RandomIndex : pfcount - 1];
            util.IdentifyPrepare(toMatch, 0);
            var result = util.Identify(i, 0);   */
            txtStatus.Text = "";
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            var key = ConfigurationManager.AppSettings["BioMatcher.CacheSize"];
            if (!string.IsNullOrEmpty(key))
                cacheSize = Convert.ToInt32(key);

            txtCount.Text = cacheSize.ToString();

            key = ConfigurationManager.AppSettings["BioMatcher.BenchmarkMode"];
            if (!string.IsNullOrEmpty(key))
                benchmarkMode = DataHelper.GetBool(key); // Convert.ToInt32(key);

            MatchManager.BreakOnMatch = chkBreakOnMarch.Checked;

            var dir = Path.Combine(Application.StartupPath, "Seeds");
            if (Directory.Exists(dir))
            {
                var files = Directory.EnumerateFiles(dir);
                foreach (var file in files)
                {
                    var seedData = File.ReadAllBytes(file);
                    seeds.Add(new MemberFingerprint(seedData));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var random = new Random().Next(0, seeds.Count - 1);
            var result = MatchManager.Identify(new MatchRequest(seeds[random])); //MatchManager.Cache.RandomSample));
            txtStatus.Text += "\r\n\r\n" + result.Extra;
        }
    }
}
