/*
 * Project: DCBCT Software: tACQ
 * Company: TCT
 * Author:  Irene Kuan
 * Created: Feb. 2012
 * Notes:   This was created to write logs to log files
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace TCT_OnlyDetector
{
    class LogFile
    {
        public static string LogFileName = @"c:\TCT\TCTLog\" + "AcqLog_" + DateTime.Today.ToString("yyyyMMdd") + ".log";//File to be write log on
        public static string LogFileName1 = @"c:\TCT\TCTLog\" + "BGWLog_" + DateTime.Today.ToString("yyyyMMdd") + ".log";//Used to be validation
        /// <summary>Write log file with stringlist</summary>
        /// <param name="Path">File to be write on</param>
        /// <param name="StringList">StringList to be write</param>
        public static void Log(string Path, List<string> StringList)
        {
            FileInfo fi = new FileInfo(Path);
            if (fi.Directory.Exists != true)
                Directory.CreateDirectory(fi.Directory.ToString());
            FileStream OpenConfigFile = new FileStream(Path, FileMode.Append, FileAccess.Write);
            StreamWriter WriteFile = new StreamWriter(OpenConfigFile, Encoding.Default);
            try
            {
                foreach (string str in StringList)
                {
                    WriteFile.WriteLine(str);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
            WriteFile.Close();
            OpenConfigFile.Close();
        }
        /// <summary>Write log file wit single string</summary>
        /// <param name="Path">File to be write on</param>
        /// <param name="String">String to be write</param>
        public static void Log(string Path, string String)
        {/*
            lock (FPGA.thisLock)
            {
                FileInfo fi = new FileInfo(Path);
                if (fi.Directory.Exists != true)
                    Directory.CreateDirectory(fi.Directory.ToString());
                FileStream OpenConfigFile = new FileStream(Path, FileMode.Append, FileAccess.Write);
                StreamWriter WriteFile = new StreamWriter(OpenConfigFile, Encoding.Default);
                try
                {
                    WriteFile.WriteLine(String);
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                }
                WriteFile.Close();
                OpenConfigFile.Close();

            }*/
        }
    }
}
