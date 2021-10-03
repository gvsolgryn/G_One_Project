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
    public partial class AddDeviceForm : Form
    {
        public AddDeviceForm()
        {
            InitializeComponent();
        }

        private void Accept_btn_Click(object sender, EventArgs e)
        {
            string _sensorName = this.SensorName.Text;
            string _sensorStatus = this.comboBox1.SelectedItem.ToString();
        }
    }
}
