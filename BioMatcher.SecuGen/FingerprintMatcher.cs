using BioMatcher.ServiceAdapter;
using SecuGen.FDxSDKPro.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioMatcher.SecuGen
{
    public class FingerprintMatcher : IFingerprintSdk
    {
        // true if ANSI template is used, otherwise ISO template is used.     
        private bool m_useAnsiTemplate = false; // true;  
        private SGFingerPrintManager fingerprintManager;
        private Int32 m_ImageWidth;
        private Int32 m_ImageHeight;
        private Int32 m_Dpi;
        private SGFPMSecurityLevel m_SecurityLevel;

        //private Byte[] m_RegMin1;
        //private Byte[] m_RegMin2;
        //private Byte[] m_VrfMin;
        private Byte[] sourceMatch;
        //private bool m_DeviceOpened;

        private bool _isInitialized;
        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
        }

        public bool BenchmarkMode { get; set; }

        public MemberFingerprint BatchIdentify(MatchRequest toMatch)
        {
            throw new NotImplementedException();
        }

        public MatchResult Identify(byte[] matchWith, int contextId)
        {
            var targetMatch = matchWith;

            if (sourceMatch == null || targetMatch == null)
            {
                //StatusBar.Text = "No data to verify";
                return new MatchResult { Found = false };
            }

            //string[] fingerpos_str = new string[]
            //               {
            //                  "Unknown finger",
            //                  "Right thumb",
            //                  "Right index finger",
            //                  "Right middle finger",
            //                  "Right ring finger",
            //                  "Right little finger",
            //                  "Left thumb",
            //                  "Left index finger",
            //                  "Left middle finger",
            //                  "Left ring finger",
            //                  "Left little finger"};

            Int32 err;
            //SGFPMFingerPosition finger_pos = SGFPMFingerPosition.FINGPOS_UK;
            bool finger_found = false;

            if (m_useAnsiTemplate)
            {
                //SGFPMANSITemplateInfo sample_info = new SGFPMANSITemplateInfo();
                //err = fingerprintManager.GetAnsiTemplateInfo(sourceMatch, sample_info);

                //for (int i = 0; i < sample_info.TotalSamples; i++)
                //{
                //    bool matched = false;
                //    err = fingerprintManager.MatchAnsiTemplate(sourceMatch, i, targetMatch, 0, m_SecurityLevel, ref matched);
                //    if (matched)
                //    {
                //        finger_found = true;
                //        finger_pos = (SGFPMFingerPosition)sample_info.SampleInfo[i].FingerNumber;
                //        break;
                //    }
                //}

                bool matched = false;
                err = fingerprintManager.MatchAnsiTemplate(sourceMatch, 0, targetMatch, 0, m_SecurityLevel, ref matched);
                if (matched)
                {
                    finger_found = true;
                }
            }
            else
            {
                //SGFPMISOTemplateInfo sample_info = new SGFPMISOTemplateInfo();
                //err = fingerprintManager.GetIsoTemplateInfo(sourceMatch, sample_info);

                //for (int i = 0; i < sample_info.TotalSamples; i++)
                //{
                //    bool matched = false;
                //    err = fingerprintManager.MatchIsoTemplate(sourceMatch, i, targetMatch, 0, m_SecurityLevel, ref matched);
                //    if (matched)
                //    {
                //        finger_found = true;
                //        finger_pos = (SGFPMFingerPosition)sample_info.SampleInfo[i].FingerNumber;
                //        break;
                //    }
                //}

                bool matched = false;
                err = fingerprintManager.MatchIsoTemplate(sourceMatch, 0, targetMatch, 0, m_SecurityLevel, ref matched);
                if (matched)
                {
                    finger_found = true;
                }
            }

            if (err == (Int32)SGFPMError.ERROR_NONE)
            {
                if (finger_found)
                {
                    //StatusBar.Text = "The matched data found. Finger position: " + fingerpos_str[(Int32)finger_pos];
                    return new MatchResult { Found = true };
                }
                else
                {
                    //StatusBar.Text = "Cannot find a matched data";
                    //return false;
                }
            }
            else
            {
                if (m_useAnsiTemplate)
                {
                    //StatusBar.Text = "MatchAnsiTemplate() Error : " + err;
                }
                else
                {
                    //StatusBar.Text = "MatchIsoTemplate() Error : " + err;
                }

                //return false;
            }

            return new MatchResult { Found = false };
        }

        private Int32 GetImageFromFile(Byte[] data, string filename)
        {
            //OpenFileDialog open_dlg;
            //open_dlg = new OpenFileDialog();

            //open_dlg.Title = "Image raw file dialog";
            //open_dlg.Filter = "Image raw files (*.raw)|*.raw";

            //if (open_dlg.ShowDialog() == DialogResult.OK)
            //{
            FileStream inStream = File.OpenRead(filename); // open_dlg.FileName);
            BinaryReader br = new BinaryReader(inStream);

            Byte[] local_data = new Byte[data.Length];
            local_data = br.ReadBytes(data.Length);
            Array.Copy(local_data, data, data.Length);

            br.Close();
            return (Int32)SGFPMError.ERROR_NONE;
            //}
            //return (Int32)SGFPMError.ERROR_FUNCTION_FAILED;
        }

        public bool IdentifyPrepare(byte[] toMatch, int contextId)
        {
            sourceMatch = toMatch;
            /*
            Byte[] fp_image = new Byte[m_ImageWidth * m_ImageHeight];
            Int32 error = (Int32)SGFPMError.ERROR_NONE;
            Int32 img_qlty = 0;

            //if (m_DeviceOpened)
            //    error = m_FPM.GetImage(fp_image);
            //else
            //error = GetImageFromFile(fp_image, "");
            toMatch.Fingerprint.CopyTo(fp_image, 0);

            m_FPM.GetImageQuality(m_ImageWidth, m_ImageHeight, fp_image, ref img_qlty);
            //progressBar_V1.Value = img_qlty;

            if (error == (Int32)SGFPMError.ERROR_NONE)
            {
                //DrawImage(fp_image, pictureBoxV1);

                SGFPMFingerInfo finger_info = new SGFPMFingerInfo();
                finger_info.FingerNumber = SGFPMFingerPosition.FINGPOS_UK; // (SGFPMFingerPosition)comboBoxSelFinger.SelectedIndex;
                finger_info.ImageQuality = (Int16)img_qlty;
                finger_info.ImpressionType = (Int16)SGFPMImpressionType.IMPTYPE_LP;
                finger_info.ViewNumber = 1;

                // Create template
                error = m_FPM.CreateTemplate(finger_info, fp_image, m_VrfMin);

                if (error == (Int32)SGFPMError.ERROR_NONE)
                {
                    //StatusBar.Text = "Verification image is captured";
                    return true;
                }
                else
                {
                    //StatusBar.Text = "GetMinutiae() Error : " + error;
                }
            }
            else
            {
                //StatusBar.Text = "GetImage() Error : " + error;
            }
            */
            return true;
        }

        public MatcherInitResult Initialize()
        {
            MatcherInitResult result = new MatcherInitResult();

            m_SecurityLevel = SGFPMSecurityLevel.NORMAL;
            sourceMatch = null;
            m_ImageWidth = 260;
            m_ImageHeight = 300;
            m_Dpi = 500;
            fingerprintManager = new SGFingerPrintManager();

            //SGFPMDeviceName device_name = SGFPMDeviceName.DEV_UNKNOWN;
            int error = fingerprintManager.InitEx(m_ImageWidth, m_ImageHeight, m_Dpi);

            if (error == (Int32)SGFPMError.ERROR_NONE)
            {
                result.Message = "Initialization Success";
                result.Success = true;
            }
            else
            {
                result.Message = "Init() Error " + error;
                return result;
            }

            if (m_useAnsiTemplate)
            {
                // Set template format to ANSI 378
                error = fingerprintManager.SetTemplateFormat(SGFPMTemplateFormat.ANSI378);
            }
            else
            {
                // Set template format to ISO 19794-2
                error = fingerprintManager.SetTemplateFormat(SGFPMTemplateFormat.ISO19794);
            }

            Int32 max_template_size = 0;
            error = fingerprintManager.GetMaxTemplateSize(ref max_template_size);

            //m_RegMin1 = new Byte[max_template_size];
            //m_RegMin2 = new Byte[max_template_size];
            //m_VrfMin = new Byte[max_template_size];

            _isInitialized = true;
            return result;
        }

        public void PreMatch(out int contextId)
        {
            contextId = 0;
        }

        public void PostMatch(int contextId)
        {
            contextId = 0;
        }

        public string WriteError(int errorCode)
        {
            return "";
        }

        public bool IsBatchIdentify { get; set; }

        public List<MemberFingerprint> Fingerprints { get; set; }


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
