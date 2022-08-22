using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCT_OnlyDetector.Native
{
    public partial class NativeConstants
    {
        /// _IOSTATUS_H ->Error generating expression: 值不能為 null。
        ///參數名稱: node
        public const string _IOSTATUS_H = "";

        /// HS_STANDBY -> (0)
        public const int HS_STANDBY = 0;

        /// HS_ACTIVE -> (1)
        public const int HS_ACTIVE = 1;

        /// HS_ACTIVE_NO_EXPOSE_CTRL -> (2)
        public const int HS_ACTIVE_NO_EXPOSE_CTRL = 2;

        /// HS_ACTIVE_SW -> (3)
        public const int HS_ACTIVE_SW = 3;

        /// HS_CANCEL -> (4)
        public const int HS_CANCEL = 4;

        /// HS_OFFSET_CAL -> (5)
        public const int HS_OFFSET_CAL = 5;

        /// IO_STANDBY -> (0)
        public const int IO_STANDBY = 0;

        /// IO_PREP -> (1)
        public const int IO_PREP = 1;

        /// IO_READY -> (2)
        public const int IO_READY = 2;

        /// IO_ACQ -> (3)
        public const int IO_ACQ = 3;

        /// IO_FETCH -> (4)
        public const int IO_FETCH = 4;

        /// IO_DONE -> (5)
        public const int IO_DONE = 5;

        /// IO_ABORT -> (6)
        public const int IO_ABORT = 6;

        /// IO_INIT -> (7)
        public const int IO_INIT = 7;

        /// IO_INIT_ERROR -> (8)
        public const int IO_INIT_ERROR = 8;

        /// EXP_STANDBY -> (0)
        public const int EXP_STANDBY = 0;

        /// EXP_AWAITING_PERMISSION -> (1)
        public const int EXP_AWAITING_PERMISSION = 1;

        /// EXP_PERMITTED -> (2)
        public const int EXP_PERMITTED = 2;

        /// EXP_REQUESTED -> (3)
        public const int EXP_REQUESTED = 3;

        /// EXP_CONFIRMED -> (4)
        public const int EXP_CONFIRMED = 4;

        /// EXP_TIMED_OUT -> (5)
        public const int EXP_TIMED_OUT = 5;

        /// EXP_COMPLETED -> (6)
        public const int EXP_COMPLETED = 6;

        /// NBR_OF_RESERVED -> (6)
        public const int NBR_OF_RESERVED = 6;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct VIP_IO_STATUS_EX
    {

        /// int
        public int structSize;

        /// int
        public int structVersion;

        /// int
        public int ioState;

        /// int
        public int expState;

        /// int
        public int windowIsOpen;

        /// int
        public int windowWaitMsec;

        /// int
        public int windowRemainingMsec;

        /// int
        public int acqRemainingMsec;

        /// int
        public int aCrntLine;

        /// int
        public int bCrntLine;

        /// int[6]
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I4)]
        public int[] reserved;
    }

}
