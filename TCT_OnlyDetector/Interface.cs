using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TCT_OnlyDetector.Native;

namespace TCT_OnlyDetector
{
    public class Interface
    {
        public enum ControlMode
        {
            ScoutView = 2,
            Pulse = 3,
            Contineous = 4,//目前無作用
            DryRun = 9,
            Calibration_EDS = 11,
            Calibration_STD = 12,
        }
        public enum MotorSequence
        {
            Asymmetric_600 = 2,
            Symmetric_300 = 4,
            //Symmetric_310 = 6,
            Symmetric_155 = 6
        }
        public enum Protocol
        {
            Adult = 1,
            Child = 2,
            Calibration = 8
        }
        public enum Region
        {
            Fast = 1,
            STD = 2,
            EDS = 4,
            Calibration = 8
        }
        public enum ReconType
        {
            BA,
            Digisens,
            Iner,
            TCT
        }
        public enum ReturnCode
        {
            Success = 0,
            FPD_NO_ERR = 0,
            FPD_INVALID_MODE_TYPE = 1,
            FPD_INVALID_IMAGE_SIZE = 2,
            FPD_GET_MODE_INFO_ERR = 3,
            FPD_CHECK_REC_LINK_ERR = 4,
            FPD_SET_PRMS_ERR = 5,
            FPD_SEND_ACQ_ERR = 6,
            FPD_TIMEOUT_SYNC_BD_ERR = 7,
            FPD_GRABBER_START_ERR = 8,
            FPD_RECORD_START_ERR = 9,
            FPD_TIMEOUT_WAIT_FRAME = 10,
            FPD_RECORD_STOP_ERR = 11,
            FPD_GRABBER_STOP_ERR = 12,

            FPD_NOT_OPEN_SYNC_BD_ERR = 13,
            FPD_NOT_GET_MODE_INFO_SYNC_BD_ERR = 14,
            FPD_NOT_DOCAL_SYNC_BD_ERR = 15,
            FPD_NOT_CHECK_REC_LINK_SYNC_BD_ERR = 16,
            FPD_NOT_SET_FRAME_RATE_SYNC_BD_ERR = 17,
            FPD_NOT_SEND_ACQ_SYNC_BD_ERR = 18,
            FPD_NOT_GRABBER_START_SYNC_BD_ERR = 19,
            FPD_NOT_RECORD_START_SYNC_BD_ERR = 20,
            FPD_NOT_RECORD_STOP_SYNC_BD_ERR = 21,
            FPD_NOT_GRABBER_STOP_SYNC_BD_ERR = 22,
            FPD_NOT_SAVE_FILE_SYNC_BD_ERR = 23,

            FPD_NOT_OPEN_USER_CANCEL = 24,
            FPD_NOT_GET_MODE_USER_CANCEL = 25,
            FPD_NOT_DOCAL_USER_CANCEL = 26,
            FPD_NOT_CHECK_REC_LINK_USER_CANCEL = 27,
            FPD_NOT_SET_FRAME_USER_CANCEL = 28,
            FPD_NOT_SEND_ACQ_USER_CANCEL = 29,
            FPD_NOT_GRABBER_START_USER_CANCEL = 30,
            FPD_NOT_RECORD_START_USER_CANCEL = 31,
            FPD_NOT_RECORD_STOP_USER_CANCEL = 32,
            FPD_NOT_GRABBER_STOP_USER_CANCEL = 33,
            FPD_NOT_SAVE_FILE_USER_CANCEL = 34,

            UARTSerialPortClosed = 35,
            XRaySerialPortClosed = 36,
            BeamLimiterSerialPortClosed = 37,
            CloseUARTSerialPortFail = 38,
            CloseXRaySerialPortFail = 39,
            CloseBeamLimiterSerialPortFail = 40,

            OpenUARTSerialPortFail = 41,
            OpenXRaySerialPortFail = 42,
            OpenBeamLimiterSerialPortFail = 43,
            FailSetMode = 44,
            FrameBufIndexError = 45,
            ReadSerialPortDataError = 46,
            CheckSendCommandError = 47,
            BL_HomingError = 48,
            BL_MovePositionError = 49,
            BL_INIError = 50,

            FailSendCommand = 51,
            CheckHomingError = 52,
            CreateDirectoryError = 53,
            DeleteDirectoryError = 54,
            NotEnoughDiskSpace = 55,
            ErrorInitializingDetectorBoard = 56,
            GetSyncBoardError = 57,
            CopyDetectorCalibrationFileError = 58,
            FPGA_WrongCheckSum = 59,
            FPGA_WrongDirection = 60,

            FPGA_TimeOutReceiveData = 61,
            FPGA_FeedbackError_Prepare_Setting_Before_Acquisition = 62,
            FPGA_FeedbackError_Start_Acquisition = 63,
            FPGA_System_Over_Temperature = 64,
            FPGA_XRAY_Interlock_Monitor_Error = 65,
            FPGA_XRAY_Ready_Status_Error = 66,
            FPGA_XRAY_On_Status_Error = 67,
            FPGA_XRAY_OverTemp_Status_Error = 68,
            FPGA_XRAY_Filament_Status_Error = 69,
            FPGA_XRAY_OVERVOLTAGE_Status_Error = 70,

            FPGA_XRAY_ARC_Status_Error = 71,
            FPGA_XRAY_Over_Current_Status_Error = 72,
            FPGA_XRAY_Line_Voltage_Monitor_Error = 73,
            FPGA_BeamLimiter_Left_Position_Error = 74,
            FPGA_BeamLimiter_Right_Position_Error = 75,
            FPGA_BeamLimiter_Top_Position_Error = 76,
            FPGA_BeamLimiter_Down_Position_Error = 77,
            FPGA_XRAY_Over_kVp = 78,
            FPGA_XRAY_Over_mA = 79,
            FPGA_System_Emergency_Stop = 80,

            FPGA_TimeOut_InitializeCheck = 81,
            FPGA_FeedbackError_Initialize_Checking = 82,
            FPGA_FeedbackError_Hardware_Start = 83,
            FPGA_FeedbackError_Background_Watcher_Alert_on_Idle = 84,
            FPGA_FeedbackError_Background_Watcher_Alert_on_Acquisition = 85,
            FPGA_FeedbackError_Emergency_Stop = 86,
            FPGA_FeedbackError_Home_Finish = 87,
            FPGA_FeedbackError_Ambient_Temperature = 88,
            FPGA_Motor_Error = 89,
            FPGA_XRAY_KVp_Error = 90,

            FPGA_XRAY_uA_Error = 91,
            FPGA_System_Lower_Temperature = 92,
            FPGA_ReturnDataLengthError = 93,
            Recon_LackingRawFile = 94,
            Recon_LibraryError = 95,
            Recon_USER_CANCEL = 96,
            FPGA_Patient_Emergency_Stop = 97,
            FPGA_FeedbackError_Power_ON = 98,
            FPGA_FeedbackError_Power_OFF = 99,
            User_Cancel_Repeat = 100,

            FPGA_Check_Address_Error = 101,
            FPGA_Time_Out_Wait_For_Finish = 102,
            FPGA_System_Not_Ready = 103,
            FPGA_System_Error = 104,
            FPGA_XRAY_Emit = 105,
            FPGA_System_Initial_Not_Ready = 106,
            FPGA_TimeOut_Ready_To_Start = 107,
            User_Cancel_Acquisition_Process = 108,
            FPGA_Motor_FanAction_Error = 109,
            FPD_SET_CORRECTION_SETTINGS_ERR = 110,

            FPD_OPEN_RECEPTOR_LINK_ERR = 111,
            FPD_SET_USER_SYNC_ERR = 112,
            FPD_GET_PRMS_ERR = 113,
            FPGA_Motor_RightSensor_Error = 114,
            FPGA_Motor_LeftSensor_Error = 115,
            FPGA_Detect_OnRightSide_Stop = 116,//試改成0 原本116 by han 2021/05/26 
            FPGA_Detect_OnLeftSide_Stop = 117,
            DISK_Error = 118,
            XRAY_UNDER_CURRENT_CONDITION = 119,
            XRAY_UNDER_VOLTAGE_CONDITION = 120,

            XRAY_WATCHDOG_TIMER_EXPIRED = 121,
            XRAY_POWER_LIMIT = 122,
            XRAY_DUTY_CYCLE_FAULT = 123,
            XRAY_COOL_DOWN_MODE = 124,

            FPD_GAIN_CAL_PREPARE_ERR,
            FPD_SW_HANDSHAKING_ERR,
            FPD_QUERY_PROG_INFO_ERR,
            FPD_GET_CAL_INFO_ERR,

            LACK_CALIBRATION_FILE,
            ExceptionError
        }
        #region ErrStrList
        public static string[] ErrStrList = new string[]{
            "No Error",				                                      //  FPD_NO_ERR_0
            "Error: Invalid mode type returned by vip_get_mode_info.",    //  FPD_INVALID_MODE_TYPE
            "Error: Invalid image size returned by vip_get_mode_info.",   //  FPD_INVALID_IMAGE_SIZE
            "vip_get_mode_info returned err",                             //  FPD_GET_MODE_INFO_ERR
            "vip_check_link returned err",                                //  FPD_CHECK_REC_LINK_ERR
            "vip_fluoro_set_prms returned err.",                          //  FPD_SET_PRMS_ERR
            "Fail to send ACQ command.",                                  //  FPD_SEND_ACQ_ERR 
            "Time out wait for serialPort response.",                     //  FPD_TIMEOUT_SYNC_BD_ERR
            "vip_fluoro_grabber_start returned err.",                     //  FPD_GRABBER_START_ERR
            "vip_fluoro_record_start returned err.",                      //  FPD_RECORD_START_ERR
            "Error: Timed out waiting for frames.",                       //  FPD_TIMEOUT_WAIT_FRAME_10
            "vip_fluoro_record_stop returned err.",                       //  FPD_RECORD_STOP_ERR 
            "vip_fluoro_grabber_stop returned err.",                      //  FPD_GRABBER_STOP_ERR
            "Get Sync Board Error before vip_open_receptor_link.",        // FPD_NOT_OPEN_SYNC_BD_ERR
            "Get Sync Board Error before vip_get_mode_info.",             // FPD_NOT_GET_MODE_INFO_SYNC_BD_ERR
            "Get Sync Board Error before doCal",                          // FPD_NOT_DOCAL_SYNC_BD_ERR
            "Get Sync Board Error before CheckRecLink.",                  // FPD_NOT_CHECK_REC_LINK_SYNC_BD_ERR
            "Get Sync Board Error before vip_set_frame_rate.",            // FPD_NOT_SET_FRAME_RATE_SYNC_BD_ERR
            "Get Sync Board Error before send ACQ command.",              // FPD_NOT_SEND_ACQ_SYNC_BD_ERR
            "Get Sync Board Error before vip_fluoro_grabber_start.",      // FPD_NOT_GRABBER_START_SYNC_BD_ERR
            "Get Sync Board Error before vip_fluoro_record_start.",       // FPD_NOT_RECORD_START_SYNC_BD_ERR_20
            "Get Sync Board Error before vip_fluoro_record_stop.",        // FPD_NOT_RECORD_STOP_SYNC_BD_ERR
            "Get Sync Board Error before vip_fluoro_grabber_stop.",       // FPD_NOT_GRABBER_STOP_SYNC_BD_ERR
            "Get Sync Board Error before save file.",                     // FPD_NOT_SAVE_FILE_SYNC_BD_ERR
            "User Cancel Acquisition Process before vip_open_receptor_link.",     // FPD_NOT_OPEN_USER_CANCEL
            "User Cancel Acquisition Process before vip_get_mode_info.",          // FPD_NOT_GET_MODE_USER_CANCEL
            "User Cancel Acquisition Process before doCal.",                      // FPD_NOT_DOCAL_USER_CANCEL
            "User Cancel Acquisition Process before vip_open_receptor_link.",     // FPD_NOT_CHECK_REC_LINK_USER_CANCEL
            "User Cancel Acquisition Process before vip_set_frame_rate.",         // FPD_NOT_SET_FRAME_USER_CANCEL
            "User Cancel Acquisition Process before send ACQ command.",           // FPD_NOT_SEND_ACQ_USER_CANCEL
            "User Cancel Acquisition Process before vip_fluoro_grabber_start.",   // FPD_NOT_GRABBER_START_USER_CANCEL_30
            "User Cancel Acquisition Process before vip_fluoro_record_start.",    // FPD_NOT_RECORD_START_USER_CANCEL
            "User Cancel Acquisition Process before vip_fluoro_record_stop.",     // FPD_NOT_RECORD_STOP_USER_CANCEL
            "User Cancel Acquisition Process before vip_fluoro_grabber_stop.",    // FPD_NOT_GRABBER_STOP_USER_CANCEL
            "User Cancel Acquisition Process before save file.",                   // FPD_NOT_SAVE_FILE_USER_CANCEL
            "UART Serial Port is Closed.",//UARTSerialPortClosed_35
            "XRay Serial Port is Closed.",//XRaySerialPortClosed_36
            "BeamLimiter Serial Port is Closed.",//BeamLimiterSerialPortClosed = 37,
            "E039 : Fail to Close SerialPort.",//CloseUARTSerialPortFail = 38,
            "Fail to Close XRay SerialPort.",//CloseXRaySerialPortFail = 39,
            "Fail to Close BeamLimiter SerialPort.",//CloseBeamLimiterSerialPortFail = 40,
            "E034 : Fail to Open System SerialPort.",//OpenUARTSerialPortFail = 41,
            "Fail to Open XRay SerialPort.",//OpenXRaySerialPortFail = 42,
            "Fail to Open BeamLimiter SerialPort.",//OpenBeamLimiterSerialPortFail = 43,
            "Fail to Set Mode.",//FailSetMode = 44,
            "Frame Buf Index Error.",//FrameBufIndexError = 45,
            "Fail to Read SerialPort Data.",//ReadSerialPortDataError = 46,
            "Fail to Check Send Command.",//CheckSendCommandError = 47,
            "Fail to Homing BeamLimiter.",//BL_HomingError = 48,
            "Fail to Move BeamLimiter to Position.",//BL_MovePositionError = 49,
            "Fail to initial BeamLimiter.",//BL_INIError = 50,
            "Fail to Send Command to SerialPort.",//FailSendCommand = 51,
            "Fail to Check Homing.",//CheckHomingError = 52,
            "E037 : Fail to Create Directory.",//CreateDirectoryError = 53,
            "Fail to Delete Directory.",//DeleteDirectoryError = 54,
            "E033 : Insufficient Disk Space.",//NotEnoughDiskSpace = 55,
            "Fail to Initializing Detector Board.",//ErrorInitializingDetectorBoard = 56,
            "Fail to Get Sync Board.",//GetSyncBoardError = 57,
            "E036 : Fail to Copy Detector Calibration File.",//CopyDetectorCalibrationFileError = 58,
            "FPGA Wrong Check Sum.",//WrongCheckSum = 59,
            "FPGA Wrong Direction.",//FPGA_WrongDirection = 60,
            "E020 : FPGA Time Out Receive Data.",//FPGA_TimeOutReceiveData = 61
            "FeedbackError_Prepare_Setting_Before_Acquisition.",//FPGA_FeedbackError_Prepare_Setting_Before_Acquisition = 62,
            "FeedbackError_Start_Acquisition.",//FPGA_FeedbackError_Start_Acquisition = 63,
            "System Over Temperature.",//FPGA_System_Over_Temperature = 64,
            "X-Ray Interlock Monitor Error.",//FPGA_XRAY_Interlock_Monitor_Error    = 65,
            "X-Ray Ready Status Error.",//FPGA_XRAY_Ready_Status_Error         = 66,
            "X-Ray On Status Error.",//FPGA_XRAY_On_Status_Error            = 67,
            "X-Ray Over Temparature. The X-Ray will be shut down now if the acquisition is processing.",//FPGA_XRAY_OverTemp_Status_Error      = 68,
            "X-Ray Filament Status Error.",//FPGA_XRAY_Filament_Status_Error         = 69,
            "X-Ray Over Voltage Status Error.",//FPGA_XRAY_OVERVOLTAGE_Status_Error   = 70,
            "X-Ray ARC Status Error.",//FPGA_XRAY_ARC_Status_Error           = 71,
            "X-Ray Over Current Status_Error.",//FPGA_XRAY_Over_Current_Status_Error  = 72,
            "X-Ray Line Voltage Monitor_Error.",//FPGA_XRAY_Line_Voltage_Monitor_Error      = 73,
            "BeamLimiter_Left_Position_Error.",//FPGA_BeamLimiter_Left_Position_Error  = 74,
            "BeamLimiter_Right_Position_Error.",//FPGA_BeamLimiter_Right_Position_Error = 75,
            "BeamLimiter_Top_Position_Error.",//FPGA_BeamLimiter_Top_Position_Error   = 76,
            "BeamLimiter_Down_Position_Error.",//FPGA_BeamLimiter_Down_Position_Error  = 77,
            "X-Ray Over kVp",//FPGA_XRAY_Over_kVp = 78,
            "X-Ray Over mA",//FPGA_XRAY_Over_mA = 79,
            "System Emergency Stop.",//FPGA_System_Emergency_Stop = 80,
            "TimeOut Initialize Check",//FPGA_TimeOut_InitializeCheck = 81,
            "FPGA FeedbackError Initialize_Checking",//FPGA_FeedbackError_Initialize_Checking = 82,
            "FPGA FeedbackError Hardware_Start",//FPGA_FeedbackError_Hardware_Start = 83,
            "FPGA FeedbackError Background_Watcher_Alert_on_Idle",//FPGA_FeedbackError_Background_Watcher_Alert_on_Idle = 84,
            "FPGA FeedbackError Background_Watcher_Alert_on_Acquisition",//FPGA_FeedbackError_Background_Watcher_Alert_on_Acquisition = 85,
            "FPGA FeedbackError Emergency_Stop",//FPGA_FeedbackError_Emergency_Stop = 86,
            "FPGA FeedbackError Home_Finish",//FPGA_FeedbackError_Home_Finish = 87,
            "FPGA FeedbackError Ambient_Temperature",//FPGA_FeedbackError_Ambient_Temperature = 88,
            "FPGA Motor Error",//FPGA_Motor_Error = 89,
            "FPGA XRAY KVp Error",//FPGA_XRAY_KVp_Error = 90,
            "FPGA XRAY uA Error",//FPGA_XRAY_uA_Error = 91,
            "FPGA System Lower Temperature",//FPGA_System_Lower_Temperature = 92,
            "FPGA Return Data Length Error",//FPGA_ReturnDataLengthError=93,
            "Recon Lacking Raw File",//Recon_LackingRawFile = 94,
            "Recon Library Error",//Recon_LackingRawFile = 95,
            "Recon process is terminated by user",//Recon_USER_CANCEL=96,
            "Patient Emergency Stop",//FPGA_Patient_Emergency_Stop = 97,
            "FPGA Feedback Error Power ON",//FPGA_FeedbackError_Power_ON=98,
            "FPGA Feedback Error Power OFF",//FPGA_FeedbackError_Power_OFF
            "User Cancel Repeat",//User_Cancel_Repeat
            "FPGA Check Address Error",//FPGA_Check_Address_Error=101,
            "FPGA Time Out Wait For Finish",//FPGA_Time_Out_Wait_For_Finish=102,
            "FPGA System Not Ready",//FPGA_System_Not_Ready=103,
            "FPGA System Error",//FPGA_System_Error=104,
            "FPGA XRAY Emit",//FPGA_XRAY_Emit = 105,
            "FPGA System Initial Not Ready",//FPGA_System_Initial_Not_Ready=106
            "E020 : FPGA TimeOut Ready To Start",//FPGA_TimeOut_Ready_To_Start=107
            "User Cancel Acquisition Process",//User_Cancel_Acquisition_Process=108
            "FPGA Motor FanAction Error",//FPGA_Motor_FanAction_Error=109
            "FPD SET CORRECTION SETTINGS ERROR",//FPD_SET_CORRECTION_SETTINGS_ERROR=110
            "E038 : Fail to connect to detector.",//FPD_OPEN_RECEPTOR_LINK_ERR=111
            "FPD_SET_USER_SYNC_ERR",//FPD_SET_USER_SYNC_ERR = 112
            "FPD_GET_PRMS_ERR",//FPD_GET_PRMS_ERR=113
            "E041 : Motor Right Sensor Error!",//FPGA_Motor_RightSensor_Error = 114,
            "E040 : Motor Left Sensor Error!",//FPGA_Motor_LeftSensor_Error = 115,
            "Detect On Right Side Stop",//FPGA_Detect_OnRightSide_Stop = 117,
            "Detect On Left Side Stop",//FPGA_Detect_OnLeftSide_Stop = 118,
            "DISK ERROR",
            "X-Ray under current condition. The X-Ray will be shut down now if the acquisition is processing.",
            "X-Ray under voltage condition. The X-Ray will be shut down now if the acquisition is processing.",
            "X-Ray Watchdog timer expired. The X-Ray will be shut down now if the acquisition is processing.",//WATCHDOG_TIMER_EXPIRED
            "X-Ray Power Limit. The X-Ray will be shut down now if the acquisition is processing.",
            "X-Ray Duty cycle Fault. The X-Ray will be shut down now if the acquisition is processing. Please wait 48 seconds for cool down X-ray tube!",
            "X-Ray Cool down mode. Please wait 48 seconds for cool down X-ray tube!",
            "FPD_GAIN_CAL_PREPARE_ERR",//FPD_GAIN_CAL_PREPARE_ERR= 114,
            "FPD_SW_HANDSHAKING_ERR",//FPD_SW_HANDSHAKING_ERR = 115
            "FPD_QUERY_PROG_INFO_ERR",//FPD_QUERY_PROG_INFO_ERR = 116
            "FPD_GET_CAL_INFO_ERR",//FPD_GET_CAL_INFO_ERR=117
            "LACK_CALIBRATION_FILE",
            "Exception Error!"//Error
        };
        #endregion
        /// <summary>This class is used to define serial port parameters.</summary>
        public class SerialPortPara
        {
            public string PortName;
            public int BaudRate = 4800;
            public int DataBits = 8;
            public StopBits StopBits = StopBits.One;
            public Parity Parity = Parity.None;
            public SerialPortPara(string PortName)
            {
                this.PortName = PortName;
                this.StopBits = StopBits.One;
                this.Parity = Parity.None;
                this.BaudRate = 9600;
                this.DataBits = 8;
            }
            public SerialPortPara(string PortName, bool FPGA)
            {
                this.PortName = PortName;
                this.StopBits = StopBits.One;
                this.Parity = Parity.None;
                if (FPGA)
                    this.BaudRate = 9600;
                else
                    this.BaudRate = 4800;
                this.DataBits = 8;
            }
            public SerialPortPara(string PortName, int BaudRate, int DataBits, StopBits StopBits, Parity Parity)
            {
                this.PortName = PortName;
                this.StopBits = StopBits;
                this.Parity = Parity;
                this.BaudRate = BaudRate;
                this.DataBits = DataBits;
            }
        }
        static string spaceName = MethodInfo.GetCurrentMethod().Module.Name;
        static string className = MethodInfo.GetCurrentMethod().ReflectedType.Name;
        //public static string LogFileName = frm_tACQ.LogFolder + "Interface.ini";//File to be write log on
        public static bool GetSyncBoardError = false;
        //public static tACQ.Properties.Settings settings = tACQ.Properties.Settings.Default;
        public static DateTime SendA3;
        #region SerialPort Open/close/setting/Check
        /// <summary>Set parameter to serialport and open them</summary><returns>open port result</returns>
        public static int InitialtPort(SerialPort SerialPort, SerialPortPara SerialPortPara, out string errorMessage)
        {
            //int returnCode = ClosetShell(SerialPort, out errorMessage);//檢查目前狀態，若已開啟則關閉之()
            //if (returnCode != (int)ReturnCode.Success) return returnCode;
            errorMessage = "";
            int returnCode = (int)ReturnCode.Success;
            if (!SerialPort.IsOpen)
            {
                SetPort(SerialPort, SerialPortPara);//Set parameters
                if (!OpenPort(SerialPort, out errorMessage))//Open Port
                    return returnCode = (int)ReturnCode.OpenUARTSerialPortFail;
            }
            else//本來已開啟
            {
                if (!CheckPort(SerialPort, SerialPortPara))
                {
                    returnCode = ClosePort(SerialPort, out errorMessage);
                    SetPort(SerialPort, SerialPortPara);
                    if (!OpenPort(SerialPort, out errorMessage))
                        return returnCode = (int)ReturnCode.OpenUARTSerialPortFail;
                }
                else
                    returnCode = (int)ReturnCode.Success;
            }
            return returnCode;
        }
        /// <summary>Close assigned serialport</summary><returns>report ClosePort success or not</returns>
        public static int ClosePort(SerialPort SerialPort, out string ErrorMessage)
        {
            int returnCode = (int)ReturnCode.Success;
            ErrorMessage = "";
            if (SerialPort.IsOpen)
            {
                try
                {
                    SerialPort.Close();
                    LogFile.Log(LogFile.LogFileName, "SerialPort closed.");

                    ErrorMessage = "";
                    return returnCode;
                }
                catch (Exception E)
                {
                    ErrorMessage = E.Message;
                    return returnCode = (int)ReturnCode.CloseUARTSerialPortFail;
                }
            }
            return returnCode;
        }
        /// <summary>Check the port's parameters</summary>
        public static bool CheckPort(SerialPort SerialPort, SerialPortPara SerialPortPara)
        {
            if (SerialPort.PortName == SerialPortPara.PortName &&
            SerialPort.BaudRate == SerialPortPara.BaudRate &&
            SerialPort.DataBits == SerialPortPara.DataBits &&
            SerialPort.StopBits == SerialPortPara.StopBits &&
            SerialPort.Parity == SerialPortPara.Parity)
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>Set the port's parameters before open</summary>
        public static void SetPort(SerialPort SerialPort, SerialPortPara SerialPortPara)
        {
            SerialPort.PortName = SerialPortPara.PortName;
            SerialPort.BaudRate = SerialPortPara.BaudRate;
            SerialPort.DataBits = SerialPortPara.DataBits;
            SerialPort.StopBits = SerialPortPara.StopBits;
            SerialPort.Parity = SerialPortPara.Parity;
        }
        /// <summary>Open assigned serialport</summary> <param name="SerialPort">SerialPort to be open</param> <returns>report open port success or not</returns>
        public static bool OpenPort(SerialPort SerialPort, out string ErrorMessage)
        {
            bool result = false;
            try
            {
                SerialPort.Open();
                ErrorMessage = "";
                result = true;
            }
            catch (Exception E)
            {
                ErrorMessage = E.Message;
            }
            return result;
        }
        #endregion

        #region public command
        /// <summary>開啟port,設定port參數,設定FPD參數,執行FPD Process,關閉port,加入Beam Limiter control</summary>
        public static int DoACQ(SerialPort UARTPort, SerialPort BLPort, SerialPort XRAYPort, int Mode, int SEQNo, int FramNum, int KVp, int mA)
        {
            string EOF = "==============================EOF===============================";
            int Result = (int)Interface.ReturnCode.Success;
            string errormessage = "";
            FPD._prograss = 0;
            int waitTime = 0;
            //FPD.XRAYEmitOn = new XRAYStatus(FPGA.Status.Emit_On);
            string methodName = MethodInfo.GetCurrentMethod().Name;
            string LogFormatedString = className + "." + methodName;
            LogFormatedString = LogFormatedString.PadRight(Acquisition.PadLength);

            //硬體設定
            //if (settings.FPGA)
            if (true)
            {
                //開啟port
                Interface.SerialPortPara SerialPortPara = new Interface.SerialPortPara("com1", 9600, 8, StopBits.One, Parity.None);
                if (!UARTPort.IsOpen)
                    Result = Interface.InitialtPort(UARTPort, SerialPortPara, out errormessage);//開啟UART port
                Result = (int)Interface.ReturnCode.Success;
                if (!(Result == (int)Interface.ReturnCode.Success))
                {
                    LogFile.Log(Acquisition.FPGALog, LogFormatedString + ":" + "Initial SerialPortPara Result:" + errormessage);
                    return Result;
                }
                //write firmware version first
                //LogFile.Log(Acquisition.FPGALog, LogFormatedString + ":" + "Firmware Version : " + frm_tACQ.FirmwareVersion + "( " + DateTime.Now.ToString() + " )");
                /*if (frm_Acquisition.doRaliabilityTest)
                {
                    #region Reliability setting
                    for (int i = 0; i < 3; i++)
                    {
                        frm_tACQ.WaitA9 = true;//set this flag on, which used to judge if get A9 is effective on FPGA_serialPort_DataReceived
                        frm_Acquisition.A9FeedBack = false;
                        if (frm_Acquisition.doRaliabilityTest)
                        {
                            System.Threading.Thread.Sleep(500);
                            FPGA.SendFPGACommand(UARTPort, FPGA.DirectionType.Write, FPGA.FPGACommand.Reliability, 0x01, 0x00, 0x00);
                        }
                        else
                        {
                            FPGA.SendFPGACommand(UARTPort, FPGA.DirectionType.Write, FPGA.FPGACommand.Reliability, 0x00, 0x00, 0x00);
                        }
                        while (!frm_Acquisition.A9FeedBack)
                        {
                            if (frm_Acquisition.bgw_ACQ.CancellationPending == true)
                                return (int)Interface.ReturnCode.User_Cancel_Acquisition_Process;
                            if (waitTime > 4000)
                                break;

                            Thread.Sleep(100);
                            waitTime += 100;
                        }
                        if (frm_Acquisition.A9FeedBack)
                            break;
                    }
                    if (!frm_Acquisition.A9FeedBack)
                        return (int)Interface.ReturnCode.FPGA_TimeOutReceiveData;
                    #endregion
                }
                //send command
                byte region = 0x01;//fast
                if (frm_Acquisition.SelectMotorSequence == Interface.MotorSequence.Asymmetric_600)
                {
                    region = 0x04;
                }
                else if (frm_Acquisition.SelectMotorSequence == Interface.MotorSequence.Symmetric_155)
                {
                    region = 0x01;
                }
                else
                {
                    region = 0x02;
                }*/
                //frm_Acquisition.PassBackgroundCommunication = true;
                for (int i = 0; i < 3; i++)
                {
                    /*
                    if (frm_Acquisition.bgw_ACQ.CancellationPending == true)
                        return Result = (int)Interface.ReturnCode.User_Cancel_Acquisition_Process;
                    frm_Acquisition.A3FeedBack = false;
                    frm_Acquisition.ReceivedA5 = false;
                    frm_tACQ.WaitA3 = true;//set this flag on, which used to judge if get A3 is effective on FPGA_serialPort_DataReceived
                    frm_tACQ.WaitA5 = true;//set this flag on, which used to judge if get A5 is effective on FPGA_serialPort_DataReceived
                    byte data2 = FPGA.ComposeByte((byte)((frm_Acquisition.isAdult == true) ? (int)Protocol.Adult : (int)Protocol.Child), region);
                    Utility.FPGA.SendFPGACommand(UARTPort, FPGA.DirectionType.Write, FPGA.FPGACommand.Prepare_Setting_Before_Acquisition, Convert.ToByte(Mode), data2, 0x00);
                    SendA3 = DateTime.Now;//紀錄系統時間
                    waitTime = 0;
                    while (!frm_Acquisition.A3FeedBack)
                    {
                        if (frm_Acquisition.bgw_ACQ.CancellationPending == true)
                            return Result = (int)Interface.ReturnCode.User_Cancel_Acquisition_Process;
                        if (waitTime > 4000)
                            break;
                        Thread.Sleep(100);
                        waitTime += 100;
                    }
                    if (!frm_Acquisition.A3FeedBack)
                    {
                        Utility.FPGA.SendFPGACommand(UARTPort, FPGA.DirectionType.Write, FPGA.FPGACommand.Reset, 0x00, 0x00, 0x00);
                        Thread.Sleep(7000);
                        continue;
                    }
                    else
                    {
                        int WaitForA5TimeOut = 15 * 1000;//unit:ms
                        int TimeElasped = 0;

                        while (!frm_Acquisition.ReceivedA5)
                        {
                            if (frm_Acquisition.bgw_ACQ.CancellationPending == true)
                                return Result = (int)Interface.ReturnCode.User_Cancel_Acquisition_Process;
                            if (TimeElasped >= WaitForA5TimeOut)
                                break;
                            Thread.Sleep(100);
                            TimeElasped += 100;
                        }
                        if (frm_Acquisition.ReceivedA5)
                        {
                            break;
                        }
                        else
                        {
                            Utility.FPGA.SendFPGACommand(UARTPort, FPGA.DirectionType.Write, FPGA.FPGACommand.Reset, 0x00, 0x00, 0x00);
                            Thread.Sleep(7000);
                            continue;
                        }
                    }*/
                }
                /*if (!frm_Acquisition.A3FeedBack)
                    return Result = (int)Interface.ReturnCode.FPGA_TimeOutReceiveData;
                if (!frm_Acquisition.ReceivedA5)
                    return Result = (int)Interface.ReturnCode.FPGA_FeedbackError_Prepare_Setting_Before_Acquisition;*/
            }
            else
            {
                /*
                #region UARTPort
                //開啟port
                //Interface.SerialPortPara SerialPortPara = new Interface.SerialPortPara(settings.SyncBoardPortName, settings.FPGA);
                Result = Interface.InitialtPort(UARTPort, SerialPortPara, out errormessage);//開啟UART port
                if (!(Result == (int)Interface.ReturnCode.Success))
                {
                    LogFile.Log(LogFile.LogFileName, "InitialtShell of UARTSerialPortPara Result:" + errormessage);
                    return Result;
                }
                if (frm_Acquisition.LastScanMode != frm_Acquisition.acquireMode)
                {
                    if (Mode != (int)ControlMode.ScoutView)//Homing and check while run Dry run and Rotary mode
                    {
                        Result = UART.HomeCheck(UARTPort);
                        if (!(Result == (int)Interface.ReturnCode.Success))
                        {
                            LogFile.Log(LogFile.LogFileName, "UARTPort Homing and check Result:" + Result.ToString());
                            return Result;
                        }
                    }
                    UART.tShellPara tShellPara;
                    //設定port參數
                    tShellPara = new UART.tShellPara(Mode, SEQNo, FramNum);
                    Result = UART.SetParameter(UARTPort, tShellPara);
                    if (!(Result == (int)Interface.ReturnCode.Success))
                    {
                        LogFile.Log(LogFile.LogFileName, "UARTPort SetParameter Result:" + Result.ToString());
                        LogFile.Log(LogFile.LogFileName, EOF);
                        return Result;
                    }
                    //here start to listen emergency stop, from now on the process may be terminated because of GetSyncBoardError
                    GetSyncBoardError = false;
                    //         UARTPort.DataReceived += new SerialDataReceivedEventHandler(EmergencyWhatcher);
                }
                #endregion
                #region XRAY
                /*if (Mode != 9)//Set kvp and ma while mode!=dryrun
            {
                if (!GetSyncBoardError)
            {
                Interface.SerialPortPara XRAYSerialPortPara = new Interface.SerialPortPara(frm_Acquisition.settings.XRAYPortName, 9600, 8, StopBits.One, Parity.Even);
                Result = Interface.InitialtShell(XRAYPort, XRAYSerialPortPara, out errormessage);//開啟 port
            if (!(Result == (int)Interface.ReturnCode.Success))
            {
                    LogFile.Log(Interface.LogFileName, "InitialtShell of XRAY SerialPort Result:" + errormessage);
                LogFile.Log(LogFileName, EOF);
                return Result;
            }
                SRI.Go(XRAYPort, true, mA, KVp, 1, true);
            }*/
                /*
                if (Mode != (int)ControlMode.DryRun)//Set kvp and ma while mode!=dryrun
                {
                    if (!GetSyncBoardError)
                    {
                        Interface.SerialPortPara XRAYSerialPortPara = new Interface.SerialPortPara("XRAYSerialPortPara", 9600, 8, StopBits.One, Parity.Even);
                        Result = Interface.InitialtPort(XRAYPort, XRAYSerialPortPara, out errormessage);//開啟 port
                        if (!(Result == (int)Interface.ReturnCode.Success))
                        {
                            LogFile.Log(LogFile.LogFileName, "InitialtShell of XRAY SerialPort Result:" + errormessage);
                            LogFile.Log(LogFile.LogFileName, EOF);
                            return Result;
                        }
                    }
                    else
                    {
                        LogFile.Log(LogFile.LogFileName, "GetSyncBoardError before Initial XRAYPort");
                        LogFile.Log(LogFile.LogFileName, EOF);
                        return Result = (int)Interface.ReturnCode.GetSyncBoardError;
                    }
                    if (!GetSyncBoardError)
                    {
                        SRI.Go(XRAYPort, true, mA, KVp, 1, true);
                    }
                    else
                    {
                        LogFile.Log(LogFile.LogFileName, "GetSyncBoardError before SRI.Go");
                        LogFile.Log(LogFile.LogFileName, EOF);
                        return Result = (int)Interface.ReturnCode.GetSyncBoardError;
                    }
                }*/
                #endregion
                /*
                #region Beam Limiter
                if (settings.BeamLimiterSwitch)
                {
                    if (Mode != (int)ControlMode.DryRun)
                    {
                        Interface.SerialPortPara BLSerialPortPara = new Interface.SerialPortPara(frm_Acquisition.settings.BeamLimiterPortName, 14400, 8, StopBits.One, Parity.None);
                        Result = Interface.InitialtPort(BLPort, BLSerialPortPara, out errormessage);//開啟BL port
                        if (!(Result == (int)Interface.ReturnCode.Success))
                        {
                            LogFile.Log(LogFile.LogFileName, "InitialtShell of BLSerialPort Result:" + errormessage);
                            LogFile.Log(LogFile.LogFileName, EOF);
                            return Result;
                        }
                        Result = BeamLimiter.InitializeBL(BLPort, BeamLimiter.FOV.Homing);
                        if (!(Result == (int)Interface.ReturnCode.Success))
                        {
                            LogFile.Log(LogFile.LogFileName, "InitializeBL of BLSerialPort Result:" + errormessage);
                            LogFile.Log(LogFile.LogFileName, EOF);
                            return Result;
                        }
                        if (frm_Acquisition.nResolution == 1 || frm_Acquisition.nResolution == 2 || frm_Acquisition.nResolution == 3)//asymmetric
                        {
                            Result = BeamLimiter.InitializeBL(BLPort, BeamLimiter.FOV.Asymmetric);
                            if (!(Result == (int)Interface.ReturnCode.Success))
                            {
                                LogFile.Log(LogFile.LogFileName, "InitializeBL of BLSerialPort Result:" + errormessage);
                                LogFile.Log(LogFile.LogFileName, EOF);
                                return Result;
                            }
                        }
                        if (frm_Acquisition.nResolution == 4 || frm_Acquisition.nResolution == 5 || frm_Acquisition.nResolution == 6 ||
                            frm_Acquisition.nResolution == 7 || frm_Acquisition.nResolution == 8 || frm_Acquisition.nResolution == 9)//symmetric
                        {
                            Result = BeamLimiter.InitializeBL(BLPort, BeamLimiter.FOV.Symmetric);
                            if (!(Result == (int)Interface.ReturnCode.Success))
                            {
                                LogFile.Log(LogFile.LogFileName, "InitializeBL of BLSerialPort Result:" + errormessage);
                                LogFile.Log(LogFile.LogFileName, EOF);
                                return Result;
                            }
                        }
                    }
                }
                #endregion*/
            }
            //依照模式執行事先檢查動作，並執行FPD流程
            if (Mode != (int)ControlMode.DryRun)//in scout view and contineous mode
            {
                DirectoryInfo dir;
                FileInfo[] fileList;

                #region 0.檢查RamDisk是否存在,若否，則跳出;若存在，則刪除舊檔案。
                dir = new DirectoryInfo(Acquisition.RamDisk);
                Directory.CreateDirectory(Acquisition.RamDisk);
                if (!Directory.Exists(Acquisition.RamDisk))
                {
                    return Result = (int)Interface.ReturnCode.DISK_Error;
                }
                else
                {
                    fileList = dir.GetFiles();//get all files on the folder
                    foreach (FileInfo file in fileList)
                    {
                        if (file.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            file.Attributes = FileAttributes.Normal;
                        try
                        {
                            if (file.Extension == ".raw" || file.Extension == ".cal" || file.Extension == ".txt")
                            {
                                file.Delete();
                            }
                        }
                        catch (Exception exc)
                        {
                            //LogFile.Log(LogFileName, "FileInfo.Delete result:" + exc.Message);
                            //return Result = (int)Interface.ReturnCode.DeleteDirectoryError;
                        }
                    }
                }
                #endregion
                #region 1.檢查預設放置RAW檔資料夾是否存在,若否，則建立新目錄;若存在，則刪除舊檔案。
                dir = new DirectoryInfo(Acquisition.RawDiretion);
                if (!Directory.Exists(Acquisition.RawDiretion))//建立新目錄
                {
                    try
                    {
                        Directory.CreateDirectory(Acquisition.RawDiretion);
                        dir.Attributes = FileAttributes.Hidden;
                    }
                    catch (Exception exc)
                    {
                        LogFile.Log(LogFile.LogFileName, "Directory.CreateDirectory result:" + exc.Message);
                        return Result = (int)Interface.ReturnCode.CreateDirectoryError;
                    }
                }
                else
                {
                    //if (frm_Acquisition.isCalibration && frm_Acquisition.calibrationMode != frm_Acquisition.CalibrationMode.ScoutView)// && frm_Acquisition.round != 0)
                    {/*
                        if (frm_Acquisition.round != 0)
                        {
                        }
                        else*/
                        {//dir = new DirectoryInfo(frm_tACQ.RawDiretion);
                            fileList = dir.GetFiles();//get all files on the folder
                            foreach (FileInfo file in fileList)
                            {
                                try//先把檔屬性重置為Normal,然後再刪除
                                {
                                    if (file.Extension == ".raw")
                                    {
                                        if (file.Attributes.ToString().IndexOf("ReadOnly") != -1)
                                            file.Attributes = FileAttributes.Normal;
                                        file.Delete();
                                    }
                                }
                                catch (Exception exc)
                                {
                                    //LogFile.Log(LogFileName, "FileInfo.Delete result:" + exc.Message);
                                    //return Result = (int)Interface.ReturnCode.DeleteDirectoryError;
                                }
                            }
                        }
                    }
                    //else//if (frm_Acquisition.round == 0)
                    {
                        //dir = new DirectoryInfo(frm_tACQ.RawDiretion);
                        fileList = dir.GetFiles();//get all files on the folder
                        foreach (FileInfo file in fileList)
                        {
                            try//先把檔屬性重置為Normal,然後再刪除
                            {
                                if (file.Extension == ".raw")
                                {
                                    if (file.Attributes.ToString().IndexOf("ReadOnly") != -1)
                                        file.Attributes = FileAttributes.Normal;
                                    file.Delete();
                                }
                            }
                            catch (Exception exc)
                            {
                                //LogFile.Log(LogFileName, "FileInfo.Delete result:" + exc.Message);
                                //return Result = (int)Interface.ReturnCode.DeleteDirectoryError;
                            }
                        }
                    }
                }
                #endregion
                #region 2.檢查預設放置Recon檔資料夾是否存在,若否，則建立新目錄;若存在，則刪除舊檔案。
                if (!Directory.Exists(Acquisition.ReconDiretion))
                {
                    try
                    {
                        Directory.CreateDirectory(Acquisition.ReconDiretion);
                    }
                    catch (Exception exc)
                    {
                        LogFile.Log(LogFile.LogFileName, Acquisition.ReconDiretion + " CreateDirectory result:" + exc.Message);
                        return Result = (int)Interface.ReturnCode.CreateDirectoryError;
                    }
                }
                else
                {
                    if (Acquisition.Recon == Acquisition.ReconType.Digisens)//settings.Digisens)                        {
                    {
                        dir = new DirectoryInfo(Acquisition.ReconDiretion);
                        fileList = dir.GetFiles();//get all files on the folder
                        foreach (FileInfo file in fileList)
                        {
                            try//先把檔屬性重置為Normal,然後再刪除
                            {
                                if (file.Attributes.ToString().IndexOf("ReadOnly") != -1)
                                    file.Attributes = FileAttributes.Normal;
                                file.Delete();
                            }
                            catch (Exception exc)
                            {
                                //LogFile.Log(LogFileName, "FileInfo.Delete result:" + exc.Message);
                                //return Result = (int)Interface.ReturnCode.DeleteDirectoryError;
                            }
                        }
                    }
                }
                #endregion
                #region 3.檢查預設放置temp(real time display)檔資料夾是否存在,若否，則建立新目錄;若存在，則刪除舊檔案。
                dir = new DirectoryInfo(@"C:\Temp\");
                if (Directory.Exists(dir.FullName))
                {
                    fileList = dir.GetFiles();//get all files on the folder
                    foreach (FileInfo file in fileList)
                    {
                        try//先把檔屬性重置為Normal,然後再刪除
                        {
                            if (file.Attributes.ToString().IndexOf("ReadOnly") != -1)
                                file.Attributes = FileAttributes.Normal;
                            file.Delete();
                        }
                        catch (Exception exc)
                        {
                            //LogFile.Log(LogFileName, "FileInfo.Delete result:" + exc.Message);
                            //return Result = (int)Interface.ReturnCode.DeleteDirectoryError;
                        }
                    }
                }
                else//建立新目錄
                {
                    try
                    {
                        Directory.CreateDirectory(dir.FullName);
                        dir.Attributes = FileAttributes.Hidden;
                    }
                    catch (Exception exc)
                    {
                        LogFile.Log(LogFile.LogFileName, dir.FullName + " CreateDirectory result:" + exc.Message);
                        return Result = (int)Interface.ReturnCode.CreateDirectoryError;
                    }
                }
                #endregion
                MessageBox.Show("先前檢查通過");
                #region 4.檢查Detector連線_暫不執行
                FPD.orl.StructSize = Marshal.SizeOf(typeof(SOpenReceptorLink));
                FPD.orl.RecDirPath = Acquisition.DetectorPath;
                Result = FPD.vip_open_receptor_link(ref FPD.orl);
                if (Result == FPD.VIP_NO_ERR)
                {
                    // check ok, now close the link
                    FPD.vip_close_link();
                    //LogFile.Log(Interface.LogFileName, "Detector ckeck link OK");
                    MessageBox.Show("Detector ckeck link OK");
                }
                else
                {
                    //LogFile.Log(Interface.LogFileName, "Detector ckeck link NG: " + Result.ToString());
                    errormessage = "Error Initializing Detector board!" +
                        "\nThere were some errors initializing acquisition hardware." +
                        "\nApplication will now run in 'Demo' mode only.\nIf you do not expect such problems please contact Customer Service!";
                    MessageBox.Show(errormessage);
                    Result = (int)Interface.ReturnCode.ErrorInitializingDetectorBoard;
                    return Result;
                }
                #endregion
                #region 5.執行FPD Process
                FPD.FPDPara FPDPara;
                /*if (SEQNo == (int)Interface.MotorSequence.Asymmetric_600 && frm_tACQ.Detector == frm_tACQ.DetectorType.PaxScan1313DX)
                    FPDPara = new Utility.FPD.FPDPara(0, FramNum, 60, true);
                else*/
                    FPDPara = new FPD.FPDPara(0, FramNum, 30, true);
                if (!GetSyncBoardError)
                {
                    //if (settings.UseDetector)
                    {

                        Result = FPD.RunFluoro(FPDPara, UARTPort);
                    }/*
                    else
                    {
                        Result = FPD.RunFluoroWithoutDetector(FPDPara, UARTPort);
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
                    }*/
                    if (!(Result == (int)Interface.ReturnCode.Success))
                    {
                        LogFile.Log(LogFile.LogFileName, "RunFluoro Result:" + Result.ToString());
                        return Result;
                    }
                }
                else
                {
                    LogFile.Log(LogFile.LogFileName, "GetSyncBoardError before RunFluoro");
                    return Result = (int)Interface.ReturnCode.GetSyncBoardError;
                }
                #endregion
            }/*
            else//dry run mode_Mode == 9
            {
                if (!GetSyncBoardError)
                {
                    Result = FPD.DryRun(UARTPort);
                    if (!(Result == (int)Interface.ReturnCode.Success))
                    {
                        LogFile.Log(LogFile.LogFileName, "DryRun Result:" + Result.ToString());
                        return Result;
                    }
                }
                else
                {
                    LogFile.Log(LogFile.LogFileName, "GetSyncBoardError before DryRun");
                    return Result = (int)Interface.ReturnCode.GetSyncBoardError;
                }
            }*/

            /*
            //收尾:斷線或掛載BG whatcher
            if (!settings.FPGA)
            {
                //關閉port
                Result = Interface.ClosePort(UARTPort, out errormessage);
                Result = Interface.ClosePort(BLPort, out errormessage);
                //stop xray
                if (Mode != (int)ControlMode.DryRun)//stop xray while mode!=dryrun
                {
                    SRI.Go(XRAYPort, false, mA, KVp, 1, true);
                    Result = Interface.ClosePort(XRAYPort, out errormessage);
                }
            }*/

            return Result;
        }
        /*
        /// <summary>This is used to get FAULT command from sync board</summary>
        public static void EmergencyWhatcher(object sender, SerialDataReceivedEventArgs e)
        {
            // If the com port has been closed, do nothing
            if (!((SerialPort)sender).IsOpen) return;

            // Read all the data waiting in the buffer
            string data = ((SerialPort)sender).ReadExisting();
            if (data.Length >= 5)//calculate temp
            {
                if (data.Contains("FAULT"))
                {
                    GetSyncBoardError = true;
                }
            }
            // Display the text to the user in the terminal
            Utility.LogFile.Log(frm_tACQ.LogFolder + "mainLog.txt", data);
        }
        #endregion*/
    }
}
