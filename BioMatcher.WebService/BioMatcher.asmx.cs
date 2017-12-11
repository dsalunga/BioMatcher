using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace BioMatcher.WebService
{
    /// <summary>
    /// Summary description for BioMatcher
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BioMatcherService : System.Web.Services.WebService
    {

        [WebMethod]
        public void Initialize()
        {
            //return "Hello World";
        }

        [WebMethod]
        public MatchResult Identify(MatchRequest request)
        {
            return MatchManager.Identify(request);
        }

        [WebMethod]
        public void UpdateCache(int localeId = -1, bool fullUpdate = false)
        {
            MatchManager.UpdateCache(localeId, fullUpdate);
        }

        [WebMethod]
        public string GetCacheStatus()
        {
            return MatchManager.GetCacheStatus();
        }
    }
}
