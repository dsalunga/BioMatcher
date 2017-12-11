using BioMatcher.ServiceAdapter;
using SourceAFIS.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioMatcher.OpenAFIS
{

    //[Serializable]
    //public class MyFingerprint : Fingerprint
    //{
    //    public string Filename;
    //}

    //// Inherit from Person in order to add Name field
    //[Serializable]
    //public class MyPerson : Person
    //{
    //    public string Name;
    //}

    [Serializable]
    public class MemberPerson : Person
    {
        public MemberFingerprint Member { get; set; }
    }

    public class FingerprintMatcher : IFingerprintSdk
    {
        public bool BenchmarkMode { get; set; }

        public bool IsBatchIdentify { get; set; }

        public FingerprintMatcher()
        {
            IsBatchIdentify = true;
        }

        static AfisEngine Afis;


        public MatchResult Identify(byte[] matchWith, int contextId)
        {
            throw new NotImplementedException();
        }

        public MemberFingerprint BatchIdentify(MatchRequest toMatch)
        {
            _toMatch = new MemberPerson();
            Fingerprint f = new Fingerprint();
            f.AsIsoTemplate = toMatch.Fingerprint;
            //_toMatch.Member = toMatch;

            Afis.Threshold = 10;
            //Console.WriteLine("Identifying {0} in database of {1} persons...", probe.Name, database.Count);
            var match = Afis.Identify(_toMatch, _persons).FirstOrDefault() as MemberPerson;
            // Null result means that there is no candidate with similarity score above threshold
            if (match == null)
            {
                //Console.WriteLine("No matching person found.");
                return match.Member;
            }

            return null;
        }

        private bool _isInitialized;
        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public bool IdentifyPrepare(byte[] toMatch, int contextId)
        {
            throw new NotImplementedException();
        }

        public MatcherInitResult Initialize()
        {
            Afis = new AfisEngine();
            _isInitialized = true;
            return null;
        }

        public void PreMatch(out int contextId)
        {
            contextId = 0;
        }

        public void PostMatch(int contextId)
        {
        }

        public string WriteError(int errorCode)
        {
            return "";
        }

        MemberPerson _toMatch = null;

        private List<MemberFingerprint> _fingerPrints = new List<MemberFingerprint>();
        private List<Person> _persons = new List<Person>();
        public List<MemberFingerprint> Fingerprints
        {
            get
            {
                return _fingerPrints;
            }

            set
            {
                _fingerPrints = value;
                foreach (var print in _fingerPrints)
                {
                    try
                    {
                        MemberPerson p = new MemberPerson();
                        Fingerprint personPrint = new Fingerprint();
                        personPrint.AsIsoTemplate = print.Fingerprint;
                        p.Fingerprints.Add(personPrint);
                        p.Member = print;
                    }
                    catch { }
                }
            }
        }


        public bool IdentifyPrepareRaw(byte[] toRawMatch, int contextId)
        {
            throw new NotImplementedException();
        }

        public byte[] ExtractTemplate(byte[] rawImage, int contextId)
        {
            throw new NotImplementedException();
        }
    }
}
