using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BioMatcher.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BioMatcher" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BioMatcher.svc or BioMatcher.svc.cs at the Solution Explorer and start debugging.
    public class BioMatcherWcfService : IBioMatcherWcfService
    {
        public void Initialize()
        {
        }

        public MatchResult Identify(MatchRequest request)
        {
            return MatchManager.Identify(request);
        }

        public void UpdateCache(int localeId = -1, bool fullUpdate = false)
        {
            MatchManager.UpdateCache(localeId, fullUpdate);
        }

        public string GetCacheStatus()
        {
            return MatchManager.GetCacheStatus();
        }
    }
}
