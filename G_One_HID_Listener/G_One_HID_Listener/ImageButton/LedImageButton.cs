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
    using module;
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

        private void LedImageButton_Load(object sender, EventArgs e)
        {
            var db = new DB_Module();

            var sql = "SELECT * FROM sensor_status WHERE sensor='LED'";
            var table = db.TableLoad(sql);
            var status = string.Empty;

            while (table.Read())
            {
                status = table["status"].ToString();
            }

            if (status == "0")
            {
                this.BackgroundImage = image01;
            }
            else if (status == "1")
            {
                this.BackgroundImage = image02;
            }
        }
    }
}
