using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioMatcher.ServiceAdapter
{
    public class MemberFingerprint
    {
        public MemberFingerprint() { }

        public MemberFingerprint(byte[] fingerprint)
        {
            this.Fingerprint = fingerprint;
            this.Size = fingerprint.Length;
        }

        public int Id { get; set; }
        public int MemberId { get; set; }
        public byte[] Fingerprint { get; set; }
        public int Status { get; set; }
        public int LocaleId { get; set; }
        public DateTime DateUpdated { get; set; }
        public int FingerType { get; set; }
        public int Size { get; set; }

        /// <summary>
        /// Extra data that can be used by client as reference like Reader Device ID, Queue ID, etc
        /// </summary>
        public int ExtraRefCode { get; set; }
    }
}
