using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioMatcher;
using GriauleFingerprintLibrary;
using System.IO;
using WCMS.Common.Utilities;
using System.Data;
using GriauleFingerprintLibrary.DataTypes;
using BioMatcher.ServiceAdapter;

using System.Runtime.InteropServices;

namespace BioMatcher.Griaule
{

    public class FPThreadParameter
    {
        //public int Subset { get; set; }
        public int Index { get; set; }
        //public int ThreadSet { get; set; }
    }

    public class FingerprintMatcher : IFingerprintSdk
    {
        public FingerprintCore GriauleCore;
        //public List<MemberFingerprint> Fingerprints { get; set; }
        public bool IsBatchIdentify { get; set; }
        public bool BenchmarkMode { get; set; }

        // Some constants to make our code cleaner
        public const int ERR_CANT_OPEN_BD = -999;
        public const int ERR_INVALID_ID = -998;
        public const int ERR_INVALID_TEMPLATE = -997;

        private const int DEFAULT_IMAGE_WIDTH = 260;
        private const int DEFAULT_IMAGE_HEIGHT = 300;

        private static bool initialized = false;

        public bool IsInitialized
        {
            get
            {
                return initialized;
            }
        }

        // -----------------------------------------------------------------------------------
        // Support functions
        // -----------------------------------------------------------------------------------

        // This class creates an Util class with some functions
        // to help us to develop our GrFinger Application
        public FingerprintMatcher()
        {
        }

        public MemberFingerprint BatchIdentify(MatchRequest toMatch)
        {
            throw new NotImplementedException();
        }

        // Write and describe an error.
        public string WriteError(int errorCode)
        {
            switch (errorCode)
            {
                case FingerprintConstants.GR_ERROR_INITIALIZE_FAIL:
                    return "Fail to Initialize GrFingerX. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_NOT_INITIALIZED:
                    return "The GrFingerX Library is not initialized. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_FAIL_LICENSE_READ:
                    //MessageBox.Show("License not found. See manual for troubleshooting.");
                    return "License not found. See manual for troubleshooting. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_NO_VALID_LICENSE:
                    //MessageBox.Show("The license is not valid. See manual for troubleshooting.");
                    return "The license is not valid. See manual for troubleshooting. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_NULL_ARGUMENT:
                    return "The parameter have a null value. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_FAIL:
                    return "Fail to create a GDI object. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_ALLOC:
                    return "Fail to create a context. Cannot allocate memory. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_PARAMETERS:
                    return "One or more parameters are out of bound. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_WRONG_USE:
                    return "This function cannot be called at this time. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_EXTRACT:
                    return "Template Extraction failed. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_SIZE_OFF_RANGE:
                    return "Image is too larger or too short.  (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_RES_OFF_RANGE:
                    return "Image have too low or too high resolution. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_CONTEXT_NOT_CREATED:
                    return "The Context could not be created. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_INVALID_CONTEXT:
                    return "The Context does not exist. (Error:" + errorCode + ")";

                // Capture error codes

                case FingerprintConstants.GR_ERROR_CONNECT_SENSOR:
                    return "Error while connection to sensor. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_CAPTURING:
                    return "Error while capturing from sensor. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_CANCEL_CAPTURING:
                    return "Error while stop capturing from sensor. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_INVALID_ID_SENSOR:
                    return "The idSensor is invalid. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_SENSOR_NOT_CAPTURING:
                    return "The sensor is not capturing. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_INVALID_EXT:
                    return "The File have a unknown extension. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_INVALID_FILENAME:
                    return "The filename is invalid. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_INVALID_FILETYPE:
                    return "The file type is invalid. (Error:" + errorCode + ")";
                case FingerprintConstants.GR_ERROR_SENSOR:
                    return "The sensor raise an error. (Error:" + errorCode + ")";

                // Our error codes
                case ERR_INVALID_TEMPLATE:
                    return "Invalid Template. (Error:" + errorCode + ")";
                case ERR_INVALID_ID:
                    return "Invalid ID. (Error:" + errorCode + ")";
                case ERR_CANT_OPEN_BD:
                    return "Unable to connect to DataBase. (Error:" + errorCode + ")";

                default:
                    return "Error:" + errorCode;
            }
        }

        // -----------------------------------------------------------------------------------
        // Main functions for fingerprint recognition management
        // -----------------------------------------------------------------------------------

        // Initializes GrFinger ActiveX and all necessary utilities.
        public MatcherInitResult Initialize()//GrFingerXLib.GrFingerXCtrl grfingerx)
        {
            var result = new MatcherInitResult();
            GriauleCore = new FingerprintCore();
            //Initialize library
            if (!initialized)
            {
                GriauleCore.Initialize();

                if (MatchManager.Mode != MatcherModes.ServerFullCache)
                {
                    // Maybe required for extracting template
                    GriauleCore.CaptureInitialize();
                }

                initialized = true;
            }
            //return (int)axGrFingerXCtrl.CapInitialize();

            result.Success = true;
            return result;
        }

        public void PreMatch(out int contextId)
        {
            GriauleCore.CreateContext(out contextId);
        }

        public void PostMatch(int contextId)
        {
            GriauleCore.DestroyContext(contextId);
        }

        public bool IdentifyPrepare(byte[] toMatch, int contextId)
        {
            var template = new FingerprintTemplate();
            template.Buffer = toMatch;
            template.Size = toMatch.Length;
            GriauleCore.IdentifyPrepare(template, contextId);

            return true;
        }

        public bool IdentifyPrepareRaw(byte[] toRawMatch, int contextId)
        {
            FingerprintRawImage _fpRawImg = ConvertTemplate(toRawMatch);
            if (_fpRawImg != null)
            {
                var template = ExtractTemplate(_fpRawImg, contextId);
                GriauleCore.IdentifyPrepare(template, contextId);

                return true;
            }

            return false;
        }

        private FingerprintRawImage ConvertTemplate(byte[] rawFingerprint)
        {
            IntPtr _tmpRawImg = Marshal.AllocHGlobal(rawFingerprint.Length);
            Marshal.Copy(rawFingerprint, 0, _tmpRawImg, rawFingerprint.Length);

            FingerprintRawImage _rawImg = new FingerprintRawImage(_tmpRawImg, DEFAULT_IMAGE_WIDTH, DEFAULT_IMAGE_HEIGHT, FingerprintConstants.GR_DEFAULT_RES);

            Marshal.FreeHGlobal(_tmpRawImg);

            return _rawImg;
        }

        private FingerprintTemplate ExtractTemplate(FingerprintRawImage rawImage, int contextId)
        {
            FingerprintTemplate _tmpTemplate = null;
            GriauleCore.ExtractEx(rawImage, ref _tmpTemplate, contextId, GrTemplateFormat.GR_FORMAT_ISO);

            return _tmpTemplate;
        }

        public byte[] ExtractTemplate(byte[] rawImage, int contextId)
        {
            var raw = ConvertTemplate(rawImage);
            FingerprintTemplate _tmpTemplate = null;
            GriauleCore.ExtractEx(raw, ref _tmpTemplate, contextId, GrTemplateFormat.GR_FORMAT_ISO);

            return _tmpTemplate.Buffer;
        }

        public MatchResult Identify(byte[] matchWith, int contextId)
        {
            int score = -1;
            var f = matchWith;
            var template = new FingerprintTemplate();
            template.Buffer = f;
            template.Size = f.Length;
            // Comparing current template.
            var result = GriauleCore.Identify(template, out score, contextId);

            // Checking if query template and the reference template match.
            if (result == FingerprintConstants.GR_MATCH)
                return new MatchResult { Found = true, Score = score };
            return new MatchResult { Found = false };
        }
    }
}
