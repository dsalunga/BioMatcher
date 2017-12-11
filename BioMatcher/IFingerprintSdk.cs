using BioMatcher.ServiceAdapter;
using System;
using System.Collections.Generic;
namespace BioMatcher
{
    public interface IFingerprintSdk
    {
        //bool BenchmarkMode { get; set; }
        //string CacheFingerPrints(string startupPath, int cacheSize = -1);
        MatchResult Identify(byte[] matchWith, int contextId);
        bool IdentifyPrepare(byte[] toMatch, int contextId);
        MatcherInitResult Initialize();
        //void Initialize(Application.StartupPath, benchmarkMode, cacheSize);
        MemberFingerprint BatchIdentify(MatchRequest toMatch);
        void PreMatch(out int contextId);
        void PostMatch(int contextId);
        string WriteError(int errorCode);

        bool IsBatchIdentify { get; set; }

        bool IsInitialized { get; }

        bool IdentifyPrepareRaw(byte[] toRawMatch, int contextId);
        byte[] ExtractTemplate(byte[] rawImage, int contextId);

        //List<MemberFingerprint> Fingerprints { get; set; }
        //Dictionary<int, List<MemberFingerprint>> Fingerprints { get; set; }
        //IFingerprintMatcher Matcher { get; set; }
    }

    //public  class FingerprintMatcherBase : IFingerprintMatcher
    //{
    //    public bool BenchmarkMode { get; set; }

    //    public bool IsBatchIdentify { get; set; }

    //    public List<MemberFingerprint> FingerPrints { get; set; }
    //    //IFingerprintMatcher Matcher { get; set; }
    //}
}
