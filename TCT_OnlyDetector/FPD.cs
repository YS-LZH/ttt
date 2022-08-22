using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using TCT_OnlyDetector.Native;
using static TCT_OnlyDetector.Interface;
using static TCT_OnlyDetector.Acquisition;

namespace TCT_OnlyDetector
{
    partial class FPD
    {

        #region Data Members
        public static string _currentStatus = "";//Used to keep the Current status
        public static int _prograss = 0;//Used to keep current percentage
        public static List<IntPtr> ImageAddrList;//Used to keep ImageAddress
        //public static string IMAGERS_DIR_PATH = Interface.settings.DetectorPath;//must be set to the path to the directory containing information about the receptor
        public static string IMAGERS_DIR_PATH = @"C:\IMAGERs\234S02-0809";//must be set to the path to the directory containing information about the receptor
        public const int VIP_NO_ERR = 0;//A value used to indicate the success status value of detector, which could not be modify during run time.
        public const int RECORD_TIMEOUT = 30;//A value used to set timeout period while recording on detector, which could not be modify during run time.
        static int GImgX = 0;//The number of lines per frame of an image i.e. the vertical resolution of the mode.
        static int GImgY = 0;//The number of lines per frame of an image i.e. the horizontal resolution of the mode.
        public static int GImgSize = 0;//The image size in bytes
        public static long GNumFrames = 0;//Keep the number of frames the detector already get.
        public static int dummyFrame = 10;//A value used to set the number of dummy frames used in sync board.
        public static string LogFileName = @"c:\TCT\TCTLog\" + "FPD.ini";//File to be write FactoryMode log
        public static int SavedTempRawCount = 0;//已儲存temp raw張數_used for real time display
        public static bool UserCancelACQProcess = false;//Flag used for judge if want to cancel peocess
        static string spaceName = MethodInfo.GetCurrentMethod().Module.Name;//used for log
        static string className = MethodInfo.GetCurrentMethod().ReflectedType.Name;//used for log
        public static SOpenReceptorLink orl = new SOpenReceptorLink();
        //public static XRAYStatus XRAYEmitOn;//This represent the XRAY-on status which is used to compare to current status we get from E1, E8 and E9 commands from system on frm_tACQ.CurrentErrorCode.XRAYStatus
        static string EOF = "==============================EOF===============================";//used for log
        static bool usetNaruto = false;//this is not use on calibration mode even if this is set to ture.
        //public static int totalraw = 0;//A value used to keep the number of raw files been saved in disk._used for thread save
        //static List<SaveImage> SI;//A SaveImage class list used to keep image information to be saved on threadlist_used for thread save
        //static List<Thread> threadList;//A thread list used to keep threads to do save image work._used for thread save
        public static int PadLength = 44;
        public enum DetectorPaxScan_1313D_1313DX
        {
            DetectorSizeX = 130,
            DetectorSizeY = 130,
            FrameSizeX = 512,
            FrameSizeY = 512,
        }
        public enum DetectorPaxScan_2520D_2520DX
        {
            DetectorSizeX = 200,
            DetectorSizeY = 250,
            FrameSizeX = 768,
            FrameSizeY = 960,
        }
        public enum HighGain
        {
            Varian1313DX = 16000,
            Varian1313D = 4800,
            Varian2520D = 4800
        };
        public enum LowGain
        {
            Varian1313DX = 8000,
            Varian1313D = 1600,
            Varian2520D = 1600
        };
        #endregion
        /// <summary>This class is used to define detector parameters including Mode, FrameNumber, FrameRate, and UserSync. Those parameters are used to create specific detector settings for different acquisition processes.</summary>
        public class FPDPara
        {
            public int Mode;
            public int FrameNumber;
            public int FrameRate;
            public bool UserSync;
            /// <summary>Initialize a new instance of the FPDPara class with default parameter.</summary>
            public FPDPara()
            {
                this.Mode = 0;
                this.FrameNumber = 310;
                this.FrameRate = 30;
                this.UserSync = true;
            }
            /// <summary>Initialize a new instance of the FPDPara class with specific parameter.</summary>
            public FPDPara(int Mode, int FrameNumber, int FrameRate, bool UserSync)
            {
                this.Mode = Mode;
                this.FrameNumber = FrameNumber;
                this.FrameRate = FrameRate;
                this.UserSync = UserSync;
            }
        }
        #region FPD Acquisition process
        /*
        /// <summary>FPD Acquisition process -Scout view and Contineous mode.</summary>
        public static int RunFluoroWithoutDetector(FPDPara FPDPara, SerialPort SerialPort)
        {
            int result = (int)ReturnCode.Success;
            int _waitCount = 0;
            frm_tACQ.WaitA4 = true;
            frm_tACQ.WaitA7 = true;
            if (frm_Acquisition.doRaliabilityTest)
            {
                if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                    _currentStatus = "準備執行試運轉拍攝";
                else
                    _currentStatus = "Wait for system start acquisition.";//ProcessBar提示使用者
            }
            else if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                _currentStatus = "請按下控制盒上的SCAN鍵進行拍攝";
            else
                _currentStatus = "Please Press SCAN Button on the control box to Start.";//ProcessBar提示使用者
            _prograss += 3;
            while (!frm_Acquisition.StartACQ)
            {
                Thread.Sleep(10);
                if (frm_Acquisition.GetHWStart)
                {
                    if (_waitCount > 10000)
                    {
                        ErrorAction(true, "Time out wait for serialPort response.");
                        frm_Acquisition.StartACQ = false;
                        frm_Acquisition.GetHWStart = false;
                        return result = (int)ReturnCode.FPD_TIMEOUT_SYNC_BD_ERR;
                    }
                    if (!frm_Acquisition.SystemLightStatus)
                    {
                        ErrorAction(true, "System Error!");
                        frm_Acquisition.StartACQ = false;
                        frm_Acquisition.GetHWStart = false;
                        return result = (int)ReturnCode.FPGA_System_Error;
                    }
                    if (!frm_Acquisition.XRAYLightStatus)
                    {
                        if (Interface.settings.XRAYControlBySW)
                        {
                            if (frm_tACQ.Xray == frm_tACQ.XRAYType.VJT)
                            {
                                if (!tXRAY.VJT.xray_on)//(!frm_tACQ.CurrentErrorCode.XRAYStatus.XRayOnStatus)
                                {
                                    ErrorAction(true, "XRAY Error!");
                                    frm_Acquisition.StartACQ = false;
                                    frm_Acquisition.GetHWStart = false;
                                    return result = (int)ReturnCode.FPGA_XRAY_Ready_Status_Error;
                                }
                                else
                                {
                                    //忽略
                                }
                            }
                            else if (frm_tACQ.Xray == frm_tACQ.XRAYType.SPM)
                            {
                                if (!tXRAY.SPM.xray_on)
                                {
                                    ErrorAction(true, "XRAY Error!");
                                    frm_Acquisition.StartACQ = false;
                                    frm_Acquisition.GetHWStart = false;
                                    return result = (int)ReturnCode.FPGA_XRAY_Ready_Status_Error;
                                }
                                else
                                {
                                    //忽略
                                }
                            }
                            else
                            {
                                ErrorAction(true, "XRAY Error!");
                                frm_Acquisition.StartACQ = false;
                                frm_Acquisition.GetHWStart = false;
                                return result = (int)ReturnCode.FPGA_XRAY_Ready_Status_Error;
                            }
                        }
                        else
                        {
                            ErrorAction(true, "XRAY Error!");
                            frm_Acquisition.StartACQ = false;
                            frm_Acquisition.GetHWStart = false;
                            return result = (int)ReturnCode.FPGA_XRAY_Ready_Status_Error;
                        }
                    }
                    //if (frm_tACQ.CurrentErrorCode.MotorSensor.DetectOnRightSideStop)
                    //{
                    //    ErrorAction(true, "Detector Right Motor Error!");
                    //    frm_Acquisition.StartACQ = false;
                    //    frm_Acquisition.GetHWStart = false;
                    //    return result = (int)ReturnCode.FPGA_Detect_OnRightSide_Stop;
                    //}
                    //if (frm_tACQ.CurrentErrorCode.MotorSensor.DetectOnLeftSideStop)
                    //{
                    //    ErrorAction(true, "Detector Left Motor Error!");
                    //    frm_Acquisition.StartACQ = false;
                    //    frm_Acquisition.GetHWStart = false;
                    //    return result = (int)ReturnCode.FPGA_Detect_OnLeftSide_Stop;
                    //}
                    if (frm_Acquisition.doRaliabilityTest)
                    {
                        _waitCount += 10;
                        if (_waitCount % 1000 == 0)
                        {
                            _currentStatus += "..";//ProcessBar提示使用者.
                            _prograss++;
                        }
                    }
                    else 
                        _waitCount += 10;
                    if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                        _currentStatus = "準備開啟X-Ray";
                    else
                        _currentStatus = "Ready to Start X-Ray..";
                    #region VJT&&SPM
                    if (Interface.settings.XRAYControlBySW)
                    {
                        if (frm_tACQ.Xray == frm_tACQ.XRAYType.VJT)//For VJT, Xray is enable through hardware
                        {
                            tXRAY.VJT.XRayON_flag = true;
                        }
                        if (frm_tACQ.Xray == frm_tACQ.XRAYType.SPM)
                        {
                            tXRAY.SPM.XRayON_flag = true;
                        }
                    }
                    #endregion
                }
                if (_waitCount > 0 && _waitCount % 1000 == 0)
                {
                    _prograss += 1;
                }

                //CheckReturn = CheckCancel(true,false);
                //if (CheckReturn != (int)ReturnCode.Success)
                //    return CheckReturn;
            }

            frm_Acquisition.StartACQ = false;
            frm_Acquisition.GetHWStart = false;
            return result;

        }
        */
        public static int RunFluoro(FPDPara FPDPara, SerialPort SerialPort)
        {
            int numBuf = 0, numFrm = 0, acqType = -1;
            int result = (int)ReturnCode.Success;
            int CheckReturn = (int)ReturnCode.Success;
            //  XRAYEmitOn = new XRAYStatus(FPGA.Status.Emit_On);
            int passSaveRawCount = dummyFrame;
            int N = FPDPara.Mode;//we are using mode 0. if not it can be modified to select another mode..// N = 1;// result = vip_select_mode(N);//(automatically generates vip_fluoro_init_mode(N, ..)
            string methodName = MethodInfo.GetCurrentMethod().Name;
            string LogFormatedString = (className + "." + methodName).PadRight(PadLength);
            DateTime _beginTime = DateTime.Now;
            DateTime _endTime = DateTime.Now;
            SCorrections corr = new SCorrections();
            SModeInfo modeInfo = new SModeInfo();
            NativeConstants.SSeqPrms seqPrms = new NativeConstants.SSeqPrms();
            NativeConstants.SAcqPrms acqPrms = new NativeConstants.SAcqPrms();
            NativeConstants.SLivePrms GLiveParamsS = new NativeConstants.SLivePrms();
            ImageAddrList = new List<IntPtr>();

            LogFile.Log(LogFile.LogFileName, "----------------------------------------------------------------" + "\n------------------- FPD Acquisition process --------------------" + "\n----------------------------------------------------------------");

            _currentStatus = "準備執行拍攝";
            _prograss = 0;//歸零process
            vip_set_debug(1);//開啟debug功能
            #region STEP 1 -- Open the link _ vip_open_receptor_link
            LogFile.Log(LogFile.LogFileName, "\n**** STEP 1 -- Open the link -- on: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            CheckReturn = 0;
            if (CheckReturn != (int)ReturnCode.Success)
                return CheckReturn;
            else
            {
                orl.StructSize = Marshal.SizeOf(typeof(SOpenReceptorLink));
                orl.RecDirPath = IMAGERS_DIR_PATH;
                orl.DebugMode = 1;

                //retry for 3 times
                for (int i = 0; i < 3; i++)
                {
                    _currentStatus = "準備執行拍攝";
                    _prograss += 5;//歸零process
                    result = vip_open_receptor_link(ref orl);
                    if (result == VIP_NO_ERR)
                    {
                        //if (frm_tACQ.Detector == DetectorType.PaxScan1313DX && Utility.FPGA.setting.Rotate)
                        {
                            corr.StructSize = Marshal.SizeOf(typeof(SCorrections));
                            corr.Rotate90 = 1;
                            corr.FlipX = 1;
                            corr.FlipY = 1;
                            corr.Dfct = true;
                            corr.Gain = true;
                            corr.Ofst = true;
                            result = vip_set_correction_settings(ref corr); LogFile.Log(LogFile.LogFileName, "vip_open_receptor_link result: " + result.ToString());
                            break;
                        }/*
                        else if (frm_tACQ.Detector == frm_tACQ.DetectorType.PaxScan2520DX)//For 2520D, we have to use flip function
                        {
                            corr.StructSize = Marshal.SizeOf(typeof(SCorrections));
                            //corr.Rotate90 = 1;
                            corr.FlipX = 1;
                            corr.Dfct = true;
                            corr.Gain = true;
                            corr.Ofst = true;
                            result = vip_set_correction_settings(ref corr); LogFile.Log(LogFile.LogFileName, "vip_open_receptor_link result: " + result.ToString());
                            break;
                        }
                        else
                        {
                            result = vip_set_correction_settings(ref corr); LogFile.Log(LogFile.LogFileName, "vip_open_receptor_link result: " + result.ToString());
                            break;
                        }*/
                    }
                    if (result == NativeConstants.HCP_OFST_ERR || result == NativeConstants.HCP_GAIN_ERR || result == NativeConstants.HCP_DFCT_ERR)
                    {
                        _prograss += 3;
                        corr.StructSize = Marshal.SizeOf(typeof(SCorrections));
                        result = vip_set_correction_settings(ref corr);
                        if (result != VIP_NO_ERR)
                        {
                            ErrorAction(false, "vip_set_correction_settings result: " + result.ToString());
                            return result = (int)ReturnCode.FPD_SET_CORRECTION_SETTINGS_ERR;
                        }
                        LogFile.Log(LogFile.LogFileName, "vip_set_correction_settings result: " + result.ToString());
                        break;
                    }

                    if (result == NativeConstants.HCP_PLR_CONNECT)
                    {
                        LogFile.Log(LogFile.LogFileName, "Receptor not found during open link for " + (i + 1).ToString() + "/3");
                    }
                    if (result == NativeConstants.HCP_RECID_CONFLICT)//斷線重連
                    {
                        vip_close_link();
                        LogFile.Log(LogFile.LogFileName, "Receptor already open link, close and retry " + (i + 1).ToString() + "/3");
                    }
                    if (i == 2)
                    {
                        _currentStatus = ErrStrList[(int)ReturnCode.FPD_OPEN_RECEPTOR_LINK_ERR];
                        _prograss = 0;
                        LogFile.Log(LogFile.LogFileName, EOF);
                        return result = (int)ReturnCode.FPD_OPEN_RECEPTOR_LINK_ERR;
                    }
                }
                //result = vip_open_receptor_link(ref orl);
                //if (result == NativeConstants.HCP_OFST_ERR || result == NativeConstants.HCP_GAIN_ERR || result == NativeConstants.HCP_DFCT_ERR)
                //{
                //    _prograss += 3;
                //    corr.StructSize = Marshal.SizeOf(typeof(SCorrections));
                //    result = vip_set_correction_settings(ref corr);
                //    if (result != VIP_NO_ERR)
                //    {
                //        ErrorAction(false, "vip_set_correction_settings result: " + result.ToString());
                //        return result = (int)ReturnCode.FPD_SET_CORRECTION_SETTINGS_ERR;
                //    }
                //    LogFile.Log(LogFile.LogFileName, "vip_set_correction_settings result: " + result.ToString());
                //}
                //if (result == NativeConstants.HCP_PLR_CONNECT)
                //{
                //    ErrorAction(false, "Receptor not found during open link");
                //    return result = (int)ReturnCode.FPD_OPEN_RECEPTOR_LINK_ERR;
                //}
                //LogFile.Log(LogFile.LogFileName, "vip_open_receptor_link result: " + result.ToString());
            }
            #endregion
            #region STEP 2 -- Get mode info _ vip_get_mode_info, CheckRecLink()
            #region vip_get_mode_info
            CheckReturn = CheckCancel(true, false);
            if (CheckReturn != (int)ReturnCode.Success)
                return CheckReturn;
            else
            {
                LogFile.Log(LogFile.LogFileName, "\n**** STEP 2 -- Get mode info --");
                modeInfo.StructSize = Marshal.SizeOf(typeof(SModeInfo));
                _prograss += 3;
                result = vip_get_mode_info(N, ref modeInfo);
                if (result == VIP_NO_ERR)
                {
                    LogFile.Log(LogFile.LogFileName, "vip_get_mode_info returned " + result.ToString());
                    LogFile.Log(LogFile.LogFileName, "modeInfo.AcqType is " + modeInfo.AcqType.ToString());
                    GImgX = modeInfo.ColsPerFrame;
                    GImgY = modeInfo.LinesPerFrame;
                    LogFile.Log(LogFile.LogFileName, "<Image Size>: GImgX = " + GImgX + ", GImgY = " + GImgY + "; <Mode>:" + modeInfo.ModeNum);
                    GImgSize = GImgX * GImgY * sizeof(ushort); // image size in bytes
                    acqType = modeInfo.AcqType & NativeConstants.VIP_ACQ_MASK;
                    LogFile.Log(LogFile.LogFileName, "<Acquisition Type> " + acqType + "; <UserSync> " + modeInfo.UserSync);
                    if (acqType != NativeConstants.VIP_ACQ_TYPE_CONTINUOUS)// make sure we have a fluoro mode
                    {
                        ErrorAction(true, "Error: Invalid mode type returned by vip_get_mode_info().");
                        return result = (int)ReturnCode.FPD_INVALID_MODE_TYPE;
                    }
                    int maxSize = (512 * 512) * sizeof(ushort);
                    if (GImgSize <= 0 || GImgSize > maxSize)
                    {
                        ErrorAction(true, "Error: Invalid image size returned by vip_get_mode_info().");
                        return result = (int)ReturnCode.FPD_INVALID_IMAGE_SIZE;
                    }
                }
                else
                {
                    ErrorAction(true, "vip_get_mode_info returned " + result.ToString());
                    return result = (int)ReturnCode.FPD_GET_MODE_INFO_ERR;
                }
            }
            #endregion
            #region  do calibration if required
            int doCal = 0;
            if (doCal == 1)
                result = DoCal(N, modeInfo.CalFrmCount);
            #endregion
            #region CheckRecLink
            CheckReturn = CheckCancel(true, false);
            if (CheckReturn != (int)ReturnCode.Success)
                return CheckReturn;
            else
            {
                result = CheckRecLink();// check the receptor link is OK
                LogFile.Log(LogFile.LogFileName, "CheckRecLink result: " + result.ToString());
                if (result != VIP_NO_ERR)
                {
                    ErrorAction(true, "vip_check_link returned " + result.ToString());
                    return result = (int)ReturnCode.FPD_CHECK_REC_LINK_ERR;
                }
            }
            #endregion
            #endregion
            #region STEP 3 -- Set sequence info
            CheckReturn = CheckCancel(true, false);
            if (CheckReturn != (int)ReturnCode.Success)
                return CheckReturn;
            else
            {
                LogFile.Log(LogFile.LogFileName, "\n**** STEP 3 -- Set frame_rate, user_sync, sequence info --");
                /* Get some user input as to how many buffers to allocate and how many frames to acquire. Note that zero is valid for 
                    number of frames to acquire - it is interpreted as continuous acquisition, terminated  by vip_fluoro_record_stop() or 
                    vip_fluoro_grabber_stop(). */
                #region Set frame_rate
                //if (frm_Acquisition.acquireMode == frm_Acquisition.AcquireMode.Pulse_Full_Asymmetric)
                //{
                //    numBuf = FPDPara.FrameNumber + 12;
                //    numFrm = FPDPara.FrameNumber + 12;
                //}
                //else if (frm_Acquisition.acquireMode == frm_Acquisition.AcquireMode.Pulse_Full_Symmetric || frm_Acquisition.acquireMode == frm_Acquisition.AcquireMode.Pulse_Half_Symmetric)
                //{
                //    numBuf = FPDPara.FrameNumber + 9;
                //    numFrm = FPDPara.FrameNumber + 9;
                //}
                //else
                //{
                numBuf = FPDPara.FrameNumber + dummyFrame;
                numFrm = FPDPara.FrameNumber + dummyFrame;
                //}
                //if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                _currentStatus = "準備拍攝";
                _prograss += 3;
                result = vip_set_frame_rate(N, FPDPara.FrameRate);// Set framerate
                LogFile.Log(LogFile.LogFileName, "vip_set_frame_rate : " + FPDPara.FrameRate.ToString() + ". Result : " + result.ToString());
                _prograss += 3;
                #endregion
                #region For Test
                //result = vip_set_mode_acq_type(N, NativeConstants.VIP_CB_AUX_MODE_FLAG, 1);
                //if (result != VIP_NO_ERR)
                //{
                //    #region status update
                //    _currentStatus = "vip_set_mode_acq_type returned " + result.ToString();
                //    _prograss = 0;
                //    #endregion
                //    LogFile.Log(LogFile.LogFileName, "vip_set_mode_acq_type returned " + result.ToString());
                //    return result;
                //}
                #endregion
                #region Set user_sync
                if (FPDPara.UserSync)
                {
                    _prograss += 3;
                    result = vip_set_user_sync(N, FPDPara.UserSync);// Set USER_SYNC enable
                    result = vip_get_mode_info(N, ref modeInfo);
                    LogFile.Log(LogFile.LogFileName, "vip_get_mode_info returned " + result.ToString());
                    LogFile.Log(LogFile.LogFileName, "<<UserSync>> " + modeInfo.UserSync);
                    if (modeInfo.UserSync != FPDPara.UserSync)
                    {
                        ErrorAction(true, "Error: Fail to vip_set_user_sync.");
                        return result = (int)ReturnCode.FPD_SET_USER_SYNC_ERR;
                    }
                }
                #endregion
                #region Set sequence info
                seqPrms.StructSize = Marshal.SizeOf(typeof(NativeConstants.SSeqPrms));
                seqPrms.NumBuffers = numBuf;
                seqPrms.StopAfterN = numFrm; // zero is interpreted as acquire continuous (writing to buffers in circular fashion)
                _prograss += 3;
                result = vip_fluoro_set_prms((int)NativeConstants.HcpFluoroStruct.HCP_FLU_SEQ_PRMS, ref seqPrms);
                if (result != VIP_NO_ERR)
                {
                    ErrorAction(true, "nvip_fluoro_set_prms returned " + result.ToString());
                    return result = (int)ReturnCode.FPD_SET_PRMS_ERR;
                }
                else
                {
                    numBuf = seqPrms.NumBuffers;
                    if (numFrm > numBuf)// we may not get all we want so ...
                        numFrm = numBuf;
                    LogFile.Log(LogFile.LogFileName, "Number of buffers allocated = " + numBuf.ToString());
                }
                #endregion
            }
            #endregion
            #region STEP 4 -- Send command to system and Start the grabber
            LogFile.Log(LogFile.LogFileName, "\n**** STEP 4 -- Send command to system and Start the grabber --");
            int paused = 1;// the grabber can be started 'PAUSED' in which case it will not be writing to the grab buffers until vip_fluoro_record_start() is called (not really significant here)
            int markPix = 0;// When the MarkPixels is set, a line of pixels is overwritten in the grabber buffers. The line length is the same as the frame number. This should NOT be set in normal useage but can be useful for debug and verification.
            acqPrms.StructSize = Marshal.SizeOf(typeof(NativeConstants.SAcqPrms));
            acqPrms.MarkPixels = markPix;
            acqPrms.StartUp = paused;
            acqPrms.CorrType = (int)NativeConstants.HcpCorrType.HCP_CORR_STD;
            acqPrms.CorrFuncPtr = IntPtr.Zero; // This should essentially always be set to NULL. 
            acqPrms.ReqType = 0; // ReqType is for internal use only. 
            _prograss += 3;
            #region Note
            // This will be the normal setting. 
            // Only other option currently
            // is HCP_CORR_NONE. Setting CorrType=HCP_CORR_NONE 
            // effectively acts as an override to the settings in SCorrections.
            // Note that the various calls, that may return  
            // HCP_OFST_ERR / HCP_GAIN_ERR / HCP_DFCT_ERR etc,
            // do not take account of the CorrType setting. i.e if it is set to
            // HCP_CORR_NONE, corrections will not be done but an error code
            // could be returned if corrections are otherwise 
            // requested but not available.
            // For clarity we also set the following but actually these settings
            // are the defaults done by the memset above.
            // Only if the user is responsible for making his 
            // own corrections should the user set this.
            // Consult Varian Medical Systems Engineering staff.
            // The NULL value is interpreted as pointing
            // corrections into the Virtual CP integrated
            // corrections module.
            // Must be left at zero always.
            // just before starting the grabber we want to make sure that 
            // we are calibrated and have correction files to do what's requested
            // primarily interested that we get back HCP_NO_ERR..
            // HCP_NO_ERR - all requested corrections are available
            // HCP_OFST_ERR - no corrections are available
            // HCP_GAIN_ERR - of requested corrections only offset is available
            // HCP_DFCT_ERR - of requested corrections only offset and gain are available
            // Note that:--
            // --This call is made here to determine whether 
            // we have the requested correction capability. The return value 
            // MUST be HCP_NO_ERR before an acquisition is done.
            // --Currently requested corrections are those set in the receptor
            // configuration file or the last call to vip_set_correction_settings.
            // --The values returned in the SCorrections returned above reflect 
            // currently requested corrections. The return value reflects whether 
            //they are all available. 
            #endregion
            #region test code for vip_get_correction_settings
            //result = vip_get_correction_settings(ref corr);
            //LogFile.Log(LogFile.LogFileName, "Rotate90=" + corr.Rotate90.ToString());
            //LogFile.Log(LogFile.LogFileName, "Ofst=" + corr.Ofst.ToString());
            //LogFile.Log(LogFile.LogFileName, "Gain=" + corr.Gain.ToString());
            //LogFile.Log(LogFile.LogFileName, "Dfct=" + corr.Dfct.ToString());
            //LogFile.Log(LogFile.LogFileName, "Line=" + corr.Line.ToString());
            //LogFile.Log(LogFile.LogFileName, "PixDataFormat=" + corr.PixDataFormat.ToString());
            #endregion
            #region Wait Hardware start (FPGA) or Send ACQ command (sync board)
            int _waitCount = 0;//ms
            CheckReturn = CheckCancel(true, false);
            if (CheckReturn != (int)ReturnCode.Success)
                return CheckReturn;
            else
            {
                Interface.GetSyncBoardError = false;
                //if (Interface.settings.FPGA)
                {
                //    frm_tACQ.WaitA4 = true;
                //    frm_tACQ.WaitA7 = true;
                //    if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                //        _currentStatus = "請按下控制盒上的SCAN鍵進行拍攝";
                //    else
                //        _currentStatus = "Please Press SCAN Button on the control box to Start.";//ProcessBar提示使用者
                //    _prograss += 3;
                //    while (!frm_Acquisition.StartACQ)
                //    {
                //        Thread.Sleep(10);
                //        if (frm_Acquisition.GetHWStart)
                //        {
                //            if (_waitCount > 10000)
                //            {
                //                ErrorAction(true, "Time out wait for serialPort response.");
                //                frm_Acquisition.StartACQ = false;
                //                frm_Acquisition.GetHWStart = false;
                //                return result = (int)ReturnCode.FPD_TIMEOUT_SYNC_BD_ERR;
                //            }
                //            if (!frm_Acquisition.SystemLightStatus)
                //            {
                //                ErrorAction(true, "System Error!");
                //                frm_Acquisition.StartACQ = false;
                //                frm_Acquisition.GetHWStart = false;
                //                return result = (int)ReturnCode.FPGA_System_Error;
                //            }
                //            if (!frm_Acquisition.XRAYLightStatus)
                //            {
                //                if (Interface.settings.XRAYControlBySW)
                //                {
                //                    if (frm_tACQ.Xray == frm_tACQ.XRAYType.VJT)
                //                    {

                //                        if (!tXRAY.VJT.xray_on)//(!frm_tACQ.CurrentErrorCode.XRAYStatus.XRayOnStatus)
                //                        {
                //                            ErrorAction(true, "XRAY Error!");
                //                            frm_Acquisition.StartACQ = false;
                //                            frm_Acquisition.GetHWStart = false;
                //                            return result = (int)ReturnCode.FPGA_XRAY_Ready_Status_Error;
                //                        }
                //                        else
                //                        {
                //                            //忽略
                //                        }

                //                    }
                //                    else if (frm_tACQ.Xray == frm_tACQ.XRAYType.SPM)
                //                    {
                //                        if (!tXRAY.SPM.xray_on)
                //                        {
                //                            ErrorAction(true, "XRAY Error!");
                //                            frm_Acquisition.StartACQ = false;
                //                            frm_Acquisition.GetHWStart = false;
                //                            return result = (int)ReturnCode.FPGA_XRAY_Ready_Status_Error;
                //                        }
                //                        else
                //                        {
                //                            //忽略
                //                        }
                //                    }
                //                    else
                //                    {
                //                        ErrorAction(true, "XRAY Error!");
                //                        frm_Acquisition.StartACQ = false;
                //                        frm_Acquisition.GetHWStart = false;
                //                        return result = (int)ReturnCode.FPGA_XRAY_Ready_Status_Error;
                //                    }
                //                }
                //                else
                //                {
                //                    ErrorAction(true, "XRAY Error!");
                //                    frm_Acquisition.StartACQ = false;
                //                    frm_Acquisition.GetHWStart = false;
                //                    return result = (int)ReturnCode.FPGA_XRAY_Ready_Status_Error;
                //                }

                //            }
                //            if (frm_tACQ.CurrentErrorCode.MotorSensor.DetectOnRightSideStop)
                //            {
                //                //MessageBox.Show("FPGA_Detect_OnRightSide_Stop");
                //                /* bypass by han 2021/05/26
                //                ErrorAction(true, "Detector Right Motor Error!");
                //                frm_Acquisition.StartACQ = false;
                //                frm_Acquisition.GetHWStart = false;
                //                return result = (int)ReturnCode.FPGA_Detect_OnRightSide_Stop;*/
                //            }
                //            if (frm_tACQ.CurrentErrorCode.MotorSensor.DetectOnLeftSideStop)
                //            {
                //                /*
                //                 * bypass by han 2021/05/26
                //                ErrorAction(true, "Detector Left Motor Error!");
                //                frm_Acquisition.StartACQ = false;
                //                frm_Acquisition.GetHWStart = false;
                //                return result = (int)ReturnCode.FPGA_Detect_OnLeftSide_Stop;*/
                //            }
                //            _waitCount += 10;
                //            if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                //                _currentStatus = "準備開啟X-Ray";
                //            else
                //                _currentStatus = "Ready to Start X-Ray..";
                //            #region VJT&&SPM
                //            if (Interface.settings.XRAYControlBySW)
                //            {
                //                if (frm_tACQ.Xray == frm_tACQ.XRAYType.VJT)//For VJT, Xray is enable through hardware
                //                {
                //                    tXRAY.VJT.XRayON_flag = true;
                //                }
                //                if (frm_tACQ.Xray == frm_tACQ.XRAYType.SPM)
                //                {
                //                    tXRAY.SPM.XRayON_flag = true;
                //                }
                //            }
                //            #endregion
                //        }
                //        if (_waitCount > 0 && _waitCount % 1000 == 0)
                //        {
                //            _prograss += 1;
                //        }
                //        //if (!frm_Acquisition.GetHWStart && !frm_Acquisition.SystemLightStatus)
                //        //{
                //        //    ErrorAction(true, "System Error!");
                //        //    frm_Acquisition.StartACQ = false;
                //        //    frm_Acquisition.GetHWStart = false;
                //        //    return result = (int)ReturnCode.FPGA_System_Error;
                //        //}
                //        //if (!frm_Acquisition.GetHWStart && !frm_Acquisition.XRAYLightStatus)
                //        //{
                //        //    ErrorAction(true, "XRAY Error!");
                //        //    frm_Acquisition.StartACQ = false;
                //        //    frm_Acquisition.GetHWStart = false;
                //        //    return result = (int)ReturnCode.FPGA_XRAY_Ready_Status_Error;
                //        //}
                //        //if (!frm_Acquisition.GetHWStart && frm_tACQ.CurrentErrorCode.MotorSensor.DetectOnRightSideStop)
                //        //{
                //        //    ErrorAction(true, "Detector Right Motor Error!");
                //        //    frm_Acquisition.StartACQ = false;
                //        //    frm_Acquisition.GetHWStart = false;
                //        //    return result = (int)ReturnCode.FPGA_Detect_OnRightSide_Stop;
                //        //}
                //        //if (!frm_Acquisition.GetHWStart && frm_tACQ.CurrentErrorCode.MotorSensor.DetectOnLeftSideStop)
                //        //{
                //        //    ErrorAction(true, "Detector Left Motor Error!");
                //        //    frm_Acquisition.StartACQ = false;
                //        //    frm_Acquisition.GetHWStart = false;
                //        //    return result = (int)ReturnCode.FPGA_Detect_OnLeftSide_Stop;
                //        //}
                //        //
                //        //result = vip_get_mode_info(N, ref modeInfo);// check the receptor link is OK
                //        //SCheckLink clk = new SCheckLink();
                //        //clk.StructSize = Marshal.SizeOf(typeof(SCheckLink));
                //        //result = vip_check_link(ref clk);

                //        //LogFile.Log(LogFile.LogFileName, "CheckRecLink result: " + result.ToString());
                //        //if (result != VIP_NO_ERR)
                //        //{
                //        //    ErrorAction(true, "vip_check_link returned " + result.ToString());
                //        //    frm_Acquisition.StartACQ = false;
                //        //    frm_Acquisition.GetHWStart = false;
                //        //    return result = (int)ReturnCode.FPD_CHECK_REC_LINK_ERR;
                //        //}
                //        CheckReturn = CheckCancel(true, false);
                //        if (CheckReturn != (int)ReturnCode.Success)
                //            return CheckReturn;
                //    }

                //    frm_Acquisition.StartACQ = false;
                //    frm_Acquisition.GetHWStart = false;

                }
                //else
                {/*
                    string sendresult;
                    try
                    {
                        sendresult = UART.ACQ(SerialPort);
                        LogFile.Log(LogFile.LogFileName, "we've send ACQ command to tShell on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                    }
                    catch (Exception exc)
                    {
                        ErrorAction(true, "Fail to send ACQ command.");
                        LogFile.Log(LogFile.LogFileName, exc.Message);
                        return result = (int)ReturnCode.FPD_SEND_ACQ_ERR;
                    }*/
                }
            }
            #endregion
            #region Wait for '0x2E' (Homing Finish)_sync board
            /*if (!Interface.settings.FPGA)
            {
                _waitCount = 0;//(waitHomingMilliSecond)
                UART._getMessageSuccess = false;
                _prograss += 3;
                do
                {
                    UART.DataReceived(SerialPort);
                    Thread.Sleep(10);
                    _waitCount += 10;
                    if (_waitCount % 1000 == 0)
                    {
                        _prograss += 5;
                    }
                    if (_waitCount > 5000)
                    {
                        ErrorAction(true, "Time out wait for serialPort response.");
                        return result = (int)ReturnCode.FPD_TIMEOUT_SYNC_BD_ERR;
                    }
                }
                while (!UART._getMessageSuccess);
                LogFile.Log(LogFile.LogFileName, "We've got waitHoming response from system on:" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                UART._receivedString = "";
            }*/
            #endregion
            #region grabber_start
            _prograss += 3;
            CheckReturn = CheckCancel(true, false);
            if (CheckReturn != (int)ReturnCode.Success)
                return CheckReturn;
            else
            {
                result = vip_fluoro_grabber_start(ref acqPrms);
                if (result != VIP_NO_ERR)
                {
                    ErrorAction(true, "Grabber_start returned " + Native.NativeConstants.HcpErrStrList[result]);
                    return result = (int)ReturnCode.FPD_GRABBER_START_ERR;
                }
                /*if (!Interface.settings.FPGA)
                    frm_Acquisition.SetXRAYStatusLightOn();*/

                LogFile.Log(LogFile.LogFileName, "vip_fluoro_grabber_start return: " + result.ToString() + "on:" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                // A pointer to a SLivePrms structure is returned in the acqPrms
                //tACQ.Native.NativeConstants.SLivePrms gp = new NativeConstants.SLivePrms();

                GLiveParamsS = (NativeConstants.SLivePrms)Marshal.PtrToStructure(acqPrms.LivePrmsPtr, typeof(NativeConstants.SLivePrms));//將unmanaged Intptr COPY給managed GLiveParamsS
                //GLiveParams = (NativeConstants.SLivePrms*)acqPrms.LivePrmsPtr;
            }
            #endregion
            #endregion
            #region STEP 5 -- Start the recording
            LogFile.Log(LogFile.LogFileName, "\n**** STEP 5 -- Start the recording --");// Now we will save frames being written to the grab buffers into the sequence buffers. If we want we can set StopAfterN and startFromBufIdx at this point too.. // result = vip_fluoro_record_start(numFrm, startFromBufIdx);// OR ..
            CheckReturn = CheckCancel(true, false);
            if (CheckReturn != (int)ReturnCode.Success)
                return CheckReturn;
            else// START #1
            {
                //if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                    _currentStatus = "擷取影像中";
                /*else
                    _currentStatus = "Record Image....";*/
                _prograss += 5;
                result = vip_fluoro_record_start();
                if (result != VIP_NO_ERR)
                {
                    ErrorAction(true, "Record_start returned " + Native.NativeConstants.HcpErrStrList[result]);
                    return result = (int)ReturnCode.FPD_RECORD_START_ERR;
                }
                LogFile.Log(LogFile.LogFileName, "vip_fluoro_record_start result " + result.ToString());
            }
            #endregion
            #region STEP 6 -- End recording
            LogFile.Log(LogFile.LogFileName, "\n**** STEP 6 -- End recording --on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            CheckReturn = CheckCancel(true, false);
            if (CheckReturn != (int)ReturnCode.Success)
                return CheckReturn;
            else
            {
                if (numFrm != 0) // wait for the requested number of frames to complete
                {
                    int to = RECORD_TIMEOUT * 1000; //總等待時間ms.
                    int lpNum = 200;//loop number
                    int qRate = to / lpNum;
                    if (qRate < 1) qRate = 1;
                    int i = 0;
                    long lastGNumFrames = 0;
                    /*
                    frm_tACQ.StopRealTimeDisplay = false;
                    frm_tACQ.DisplayedImageCount = 0;//開始Display前先歸0*/
                    SavedTempRawCount = 0;//開始儲存前先歸0
                    /*int skipFrames = 50;
                    if (frm_Acquisition.acquireMode == frm_Acquisition.AcquireMode.Pulse_Full_Asymmetric)
                        skipFrames = 90;*/
                    for (i = 0; i < lpNum; i++)
                    {
                        //CheckCancel狀態
                        CheckReturn = CheckCancel(true, true);//這裡call grabber_stop可加快速度
                        if (CheckReturn != (int)ReturnCode.Success)
                            return CheckReturn;

                        GLiveParamsS = (NativeConstants.SLivePrms)Marshal.PtrToStructure(acqPrms.LivePrmsPtr, typeof(NativeConstants.SLivePrms));//將unmanaged Intptr COPY給managed GLiveParamsS
                        GNumFrames = GLiveParamsS.NumFrames;

                        //Check XRAY狀態
                        /*
                        XRAYEmitOn.kVp = frm_Acquisition.acquisition.kVp;
                        XRAYEmitOn.mA = frm_Acquisition.acquisition.mA;
                        if (i > skipFrames && GNumFrames < numFrm - skipFrames)//等待約莫1500ms(pass前面尚未偵測到XRAY狀態的部分)，且確認Detector尚未完成抓取才檢查
                        {
                            if (frm_tACQ.Xray == frm_tACQ.XRAYType.VJT || frm_tACQ.Xray == frm_tACQ.XRAYType.SPM)
                            {
                            }
                            else
                            {
                                CheckReturn = XRAYStatusCompare(true, true, XRAYEmitOn);
                                if (CheckReturn != (int)ReturnCode.Success)//此處偵測到當前抓回XRAY狀態並非正常EMIT ON狀態
                                {
                                    LogFile.Log(LogFile.LogFileName, "XRAYStatus error = " + Interface.ErrStrList[CheckReturn] + " on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                                    //return CheckReturn;//暫時忽略此錯誤，待確認!!
                                }
                            }
                        }*/
                        LogFile.Log(LogFile.LogFileName, "GNumFrames = " + GNumFrames.ToString() + " on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                        for (int j = (int)lastGNumFrames; j < GNumFrames; j++)
                        {
                            if (j >= passSaveRawCount)
                            {
                                IntPtr buf_intptr = IntPtr.Zero;
                                vip_fluoro_get_buffer_ptr(ref buf_intptr, j);//Get intptr from Detector
                                ImageAddrList.Add(buf_intptr);
                                //if (frm_Acquisition.acquireMode != frm_Acquisition.AcquireMode.Scout)//not scout view mode
                                {
                                    //byte[] bytearray = new byte[GImgSize];
                                    //string filename = "C:\\Temp\\" + j.ToString("D3") + ".raw";//原始raw黨存放處
                                    //try
                                    //{
                                    //    Marshal.Copy(ImageAddrList[j - passSaveRawCount], bytearray, 0, GImgSize);
                                    //    SaveImage.WriteByteArrayFile(filename, bytearray, false, 0);
                                    //}
                                    //catch (Exception exc)
                                    //{
                                    //    LogFile.Log(LogFile.LogFileName1, "Error in Marshal.Copy or SaveImage.WriteByteArrayFile: " + filename + "." + exc.Message);
                                    //}
                                    SavedTempRawCount++;
                                    if (j == passSaveRawCount)
                                    {
                                        //if (!frm_tACQ.bgw_RealTimeDiaplay.IsBusy)
                                        //    frm_tACQ.bgw_RealTimeDiaplay.RunWorkerAsync();
                                        /*if (!(frm_tACQ.RealTimeDisplayThread.ThreadState == ThreadState.Running))
                                        {
                                            frm_tACQ.PrepareRealTimeDisplayThread(out frm_tACQ.RealTimeDisplayThread, "RealTimeDisplayThread", frm_tACQ.RealTimeDisplayThreadStart);
                                            frm_tACQ.RealTimeDisplayThread.Start();
                                        }*/
                                    }
                                    if (SavedTempRawCount % 50 == 0)
                                    {
                                        _prograss += 2;
                                    }
                                }
                            }
                        }
                        if (GNumFrames >= numFrm)
                        {
                            break;
                        }
                        Thread.Sleep(qRate);
                        lastGNumFrames = GNumFrames;
                    }
                    if (i == lpNum)//time out
                    {
                        if (SavedTempRawCount < FPDPara.FrameNumber)
                        {   //jack modify 20200504 經經理指示，目前先略過此錯誤.
                            ErrorAction(false, "Timed out waiting for frames");
                            string _messageBoxHeader = "M-CBCT Acquisition Control Message";//define pop up message box header from Patient management page
                            string returnMessage = "Timed out waiting for frames！" +
                                "\n從擷取影像開始經過 30 秒後，應取得 " + FPDPara.FrameNumber + " 張影像，" +
                                "\n但目前只取得了 " + SavedTempRawCount + " 張影像，先略過此錯誤！";
                            MessageBox.Show(returnMessage, _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //ErrorAction(true, "Timed out waiting for frames");
                            //return (int)ReturnCode.FPD_TIMEOUT_WAIT_FRAME;
                        }
                        else//已抓滿recon所需張數，仍然繼續，update passSaveRawCount
                        {
                            passSaveRawCount = SavedTempRawCount - FPDPara.FrameNumber;
                        }
                    }
                }
                //record FinishXrayTime for cooling time display
                //frm_Acquisition.FinishXrayTime = DateTime.Now;
                // STOP #1
                _prograss += 3;
                result = vip_fluoro_record_stop();
                if (result != VIP_NO_ERR)
                {
                    vip_fluoro_grabber_stop();// We can always just call grabber_stop - we don't have to call record_stop first.
                    ErrorAction(true, "record_stop() returned: " + result.ToString());
                    return (int)ReturnCode.FPD_RECORD_STOP_ERR;
                }
                /*
                #region VJT&&SPM
                if (Interface.settings.XRAYControlBySW)
                {
                    if (frm_tACQ.Xray == frm_tACQ.XRAYType.VJT)
                    {
                        tXRAY.VJT.XRayON_flag = false;
                    }
                    else if (frm_tACQ.Xray == frm_tACQ.XRAYType.SPM)
                    {
                        Thread.Sleep(1000);
                        tXRAY.SPM.XRayON_flag = false;
                    }
                }
                #endregion
                */
                LogFile.Log(LogFile.LogFileName, "vip_fluoro_record_stop result: " + result.ToString());
            }
            #region if doMore_暫不執行
            // if you want you can capture another segment
            //int doMore = 0;
            //if (result == VIP_NO_ERR)
            //{
            //    //#if 0
            //    //        printf("\nDo you want to capture another segment YES=1 or NO=0 ?: ");
            //    //        scanf("%d",&doMore);
            //    //#else
            //    doMore = 0;
            //    //#endif
            //}
            //if (doMore == 1)
            //{
            //    int bufIdx = -1;
            //    result = vip_fluoro_record_start(numFrm, bufIdx);
            //    // START #2
            //}
            //if (result != VIP_NO_ERR && doMore == 1)
            //{
            //    //if(!msgShown) printf("\nvip_fluoro_record_start() returned %d", result);
            //    //msgShown = true;
            //    MessageBox.Show("vip_fluoro_record_start returned " + result.ToString());
            //}
            //else if (doMore == 1)
            //{
            //    if (numFrm != 0) // wait for the requested number of frames to complete
            //    {
            //        int to = RECORD_TIMEOUT * 1000; //ms.
            //        int lpNum = 200;
            //        int qRate = to / lpNum;
            //        if (qRate < 1) qRate = 1;
            //        int i = 0;
            //        for (i = 0; i < lpNum; i++)
            //        {
            //            if (GNumFrames >= numFrm) break;
            //            Thread.Sleep(qRate);
            //        }

            //        if (i == lpNum)
            //        {
            //            result = -2;
            //            //msgShown = true;
            //        }
            //    }
            //    else // user stops at any time
            //    {
            //        //printf("\n\n**Hit any key to send vip_fluoro_record_stop()");
            //        //_getch();
            //        //while(!_kbhit()) Sleep (100);
            //    }

            //    //printf("\nSending  vip_fluoro_record_stop()");
            //    result = vip_fluoro_record_stop();
            //    // STOP #2
            //}
            #endregion
            #endregion
            #region STEP 7 -- End grabbing
            LogFile.Log(LogFile.LogFileName, "\n**** STEP 7 -- End grabbing --");// We're back to the state where we are grabbing only and frames are not being copied to sequence buffers.
            CheckReturn = CheckCancel(true, false);
            if (CheckReturn != (int)ReturnCode.Success)
                return CheckReturn;
            else
            {
                _prograss += 3;
                if (passSaveRawCount == dummyFrame)
                {
                    result = vip_fluoro_grabber_stop();
                    if (result != VIP_NO_ERR)
                    {
                        ErrorAction(true, "grabber_stop() returned: " + result.ToString());
                        return result = (int)ReturnCode.FPD_GRABBER_STOP_ERR;
                    }
                    LogFile.Log(LogFile.LogFileName, "vip_fluoro_grabber_stop result: " + result.ToString());
                }
            }
            _prograss += 1;
            result = vip_set_user_sync(N, false);

            #endregion
            #region STEP 8 -- Save images
            LogFile.Log(LogFile.LogFileName, "\n**** STEP 8 -- Save images --Current time:" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            //if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
            _currentStatus = "儲存影像中";
            _prograss += 3;
            DateTime time = DateTime.Now;
            CheckReturn = CheckCancel(true, false);
            if (CheckReturn != (int)ReturnCode.Success)
                return CheckReturn;
            else
            {
                string fName = "";
                /*
                if (frm_Acquisition.acquireMode == frm_Acquisition.AcquireMode.Scout)
                {
                    for (int i = 0; i < ImageAddrList.Count; i++)
                    {
                        CheckReturn = CheckCancel(true, false);
                        if (CheckReturn != (int)ReturnCode.Success)
                            return CheckReturn;
                        else
                        {
                            fName = tACQ.frm_tACQ.RawDiretion + "\\" + i.ToString("D3") + ".raw";
                            byte[] bytearray = new byte[GImgSize];
                            try
                            {
                                Marshal.Copy(ImageAddrList[i], bytearray, 0, GImgSize);
                            }
                            catch (Exception exc)
                            {
                                LogFile.Log(LogFile.LogFileName, exc.Message.ToString());
                            }
                            SaveImage.WriteByteArrayFile(fName, bytearray, false, 0);//寫檔
                            FileInfo file = new FileInfo(fName);
                            file.Attributes = FileAttributes.ReadOnly;//將檔案屬性改為 ReadOnly
                        }
                    }
                }*/
                //else
                {
                    //if (usetNaruto && !frm_Acquisition.isCalibration)
                    {
                        /*
                        #region tNaruto mode
                        string outPath = frm_tACQ.settings.RamDisk; // destination to store generated raw files
                        int caId = 0; // will store REAL calibration profile ID
                        int maId = 0; //  and MAR-spacific calibration profile ID
                        frm_Acquisition.OriginalCalibrationFile = frm_Acquisition.CalibrationFile;
                        tNaruto(ImageAddrList.ToArray(), GImgSize, ImageAddrList.Count, 64, frm_Acquisition.CalibrationFile
                            , frm_Acquisition.CalibrationFile.Substring(0, frm_Acquisition.CalibrationFile.Length - 4) + "_Metal.cal"
                            , outPath, out caId, out maId);
                        frm_Acquisition.CalibrationFile = Path.Combine(outPath, String.Format("{0:x08}.cal", caId));
                        frm_Acquisition.MetalCalibrationFile = Path.Combine(outPath, String.Format("{0:x08}.cal", maId));
                        #endregion*/
                    }
                    //else//we always save raw in this way to disk while clibration
                    {/*
                        frm_Acquisition.OriginalCalibrationFile = frm_Acquisition.CalibrationFile;
                        frm_Acquisition.MetalCalibrationFile = frm_Acquisition.CalibrationFile.Substring(0, frm_Acquisition.CalibrationFile.Length - 4) + "_Metal.cal";
                        */
                        for (int i = 0; i < ImageAddrList.Count; i++)
                        {
                            CheckReturn = CheckCancel(true, false);
                            if (CheckReturn != (int)ReturnCode.Success)
                                return CheckReturn;
                            else
                            {/*
                                if (frm_Acquisition.isCalibration && frm_Acquisition.calibrationMode == frm_Acquisition.CalibrationMode.Geometry)
                                    fName = tACQ.frm_tACQ.RawDiretion + "\\" + (i + frm_Acquisition.round * FPDPara.FrameNumber).ToString("D4") + ".raw";
                                else*/
                                    fName = Acquisition.RawDiretion + "\\" + i.ToString("D3") + ".raw";
                                byte[] bytearray = new byte[GImgSize];
                                try
                                {
                                    Marshal.Copy(ImageAddrList[i], bytearray, 0, GImgSize);
                                }
                                catch (Exception exc)
                                {
                                    LogFile.Log(LogFile.LogFileName, exc.Message.ToString());
                                }
                                SaveImage.WriteByteArrayFile(fName, bytearray, false, 0);//寫檔
                                //FileInfo file = new FileInfo(fName);
                                //file.Attributes = FileAttributes.ReadOnly;//將檔案屬性改為 ReadOnly
                                if (i % 50 == 0) _prograss += 2;
                            }
                        }
                    }
                }
            }
            LogFile.Log(LogFile.LogFileName, "Saving images finished. Use time:" + (DateTime.Now - time).ToString());
            _prograss += 3;

            // get stats for acquisition
            NativeConstants.SSeqStats seqStats = new NativeConstants.SSeqStats();
            _prograss += 3;
            result = vip_fluoro_get_prms((int)NativeConstants.HcpFluoroStruct.HCP_FLU_STATS_PRMS, ref seqStats);
            if (result != VIP_NO_ERR)
            {
                ErrorAction(true, "Problem getting stats, now vip_close_link..." + result.ToString());
                return (int)ReturnCode.FPD_GET_PRMS_ERR;
            }
            else
            {
                LogFile.Log(LogFile.LogFileName, "Returned Seq Stats: "
                                + "SmplFrms = " + seqStats.SmplFrms + "; "
                                + "HookFrms = " + seqStats.HookFrms + "; "
                                + "CaptFrms = " + seqStats.CaptFrms + "; "
                                + "HookOverrun = " + seqStats.HookOverrun + "; "
                                + "StartIdx = " + seqStats.StartIdx + "; "
                                + "EndIdx = " + seqStats.EndIdx + "; "
                                + "CaptRate = " + seqStats.CaptRate);
            }

            #endregion
            #region STEP 9 -- Close the link
            LogFile.Log(LogFile.LogFileName, "\n**** STEP 9 -- Close the link --");
            _prograss += 5;
            vip_set_debug(0);
            result = vip_close_link(NativeConstants.CLSLNK_RELMEM);
            if (result != VIP_NO_ERR)
            {
                ErrorAction(false, "Close_link Error:" + result.ToString());
                LogFile.Log(LogFile.LogFileName, "vip_close_link() returned " + result.ToString());
                if (result == 81)
                {
                    LogFile.Log(LogFile.LogFileName, "Check that the IMAGERS_DIR_PATH exists");
                }
            }
            else
            {
                LogFile.Log(LogFile.LogFileName, "vip_close_link() result: " + result.ToString());
                _endTime = DateTime.Now;
                LogFile.Log(LogFile.LogFileName, "Acquisition complete on " + _endTime.ToString());
                LogFile.Log(LogFile.LogFileName, "Total Time Elapsed: " + (_endTime - _beginTime).TotalSeconds.ToString() + " secs");
                //if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                    _currentStatus = "拍攝完成";
                /*else
                    _currentStatus = "Acquisition complete successfully.";*/
                _prograss = 100;
            }
            LogFile.Log(LogFile.LogFileName, "----------------------------------------------------------------" + "\n------------------ Finish Acquisition process ------------------" + "\n----------------------------------------------------------------");
            #endregion
            return result;
        }

        /*
        /// <summary>FPD Acquisition process -Dry run.</summary>
        public static int DryRun(SerialPort SerialPort)
        {
            int result = (int)Interface.ReturnCode.ExceptionError;
            string sendresult = "";
            string methodName = MethodInfo.GetCurrentMethod().Name;
            string LogFormatedString = className + "." + methodName;
            LogFormatedString = LogFormatedString.PadRight(PadLength);
            #region status update
            _prograss = 0;//歸零process
            if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                _currentStatus = "準備執行試運轉";
            else
                _currentStatus = "Start Dry run";
            #endregion
            if (!SerialPort.IsOpen)//加入嘗試重新連結?
            {
                result = (int)Interface.ReturnCode.UARTSerialPortClosed;
                return result;
            }

            SerialPort.ReadExisting();
            #region status update
            _prograss++;
            //_currentStatus = "Ready for sending command";
            #endregion
            try
            {
                if (Interface.settings.FPGA)
                {
                    #region New mode_wait for hardware start(after A3 command + 10 secs)
                    //背景開始等候HardWare Start指令
                    int _waitCount = 0;//ms
                    frm_tACQ.WaitA4 = true;
                    frm_tACQ.WaitA7 = true;
                    if (frm_Acquisition.doRaliabilityTest)
                    {
                        if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                            _currentStatus = "準備執行試運轉";
                        else
                            _currentStatus = "Wait for system start.";//ProcessBar提示使用者
                    }
                    else
                    {
                        if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                            _currentStatus = "請按下控制器上的SCAN鍵進行試運轉";
                        else
                            _currentStatus = "Please Press SCAN Button on the control box to Satrt.";//ProcessBar提示使用者
                    }
                    _prograss++;
                    while (!frm_Acquisition.StartACQ)
                    {
                        Thread.Sleep(10);
                        if (frm_Acquisition.GetHWStart)
                        {
                            if (_waitCount > 20000)//等待A7 return for 10 sec after send A7
                            {
                                #region status update
                                _currentStatus = "Time out wait for serialPort response.";
                                _prograss = 0;
                                #endregion
                                //vip_set_debug(0);
                                //result = vip_close_link(NativeConstants.CLSLNK_RELMEM);
                                LogFile.Log(LogFile.LogFileName, _currentStatus);
                                LogFile.Log(LogFile.LogFileName, EOF);
                                frm_Acquisition.GetHWStart = false;
                                return result = (int)ReturnCode.FPD_TIMEOUT_SYNC_BD_ERR;
                            }
                            _waitCount += 10;
                            if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                                _currentStatus = "準備進行試運轉流程";
                            else
                                _currentStatus = "Ready to Start Dry Run..";
                        }
                        if (frm_Acquisition.doRaliabilityTest)
                        {
                            _waitCount += 10;
                            if (_waitCount % 1000 == 0)
                            {
                                _currentStatus += "..";//ProcessBar提示使用者.
                                _prograss++;
                            }
                        }
                        if (UserCancelACQProcess)
                        {
                            _currentStatus = "使用者中斷試運轉流程";
                            _prograss = 0;
                            LogFile.Log(LogFile.LogFileName, _currentStatus);
                            LogFile.Log(LogFile.LogFileName, EOF);
                            frm_Acquisition.StartACQ = false;
                            frm_Acquisition.GetHWStart = false;
                            return result = (int)ReturnCode.FPD_NOT_SEND_ACQ_USER_CANCEL;
                        }
                    }
                    //  frm_Acquisition.StartACQ = false;
                    frm_Acquisition.GetHWStart = false;
                    #endregion
                }
                else
                {
                    sendresult = UART.ACQ(SerialPort);
                    LogFile.Log(UART.UARTLogFileName, LogFormatedString + ":" + "SendFPGACommand result: " + sendresult + "( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
                }

                #region status update
                _prograss++;
                if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                    _currentStatus = "開始執行試運轉";
                else
                    _currentStatus = "Dry Run Start";
                #endregion
            }
            catch (Exception exc)
            {
                if (Interface.settings.FPGA)
                {
                    LogFile.Log(frm_tACQ.FPGALog, LogFormatedString + ":" + "SendFPGACommand result: " + exc.Message + "( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
                }
                else
                {
                    LogFile.Log(UART.UARTLogFileName, LogFormatedString + ":" + "SendFPGACommand result: " + exc.Message + "( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
                }
                #region status update
                _prograss = 0;
                _currentStatus = exc.Message;
                #endregion
                return (int)Interface.ReturnCode.ExceptionError;
            }
            if (Interface.settings.FPGA)
            {
                if ((frm_Acquisition.doRaliabilityTest && !frm_tACQ.settings.ActiveXRAY) || (!frm_tACQ.settings.ActiveXRAY && frm_Acquisition.ShowDemo))
                {
                    frm_tACQ.DisplayedImageCount = 0;
                    SavedTempRawCount = 0;
                    frm_tACQ.StopRealTimeDisplay = false;
                    if (!(frm_tACQ.RealTimeDisplayThread.ThreadState == ThreadState.Running))
                    {
                        frm_tACQ.PrepareRealTimeDisplayThread(out frm_tACQ.RealTimeDisplayThread, "RealTimeDisplayThread", frm_tACQ.RealTimeDisplayThreadStart);
                        frm_tACQ.RealTimeDisplayThread.Start();
                    }
                }
                #region 背景開始等候HardWare Finish指令
                int timeout = 60 * 1000;//unit:ms
                int timeElapsed = 0;
                frm_Acquisition.FinishACQ = false;
                while (!frm_Acquisition.FinishACQ)
                {
                    if ((frm_Acquisition.doRaliabilityTest && !frm_tACQ.settings.ActiveXRAY) || (!frm_tACQ.settings.ActiveXRAY && frm_Acquisition.ShowDemo))
                    {
                        SavedTempRawCount++;
                    }
                    Thread.Sleep(10);
                    timeElapsed += 10;

                    if (timeElapsed >= timeout)//Time out 60sec即跳出
                    {
                        if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                            _currentStatus = "準備執行試運轉";
                        else
                            _currentStatus = "Get Sync Board Error before send ACQ command.";
                        _prograss = 0;
                        LogFile.Log(LogFile.LogFileName, _currentStatus);
                        LogFile.Log(LogFile.LogFileName, EOF);
                        return result = (int)ReturnCode.FPGA_Time_Out_Wait_For_Finish;
                    }
                    if (UserCancelACQProcess)
                    {
                        if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                            _currentStatus = "使用者中斷試運轉";
                        else
                            _currentStatus = "User Cancel ACQ Process before send ACQ command.";
                        _prograss = 0;
                        LogFile.Log(LogFile.LogFileName, _currentStatus);
                        LogFile.Log(LogFile.LogFileName, EOF);
                        frm_Acquisition.StartACQ = false;
                        frm_Acquisition.GetHWStart = false;
                        return result = (int)ReturnCode.FPD_NOT_SEND_ACQ_USER_CANCEL;
                    }
                    if (_prograss < 99 && timeElapsed % 150 == 0)
                    {
                        _prograss++;
                        if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                            _currentStatus = "試運轉運行中..." + _prograss.ToString() + "%";
                        else
                            _currentStatus = "Dry Run..." + _prograss.ToString() + "%";
                    }
                    try
                    {
                        Application.DoEvents();
                    }
                    catch
                    {
                    }
                }
                #endregion
            }
            else
            {
                #region 使用軟體倒數結束
                if (frm_Acquisition.SelectMotorSequence == Interface.MotorSequence.Symmetric_155)
                {
                    for (int i = 0; i < 45; i++)
                    {
                        Thread.Sleep(320);
                        _prograss += 2;
                        if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                            _currentStatus = "試運轉運行中..." + _prograss.ToString() + "%";
                        else
                            _currentStatus = "Dry Run..." + _prograss.ToString() + "%";
                    }
                }
                else
                {
                    for (int i = 0; i < 45; i++)
                    {
                        Thread.Sleep(550);
                        _prograss += 2;
                        if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                            _currentStatus = "試運轉運行中..." + _prograss.ToString() + "%";
                        else
                            _currentStatus = "Dry Run..." + _prograss.ToString() + "%";
                    }
                }
                #endregion
            }
            result = (int)Interface.ReturnCode.Success;
            #region status update
            if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                _currentStatus = "試運轉完成";
            else
                _currentStatus = "Complete Dry Run Mode";
            _prograss = 100;
            #endregion
            try
            {
                Application.DoEvents();
            }
            catch
            {
            }
            return result;
        }*/

        /// <summary>Used to do error actions for acquisition process, including close detector link,represend process bar str and log to file</summary><param name="TurnOffLink">true if need to process close link action</param><param name="errorStr">str used to log on file while error occurs</param>
        public static void ErrorAction(bool TurnOffLink, string errorStr)
        {
            _currentStatus = errorStr;
            _prograss = 0;
            if (TurnOffLink)
            {
                vip_set_debug(0);
                vip_close_link(NativeConstants.CLSLNK_RELMEM);
            }
            LogFile.Log(LogFile.LogFileName, _currentStatus);
            LogFile.Log(LogFile.LogFileName, EOF);
            /*
            #region VJT&&SPM
            if (Interface.settings.XRAYControlBySW)
            {
                if (frm_tACQ.Xray == frm_tACQ.XRAYType.VJT)
                {
                    tXRAY.VJT.XRayON_flag = false;
                }
                else if (frm_tACQ.Xray == frm_tACQ.XRAYType.SPM)
                {
                    tXRAY.SPM.XRayON_flag = false;
                }
            }
            #endregion
            */
        }
        /*
        /// <summary>判斷XRAY狀態是否是WishXRAYStatus，如否，則呼叫此函數執行必要action</summary>
        /// <param name="TurnOffLink">此時是否關閉Detector連線</param>
        /// <param name="GrabberStop">此時是否GrabberStop</param>
        /// <param name="WishXRAYStatus">期望之XRAY狀態</param>
        /// <returns>回應XRAY發生錯誤的狀態</returns>
        public static int XRAYStatusCompare(bool TurnOffLink, bool GrabberStop, XRAYStatus WishXRAYStatus)
        {
            int result = (int)ReturnCode.Success;
            
            if (frm_tACQ.CurrentErrorCode.XRAYStatus.XRayOverI != WishXRAYStatus.XRayOverI)
            {
                result = (int)ReturnCode.FPGA_XRAY_Over_Current_Status_Error;
                LogFile.Log(frm_tACQ.FPGALog, ("Get Over_Current =" + frm_tACQ.CurrentErrorCode.XRAYStatus.XRayOverI.ToString()).PadRight(PadLength) + " ( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
            }
            else if (frm_tACQ.CurrentErrorCode.XRAYStatus.XRayTubeARC != WishXRAYStatus.XRayTubeARC)
            {
                result = (int)ReturnCode.FPGA_XRAY_ARC_Status_Error;
                LogFile.Log(frm_tACQ.FPGALog, ("Get XRAY_ARC =" + frm_tACQ.CurrentErrorCode.XRAYStatus.XRayTubeARC.ToString()).PadRight(PadLength) + " ( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
            }
            else if (frm_tACQ.CurrentErrorCode.XRAYStatus.XRayOverV != WishXRAYStatus.XRayOverV)
            {
                result = (int)ReturnCode.FPGA_XRAY_OVERVOLTAGE_Status_Error;
                LogFile.Log(frm_tACQ.FPGALog, ("Get XRAY_OVERVOLTAGE =" + frm_tACQ.CurrentErrorCode.XRAYStatus.XRayOverV.ToString()).PadRight(PadLength) + " ( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
            }
            else if (frm_tACQ.CurrentErrorCode.XRAYStatus.XRayFilamentStatus != WishXRAYStatus.XRayFilamentStatus)
            {
                result = (int)ReturnCode.FPGA_XRAY_Filament_Status_Error;
                LogFile.Log(frm_tACQ.FPGALog, ("Get Filament =" + frm_tACQ.CurrentErrorCode.XRAYStatus.XRayFilamentStatus.ToString()).PadRight(PadLength) + " ( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
            }
            else if (frm_tACQ.CurrentErrorCode.XRAYStatus.XRayOverTemp != WishXRAYStatus.XRayOverTemp)
            {
                result = (int)ReturnCode.FPGA_XRAY_OverTemp_Status_Error;
                LogFile.Log(frm_tACQ.FPGALog, ("Get XRAY_OverTemp =" + frm_tACQ.CurrentErrorCode.XRAYStatus.XRayOverTemp.ToString()).PadRight(PadLength) + " ( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
            }
            else if (frm_tACQ.CurrentErrorCode.XRAYStatus.XRayOnStatus != WishXRAYStatus.XRayOnStatus)
            {
                result = (int)ReturnCode.FPGA_XRAY_On_Status_Error;
                LogFile.Log(frm_tACQ.FPGALog, ("Get On_Status =" + frm_tACQ.CurrentErrorCode.XRAYStatus.XRayOnStatus.ToString()).PadRight(PadLength) + " ( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
            }
            else if (frm_tACQ.CurrentErrorCode.XRAYStatus.XRayReady != WishXRAYStatus.XRayReady)
            {
                result = (int)ReturnCode.FPGA_XRAY_Ready_Status_Error;
                LogFile.Log(frm_tACQ.FPGALog, ("Get XRAY_Ready_Status =" + frm_tACQ.CurrentErrorCode.XRAYStatus.XRayReady.ToString()).PadRight(PadLength) + " ( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
            }
            else if (frm_tACQ.CurrentErrorCode.XRAYStatus.XRayInterlock != WishXRAYStatus.XRayInterlock)
            {
                result = (int)ReturnCode.FPGA_XRAY_Interlock_Monitor_Error;
                LogFile.Log(frm_tACQ.FPGALog, ("Get XRAY_Interlock_Monitor =" + frm_tACQ.CurrentErrorCode.XRAYStatus.XRayInterlock.ToString()).PadRight(PadLength) + " ( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
            }
            else if (Math.Abs(frm_tACQ.CurrentErrorCode.XRAYStatus.kVp - WishXRAYStatus.kVp) > 2)
            {
               // result = (int)ReturnCode.FPGA_XRAY_KVp_Error;
                LogFile.Log(frm_tACQ.FPGALog, ("Get kVp =" + frm_tACQ.CurrentErrorCode.XRAYStatus.kVp.ToString()).PadRight(PadLength) + " ( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
            }
            else if (Math.Abs(frm_tACQ.CurrentErrorCode.XRAYStatus.mA - WishXRAYStatus.mA) > 2)
            {
                result = (int)ReturnCode.FPGA_XRAY_uA_Error;
                LogFile.Log(frm_tACQ.FPGALog, ("Get mA=" + frm_tACQ.CurrentErrorCode.XRAYStatus.mA.ToString()).PadRight(PadLength) + " ( " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + " )");
            }
            else
                result = (int)ReturnCode.Success;
            
            if (result != (int)ReturnCode.Success)
            {
                if (GrabberStop)
                    vip_fluoro_grabber_stop();
                if (TurnOffLink)
                    vip_close_link(NativeConstants.CLSLNK_RELMEM);
                if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                    _currentStatus = "XRAY狀態異常";
                else
                    _currentStatus = "Get XRAY status Error";
                LogFile.Log(LogFile.LogFileName, _currentStatus);
                LogFile.Log(LogFile.LogFileName, EOF);
                frm_Acquisition.StartACQ = false;
                frm_Acquisition.GetHWStart = false;
            }
            return result;
        }*/


        /// <summary>Check if user cancel or sync board error during acquisition process</summary><param name="TurnOffLink">true:turn off detector link</param><param name="GrabberStop">true:Call GrabberStop</param>
        public static int CheckCancel(bool TurnOffLink, bool GrabberStop)
        {

            int result = (int)ReturnCode.Success;
            if (Interface.GetSyncBoardError)
            {
                if (GrabberStop)
                    vip_fluoro_grabber_stop();
                if (TurnOffLink)
                    vip_close_link(NativeConstants.CLSLNK_RELMEM);
                LogFile.Log(LogFile.LogFileName, _currentStatus);
                LogFile.Log(LogFile.LogFileName, EOF);
                /*
                #region VJT&&SPM
                if (Interface.settings.XRAYControlBySW)
                {
                    if (frm_tACQ.Xray == frm_tACQ.XRAYType.VJT)
                    {
                        tXRAY.VJT.XRayON_flag = false;
                    }
                    else if (frm_tACQ.Xray == frm_tACQ.XRAYType.SPM)
                    {
                        tXRAY.SPM.XRayON_flag = false;
                    }
                }
                #endregion
                */
                return result = (int)ReturnCode.GetSyncBoardError;
            }

            if (UserCancelACQProcess)
            {
                if (GrabberStop)
                    vip_fluoro_grabber_stop();
                if (TurnOffLink)
                    vip_close_link(NativeConstants.CLSLNK_RELMEM);

                return result = (int)ReturnCode.User_Cancel_Acquisition_Process;
            }

            return result;
        }

        /// <summary>Check link with calling vip_check_link</summary>
        static int CheckRecLink()
        {
            SCheckLink clk = new SCheckLink();
            clk.StructSize = Marshal.SizeOf(typeof(SCheckLink));
            int result = vip_check_link(ref clk);
            int i = 0;
            while (result != NativeConstants.HCP_NO_ERR && i++ < 5)
            {
                Thread.Sleep(1000);
                result = vip_check_link(ref clk);
            }
            //忽略失敗
            //if (result != NativeConstants.HCP_NO_ERR)
            //{
            //    // If check link fails it could be because of several things in addition
            //    // to the link itself not working correctly. It may be that the receptor
            //    // hasn't settled to its normal range for example. Give the option to move on
            //    // here but in a real application we should understand why.
            //    int ignore = 0;
            //    //printf("\nCheck link failed. Do you want to attempt to continue "
            //    //        "YES=1 or NO=0 ?: ");     
            //    //scanf("%d",&ignore);
            //    if (ignore == 1) result = NativeConstants.HCP_NO_ERR;
            //}
            return result;
        }
        /// <summary>Return the current status string of detector and progress percentage of acquisition process.</summary>
        public static int ProgressBar(out string status)
        {
            status = _currentStatus;
            return _prograss;
        }
        #endregion

        #region Calibration
        /// <summary>Do calibration work of detector includes offset and gain calibration.</summary><param name="mode">The mode used to do calibration on.</param>
        static int DoCal(int mode, int CalFrmCount)
        {
            int ret;
            ret = FluoroOffsetCal(mode, CalFrmCount);
            ret = FluoroGainCal(mode, CalFrmCount);
            return ret;
        }
        /// <summary>do offset cal</summary>
        public static int FluoroOffsetCal(int mode, int CalFrmCount)
        {
            int numCalFrmSet = CalFrmCount;
            int result = 0;
            //連線
            //SOpenReceptorLink orl = new SOpenReceptorLink();
            //orl.StructSize = Marshal.SizeOf(typeof(SOpenReceptorLink));
            //orl.RecDirPath = IMAGERS_DIR_PATH;
            //result = vip_open_receptor_link(ref orl);//Original_1227
            //// find how many calibration frames we have set_currently cancel this cus we've pass this value from mode_info and pass through argument
            result = vip_get_num_cal_frames(mode, ref numCalFrmSet);
            //LogFile.Log(LogFile.LogFileName, "numCalFrmSet:" + numCalFrmSet.ToString());
            result = vip_set_user_sync(mode, false);
            if (result == VIP_NO_ERR)
            {
                // start the offset cal
                result = vip_offset_cal(mode);
            }
            LogFile.Log(LogFile.LogFileName, "vip_offset_cal result:" + result.ToString() + " on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

            #region original C++ Code
            //SQueryProgInfo qpi;
            //memset(&qpi, 0, sizeof(SQueryProgInfo));
            //qpi.StructSize = sizeof(SQueryProgInfo);
            //UQueryProgInfo* uqpi = (UQueryProgInfo*)&qpi;
            //while (!qpi.Complete && result == VIP_NO_ERR)
            //{
            //    int lastNum = 0;
            //    result = vip_query_prog_info(HCP_U_QPI, uqpi);
            //    if (qpi.NumFrames <= numCalFrmSet)
            //    {
            //        if (lastNum != qpi.NumFrames)
            //        {
            //            printf("\nOffset frames accumulated = %d; Complete = %d",
            //                    qpi.NumFrames, qpi.Complete);
            //            lastNum = qpi.NumFrames;
            //        }
            //    }
            //    Sleep(100);
            //}
            #endregion

            //UQueryProgInfo* uqp = (UQueryProgInfo*)&qpi;//original
            //            UQueryProgInfo uq = new UQueryProgInfo();

            //                GCHandle handle = GCHandle.Alloc(qpi, GCHandleType.Pinned);
            //                IntPtr uqpi = handle.AddrOfPinnedObject();
            //                Marshal.StructureToPtr(qpi, qpi_ptr, true);

            SQueryProgInfo qpi = new SQueryProgInfo();
            qpi.StructSize = Marshal.SizeOf(typeof(SQueryProgInfo));
            UQueryProgInfo uqp = new UQueryProgInfo();
            IntPtr uqpi = Marshal.AllocHGlobal(Marshal.SizeOf(uqp));

            // wait for the calibration to complete
            while (!uqp.qpi.Complete && result == VIP_NO_ERR)
            {
                int lastNum = 0;
                result = vip_query_prog_info(NativeConstants.HCP_U_QPI, ref uqp);
                //Marshal.StructureToPtr(uqp, uqpi, true);
                //Marshal.PtrToStructure(uqpi, qpi);

                if (qpi.NumFrames <= numCalFrmSet)
                {
                    if (lastNum != qpi.NumFrames)
                    {
                        lastNum = qpi.NumFrames;
                    }
                }
                Thread.Sleep(100);
            }

            if (result == VIP_NO_ERR)
            {
                LogFile.Log(LogFile.LogFileName, "Offset calibration completed successfully");
            }
            else
            {
                LogFile.Log(LogFile.LogFileName, "Error in offset calibration" + " on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            }
            //     result = vip_close_link(NativeConstants.CLSLNK_RELMEM);
            return result;
        }
        /// <summary>do a gain cal</summary>
        public static int FluoroGainCal(int mode, int CalFrmCount)
        {
            int numCalFrmSet = CalFrmCount;
            int result = 0;
            //result = vip_get_num_cal_frames(mode, ref numCalFrmSet);
            result = vip_set_user_sync(mode, false);

            // tell the system to prepare for a gain calibration
            if (result == VIP_NO_ERR)
            {
                result = vip_gain_cal_prepare(mode, 0);//2nd argument=0 means cal with single gain
            }

            // send prepare = true
            if (result == VIP_NO_ERR)
            {
                result = vip_sw_handshaking(NativeConstants.VIP_SW_PREPARE, true);
            }

            SQueryProgInfo qpi = new SQueryProgInfo();
            qpi.StructSize = Marshal.SizeOf(qpi);
            UQueryProgInfo uqp = new UQueryProgInfo();
            //var uqpi = Marshal.AllocHGlobal(Marshal.SizeOf(uqp));
            //Marshal.StructureToPtr(uqp, uqpi, false);
            //GCHandle handle = GCHandle.Alloc(qpi, GCHandleType.Pinned);
            //uqpi = GCHandle.ToIntPtr(handle);

            // wait for readyForPulse
            while (!uqp.qpi.ReadyForPulse && result == VIP_NO_ERR)
            {
                result = vip_query_prog_info(NativeConstants.HCP_U_QPI, ref uqp);
                Thread.Sleep(100);
            }

            // send xrays = true - this signals the START of the FLAT-FIELD ACQUISITION
            if (result == VIP_NO_ERR)
            {
                result = vip_sw_handshaking(NativeConstants.VIP_SW_VALID_XRAYS, true);
            }

            uqp.qpi.NumFrames = 0;
            int maxFrms = 0;
            while (uqp.qpi.NumFrames < numCalFrmSet && result == VIP_NO_ERR)
            {
                result = vip_query_prog_info(NativeConstants.HCP_U_QPI, ref uqp);
                //printf("\nFlat-field frames accumulated = %d", qpi.NumFrames);
                Thread.Sleep(100);

                // just in case the number of frames resets to zero before we see the 
                // limit reached
                if (maxFrms > uqp.qpi.NumFrames) break;
                maxFrms = uqp.qpi.NumFrames;
            }

            // wait for readyForPulse
            while (!uqp.qpi.ReadyForPulse && result == VIP_NO_ERR)
            {
                result = vip_query_prog_info(Native.NativeConstants.HCP_U_QPI, ref uqp);
                Thread.Sleep(100);
            }

            // send xrays = false - this signals the START of the DARK-FIELD ACQUISITION
            if (result == VIP_NO_ERR)
            {
                result = vip_sw_handshaking(NativeConstants.VIP_SW_VALID_XRAYS, false);
            }

            // wait for the calibration to complete
            qpi.Complete = false;
            while (!uqp.qpi.Complete && result == VIP_NO_ERR)
            {
                result = vip_query_prog_info(Native.NativeConstants.HCP_U_QPI, ref uqp);
                //if (qpi.NumFrames < numCalFrmSet)
                //{
                //    //printf("\nDark-field frames accumulated = %d", qpi.NumFrames);
                //}
                Thread.Sleep(100);
            }

            //if (result == VIP_NO_ERR)
            //{
            //    //printf("\n\nGain calibration completed successfully");
            //}
            //else
            //{
            //    //printf("\n\nError in gain calibration");
            //}
            return result;
        }
        #endregion

    }
}
