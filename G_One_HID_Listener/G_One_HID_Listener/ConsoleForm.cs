using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G_One_HID_Listener
{
    using module;
    public partial class ConsoleForm : Form
    {
        public ConsoleForm()
        {
            InitializeComponent();
        }

        private readonly string _now = DateTime.Now.ToString("HH:mm") + " : ";

        public void ConsoleText(RichTextBox logRichTextBox, string text)
        {
            if (!logRichTextBox.InvokeRequired)
            {
                text += Environment.NewLine;
                logRichTextBox.AppendText(_now + text);
                logRichTextBox.ScrollToCaret();
            }
            else
            {
                logRichTextBox.Invoke(new Action<RichTextBox, string>(ConsoleText), logRichTextBox, text);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            consoleTextBox.Clear();
        }

        private void ConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowIcon = false;
                this.ShowInTaskbar = false;
            }
        }

        private void DBLoad_btn_Click(object sender, EventArgs e)
        {
            var db = new DB_Module();
            var sql = "SELECT * FROM sensor_status";
            var table = db.TableLoad(sql);

            while (table.Read())
            {
                string text = $"이름 : {table["sensor"]} // 상태 : {table["status"]} // 마지막 사용 시간 : {table["Last_Use"]}";
                ConsoleText(this.consoleTextBox, text);
            }
            ConsoleText(this.consoleTextBox, "============================");
        }
    }
}
