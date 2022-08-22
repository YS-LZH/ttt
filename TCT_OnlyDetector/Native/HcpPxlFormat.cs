//////////////////////////////////////////////////////////////////////////////
// Filename:	CBPxlFormat.h
// Description:	This file contains #defines and masks for definition of pixel
//				formats used with ConeBeam receptors.	
// Copyright:	Varian Medical Systems
//				All Rights Reserved
//				Varian Proprietary   
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCT_OnlyDetector.Native
{
    public partial class NativeConstants
    {
        /// CBPXLFORMAT_H_INCLUDED -> 
        /// Error generating expression: 值不能為 null。
        ///參數名稱: node
        public const string CBPXLFORMAT_H_INCLUDED = "";

        /// CB_ORIG_FRMT_MASK -> 0x0000FF
        public const int CB_ORIG_FRMT_MASK = 255;

        /// CB_CURR_FRMT_MASK -> 0x00FF00
        public const int CB_CURR_FRMT_MASK = 65280;

        /// CB_CORCTD_FRMT_MASK -> 0x000F00
        public const int CB_CORCTD_FRMT_MASK = 3840;

        /// CB_RAW_FRMT_MASK -> 0x00F000
        public const int CB_RAW_FRMT_MASK = 61440;

        /// CB_HILO_FRMT_MASK -> 0x0F0000
        public const int CB_HILO_FRMT_MASK = 983040;

        /// CB_ORIG_NOT_DEFINED -> 0x000000
        public const int CB_ORIG_NOT_DEFINED = 0;

        /// CB_ORIG_DUAL_READ -> 0x000001
        public const int CB_ORIG_DUAL_READ = 1;

        /// CB_ORIG_DYNAMIC_GAIN -> 0x000002
        public const int CB_ORIG_DYNAMIC_GAIN = 2;

        /// CB_FRMT_NORMAL -> 0x000000
        public const int CB_FRMT_NORMAL = 0;

        /// CB_FRMT_EXP_2 -> 0x000200
        public const int CB_FRMT_EXP_2 = 512;

        /// CB_FRMT_RAW_DUAL_RD -> 0x001000
        public const int CB_FRMT_RAW_DUAL_RD = 4096;

        /// CB_FRMT_RAW_DYN_GN -> 0x002000
        public const int CB_FRMT_RAW_DYN_GN = 8192;

        /// CB_FRMT_HIGN_DUAL_RD -> 0x010000
        public const int CB_FRMT_HIGN_DUAL_RD = 65536;

        /// CB_FRMT_LOGN_DUAL_RD -> 0x020000
        public const int CB_FRMT_LOGN_DUAL_RD = 131072;
    }
}
