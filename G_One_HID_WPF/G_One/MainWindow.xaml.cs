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
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        /*----------------------------------------------------------------------------------------------------------------------------*/
        /* QMK 키보드 연결을 위한 Library 설정 */
        private List<HidDevice> _devices = new List<HidDevice>();

        /* QMK 펌웨어에서 사용되는 USB HID 콘솔 사용량과 페이지의 값 지정 */
        private const ushort ConsoleUsagePage = 0xFF31;
        private const int ConsoleUsage = 0x0074;
        
        /* 키보드로 LED 밝기 제어 를 위해 전역 변수 설정 */
        int ledValue = 0;
        
        /* QMK 펌웨어에서 사용되는 특정 값을 이용하여 QMK 키보드들만 불러오는 코드 */
        private static IEnumerable<HidDevice> GetListableDevices() =>
            HidDevices.Enumerate()
                .Where(d => d.IsConnected)
                .Where(device => device.Capabilities.InputReportByteLength > 0)
                .Where(device => (ushort)device.Capabilities.UsagePage == ConsoleUsagePage)
                .Where(device => (ushort)device.Capabilities.Usage == ConsoleUsage);

        
        /// <summary>
        ///  QMK 키보드가 연결 될 때 사용되는 메서드입니다.
        /// 프로그램 시작 시 disconnected 값을 false(0) 으로 두어야 이 함수가 작동합니다.
        /// </summary>
        /// <param name="disconnected">true(1) or false(0) 값을 입력하여 디바이스 리스트를 업데이트합니다.</param>
        private void UpdateHidDevices(bool disconnected)
        {
            /* QMK 기기 리스트를 변수에 저장 */
            var devices = GetListableDevices().ToList();

            /* 기기 연결 시 작동 */
            if (!disconnected)
            {
                /* devices 안에 데이터가 있을 경우 작동 */
                foreach (var device in devices)
                {
                    /* QMK 키보드의 위치(연결된 USB 포트) 저장 */
                    var deviceExists = _devices.Aggregate(false, (current, dev) => current | dev.DevicePath.Equals(device.DevicePath));

                    /* QMK 키보드가 존재하지 않거나 작동하지 않을결우 반복문 탈출 */
                    if (device == null || deviceExists) continue;

                    /* HID 기기 추가 및 기기와 프로그램을 연결 */
                    _devices.Add(device);
                    device.OpenDevice();

                    /* HID 기기의 이벤트를 모니터링하도록 설정 */
                    device.MonitorDeviceEvents = true;
                    
                    /* 키보드 연결 시 Log 창에 키보드 제작자 이름과 VendorID, ProductID 를 출력 */
                    string text = $"G.One 키보드 연결 됨 : {GetManufacturerString(device)} {GetProductString(device)} ({device.Attributes.VendorId:X4}:{device.Attributes.ProductId:X4}:{device.Attributes.Version:X4})\n";
                    AppendText(text);

                    /* 기기 삽입, 제거 이벤트 핸들러 설정 */
                    device.Inserted += DeviceAttachedHandler;
                    device.Removed += DeviceRemovedHandler;

                    /* 기기의 HID Report를 읽어오고 기기 연결을 종료 */
                    device.ReadReport(OnReport);
                    device.CloseDevice();
                }
            }
            /* 기기 연결 외 상황에서 작동 */
            else
            {
                /* 기기가 존재 할 경우 작동 */
                foreach (var existingDevice in _devices)
                {
                    /* 기기가 연결된 위치 저장 */
                    var deviceExists = devices.Aggregate(false, (current, device) => current | existingDevice.DevicePath.Equals(device.DevicePath));
                    
                    /* QMK 키보드가 존재하지 않을 경우 작동(연결 해제시에도 작동) */
                    if (!deviceExists)
                    {
                        /* Log 창에 문자 출력 */
                        AppendText($"G.One 키보드 연결 해제 됨 : ({existingDevice.Attributes.VendorId:X4}:{existingDevice.Attributes.ProductId:X4}:{existingDevice.Attributes.Version:X4})\n");
                    }
                }
            }
            
            /* HID Library 에 QMK 디바이스 리스트 지정 */
            _devices = devices;
        }

        /// <summary>
        /// 기기 연결 시 작동하는 메서드 입니다.
        /// </summary>
        private void DeviceAttachedHandler()
        {
            UpdateHidDevices(false);
        }

        /// <summary>
        /// 기기 연결 해제 시 작동하는 메서드 입니다.
        /// </summary>
        private void DeviceRemovedHandler()
        {
            UpdateHidDevices(true);
        }

        /// <summary>
        /// HID 기기에서 HID Report 가 전송 되었을 때 사용하는 메서드 입니다.
        /// </summary>
        /// <param name="report">HID Raw Data</param>
        private void OnReport(HidReport report)
        {
            /* data 에 리포트 데이터를 저장 */
            var data = report.Data;
    
            /* 리포트 데이터의 길이가 0이 아니거나 null값이 아닐때 작동 */
            if (0 < data.Length || report.Data != null)
            {
                /* 리포트 데이터를 받아와 문자열로 인코딩 후 Log 출력 및 기기 제어 메서드에 값을 전송  */
                var outputString = Encoding.UTF8.GetString(data).Trim('\0');
                if (outputString == string.Empty) UpdateHidDevices(true);
                else
                {
                    AppendText(outputString);
                    HID_Status_ChangeAsync(outputString.TrimEnd('\n'));
                }
            }
            /* 예외처리 */
            else
            {
                MessageBox.Show(@"에러!");
            }
            
            /* 기기가 있을 때 작동 */
            foreach (var device in _devices)
            {
                /* 기기의 HID 데이터를 읽어들임 */
                device.ReadReport(OnReport);
            }
        }

        /// <summary>
        /// 기기의 이름을 불러오는 메서드입니다.
        /// </summary>
        /// <param name="d">기기의 인터페이스 값</param>
        /// <returns>기기의 제품명</returns>
        private static string GetProductString(IHidDevice d)
        {
            if (d == null) return "";
            d.ReadProduct(out var bs);
            return Encoding.Default.GetString(bs.Where(b => b > 0).ToArray());
        }
        
        /// <summary>
        /// 기기의 제작자 이름을 불러오는 메서드 입니다.
        /// </summary>
        /// <param name="d">기기의 인터페이스 값</param>
        /// <returns>기기의 제작자 명</returns>
        private static string GetManufacturerString(IHidDevice d)
        {
            if (d == null) return "";
            d.ReadManufacturer(out var bs);
            return Encoding.Default.GetString(bs.Where(b => b > 0).ToArray());
        }

        /// <summary>
        /// HID 데이터를 바탕으로 기기의 상태를 변경하는 메서드입니다.
        /// </summary>
        /// <param name="HID_Data">QMK 키보드(G.One 키보드)에서 PC에 보내는 HID 값</param>
        public void HID_Status_ChangeAsync(string HID_Data)
        {
            /* 모듈 사용 준비 */
            var db = new DB_Module();
            var mqtt = new MQTT_Module();
            
            /* 예외 처리 */
            try
            {
                /* 사용 할 변수들 미리 초기화 및 DB 값 불러오기  */
                const string sql = "SELECT * FROM sensor_status";
                var table = db.TableLoad(sql);
                string sensor;
                string status;
                
                /* DB table 값을 읽어 올 수 있을 경우 읽을 값이 없을때까지 작동 */
                while (table.Read())
                {
                    /* table 에 있는 값을 변수에 저장 */
                    sensor = table["sensor"].ToString();
                    status = table["status"].ToString();
                    
                    /* 구버전 펌웨어를 위해 둔 코드(Multi 란 값을 Power_Strip 으로 변경해줌) */
                    if (HID_Data == "MULTI") HID_Data = HID_Data.Replace("MULTI", "Power_Strip");
                    
                    /* HID 값과 sensor 이름을 비교하여 조건에 맞는 기기를 제어하는 함수 */
                    if (HID_Data.ToLower() == sensor.ToLower())
                    {
                        var deviceControl = new DeviceControl();

                        var topic = "iot/" + HID_Data;

                        if (status == "0") status = "1";

                        else if (status == "1") status = "0";

                        string sql2 = "UPDATE sensor_status SET status = @sensorStatus, last_use = now() WHERE sensor = @sensorName";
                        db.Execute(sql2, new[] { "@sensorStatus", "@sensorName" }, new[] { status.ToString(), HID_Data });
                        Console.WriteLine(topic);
                        Console.WriteLine(status);
                        mqtt.Publish(topic, status.ToString());

                        HID_Data = String.Empty;
                    }
                    
                    /* LED 밝기 제어 함수 */
                    if (HID_Data == "LED_UP")
                    {
                        var deviceControl = new DeviceControl();


                        var topic = "iot/Brightness_control_LED";
                        var adjustTopic = "iot/LEDAdjust";

                        Console.WriteLine(topic);

                        string sql3 = "UPDATE sensor_status SET status = @sensorStatus, last_use = now() WHERE sensor = @sensorName";
                        db.Execute(sql3, new[] { "@sensorStatus", "@sensorName" }, new[] { "1", "Brightness_control_LED" });
                        mqtt.Publish(topic, "1");
                        

                        ledValue = ledValue + 25;
                        if (ledValue > 100)
                        {
                            ledValue = 100;
                            break;
                        }

                        mqtt.Publish(adjustTopic, ledValue.ToString());

                        deviceControl.LedValueChange("Brightness_control_LED", "iot/LEDAdjust", ledValue.ToString());
                        HID_Data = String.Empty;
                    }

                    else if (HID_Data == "LED_DOWN")
                    {
                        var deviceControl = new DeviceControl();
                        var adjustTopic = "iot/LEDAdjust";
                        ledValue = ledValue - 25;
                        if (ledValue <= 0)
                        {
                            ledValue = 0;

                            var topic = "iot/Brightness_control_LED";

                            string sql3 = "UPDATE sensor_status SET status = @sensorStatus, last_use = now() WHERE sensor = @sensorName";
                            db.Execute(sql3, new[] { "@sensorStatus", "@sensorName" }, new[] { "0", "Brightness_control_LED" });
                            mqtt.Publish(topic, "0");
                            break;
                        }
                        mqtt.Publish(adjustTopic, ledValue.ToString());
                        deviceControl.LedValueChange("Brightness_control_LED", "iot/LEDAdjust", ledValue.ToString());

                        HID_Data = String.Empty;
                    }
                    /* LED 밝기 제어 함수 종료 */
                }
            }
            /* 에러가 난다면 에러코드 출력 */
            catch (Exception ex)
            {
                MessageBox.Show("HID_Stat_Change Error : " + ex.Message);
            }
        }

        /// <summary>
        /// log 창에 텍스트를 출력하는 메서드입니다.
        /// </summary>
        /// <param name="text">string 형식의 텍스트가 필요합니다.</param>
        private void AppendText(string text)
        {
            /* 이 메서드를 사용하고 있는 쓰레드가 있을경우 그 쓰레드를 종료한 뒤 새로운 작업을 하는 코드 */
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

        /*-------------------------------------------------HID 기기 관련 코드부분 종료--------------------------------------------------------------*/

        /* 프로그램에서 사용되는 리스트(배열) 선언 */
        public static readonly List<DevicePanel> devicePanel = new List<DevicePanel>();

        private static readonly List<string> listSensor = new List<string>();
        private static readonly List<string> listStatus = new List<string>();
        private static readonly List<string> listType = new List<string>();

        private readonly ObservableCollection<AddComboBox> _addComboBox = new ObservableCollection<AddComboBox>();
        private readonly ObservableCollection<TypeComboBox> _typeComboBox = new ObservableCollection<TypeComboBox>();
        private readonly ObservableCollection<RemoveComboBox> _removeComboBox = new ObservableCollection<RemoveComboBox>();
        
        /* 윈도우 프로그램 UI가 뜨기 전 작동하는 메서드입니다. */
        public MainWindow()
        {
            InitializeComponent();
            
            Add_DevicePanel();

            ComboDataLoad();
        }

        /// <summary>
        /// 메인 윈도우에 디바이스 패널을 추가하는 메서드입니다.
        /// </summary>
        public void Add_DevicePanel()
        {
            DB_Load();

            /* DB 에서 받아온 데이터를 토대로 매인 윈도우에 디바이스 패널을 추가하는 반복문입니다. */
            for (var i = 0; i < listSensor.Count; i++)
            {
                var _devicePanel = new DevicePanel(this);
                
                devicePanel.Add(_devicePanel);
                MainStackPanel.Children.Add(_devicePanel);
            }
        }

        /// <summary>
        /// DB 서버에 접근하여 SQL 문장에 따라 데이터를 불러오는 메서드입니다.
        /// </summary>
        private void DB_Load()
        {
            /* DB 데이터를 저장한 LIST 를 초기화 합니다. */
            listSensor.Clear();
            listStatus.Clear();
            listType.Clear();
            
            /* 콤보박스를 초기화합니다 */
            _removeComboBox.Clear();
            
            /* DB 모듈 사용 선언 */
            DB_Module db = new DB_Module();
            
            /* DB에 사용할 SQL을 지정합니다. */
            const string sql = "SELECT * FROM sensor_status";

            /* 예외 처리 */
            try
            {
                /* 테이블 로드 */
                var table = db.TableLoad(sql);
                
                /* 테이블 데이터 있을 때 반복 */
                while (table.Read())
                {
                    /* 데이터를 배열(리스트)에 저장 */
                    listSensor.Add(table["sensor"].ToString());
                    listStatus.Add(table["status"].ToString());
                    listType.Add(table["device_type"].ToString());
                    
                    /* 콤보박스에 데이터 입력 */
                    _removeComboBox.Add(new RemoveComboBox { RemoveSensorName = table["sensor"].ToString() });
                }

            }
            /* 오류 시 메세지 출력 */
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }
            
            /* 콤보박스 데이터 바인딩 */
            RemoveComboDeviceName.ItemsSource = _removeComboBox;
        }

        /// <summary>
        /// 콤보박스 데이터 로드하는 메서드 입니다.
        /// </summary>
        private void ComboDataLoad()
        {
            /* 콤보박스 데이터 초기화 */
            _addComboBox.Clear();
            _typeComboBox.Clear();
            /* 예외 처리 */
            try
            {
                /* DB연결 */
                DB_Module db = new DB_Module();
                string sql = "SELECT * FROM compatible_device";
                var table = db.TableLoad(sql);
    
                /* DB 데이터 콤보박스에 추가 */
                while (table.Read())
                {
                    _addComboBox.Add(new AddComboBox { SensorName = table["name"].ToString() });
                    _typeComboBox.Add(new TypeComboBox { SensorType = table["device_type"].ToString() });
                }
            }
            /* 에러시 작동 */
            catch (Exception ex)
            {
                Console.WriteLine("Error Add_Device_Page : " + ex.Message);
            }

            /* 콤보박스에 데이터 추가 */
            ComboDeviceName.ItemsSource = _addComboBox;
            ComboDeviceType.ItemsSource = _typeComboBox;
        }

        /// <summary>
        /// 디바이스 패널 메인 화면에 출력하는 메서드
        /// </summary>
        public void LoadPanel()
        {
            /* 리스트를 스트링 배열로 변환 */
            string[] arrSensor = listSensor.ToArray();
            string[] arrStatus = listStatus.ToArray();
            string[] arrType = listType.ToArray();
                
            /* 디바이스 패널 이름 및 설명 변경하는 함수 */
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

        /// <summary>
        /// 메인 윈도우가 로드 되었을 때 실행되는 메서드
        /// </summary>
        private void MainWindows_Load(object sender, RoutedEventArgs e)
        {
            /* 패널 로드와 HID 디바이스 연결 시도 */
            LoadPanel();
            UpdateHidDevices(false);
        }

        /// <summary>
        /// 메뉴에서 새로고침 버튼을 눌렀을 때 실행되는 메서드
        /// </summary>
        private void Menu_Refresh_Click(object sender, RoutedEventArgs e)
        {
            devicePanel.Clear();
            this.MainStackPanel.Children.Clear();

            Add_DevicePanel();
            LoadPanel();
            UpdateHidDevices(false);
        }

        /// <summary>
        /// 메뉴에서 종료 버튼을 눌렀을 때 실행되는 메서드
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 기기 추가 버튼을 눌렀을 때 실행되는 메서드
        /// </summary>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            /* DB 연결 */
            var db = new DB_Module();

            const string sql = "INSERT INTO sensor_status(sensor, status, device_type, led_value, last_use) VALUES(@sensor, '0', @device_type, '0', now())";

            /* 예외처리 */
            try
            {
                db.Execute(sql, new[] { "@sensor", "@device_type" },
                    new[] { ComboDeviceName.Text, ComboDeviceType.Text });
            }
            /* 에러시 실행 */
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            /* 메세지 출력 */
            MessageBox.Show("추가 완료", "테스트");
        }

        /// <summary>
        /// 기기 삭제 버튼을 눌렀을 때 실행되는 메서드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            /* DB 연결 */
            var db = new DB_Module();

            const string sql = "DELETE FROM sensor_status WHERE sensor=@sensor";

            /* 예외 처리 */
            try
            {
                db.Execute(sql, new[] { "@sensor", "@device_type" },
                    new[] { RemoveComboDeviceName.Text, RemoveComboDeviceName.Text });
            }
            /* 에러 시 실행 */
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            /* 메세지 출력 */
            MessageBox.Show("삭제 완료", "테스트");
        }
    }
}
