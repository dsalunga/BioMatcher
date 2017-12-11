using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BioMatcher.ServiceAdapter.FingerPrintServiceReference;

namespace BioMatcher.ServiceAdapter
{
    public class ServiceAdapterUtil
    {
        public static List<MemberFingerprint> GetList(DateTime lastUpdate, int localeId = -1)
        {
            var client = new FingerPrintSoapClient();
            var items = client.Get(-1, -1, localeId > 0 ? localeId : -1, -1, 0, 1, lastUpdate, false); // 2=ISO,0=Native

            return (from i in items
                   select From(i)).ToList();
        }

        public static MemberFingerprint From(MemberFingerPrintRow row)
        {
            return new MemberFingerprint
                   {
                       Id = (int)row.MemberFingerPrintID,
                       MemberId = (int)row.MemberID,
                       Size = row.FingerPrintSize,
                       Fingerprint = row.FingerPrint,
                       FingerType = row.FingerType,
                       Status = row.Status,
                       LocaleId = row.LocaleID,
                       DateUpdated = row.DateUpdated
                   };
        }
    }
}
