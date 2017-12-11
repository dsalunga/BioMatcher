using BioMatcher.ServiceAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioMatcher
{
    public class MatchRequest
    {
        public MatchRequest()
        {
            SkipLocale = true;
            this.RequestDate = DateTime.Now;
        }

        public MatchRequest(MemberFingerprint data)
            : this()
        {
            this.Fingerprint = data.Fingerprint;
            this.LocaleId = data.LocaleId;
        }

        public MatchRequest(byte[] data)
            : this()
        {
            this.Fingerprint = data;
        }

        public byte[] Fingerprint { get; set; }

        /// <summary>
        /// Origin LocaleId
        /// </summary>
        public int LocaleId { get; set; }
        public int ExtraRefCode { get; set; }
        public DateTime RequestDate { get; set; }
        public bool SkipLocale { get; set; }
    }
}
