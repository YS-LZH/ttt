using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace TCT_OnlyDetector.Native
{
    public partial class NativeConstants
    {
        #region consts
        /// VIP_INC_SUNDRYSTRUCTS_H -> 
        /// Error generating expression: 值不能為 null。
        ///參數名稱: node
        public const string VIP_INC_SUNDRYSTRUCTS_H = "";

        /// MAX_STR -> 256
        public const int MAX_STR = 256;

        /// MIN_STR -> 32
        public const int MIN_STR = 32;

        /// HCP_CHKLNK_LONG -> 3
        public const int HCP_CHKLNK_LONG = 3;

        /// HCP_CHKLNK_SHRT -> 4
        public const int HCP_CHKLNK_SHRT = 4;

        /// CLSLNK_POWERDOWN -> 0x0100
        public const int CLSLNK_POWERDOWN = 256;

        /// CLSLNK_RELMEM -> 0x0200
        public const int CLSLNK_RELMEM = 512;

        /// CLSLNK_CLOSEALL -> 0x0400
        public const int CLSLNK_CLOSEALL = 1024;

        /// EXGN_ID -> 0xCB
        public const int EXGN_ID = 203;

        /// GNSTATE_DYN_HI -> 0
        public const int GNSTATE_DYN_HI = 0;

        /// GNSTATE_FLG_LO -> 1
        public const int GNSTATE_FLG_LO = 1;

        /// STR_TYPE1 -> 1
        public const int STR_TYPE1 = 1;

        /// HCP_U_QPI_MASK -> (0x7FFF)
        public const int HCP_U_QPI_MASK = 32767;

        /// HCP_U_QPI_CRNT_DIAG_DATA -> (0x8000)
        public const int HCP_U_QPI_CRNT_DIAG_DATA = 32768;
        /// SELREC_REC_MASK -> 0x00FF
        public const int SELREC_REC_MASK = 255;

        /// SELREC_MODE_MASK -> 0xFF00
        public const int SELREC_MODE_MASK = 65280;

        /// SELREC_MODE_SHIFT -> 8
        public const int SELREC_MODE_SHIFT = 8;

        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        // VIP_SET_DEBUG
        // the following are used by vip_set_debug(enable) and relatedly to specify
        // type of debug required
        public const int HCP_DBG_OFF = 0;
        public const int HCP_DBG_ON = 1;
        public const int HCP_DBG_ON_FLSH = 2;
        public const int HCP_DBG_ON_DLG = 3;

        #region define values ADD ON PaxscanL07
        public const int HCP_FG_CALLBACK_FLAG = 3263;
        public const int MAX_TV_RESERVED = 32;
        public const int TEMPS_ALLOC = 16;
        public const int VOLTS_ALLOC = 16;
        public const int NUM_PIX_WITH_DATA = 136;	// both hw and sw including signature (8)
        public const int DATA_START_LOC = 8;	// hw data start here
        public const int NUM_PIX_WITH_HW_DATA = 96;
        public const int SW_DATA_START_LOC = 104;
        public const int REC_ID_LOC = 12; // location of the receptor id
        public const int GRB_ID_LOC = 104;// location of the grabber (pleora) id
        public const int VCP_ID_LOC = 106; // location of the VCP id
        public const int BAD_IMG_LOC = 108;// location of the bad image size pixel 
        #endregion
        #region Codes for qpi.Cancellation
        public const int NO_CANCEL = 0;
        public const int CANCEL_SUCCEEDED = 1;
        public const int CANCEL_FAILED = 2;
        public const int CANCEL_TIMEOUT_PREP = 3;
        public const int CANCEL_TRUNCATE_EXPOK = 4;//add on PaxscanL07
        #endregion
        #endregion consts
        #region enum group1
        public const int HCP_CTRLTYPE_MIN = 46;
        public const int HCP_CTRLTYPE_OFST = 47;
        public const int HCP_CTRLTYPE_GAIN = 48;
        public const int HCP_CTRLTYPE_GAIN_HI_G = 49;
        public const int HCP_CTRLTYPE_GAIN_LO_G = 50;
        public const int HCP_CTRLTYPE_EXT0 = 51;
        public const int HCP_CTRLTYPE_EXT1 = 52;
        public const int HCP_CTRLTYPE_EXT2 = 53;
        public const int HCP_CTRLTYPE_EXT3 = 54;
        public const int HCP_CTRLTYPE_EXTCAL = 55;
        public const int HCP_CTRLTYPE_GR0 = 56;
        public const int HCP_CTRLTYPE_GR1 = 57;
        public const int HCP_CTRLTYPE_GRCAL = 58;
        public const int HCP_CTRLTYPE_MAX = 59;
        #endregion
        #region enum group2
        public const int HCP_ACCMODE_RESET_AND_SEND = 0;// resets any previous acquired data to zero
        // and sends the new data for storage
        public const int HCP_ACCMODE_RESET_AND_HOLD = 1;// resets any previous data and holds the new data
        // without sending for storage
        public const int HCP_ACCMODE_SUM_AND_HOLD = 2;// sums to any previous data and holds the new data
        // without sending for storage
        public const int HCP_ACCMODE_END_AND_SEND = 3;// no further acquisition is done; sends
        // existing data for storage
        #endregion
        #region enum group3
        public const int HCP_CALTYPE_MIN = 23;// do not use
        public const int HCP_CALTYPE_GAIN = 24;// gain cal that may involved additional steps in dual gain modes
        public const int HCP_CALTYPE_EXTGAIN = 25;// extended gain cal - DYNAMIC_GAIN modes only
        public const int HCP_CALTYPE_GAINRATIO = 26;// gain ratio calibration - DUAL_READ modes only
        public const int HCP_CALTYPE_MAX = 27;// do not use
        #endregion
        #region enum group4
        public const int HCP_FRMINTRPT_FG_PULS0 = 0;
        public const int HCP_FRMINTRPT_FG_FVAL = 1;
        public const int HCP_FRMINTRPT_FG_MAX = 2;
        #endregion
        #region enum group5
        public const int HCP_GENSYNC_AC_ADU200 = 0;
        public const int HCP_GENSYNC_AC_NONE = 1;
        public const int HCP_GENSYNC_AC_MAX = 2;
        #endregion
        #region enum group6
        public const int HCP_U_QPI = 0;
        public const int HCP_U_QPIX = 1;
        public const int HCP_U_QPIFW = 2;// Codes for reading image diagnostic data
        public const int HCP_U_QPIRCPT = 3;// Receptor ID
        public const int HCP_U_QPIFRAME = 4;// Frame ID
        public const int HCP_U_QPITEMPS = 5;// Temperature measurement data
        public const int HCP_U_QPIVOLTS = 6;// Voltage measurement data
        public const int HCP_U_QPIDIAGDATA = 7;// Raw diagnostic pixel data for rad panels
        public const int HCP_U_QPI_MAX = 8;
        #endregion
        #region enum group7
        public const int HCP_CALACQ_OFST = 0;
        public const int HCP_CALACQ_GAIN = 1;
        public const int HCP_CALACQ_ANLG = 2;// analog offset
        public const int HCP_CALACQ_OFST_HI = 3;// CB offset HI_GAIN state
        public const int HCP_CALACQ_OFST_LO = 4;// CB offset LO_GAIN state
        public const int HCP_CALACQ_GAIN_HI = 5;// CB gain HI_GAIN state
        public const int HCP_CALACQ_GAIN_LO = 6;// CB gain LO_GAIN state
        public const int HCP_CALACQ_EXT0_HI = 7;// CB extended gain cal - ofst - HI_GAIN (DYN)
        public const int HCP_CALACQ_EXT0_LO = 8;// CB extended gain cal - ofst - LO_GAIN (FLG)
        public const int HCP_CALACQ_EXT1_HI = 9;// CB extended gain cal - xry1 - HI_GAIN (DYN)
        public const int HCP_CALACQ_EXT1_LO = 10;// CB extended gain cal - xry1 - LO_GAIN (FLG)
        public const int HCP_CALACQ_EXT2_HI = 11;// CB extended gain cal - xry2 - HI_GAIN (DYN)
        public const int HCP_CALACQ_EXT2_LO = 12;// CB extended gain cal - xry2 - LO_GAIN (FLG)
        public const int HCP_CALACQ_EXT3_HI = 13;// CB extended gain cal - xry3 - HI_GAIN (DYN)
        public const int HCP_CALACQ_EXT3_LO = 14;// CB extended gain cal - xry3 - LO_GAIN (FLG)
        public const int HCP_CALACQ_GN_RATIO = 15;// dual read calibration
        public const int HCP_CALACQ_MAX = 16;
        /// HCP_CALACQ_CALEND ->
        public const int HCP_CALACQ_CALEND = 100;// no acquisition involved - used at end of extended
        // gain cal to force the calibration call and retrieve
        // the HL_ratio
        public const int HCP_CALACQ_GREND = 101;// CB gain ratio cal
        #endregion

    }
    public enum RAD_FRAME_TYPE// Codes for FrameType in SQueryProgInfoFrame
    {
        RAD_FRAME_IDLE,
        RAD_FRAME_OFFSET,
        RAD_FRAME_EXPOSED,
        RAD_FRAME_DISABLED,
        RAD_FRAME_OFSDIS,
        RAD_FRAME_OTHER,
        RAD_FRAME_CANCEL,
        RAD_FRAME_TIMEOUT_PREP
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SCalCtrl
    {
        public int StructSize;// Initialize to sizeof(SCalCtrl)
        public int CtrlType;// specifies acquisition type requested
        public int AccMode;// specifies how frames are used relative to any previous acquisition
        public int NumFrames;// number of frames to acquire
        // zero default implies number calibration
        // frames or acquisition frames if rad gain
        public float GainRatio;// returned when CtrlType = HCP_CTRLTYPE_EXTCAL
        public int Reserved1;
        public int Reserved2;
        public int Reserved3;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SCheckLink
    {
        public int StructSize;// Initialize to sizeof(SOpenReceptorLink)
        public int ImgMedianVal;// result of check_link (input ignored) - image Median
        public float ImgStdDev;// result of check_link (input ignored) - image StdDev
        public int ImgMedLoLim;// FOR NORMAL OPERATION LEAVE AT ZERO DEFAULT.
        // lo limit of acceptable median - zero
        // implies 100 default
        public int ImgMedHiLim;// FOR NORMAL OPERATION LEAVE AT ZERO DEFAULT.
        // hi limit of acceptable median - zero
        // implies value derived from receptor
        // configuration
        public float ImgMedSDRatioLim;// FOR NORMAL OPERATION LEAVE AT ZERO DEFAULT.
        // Acceptable ratio Median / StdDev -
        // zero implies that it must not be below 2
        public int NumImgAcq;// FOR NORMAL OPERATION LEAVE AT ZERO DEFAULT.
        // MUST be 0 or 1 when using ChkLnkType below.
        // number of images to acquire - zero is
        // interpreted as 1 -- BUT this should NOT
        // be set to a value greater than 1 for rad
        // panels as it will override the special efficient
        // operation provided and contolled through
        // ChkLnkType below
        public int ChkLnkType;// FOR NORMAL OPERATION LEAVE AT ZERO DEFAULT.
        // NOTE: ignored if NumImgAcq > 1.
        // May be used to control check link type
        // using one of the values #defined above
        // HCP_CHKLNK_SHRT -- gets 1 frame only
        // HCP_CHKLNK_LONG -- gets 2 frames AND
        // updates the fixed frame rate field in
        // HcpConfig.ini if applicable.
        //
        // Default operation uses HCP_CHKLNK_LONG
        // for first call for each receptor index after
        // application launch. HCP_CHKLNK_SHRT is used
        // subsequently.
        public System.IntPtr ChkBufPtr;		// As of L05 build 18 the image buffer
        // pointer is returned to user application
        // If required be sure to INITIALIZE TO ZERO
        // and test the returned value.
        public int Reserved2;
        public int Reserved1;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SCorrectImage
    {
        public int StructSize;// Initialize to sizeof(SCorrectImage)
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding1;// padding with 64-bit compiler to match VS default alignment
        /// WORD*
        public System.IntPtr BufIn;
        public int BufInX;
        public int BufInY;
        /// WORD*
        public System.IntPtr BufOut;// Note that if the image manipulation to rotate 90 degrees is in use (see
        // vip_set_correction_settings), then BufOutX and BufOutY should
        // be set accordingly to accept the rotated image.
        public int BufOutX;
        public int BufOutY;
        public int CorrType;
        public int Reserved1;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SAnalogOffsetInfo
    {
        public int StructSize;// Initialize to sizeof(SAnalogOffsetInfo)
        public int AsicNum;
        public uint AnalogOfstElapsdTime;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding1;// padding with 64-bit compiler to match VS default alignment
        public System.IntPtr AsicOffsets;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SAnalogOffsetParams
    {
        public int StructSize;// Initialize to sizeof(SAnalogOffsetParams)
        public int TargetValue;
        public int Tolerance;
        public int MedianPercent;
        public float FracIterDelta;
        public int NumIterations;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SCalInfo
    {
        public int StructSize;// Initialize to sizeof(SCalInfo)
        public float OfstMedian;
        public float OfstStdDev;
        public float GainMedian;
        public float GainStdDev;
        public float GainScaling;
        public uint Time;/// unsigned int
        public int GainState;// Set when using dual read or dynamic gain
        // modes. Use one of the values defined
        // below GNSTATE_DYN_HI or
        // GNSTATE_FLG_LO
        public int Reserved1;
        public int Reserved2;
        public int Reserved3;
        public int Reserved4;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SCalInfoExGn
    {
        public int StructSize;// Initialize to sizeof(SCalInfoExGn)
        public int CallerID;// Set to EXGN_ID to identify struct type
        public int GainState;// Set to GNSTATE_DYN_HI or GNSTATE_FLG_LO
        public int XrayLevel;// Set to HCP_CTRLTYPE_EXTn where n = 0,1,2,3
        public int CbMedian;// Value returned
        public float CbStdDev;// Value returned
        public int ImgCnt;// Value returned
        public int BadFlagCnt;// Value returned
        public uint Time;// Value returned
        public float GainRatio;// Value returned
        public int Reserved1;
        public int Reserved2;
        public int Reserved3;
        public int Reserved4;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SCalLimits
    {
        public int StructSize;// Initialize to sizeof(SCalLimits)
        public int OfstLimitLo;
        public int OfstLimitHi;
        public int GainLimitHi;
        public int OfstPcntLimitLo;
        public int OfstPcntLimitHi;
        public int GainPcntLimitLo;
        public int GainPcntLimitHi;

        public int OfstPcntLimitLognLo; // these will be used for LO_GAIN with CB modes
        public int OfstPcntLimitLognHi;
        public int GainPcntLimitLognLo;
        public int GainPcntLimitLognHi;

        public int OfstPcntLimitHignLo; // these will be used for HI_GAIN with CB modes
        public int OfstPcntLimitHignHi;
        public int GainPcntLimitHignLo;
        public int GainPcntLimitHignHi;

    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SCorrections
    {
        public int StructSize;// Initialize to sizeof(SCorrections)

        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool Ofst;
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool Gain;
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool Dfct;
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool Line;// the next two settings are mode specific and are returned only for
        // CB mode types for the currently selected mode
        public int PixDataFormat;// ignored by vip_set_correction_settings
        public float GainRatio;// ignored by vip_set_correction_settings
        // Requests for gain, offset and defect images
        // always return the unmanipulated image.
        public int Rotate90;// Returned image is rotated 90 degrees
        public int FlipX;// Returned image is flipped in X direction
        // (mirrored about vertical axis)
        public int FlipY;// Returned image is flipped in Y direction
        public int Reserved1;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SGainScaling
    {
        public int StructSize;// Initialize to sizeof(SGainScaling)
        public int GainType;
        public int MaxLinearValue;
        public int PxSatValue;
        public int PxRepValue;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SHwConfig
    {
        public int StructSize;// Initialize to sizeof(SHwConfig)
        public int FrmIntrptSrc;// This defines frame interrupt source
        // for the FG module - PULS0 is the pulse
        // generator used to generate internal
        // frame starts. FVAL is FrameValid.
        public int GenSyncType;// This defines how AC handles Generator Sync.
        // ADU200 uses a USB board to interface to
        // generator. NONE implies no AC control
        // over generator. All signal_frame_starts
        // are treated as valid frames.
        public int TimeParam1;
        public System.IntPtr OffsetPtr;// used by analog offset calibration
        // otherwise must be NULL
        public int OffsetLen;
        public int ModeNum;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SModeInfo
    {
        public int StructSize;// Initialize to sizeof(SModeInfo)
        public int ModeNum;
        public int AcqType;// VIP_ACQ_TYPE_ACCUMULATION -
        // for DGS incorporates CB type see VIP_ACQ_MASK,
        // VIP_CB_MASK, VIP_CB_SHIFT at HcpErrors.h
        public float FrameRate;
        public float AnalogGain;
        public int LinesPerFrame;
        public int ColsPerFrame;
        public int LinesPerPixel;
        public int ColsPerPixel;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ModeDescription;/// char[256]
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string DirReadyModeDescription;/// char[256]
        public int DcdsEnable;
        public float MxAllowedFrameRate;
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool UserSync;
        public int AcqFrmCount;
        public int CalFrmCount;
        public int GainRoiUpperLeftX;
        public int GainRoiUpperLeftY;
        public int GainRoiLowerRightX;
        public int GainRoiLowerRightY;
        public int UncorrectablePixelRepValue;
        public int OffsetCalShift;
        public int MaxDefectRange;
        public int LeanBufferStatus;// in rad mode we may choose to allocate
        // fewer buffers and operate a sum
        // as we go mode when calibrating
        public int Reserved3;
        public int Reserved2;
        public int Reserved1;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding1;// padding with 64-bit compiler to match VS default alignment
        public System.IntPtr ExtInfoPtr;
        public int ExtInfoLen;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding2;// padding with 64-bit compiler to match VS default alignment
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SSysInfo
    {
        public int StructSize;// Initialize to sizeof(SSysInfo)
        public int NumModes;
        public int DfltModeNum;
        public int MxLinesPerFrame;
        public int MxColsPerFrame;
        public int MxPixelValue;
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool HasVideo;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string SysDescription;/// char[256]
        public int StartUpConfig;
        public int NumAsics;
        public int ReceptorType;
        public int BorderPixels;
        public int MxImageValue;// normally the same as MxPixelValue
        public System.IntPtr DeviceInfoPtr;// pointer to a SDeviceInfoN struct -
        // must be allocated by caller
        public int Reserved3;
        public int Reserved2;
        public int Reserved1;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding1;// padding with 64-bit compiler to match VS default alignment
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SDeviceInfo1
    {
        public int StructSize;// Initialize to sizeof(SDeviceInfo1)
        public int StructType;// User sets to the N value in SDeviceInfoN (here 1)
        public int ReceptorIndex;// User specifies the receptor index for which
        // device info is requested
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ReceptorConfigPath;// Returns the receptor config
        // path as determined when the link opens
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string MacAddress;// Returns the MAC address of the device
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string IpAddress;// Returns the IP address of the device
        // (if applicable)
        public int Reserved4;
        public int Reserved3;
        public int Reserved2;
        public int Reserved1;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SSysMode
    {
        public int StructSize;// Initialize to sizeof(SSysMode)
        public int SystemMode;// This is the system mode specified in the receptor configuration - not currently used by VCP
        public int CurrentReceptorIndex; // The currently selected receptor index (zero-based)
        public int CurrentMode;// The currently selected mode (zero-based)
        public int NumReceptorsOpen;// The number of receptors currently open (return only - ignored by vip_set_sys_mode)
        public int Reserved4;
        public int Reserved3;
        public int Reserved2;
        public int Reserved1;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SOpenReceptorLink
    {
        public int StructSize;// Initialize to sizeof(SOpenReceptorLink)
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding1;// padding with 64-bit compiler to match VS default alignment
        public System.IntPtr VcpDatPtr;// Internal Virtual CP use only - must be NULL
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string RecDirPath;
        public int TestMode;	// Normally zero - enables frame stamp which overwrites some pixels
        public int DebugMode;	// May be used to turn on debug verbosity
        public int RcptNum;
        public int MaxRcptCount;
        public int MaxModesPerRcpt;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding2;// padding with 64-bit compiler to match VS default alignment
        public System.IntPtr FgTargetPtr;// Normally zero. Custom use only by FG module
        public int BufferLen;	// Normally zero. Custom use only. Use in conjunction with FgTargetPtr
        public int SubModeBinX;// Normally zero. Custom use only. Specifies submode binning or sample rate in X.
        public int SubModeBinY;// Normally zero. Custom use only. Specifies submode binning or sample rate in Y.
        public int TimeoutBoostSec;// If permitted boosts the timeout used by frame grabber -- max 1000
        public int TimeoutBoostMsVcpCall;	// Allows the caller to increase the time allowed to wait for the call mutex - may prevent HCP_STATE_ERR
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool CallMutexOverride;	// Overrides the VCP entry point call mutex. This should only set if the VCP is always called from the same thread.
        public int FgCallbackFlag;		// set this value to HCP_FG_CALLBACK_FLAG
        // allows use of FgCallbackPtr as pointer to
        // callback function
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding3;// padding with 64-bit compiler to match VS default alignment
        public System.IntPtr FgCallbackPtr;		// pointer to a callback function [void FgCallbackPtr()]
        // which is called during acquisition
        // if the ethernet connection fails
        public int Reserved2;
        public int Reserved1;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SQueryErrorInfo
    {
        public int StructSize;
        public int ErrorCode;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ErrMsg;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ActionStr;
    }

    [StructLayoutAttribute(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 68)]
    public struct SQueryProgInfo// uType = HCP_U_QPI
    {
        [FieldOffsetAttribute(0)]
        public int StructSize;// Initialize to sizeof(SQueryProgInfo)
        [FieldOffsetAttribute(4)]
        public int NumFrames;
        [FieldOffsetAttribute(8)]
        public bool Complete;
        [FieldOffsetAttribute(9)]
        public int NumPulses;
        [FieldOffsetAttribute(13)]
        public bool ReadyForPulse;
        [FieldOffsetAttribute(14)]
        public int ProgLimit;// In some cases a limit is provided e.g.for progress bar upper bound
        [FieldOffsetAttribute(18)]
        public byte StatusStr;// Status message when available_ SizeConst=32
        [FieldOffsetAttribute(50)]
        public int Cancellation;
        [FieldOffsetAttribute(54)]
        public int Prepare;
        [FieldOffsetAttribute(58)]
        public int ApiCallFlag;
        [FieldOffsetAttribute(62)]
        public int Reserved1;
    }
    //[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    //public struct SQueryProgInfo// uType = HCP_U_QPI
    //{
    //    public int StructSize;// Initialize to sizeof(SQueryProgInfo)
    //    public int NumFrames;
    //    [MarshalAsAttribute(UnmanagedType.Bool)]
    //    public bool Complete;
    //    public int NumPulses;
    //    [MarshalAsAttribute(UnmanagedType.Bool)]
    //    public bool ReadyForPulse;
    //    public int ProgLimit;// In some cases a limit is provided e.g.for progress bar upper bound
    //    [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 32)]
    //    public string StatusStr;// Status message when available
    //    public int Cancellation;
    //    public int Prepare;
    //    public int ApiCallFlag;
    //    public int Reserved1;
    //}
    //[StructLayoutAttribute(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 46)]
    //unsafe public struct SQueryProgInfoEx
    //{
    //    [FieldOffsetAttribute(0)]
    //    public int StructSize;// Initialize to sizeof(SQueryProgInfoEx)
    //    [FieldOffsetAttribute(4)]
    //    public int NumFrames;
    //    [FieldOffsetAttribute(8)]
    //    public bool Complete;
    //    [FieldOffsetAttribute(9)]
    //    public int NumPulses;
    //    [FieldOffsetAttribute(13)]
    //    public bool ReadyForPulse;
    //    [FieldOffsetAttribute(14)]
    //    public byte Padding1;// padding with 64-bit compiler to match VS default alignment_SizeConst = 4
    //    [FieldOffsetAttribute(18)]
    //    public void* FgTargetPtr;//public System.IntPtr FgTargetPtr;// Normally zero. Custom use only by FG module
    //    [FieldOffsetAttribute(22)]
    //    public int BufferLen;// Normally zero. Custom use only. Use in conjunction with FgTargetPtr
    //    [FieldOffsetAttribute(26)]
    //    public int Reserved4;
    //    [FieldOffsetAttribute(30)]
    //    public int Reserved3;
    //    [FieldOffsetAttribute(34)]
    //    public int Reserved2;
    //    [FieldOffsetAttribute(38)]
    //    public int Reserved1;
    //    [FieldOffsetAttribute(42)]
    //    public byte Padding2;// padding with 64-bit compiler to match VS default alignment_SizeConst = 4
    //}
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SQueryProgInfoEx
    {
        public int StructSize;// Initialize to sizeof(SQueryProgInfoEx)
        public int NumFrames;
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool Complete;
        public int NumPulses;
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool ReadyForPulse;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding1;// padding with 64-bit compiler to match VS default alignment
        public System.IntPtr FgTargetPtr;// Normally zero. Custom use only by FG module
        public int BufferLen;// Normally zero. Custom use only. Use in
        // conjunction with FgTargetPtr
        public int Reserved4;
        public int Reserved3;
        public int Reserved2;
        public int Reserved1;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding2;// padding with 64-bit compiler to match VS default alignment
    }
    //[StructLayoutAttribute(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 13)]
    //public struct SQueryProgInfoFw// uType = HCP_U_QPIFW
    //{
    //    [FieldOffsetAttribute(0)]
    //    public int StructSize;// Initialize to sizeof(SQueryProgInfoFw)
    //    [FieldOffsetAttribute(4)]
    //    public int ProgressCurrent;
    //    [FieldOffsetAttribute(8)]
    //    public int ProgressLimit;
    //    [FieldOffsetAttribute(12)]
    //    public bool Complete;
    //}
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SQueryProgInfoFw// uType = HCP_U_QPIFW
    {
        public int StructSize;// Initialize to sizeof(SQueryProgInfoFw)
        public int ProgressCurrent;
        public int ProgressLimit;
        [MarshalAsAttribute(UnmanagedType.Bool)]
        public bool Complete;
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SQueryProgInfoRcpt
    {
        public int StructSize;// Initialize to sizeof(SQueryProgInfoRc)
        public int PanelType;
        public int FwVersion;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U2)]
        public ushort[] BoardSNbr;// 48 bits
        public ushort ReservedWd;// Restore word alignment
        public int ReservedRcpt2;
        public int ReservedRcpt1;
    }
    //[StructLayoutAttribute(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 28)]
    //public struct SQueryProgInfoRcpt
    //{
    //    [FieldOffsetAttribute(0)]
    //    public int StructSize;// Initialize to sizeof(SQueryProgInfoRc)
    //    [FieldOffsetAttribute(4)]
    //    public int PanelType;
    //    [FieldOffsetAttribute(8)]
    //    public int FwVersion;
    //    [FieldOffsetAttribute(12)]
    //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U2)]
    //    public ushort[] BoardSNbr;// 48 bits ,SizeConst = 3
    //    [FieldOffsetAttribute(18)]
    //    public ushort ReservedWd;// Restore word alignment
    //    [FieldOffsetAttribute(20)]
    //    public int ReservedRcpt2;
    //    [FieldOffsetAttribute(24)]
    //    public int ReservedRcpt1;
    //}
    //[StructLayoutAttribute(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 52)]
    //public struct SQueryProgInfoFrame// uType = HCP_U_QPIFRAME
    //{
    //    [FieldOffsetAttribute(0)]
    //    public int StructSize;// Initialize to sizeof(SQueryProgInfoFr)
    //    [FieldOffsetAttribute(4)]
    //    public int RcptFrameId;// also referred to as RecId
    //    [FieldOffsetAttribute(8)]
    //    public int Exposed;
    //    [FieldOffsetAttribute(12)]
    //    public uint AcqTime;
    //    [FieldOffsetAttribute(16)]
    //    public int FrameType;   // not valid for fluoro - returns -1
    //    [FieldOffsetAttribute(20)]
    //    public int Cancellation;// not valid for fluoro - returns -1
    //    [FieldOffsetAttribute(24)]
    //    public int Prepare;     // not valid for fluoro - returns -1
    //    [FieldOffsetAttribute(28)]
    //    public int ReadyForExposure; // not valid for fluoro - returns -1
    //    // THIS VALUE IS SET BY CALLER
    //    [FieldOffsetAttribute(32)]
    //    public int RequestedVcpBufferIdx; // This  is the *** "VcpId" ***
    //    // It must reference the VCP buffer index for which data are requested
    //    // If the HCP_U_QPI_CRNT_DIAG_DATA flag bit is set in the union type parameter,
    //    // or for rad modes, then it is ignored.
    //    [FieldOffsetAttribute(36)]
    //    public int GrbId; // refers to grabber (pleora) index
    //    [FieldOffsetAttribute(40)]
    //    public int Reserved3;
    //    [FieldOffsetAttribute(44)]
    //    public int Reserved2;
    //    [FieldOffsetAttribute(48)]
    //    public int Reserved1;
    //}
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SQueryProgInfoFrame// uType = HCP_U_QPIFRAME
    {
        public int StructSize;// Initialize to sizeof(SQueryProgInfoFr)
        public int RcptFrameId;// also referred to as RecId
        public int Exposed;
        public uint AcqTime;
        public int FrameType;   // not valid for fluoro - returns -1
        public int Cancellation;// not valid for fluoro - returns -1
        public int Prepare;     // not valid for fluoro - returns -1
        public int ReadyForExposure; // not valid for fluoro - returns -1
        // THIS VALUE IS SET BY CALLER
        public int RequestedVcpBufferIdx; // This  is the *** "VcpId" ***
        // It must reference the VCP buffer index for which data are requested
        // If the HCP_U_QPI_CRNT_DIAG_DATA flag bit is set in the union type parameter,
        // or for rad modes, then it is ignored.
        public int GrbId; // refers to grabber (pleora) index
        public int Reserved3;
        public int Reserved2;
        public int Reserved1;
    }
    //[StructLayoutAttribute(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 152)]
    //public struct SQueryProgInfoTemps
    //{
    //    [FieldOffsetAttribute(0)]
    //    public int StructSize;// Initialize to sizeof(SQueryProgInfoTh)
    //    [FieldOffsetAttribute(4)]
    //    public int NumSensors;
    //    [FieldOffsetAttribute(8)]
    //    public float[] Celsius;/// float[16],SizeConst = 16
    //    [FieldOffsetAttribute(72)]
    //    public int[] Reserved;//SizeConst = 16
    //    // THIS VALUE IS SET BY CALLER
    //    [FieldOffsetAttribute(136)]
    //    public int RequestedVcpBufferIdx; // This  is the *** "VcpId" ***
    //    // It must reference the VCP buffer index for which data are requested
    //    // If the HCP_U_QPI_CRNT_DIAG_DATA flag bit is set in the union type parameter,
    //    // or for rad modes, then it is ignored.
    //    [FieldOffsetAttribute(140)]
    //    public int Reserved3;
    //    [FieldOffsetAttribute(144)]
    //    public int Reserved2;
    //    [FieldOffsetAttribute(148)]
    //    public int Reserved1;
    //}
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SQueryProgInfoTemps
    {
        public int StructSize;// Initialize to sizeof(SQueryProgInfoTh)
        public int NumSensors;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.R4)]
        public float[] Celsius;/// float[16]
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I4)]
        public int[] Reserved;
        // THIS VALUE IS SET BY CALLER
        public int RequestedVcpBufferIdx; // This  is the *** "VcpId" ***
        // It must reference the VCP buffer index for which data are requested
        // If the HCP_U_QPI_CRNT_DIAG_DATA flag bit is set in the union type parameter,
        // or for rad modes, then it is ignored.
        public int Reserved3;
        public int Reserved2;
        public int Reserved1;
    }
    //[StructLayoutAttribute(LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 152)]
    //public struct SQueryProgInfoVolts// uType = HCP_U_QPIVOLTS
    //{
    //    [FieldOffsetAttribute(0)]
    //    public int StructSize;// Initialize to sizeof(SQueryProgInfoVo)
    //    [FieldOffsetAttribute(4)]
    //    public int NumSensors;
    //    [FieldOffsetAttribute(8)]
    //    public float[] Volts;//SizeConst = 16
    //    [FieldOffsetAttribute(72)]
    //    public int[] Reserved;//SizeConst = 16
    //    // THIS VALUE IS SET BY CALLER
    //    [FieldOffsetAttribute(136)]
    //    public int RequestedVcpBufferIdx; // This  is the *** "VcpId" ***
    //    // It must reference the VCP buffer index for which data are requested
    //    // If the HCP_U_QPI_CRNT_DIAG_DATA flag bit is set in the union type parameter,
    //    // or for rad modes, then it is ignored.
    //    [FieldOffsetAttribute(140)]
    //    public int Reserved3;
    //    [FieldOffsetAttribute(144)]
    //    public int Reserved2;
    //    [FieldOffsetAttribute(148)]
    //    public int Reserved1;
    //}

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SQueryProgInfoVolts// uType = HCP_U_QPIVOLTS
    {
        public int StructSize;// Initialize to sizeof(SQueryProgInfoVo)
        public int NumSensors;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.R4)]
        public float[] Volts;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I4)]
        public int[] Reserved;
        // THIS VALUE IS SET BY CALLER
        public int RequestedVcpBufferIdx; // This  is the *** "VcpId" ***
        // It must reference the VCP buffer index for which data are requested
        // If the HCP_U_QPI_CRNT_DIAG_DATA flag bit is set in the union type parameter,
        // or for rad modes, then it is ignored.
        public int Reserved3;
        public int Reserved2;
        public int Reserved1;
    }
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SDiagData
    {
        public int StructSize;
        public uint AcqTime;
        // ******************************************************************************
        // The following members are in the same order as the raw pixel data,
        // starting from pixel column 8:
        public ushort Protocol;
        public ushort PanelType;
        public ushort FwVersion;
        public ushort Exposed;
        public ushort RcptFrameId;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U2)]
        public ushort[] BoardSNbr;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U2)]
        public ushort[] TemperatureData;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U2)]
        public ushort[] VoltageData;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.U2)]
        public ushort[] Reserved;
        // EXPANDED for fluoro use
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.U2)]
        public ushort[] ReservedHw;		// (pixel 68..103) reserved for hardware expansion
        public int GrbId;				// (pixels 104,105) this is the Grabber (Pleora) frame ID
        public int VcpId;				// (pixels 106,107) this is the returned VCP ID - should match requested
        public int BadImageSize;		// (pixels 108,109) may be used when bad image pass through is operating
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 13, ArraySubType = UnmanagedType.I4)]
        public int[] ReservedSw;		// (pixels 110..135) reserved for software expansion
        // ******************************************************************************
        // THIS VALUE IS SET BY CALLER
        public int RequestedVcpBufferIdx; // This  is the *** "VcpId" ***
        // It must reference the VCP buffer index for which data are requested
        // If the HCP_U_QPI_CRNT_DIAG_DATA flag bit is set in the union type parameter,
        // or for rad modes, then it is ignored.
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I4)]
        public int[] Reserved1;
    }



    

    

    //[StructLayoutAttribute(LayoutKind.Sequential)]
    //public struct SQueryProgInfoVolts// uType = HCP_U_QPIVOLTS
    //{
    //    public int StructSize;// Initialize to sizeof(SQueryProgInfoVo)
    //    public int NumSensors;
    //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.R4)]
    //    public float[] Volts;
    //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I4)]
    //    public int[] Reserved;
    //    // THIS VALUE IS SET BY CALLER
    //    public int RequestedVcpBufferIdx; // This  is the *** "VcpId" ***
    //    // It must reference the VCP buffer index for which data are requested
    //    // If the HCP_U_QPI_CRNT_DIAG_DATA flag bit is set in the union type parameter,
    //    // or for rad modes, then it is ignored.
    //    public int Reserved3;
    //    public int Reserved2;
    //    public int Reserved1;
    //}

    //[StructLayoutAttribute(LayoutKind.Sequential)]
    //public struct SDiagData
    //{
    //    public int StructSize;
    //    public uint AcqTime;
    //    // ******************************************************************************
    //    // The following members are in the same order as the raw pixel data,
    //    // starting from pixel column 8:
    //    public ushort Protocol;
    //    public ushort PanelType;
    //    public ushort FwVersion;
    //    public ushort Exposed;
    //    public ushort RcptFrameId;
    //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U2)]
    //    public ushort[] BoardSNbr;
    //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U2)]
    //    public ushort[] TemperatureData;
    //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U2)]
    //    public ushort[] VoltageData;
    //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 28, ArraySubType = UnmanagedType.U2)]
    //    public ushort[] Reserved;
    //    // EXPANDED for fluoro use
    //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 36, ArraySubType = UnmanagedType.U2)]
    //    public ushort[] ReservedHw;		// (pixel 68..103) reserved for hardware expansion
    //    public int GrbId;				// (pixels 104,105) this is the Grabber (Pleora) frame ID
    //    public int VcpId;				// (pixels 106,107) this is the returned VCP ID - should match requested
    //    public int BadImageSize;		// (pixels 108,109) may be used when bad image pass through is operating
    //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 13, ArraySubType = UnmanagedType.I4)]
    //    public int[] ReservedSw;		// (pixels 110..135) reserved for software expansion
    //    // ******************************************************************************
    //    // THIS VALUE IS SET BY CALLER
    //    public int RequestedVcpBufferIdx; // This  is the *** "VcpId" ***
    //    // It must reference the VCP buffer index for which data are requested
    //    // If the HCP_U_QPI_CRNT_DIAG_DATA flag bit is set in the union type parameter,
    //    // or for rad modes, then it is ignored.
    //    [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I4)]
    //    public int[] Reserved1;
    //}

    //[System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    //[StructLayoutAttribute(LayoutKind.Explicit, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    //public struct UQueryProgInfo
    //{
    //    [FieldOffsetAttribute(0)]
    //    public SQueryProgInfo qpi;
    //    [FieldOffsetAttribute(0)]
    //    public SQueryProgInfoEx qpix;
    //    [FieldOffsetAttribute(0)]
    //    public SQueryProgInfoFw qpifw;
    //    //[FieldOffsetAttribute(0)]
    //    //public SQueryProgInfoRcpt qpircpt;
    //    //[FieldOffsetAttribute(0)]
    //    //public SQueryProgInfoFrame qpiframe;
    //    //[FieldOffsetAttribute(0)]
    //    //public SQueryProgInfoTemps qpitemps;
    //    //[FieldOffsetAttribute(0)]
    //    //public SQueryProgInfoVolts qpivolts;
    //    //[FieldOffsetAttribute(0)]
    //    //public SDiagData qpidiag;
    //}
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
    public struct UQueryProgInfo
    {
        public SQueryProgInfo qpi;
        public SQueryProgInfoEx qpix;
        public SQueryProgInfoFw qpifw;
        public SQueryProgInfoRcpt qpircpt;
        public SQueryProgInfoFrame qpiframe;
        public SQueryProgInfoTemps qpitemps;
        public SQueryProgInfoVolts qpivolts;
        public SDiagData qpidiag;
    }
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct SCalAcqData
    {
        public int StructSize;  // Initialize to sizeof(SCalAcqData)
        public int DataType;    // offset, gain, extended gain etc
        public int DataByteSize;// size of data (int=4; WORD=2)
        public int FrmX;
        public int FrmY;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding1;// padding with 64-bit compiler to match VS default alignment
        public System.IntPtr Data;
        public int NumFrames;
        public float HLratio;
        public int Reserved1;
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Padding2;// padding with 64-bit compiler to match VS default alignment
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SCallStatus
    {
        public int StructSize;// Initialize to sizeof(SCallStatus)
        /// SCallStatus*
        public System.IntPtr LinkedCS;// pointer to a CallStatus structure (to allow a linked list if required.) Initialize to NULL
        public int ErrorCode;// initialize to VIP_NO_ERR
        public int State;    // initialize to IDLE
        public int Timeout;  // initialize to 5000 (ms)
        // (Implementation not planned initially)
        public System.IntPtr SetStatePtr;// function pointer to the 'SetState' function
        // - initialize to the address of the SetState function.
        public System.IntPtr GetFuncPtr;// function pointer to the 'GetFunc' function
        // - initialize to the address of the GetFunc function.
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ErrorMsg;// (char[256] - initialize to "")
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ActionStr;// (char[256] - initialize to "")
        public System.IntPtr RelLogIt;// function pointer to the HcpLogIt
        public long Progress;
        public long Limit;
    }

    public enum HcpDllRef
    {
        HCP_DLL_REF_AC,
        HCP_DLL_REF_RC,
        HCP_DLL_REF_CR,
        HCP_DLL_REF_CA,
        HCP_DLL_REF_MC,
        HCP_DLL_REF_IO,
        HCP_DLL_REF_RP,
        HCP_DLL_REF_FG,
        HCP_DLL_REF_MAX
    }
}
