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
        /* DB 관련 코드 */
        private static MySqlConnection Database()
        {
            const string connStr = "server=gone.gvsolgryn.de;" +
                                   "user=gvsolgryn;" +
                                   "database=G_One_DB;" +
                                   "port=3306;" +
                                   "password=tkdeh3554";

            var conn = new MySqlConnection(connStr);

            return conn;
        }

        private static MySqlCommand Test()
        {
            return null;
        }

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
                    Console.WriteLine($"G.One 키보드 연결 됨 : {GetManufacturerString(device)} {GetProductString(device)} ({device.Attributes.VendorId:X4}:{device.Attributes.ProductId:X4}:{device.Attributes.Version:X4})\n");

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
                else AppendText(outputString);
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

        ConsoleForm _logForm = new ConsoleForm();

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
            var test = new DB_Module();
            string sql = "SELECT * FROM sensor_status";
            test.Command(sql);
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
            AddDeviceForm addDeviceForm = new AddDeviceForm();

            addDeviceForm.StartPosition = FormStartPosition.CenterScreen;
            addDeviceForm.Show();
        }

        /* MainWindows 액션 관련 코드 */
        private void MainWindow_Load(object sender, EventArgs e)
        {
            UpdateHidDevices(false);
            this.CenterToScreen();
        }
    }
}
