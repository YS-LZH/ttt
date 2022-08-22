using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCT_OnlyDetector.Native
{
    public partial class NativeConstants
    {

        //////////////////////////////////////////////////////////////////////////////
        // keep sync'd with Error enum above above
        #region string[] HcpErrStrList
        static public string[] HcpErrStrList = new string[]{
	        "No Error",				//  0
	        "Communication Error",	//  1
	        "State Error",			//  2
	        "Initialization error OR Device not open",	//  3
	        "Data Error",			//  4
	        "Not Implemented Error",//  5
	        "Other Error",			//  6
	        "!!Unknown Error",		//  7
	        "No Data Error",		//  8
	        "Bad Ini File",			//  9
	        "Bad Gain Ratio",		// 10
	        "Bad Threshold Values",	// 11
	        "Bad File Path",		// 12
	        "File Open Error",		// 13
	        "File Read Error",		// 14
	        "File Write Error",		// 15
	        "No Server Error",		// 16
	        "Number of Calibration Frames exceeds Allocated Buffers",	// 17
	        "Memory Allocation Error",									// 18
	        "Video Not Initialized",									// 19
	        "Acquisition State: Not Ready",								// 20
	        "Directory Not Found",										// 21
	        "Error Initializing IMAGERs Directory",						// 22
	        "Error Selecting Mode",										// 23
	        "Function address not found in library",					// 24
	        "Sequence Not Saved",										// 25
	        "Error Initializing Video",									// 26
	        "Logic Error",												// 27
	        "Timeout in Data Thread - Possible Memory Leak",			// 28
	        "Bad Pointer",			// 29
	        "Timed out",			// 30
	        "!!Unknown Error",		// 31
	        "Setup Error",			// 32
	        "Drive Not Found",		// 33
	        "Function Address not found in Library",					// 34
	        "", // "Abort Operation" no message should be displayed		// 35
	        "Bad Drive Letter",		// 36
	        "Error Opening Device", // 37
	        "Not Enough Space on Device",	// 38
	        "Error Writing to Device",		// 39
	        "Media Size Not Supported",		// 40
	        "Header not found on flash card",//41
	        "Error Reading from Device",	// 42
	        "File read/write error",		// 43
	        "Logic Error",					// 44
	        "Too few Images for Calibration",//45
	        "Multiple headers found on Flash - Not Usable",				// 46
	        "Offset correction not accepted",							// 47
	        "Gain correction not accepted",								// 48
	        "Defect correction not accepted",							// 49
	        "Header Verification Problem." +
			        "This could result from changing the memory card while" +
			        " the link is open.Select 'Reset Link' from the" +
			        " Acquisition menu.",								// 50
	        "Error due to probable read-only ini file",					// 51
	        "Image dimensions do not match with ini file",				// 52
	        "Bad CB state selected",									// 53
	        "Unhandled exception in dll",								// 54
	        "Timed out waiting for ReadyForPulse",						// 55
	        "Operation Cancelled",			// 56
	        "Not enough modes allocated",	// 57
	        "Iteration limit reached, or convergence not achievable",		// 58
	        "Diagnostic data not recognized",	// 59
	        "Receptor frame number out of sequence",	// 60
	        "Receptor frame unexpected state",		// 61
	        "!!Unknown Error",				// 62
	        "!!Unknown Error",				// 63
	        "No Calibration Error",			// 64
	        "No Offset Calibration",		// 65
	        "No Gain Calibration Error",	// 66
	        "Command Processor is not Correction Capable",				// 67
	        "Device ambiguity - Multiple devices found, " +
			        "and unable to resolve which is required. Possibly " +
			        "because MAC address not set in ini file.",			// 68
	        "Unable to find the device with the specified MAC address. " +
			        "Yo may need to select another or create a new " +
			        "receptor directory for the device in use. (e.g. " +
			        "ViVA -- Acquisition>Receptor Select>New)",			// 69
	        "Multiple link open failed. Receptors must be supported " +
			        "by the same RP & FG dlls - the latter of type 'FgEx'",		// 70
	        "Open link failed AND unable to reselect the prior receptor --\r\n" +
	        "ALL RECEPTOR LINKS HAVE BEEN CLOSED",						// 71
	        "The selected device is already open. Check MAC address " +
	        "or other identifier in HcpConfig.ini",						// 72
	        "Calibration Error",			// 73
	        "!!Unknown Error",				// 74
	        "!!Unknown Error",				// 75
	        "!!Unknown Error",				// 76
	        "WARNING: Extended gain calibration completed, but problem gain bits exceed 10%", // 77_PaxscanL07
	        "WARNING: Captured image has unexpected size",				// 78_PaxscanL07
	        "WARNING: The number of frames specified in the call has " +
	        "been accepted.\r\nBUT errors could result with rad panels.",// 79
				        // This code is returned when the number of acqusition
				        // frames exceeds 1 or the number of calibration frames
				        // exceeds 4 if a fixed frame rate panel is detected.
	        "Function Not Exported",			// 80
	        "Sub-Module Not Found",			// 81
	        "No Mode Selected",				// 82
	        "Pleora Error",					// 83
	        "Frame Grabber Initialization Error",// 84
	        "Frame Grabber Error",			// 85
	        "Error Starting Grabber",		// 86
	        "Error Stopping Grabber",		// 87
	        "Error Starting Record Sequence",// 88
	        "Error Stopping Record Sequence",// 89
	        "Error Communicating With Frame Grabber",//90
	        "Data Transmission Error",		// 91
	        "No Event For Index",			// 92
	        "Error Occurred During Grab",	// 93
	        "Not Grabbing Error",			// 94
	        "Bad Parameter",				// 95
	        "Zero frames specified",		// 96
	        "Resource allocation problem",	// 97
	        "Bad Image Dimension",			// 98
	        "Frame Rate is Overridden",		// 99
	        "Receptor Not Supported",		//100
	        "Problem with receptor config file",	//101
	        "Inadequate buffer supplied",	//102
	        "Resource already allocated",	//103
	        "Unable to handle request",		//104
	        "Receptor Not Ready",			//105
	        "Pleora Connect Error\r\n"+
		        "For driver info, search for:\r\n"+
		        "'iPORT Driver Manual.pdf'."+
		        "A suggested IP address for your ethernet adapter is: "+
		        "192.168.2.11",				//106
	        "Internal Logic Error",			//107
	        "Serial transmission error - Please try again",				//108
	        "Storage device is not formatted correctly",				//109
	        "Driver Version Error",			//110
	        "Problem with structure or StrucSize member",				//111
	        "Dll Version Error",			//112
	        "Fixed Frame Rate Receptor",	//113
	        "Receptor temperature over threshold",//114_PaxscanL07
	        "Link speed error",				//115_PaxscanL07
	        "vip_sw_handshaking() not supported for acquisition in DualRead mode",//116_PaxscanL07
	        "ACE error or unsupported",		//117_PaxscanL07
	        "!!Unknown Error",				//118
	        "!!Unknown Error",				//119
	        "!!Unknown Error",				//120
	        "!!Unknown Error",				//121
	        "!!Unknown Error",				//122
	        "!!Unknown Error",				//123
	        "!!Unknown Error",				//124
	        "!!Unknown Error",				//125
	        "!!Unknown Error",				//126
	        "!!Unknown Error",				//127
	        "No Image",						//128
	        "UUU"// Unknown Error String - MUST BE LAST
        };
        #endregion
        #region consts
        /// INC_PAXERRORS_H -> 
        /// Error generating expression: 值不能為 null。
        ///參數名稱: node
        public const string INC_PAXERRORS_H = "";

        /// VIP_ANALOG_OFFSET_IMAGE -> 8
        public const int VIP_ANALOG_OFFSET_IMAGE = 8;

        /// VIP_PREVIEW_IMAGE -> 9
        public const int VIP_PREVIEW_IMAGE = 9;

        /// VIP_RAD_OFFSET_IMAGE -> 10
        public const int VIP_RAD_OFFSET_IMAGE = 10;

        /// HCP_FG_TARGET_FLAG -> 0xF3000000
        public const int HCP_FG_TARGET_FLAG = -218103808;

        /// HCP_RP_TARGET_FLAG -> 0xF4000000
        public const int HCP_RP_TARGET_FLAG = -201326592;

        /// HCP_RESEND_TARGET_FLAG -> 0xF5000000
        public const int HCP_RESEND_TARGET_FLAG = -184549376;

        /// HCP_XX_TARGET_MASK -> 0xFF000000
        public const int HCP_XX_TARGET_MASK = -16777216;

        /// HCP_NON_TARGET_MASK -> 0x00FFFFFF
        public const int HCP_NON_TARGET_MASK = 16777215;

        /// VIP_CURRENT_IMG_0 -> 0x10  | HCP_RP_TARGET_FLAG
        public const int VIP_CURRENT_IMG_0 = (16 | NativeConstants.HCP_RP_TARGET_FLAG);

        /// VIP_CURRENT_IMG_1 -> 0x11  | HCP_RP_TARGET_FLAG
        public const int VIP_CURRENT_IMG_1 = (17 | NativeConstants.HCP_RP_TARGET_FLAG);

        /// VIP_CURRENT_IMG_2 -> 0x12  | HCP_RP_TARGET_FLAG
        public const int VIP_CURRENT_IMG_2 = (18 | NativeConstants.HCP_RP_TARGET_FLAG);

        /// VIP_CURRENT_IMG_RAW -> 0x1F  | HCP_RP_TARGET_FLAG
        public const int VIP_CURRENT_IMG_RAW = (31 | NativeConstants.HCP_RP_TARGET_FLAG);

        /// VIP_OFFSET_IMG_0 -> 1
        public const int VIP_OFFSET_IMG_0 = 1;

        /// VIP_OFFSET_IMG_1 -> 0x21  | HCP_RP_TARGET_FLAG
        public const int VIP_OFFSET_IMG_1 = (33 | NativeConstants.HCP_RP_TARGET_FLAG);

        /// VIP_OFFSET_IMG_2 -> 0x22  | HCP_RP_TARGET_FLAG
        public const int VIP_OFFSET_IMG_2 = (34 | NativeConstants.HCP_RP_TARGET_FLAG);

        /// VIP_OFFSET_IMG_AV -> 0x2F  | HCP_RP_TARGET_FLAG
        public const int VIP_OFFSET_IMG_AV = (47 | NativeConstants.HCP_RP_TARGET_FLAG);

        /// HCP_STATE_HI -> 0
        public const int HCP_STATE_HI = 0;

        /// HCP_STATE_LO -> 1
        public const int HCP_STATE_LO = 1;

        /// HCP_STATE_SHIFT -> 8
        public const int HCP_STATE_SHIFT = 8;

        /// HCP_OFST_HI -> (HCP_STATE_HI << HCP_STATE_SHIFT)
        public const int HCP_OFST_HI = (NativeConstants.HCP_STATE_HI) << (NativeConstants.HCP_STATE_SHIFT);

        /// HCP_OFST_LO -> (HCP_STATE_LO << HCP_STATE_SHIFT)
        public const int HCP_OFST_LO = (NativeConstants.HCP_STATE_LO) << (NativeConstants.HCP_STATE_SHIFT);

        /// HCP_IMGTYPE_MASK -> 0x000000FF
        public const int HCP_IMGTYPE_MASK = 255;

        /// HCP_STATE_MASK -> 0x0000FF00
        public const int HCP_STATE_MASK = 65280;

        /// VIP_OFFSET_IMAGE_HI -> (VIP_OFFSET_IMAGE + HCP_OFST_HI)
        public const int VIP_OFFSET_IMAGE_HI = (NativeConstants.VIP_OFFSET_IMAGE + NativeConstants.HCP_OFST_HI);

        /// VIP_GAIN_IMAGE_HI -> (VIP_GAIN_IMAGE + HCP_OFST_HI)
        public const int VIP_GAIN_IMAGE_HI = (NativeConstants.VIP_GAIN_IMAGE + NativeConstants.HCP_OFST_HI);

        /// VIP_BASE_DEFECT_IMAGE_HI -> (VIP_BASE_DEFECT_IMAGE + HCP_OFST_HI)
        public const int VIP_BASE_DEFECT_IMAGE_HI = (NativeConstants.VIP_BASE_DEFECT_IMAGE + NativeConstants.HCP_OFST_HI);

        /// VIP_AUX_DEFECT_IMAGE_HI -> (VIP_AUX_DEFECT_IMAGE + HCP_OFST_HI)
        public const int VIP_AUX_DEFECT_IMAGE_HI = (NativeConstants.VIP_AUX_DEFECT_IMAGE + NativeConstants.HCP_OFST_HI);

        /// VIP_OFFSET_IMAGE_LO -> (VIP_OFFSET_IMAGE + HCP_OFST_LO)
        public const int VIP_OFFSET_IMAGE_LO = (NativeConstants.VIP_OFFSET_IMAGE + NativeConstants.HCP_OFST_LO);

        /// VIP_GAIN_IMAGE_LO -> (VIP_GAIN_IMAGE + HCP_OFST_LO)
        public const int VIP_GAIN_IMAGE_LO = (NativeConstants.VIP_GAIN_IMAGE + NativeConstants.HCP_OFST_LO);

        /// VIP_BASE_DEFECT_IMAGE_LO -> (VIP_BASE_DEFECT_IMAGE + HCP_OFST_LO)
        public const int VIP_BASE_DEFECT_IMAGE_LO = (NativeConstants.VIP_BASE_DEFECT_IMAGE + NativeConstants.HCP_OFST_LO);

        /// VIP_AUX_DEFECT_IMAGE_LO -> (VIP_AUX_DEFECT_IMAGE + HCP_OFST_LO)
        public const int VIP_AUX_DEFECT_IMAGE_LO = (NativeConstants.VIP_AUX_DEFECT_IMAGE + NativeConstants.HCP_OFST_LO);

        /// VIP_NO_ERR -> HCP_NO_ERR
        public const int VIP_NO_ERR = HCP_NO_ERR;

        /// VIP_COMM_ERR -> HCP_COMM_ERR
        public const int VIP_COMM_ERR = HCP_COMM_ERR;

        /// VIP_STATE_ERR -> HCP_STATE_ERR
        public const int VIP_STATE_ERR = HCP_STATE_ERR;

        /// VIP_DATA_ERR -> HCP_DATA_ERR
        public const int VIP_DATA_ERR = HCP_DATA_ERR;

        /// VIP_NO_DATA_ERR -> HCP_NO_DATA_ERR
        public const int VIP_NO_DATA_ERR = HCP_NO_DATA_ERR;

        /// VIP_NO_SRVR_ERR -> HCP_NO_SRVR_ERR
        public const int VIP_NO_SRVR_ERR = HCP_NO_SRVR_ERR;

        /// VIP_SETUP_ERR -> HCP_SETUP_ERR
        public const int VIP_SETUP_ERR = HCP_SETUP_ERR;

        /// VIP_NO_CAL_ERR -> HCP_NO_CAL_ERR
        public const int VIP_NO_CAL_ERR = HCP_NO_CAL_ERR;

        /// VIP_NO_OFFSET_CAL_ERR -> HCP_NO_OFFSET_CAL_ERR
        public const int VIP_NO_OFFSET_CAL_ERR = HCP_NO_OFFSET_CAL_ERR;

        /// VIP_NO_GAIN_CAL_ERR -> HCP_NO_GAIN_CAL_ERR
        public const int VIP_NO_GAIN_CAL_ERR = HCP_NO_GAIN_CAL_ERR;

        /// VIP_NO_CORR_CAP_ERR -> HCP_NO_CORR_CAP_ERR
        public const int VIP_NO_CORR_CAP_ERR = HCP_NO_CORR_CAP_ERR;

        /// VIP_NO_IMAGE_ERR -> HCP_NO_IMAGE_ERR
        public const int VIP_NO_IMAGE_ERR = HCP_NO_IMAGE_ERR;

        /// VIP_NOT_IMPL_ERR -> HCP_NOT_IMPL_ERR
        public const int VIP_NOT_IMPL_ERR = HCP_NOT_IMPL_ERR;

        /// VIP_OTHER_ERR -> HCP_OTHER_ERR
        public const int VIP_OTHER_ERR = HCP_OTHER_ERR;

        /// VIP_NO_LINK -> -1
        public const int VIP_NO_LINK = -1;

        /// VIP_ETHERNET_LINK -> 0
        public const int VIP_ETHERNET_LINK = 0;

        /// VIP_SERIAL_LINK -> 1
        public const int VIP_SERIAL_LINK = 1;

        /// VIP_COM1 -> 0
        public const int VIP_COM1 = 0;

        /// VIP_COM2 -> 1
        public const int VIP_COM2 = 1;

        /// VIP_COM3 -> 2
        public const int VIP_COM3 = 2;

        /// VIP_COM4 -> 3
        public const int VIP_COM4 = 3;

        /// VIP_DISPL_TST_PTTRN_STRTUP -> 0
        public const int VIP_DISPL_TST_PTTRN_STRTUP = 0;

        /// VIP_STANDALONE_STRTUP -> 1
        public const int VIP_STANDALONE_STRTUP = 1;

        /// VIP_ACTIVE_ACQ_STRTUP -> 2
        public const int VIP_ACTIVE_ACQ_STRTUP = 2;

        /// VIP_MOTHERBOARD_VER -> 0
        public const int VIP_MOTHERBOARD_VER = 0;

        /// VIP_SYS_SW_VER -> 1
        public const int VIP_SYS_SW_VER = 1;

        /// VIP_GLOBAL_CTRL_VER -> 2
        public const int VIP_GLOBAL_CTRL_VER = 2;

        /// VIP_GLOBAL_CTRL_FW_VER -> 3
        public const int VIP_GLOBAL_CTRL_FW_VER = 3;

        /// VIP_RECEPTOR_VER -> 4
        public const int VIP_RECEPTOR_VER = 4;

        /// VIP_RECEPTOR_FW_VER -> 5
        public const int VIP_RECEPTOR_FW_VER = 5;

        /// VIP_IPS_VER -> 6
        public const int VIP_IPS_VER = 6;

        /// VIP_VIDEO_OUT_VER -> 7
        public const int VIP_VIDEO_OUT_VER = 7;

        /// VIP_VIDEO_OUT_FW_VER -> 8
        public const int VIP_VIDEO_OUT_FW_VER = 8;

        /// VIP_COMMAND_PROC_VER -> 9
        public const int VIP_COMMAND_PROC_VER = 9;

        /// VIP_RECEPTOR_CONFIG_VER -> 10
        public const int VIP_RECEPTOR_CONFIG_VER = 10;

        /// VIP_CURRENT_IMAGE -> 0
        public const int VIP_CURRENT_IMAGE = 0;

        /// VIP_OFFSET_IMAGE -> 1
        public const int VIP_OFFSET_IMAGE = 1;

        /// VIP_GAIN_IMAGE -> 2
        public const int VIP_GAIN_IMAGE = 2;

        /// VIP_BASE_DEFECT_IMAGE -> 3
        public const int VIP_BASE_DEFECT_IMAGE = 3;

        /// VIP_AUX_DEFECT_IMAGE -> 4
        public const int VIP_AUX_DEFECT_IMAGE = 4;

        /// VIP_TEST_IMAGE -> 5
        public const int VIP_TEST_IMAGE = 5;

        /// VIP_RECEPTOR_TEST_IMAGE -> 6
        public const int VIP_RECEPTOR_TEST_IMAGE = 6;

        /// VIP_RECEPTOR_TEST_IMAGE_OFF -> 7
        public const int VIP_RECEPTOR_TEST_IMAGE_OFF = 7;

        /// VIP_SW_PREPARE -> 0
        public const int VIP_SW_PREPARE = 0;

        /// VIP_SW_VALID_XRAYS -> 1
        public const int VIP_SW_VALID_XRAYS = 1;

        /// VIP_SW_RADIATION_WARNING -> 2
        public const int VIP_SW_RADIATION_WARNING = 2;

        /// VIP_SW_RESET -> 3
        public const int VIP_SW_RESET = 3;

        /// VIP_SW_FRAME_SUMMING -> 4
        public const int VIP_SW_FRAME_SUMMING = 4;

        /// VIP_RAD_SCALE_NONE -> 0
        public const int VIP_RAD_SCALE_NONE = 0;

        /// VIP_RAD_SCALE_UP -> 1
        public const int VIP_RAD_SCALE_UP = 1;

        /// VIP_RAD_SCALE_DOWN -> 2
        public const int VIP_RAD_SCALE_DOWN = 2;

        /// VIP_RAD_SCALE_BOTH -> 3
        public const int VIP_RAD_SCALE_BOTH = 3;

        /// VIP_GAIN_SCALE_NONE -> 0
        public const int VIP_GAIN_SCALE_NONE = 0;

        /// VIP_GAIN_SCALE_EXPAND -> 1
        public const int VIP_GAIN_SCALE_EXPAND = 1;

        /// VIP_ACQ_TYPE_CONTINUOUS -> 0
        public const int VIP_ACQ_TYPE_CONTINUOUS = 0;

        /// VIP_ACQ_TYPE_ACCUMULATION -> 1
        public const int VIP_ACQ_TYPE_ACCUMULATION = 1;

        /// VIP_CB_NORMAL -> 0x0000
        public const int VIP_CB_NORMAL = 0;

        /// VIP_CB_DUAL_READ -> 0x0100
        public const int VIP_CB_DUAL_READ = 256;

        /// VIP_CB_DYNAMIC_GAIN -> 0x0200
        public const int VIP_CB_DYNAMIC_GAIN = 512;

        /// VIP_INVALID_ACQ_MODE_TYPE -> -1
        public const int VIP_INVALID_ACQ_MODE_TYPE = -1;

        /// VIP_VALID_XRAYS_N_FRAMES -> 0
        public const int VIP_VALID_XRAYS_N_FRAMES = 0;

        /// VIP_VALID_XRAYS_ALL_FRAMES -> 1
        public const int VIP_VALID_XRAYS_ALL_FRAMES = 1;

        /// VIP_AUTO_SENSE_N_FRAMES -> 2
        public const int VIP_AUTO_SENSE_N_FRAMES = 2;

        /// VIP_AUTO_SENSE_ALL_FRAMES -> 3
        public const int VIP_AUTO_SENSE_ALL_FRAMES = 3;

        /// VIP_CB_AUX_MODE_FLAG -> 500
        public const int VIP_CB_AUX_MODE_FLAG = 500;

        /// VIP_CB_BASE_MODE -> 0
        public const int VIP_CB_BASE_MODE = 0;

        /// VIP_CB_AUX_MODE_1 -> 1
        public const int VIP_CB_AUX_MODE_1 = 1;

        /// VIP_ACQ_MASK -> 0x00FF
        public const int VIP_ACQ_MASK = 255;

        /// VIP_CB_MASK -> 0x0F00
        public const int VIP_CB_MASK = 3840;

        /// VIP_CB_SHIFT -> 8
        public const int VIP_CB_SHIFT = 8;

        /// VIP_WL_MAPPING_LINEAR -> 0
        public const int VIP_WL_MAPPING_LINEAR = 0;

        /// VIP_WL_MAPPING_NORM_ATAN -> 1
        public const int VIP_WL_MAPPING_NORM_ATAN = 1;

        /// VIP_WL_MAPPING_CUSTOM -> 2
        public const int VIP_WL_MAPPING_CUSTOM = 2;

        /// VIP_AUTO_GAIN_CTRL -> 0x12
        public const int VIP_AUTO_GAIN_CTRL = 18;

        /// VIP_AUTO_GAIN_CTRL_ON -> 1
        public const int VIP_AUTO_GAIN_CTRL_ON = 1;

        /// VIP_AUTO_GAIN_CTRL_OFF -> 0
        public const int VIP_AUTO_GAIN_CTRL_OFF = 0;

        /// VIP_VERT_REVERSAL_OP -> 0x1d
        public const int VIP_VERT_REVERSAL_OP = 29;

        /// VIP_VERT_REVERSAL_ON -> 1
        public const int VIP_VERT_REVERSAL_ON = 1;

        /// VIP_VERT_REVERSAL_OFF -> 0
        public const int VIP_VERT_REVERSAL_OFF = 0;

        /// VIP_HORZ_REVERSAL_OP -> 0x1e
        public const int VIP_HORZ_REVERSAL_OP = 30;

        /// VIP_HORZ_REVERSAL_ON -> 1
        public const int VIP_HORZ_REVERSAL_ON = 1;

        /// VIP_HORZ_REVERSAL_OFF -> 0
        public const int VIP_HORZ_REVERSAL_OFF = 0;

        /// VIP_SYSTEM_MODE_NORMAL -> 0
        public const int VIP_SYSTEM_MODE_NORMAL = 0;

        /// VIP_SYSTEM_MODE_TEST -> 1
        public const int VIP_SYSTEM_MODE_TEST = 1;

        /// VIP_SYSTEM_MODE_FLASH_TEST -> 2
        public const int VIP_SYSTEM_MODE_FLASH_TEST = 2;

        /// GLOBAL_CONTROL_REGISTER -> 0
        public const int GLOBAL_CONTROL_REGISTER = 0;

        /// EXPOSE_OK_REGISTER -> 72
        public const int EXPOSE_OK_REGISTER = 72;

        /// SYS_SOFTWARE_FILE_STR -> "SystemSoftwareFile"
        public const string SYS_SOFTWARE_FILE_STR = "SystemSoftwareFile";

        /// RCPT_CONFIG_FILE_STR -> "ConfigDataFile"
        public const string RCPT_CONFIG_FILE_STR = "ConfigDataFile";

        /// BASE_DEFECT_IMAGE_STR -> "BaseDefectImage"
        public const string BASE_DEFECT_IMAGE_STR = "BaseDefectImage";

        /// AUX_DEFECT_IMAGE_STR -> "AuxDefectImage"
        public const string AUX_DEFECT_IMAGE_STR = "AuxDefectImage";

        /// GAIN_CAL_IMAGE_STR -> "GainCalImage"
        public const string GAIN_CAL_IMAGE_STR = "GainCalImage";

        /// OFFSET_CAL_IMAGE_STR -> "OffsetCalImage"
        public const string OFFSET_CAL_IMAGE_STR = "OffsetCalImage";

        /// SYS_CONFIG_FILE_STR -> "SystemDataFile"
        public const string SYS_CONFIG_FILE_STR = "SystemDataFile";

        /// GC_FW_FILE_STR -> "GcFirmware"
        public const string GC_FW_FILE_STR = "GcFirmware";

        /// RCPT_FW_FILE_STR -> "RcptFirmware"
        public const string RCPT_FW_FILE_STR = "RcptFirmware";

        /// VIDEO_OUT_FILE_STR -> "VideoOutFirmware"
        public const string VIDEO_OUT_FILE_STR = "VideoOutFirmware";

        /// DETAILED_MODE_DESCR -> "DetailedModeDesc"
        public const string DETAILED_MODE_DESCR = "DetailedModeDesc";
        #endregion

    }
    public partial class NativeConstants
    {
        public const int HCP_NO_ERR = 0;
        public const int HCP_COMM_ERR = 1;//VIP_COMM ERRCODE
        public const int HCP_STATE_ERR = 2;//;VIP_COMM ERRCODE
        public const int HCP_NOT_OPEN = 3;//						// was -1
        public const int HCP_DATA_ERR = 4;//VIP_COMM ERRCODE
        public const int HCP_NOT_IMPL_ERR = 5;//						// was 0x4000
        public const int HCP_OTHER_ERR = 6;//						// was 0x8000
        public const int HCP_DUM_007 = 7;//
        public const int HCP_NO_DATA_ERR = 8;//VIP_COMM ERRCODE
        public const int HCP_BAD_INI_FILE = 9;//						// was 3501
        public const int HCP_BAD_HL_RATIO = 10;//						// was 3502
        public const int HCP_BAD_THRESHOLD_VALUES = 11;//					// was 3503
        public const int HCP_BAD_FILE_PATH = 12;//						// was 3504
        public const int HCP_FILE_OPEN_ERR = 13;//
        public const int HCP_FILE_READ_ERR = 14;//
        public const int HCP_FILE_WRITE_ERR = 15;//
        public const int HCP_NO_SRVR_ERR = 16;//
        public const int HCP_NUMCAL_ERR = 17;//						// was 3300
        public const int HCP_MEMALLOC_ERR = 18;//						// was 3301
        public const int HCP_NOVIDEO_ERR = 19;//						// was 3302
        public const int HCP_NOTREADY_ERR = 20;//						// was 3303
        public const int HCP_DIR_NOT_FND = 21;//						// was 3304
        public const int HCP_INIT_ERR = 22;//						// was 3305
        public const int HCP_SEL_MODE_ERR = 23;//						// was 3306
        public const int HCP_NO_FNC_ADDR_ERR = 24;//						// was 3307
        public const int HCP_SEQ_SAVE_ERR = 25;//						// was 3308
        public const int HCP_VID_INIT_ERR = 26;//						// was 3309
        public const int HCP_LOGIC_ERR = 27;//						// was 3310
        public const int HCP_CALEND_TMOUT = 28;//						// was 3311
        public const int HCP_BAD_POINTER = 29;//						// was 3312
        public const int HCP_TIMEOUT = 30;//						// was 3313
        public const int HCP_DUM_031 = 31;//
        public const int HCP_SETUP_ERR = 32;//	// 0x0020 VIP_COMM ERRCODE
        public const int HCP_NO_DRIVE = 33;//						// was 3400
        public const int HCP_FUNC_ADDR = 34;//						// was 3401
        public const int HCP_ABORT_OPERATION = 35;//						// was 3402
        public const int HCP_BAD_DRIVE = 36;//						// was 3403
        public const int HCP_OPEN_DEVICE_ERR = 37;//						// was 3404
        public const int HCP_DEVICE_FULL = 38;//						// was 3405
        public const int HCP_WRITE_ERROR = 39;//						// was 3406
        public const int HCP_UNSUPPORTED_SIZE = 40;//						// was 3407
        public const int HCP_HEADER_NOT_FOUND = 41;//						// was 3408
        public const int HCP_READ_ERROR = 42;//						// was 3409
        public const int HCP_BAD_FILE = 43;//						// was 3410
        public const int HCP_LOGIC_ERROR = 44;//						// was 3411
        public const int HCP_TOO_FEW_IMS = 45;//						// was 3412
        public const int HCP_MULTIPLE_HDR = 46;//						// was 3413
        public const int HCP_OFST_ERR = 47;//						// was 3414
        public const int HCP_GAIN_ERR = 48;//						// was 3415
        public const int HCP_DFCT_ERR = 49;//						// was 3416
        public const int HCP_HDR_VERIFY_ERR = 50;//						// was 3417
        public const int HCP_VERIFY_INI_FILE_ERR = 51;//					// was 3418
        public const int HCP_IMG_DIM_ERR = 52;//						// was 3430
        public const int HCP_BAD_STATE_ERR = 53;//						// was 3431
        public const int HCP_UNHANDLED_EXCEP = 54;//						// was 3432
        public const int HCP_TIMEOUT_PANELREADY = 55;//					    // was 3433
        public const int HCP_OPERATION_CNCLD = 56;//						// was 3434
        public const int HCP_TOO_FEW_MODES = 57;//
        public const int HCP_MAX_ITER = 58;//
        public const int HCP_DIAG_DATA_ERR = 59;//
        public const int HCP_DIAG_SEQ_ERR = 60;//
        public const int HCP_DIAG_STATE_ERR = 61;//
        public const int HCP_DUM_062 = 62;//
        public const int HCP_DUM_063 = 63;//
        public const int HCP_NO_CAL_ERR = 64;//	
        public const int HCP_NO_OFFSET_CAL_ERR = 65;//	
        public const int HCP_NO_GAIN_CAL_ERR = 66;//  
        public const int HCP_NO_CORR_CAP_ERR = 67;//	
        public const int HCP_DEV_AMBIG = 68;//
        public const int HCP_MAC_NOT_FOUND = 69;//
        public const int HCP_MULTI_REC = 70;//
        public const int HCP_RESELECT_FAIL = 71;//
        public const int HCP_RECID_CONFLICT = 72;//
        public const int HCP_CAL_ERROR = 73;//
        public const int HCP_DUM_074 = 74;//
        public const int HCP_DUM_075 = 75;//
        public const int HCP_DUM_076 = 76;//
        public const int HCP_EXTGAIN_WARN = 77;//PaxscanL07
        public const int HCP_FRAME_SIZE_WARN = 78;//PaxscanL07
        public const int HCP_CALACQ_WARN = 79;// 
        public const int HCP_NULL_FUNC = 80;// 
        public const int HCP_NO_SUBMOD_ERR = 81;//
        public const int HCP_NO_MODE_SEL = 82;//
        public const int HCP_PLEORA_ERR = 83;//
        public const int HCP_FG_INIT_ERR = 84;//
        public const int HCP_GRAB_ERR = 85;//
        public const int HCP_STARTGRAB_ERR = 86;//
        public const int HCP_STOPGRAB_ERR = 87;//
        public const int HCP_STARTREC_ERR = 88;//
        public const int HCP_STOPREC_ERR = 89;//
        public const int HCP_NO_CONNECT_ERR = 90;//
        public const int HCP_DATA_TRANS_ERR = 91;//
        public const int HCP_NO_EVENT_ERR = 92;//
        public const int HCP_FRM_GRAB_ERR = 93;//
        public const int HCP_NOT_GRAB_ERR = 94;//
        public const int HCP_BAD_PARAM = 95;//
        public const int HCP_ZERO_FRM = 96;//
        public const int HCP_RES_ALLOC_ERR = 97;//
        public const int HCP_BAD_IMG_DIM = 98;//
        public const int HCP_FRM_DFLT = 99;//
        public const int HCP_REC_NOT_SUPP = 100;//
        public const int HCP_REC_CNFG_ERR = 101;//
        public const int HCP_SHORT_BUF_ERR = 102;//
        public const int HCP_RES_ALREADY_ALLOC = 103;//
        public const int HCP_BAD_REQ_ERR = 104;//
        public const int HCP_REC_NOT_READY = 105;//
        public const int HCP_PLR_CONNECT = 106;//
        public const int HCP_INTERNAL_LOGIC_ERROR = 107;//					// was 3599
        public const int HCP_PLR_SERIAL = 108;//
        public const int HCP_BAD_FORMAT = 109;//
        public const int HCP_DRV_VERS_ERR = 110;//
        public const int HCP_STRCT_ERR = 111;//
        public const int HCP_DLL_VERS_ERR = 112;//
        public const int HCP_FXD_RATE_ERR = 113;//
        public const int HCP_TEMP_OVER = 114;//PaxscanL07
        public const int HCP_LINK_SPEED_ERR = 115;//PaxscanL07
        public const int HCP_DREAD_ERR = 116;//PaxscanL07
        public const int HCP_ACE_ERR = 117;//PaxscanL07
        public const int HCP_DUM_118 = 118;//
        public const int HCP_DUM_119 = 119;//
        public const int HCP_DUM_120 = 120;//
        public const int HCP_DUM_121 = 121;//
        public const int HCP_DUM_122 = 122;//
        public const int HCP_DUM_123 = 123;//
        public const int HCP_DUM_124 = 124;//
        public const int HCP_DUM_125 = 125;//
        public const int HCP_DUM_126 = 126;//
        public const int HCP_DUM_127 = 127;//
        public const int HCP_NO_IMAGE_ERR = 128;	// 0x0080 VIP_COMM ERRCODE
        public const int HCP_MAX_ERR_CODE = 129;
    }
    public enum VistCapableList
    {

        /// VIP_VISTA_SUPPORT_UNKNOWN -> 0
        VIP_VISTA_SUPPORT_UNKNOWN = 0,

        VIP_VISTA_UNSUPPORTED,

        VIP_VISTA_SUPPORTED,
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Explicit)]
    public struct RegisterUnion
    {

        /// UCHAR->unsigned char
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public byte value8Bit;

        /// USHORT->unsigned short
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public ushort value16Bit;

        /// ULONG->unsigned int
        [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
        public uint value32Bit;
    }

}
