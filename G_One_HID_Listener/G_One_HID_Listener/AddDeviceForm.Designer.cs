
namespace G_One_HID_Listener
{
    partial class AddDeviceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddDeviceForm));
            this.sensorStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.accept_btn = new System.Windows.Forms.Button();
            this.sensorList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // sensorStatus
            // 
            this.sensorStatus.FormattingEnabled = true;
            this.sensorStatus.Items.AddRange(new object[] {
            "켜짐",
            "꺼짐"});
            this.sensorStatus.Location = new System.Drawing.Point(75, 49);
            this.sensorStatus.Name = "sensorStatus";
            this.sensorStatus.Size = new System.Drawing.Size(198, 20);
            this.sensorStatus.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "기기 이름";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "기기 상태";
            // 
            // accept_btn
            // 
            this.accept_btn.Location = new System.Drawing.Point(279, 12);
            this.accept_btn.Name = "accept_btn";
            this.accept_btn.Size = new System.Drawing.Size(121, 57);
            this.accept_btn.TabIndex = 4;
            this.accept_btn.Text = "등록";
            this.accept_btn.UseVisualStyleBackColor = true;
            this.accept_btn.Click += new System.EventHandler(this.Accept_btn_Click);
            // 
            // sensorList
            // 
            this.sensorList.FormattingEnabled = true;
            this.sensorList.Location = new System.Drawing.Point(75, 10);
            this.sensorList.Name = "sensorList";
            this.sensorList.Size = new System.Drawing.Size(198, 20);
            this.sensorList.TabIndex = 5;
            // 
            // AddDeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(412, 81);
            this.Controls.Add(this.sensorList);
            this.Controls.Add(this.accept_btn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sensorStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddDeviceForm";
            this.Text = "기기 추가";
            this.Load += new System.EventHandler(this.AddDeviceForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox sensorStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button accept_btn;
        private System.Windows.Forms.ComboBox sensorList;
    }
}