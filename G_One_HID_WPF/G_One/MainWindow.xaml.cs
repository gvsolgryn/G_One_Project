using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace G_One
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    using Module;
    using HidLibrary;
    using System.Threading;
    using System.Collections.ObjectModel;

    public class AddComboBox
    {
        public string SensorName { get; set; }
    }

    public class TypeComboBox
    {
        public string SensorType { get; set; }
    }

    public class RemoveComboBox
    {
        public string RemoveSensorName { get; set; }
    }

    public partial class MainWindow : Window
    {

        /*----------------------------------------------------------------------------------------------------------------------------*/

        private List<HidDevice> _devices = new List<HidDevice>();

        private const ushort ConsoleUsagePage = 0xFF31;
        private const int ConsoleUsage = 0x0074;
        int ledValue = 0;

        
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
                    string text = $"G.One 키보드 연결 됨 : {GetManufacturerString(device)} {GetProductString(device)} ({device.Attributes.VendorId:X4}:{device.Attributes.ProductId:X4}:{device.Attributes.Version:X4})\n";
                    AppendText(text);

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
                    HID_Status_ChangeAsync(outputString.TrimEnd('\n'));
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

        public void HID_Status_ChangeAsync(string HID_Data)
        {
            var db = new DB_Module();
            var mqtt = new MQTT_Module();
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
                    if (HID_Data.ToLower() == sensor.ToLower())
                    {
                        var deviceControl = new DeviceControl();

                        var topic = "iot/" + HID_Data;

                        deviceControl.StatusChange(Int32.Parse(status), HID_Data, topic);

                        string sql2 = "UPDATE sensor_status SET status = @sensorStatus, last_use = now() WHERE sensor = @sensorName";
                        db.Execute(sql2, new[] { "@sensorStatus", "@sensorName" }, new[] { status.ToString(), HID_Data });
                        mqtt.Publish(topic, status.ToString());

                        HID_Data = String.Empty;
                    }

                    else if (HID_Data.ToLower() == "LED_UP")
                    {
                        var deviceControl = new DeviceControl();
                        ledValue = ledValue + 25;
                        if (ledValue > 100)
                        {
                            ledValue = 100;
                        }
                        deviceControl.LedValueChange("Brightness_control_LED", "iot/LEDAdjust", ledValue.ToString());
                    }

                    else if (HID_Data.ToLower() == "LED_DOWN")
                    {
                        var deviceControl = new DeviceControl();
                        ledValue = ledValue - 25;
                        if (ledValue <= 0)
                        {
                            ledValue = 0;
                        }
                        deviceControl.LedValueChange("Brightness_control_LED", "iot/LEDAdjust", ledValue.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HID_Stat_Change Error : " + ex.Message);
            }
        }

        private void AppendText(string text)
        {
            if (CMD.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.CMD.AppendText(text);
                this.CMD.ScrollToEnd();
            }
            else
            {
                this.CMD.Dispatcher.BeginInvoke(new Action(() => this.CMD.AppendText(text)));
                this.CMD.Dispatcher.BeginInvoke(new Action(() => this.CMD.ScrollToEnd()));
            }
            
        }

        /*----------------------------------------------------------------------------------------------------------------------------*/

        public static List<DevicePanel> devicePanel = new List<DevicePanel>();

        public static readonly List<string> listSensor = new List<string>();
        public static readonly List<string> listStatus = new List<string>();
        public static readonly List<string> listType = new List<string>();

        private ObservableCollection<AddComboBox> _addComboBox = new ObservableCollection<AddComboBox>();
        private ObservableCollection<TypeComboBox> _typeComboBox = new ObservableCollection<TypeComboBox>();
        private ObservableCollection<RemoveComboBox> _removeComboBox = new ObservableCollection<RemoveComboBox>();
        public MainWindow()
        {
            InitializeComponent();

            Add_DevicePanel();

            ComboDataLoad();
        }

        public void Add_DevicePanel()
        {
            DB_Load();

            for (int i = 0; i < listSensor.Count; i++)
            {
                var _devicePanel = new DevicePanel(this);
                devicePanel.Add(_devicePanel);
                MainStackPanel.Children.Add(_devicePanel);
            }
        }

        private void DB_Load()
        {
            listSensor.Clear();
            listStatus.Clear();
            listType.Clear();

            DB_Module db = new DB_Module();

            string sql = "SELECT * FROM sensor_status";

            try
            {
                var table = db.TableLoad(sql);

                while (table.Read())
                {
                    listSensor.Add(table["sensor"].ToString());
                    listStatus.Add(table["status"].ToString());
                    listType.Add(table["device_type"].ToString());

                    _removeComboBox.Add(new RemoveComboBox { RemoveSensorName = table["sensor"].ToString() });
                }

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }

            RemoveComboDeviceName.ItemsSource = _removeComboBox;

        }

        private void ComboDataLoad()
        {
            try
            {
                DB_Module db = new DB_Module();
                string sql = "SELECT * FROM compatible_device";
                var table = db.TableLoad(sql);

                while (table.Read())
                {
                    _addComboBox.Add(new AddComboBox { SensorName = table["name"].ToString() });
                    _typeComboBox.Add(new TypeComboBox { SensorType = table["device_type"].ToString() });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Add_Device_Page : " + ex.Message);
            }

            ComboDeviceName.ItemsSource = _addComboBox;
            ComboDeviceType.ItemsSource = _typeComboBox;
        }

        public void LoadPanel()
        {
            string[] arrSensor = listSensor.ToArray();
            string[] arrStatus = listStatus.ToArray();
            string[] arrType = listType.ToArray();

            for (int i = 0; i < listSensor.Count(); i++)
            {
                devicePanel[i].DeviceNameChange(arrSensor[i]);
                devicePanel[i].TopicChange(arrSensor[i]);
                devicePanel[i].DeviceInfoChange($"{arrSensor[i]} 의 전원을 컨트롤 하기 위한 버튼입니다.");

                if (arrSensor[i].Contains("Brightness"))
                {
                    devicePanel[i].DeviceInfoChange($"{arrSensor[i]}의\n전원 및 밝기 제어를 하기 위한 버튼입니다. ");
                    devicePanel[i].Visible_LEDAdjust();
                }

                else if (arrSensor[i].Contains("Temp"))
                {
                    devicePanel[i].DeviceInfoChange($"{arrSensor[i]} 가 측정하고 있는 온도입니다.");
                }

                if (arrStatus[i] == "1")
                {
                    if (arrType[i].ToLower().Contains("led"))
                    {
                        arrSensor[i] = "led";
                    }
                    string iconImagePath = arrSensor[i].ToLower() + "_on";
                    devicePanel[i].DeviceIconChange(iconImagePath);
                    devicePanel[i].DeviceButtonTextChange("끄기");
                }
                else if (arrStatus[i] == "0")
                {
                    if (arrType[i].ToLower().Contains("led"))
                    {
                        arrSensor[i] = "led";
                    }
                    string iconImagePath = arrSensor[i].ToLower() + "_off";
                    devicePanel[i].DeviceIconChange(iconImagePath);
                    devicePanel[i].DeviceButtonTextChange("켜기");
                }
                else
                {
                    _ = MessageBox.Show("Image Change Error");
                }
            }
        }

        private void MainWindows_Load(object sender, RoutedEventArgs e)
        {
            LoadPanel();
            UpdateHidDevices(false);
        }

        private void Menu_Refresh_Click(object sender, RoutedEventArgs e)
        {
            devicePanel.Clear();
            this.MainStackPanel.Children.Clear();

            Add_DevicePanel();
            LoadPanel();
            UpdateHidDevices(false);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var db = new DB_Module();

            const string sql = "INSERT INTO sensor_status(sensor, status, device_type, led_value, last_use) VALUES(@sensor, '0', @device_type, '0', now())";

            try
            {
                db.Execute(sql, new[] { "@sensor", "@device_type" },
                    new[] { ComboDeviceName.Text, ComboDeviceType.Text });
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            MessageBox.Show("추가 완료?", "테스트");
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var db = new DB_Module();

            const string sql = "DELETE FROM sensor_status WHERE sensor=@sensor";

            try
            {
                db.Execute(sql, new[] { "@sensor", "@device_type" },
                    new[] { RemoveComboDeviceName.Text, RemoveComboDeviceName.Text });
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            MessageBox.Show("삭제 완료?", "테스트");
        }
    }
}
