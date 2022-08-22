using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCT_OnlyDetector
{
    public partial class Acquisition : Form
    {
        public const string RawDiretion = "C:\\TCTData\\Raw";//Keep Raw Diretion
        public const string ReconDiretion = "C:\\TCTData\\Recon";//Keep Recon Diretion
        public static int nAcquire = 0;//0:idle; 1:ready; 2:start
        static string _messageBoxHeader = "M-CBCT Acquisition Control Message";//define pop up message box header from Patient management page
        public Acquisition()
        {
            InitializeComponent();
        }
        public enum DetectorType
        {
            PaxScan1313DX,
            PaxScan2520DX
        }

        private void btn_Acquisition_Click(object sender, EventArgs e)
        {
            Go();
        }
        void Go()
        {
            string returnMessage = "";
            /*if (!settings.FPGA)
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(StartWhatcher);*/
            nAcquire = 2;//set to start status
            FPD._prograss = 0;
            pgb_ProgressBar.Value = 0;
            FPD.UserCancelACQProcess = false;
            ptb_Preview.Image = null;
            //check memory_irene_2012/7/11
            if (frm_tACQ.getMemory() < (ulong)(2 * acquisition.ImageSize * (acquisition.Views + Utility.FPD.dummyFrame) / 1024))
            {
                returnMessage = "E032 : Insufficient Memory! Available memory: " + frm_tACQ.getMemory().ToString() + " MB" +
                    "\nThere were some errors initializing acquisition process." +
                    "\nIf you do not expect such problems please contact Customer Service!"; ;
                MessageBox.Show(returnMessage, _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //check disk space
            long diskFreeSpace = (new DriveInfo(RawDiretion)).AvailableFreeSpace;//位元組
            long GB = 1024 * 1024 * 1024;
            if (diskFreeSpace < 10 * GB)//小於10GB不允許拍攝
            {
                returnMessage = "E033 : Insufficient Disk Space! Available Disk Space : " + (diskFreeSpace / 1024 / 1024 / 1024).ToString() +
                    " GB is lower then 10 GB. Please clear unused files in case of leaking space during Acquisition.";
                MessageBox.Show(returnMessage, _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //
            if (SelectMotorSequence == Interface.MotorSequence.Asymmetric_600)// (frm_Acquisition.nResolution == 1 || frm_Acquisition.nResolution == 2 || frm_Acquisition.nResolution == 3)
                acquireMode = AcquireMode.Pulse_Full_Asymmetric;
            else if (SelectMotorSequence == Interface.MotorSequence.Symmetric_155)//(frm_Acquisition.nResolution == 4 || frm_Acquisition.nResolution == 5 || frm_Acquisition.nResolution == 6)
                acquireMode = AcquireMode.Pulse_Half_Symmetric;
            else
                acquireMode = AcquireMode.Pulse_Full_Symmetric;

            lbl_Message.Invoke(new EventHandler(delegate
            {
                if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                    lbl_Message.Text = "準備拍攝";
                else
                    lbl_Message.Text = "Acquisition...";
                lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);
            }));
            //
            if (!cbx_Calibration.Checked)
            {
                btn_DryRun.BackgroundImage = image[(int)ImageIcon.Dark_DryRun];
                btn_ScoutView.BackgroundImage = image[(int)ImageIcon.Dark_ScoutView];
                btn_Acquisition.BackgroundImage = image[(int)ImageIcon.Bright_Stop];
            }
            patient.StudyDateTime = DateTime.Now;
            acquisition.StudyPath = getStudyPath();
            acquisition.BodyPartExamined = txt_BodyPartExamined.Text;
            acquisition.LotNumber = txt_LotNumber.Text;
            acquisition.PhysiciansName = txt_PhysiciansName.Text;
            acquisition.StudyDescription = txt_StudyTitle.Text;
            acquisition.SeriesDescription = txt_SeriesTitle.Text;
            if (frm_tACQ.LaunchByHIS)//從PACS取得STUDY UID
            {
                acquisition.StudyUID = frm_tACQ.HISPatient.StudyInstanceUID;
                acquisition.SeriesUID = settings.UIDRoot + "." + DateTime.Today.ToString("yyyyMMdd") + "." + DateTime.Now.ToString("HHmmss.fffff");
            }
            else
            {
                acquisition.StudyUID = GenerateStudyUID(settings.UIDRoot, 46);
                acquisition.SeriesUID = settings.UIDRoot + "." + DateTime.Today.ToString("yyyyMMdd") + "." + DateTime.Now.ToString("HHmmss.fffff");
            }
            if (studyID == -1)
                return;
            //clean folder
            CleanFolder(acquisition.StudyPath);
            //if (!CleanFolder(acquisition.StudyPath))
            //{
            //    return;
            //}
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            LogFile.Log(LogFile.LogFileName1, "================== Acquisition start on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + "===================");
            if (!cbx_Calibration.Checked)
            {
                SwitchResolution();
                SelectCalibrationFiles(false);//start
            }
            bgw_ACQ.RunWorkerAsync();
            bgw_ACQProgress.RunWorkerAsync();
        }
    }
}
