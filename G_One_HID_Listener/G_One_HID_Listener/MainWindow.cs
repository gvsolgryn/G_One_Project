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
    using HidLibrary;
    using MySql.Data.MySqlClient;
    using uPLibrary.Networking.M2Mqtt;

    public partial class MainWindow : Form
    {
        /* HID Data 관련 코드 */
        private readonly List<HidDevice> _devices = new List<HidDevice>();

        public const ushort ConsoleUsagePage = 0xFF31;
        public const int ConsoleUsage = 0x0074;

        private static IEnumerable<HidDevice> GetListableDevices() =>
            HidDevices.Enumerate()
                .Where(d => d.IsConnected)
                .Where(device => device.Capabilities.InputReportByteLength > 0)
                .Where(device => (ushort)device.Capabilities.UsagePage == ConsoleUsagePage)
                .Where(device => (ushort)device.Capabilities.Usage == ConsoleUsage);

        public void Device(bool disconnected)
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

                    device.Inserted += DeviceAttachedHandler;
                    device.Removed += DeviceRemovedHandler;

                    device.MonitorDeviceEvents = true;

                    AppendText("G.One 키보드 연결 됨");
                    device.ReadReport(OnReport);
                    device.CloseDevice();
                }
            }
        }

        private void OnReport(HidReport report)
        {
            var data = report.Data;
            var stringData = string.Empty;

            if (0 < data.Length)
            {
                stringData = Encoding.UTF8.GetString(data).Trim('\0');
            }
            else
            {
                MessageBox.Show("에러!");
            }
            AppendText(stringData);
            stringData = string.Empty;

            foreach (var device in _devices)
            {
                device.ReadReport(OnReport);
            }
        }

        private void DeviceAttachedHandler()
        {
            AppendText("키보드가 연결 되었습니다.");
        }

        private void DeviceRemovedHandler()
        {
            AppendText("키보드가 제거 되었습니다.");
        }

        /* Console Form 관련 코드 */
        readonly ConsoleForm consoleForm = new ConsoleForm();

        public void AppendText(string text)
        {
            consoleForm.ConsoleText(consoleForm.consoleTextBox, text);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            consoleForm.Close();
        }

        private void ConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consoleForm.Show();
        }

        public MainWindow()
        {
            InitializeComponent();
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

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.Yes != MessageBox.Show("정말 종료 하시겠습니까?", "프로그램 종료", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            {
                e.Cancel = true;
            }
        }
    }
}
