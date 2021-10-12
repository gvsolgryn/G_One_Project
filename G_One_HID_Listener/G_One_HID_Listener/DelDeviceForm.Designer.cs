namespace G_One_HID_Listener
{
    partial class DelDeviceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DelDeviceForm));
            this.DeleteButton = new System.Windows.Forms.Button();
            this.sensorList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(237, 126);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 0;
            this.DeleteButton.Text = "삭제";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // sensorList
            // 
            this.sensorList.DropDownHeight = 200;
            this.sensorList.FormattingEnabled = true;
            this.sensorList.IntegralHeight = false;
            this.sensorList.Location = new System.Drawing.Point(12, 12);
            this.sensorList.Name = "sensorList";
            this.sensorList.Size = new System.Drawing.Size(300, 20);
            this.sensorList.TabIndex = 1;
            // 
            // DelDeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 161);
            this.Controls.Add(this.sensorList);
            this.Controls.Add(this.DeleteButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DelDeviceForm";
            this.Text = "기기 삭제";
            this.Load += new System.EventHandler(this.DelDeviceForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.ComboBox sensorList;
    }
}