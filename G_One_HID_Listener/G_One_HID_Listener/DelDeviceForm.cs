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
    public partial class DelDeviceForm : Form
    {
        public DelDeviceForm()
        {
            InitializeComponent();
        }

        private void DelDeviceForm_Load(object sender, EventArgs e)
        {
            var db = new DB_Module();
            var sql = "SELECT sensor FROM sensor_status";
            var table = db.TableLoad(sql);

            while (table.Read())
            {
                this.sensorList.Items.Add(table["sensor"].ToString());
            }

            this.sensorList.SelectedIndex = 0;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var db = new DB_Module();
            var sql = "DELETE FROM sensor_status WHERE sensor=@sensor";
            var sensor = this.sensorList.SelectedItem.ToString();

            db.Execute(sql, new[] { "@sensor" }, new[] { sensor });

            MessageBox.Show($"{sensor} 삭제 완료", "기기 삭제", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            this.Close();
        }
    }
}
