using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace TCT_OnlyDetector.Native
{
    public partial class NativeConstants
    {
        // this enum provides a reference for structure types used in calls
        // vip_fluoro_get_prms(..) and vip_fluoro_set_prms(..)
        public enum HcpFluoroStruct
        {
            HCP_FLU_SYS_PRMS,		// specifies pointer to a SFluoroSysPrms
            HCP_FLU_MODE_PRMS,		// specifies pointer to a SFluoroModePrms
            HCP_FLU_GRAB_PRMS,		// specifies pointer to a SGrbPrms
            HCP_FLU_SEQ_PRMS,		// specifies pointer to a SSeqPrms
            HCP_FLU_ACQ_PRMS,		// specifies pointer to a SAcqPrms
            HCP_FLU_LIVE_PRMS,		// specifies pointer to a SLivePrms
            HCP_FLU_THRESH_PRMS,	// specifies pointer to a SThreshSelect
            HCP_FLU_STATS_PRMS,		// specifies pointer to a SSeqStats
            HCP_FLU_REC_PRMS,		// not valid
            HCP_FLU_TIME_INIT,		// specifies pointer to a SSeqTimer
            HCP_FLU_INDEX_RANGE,	// specifies pointer to a SIndexRange
            HCP_FLU_MAX
        }
        // this enum is used in the VideoStatus member of the SLivePrms structure
        public enum HcpFluoroStatus
        {
            HCP_REC_IDLE,
            HCP_REC_PAUSED,
            HCP_REC_GRABBING,
            HCP_REC_RECORDING,
            HCP_REC_MAX,
        }
        // this enum is used to specify corrections type requested by the user
        // set the CorrType in SAcqPrms
        // only used in fluoro modes, ignored in rad mode
        public enum HcpCorrType
        {
            HCP_CORR_NONE,
            HCP_CORR_STD,
            HCP_CORR_MAX,
        }
        // this enum is used by the vip_fluoro_get_event_name
        public enum HcpFgEvent
        {
            HCP_FG_FRM_TO_DISP,
            HCP_FG_FRM_GRBD,
            HCP_FG_STRT_REC,
            HCP_FG_HOST_SYNC,
            HCP_FG_RAD_RESET_RELEASE, // Special for rad panel MVH 20060908
            HCP_FG_RAD_RESET_ASSERT,  // Additional for rad panel MVH 20060922
            HCP_FG_EVENT_MAX
        }
        // this structure is used in the vip_fluoro_init_sys(..) call
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct SFluoroSysPrms
        {
            public int StructSize;
            public int FileRev;// default to zero
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)]
            public string InitStr;// =NULL default
            public int TestMode;
            public int RecModeControl;
            public int TimeoutBoostMs;
            public int NumVideoThreads;// returned by device object - internal use only
            public int Reserved3;
            public int Reserved2;
            public int Reserved1;
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string Padding1;// padding with 64-bit compiler to match VS default alignment
        }

        // this structure is used in the vip_fluoro_init_mode(..) call
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SFluoroModePrms
        {
            public int StructSize;
            public int FrameX;
            public int FrameY;
            public int BinX;
            public int BinY;
            public int RecType;// =0 default
            public float FrmRate;// =0.0 default (if not needed)
            public int UserSync; // =0 default
            public int TrigSrc;  // =0 default
            public int TrigMode; // =0 default
            public System.IntPtr GrabPrms;  // =NULL default - not implemented
            public System.IntPtr TimingPrms;// =NULL default - not implemented
            [MarshalAsAttribute(UnmanagedType.LPStr)]
            public string FilePath;// =NULL default path to cnfg file
        }

        // this structure - currently unimplemented - is intended to be used to
        // initialize the grab buffers - i.e. buffers written directly by the
        // frame grabber
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SGrbPrms
        {
            public int StructSize;
            public int NumBuffers;
            public int GrbX;
            public int GrbY;
            public int ByteDepth;
        };
        // may be used to get or set parameters using vip_fluoro_set_prms
        /// SNUG_MEM_FLAG -> 44
        public const int SNUG_MEM_FLAG = 44;
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SSeqPrms
        {
            public int StructSize;
            public int NumBuffers;			// number request - a smaller number may be allocated
            // and returned at this same location
            public int SeqX;					// dflt = 0 interpret = grbX
            public int SeqY;					// dflt = 0 interpret = grbY
            public int SumSize;				// dflt = 0 interpret =1
            public int SampleRate;			// dflt = 0 interpret =1
            public int BinFctr;				// dflt = 0 interpret =1
            public int StopAfterN;			// dflt = 0
            public int OfstX;				// dflt = 0
            public int OfstY;				// dflt = 0
            public int RqType;				// must be zero
            public int SnugMemory;			// Arbitrary Value -- implies that the VCP will  
            // handle memory in its normal manner (once allocated, 
            // it stays allocated until VirtCp.dll detaches OR
            // CLSLNK_RELMEM flag set on close link).
            // SnugMemory=SNUG_MEM_FLAG -- implies that VCP will 
            // deallocate any excess memory above that needed 
            // to keep NumBuffers available.
        };

        // may be used when starting a grab to customize ot entire structure zeroed
        // Note that a pointer to a SLiveParams structure is returned. Access
        // to this normally should be handled more safely using a call to
        // vip_fluoro_get_prms(..).
        //
        // Values for StartUp:
        // IDLE, --> (Zero default) is converted to GRABBING
        // PAUSED,
        // GRABBING,
        // RECORDING
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SAcqPrms
        {
            public int StructSize;
            public int StartUp;				// dflt = 0 --> GRABBING (fluoro) PAUSED (rad)
            public int ReqType;				// internal use only - must be zero
            public int CorrType;				// dflt = 0 --> no corrections. Use to set required corrections in fluoro modes see HcpCorrType enum
            public System.IntPtr CorrFuncPtr;			// dflt = NULL; user should not set this
            public System.IntPtr ThresholdSelect;		// dflt = NULL; pointer to a structure set threshold select parameters
            public System.IntPtr CopyBegin;// dflt = NULL
            public System.IntPtr CopyEnd;  // dflt = NULL
            public int ArraySize;			// dflt = 0
            public int MarkPixels;			// dflt = 0 --> off
            public int FrameErrorTolerance;		// dflt = 0
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string Padding1;// padding with 64-bit compiler to match VS default alignment
            public System.IntPtr LivePrmsPtr;			// pointer to the SLivePrms structure is returned here
        };

        // A structure of this type is updated during the acquisition.
        //
        // Values for VideoStatus:
        // IDLE,
        // PAUSED,
        // GRABBING,
        // RECORDING
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SLivePrms
        {
            public int StructSize;
            public volatile int NumFrames;
            public volatile int BufIndex; // "VcpId" - index to buffer currently most recent for display
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string Padding1;// padding with 64-bit compiler to match VS default alignment
            public volatile System.IntPtr BufPtr;	// pointer to buffer currently most recent for display
            public volatile int VideoStatus;
            public volatile int ErrorCode;
            public volatile int RecId;	// frame ID created by receptor
            public volatile int GrbId;	// frame ID created by grabber (pleora)
            public volatile int Reserved3;
            public volatile int Reserved2;
            public volatile int Reserved1;
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string Padding2;// padding with 64-bit compiler to match VS default alignment
        };

        // not implemented currently
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SThreshSelect
        {
            public int StructSize;
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string Padding1;// padding with 64-bit compiler to match VS default alignment
            public System.IntPtr TimerPtr;
            public int ThreshHi;
            public int Left;  // ROI definition..
            public int Top;
            public int Right;
            public int Bottom;
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string Padding2;// padding with 64-bit compiler to match VS default alignment
        };

        // used to retrieve sequence statistics via a call to
        // vip_fluoro_get_prms(..)
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SSeqStats
        {
            public int StructSize;
            public int SmplFrms;
            public int HookFrms;
            public int CaptFrms;
            public int HookOverrun;
            public int StartIdx;
            public int EndIdx;
            public float CaptRate;
        };

        // used to initialize the time and retrieve the pointer to the
        // CTickCounter
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SSeqTimer
        {
            public int StructSize;
            [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string Padding1;// padding with 64-bit compiler to match VS default alignment
            public System.IntPtr SeqTimerPtr;
        };
        // used to determine the range indices which reference available images
        // stored on the receptor 
        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct SIndexRange
        {
            public int StructSize;
            public uint StartIndex;
            public uint EndIndex;
            public int ImgSizeX;
            public int ImgSizeY;
        };
    }
}
