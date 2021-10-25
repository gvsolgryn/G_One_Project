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

    public partial class MainWindow : Window
    {
        public static List<DevicePanel> devicePanel = new List<DevicePanel>();

        public static readonly List<string> listSensor = new List<string>();
        public static readonly List<string> listStatus = new List<string>();
        public static readonly List<string> listType = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            Add_DevicePanel();
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

        private static void DB_Load()
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
                }

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }

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
                devicePanel[i].DeviceInfoChange($"{arrSensor[i]} 의 전원 및 부가기능을 컨트롤 하기 위한 버튼입니다.");

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
        }

        private void Menu_Refresh_Click(object sender, RoutedEventArgs e)
        {
            devicePanel.Clear();
            this.MainStackPanel.Children.Clear();

            Add_DevicePanel();
        }
    }
}
