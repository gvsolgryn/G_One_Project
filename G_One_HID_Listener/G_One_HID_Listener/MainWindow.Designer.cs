﻿
namespace G_One_HID_Listener
{
    partial class MainWindow
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestButton1 = new System.Windows.Forms.Button();
            this.TestButton2 = new System.Windows.Forms.Button();
            this.ledImageButton1 = new G_One_HID_Listener.ImageButton.LedImageButton();
            this.powerStripImageButton1 = new G_One_HID_Listener.ImageButton.PowerStripImageButton();
            this.LED_Control = new System.Windows.Forms.Label();
            this.PowerStrip_Control = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gOneToolStripMenuItem,
            this.ToolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(684, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gOneToolStripMenuItem
            // 
            this.gOneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TrayToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.gOneToolStripMenuItem.Name = "gOneToolStripMenuItem";
            this.gOneToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.gOneToolStripMenuItem.Text = "G.One";
            // 
            // TrayToolStripMenuItem
            // 
            this.TrayToolStripMenuItem.Name = "TrayToolStripMenuItem";
            this.TrayToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.TrayToolStripMenuItem.Text = "프로그램 최소화";
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.ExitToolStripMenuItem.Text = "프로그램 종료";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // ToolsToolStripMenuItem
            // 
            this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConsoleToolStripMenuItem});
            this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.ToolsToolStripMenuItem.Text = "도구";
            // 
            // ConsoleToolStripMenuItem
            // 
            this.ConsoleToolStripMenuItem.Name = "ConsoleToolStripMenuItem";
            this.ConsoleToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.ConsoleToolStripMenuItem.Text = "콘솔창 실행";
            this.ConsoleToolStripMenuItem.Click += new System.EventHandler(this.ConsoleToolStripMenuItem_Click);
            // 
            // TestButton1
            // 
            this.TestButton1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TestButton1.Location = new System.Drawing.Point(516, 326);
            this.TestButton1.Name = "TestButton1";
            this.TestButton1.Size = new System.Drawing.Size(75, 23);
            this.TestButton1.TabIndex = 1;
            this.TestButton1.Text = "테스트1";
            this.TestButton1.UseVisualStyleBackColor = true;
            this.TestButton1.Click += new System.EventHandler(this.TestButton1_Click);
            // 
            // TestButton2
            // 
            this.TestButton2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestButton2.Location = new System.Drawing.Point(597, 326);
            this.TestButton2.Name = "TestButton2";
            this.TestButton2.Size = new System.Drawing.Size(75, 23);
            this.TestButton2.TabIndex = 2;
            this.TestButton2.Text = "테스트2";
            this.TestButton2.UseVisualStyleBackColor = true;
            this.TestButton2.Click += new System.EventHandler(this.TestButton2_Click);
            // 
            // ledImageButton1
            // 
            this.ledImageButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ledImageButton1.BackgroundImage")));
            this.ledImageButton1.Image_01 = ((System.Drawing.Image)(resources.GetObject("ledImageButton1.Image_01")));
            this.ledImageButton1.Image_02 = ((System.Drawing.Image)(resources.GetObject("ledImageButton1.Image_02")));
            this.ledImageButton1.Location = new System.Drawing.Point(131, 27);
            this.ledImageButton1.Name = "ledImageButton1";
            this.ledImageButton1.Size = new System.Drawing.Size(150, 150);
            this.ledImageButton1.TabIndex = 3;
            // 
            // powerStripImageButton1
            // 
            this.powerStripImageButton1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("powerStripImageButton1.BackgroundImage")));
            this.powerStripImageButton1.Image_01 = ((System.Drawing.Image)(resources.GetObject("powerStripImageButton1.Image_01")));
            this.powerStripImageButton1.Image_02 = ((System.Drawing.Image)(resources.GetObject("powerStripImageButton1.Image_02")));
            this.powerStripImageButton1.Location = new System.Drawing.Point(399, 27);
            this.powerStripImageButton1.Name = "powerStripImageButton1";
            this.powerStripImageButton1.Size = new System.Drawing.Size(150, 150);
            this.powerStripImageButton1.TabIndex = 4;
            // 
            // LED_Control
            // 
            this.LED_Control.AutoSize = true;
            this.LED_Control.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LED_Control.Location = new System.Drawing.Point(141, 180);
            this.LED_Control.Name = "LED_Control";
            this.LED_Control.Size = new System.Drawing.Size(129, 37);
            this.LED_Control.TabIndex = 5;
            this.LED_Control.Text = "LED 제어";
            this.LED_Control.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PowerStrip_Control
            // 
            this.PowerStrip_Control.AutoSize = true;
            this.PowerStrip_Control.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold);
            this.PowerStrip_Control.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PowerStrip_Control.Location = new System.Drawing.Point(392, 180);
            this.PowerStrip_Control.Name = "PowerStrip_Control";
            this.PowerStrip_Control.Size = new System.Drawing.Size(161, 37);
            this.PowerStrip_Control.TabIndex = 6;
            this.PowerStrip_Control.Text = "멀티탭 제어";
            this.PowerStrip_Control.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 361);
            this.Controls.Add(this.PowerStrip_Control);
            this.Controls.Add(this.LED_Control);
            this.Controls.Add(this.powerStripImageButton1);
            this.Controls.Add(this.ledImageButton1);
            this.Controls.Add(this.TestButton2);
            this.Controls.Add(this.TestButton1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "G.One IoT 제어";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gOneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConsoleToolStripMenuItem;
        private System.Windows.Forms.Button TestButton1;
        private System.Windows.Forms.Button TestButton2;
        private ImageButton.LedImageButton ledImageButton1;
        private ImageButton.PowerStripImageButton powerStripImageButton1;
        private System.Windows.Forms.Label LED_Control;
        private System.Windows.Forms.Label PowerStrip_Control;
    }
}
