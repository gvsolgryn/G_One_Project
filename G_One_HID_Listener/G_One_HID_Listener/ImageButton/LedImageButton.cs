using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G_One_HID_Listener.ImageButton
{
    public partial class LedImageButton : UserControl
    {
        private Image image01 = SystemIcons.Hand.ToBitmap();
        private Image image02 = SystemIcons.Hand.ToBitmap();

        [Browsable(true)]
        public Image Image_01
        {
            get
            {
                return this.image01;
            }
            set
            {
                this.image01 = value;
                this.BackgroundImage = this.image01;
            }
        }

        [Browsable(true)]
        public Image Image_02
        {
            get
            {
                return this.image02;
            }
            set
            {
                this.image02 = value;
                this.BackgroundImage = this.image02;
            }
        }

        readonly ConsoleForm consoleForm = new ConsoleForm();

        public void AppendText(string text)
        {
            consoleForm.ConsoleText(consoleForm.consoleTextBox, text);
        }

        public LedImageButton()
        {
            InitializeComponent();
        }

        private void LedImageButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.BackgroundImage == image01)
            {
                this.BackgroundImage = image02;
            }

            else if (this.BackgroundImage == image02)
            {
                this.BackgroundImage = image01;
            }
        }
    }
}
