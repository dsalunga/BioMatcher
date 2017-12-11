using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioMatcher
{
    public class CompareResult
    {
        public TimeSpan Elapsed { get; set; }
        //public bool Success { get; set; }
        public int Scanned { get; set; }
        public DateTime TimeCompleted { get; set; }

        public MatchResult Result { get; set; }

        public CompareResult()
        {
            Result = new MatchResult();
        }
    }
}
