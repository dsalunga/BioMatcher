using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Configuration;
using System.Reflection;
using BioMatcher.ServiceAdapter;
using System.IO;
using System.Data;
using WCMS.Common.Utilities;

namespace BioMatcher
{
    public static class MatchManager
    {
        private static IFingerprintSdk sdk;
        public static FingerprintCache Cache { get; set; }

        private static bool found = false;
        public static bool BreakOnMatch = true;
        public static bool SaveTempalates { get; set; }

        static MatchManager()
        {
            LocaleId = 0;
            Mode = MatcherModes.ClientLocaleCache;
        }

        public static MatcherModes Mode { get; set; }

        private static int _localeId;
        public static int LocaleId
        {
            get
            {
                return _localeId;
            }

            set
            {
                var oldLocaleId = _localeId;
                _localeId = value;
                if (_localeId > 0 && _localeId != value && Mode == MatcherModes.ClientLocaleCache)
                    Initialize();
            }
        }
        public static List<List<int>> ThreadDistribution { get; set; }

        //public static int RandomIndex { get; set; }
        public static int CacheSize { get; set; }
        public static string StartupPath { get; set; }
        public static bool BenchmarkMode { get; set; }

        private static int coreCount = 0;
        public static int ProcessorCores
        {
            get
            {
                if (coreCount == 0)
                {
                    foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
                        coreCount += int.Parse(item["NumberOfCores"].ToString());

                    /*foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
                    {
                        Console.WriteLine("Number Of Logical Processors: {0}", item["NumberOfLogicalProcessors"]);
                    }*/
                }
                return coreCount;
            }
        }


        public static T CreateMatcher<T>()
        {
            Type dal = typeof(T);
            string matcherPath = ConfigurationManager.AppSettings["BioMatcher.SdkPath"];
            string className = ConfigurationManager.AppSettings["BioMatcher.SdkType"]; // _xmlProviderPath + "." + dal.Name.Substring(1);
            return (T)Assembly.Load(matcherPath).CreateInstance(className);
        }

        public static string Initialize()
        {
            InitMatcher();
            CacheFingerprints();
            var result = SetupThreadDistribution();
            if (BenchmarkMode)
                return result + "\r\nThreadSet: " + ThreadDistribution.Count + "\r\nTotal Cache: " + Cache.TotalFingerprints;
            return null;
        }

        private static void InitMatcher()
        {
            if (sdk == null)
            {
                sdk = CreateMatcher<IFingerprintSdk>();
                sdk.Initialize();
            }
        }

        private static string SetupThreadDistribution()
        {
            ThreadDistribution = new List<List<int>>();
            var result = new StringBuilder();
            var totalThreads = ProcessorCores;
            if (totalThreads > 1)
            {
                var totalFingerprints = Cache.TotalFingerprints;
                if (totalFingerprints > 0)
                {
                    var threadAvgFingerprint = totalFingerprints / totalThreads;
                    var currLocale = 0;
                    var totalLocaleCount = Cache.Locales.Count;
                    result.AppendLine("Ave. Thread Size: " + threadAvgFingerprint);

                    for (int i = 0; i < totalThreads; i++)
                    {
                        // Compute covered locales per thread
                        List<int> threadLocales = new List<int>();
                        var threadFingerprints = 0;
                        while (threadFingerprints < threadAvgFingerprint && currLocale < totalLocaleCount)
                        {
                            var locale = Cache.Locales[currLocale];
                            threadFingerprints += Cache.Fingerprints[locale.LocaleId].Count;
                            threadLocales.Add(locale.LocaleId);
                            currLocale++;
                        }

                        ThreadDistribution.Add(threadLocales);
                        if (BenchmarkMode)
                        {
                            var tfpTotal = 0;
                            foreach (var l in threadLocales)
                                tfpTotal += Cache.Fingerprints[l].Count;
                            result.AppendLine(string.Format("Thread {0}: {1}", i, tfpTotal));
                        }
                        if (currLocale >= totalLocaleCount)
                            break;
                    }
                }
            }
            return result.ToString();
        }

        public static void UpdateCache(int localeId = -1, bool fullUpdate = false)
        {
            if (localeId <= 0 && fullUpdate)
            {
                Initialize();
                return;
            }

            if (localeId > 0)
            {
                var locale = Cache.Locales.Find(i => i.LocaleId == localeId);
                var fingerPrints = ServiceAdapterUtil.GetList(locale.LastUpdated, LocaleId);
                locale.LastUpdated = DateTime.Now.AddSeconds(-15);
                UpdateLocaleCache(locale, fingerPrints);
            }
            else
            {
                var fingerPrints = ServiceAdapterUtil.GetList(Cache.LastFullUpdate);
                foreach (var locale in Cache.Locales)
                {
                    locale.LastUpdated = DateTime.Now.AddSeconds(-15);
                    var pfs = fingerPrints.FindAll(i => i.LocaleId == locale.LocaleId);
                    UpdateLocaleCache(locale, pfs);
                }
                Cache.LastFullUpdate = DateTime.Now.AddSeconds(-15);
            }
            SetupThreadDistribution();
        }

        private static void UpdateLocaleCache(LocaleInfo locale, List<MemberFingerprint> fingerPrints)
        {
            if (fingerPrints.Count > 0)
            {
                var current = Cache.Fingerprints[locale.LocaleId];
                var newlyAdded = fingerPrints.Except(current).Where(i => i.Status == 1);
                var changed = current.Intersect(newlyAdded);

                current.AddRange(newlyAdded);
                if (changed.Count() > 0)
                {
                    foreach (var c in changed)
                    {
                        if (c.Status != 0)
                        {
                            var cc = current.Find(i => i.Id == c.Id);
                            if (cc != null)
                                current.Remove(cc);
                        }
                    }
                }
            }
        }

        public static IFingerprintSdk Sdk
        {
            get { return sdk; }
        }

        private static void CacheFingerprints()
        {
            var container = new Dictionary<int, List<MemberFingerprint>>();
            var fingerPrints = new List<MemberFingerprint>();
            Cache = new FingerprintCache();

            var dataDir = "";
            if (BenchmarkMode && Directory.Exists(dataDir = Path.Combine(StartupPath, "Data")))
            {
                var files = Directory.EnumerateFiles(dataDir);
                int i = 0;
                foreach (var file in files)
                {
                    i++;
                    var print = File.ReadAllBytes(file);
                    fingerPrints.Add(new MemberFingerprint { Id = i, MemberId = i, Fingerprint = print, Size = print.Length });
                }
            }
            else
            {
                if (Mode != MatcherModes.ClientNoCache)
                {
                    if (Mode == MatcherModes.ClientLocaleCache)
                    {
                        fingerPrints = ServiceAdapterUtil.GetList(DateTime.MinValue, LocaleId);
                        if (Cache.Fingerprints.ContainsKey(LocaleId))
                            Cache.Locales[LocaleId].LastUpdated = DateTime.Now.AddSeconds(-15);
                    }
                    else if (Mode == MatcherModes.ServerFullCache)
                    {
                        fingerPrints = ServiceAdapterUtil.GetList(DateTime.MinValue);
                        Cache.LastFullUpdate = DateTime.Now.AddSeconds(-15);
                    }

                    if (SaveTempalates && BenchmarkMode)
                    {
                        if (!Directory.Exists(dataDir))
                            Directory.CreateDirectory(dataDir);

                        int i = 0;
                        foreach (var data in fingerPrints)
                        {
                            i++;
                            File.WriteAllBytes(Path.Combine(dataDir, string.Format("{0}.dat", i.ToString("D4"))), data.Fingerprint);
                        }
                    }

                    Cache.Initialize(fingerPrints);

                    if (false && fingerPrints.Count > 0 && BenchmarkMode)
                    {
                        if (!Directory.Exists(dataDir))
                            Directory.CreateDirectory(dataDir);

                        int i = 0;
                        foreach (var fp in fingerPrints)
                        {
                            i++;
                            File.WriteAllBytes(Path.Combine(dataDir, string.Format("{0}.dat", i.ToString("D4"))), fp.Fingerprint);
                        }
                    }
                }
            }

            if (BenchmarkMode)
            {
                bool suffleLastRecord = CacheSize > 0;
                var initialCount = fingerPrints.Count;

                var last = fingerPrints[initialCount - 1];
                if (suffleLastRecord)
                    fingerPrints[initialCount - 1] = fingerPrints[0];

                var index = 0;
                var cacheSize = CacheSize > initialCount ? CacheSize : initialCount;
                for (int i = initialCount; i < cacheSize; i++)
                {
                    fingerPrints.Add(fingerPrints[index]);
                    index++;
                    if (index == initialCount)
                        index = 0;
                }
                Cache.Samples = fingerPrints;

                if (suffleLastRecord)
                {
                    var random = new Random().Next(0, fingerPrints.Count - 1);
                    fingerPrints[random] = last;
                    Cache.RandomSampleIndex = random;
                }

                // Set Locales
                var totalThreads = ProcessorCores;
                var fpPerThread = fingerPrints.Count / totalThreads;

                var currThread = 0;
                var counter = 0;

                var localeFp = new List<MemberFingerprint>();
                Cache.Locales.Add(new LocaleInfo(currThread));
                Cache.Fingerprints.Add(currThread, localeFp);

                foreach (var item in fingerPrints)
                {
                    if (counter < fpPerThread)
                    {
                        item.LocaleId = currThread;
                        counter++;
                        localeFp.Add(item);
                    }
                    else
                    {
                        counter = 0;
                        currThread++;

                        localeFp = new List<MemberFingerprint>();
                        Cache.Locales.Add(new LocaleInfo(currThread));
                        Cache.Fingerprints.Add(currThread, localeFp);
                        item.LocaleId = currThread;
                    }
                }
            }
        }

        private static CompareResult StartMatcherAsync(MatchRequest toMatch, List<int> chaptersToScan)
        {
            if (BreakOnMatch && found)
                return new CompareResult { Elapsed = new TimeSpan(), Scanned = 0 };

            var matcher = CreateMatcher<IFingerprintSdk>();
            matcher.Initialize();
            var sw = new Stopwatch();
            int scanned = 0;
            int contextId = 0;
            var compareResult = new CompareResult();

            sw.Start();
            matcher.PreMatch(out contextId);
            matcher.IdentifyPrepare(toMatch.Fingerprint, contextId);
            foreach (var chapter in chaptersToScan)
            {
                var chapterFp = Cache.Fingerprints[chapter];
                foreach (var fp in chapterFp)
                {
                    if (BreakOnMatch && found)
                        break;

                    MatchResult result = null;
                    try
                    {
                        result = matcher.Identify(fp.Fingerprint, contextId);
                    }
                    catch (Exception ex) { throw ex; }

                    scanned++;
                    if (result.Found)
                    {
                        found = true;
                        result.MemberId = fp.MemberId;
                        compareResult.Result = result;

                        if (BreakOnMatch)
                            break;
                    }
                }
            }

            matcher.PostMatch(contextId);
            sw.Stop();

            compareResult.Elapsed = sw.Elapsed;
            compareResult.Scanned = scanned;
            return compareResult;
        }

        public static string GetCacheStatus()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var locale in Cache.Locales)
            {
                sb.AppendLine(string.Format("Locale: {0}, Last Update: {1}, Fingerprints: {2}; ", locale.LocaleId, locale.LastUpdated, Cache.Fingerprints[locale.LocaleId].Count));
            }
            sb.AppendLine("Cache Last Updated: " + Cache.LastFullUpdate);

            return sb.ToString();
        }

        public static MatchResult Identify(MatchRequest request)
        {
            found = false;
            //var subset = Cache.Fingerprints[0].Count;
            var sw = new Stopwatch();
            sw.Start();

            var compareTasks = new Task<CompareResult>[ThreadDistribution.Count];
            for (int i = 0; i < ThreadDistribution.Count; i++)
            {
                if (!found)
                {
                    var scanSet = ThreadDistribution[i];
                    compareTasks[i] = Task<CompareResult>.Factory.StartNew(() => StartMatcherAsync(request, scanSet));
                }
            }

            Task.WaitAll(compareTasks);
            sw.Stop();

            var status = new StringBuilder();
            if (BenchmarkMode)
                status.Append(ProcessorCores + " Tasks");
            int totalScanned = 0;
            var totalElapsed = new TimeSpan();
            var identifyResult = new MatchResult();
            identifyResult.RequestDate = request.RequestDate;
            identifyResult.ExtraRefCode = request.ExtraRefCode;

            foreach (var task in compareTasks)
            {
                if (task == null || task.Result == null)
                    continue;

                var result = task.Result;
                if (BenchmarkMode)
                    status.Append(string.Format("\r\nFingerPrints: {0}, Duration: {1}", result.Scanned, result.Elapsed));
                totalElapsed += result.Elapsed;
                totalScanned += result.Scanned;

                if (result.Result.Found)
                {
                    identifyResult.Found = true;
                    identifyResult.Score = result.Result.Score;
                    identifyResult.MemberId = result.Result.MemberId;
                    //identifyResult.ExtraRefCode = result.Result.ExtraRefCode;
                }
            }
            if (BenchmarkMode)
                status.Append("\r\n= " + Convert.ToInt32(totalScanned / (totalElapsed.TotalSeconds / ProcessorCores)) +
                    "/sec, Run time: " + sw.Elapsed + ", found: " + found);
            identifyResult.Extra = status.ToString();
            return identifyResult;
        }

    }
}
