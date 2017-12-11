using BioMatcher.ServiceAdapter;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WCMS.Common.Utilities;

namespace BioMatcher
{
    public class BioMatcherClient
    {
        public static bool IsSdkInitialized
        {
            get
            {
                return MatchManager.Sdk != null && MatchManager.Sdk.IsInitialized;
            }
        }

        private static BioMatcherClient _instance = null;
        public static BioMatcherClient GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BioMatcherClient();
            }

            return _instance;
        }

        public event EventHandler<MatchResult> OnMatchComplete;

        /// <summary>
        /// This name is simply added to sent messages to identify the user; this 
        /// sample does not include authentication.
        /// </summary>

        public IHubProxy HubProxy { get; set; }
        public string ServerURI = "http://localhost:8080/signalr";
        public HubConnection Connection { get; set; }
        public bool EnableLocaleCache { get; set; }
        public ClientModes ClientMode { get; set; }

        public BioMatcherClient()
        {
            ClientMode = ClientModes.Asmx;

            var serverURI = ConfigurationManager.AppSettings["BioMatcher.ServerURI"];
            if (!string.IsNullOrEmpty(serverURI))
                ServerURI = serverURI;

            var key = ConfigurationManager.AppSettings["BioMatcher.BenchmarkMode"];
            if (!string.IsNullOrEmpty(key))
                MatchManager.BenchmarkMode = DataHelper.GetBool(key);

            key = ConfigurationManager.AppSettings["BioMatcher.CacheMode"];
            if (!string.IsNullOrEmpty(key))
                MatchManager.Mode = (MatcherModes)DataHelper.GetInt32(key);
            EnableLocaleCache = MatchManager.Mode == MatcherModes.ClientLocaleCache;

            key = ConfigurationManager.AppSettings["BioMatcher.ClietMode"];
            if (!string.IsNullOrEmpty(key))
                ClientMode = (ClientModes)DataHelper.GetInt32(key);
        }

        public int LocaleId
        {
            set { MatchManager.LocaleId = value; }
            get { return MatchManager.LocaleId; }
        }

        private bool? _enabled = null;
        public bool Enabled
        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = DataHelper.GetBool(ConfigurationManager.AppSettings["BioMatcher.Enabled"], true);
                }

                return _enabled.Value;
            }

            set
            {
                _enabled = value;
            }
        }

        private bool _initialized = false;
        public void Initialize()
        {
            if (!_initialized)
            {
                MatchManager.Mode = EnableLocaleCache ? MatcherModes.ClientLocaleCache : MatcherModes.ClientNoCache;
                MatchManager.Initialize();

                switch (ClientMode)
                {
                    case ClientModes.SignalR:
                        ConnectAsync();
                        break;
                    case ClientModes.Asmx:
                        var asmxClient = new BioMatcherAsmxReference.BioMatcherServiceSoapClient();
                        asmxClient.Initialize();
                        break;
                    case ClientModes.Wcf:
                        var wcfClient = new BioMatcherWcfReference.BioMatcherWcfServiceClient();
                        wcfClient.Initialize();
                        break;
                }

                //lib = FingerprintUtil.CreateMatcher<IFingerprintMatcher>();
                //lib.Initialize();
                //lib.BenchmarkMode = benchmarkMode;
                //lib.FingerPrints = FingerprintUtil.CacheFingerPrints(benchmarkMode, Application.StartupPath);
                _initialized = true;
            }
        }

        /// <summary>
        /// Creates and connects the hub connection and hub proxy. This method
        /// is called asynchronously from SignInButton_Click.
        /// </summary>
        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerURI);
            HubProxy = Connection.CreateHubProxy("BioMatcherHub");

            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException ex)
            {
                throw ex;
                //StatusText.Text = "Unable to connect to server: Start server before connecting clients.";
                //No connection: Don't enable Send button or show chat UI
                //return;
            }

            HubProxy.On<MatchResult>("IdentifyComplete", (result) =>
            {
                var handler = OnMatchComplete;
                if (handler != null)
                    handler(this, result);
            });
        }

        public void IdentifyAsyncRaw(byte[] rawImage)
        {
            IdentifyAsync(new MatchRequest(Extract(rawImage)));
        }

        public byte[] Extract(byte[] fp)
        {
            return MatchManager.Sdk.ExtractTemplate(fp, 0);
        }


        public void IdentifyAsync(MatchRequest item)
        {
            if (item.LocaleId <= 0 && LocaleId > 0)
                item.LocaleId = LocaleId;

            MatchResult result = null;
            if (EnableLocaleCache)
            {
                result = MatchManager.Identify(item);
                if (result.Found)
                {
                    var handler = OnMatchComplete;
                    if (handler != null)
                        handler(this, result);
                    return;
                }
            }

            switch (ClientMode)
            {
                case ClientModes.SignalR:
                    HubProxy.Invoke("Identify", item);
                    break;
                case ClientModes.Asmx:
                    result = IdentifyAsmx(item);
                    var handler = OnMatchComplete;
                    if (handler != null)
                        handler(this, result);
                    break;
                case ClientModes.Wcf:
                    result = IdentifyWcf(item);
                    handler = OnMatchComplete;
                    if (handler != null)
                        handler(this, result);
                    break;
            }
        }

        //public void OnIdentifyComplete(Action<MatchResult> onResult)
        //{
        //    HubProxy.On<MatchResult>("IdentifyComplete", (result) =>
        //    {
        //        onResult(result);
        //    });
        //}

        public void Dispose()
        {
            if (ClientMode == ClientModes.SignalR)
            {
                if (Connection != null)
                {
                    Connection.Stop();
                    Connection.Dispose();
                }
            }
        }

        public void UpdateCache(int localeId = -1, bool fullUpdate = false)
        {
            if (EnableLocaleCache && localeId == LocaleId)
                MatchManager.UpdateCache(localeId, fullUpdate);

            switch (ClientMode)
            {
                case ClientModes.SignalR:
                    HubProxy.Invoke("UpdateCache", localeId, fullUpdate);
                    break;
                case ClientModes.Asmx:
                    UpdateCacheAsmx(localeId, fullUpdate);
                    break;
                case ClientModes.Wcf:
                    UpdateCacheWcf(localeId, fullUpdate);
                    break;
            }
        }

        /// <summary>
        /// Does not update the locale
        /// </summary>
        /// <param name="localeId"></param>
        /// <param name="fullUpdate"></param>
        public void UpdateCacheAsync(int localeId = -1, bool fullUpdate = false)
        {
            switch (ClientMode)
            {
                case ClientModes.SignalR:
                    HubProxy.Invoke("UpdateCache", localeId, fullUpdate);
                    break;
                case ClientModes.Asmx:
                    UpdateCacheAsmx(localeId, fullUpdate, true);
                    break;
                case ClientModes.Wcf:
                    UpdateCacheWcf(localeId, fullUpdate, true);
                    break;
            }
        }

        public string GetCacheStatus()
        {
            switch (ClientMode)
            {
                case ClientModes.Asmx:
                    var asmxClient = new BioMatcherAsmxReference.BioMatcherServiceSoapClient();
                    return asmxClient.GetCacheStatus();
                case ClientModes.Wcf:
                    var wcfClient = new BioMatcherWcfReference.BioMatcherWcfServiceClient();
                    return wcfClient.GetCacheStatus();
            }

            return string.Empty;
        }

        public MatchResult Identify(MatchRequest request)
        {
            if (request.LocaleId <= 0 && LocaleId > 0)
                request.LocaleId = LocaleId;

            if (EnableLocaleCache)
            {
                var result = MatchManager.Identify(request);
                if (result.Found)
                    return result;
            }

            switch (ClientMode)
            {
                case ClientModes.Asmx:
                    return IdentifyAsmx(request);
                case ClientModes.Wcf:
                    return IdentifyWcf(request);
            }

            return null;
        }

        private MatchResult IdentifyAsmx(MatchRequest request)
        {
            var svcRequest = ToAsmxRequest(request);
            var client = new BioMatcherAsmxReference.BioMatcherServiceSoapClient();
            var svcResult = client.Identify(svcRequest);
            return FromAsmxResult(svcResult);
        }

        private MatchResult IdentifyWcf(MatchRequest request)
        {
            var svcRequest = ToWcfRequest(request);
            var client = new BioMatcherWcfReference.BioMatcherWcfServiceClient();
            var svcResult = client.Identify(svcRequest);
            client.Close();
            return FromWcfResult(svcResult);
        }

        private BioMatcherAsmxReference.MatchRequest ToAsmxRequest(MatchRequest request)
        {
            var svcRequest = new BioMatcherAsmxReference.MatchRequest();
            svcRequest.Fingerprint = request.Fingerprint;
            svcRequest.LocaleId = request.LocaleId;
            svcRequest.SkipLocale = request.SkipLocale;
            svcRequest.RequestDate = request.RequestDate;
            svcRequest.ExtraRefCode = request.ExtraRefCode;
            return svcRequest;
        }

        private MatchResult FromAsmxResult(BioMatcherAsmxReference.MatchResult svcResult)
        {
            var result = new MatchResult();
            result.Extra = svcResult.Extra;
            result.ExtraRefCode = svcResult.ExtraRefCode;
            result.Found = svcResult.Found;
            result.MemberId = svcResult.MemberId;
            result.RequestDate = svcResult.RequestDate;
            result.Score = svcResult.Score;
            return result;
        }

        private BioMatcherWcfReference.MatchRequest ToWcfRequest(MatchRequest request)
        {
            var svcRequest = new BioMatcherWcfReference.MatchRequest();
            svcRequest.Fingerprint = request.Fingerprint;
            svcRequest.LocaleId = request.LocaleId;
            svcRequest.SkipLocale = request.SkipLocale;
            svcRequest.RequestDate = request.RequestDate;
            svcRequest.ExtraRefCode = request.ExtraRefCode;
            return svcRequest;
        }

        private MatchResult FromWcfResult(BioMatcherWcfReference.MatchResult svcResult)
        {
            var result = new MatchResult();
            result.Extra = svcResult.Extra;
            result.ExtraRefCode = svcResult.ExtraRefCode;
            result.Found = svcResult.Found;
            result.MemberId = svcResult.MemberId;
            result.RequestDate = svcResult.RequestDate;
            result.Score = svcResult.Score;
            return result;
        }

        private void UpdateCacheAsmx(int localeId = -1, bool fullUpdate = false, bool async = false)
        {
            var client = new BioMatcherAsmxReference.BioMatcherServiceSoapClient();
            if (async)
                client.UpdateCacheAsync(localeId, fullUpdate);
            else
                client.UpdateCache(localeId, fullUpdate);
            client.Close();
        }

        private void UpdateCacheWcf(int localeId = -1, bool fullUpdate = false, bool async = false)
        {
            var client = new BioMatcherWcfReference.BioMatcherWcfServiceClient();
            if (async)
                client.UpdateCacheAsync(localeId, fullUpdate);
            else
                client.UpdateCache(localeId, fullUpdate);
            client.Close();
        }
    }
}
