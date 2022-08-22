using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using TCT_OnlyDetector;

namespace TCT_OnlyDetector
{
    class SaveImage
    {
        public static string LogFileName = LogFile.LogFileName;//File to be write log on
        int from;
        int to;
        int gImgSize;
        bool pad;
        int padAmount;
        public SaveImage(int From, int To, int GImgSize, bool Pad, int PadAmount)
        {
            from = From;
            to = To;
            gImgSize = GImgSize;
            pad = Pad;
            padAmount = PadAmount;
        }
        public void WriteByteArrayFile()
        {
            try
            {
                string filename = "";
                byte[] bytearray = new byte[gImgSize];
                for (int i = from; i < to + 1; i++)
                {
                    if (i < FPD.dummyFrame - 1)
                        continue;
                    else
                        filename = Acquisition.RawDiretion + "\\" + (i - (FPD.dummyFrame - 1)).ToString("D3") + ".raw";
                    try
                    {
                        Marshal.Copy(FPD.ImageAddrList[i], bytearray, 0, gImgSize);
                    }
                    catch
                    {
                        return;
                    }
                    //開啟建立檔案
                    FileStream file = File.Open(filename, FileMode.Create, FileAccess.ReadWrite);
                    BinaryWriter write = new BinaryWriter(file);
                    if (pad)
                    {
                        for (int j = 0; j < bytearray.Length; j += 2)
                        {
                            byte[] bytes = new byte[2] { bytearray[j], bytearray[j + 1] };
                            //write.Write(PadBit(padAmount, bytes));
                            write.Write(TransferValue(bytes, 1f, 0f));

                        }
                    }
                    else
                        write.Write(bytearray);
                    write.Close();
                    file.Close();
                    //將文件設定為系統文件，避免被刪除_irene_2012/7/11_以下將導致Digisens無法讀取
                    //File.SetAttributes(filename, FileAttributes.Hidden | FileAttributes.System);
                }
                return;
            }
            catch
            {
                return;
            }
        }

        /// <summary>Right or Left shift PadAmount of byte pair and return</summary>
        /// <param name="LSB">true:LSB(Least significant bit); false:MSB(Most significant bit)</param>
        /// <param name="ShiftDirection">true:Right; false:Left</param>
        /// <returns></returns>
        public static byte[] PadBit(int PadAmount, byte[] bytePair, bool ShiftDirection, bool LSB)
        {
            string s;
            byte[] bytes = new byte[2];
            if (LSB)//將兩筆BYTE倒置轉二進位資料並補0成為8+8=16bit
                s = Convert.ToString(bytePair[1], 2).PadLeft(8, '0') + Convert.ToString(bytePair[0], 2).PadLeft(8, '0');
            else
                s = Convert.ToString(bytePair[0], 2).PadLeft(8, '0') + Convert.ToString(bytePair[1], 2).PadLeft(8, '0') ;
            //將最後兩位刪除，前方補0
            for (int i = 0; i < PadAmount; i++)
            {
                if (ShiftDirection)
                    s = "0" + s;
                else
                    s = s + "0";
            }
            if (ShiftDirection)
                s = s.Substring(0, 16);
            else
                s = s.Substring(PadAmount, 16);
            //將二進位字串轉為兩個BYTE，並將兩個BYTE顛倒(if it is shout)
            if (LSB)
                s = s.Substring(8, 8) + s.Substring(0, 8);//先將字串倒置
            for (int i = 0; i < 2; ++i)
            {
                bytes[i] = Convert.ToByte(s.Substring(8 * i, 8), 2);
            }
            return bytes;
        }
        /// <summary>傳入bytepair以公式轉換，再將其轉為bytepair回傳</summary>
        /// <param name="bytePair"></param>
        /// <returns></returns>
        public static byte[] TransferValue(byte[] bytePair,float FirstValue, float SecondValue)
        {
            ushort value = BitConverter.ToUInt16(bytePair, 0);//先取得原始值
            //將值帶入公式轉換
            value = (ushort)(FirstValue * value - SecondValue);
            byte[] bytes = BitConverter.GetBytes(value);
            return bytes;
        }
        public static int WriteByteArrayFile(string filename, byte[] data, bool Pad, int PadAmount)
        {
            try
            {
                //開啟建立檔案
                FileStream file = File.Open(filename, FileMode.Create, FileAccess.ReadWrite);
                BinaryWriter write = new BinaryWriter(file);
               // LogFile.Log(LogFileName, "data.Length=" + data.Length.ToString());
                if (Pad)
                {
                    for (int i = 0; i < data.Length; i += 2)
                    {
                        byte[] bytes = new byte[2] { data[i], data[i + 1] };
                        write.Write(SaveImage.PadBit(PadAmount, bytes, true, true));
                    }
                }
                else
                    write.Write(data);
                write.Close();
                file.Close();
                return 1;
            }
            catch (Exception exc)
            {
                LogFile.Log(LogFileName, "Error:" + exc.Message.ToString());
                return -1;
            }
        }

    }
}
