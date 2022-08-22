/*
 * Project: DCBCT Software: tACQ
 * Company: TCT
 * Author:  Irene Kuan
 * Created: May. 2012
 * Notes:   This was created base on x64 version.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using TCT_OnlyDetector.Native;

namespace TCT_OnlyDetector
{
    partial class FPD
    {
        #region DllImport section
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_hw_reset();
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_reset_state();//This function allows the user to abortany incomplete acquisition or calibration
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_open_receptor_link(ref SOpenReceptorLink orl);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_check_link(ref SCheckLink linkCheck);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_get_num_cal_frames(int mode_num, ref int num_cal_frames);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_offset_cal(int mode_num);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_query_prog_info(int uType, ref UQueryProgInfo uq);
        //public static extern int vip_query_prog_info(int uType, IntPtr uq);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_gain_cal_prepare(int mode_num, int calType);
      //  public static extern int vip_gain_cal_prepare(int mode_num, [MarshalAsAttribute(UnmanagedType.I1)] bool auto_sense = false);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_get_cal_info(int mode_num, ref SCalInfo calInfo);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_sw_handshaking(int signal_type, [MarshalAsAttribute(UnmanagedType.I1)] bool active);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_get_analog_offset_params(int mode_num, ref SAnalogOffsetParams aop);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_analog_offset_cal(int modeNum);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_set_correction_settings(ref SCorrections corr);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_get_mode_info(int mdNum, ref SModeInfo mdInfo);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_set_frame_rate(int mdNum, double frame_rate);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_set_user_sync(int mdNum, [MarshalAsAttribute(UnmanagedType.I1)] bool user_sync);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_fluoro_set_prms(int structType, ref NativeConstants.SSeqPrms structPtr);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_get_correction_settings(ref SCorrections corr);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_fluoro_grabber_start(ref NativeConstants.SAcqPrms acqPrms);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_fluoro_get_event_name(int eventType, IntPtr eventName);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_fluoro_record_stop();
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_fluoro_record_start(int stopAfterN = 0, int startFromBufIdx = -1);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_fluoro_grabber_stop();
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_fluoro_get_prms(int structType, ref NativeConstants.SSeqStats seqStats);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_fluoro_get_buffer_ptr(ref IntPtr buf, int bufIdx, int bufType = 0);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_close_link(int recNum = 0);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_set_debug(int enable);
        [DllImport("VirtCp64.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int vip_set_mode_acq_type(int mode_num, int mode_acq_type, int num_frames);
        #endregion
    }
}
