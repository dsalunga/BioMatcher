using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BioMatcher.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBioMatcher" in both code and config file together.
    [ServiceContract]
    public interface IBioMatcherWcfService
    {
        [OperationContract]
        void Initialize();

        [OperationContract]
        MatchResult Identify(MatchRequest request);

        [OperationContract]
        void UpdateCache(int localeId = -1, bool fullUpdate = false);

        [OperationContract]
        string GetCacheStatus();
    }
}
