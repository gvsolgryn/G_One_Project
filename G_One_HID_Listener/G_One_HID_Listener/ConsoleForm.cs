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

        private readonly string Now = DateTime.Now.ToString("HH:mm") + " : ";

        public void ConsoleText(RichTextBox consoleTextBox, string text)
        {
            if (!consoleTextBox.InvokeRequired)
            {
                text += Environment.NewLine;
                consoleTextBox.AppendText(Now + text);
                consoleTextBox.ScrollToCaret();
            }
            else
            {
                consoleTextBox.Invoke(new Action<RichTextBox, string>(ConsoleText), consoleTextBox, text);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            consoleTextBox.Clear();
        }
    }
}
