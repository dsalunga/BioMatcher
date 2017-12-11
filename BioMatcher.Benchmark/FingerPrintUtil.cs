/*
 -------------------------------------------------------------------------------
 GrFinger Sample
 (c) 2005 Griaule Tecnologia Ltda.
 http://www.griaule.com
 -------------------------------------------------------------------------------

 This sample is provided with "GrFinger Fingerprint Recognition Library" and
 can't run without it. It's provided just as an example of using GrFinger
 Fingerprint Recognition Library and should not be used as basis for any
 commercial product.

 Griaule Tecnologia makes no representations concerning either the merchantability
 of this software or the suitability of this sample for any particular purpose.

 THIS SAMPLE IS PROVIDED BY THE AUTHOR "AS IS" AND ANY EXPRESS OR
 IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 IN NO EVENT SHALL GRIAULE BE LIABLE FOR ANY DIRECT, INDIRECT,
 INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

 You can download the free version of GrFinger directly from Griaule website.
                                                                   
 These notices must be retained in any copies of any part of this
 documentation and/or sample.

 -------------------------------------------------------------------------------
*/

// -----------------------------------------------------------------------------------
// Support and fingerprint management routines
// -----------------------------------------------------------------------------------

using GrFingerXLib;
using System;
using System.IO;
using System.Drawing;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using stdole;
using WCMS.Common.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace FingerPrintBenchMark
{
    // Raw image data type.
    //public struct TRawImage
    //{
    //    // Image data.
    //    public object img;
    //    // Image width.
    //    public int width;
    //    // Image height.
    //    public int height;
    //    // Image resolution.
    //    public int Res;
    //};

    public struct RawFingerPrintImage
    {
        public object Image;
        public int Width;
        public int Height;
        public int Resolution;
    }

    public class FingerPrintTemplate
    {
        public System.Array ImageData;
        public int ImageSize;

        public FingerPrintTemplate()
        {
            ImageData = new byte[100000];
            ImageSize = 0;
        }
    }

    public class CompareResult
    {
        public TimeSpan Elapsed { get; set; }
        public bool Success { get; set; }
        public int Scanned { get; set; }
    }

    public class Util
    {
        public GrFingerXLib.GrFingerXCtrl axGrFingerXCtrl;

        public RawFingerPrintImage RawFPImg;
        public FingerPrintTemplate FPTemplate;

        public List<Array> FingerPrints = new List<Array>();

        // Some constants to make our code cleaner
        public const int ERR_CANT_OPEN_BD = -999;
        public const int ERR_INVALID_ID = -998;
        public const int ERR_INVALID_TEMPLATE = -997;

        //Importing necessary HDC functions
        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        // -----------------------------------------------------------------------------------
        // Support functions
        // -----------------------------------------------------------------------------------

        // This class creates an Util class with some functions
        // to help us to develop our GrFinger Application
        public Util()
        {
        }

        ~Util()
        {
        }

        // Write and describe an error.
        public string WriteError(GrFingerXLib.GRConstants errorCode)
        {
            switch ((int)errorCode)
            {
                case (int)GRConstants.GR_ERROR_INITIALIZE_FAIL:
                    return "Fail to Initialize GrFingerX. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_NOT_INITIALIZED:
                    return "The GrFingerX Library is not initialized. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_FAIL_LICENSE_READ:
                    MessageBox.Show("License not found. See manual for troubleshooting.");
                    return "License not found. See manual for troubleshooting. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_NO_VALID_LICENSE:
                    MessageBox.Show("The license is not valid. See manual for troubleshooting.");
                    return "The license is not valid. See manual for troubleshooting. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_NULL_ARGUMENT:
                    return "The parameter have a null value. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_FAIL:
                    return "Fail to create a GDI object. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_ALLOC:
                    return "Fail to create a context. Cannot allocate memory. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_PARAMETERS:
                    return "One or more parameters are out of bound. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_WRONG_USE:
                    return "This function cannot be called at this time. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_EXTRACT:
                    return "Template Extraction failed. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_SIZE_OFF_RANGE:
                    return "Image is too larger or too short.  (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_RES_OFF_RANGE:
                    return "Image have too low or too high resolution. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_CONTEXT_NOT_CREATED:
                    return "The Context could not be created. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_INVALID_CONTEXT:
                    return "The Context does not exist. (Error:" + errorCode + ")";

                // Capture error codes

                case (int)GRConstants.GR_ERROR_CONNECT_SENSOR:
                    return "Error while connection to sensor. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_CAPTURING:
                    return "Error while capturing from sensor. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_CANCEL_CAPTURING:
                    return "Error while stop capturing from sensor. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_INVALID_ID_SENSOR:
                    return "The idSensor is invalid. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_SENSOR_NOT_CAPTURING:
                    return "The sensor is not capturing. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_INVALID_EXT:
                    return "The File have a unknown extension. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_INVALID_FILENAME:
                    return "The filename is invalid. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_INVALID_FILETYPE:
                    return "The file type is invalid. (Error:" + errorCode + ")";
                case (int)GRConstants.GR_ERROR_SENSOR:
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

        // Check if we have a valid template
        private bool TemplateIsValid()
        {
            // Check the template size and data
            return true; //((ImageData._size > 0) && (ImageData.ImageData != null));
        }

        // -----------------------------------------------------------------------------------
        // Main functions for fingerprint recognition management
        // -----------------------------------------------------------------------------------

        // Initializes GrFinger ActiveX and all necessary utilities.
        public int InitializeGrFinger()//GrFingerXLib.GrFingerXCtrl grfingerx)
        {
            int result = 0;

            axGrFingerXCtrl = new GrFingerXCtrl();

            //axGrFingerXCtrl = grfingerx;

            ////Create a new Template
            if (FPTemplate == null)
                FPTemplate = new FingerPrintTemplate();

            ////Create a new raw image
            RawFPImg = new RawFingerPrintImage();

            //Initialize library
            result = axGrFingerXCtrl.Initialize();
            if (result < 0) return result;

            //return (int)axGrFingerXCtrl.CapInitialize();

            return 0;
        }

        //  Finalizes library and close DB.
        public void FinalizeUtil()
        {
            // finalize library
            axGrFingerXCtrl.Finalize();
            axGrFingerXCtrl.CapFinalize();
        }

        // Display fingerprint image on screen
        public IPictureDisp PrintBiometricDisplay(bool isBiometric, GrFingerXLib.GRConstants contextId)
        {
            // handle to finger image
            IPictureDisp handle = null;
            // screen HDC
            IntPtr hdc = GetDC(System.IntPtr.Zero);

            if (isBiometric)
            {
                // get image with biometric info
                axGrFingerXCtrl.BiometricDisplay(ref FPTemplate.ImageData,
                    ref RawFPImg.Image, RawFPImg.Width, RawFPImg.Height, RawFPImg.Resolution, hdc.ToInt32(),
                    ref handle, (int)contextId);
            }
            else
            {
                // get raw image
                axGrFingerXCtrl.CapRawImageToHandle(ref RawFPImg.Image, RawFPImg.Width,
                    RawFPImg.Height, hdc.ToInt32(), ref handle);

                //Consolidate();
            }

            // draw image on picture box
            if (handle != null)
            {
                return handle;
            }

            // release screen HDC
            ReleaseDC(System.IntPtr.Zero, hdc);

            return null;
        }

        public int ExtractTemplate()
        {
            int result = 0;

            FPTemplate.ImageSize = (int)GRConstants.GR_MAX_SIZE_TEMPLATE;
            result = axGrFingerXCtrl.Extract(ref RawFPImg.Image, RawFPImg.Width, RawFPImg.Height, RawFPImg.Resolution, ref FPTemplate.ImageData, ref FPTemplate.ImageSize, (int)GRConstants.GR_DEFAULT_CONTEXT);

            // if error, set template size to 0
            if (result < 0)
            {
                // Result < 0 => enroll problem
                FPTemplate.ImageSize = 0;
            }
            return result;
        }

        public int SaveToFile(string pathFileName)
        {
            int result = 0;
            result = axGrFingerXCtrl.CapSaveRawImageToFile(ref RawFPImg.Image, RawFPImg.Width, RawFPImg.Height, pathFileName, (int)GRConstants.GRCAP_IMAGE_FORMAT_BMP);

            return result;
        }

        // Add a fingerprint template to database
        public int Enroll()
        {
            int id = 0;
            // Checks if template is valid.
            if (TemplateIsValid())
            {
                // Adds template to database and returns template ID.
                return id;
            }
            else
            {
                return -1;
            }
        }

        // Show GrFinger version and type
        public void MessageVersion()
        {
            byte majorVersion = 0, minorVersion = 0;
            GRConstants result;
            string vStr = "";

            result = (GRConstants)axGrFingerXCtrl.GetGrFingerVersion(ref majorVersion,
                ref minorVersion);
            if (result == GRConstants.GRFINGER_FULL)
                vStr = "FULL";
            else if (result == GRConstants.GRFINGER_LIGHT)
                vStr = "LIGHT";

            MessageBox.Show("The GrFinger DLL version is " +
                majorVersion + "." + minorVersion + ". \n" +
                "The license type is '" + vStr + "'.", "GrFinger Version");
        }

        public string CacheFingerPrints()
        {
            var dataDir = Path.Combine(Application.StartupPath, "Data");
            if (Directory.Exists(dataDir))
            {
                var files = Directory.EnumerateFiles(dataDir);
                foreach (var file in files)
                {
                    FingerPrints.Add(File.ReadAllBytes(file));
                }

                return "Fingerprints loaded from data folder.";
            }
            else
            {
                var cmd = "SELECT [MemberFingerPrintID],[MemberID],[FingerPrint1],[FingerPrint2] FROM [MemberFingerPrints]";
                using (var r = SqlHelper.ExecuteReader(System.Data.CommandType.Text, cmd))
                {
                    int i = 0;
                    var dataDirCreated = false;

                    while (r.Read())
                    {
                        if (!r.IsDBNull(2))
                        {
                            i++;

                            var print = (byte[])r[2];
                            if (!dataDirCreated)
                            {
                                Directory.CreateDirectory(dataDir);
                            }

                            File.WriteAllBytes(Path.Combine(dataDir, string.Format("{0}.dat", i.ToString("D4"))), print);

                            FingerPrints.Add(print);
                            if (i == 100)
                                break;
                        }
                    }
                }

                return "Fingerprints loaded from DB and persisted to data folder.";
            }
        }

        public GRConstants IdentifyPrepare(Array toMatch, int contextId = -999)
        {
            GRConstants result = (GRConstants)axGrFingerXCtrl.IdentifyPrepare(ref toMatch,
                contextId == -999 ? (int)GRConstants.GR_DEFAULT_CONTEXT : contextId);

            return result;
        }

        public CompareResult Identify(int contextId = -999)
        {
            GRConstants result;
            //FingerPrintTemplate tptRef;
            int score = -1;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Checking if template is valid.
            //if (!TemplateIsValid()) return (long)ERR_INVALID_TEMPLATE;
            // Starting identification process and supplying query template.
            //result = (GRConstants)axGrFingerXCtrl.IdentifyPrepare(ref toMatchArray,
            //    (int)GRConstants.GR_DEFAULT_CONTEXT);
            // error?
            //if (result < 0) return (long)result;
            // Getting enrolled templates from database.

            bool found = false;
            int i = 0;
            var ctxId = contextId == -999 ? (int)GRConstants.GR_DEFAULT_CONTEXT : contextId;

            foreach (var f in FingerPrints)
            {
                Array fArray = f;
                i++;
                // Comparing current template.
                result = (GRConstants)axGrFingerXCtrl.Identify(ref fArray, ref score, ctxId);

                // Checking if query template and the reference template match.
                if (result == GRConstants.GR_MATCH)
                {
                    //memFP.MemberFingerPrintID = ds.DataReader.GetInt64(0);
                    //memFP.MemberID = ds.DataReader.GetInt64(1);

                    //ds.Dispose();
                    //return -1; //memFP.MemberID;

                    found = true;
                    break;
                }
            }

            sw.Stop();

            return new CompareResult { Elapsed = sw.Elapsed, Scanned = i, Success = found };
        }

        // Identify current fingerprint on our database
        public long Identify(ref int score)
        {
            GRConstants result;
            FingerPrintTemplate tptRef;

            // Checking if template is valid.
            if (!TemplateIsValid()) return (long)ERR_INVALID_TEMPLATE;
            // Starting identification process and supplying query template.
            result = (GRConstants)axGrFingerXCtrl.IdentifyPrepare(ref FPTemplate.ImageData,
                (int)GRConstants.GR_DEFAULT_CONTEXT);
            // error?
            if (result < 0) return (long)result;
            // Getting enrolled templates from database.

            var cmd = "SELECT [MemberFingerPrintID],[MemberID],[FingerPrint1],[FingerPrint2] FROM [MemberFingerPrints]";
            using (var r = SqlHelper.ExecuteReader(cmd))
            {
                while (r.Read())
                {
                    long readBytes;
                    byte[] temp;

                    if (!r.IsDBNull(2))
                    {
                        temp = new byte[10000];
                        readBytes = r.GetBytes(2, 0, temp, 0, temp.Length);

                        tptRef = new FingerPrintTemplate();
                        System.Array.Copy(temp, 0, tptRef.ImageData, 0, (int)readBytes);

                        // Comparing current template.
                        result = (GRConstants)axGrFingerXCtrl.Identify(ref tptRef.ImageData, ref score, (int)GRConstants.GR_DEFAULT_CONTEXT);

                        // Checking if query template and the reference template match.
                        if (result == GRConstants.GR_MATCH)
                        {
                            //memFP.MemberFingerPrintID = ds.DataReader.GetInt64(0);
                            //memFP.MemberID = ds.DataReader.GetInt64(1);

                            //ds.Dispose();
                            return -1; //memFP.MemberID;
                        }
                    }
                    if (!r.IsDBNull(3))
                    {
                        temp = new byte[10000];
                        readBytes = r.GetBytes(3, 0, temp, 0, temp.Length);
                        tptRef = new FingerPrintTemplate();
                        System.Array.Copy(temp, 0, tptRef.ImageData, 0, (int)readBytes);

                        result = (GRConstants)axGrFingerXCtrl.Identify(ref tptRef.ImageData, ref score, (int)GRConstants.GR_DEFAULT_CONTEXT);

                        if (result == GRConstants.GR_MATCH)
                        {
                            //memFP.MemberFingerPrintID = ds.DataReader.GetInt64(0);
                            //memFP.MemberID = ds.DataReader.GetInt64(1);

                            //ds.Dispose();
                            return -1; //memFP.MemberID;
                        }
                    }
                }
            }
            //ds.Dispose();

            // Closing recordset.
            return (long)GRConstants.GR_NOT_MATCH;
        }
    };
}