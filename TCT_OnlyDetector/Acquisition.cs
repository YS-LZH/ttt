using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TCT_OnlyDetector.Interface;

namespace TCT_OnlyDetector
{
    public struct AcquisitionInfo
    {
        public int kVp;
        public int mA;
        public float Duration;
        public ushort DataWidth;
        public ushort DataHeight;
        public ushort Views;
        public float AngularStep;
        public ushort ImageSize;
        public ushort Slices;
        public float VoxelSize;
        public String RawImagePath;
        public String StudyPath;
        public string StudyUID;
        public string SeriesUID;
        public string BodyPartExamined;
        public string LotNumber;
        public string PhysiciansName;
        public string StudyDescription;
        public string SeriesDescription;
    }
    public partial class Acquisition : Form
    {
        public const string RawDiretion = "C:\\TCTData\\Raw";//Keep Raw Diretion
        public const string ReconDiretion = "C:\\TCTData\\Recon";//Keep Recon Diretion
        public static int nAcquire = 0;//0:idle; 1:ready; 2:start
        static string _messageBoxHeader = "M-CBCT Acquisition Control Message";//define pop up message box header from Patient management page
        public static AcquisitionInfo acquisition;
        public static AcquireMode acquireMode;
        public static DetectorType Detector = DetectorType.PaxScan1313DX;
        public static Interface.MotorSequence SelectMotorSequence = Interface.MotorSequence.Symmetric_300;
        public static int nResolution = 6;
        public static ReconType Recon = ReconType.Digisens;
        public static string CalibrationFolder = @"c:\TCT\tACQ\CalibrationFile\TD0";
        public static string CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_025.cal";
        public static string MetalCalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_025.cal";
        public static string[] split;
        public static System.ComponentModel.BackgroundWorker bgw_ACQProgress;
        public static System.ComponentModel.BackgroundWorker bgw_ACQ;
        public static string DetectorPath = @"C:\IMAGERs\B44S03-0503";
        public static int PadLength = 44;
        public static string CalibrationTempFile_STD = Path.GetDirectoryName(Application.ExecutablePath) + "\\Calibration\\TCT_cal_STD.cal";
        public static string CalibrationTempFile_EDS = Path.GetDirectoryName(Application.ExecutablePath) + "\\Calibration\\TCT_cal_EDS.cal";
        public static SerialPort MotorSerialPort = new SerialPort("COM11", 9600, Parity.None, 8, StopBits.One);//add for global tShell serialport_irene@2012.01
        public static double RedundantCoolingTime = 0;//unit:ms, this will be used for judge if the acquisition work could be fired
        public static System.ComponentModel.BackgroundWorker bgw_CoolingTime;
        public static string FPGALog = @"c:\TCT\TCTLog\" + "FPGA_" + DateTime.Today.ToString("yyyyMMdd") + ".log";//FPGA通訊紀錄
        public static string RamDisk=@"D:\RAMDISK";
        public Acquisition()
        {
            InitializeComponent();
            bgw_ACQ = new System.ComponentModel.BackgroundWorker();
            bgw_ACQProgress = new System.ComponentModel.BackgroundWorker();
            // 
            // bgw_ACQ
            // 
            bgw_ACQ.WorkerSupportsCancellation = true;
            bgw_ACQ.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_ACQ_DoWork);
            bgw_ACQ.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgw_ACQ_RunWorkerCompleted);
            // 
            // bgw_ACQProgress
            // 
            bgw_ACQProgress.WorkerReportsProgress = true;
            bgw_ACQProgress.WorkerSupportsCancellation = true;
            bgw_ACQProgress.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgw_ACQProgress_DoWork);
            bgw_ACQProgress.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgw_ACQProgress_ProgressChanged);
            //
            /*bgw_CoolingTime = new BackgroundWorker();
            bgw_CoolingTime.WorkerReportsProgress = true;
            bgw_CoolingTime.ProgressChanged += new ProgressChangedEventHandler(bgw_CoolingTime_ProgressChanged);
            bgw_CoolingTime.DoWork += new DoWorkEventHandler(bgw_CoolingTime_DoWork);
            bgw_CoolingTime.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_CoolingTime_RunWorkerCompleted);*/
            Graphics g;
            g = ptb_Preview.CreateGraphics();
            acquisition.Views = 300;
        }
        public enum DetectorType
        {
            PaxScan1313DX,
            PaxScan2520DX
        }
        public enum AcquireMode
        {
            Scout,
            DryRun,
            Pulse_Full_Asymmetric,//EDS
            Pulse_Full_Symmetric,//Standard
            Pulse_Half_Symmetric, //Fast
            None
        };
        public enum ReconType
        {
            BA,
            Digisens,
            Iner,
            TCT
        }
        public enum ScanTime//unit:ms
        {
            Fast_9x9 = 5775,
            STD_9x9 = 10560,
            EDS_15x9 = 10292,
            Fast_13x16 = 5742,
            STD_13x16 = 10593,
            EDS_20x16 = 20592
        }

        /// <summary>取得可用記憶體資訊(MB)</summary>
        public static ulong getMemory()
        {
            Computer myComputer = new Computer();
            //回傳MB
            return myComputer.Info.AvailablePhysicalMemory / 1024 / 1024;
        }
        public static bool CleanFolder(string Folder)
        {
            DirectoryInfo dir = new DirectoryInfo(Folder);

            if (!System.IO.Directory.Exists(Folder))
                return false;
            else
            {
                FileInfo[] fileList = dir.GetFiles();//get all files on the folder
                foreach (FileInfo file in fileList)
                {
                    try
                    {
                        file.Attributes = FileAttributes.Normal;
                        file.Delete();
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("E035 : Fail to delete file." + exc.Message, _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
            }
            return true;
        }

        void SwitchResolution()
        {
            if (Detector == DetectorType.PaxScan1313DX)
            {
                if (SelectMotorSequence == Interface.MotorSequence.Asymmetric_600)//acquireMode == AcquireMode.Pulse_Full_Asymmetric)
                {
                    if (cbx_Resolution.Text == "0.125")//0.125
                        nResolution = 1;
                    else if (cbx_Resolution.Text == "0.2")//0.2
                        nResolution = 2;
                    else//0.25
                        nResolution = 3;
                    return;
                }
                if (SelectMotorSequence == Interface.MotorSequence.Symmetric_300)//acquireMode == AcquireMode.Pulse_Full_Symmetric)
                {
                    if (cbx_Resolution.Text == "0.125")//0.125
                        nResolution = 7;
                    else if (cbx_Resolution.Text == "0.2")//0.2
                        nResolution = 8;
                    else//0.25
                        nResolution = 9;
                    return;

                }
                if (SelectMotorSequence == Interface.MotorSequence.Symmetric_155)//(acquireMode == AcquireMode.Pulse_Half_Symmetric)
                {
                    if (cbx_Resolution.Text == "0.125")//0.125
                        nResolution = 4;
                    else if (cbx_Resolution.Text == "0.2")//0.2
                        nResolution = 5;
                    else//0.25
                        nResolution = 6;
                }
            }
            else//2520
            {
                if (SelectMotorSequence == Interface.MotorSequence.Asymmetric_600)//acquireMode == AcquireMode.Pulse_Full_Asymmetric)
                {
                    if (cbx_Resolution.Text == "0.2")
                        nResolution = 1;
                    else// if (cbx_Resolution.Text == "0.3")
                        nResolution = 2;
                    return;
                }
                if (SelectMotorSequence == Interface.MotorSequence.Symmetric_300)//acquireMode == AcquireMode.Pulse_Full_Symmetric)
                {
                    if (cbx_Resolution.Text == "0.2")
                        nResolution = 3;
                    else// if (cbx_Resolution.Text == "0.3")
                        nResolution = 4;
                    return;

                }
                if (SelectMotorSequence == Interface.MotorSequence.Symmetric_155)//(acquireMode == AcquireMode.Pulse_Half_Symmetric)
                {
                    if (cbx_Resolution.Text == "0.3")
                        nResolution = 5;
                    else// if (cbx_Resolution.Text == "0.4")
                        nResolution = 6;
                }
            }
        }

        public static void SelectCalibrationFiles(bool MetalArtifactor)
        {
            if (Detector == DetectorType.PaxScan1313DX)
            {
                if (nResolution == 1)
                {
                    acquisition.DataWidth = 512;
                    acquisition.DataHeight = 512;
                    acquisition.Views = 600;
                    acquisition.Duration = 0.0166f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 1024;
                    acquisition.Slices = 1024;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_0125_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_0125.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.125f;
                    }
                }
                else if (nResolution == 2)
                {
                    acquisition.DataWidth = 512;
                    acquisition.DataHeight = 512;
                    acquisition.Views = 600;
                    acquisition.Duration = 0.0166f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 640;
                    acquisition.Slices = 640;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_02_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_02.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.2f;
                    }
                }
                else if (nResolution == 3)
                {
                    acquisition.DataWidth = 512;
                    acquisition.DataHeight = 512;
                    acquisition.Views = 600;
                    acquisition.Duration = 0.0166f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 512;
                    acquisition.Slices = 512;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_025_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_025.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.25f;
                    }
                }
                else if (nResolution == 4)
                {
                    acquisition.DataWidth = 512;
                    acquisition.DataHeight = 512;
                    acquisition.Views = 155;
                    acquisition.Duration = 0.013f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 1024;
                    acquisition.Slices = 1024;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Half_Sy_0125_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Half_Sy_0125.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.125f;
                    }
                }
                else if (nResolution == 5)
                {
                    acquisition.DataWidth = 512;
                    acquisition.DataHeight = 512;
                    acquisition.Views = 155;
                    acquisition.Duration = 0.013f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 640;
                    acquisition.Slices = 640;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Half_Sy_02_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Half_Sy_02.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.2f;
                    }
                }
                else if (nResolution == 6)
                {
                    acquisition.DataWidth = 512;
                    acquisition.DataHeight = 512;
                    acquisition.Views = 155;
                    acquisition.Duration = 0.013f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 512;
                    acquisition.Slices = 512;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Half_Sy_025_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Half_Sy_025.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.25f;
                    }
                }
                else if (nResolution == 7)
                {
                    acquisition.DataWidth = 512;
                    acquisition.DataHeight = 512;
                    acquisition.Views = 300;
                    acquisition.Duration = 0.013f;
                    acquisition.AngularStep = 1.2000f;
                    acquisition.ImageSize = 1024;
                    acquisition.Slices = 1024;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Sy_0125_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Sy_0125.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.125f;
                    }
                }
                else if (nResolution == 8)
                {
                    acquisition.DataWidth = 512;
                    acquisition.DataHeight = 512;
                    acquisition.Views = 300;
                    acquisition.Duration = 0.013f;
                    acquisition.AngularStep = 1.2000f;
                    acquisition.ImageSize = 640;
                    acquisition.Slices = 640;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Sy_02_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Sy_02.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.2f;
                    }
                }
                else if (nResolution == 9)
                {
                    acquisition.DataWidth = 512;
                    acquisition.DataHeight = 512;
                    acquisition.Views = 300;
                    acquisition.Duration = 0.013f;
                    acquisition.AngularStep = 1.2000f;
                    acquisition.ImageSize = 512;
                    acquisition.Slices = 512;
                    /*
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Sy_025_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Sy_025.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.25f;
                    }*/
                }
                /*
                if (ckb_Phantom.Checked == true)
                {
                    CalibrationFile = CalibrationFolder + "\\calibration_Full_Phantom.cal";
                    split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                }*/
            }
            else//2520
            {
                if (nResolution == 1)//20*13_0.2
                {
                    acquisition.DataWidth = 768;
                    acquisition.DataHeight = 960;
                    acquisition.Views = 600;
                    acquisition.Duration = 0.0166f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 1024;
                    acquisition.Slices = 1024;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_02_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_02.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.2f;
                    }
                }
                else if (nResolution == 2)//20*13_0.3
                {
                    acquisition.DataWidth = 768;
                    acquisition.DataHeight = 960;
                    acquisition.Views = 600;
                    acquisition.Duration = 0.0166f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 640;
                    acquisition.Slices = 640;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_03_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Asy_03.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.3f;
                    }
                }
                else if (nResolution == 3)//16*13_0.2
                {
                    acquisition.DataWidth = 768;
                    acquisition.DataHeight = 960;
                    acquisition.Views = 300;
                    acquisition.Duration = 0.0166f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 512;
                    acquisition.Slices = 512;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Sy_02_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Sy_02.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.2f;
                    }
                }
                else if (nResolution == 4)//16*13_0.3
                {
                    acquisition.DataWidth = 768;
                    acquisition.DataHeight = 960;
                    acquisition.Views = 300;
                    acquisition.Duration = 0.013f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 1024;
                    acquisition.Slices = 1024;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Sy_03_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Full_Sy_03.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.3f;
                    }
                }
                else if (nResolution == 5)//16*13Fast_0.3
                {
                    acquisition.DataWidth = 768;
                    acquisition.DataHeight = 960;
                    acquisition.Views = 155;
                    acquisition.Duration = 0.013f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 640;
                    acquisition.Slices = 640;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Half_Sy_03_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Half_Sy_03.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.3f;
                    }
                }
                else// if (nResolution == 6)//16*13Fast_0.4
                {
                    acquisition.DataWidth = 768;
                    acquisition.DataHeight = 960;
                    acquisition.Views = 155;
                    acquisition.Duration = 0.013f;
                    acquisition.AngularStep = 0.6000f;
                    acquisition.ImageSize = 512;
                    acquisition.Slices = 512;
                    if (Recon == ReconType.Digisens)//settings.Digisens)                        {
                    {
                        if (MetalArtifactor)
                            CalibrationFile = CalibrationFolder + "\\calibration_Half_Sy_04_Metal.cal";
                        else
                            CalibrationFile = CalibrationFolder + "\\calibration_Half_Sy_04.cal";
                        split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                    }
                    else
                    {
                        acquisition.VoxelSize = 0.4f;
                    }
                }
                /*
                if (ckb_Phantom.Checked == true)
                {
                    CalibrationFile = CalibrationFolder + "\\calibration_Full_Phantom.cal";
                    split = XMLReader.ReadXMLFile("CalibrationGeometry/Grid", "scale", CalibrationFile).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    acquisition.VoxelSize = (float)Convert.ToDouble(split[1]);
                }*/
            }
        }
        private void btn_Acquisition_Click(object sender, EventArgs e)
        {
            ptb_Preview.Image = null;
            Go();
        }
        private string getStudyPath()
        {
            //studyID = frm_PatientManagement.GetStudyID();
            //return String.Format("C:\\TCTData\\{0} {1}\\{2}\\{3}", patient.ID, patient.Name, patient.StudyDateTime.ToString("yyyyMMdd"), studyID);
            /*
            if (settings.User == "NCKUH")
            {
                if (frm_tACQ.LaunchByHIS)
                {
                    //return String.Format("C:\\TCTData\\DICOM\\{0}\\{1}\\{2}", frm_tACQ.HISPatient.OtherPatientIDs, patient.StudyDateTime.ToString("yyyyMMdd"), studyID);
                    return String.Format("C:\\TCTData\\DICOM\\{0}\\{1}\\{2}", frm_tACQ.HISPatient.ID, patient.StudyDateTime.ToString("yyyyMMdd"), studyID);
                }
                else
                    return String.Format("C:\\TCTData\\DICOM\\{0}\\{1}\\{2}", patient.AccessionNumber, patient.StudyDateTime.ToString("yyyyMMdd"), studyID);
            }
            else*/
                return String.Format("C:\\TCTData\\DICOM\\{0}\\{1}\\{2}", 001, DateTime.Now.ToString("yyyyMMdd"), 001);
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
            /*
            if (getMemory() < (ulong)(2 * acquisition.ImageSize * (acquisition.Views + FPD.dummyFrame) / 1024))
            {
                returnMessage = "E032 : Insufficient Memory! Available memory: " + getMemory().ToString() + " MB" +
                    "\nThere were some errors initializing acquisition process." +
                    "\nIf you do not expect such problems please contact Customer Service!"; ;
                MessageBox.Show(returnMessage, _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/
            //check disk space
            long diskFreeSpace = (new DriveInfo(RawDiretion)).AvailableFreeSpace;//位元組
            long GB = 1024 * 1024 * 1024;
            /*
            if (diskFreeSpace < 10 * GB)//小於10GB不允許拍攝
            {
                returnMessage = "E033 : Insufficient Disk Space! Available Disk Space : " + (diskFreeSpace / 1024 / 1024 / 1024).ToString() +
                    " GB is lower then 10 GB. Please clear unused files in case of leaking space during Acquisition.";
                MessageBox.Show(returnMessage, _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/
            //
            /*
            if (SelectMotorSequence == Interface.MotorSequence.Asymmetric_600)// (frm_Acquisition.nResolution == 1 || frm_Acquisition.nResolution == 2 || frm_Acquisition.nResolution == 3)
                acquireMode = AcquireMode.Pulse_Full_Asymmetric;
            else if (SelectMotorSequence == Interface.MotorSequence.Symmetric_155)//(frm_Acquisition.nResolution == 4 || frm_Acquisition.nResolution == 5 || frm_Acquisition.nResolution == 6)
                acquireMode = AcquireMode.Pulse_Half_Symmetric;
            else*/
                acquireMode = AcquireMode.Pulse_Full_Symmetric;

            lbl_Message.Invoke(new EventHandler(delegate
            {
                //if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                    lbl_Message.Text = "準備拍攝";
                /*else
                    lbl_Message.Text = "Acquisition...";*/
                lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);
            }));
            //
            /*
            if (!cbx_Calibration.Checked)
            {
                btn_DryRun.BackgroundImage = image[(int)ImageIcon.Dark_DryRun];
                btn_ScoutView.BackgroundImage = image[(int)ImageIcon.Dark_ScoutView];
                btn_Acquisition.BackgroundImage = image[(int)ImageIcon.Bright_Stop];
            }*/
            /*
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
            */
            //clean folder
            CleanFolder(getStudyPath());
            //if (!CleanFolder(acquisition.StudyPath))
            //{
            //    return;
            //}
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            LogFile.Log(LogFile.LogFileName1, "================== Acquisition start on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt") + "===================");
            //if (!cbx_Calibration.Checked)
            {
                SwitchResolution();
                SelectCalibrationFiles(false);//start
            }
            bgw_ACQ.RunWorkerAsync();
            bgw_ACQProgress.RunWorkerAsync();
        }
        bool CheckCalFiles()
        {
            bool result = true;
            if (!CheckFileExistence(CalibrationTempFile_STD))
            {
                result = false;
            }
            if (!CheckFileExistence(CalibrationTempFile_EDS))
            {
                result = false;
            }
            //if (!CheckCalFile(Path.GetDirectoryName(Application.ExecutablePath)))
            //{
            //    result = false;
            //}
            //if (!CheckCalFileTypeExistence(Path.GetDirectoryName(Application.ExecutablePath)))
            //{
            //    result = false;
            //}
            return result;
        }
        bool CheckFileExistence(string FileName)
        {
            bool FileExistence = false;
            FileInfo file = new FileInfo(FileName);
            if (file.Exists)
                FileExistence = true;
            return FileExistence;
        }
        private void bgw_ACQ_DoWork(object sender, DoWorkEventArgs e)
        {
            //  System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.AboveNormal;
            System.Threading.Thread.CurrentThread.Name = "bgw_ACQ";
            int frames = acquisition.Views;
            int kVp = acquisition.kVp;
            int mA = acquisition.mA;
            long diskFreeSpace = (new DriveInfo(Acquisition.RawDiretion)).AvailableFreeSpace;//位元組
            int result = (int)Interface.ReturnCode.Success;
            int repeat = 1;

            EnableComponent(false);
            LogFile.Log(LogFile.LogFileName1, "Start bgw_ACQ_DoWork on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            /*
            #region copy detector calibration files
            if (acquireMode != AcquireMode.DryRun)
            {
                string dirTo = DetectorPath + "\\00_2x21pFVG1\\";
                if (Detector == DetectorType.PaxScan2520DX)
                    dirTo = DetectorPath + "\\00_2x21pFVG0.5\\";
                string dirFrom;
                DirectoryInfo dir;
                if (Detector == DetectorType.PaxScan2520DX)
                {
                    if (nResolution == 1 || nResolution == 2)//asymmetric
                        dirFrom = CalibrationFolder + "\\Asymmetric";
                    else
                        dirFrom = CalibrationFolder + "\\Symmetric";
                }
                else
                {
                    if (nResolution == 1 || nResolution == 2 || nResolution == 3)//asymmetric
                        dirFrom = CalibrationFolder + "\\Asymmetric";
                    else
                        dirFrom = CalibrationFolder + "\\Symmetric";
                }
                if (!System.IO.Directory.Exists(dirFrom))
                {
                    try
                    {
                        Directory.CreateDirectory(dirFrom);
                    }
                    catch// (Exception exc)
                    {
                        // MessageBox.Show("Can not find the folder: "+dirFrom, _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Result = Interface.ReturnCode.CreateDirectoryError;
                        return;
                    }
                }
                else
                {
                    dir = new DirectoryInfo(dirFrom);
                    FileInfo[] fileList = dir.GetFiles();
                    foreach (FileInfo file in fileList)
                    {
                        try
                        {
                            file.CopyTo(dirTo + file.Name, true);
                        }
                        catch //(Exception exc)
                        {
                            //MessageBox.Show(exc.Message, _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Result = ReturnCode.CopyDetectorCalibrationFileError;
                            return;
                        }
                    }
                }
            }
            #endregion
            */
            if (bgw_ACQ.CancellationPending == true)
            {
                e.Cancel = true;
                return;
            }
            /*
            if (acquireMode == AcquireMode.Scout)
            {
                result = Interface.DoACQ(serialPort, BLSerialPort, xRAYserialPort, (int)Interface.ControlMode.ScoutView, 0, 1, kVp, mA);
            }
            else if (acquireMode == AcquireMode.DryRun)
            {
                if (doRaliabilityTest)
                {
                    LogFile.Log(frm_tACQ.LogFolder + "Raliability.log", "Start Raliability test on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                    #region used for repeat test
                    for (int i = 0; i < RaliabilityTimes; i++)
                    {
                        DateTime StartTime = DateTime.Now;
                        DateTime EndTime;
                        if ((bgw_ACQ.CancellationPending == true))
                        {
                            result = (int)tACQ.Utility.Interface.ReturnCode.User_Cancel_Acquisition_Process;
                            e.Cancel = true;
                            LogFile.Log(frm_tACQ.LogFolder + "Raliability.log", "User Cancel Raliability test on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                            return;
                        }
                        else
                        {
                            //if (i % 3 == 0)
                            //{
                            //    result = tACQ.Utility.Interface.DoACQ(serialPort, BLSerialPort, xRAYserialPort, (int)tACQ.Utility.Interface.ControlMode.DryRun,
                            //            (int)tACQ.Utility.Interface.MotorSequence.Symmetric_155, 0, kVp, mA);
                            //}

                            frm_Acquisition.PauseSendCommand = true;
                            if (i % 2 == 0)
                            {
                                result = tACQ.Utility.Interface.DoACQ(serialPort, BLSerialPort, xRAYserialPort, (int)tACQ.Utility.Interface.ControlMode.DryRun,
                                        (int)tACQ.Utility.Interface.MotorSequence.Symmetric_300, 0, kVp, mA);
                            }
                            else
                            {
                                result = tACQ.Utility.Interface.DoACQ(serialPort, BLSerialPort, xRAYserialPort, (int)tACQ.Utility.Interface.ControlMode.DryRun,
                                        (int)tACQ.Utility.Interface.MotorSequence.Asymmetric_600, 0, kVp, mA);
                            }
                            if (result != (int)Interface.ReturnCode.Success)
                            {
                                e.Result = result;
                                return;
                            }
                            else
                            {
                                nud_Reliability.Invoke(new EventHandler(delegate
                                {
                                    nud_Reliability.Value = nud_Reliability.Value - 1;
                                }));
                            }
                        }
                        EndTime = DateTime.Now;
                        LogFile.Log(frm_tACQ.LogFolder + "Raliability.log", "The " + (i + 1).ToString() + " / " + RaliabilityTimes.ToString() + " round. Time interval = " + (EndTime - StartTime).TotalSeconds.ToString() + " sec on " + DateTime.Now.ToString());
                    }
                    #endregion
                    LogFile.Log(frm_tACQ.LogFolder + "Raliability.log", "Complete Raliability test on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                }
                else
                {
                    result = tACQ.Utility.Interface.DoACQ(serialPort, BLSerialPort, xRAYserialPort, (int)Utility.Interface.ControlMode.DryRun, (int)SelectMotorSequence, 0, kVp, mA);
                }
            }
            */
            //else
            {
                /*if (doRaliabilityTest)
                {
                    LogFile.Log(frm_tACQ.LogFolder + "Raliability.log", "Start Raliability test on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                    #region used for repeat test
                    for (int i = 0; i < RaliabilityTimes; i++)
                    {
                        DateTime StartTime = DateTime.Now;
                        DateTime EndTime;
                        if ((bgw_ACQ.CancellationPending == true))
                        {
                            result = (int)tACQ.Utility.Interface.ReturnCode.User_Cancel_Acquisition_Process;
                            e.Cancel = true;
                            LogFile.Log(frm_tACQ.LogFolder + "Raliability.log", "User Cancel Raliability test on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                            return;
                        }
                        else
                        {
                            //if (i % 3 == 0)
                            //{
                            //    result = tACQ.Utility.Interface.DoACQ(serialPort, BLSerialPort, xRAYserialPort, (int)tACQ.Utility.Interface.ControlMode.DryRun,
                            //            (int)tACQ.Utility.Interface.MotorSequence.Symmetric_155, 0, kVp, mA);
                            //}

                            frm_Acquisition.PauseSendCommand = true;
                            if (i % 2 == 0)
                            {
                                result = tACQ.Utility.Interface.DoACQ(serialPort, BLSerialPort, xRAYserialPort, (int)tACQ.Utility.Interface.ControlMode.Pulse,
                                        (int)tACQ.Utility.Interface.MotorSequence.Symmetric_300, 0, kVp, mA);
                            }
                            else
                            {
                                result = tACQ.Utility.Interface.DoACQ(serialPort, BLSerialPort, xRAYserialPort, (int)tACQ.Utility.Interface.ControlMode.Pulse,
                                        (int)tACQ.Utility.Interface.MotorSequence.Asymmetric_600, 0, kVp, mA);
                            }
                            if (result != (int)Interface.ReturnCode.Success)
                            {
                                e.Result = result;
                                return;
                            }
                            else
                            {
                                nud_Reliability.Invoke(new EventHandler(delegate
                                {
                                    nud_Reliability.Value = nud_Reliability.Value - 1;
                                }));
                            }
                        }
                        EndTime = DateTime.Now;
                        LogFile.Log(frm_tACQ.LogFolder + "Raliability.log", "The " + (i + 1).ToString() + " / " + RaliabilityTimes.ToString() + " round. Time interval = " + (EndTime - StartTime).TotalSeconds.ToString() + " sec on " + DateTime.Now.ToString());
                    }
                    #endregion
                    LogFile.Log(frm_tACQ.LogFolder + "Raliability.log", "Complete Raliability test on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                }
                else if (cbx_Calibration.Checked)
                {
                    if (!CheckCalFiles())
                    {
                        result = (int)tACQ.Utility.Interface.ReturnCode.LACK_CALIBRATION_FILE;
                        e.Cancel = true;
                        return;
                    }
                    
                    if (calibrationMode == CalibrationMode.Geometry)
                    {
                        repeat = (int)settings.Cycle;
                    }
                    for (int i = 0; i < repeat; i++)
                    {
                        if ((bgw_ACQ.CancellationPending == true))
                        {
                            result = (int)tACQ.Utility.Interface.ReturnCode.User_Cancel_Acquisition_Process;
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            FinishACQ = false;
                            result = tACQ.Utility.Interface.DoACQ(serialPort, BLSerialPort, xRAYserialPort, (int)tACQ.Utility.Interface.ControlMode.Pulse, (int)SelectMotorSequence, frames, kVp, mA);
                            if (result != (int)tACQ.Utility.Interface.ReturnCode.Success)
                            {
                                break;
                            }
                            else
                            {
                                tACQ.Utility.FPD._prograss = 100;
                                while (!FinishACQ)
                                {
                                    Thread.Sleep(1);
                                }
                                FinishACQ = false;
                            }
                            if (result != 0)
                            {
                                e.Result = result;
                                return;
                            }
                            round++;
                        }
                    }
                }
                else
                {*/
                //result = 0;//for test
                //result = Interface.DoACQ(serialPort, BLSerialPort, xRAYserialPort, (int)Utility.Interface.ControlMode.Pulse, (int)SelectMotorSequence, frames, kVp, mA);
                result = Interface.DoACQ(MotorSerialPort, MotorSerialPort, MotorSerialPort, (int)Interface.ControlMode.Pulse, (int)SelectMotorSequence, frames, kVp, mA);
                result = (int)Interface.ReturnCode.Success;
                    if (result != (int)Interface.ReturnCode.Success)
                    {/*
                        if (FPD.XRAYEmitOn.kVp != 0)//This means the xray has already on
                            FinishXrayTime = DateTime.Now;*/
                    }
                //}
            }
            //用來記錄拍攝次數
            if (result == (int)Interface.ReturnCode.Success)
                //LastScanMode = acquireMode;

            e.Result = result;
        }
        private double CalculateCoolingTime()
        {
            double CollingRate = 165;//unit:(HU/sec)
            double xrayOnPeriod = 16.6;//unit:ms
            if (acquireMode != AcquireMode.Pulse_Full_Asymmetric)
                xrayOnPeriod = 13;
            // for SRI
            //double desiredcoolingTime = (acquisition.kVp * acquisition.mA * (acquisition.Views + 20) * xrayOnPeriod * 0.001) / CollingRate;//unit:sec

            // for 2520DX
            double scanTime = 0.0;
            /*if (SelectMotorSequence == Interface.MotorSequence.Asymmetric_600)
            {
                scanTime = (double)ScanTime.EDS_20x16;
            }
            else if (SelectMotorSequence == Interface.MotorSequence.Symmetric_300)*/
            {
                scanTime = (double)ScanTime.STD_13x16;
            }/*
            else//fast
            {
                scanTime = (double)ScanTime.Fast_13x16;
            }*/
            double desiredcoolingTime = (acquisition.kVp * acquisition.mA * scanTime * 0.001) / CollingRate;//unit:sec
            //double totalElaspedTime = (DateTime.Now - FinishXrayTime).TotalMilliseconds;//unit:ms
            double totalElaspedTime = (DateTime.Now - DateTime.Now).TotalMilliseconds;//unit:ms
            double redundantCoolingTime = desiredcoolingTime * 1000 - totalElaspedTime;//unit:ms
            return redundantCoolingTime;
        }
        private void bgw_ACQ_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //PauseSendCommand = false;
            bgw_ACQProgress.CancelAsync();
            pgb_ProgressBar.Value = 100;

            #region cooling Time_calculate desired cooling Time and redundant cooling Time, display and count down cooling time
            if (acquireMode != AcquireMode.DryRun && acquireMode != AcquireMode.Scout && acquireMode != AcquireMode.None)
            {/*
                RedundantCoolingTime = CalculateCoolingTime();
                if ((int)(RedundantCoolingTime / 1000) > 0)//剩餘時間大於0秒才需顯示並倒數
                {
                    bgw_CoolingTime.RunWorkerAsync();
                    lbl_CoolingTime.Visible = true;
                }*/
            }
            #endregion

            if (e.Cancelled == true)
            {
                LogFile.Log(LogFile.LogFileName1, "bgw_ACQ_RunWorkerCompleted cancel by user on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                //if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                    lbl_Message.Text = "拍攝流程被中斷";
                /*else
                    lbl_Message.Text = "Acquisition process is terminated!";*/
                lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);
                //add lock component feature_irene_2012/7/11
                EnableComponent(true);
                MessageBox.Show(Interface.ErrStrList[(int)e.Result], _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);//should check error on bgw_ACQ_DoWork
                FPD.UserCancelACQProcess = false;//reset
                //frm_tACQ.StopRealTimeDisplay = true;
            }
            else
            {
#if debug   //jack back door
                if (true)    //jack back door;
#else
                //if ((int)e.Result == (int)Interface.ReturnCode.Success)   // Success
#endif

                {
                    LogFile.Log(LogFile.LogFileName1, "bgw_ACQ_RunWorkerCompleted Success on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                    /*if (acquireMode == AcquireMode.Scout)
                    {
                        float mAs = acquisition.mA * acquisition.Duration;
                        // Show scout view
                        frm_ScoutView scoutView = new frm_ScoutView(patient);
                        scoutView.MdiParent = this.ParentForm;
                        scoutView.kVp = acquisition.kVp.ToString() + " kVp";
                        scoutView.mAs = mAs.ToString("F2") + " mAs";
                        if (cbx_Calibration.Checked)
                        {
                            scoutView.FilePath = acquisition.RawImagePath + "\\000.raw";//Irene@20160503
                            ptb_Preview.Image = scoutView.bmp;
                            //畫十字 by han 2021/10/12
                            ptb_Preview.Refresh();
                            g.DrawLine(new Pen(Color.Black, 1), scoutView.bmp.Size.Width / 2, 0, scoutView.bmp.Size.Width / 2, scoutView.bmp.Size.Height);
                            g.DrawLine(new Pen(Color.Black, 1), 0, scoutView.bmp.Size.Height / 2, scoutView.bmp.Size.Width, scoutView.bmp.Size.Height / 2);
                        }
                        else
                        {
                            // Copy raw image from cource to destination
                            string source = acquisition.RawImagePath + "\\000.raw";//Irene@20160503
                            string destination = acquisition.StudyPath + "\\000.raw";
                            if (!System.IO.Directory.Exists(acquisition.StudyPath))
                                System.IO.Directory.CreateDirectory(acquisition.StudyPath);
                            System.IO.File.Copy(source, destination, true);

                            // Save study information to Database
                            saveStudy();

                            // Show scout view
                            scoutView.FilePath = acquisition.StudyPath;
                            ptb_Preview.Image = scoutView.bmp;

                            //畫十字 by han 2021/10/12
                            ptb_Preview.Refresh();
                            g.DrawLine(new Pen(Color.Black, 1), scoutView.bmp.Size.Width / 2, 0, scoutView.bmp.Size.Width / 2, scoutView.bmp.Size.Height);
                            g.DrawLine(new Pen(Color.Black, 1), 0, scoutView.bmp.Size.Height / 2, scoutView.bmp.Size.Width, scoutView.bmp.Size.Height / 2);
                        }
                        if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                            lbl_Message.Text = "完成預覽掃描流程";
                        else
                            lbl_Message.Text = "Scout View Complete.";
                        lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);
                        //add lock component feature_irene_2012/7/11
                        EnableComponent(true);
                        //plus the counter
                        settings.ScoutViewCount++;
                        settings.Save();
                    }
                    else if (acquireMode == AcquireMode.DryRun)
                    {
                        if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                            lbl_Message.Text = "完成試運轉流程";
                        else
                            lbl_Message.Text = "Complete Dry Run.";
                        lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);
                        //add lock component feature_irene_2012/7/11
                        EnableComponent(true);
                        //plus the counter
                        settings.DryRunCount++;
                        settings.Save();
                        if (!settings.ActiveXRAY && ShowDemo)
                        {
                            #region open tWIN
                            DirectoryInfo tWINdir = new DirectoryInfo(Path.GetDirectoryName(Application.ExecutablePath) + @"\tWIN\");
                            if (!tWINdir.Exists)
                            {
                                MessageBox.Show("E026 : Cannot find tWIN.", _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                bool exist = false;
                                FileInfo[] fileList = tWINdir.GetFiles();//get all files on the folder
                                foreach (FileInfo files in fileList)
                                {
                                    if (files.Name == "tWIN.exe")
                                    {
                                        exist = true;
                                        break;
                                    }
                                }
                                if (!exist)
                                {
                                    MessageBox.Show("E026 : Cannot find tWIN.", _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            string tWINpath = tWINdir.FullName + "tWIN.exe";
                            //string arg1 = acquisition.StudyPath + @"\";
                            string arg1 = acquisition.StudyPath + @"\";//@"C:\Users\user\Desktop\temp\Demo data\little blue_EDS\";
                            //#region for demo
                            //if (SelectMotorSequence == Interface.MotorSequence.Asymmetric_600)
                            arg1 = @"C:\TCT\DemoData\EDS\DICOM\";// acquisition.StudyPath + @"\";
                            //else
                            //    arg1 = @"C:\Users\user\Desktop\temp\Demo data\little blue_standard\";// acquisition.StudyPath + @"\";
                            //#endregion
                            try
                            {
                                System.Diagnostics.Process.Start(tWINpath, arg1);
                            }
                            catch
                            {
                            }
                            #endregion
                        }
                    }
                    else*/// if (scanmode == ScanMode.Pulse_Full_Asymmetric || scanmode == ScanMode.Pulse_Half_Symmetric)
                    {
                        //if (cbx_Calibration.Checked)
                        {/*
                            lbl_Message.Text = "Acquisition Complete";
                            lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);
                            if (calibrationMode == CalibrationMode.Geometry)
                            {
                                //open the raw file
                                view.FilePath = acquisition.RawImagePath + "\\0000.raw";//while we use Geometry mode, we might have more than hundred files
                                ptb_Preview.Image = view.bmp;

                                //使用指定點數量
                                //MessageBox.Show("Please select " + settings.BallNumber.ToString() + " balls on the screen.");
                                if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")//使用任意點數量_數量介於MaxPointCount與MinPointCount之間_以滑鼠左鍵點選，右鍵結束
                                {
                                    MessageBox.Show("請按滑鼠左鍵點選4~7個點，選點後按滑鼠右鍵結束。");
                                    lbl_Message.Text = "請按滑鼠左鍵點選4~7個點，選點後按滑鼠右鍵結束。";
                                }
                                else
                                {
                                    MessageBox.Show("Please left-click mouse for 4~7 points in the screen and finish by right-clicking mouse.");
                                    lbl_Message.Text = "Please left-click mouse for 4~7 points in the screen and finish by right-clicking mouse.";
                                }
                                lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);

                                SelectBall();
                            }
                            if (calibrationMode == CalibrationMode.HU)
                            {
                                reconstruction.Patient = patient;
                                reconstruction.Acquisition = acquisition;
                                reconstruction.Start();
                            }*/
                        }
                        //else
                        {/*
                            if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                                lbl_Message.Text = "開始重建";
                            else
                                lbl_Message.Text = "Start Reconstruction...";
                            lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);

                            //PrepareDicomHeader
                            if (frm_tACQ.Recon == frm_tACQ.ReconType.Digisens)//settings.Digisens)                        {
                            {
                                PrepareDicomHeaderXML();
                                Utility.XMLWriter.WriteXMLFile(DicomHeaderXML, frm_tACQ.LogFolder + "DicomHeader.xml");
                            }
                            //SwitchResolution();
                            //SelectCalibrationFiles(false);
                            //before do this, check for raw file counts
                            int checkRaw = (int)Interface.ReturnCode.Success;//CheckRawFileCount(acquisition.Views);
                            if (checkRaw != (int)Interface.ReturnCode.Success)
                            {
                                if (frm_tACQ.UserUICulture.ToLower() == "zh-tw")
                                    lbl_Message.Text = "重建失敗。原因 : " + Interface.ErrStrList[checkRaw];
                                else
                                    lbl_Message.Text = "Reconstruction stop due to " + Interface.ErrStrList[checkRaw];
                                lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);
                            }
                            else
                            {
                                if (settings.UseDetector)
                                {
                                    reconstruction.Patient = patient;
                                    reconstruction.Acquisition = acquisition;
                                    reconstruction.Start();
                                    //EnableComponent(true);//for test
                                }
                                else
                                {
                                    lbl_Message.Text = "完成";
                                    EnableComponent(true);//for test
                                }
                            }
                            */
                        }
                    }
                }
                //else
                {
                    LogFile.Log(LogFile.LogFileName1, "bgw_ACQ_RunWorkerCompleted finish error on " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                    lbl_Message.Text = "拍攝流程被中斷";
                    lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);
                    //add lock component feature_irene_2012/7/11
                    EnableComponent(true);
                    //frm_tACQ.StopRealTimeDisplay = true;

                    MessageBox.Show(Interface.ErrStrList[(int)e.Result], _messageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);//should check error on bgw_ACQ_DoWork
                    //Utility.FPD.UserCancelACQProcess = false;//reset
                }
            }
            /*
            #region Total Emit Time
            if (acquireMode != AcquireMode.DryRun && acquireMode != AcquireMode.None)
            {
                if (FPD.XRAYEmitOn.kVp != 0)
                {
                    EmitTime = CalculateEmitTime();
                    //寫入資料庫
                    UpdateEmitTime(EmitTime);
                }
                else
                    EmitTime = 0;
            }
            #endregion*/
        }
        private void bgw_ACQProgress_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.CurrentThread.Name = "bgw_ACQProgress";
            string str;
            int percentage;
            //percentage = 0;
            //bgw_ACQProgress.ReportProgress(percentage);
            while ((percentage = FPD.ProgressBar(out str)) <= 100)
            {
                if (bgw_ACQProgress.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                // Perform a time consuming operation and reprot progress.
                if (percentage >= 100) percentage = 100;
                /*if (isCalibration && calibrationMode == CalibrationMode.Geometry)
                    bgw_ACQProgress.ReportProgress(percentage / (int)settings.Cycle + round * (100 / (int)settings.Cycle));
                else*/
                    bgw_ACQProgress.ReportProgress(percentage);
                System.Threading.Thread.Sleep(100);
                //if (percentage == 100) break;
            }
        }
        private void bgw_ACQProgress_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgb_ProgressBar.Value = e.ProgressPercentage;
            string str;
            FPD.ProgressBar(out str);
            lbl_Message.Invoke(new EventHandler(delegate
            {
                        lbl_Message.Text = "準備拍攝";
                
                {
                    /*if (isCalibration && calibrationMode == CalibrationMode.Geometry)
                    {
                        lbl_Message.Text = str + " - Round ( " + (round + 1).ToString() + " / " + settings.Cycle.ToString() + " )";

                    }
                    else*/
                    {
                        lbl_Message.Text = str;
                    }
                }
                lbl_Message.Location = new Point((int)(pgb_ProgressBar.Width / 2.0f - (lbl_Message.Width / 2.0F)), lbl_Message.Location.Y);
            }));

        }
        /// <summary>Switch Acquisition interface component enability</summary>
        /// <param name="Enable">false means start acquisition</param>
        public void EnableComponent(bool Enable)
        {/*
            cbx_Calibration.Invoke(new EventHandler(delegate { cbx_Calibration.Enabled = Enable; }));
            if (acquireMode == AcquireMode.Scout || !cbx_Calibration.Checked)
            {
                btn_DryRun.Invoke(new EventHandler(delegate { btn_DryRun.Enabled = Enable; }));
                btn_ScoutView.Invoke(new EventHandler(delegate { btn_ScoutView.Enabled = Enable; }));
                btn_Acquisition.Invoke(new EventHandler(delegate { btn_Acquisition.Enabled = Enable; }));

                btn_LaserOn.Invoke(new EventHandler(delegate { btn_LaserOn.Enabled = Enable; }));
                btn_LaserOff.Invoke(new EventHandler(delegate { btn_LaserOff.Enabled = Enable; }));

                btn_Half_Symmetric_ACQ.Invoke(new EventHandler(delegate { btn_Half_Symmetric_ACQ.Enabled = Enable; }));
                btn_Full_Symmetric_ACQ.Invoke(new EventHandler(delegate { btn_Full_Symmetric_ACQ.Enabled = Enable; }));
                btn_Full_Asymmetric_ACQ.Invoke(new EventHandler(delegate { btn_Full_Asymmetric_ACQ.Enabled = Enable; }));
                btn_Adult.Invoke(new EventHandler(delegate { btn_Adult.Enabled = Enable; }));
                btn_Child.Invoke(new EventHandler(delegate { btn_Child.Enabled = Enable; }));
                cbx_Resolution.Invoke(new EventHandler(delegate { cbx_Resolution.Enabled = Enable; }));
                txt_StudyTitle.Invoke(new EventHandler(delegate { txt_StudyTitle.Enabled = Enable; }));
                txt_SeriesTitle.Invoke(new EventHandler(delegate { txt_SeriesTitle.Enabled = Enable; }));
                txt_BodyPartExamined.Invoke(new EventHandler(delegate { txt_BodyPartExamined.Enabled = Enable; }));
                txt_PhysiciansName.Invoke(new EventHandler(delegate { txt_PhysiciansName.Enabled = Enable; }));
                txt_LotNumber.Invoke(new EventHandler(delegate { txt_LotNumber.Enabled = Enable; }));
                #region reliability controls
                ckb_Raliability.Invoke(new EventHandler(delegate { ckb_Raliability.Enabled = Enable; }));
                nud_Reliability.Invoke(new EventHandler(delegate { nud_Reliability.Enabled = Enable; }));
                #endregion
                if (Enable)
                {
                    nAcquire = 0;//reset to idle status
                    //btn_DryRun.BackgroundImage = image[(int)ImageIcon.Dark_DryRun];
                    //btn_ScoutView.BackgroundImage = image[(int)ImageIcon.Dark_ScoutView];
                    btn_Acquisition.BackgroundImage = image[(int)ImageIcon.Bright_Start];
                }
                else
                {
                    if (settings.FPGA)
                    {
                        if (isMAR == false)
                        {
                            if (acquireMode == AcquireMode.DryRun)
                            {
                                btn_DryRun.Invoke(new EventHandler(delegate { btn_DryRun.Enabled = true; }));
                                btn_DryRun.BackgroundImage = image[(int)ImageIcon.Bright_Stop];
                                btn_ScoutView.BackgroundImage = image[(int)ImageIcon.Dark_ScoutView];
                                btn_Acquisition.BackgroundImage = image[(int)ImageIcon.Dark_Start];
                            }
                            else if (acquireMode == AcquireMode.Scout)
                            {
                                btn_ScoutView.Invoke(new EventHandler(delegate { btn_ScoutView.Enabled = true; }));
                                btn_DryRun.BackgroundImage = image[(int)ImageIcon.Dark_DryRun];
                                btn_ScoutView.BackgroundImage = image[(int)ImageIcon.Bright_Stop];
                                btn_Acquisition.BackgroundImage = image[(int)ImageIcon.Dark_Start];
                            }
                            else
                            {
                                btn_Acquisition.Invoke(new EventHandler(delegate { btn_Acquisition.Enabled = true; }));
                                btn_DryRun.BackgroundImage = image[(int)ImageIcon.Dark_DryRun];
                                btn_ScoutView.BackgroundImage = image[(int)ImageIcon.Dark_ScoutView];
                                btn_Acquisition.BackgroundImage = image[(int)ImageIcon.Bright_Stop];
                            }
                            btn_MAR.Invoke(new EventHandler(delegate { btn_MAR.Enabled = Enable; }));
                            btn_MAR.BackgroundImage = image[(int)ImageIcon.Dark_MAR];
                        }
                    }
                    else
                    {
                        btn_ScoutView.Invoke(new EventHandler(delegate { btn_ScoutView.Enabled = Enable; }));
                        btn_DryRun.Invoke(new EventHandler(delegate { btn_DryRun.Enabled = Enable; }));
                        btn_LaserOn.Invoke(new EventHandler(delegate { btn_LaserOn.Enabled = Enable; }));
                        btn_LaserOff.Invoke(new EventHandler(delegate { btn_LaserOff.Enabled = Enable; }));
                    }
                }
                if (!settings.ActiveXRAY)
                {
                    btn_ScoutView.Enabled = false;
                    //btn_Acquisition.Enabled = false;
                    btn_MAR.Enabled = false;
                }

                if (cbx_Calibration.Checked)
                {
                    btn_Acquisition.Invoke(new EventHandler(delegate { btn_Acquisition.Enabled = false; }));
                    btn_DryRun.Invoke(new EventHandler(delegate { btn_DryRun.Enabled = false; }));
                    btn_MAR.Invoke(new EventHandler(delegate { btn_MAR.Enabled = false; }));
                }
            }
            else//calibration mode
            {
                btn_ScoutView.Invoke(new EventHandler(delegate { btn_ScoutView.Enabled = Enable; }));
                btn_GainCalibration.Invoke(new EventHandler(delegate { btn_GainCalibration.Enabled = Enable; }));
                btn_GeometryCalibration.Invoke(new EventHandler(delegate { btn_GeometryCalibration.Enabled = Enable; }));
                btn_HUCalibration.Invoke(new EventHandler(delegate { btn_HUCalibration.Enabled = Enable; }));

                if (Enable)//reset
                {
                    nAcquire = 0;//reset to idle status
                    btn_ScoutView.Invoke(new EventHandler(delegate { btn_ScoutView.Text = (string)btn_ScoutView.Tag; }));
                    btn_GainCalibration.Invoke(new EventHandler(delegate { btn_GainCalibration.Text = (string)btn_GainCalibration.Tag; }));
                    btn_GeometryCalibration.Invoke(new EventHandler(delegate { btn_GeometryCalibration.Text = (string)btn_GeometryCalibration.Tag; }));
                    btn_HUCalibration.Invoke(new EventHandler(delegate { btn_HUCalibration.Text = (string)btn_HUCalibration.Tag; }));
                }
                else
                {
                    if (calibrationMode == CalibrationMode.ScoutView)
                    {
                        btn_ScoutView.Invoke(new EventHandler(delegate
                        {
                            btn_ScoutView.Enabled = true;
                            btn_ScoutView.Text = "Cancel";
                            Application.DoEvents();
                        }));
                    }
                    if (calibrationMode == CalibrationMode.Geometry)
                    {
                        btn_GeometryCalibration.Invoke(new EventHandler(delegate
                        {
                            btn_GeometryCalibration.Enabled = true;
                            btn_GeometryCalibration.Text = "Cancel";
                            Application.DoEvents();
                        }));
                    }
                    if (calibrationMode == CalibrationMode.HU)
                    {
                        btn_HUCalibration.Invoke(new EventHandler(delegate
                        {
                            btn_HUCalibration.Enabled = true;
                            btn_HUCalibration.Text = "Cancel";
                            Application.DoEvents();
                        }));
                    }
                    if (calibrationMode == CalibrationMode.Gain)
                    {
                        btn_GainCalibration.Invoke(new EventHandler(delegate
                        {
                            btn_GainCalibration.Enabled = true;
                            btn_GainCalibration.Text = "Cancel";
                            Application.DoEvents();
                        }));
                    }
                }
                #region normal acquisition
                btn_Adult.Invoke(new EventHandler(delegate { btn_Adult.Enabled = Enable; }));
                btn_Child.Invoke(new EventHandler(delegate { btn_Child.Enabled = Enable; }));

                btn_DryRun.Invoke(new EventHandler(delegate { btn_DryRun.Enabled = false; }));
                btn_ScoutView.Invoke(new EventHandler(delegate { btn_ScoutView.Enabled = Enable; }));
                btn_Acquisition.Invoke(new EventHandler(delegate { btn_Acquisition.Enabled = false; }));
                btn_MAR.Invoke(new EventHandler(delegate { btn_MAR.Enabled = false; }));

                btn_Half_Symmetric_ACQ.Invoke(new EventHandler(delegate { btn_Half_Symmetric_ACQ.Enabled = Enable; }));
                btn_Full_Symmetric_ACQ.Invoke(new EventHandler(delegate { btn_Full_Symmetric_ACQ.Enabled = Enable; }));
                btn_Full_Asymmetric_ACQ.Invoke(new EventHandler(delegate { btn_Full_Asymmetric_ACQ.Enabled = Enable; }));

                cbx_Resolution.Invoke(new EventHandler(delegate { cbx_Resolution.Enabled = Enable; }));

                txt_StudyTitle.Invoke(new EventHandler(delegate { txt_StudyTitle.Enabled = Enable; }));
                txt_SeriesTitle.Invoke(new EventHandler(delegate { txt_SeriesTitle.Enabled = Enable; }));
                txt_BodyPartExamined.Invoke(new EventHandler(delegate { txt_BodyPartExamined.Enabled = Enable; }));
                txt_PhysiciansName.Invoke(new EventHandler(delegate { txt_PhysiciansName.Enabled = Enable; }));
                txt_LotNumber.Invoke(new EventHandler(delegate { txt_LotNumber.Enabled = Enable; }));

                btn_LaserOn.Invoke(new EventHandler(delegate { btn_LaserOn.Enabled = Enable; }));
                btn_LaserOff.Invoke(new EventHandler(delegate { btn_LaserOff.Enabled = Enable; }));
                #endregion
            }*/
        }
        private void saveStudy()
        {
            /*
            PatientStudy study = new PatientStudy();
            if (settings.User == "NCKUH")
            {
                if (frm_tACQ.LaunchByHIS)
                {
                    if (frm_tACQ.HISPatient.OtherPatientIDs == "")
                        study.PatientID = frm_tACQ.HISPatient.ID;
                    else
                        study.PatientID = frm_tACQ.HISPatient.OtherPatientIDs;
                }
                else
                    study.PatientID = patient.ID;
            }
            else
                study.PatientID = patient.ID;
            study.StudyID = studyID;

            study.FileType = (acquireMode == AcquireMode.Scout) ? "Scout" : "Recon";
            if (isMAR) study.FileType = "MAR";
            study.Date = DateTime.Now;
            study.FileDirection = acquisition.StudyPath;
            study.StudyTitle = txt_StudyTitle.Text; //acquisition.StudyDescription;
            study.SeriesTitle = acquisition.SeriesDescription;
            study.AcquiredUserID = frm_tACQ.CurrentUserID;
            if (acquireMode != AcquireMode.DryRun)
                study.Protocol = acquisition.kVp.ToString() + " kVp" + acquisition.mA.ToString() + " mA";
            if (acquireMode != AcquireMode.Scout)
                study.StudyUID = acquisition.StudyUID;
            study.OrderingDoctorID = acquisition.PhysiciansName;
            study.LotNumber = acquisition.LotNumber;
            study.BodyPartExamined = acquisition.BodyPartExamined;
            frm_PatientManagement.SaveStudy(study);*/
        }
    }
}
