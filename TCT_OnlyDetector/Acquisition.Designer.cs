﻿namespace TCT_OnlyDetector
{
    partial class Acquisition
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Acquisition = new System.Windows.Forms.Button();
            this.ptb_Preview = new System.Windows.Forms.PictureBox();
            this.pgb_ProgressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Acquisition
            // 
            this.btn_Acquisition.Font = new System.Drawing.Font("新細明體", 20F);
            this.btn_Acquisition.Location = new System.Drawing.Point(700, 41);
            this.btn_Acquisition.Name = "btn_Acquisition";
            this.btn_Acquisition.Size = new System.Drawing.Size(67, 67);
            this.btn_Acquisition.TabIndex = 0;
            this.btn_Acquisition.Text = "start";
            this.btn_Acquisition.UseVisualStyleBackColor = true;
            this.btn_Acquisition.Click += new System.EventHandler(this.btn_Acquisition_Click);
            // 
            // ptb_Preview
            // 
            this.ptb_Preview.Location = new System.Drawing.Point(12, 12);
            this.ptb_Preview.Name = "ptb_Preview";
            this.ptb_Preview.Size = new System.Drawing.Size(313, 274);
            this.ptb_Preview.TabIndex = 1;
            this.ptb_Preview.TabStop = false;
            // 
            // pgb_ProgressBar
            // 
            this.pgb_ProgressBar.Location = new System.Drawing.Point(12, 323);
            this.pgb_ProgressBar.Name = "pgb_ProgressBar";
            this.pgb_ProgressBar.Size = new System.Drawing.Size(313, 35);
            this.pgb_ProgressBar.TabIndex = 2;
            // 
            // Acquisition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pgb_ProgressBar);
            this.Controls.Add(this.ptb_Preview);
            this.Controls.Add(this.btn_Acquisition);
            this.Name = "Acquisition";
            this.Text = "Acquisition";
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Preview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Acquisition;
        private System.Windows.Forms.PictureBox ptb_Preview;
        private System.Windows.Forms.ProgressBar pgb_ProgressBar;
    }
}

