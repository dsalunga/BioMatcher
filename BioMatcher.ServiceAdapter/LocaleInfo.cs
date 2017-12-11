using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioMatcher.ServiceAdapter
{
    public class LocaleInfo
    {
        public LocaleInfo(int localeId)
        {
            LocaleId = localeId;
            LastUpdated = DateTime.Now.AddSeconds(-15);
        }

        public int LocaleId { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
