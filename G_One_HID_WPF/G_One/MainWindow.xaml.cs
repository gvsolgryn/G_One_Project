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
        public MainWindow()
        {
            InitializeComponent();
        }

        private static List<string> listSensor = new List<string>();
        private static List<string> listStatus = new List<string>();

        private void DB_Load()
        {
            DB_Module db = new DB_Module();

            string sql = "SELECT * FROM sensor_status";

            try
            {
                var table = db.TableLoad(sql);

                while (table.Read())
                {
                    listSensor.Add(table["sensor"].ToString());
                    listStatus.Add(table["status"].ToString());
                }

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindows_Load(sender, e);
        }

        private void MainWindows_Load(object sender, RoutedEventArgs e)
        {
            MainStackPanel.Children.Clear();
            listSensor.Clear();
            DB_Load();

            string[] arrSensor = listSensor.ToArray();
            string[] arrStatus = listStatus.ToArray();

            for (int i = 0; i < arrSensor.Length; i++)
            {
                DevicePanel devicePanal = new DevicePanel();
                devicePanal.DeviceNameChange(arrSensor[i]);
                devicePanal.DeviceInfoChange($"{arrSensor[i]} 의 전원 및 부가기능을 컨트롤 하기 위한 버튼입니다.");

                if (arrStatus[i] == "1")
                {
                    if (arrSensor[i].ToLower().Contains("led"))
                    {
                        arrSensor[i] = "led";
                    }
                    string iconImagePath = arrSensor[i].ToLower() + "_on";
                    devicePanal.DeviceIconChange(iconImagePath);
                    devicePanal.DeviceButtonTextChange("끄기");
                }
                else if (arrStatus[i] == "0")
                {
                    if (arrSensor[i].ToLower().Contains("led"))
                    {
                        arrSensor[i] = "led";
                    }
                    string iconImagePath = arrSensor[i].ToLower() + "_off";
                    devicePanal.DeviceIconChange(iconImagePath);
                    devicePanal.DeviceButtonTextChange("켜기");
                }
                else
                {
                    _ = MessageBox.Show("Image Change Error");
                }

                _ = MainStackPanel.Children.Add(devicePanal);
            }
        }

    }
}
