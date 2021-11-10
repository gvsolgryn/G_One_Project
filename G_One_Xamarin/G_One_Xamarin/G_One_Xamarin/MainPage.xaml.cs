using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using G_One.Module;
using G_One_Xamarin.module;

namespace G_One_Xamarin
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Add_DevicePanel();
        }

        public static List<DevicePanel> devicePanel = new List<DevicePanel>();

        public static readonly List<string> listSensor = new List<string>();
        public static readonly List<string> listStatus = new List<string>();
        public static readonly List<string> listType = new List<string>();

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
                }

            }
            catch (Exception ex)
            {
                //_ = MessageBox.Show(ex.Message);
                Alert("Error", ex.Message, "확인");
            }

        }

        async void Alert(string a, string b, string c)
        {
            await DisplayAlert(a, b, c);
        }

        public void LoadPanel()
        {
            /*
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
            */
        }

    }
}
