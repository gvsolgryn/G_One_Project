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
    using MySql.Data.MySqlClient;
    using module;

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
            return Encoding.Default.GetString(bs.Where(b => b > 0).ToArray());
        }

        private static string GetManufacturerString(IHidDevice d)
        {
            if (d == null) return "";
            d.ReadManufacturer(out var bs);
            return Encoding.Default.GetString(bs.Where(b => b > 0).ToArray());
        }

        /* Console Form 관련 코드 */

        readonly ConsoleForm _logForm = new ConsoleForm();

        private void AppendText(string text)
        {
            _logForm.ConsoleText(_logForm.consoleTextBox, text);
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

        /* 테스트 버튼 관련 코드 */
        private void TestButton1_Click(object sender, EventArgs e)
        {
            AppendText("테스트버튼 1 클릭됨.");
        }

        private void TestButton2_Click(object sender, EventArgs e)
        {
            AppendText("키보드가 제거 되었습니다.");
        }

        /* 기기추가Form 관련 코드 */
        private void 기기추가ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDeviceForm addDeviceForm = new AddDeviceForm
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            addDeviceForm.ShowDialog();
        }

        private void 기기삭제ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DelDeviceForm delDeviceForm = new DelDeviceForm
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            delDeviceForm.ShowDialog();
        }

        /* MainWindows 액션 관련 코드 */
        private void MainWindow_Load(object sender, EventArgs e)
        {
            UpdateHidDevices(false);
            this.CenterToScreen();
        }
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show(@"정말 종료 하시겠습니까?", @"프로그램 종료", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                e.Cancel = true;
            }

            System.Diagnostics.Process.GetCurrentProcess().Kill();
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
                        
                        ChangeStatus(Int32.Parse(status), HID_Data, topic);

                        HID_Data = String.Empty;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("HID_Stat_Change Error : " + ex.Message);
            }
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

        /* StripMenu (상단바) 구간 */
        public MainWindow()
        {
            InitializeComponent();

            this.Load += Tray_Icon_Load;
        }

        private void Tray_Icon_Load(object sender, EventArgs e)
        {
            Tray_Icon.ContextMenuStrip = Tray_Icon_Strip;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.ShowIcon = false;
            Tray_Icon.Visible = true;
        }

        private void Tray_Icon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void ShowTrayStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void ExitTrayStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
