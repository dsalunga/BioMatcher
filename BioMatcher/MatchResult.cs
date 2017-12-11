using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioMatcher
{
    public class MatchResult
    {
        public bool Found { get; set; }
        public int Score { get; set; }
        public int MemberId { get; set; }
        public string Extra { get; set; }
        public int ExtraRefCode { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
