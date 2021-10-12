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
    public partial class AddDeviceForm : Form
    {
        public AddDeviceForm()
        {
            InitializeComponent();
        }

        private void Accept_btn_Click(object sender, EventArgs e)
        {
            string _sensorName = SensorName.Text;
            string _sensorStatus = comboBox1.SelectedItem.ToString();

            if (_sensorStatus == "켜짐") _sensorStatus = "1";
            else if (_sensorStatus == "꺼짐") _sensorStatus = "0";

            var db = new DB_Module();

            var sql = "INSERT INTO sensor_status(sensor, status, led_value, last_use) VALUES(@sensor, @status, '0', now())";

            db.Execute(sql, new[] { "@sensor", "@status" }, new[] { _sensorName, _sensorStatus });

            MessageBox.Show($"{_sensorName} 추가 완료", "기기 추가", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            this.Close();
        }

        private void AddDeviceForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 1;
        }
    }
}
