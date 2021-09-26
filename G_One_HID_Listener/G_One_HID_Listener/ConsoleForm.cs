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
                //text += Environment.NewLine;
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
    }
}
