using BioMatcher.ServiceAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioMatcher
{
    public class FingerprintCache
    {
        public Dictionary<int, List<MemberFingerprint>> Fingerprints { get; set; }
        public int RandomSampleIndex { get; set; }
        public List<MemberFingerprint> Samples { get; set; }

        public List<LocaleInfo> Locales { get; set; }
        public DateTime LastFullUpdate { get; set; }

        public FingerprintCache()
        {
            Fingerprints = new Dictionary<int, List<MemberFingerprint>>();
            Locales = new List<LocaleInfo>();
            Samples = new List<MemberFingerprint>();

            LastFullUpdate = DateTime.MinValue;
        }

        public void Initialize(List<MemberFingerprint> items)
        {
            Fingerprints = new Dictionary<int, List<MemberFingerprint>>();
            Locales = new List<LocaleInfo>();

            foreach (var print in items)
            {
                if (print.Status == 1)
                {
                    if (!Fingerprints.ContainsKey(print.LocaleId))
                    {
                        Locales.Add(new LocaleInfo(print.LocaleId));
                        Fingerprints.Add(print.LocaleId, new List<MemberFingerprint>());
                    }

                    Fingerprints[print.LocaleId].Add(print);
                }
            }
        }

        public int TotalFingerprints
        {
            get
            {
                int total = 0;
                foreach (var locale in Fingerprints)
                {
                    total += locale.Value.Count;
                }

                return total;
            }
        }

        public MemberFingerprint RandomSample
        {
            get
            {
                if (Samples.Count > 0)
                {
                    if (RandomSampleIndex < Samples.Count)
                    {
                        return Samples[RandomSampleIndex];
                    }
                }
                else if (TotalFingerprints > 0)
                {
                    return Fingerprints[Locales[Locales.Count - 1].LocaleId][0];
                }

                return null;
            }
        }

        public MemberFingerprint LastFingerprint
        {
            get
            {
                var last = Fingerprints[Locales[Locales.Count - 1].LocaleId];
                return last[last.Count - 1];
            }
        }
    }
}
