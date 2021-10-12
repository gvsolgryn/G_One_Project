using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G_One_HID_Listener
{
    using HidLibrary;
    using uPLibrary.Networking.M2Mqtt;
    using MySql.Data;
    using MySql.Data.MySqlClient;
    using G_One_HID_Listener.module;

    public partial class MainWindow : Form
    {
        /* HID Data 관련 코드 */
        private List<HidDevice> _devices = new List<HidDevice>();

        private const ushort ConsoleUsagePage = 0xFF31;
        private const int ConsoleUsage = 0x0074;

        private static IEnumerable<HidDevice> GetListableDevices() =>
            HidDevices.Enumerate()
                .Where(d => d.IsConnected)
                .Where(device => device.Capabilities.InputReportByteLength > 0)
                .Where(device => (ushort)device.Capabilities.UsagePage == ConsoleUsagePage)
                .Where(device => (ushort)device.Capabilities.Usage == ConsoleUsage);

        private void UpdateHidDevices(bool disconnected)
        {
            var devices = GetListableDevices().ToList();

            if (!disconnected)
            {
                foreach (var device in devices)
                {
                    var deviceExists = _devices.Aggregate(false, (current, dev) => current | dev.DevicePath.Equals(device.DevicePath));

                    if (device == null || deviceExists) continue;

                    _devices.Add(device);
                    device.OpenDevice();

                    device.MonitorDeviceEvents = true;

                    AppendText($"G.One 키보드 연결 됨 : {GetManufacturerString(device)} {GetProductString(device)} ({device.Attributes.VendorId:X4}:{device.Attributes.ProductId:X4}:{device.Attributes.Version:X4})\n");

                    device.Inserted += DeviceAttachedHandler;
                    device.Removed += DeviceRemovedHandler;

                    device.ReadReport(OnReport);
                    device.CloseDevice();
                }
            }
            else
            {
                foreach (var existingDevice in _devices)
                {
                    var deviceExists = devices.Aggregate(false, (current, device) => current | existingDevice.DevicePath.Equals(device.DevicePath));
                    
                    if (!deviceExists)
                    {
                        AppendText($"G.One 키보드 연결 해제 됨 : ({existingDevice.Attributes.VendorId:X4}:{existingDevice.Attributes.ProductId:X4}:{existingDevice.Attributes.Version:X4})\n");
                    }
                }
            }

            _devices = devices;
        }

        private void DeviceAttachedHandler()
        {
            UpdateHidDevices(false);
        }

        private void DeviceRemovedHandler()
        {
            UpdateHidDevices(true);
        }

        private void OnReport(HidReport report)
        {
            var data = report.Data;
            var outputString = string.Empty;

             if (0 < data.Length || report.Data != null)
            {
                outputString = Encoding.UTF8.GetString(data).Trim('\0');
                if (outputString == string.Empty) UpdateHidDevices(true);
                else
                {
                    AppendText(outputString);
                    HID_Status_Change(outputString.TrimEnd('\n'));
                }
            }
            else
            {
                MessageBox.Show(@"에러!");
            }
            

            foreach (var device in _devices)
            {
                device.ReadReport(OnReport);
            }
        }
        
        private static string GetProductString(IHidDevice d)
        {
            if (d == null) return "";
            d.ReadProduct(out var bs);
            return System.Text.Encoding.Default.GetString(bs.Where(b => b > 0).ToArray());
        }

        private static string GetManufacturerString(IHidDevice d)
        {
            if (d == null) return "";
            d.ReadManufacturer(out var bs);
            return System.Text.Encoding.Default.GetString(bs.Where(b => b > 0).ToArray());
        }

        /* Console Form 관련 코드 */

        readonly ConsoleForm _logForm = new ConsoleForm();

        private void AppendText(string text)
        {
            _logForm.ConsoleText(_logForm.consoleTextBox, text);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _logForm.StartPosition = FormStartPosition.CenterScreen;
            
            if(_logForm.WindowState == FormWindowState.Minimized)
            {
                _logForm.WindowState = FormWindowState.Normal;
                _logForm.ShowIcon = true;
                _logForm.ShowInTaskbar = true;
            }
            _logForm.Show();
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        /* 테스트 버튼 관련 코드 */
        private void TestButton1_Click(object sender, EventArgs e)
        {
            AppendText("테스트버튼 1 클릭됨.");
            var db = new DB_Module();
            string sql = "SELECT * FROM sensor_status";
            db.Command(sql);
            MySqlDataReader table = db.Command(sql).ExecuteReader();
            while (table.Read())
            {
                Console.WriteLine("sensor : {0} | status : {1}", table["sensor"], table["status"]);
            }
        }

        private void TestButton2_Click(object sender, EventArgs e)
        {
            AppendText("키보드가 제거 되었습니다.");
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show(@"정말 종료 하시겠습니까?", @"프로그램 종료", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                e.Cancel = true;
            }
        }

        /* 기기추가Form 관련 코드 */
        private void 기기추가ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDeviceForm addDeviceForm = new AddDeviceForm
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            addDeviceForm.Show();
        }

        private void 기기삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DelDeviceForm delDeviceForm = new DelDeviceForm
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            delDeviceForm.Show();
        }

        /* MainWindows 액션 관련 코드 */
        private void MainWindow_Load(object sender, EventArgs e)
        {
            UpdateHidDevices(false);
            this.CenterToScreen();
        }

        private void LedImageButton1_Click(object sender, EventArgs e)
        {
            var db = new DB_Module();

            string sql = "SELECT * FROM sensor_status WHERE sensor='LED'";
            var table = db.TableLoad(sql);

            var sensor = string.Empty;
            var status = string.Empty;

            try
            {
                while (table.Read())
                {
                    sensor = table["sensor"].ToString();
                    status = table["status"].ToString();

                    string text = ($"sensor : {sensor} | status : {status}");
                    AppendText(text);
                }
                ChangeStatus(Int32.Parse(status), sensor, "iot/LED");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PowerStripImageButton1_Click(object sender, EventArgs e)
        {
            var db = new DB_Module();

            string sql = "SELECT * FROM sensor_status WHERE sensor='Power_Strip'";
            var table = db.TableLoad(sql);

            var sensor = string.Empty;
            var status = string.Empty;

            try
            {
                while (table.Read())
                {
                    sensor = table["sensor"].ToString();
                    status = table["status"].ToString();

                    string text = ($"sensor : {sensor} | status : {status}");
                    AppendText(text);
                }
                ChangeStatus(Int32.Parse(status), sensor, "iot/Power_Strip");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HID_Status_Change(string HID_Data)
        {
            var db = new DB_Module();
            try
            {
                var sql = "SELECT * FROM sensor_status";
                var table = db.TableLoad(sql);
                var sensor = string.Empty;
                var status = string.Empty;

                while (table.Read())
                {
                    sensor = table["sensor"].ToString();
                    status = table["status"].ToString();
                    if (HID_Data == "MULTI") HID_Data = HID_Data.Replace("MULTI", "Power_Strip");
                    if(HID_Data.ToLower() == sensor.ToLower())
                    {
                        var topic = "iot/" + HID_Data;
                        Console.WriteLine(topic);
                        
                        ChangeStatus(Int32.Parse(status), HID_Data, topic);

                        HID_Data = String.Empty;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("HID_Stat_Change Error : " + ex.Message);
            }

            /*if (HID_Data == "LED")
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                string sql = "SELECT STATUS,SENSOR from sensor_status where ID='1'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader tableData = cmd.ExecuteReader();
                tableData.Read();
                Console.WriteLine("Status: {0}", tableData["STATUS"]);
                string name, topic;
                int status;
                status = (int)tableData["STATUS"];
                name = tableData["SENSOR"].ToString();
                topic = "iot/LED";
                ChangeStatus(status, name, topic);
                tableData.Close();
                conn.Close();
            }
            else if (HID_Data == "MULTI")
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                string sql = "SELECT STATUS,SENSOR from sensor_status where sensor='MULTI'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader tableData = cmd.ExecuteReader();
                tableData.Read();
                Console.WriteLine("Status: {0}", tableData["STATUS"]);
                string name, topic;
                int status;
                status = (int)tableData["STATUS"];
                name = tableData["SENSOR"].ToString();
                topic = "iot/Power_Strip";
                ChangeStatus(status, name, topic);
                tableData.Close();
                conn.Close();
            }*/
        }

        public void ChangeStatus(int status, string name, string topic)
        {
            MqttClient client = new MqttClient("gone.gvsolgryn.de");
            var db = new DB_Module();
            try
            {
                client.Connect("G.One_HID", "ID", "PW");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                AppendText(ex.Message);
            }

            if (status == 0)
            {
                try
                {
                    string sql = "UPDATE sensor_status SET STATUS = 1, last_use = now() WHERE SENSOR = @sensorName";

                    db.Execute(sql, new[] { "@sensorName" }, new[] { name });

                    client.Publish(topic, Encoding.UTF8.GetBytes("1"));

                    AppendText(name + " 켜기 완료!");
                    
                    if (name == "LED")
                    {
                        ledImageButton1.BackgroundImage = ledImageButton1.Image_02;
                    }
                    else if (name == "Power_Strip")
                    {
                        powerStripImageButton1.BackgroundImage = powerStripImageButton1.Image_02;
                    }
                    
                }
                catch (Exception e)
                {
                    AppendText("데이터 베이스 에러로그 : " + e.Message + "\n");
                }
            }
            else if (status == 1)
            {
                try
                {
                    string sql = "UPDATE sensor_status SET STATUS = 0, last_use = now() WHERE SENSOR = @sensorName";

                    db.Execute(sql, new[] { "@sensorName" }, new[] { name });

                    client.Publish(topic, Encoding.UTF8.GetBytes("0"));

                    AppendText(name + " 끄기 완료!");

                    if (name == "LED")
                    {
                        ledImageButton1.BackgroundImage = ledImageButton1.Image_01;
                    }
                    else if (name == "Power_Strip")
                    {
                        powerStripImageButton1.BackgroundImage = powerStripImageButton1.Image_01;
                    }
                }
                catch (Exception e)
                {
                    AppendText("데이터 베이스 에러로그 : " + e.Message);
                }
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        
    }
}
