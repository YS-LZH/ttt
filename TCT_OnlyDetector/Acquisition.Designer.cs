namespace TCT_OnlyDetector
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
            // 
            // Acquisition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_Acquisition);
            this.Name = "Acquisition";
            this.Text = "Acquisition";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Acquisition;
    }
}

