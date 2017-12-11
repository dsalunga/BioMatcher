using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using WCMS.Common.Utilities;

namespace BioMatcher.WebService
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            int cacheSize = 0;
            bool benchmarkMode = false;

            var key = ConfigurationManager.AppSettings["BioMatcher.CacheSize"];
            if (!string.IsNullOrEmpty(key))
                cacheSize = Convert.ToInt32(key);

            key = ConfigurationManager.AppSettings["BioMatcher.BenchmarkMode"];
            if (!string.IsNullOrEmpty(key))
                benchmarkMode = DataHelper.GetBool(key); // Convert.ToInt32(key);
            //LogMessage("BenchmarkMode: " + BenchmarkMode);

            MatchManager.Mode = MatcherModes.ServerFullCache;
            MatchManager.StartupPath = Server.MapPath("~");
            MatchManager.BenchmarkMode = benchmarkMode;
            MatchManager.CacheSize = cacheSize;
            var cacheStatus = MatchManager.Initialize();

            //LogMessage(cacheStatus);

            //LogMessage("CacheSize: " + (BenchmarkMode ? CacheSize : FingerprintMatcher.Cache.TotalFingerprints));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}