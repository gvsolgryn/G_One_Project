using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using G_One_Xamarin.module;
using G_One_Xamarin.page;

namespace G_One_Xamarin
{
    public partial class MainPage : ContentPage
    {
        private static readonly List<DevicePanel> DevicePanel = new List<DevicePanel>();
        private static readonly List<string> ListSensor = new List<string>();
        private static readonly List<string> ListStatus = new List<string>();
        private static readonly List<string> ListType = new List<string>();

        public MainPage()
        {
            InitializeComponent();

            StartAlert();
            
            MqttModule.MqttConnect();
        }

        private async void StartAlert()
        {
            var answer = await DisplayAlert("주의사항", "이 앱은 인터넷 환경에서만 작동이 됩니다.\n인터넷이 연결 되어있는지 확인 후 실행해주세요.", "확인", "앱 종료");

            if (answer == false)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
            else if (true)
            {
                LoadDevice();
            }
        }

        private void LoadDevice()
        {
            MainStackLayout.Children.Clear();

            var db = new DbModule();

            const string sql = "SELECT * FROM sensor_status";

            try
            {
                DevicePanel.Clear();
                ListSensor.Clear();
                ListStatus.Clear();
                ListType.Clear();
                
                var table = db.TableLoad(sql);

                while (table.Read())
                {
                    ListSensor.Add(table["sensor"].ToString());
                    ListStatus.Add(table["status"].ToString());
                    ListType.Add(table["device_type"].ToString());
                }
                AddDevicePanel();
            }
            catch (Exception ex)
            {
                DisplayAlert("DB Load Error", "에러 내용 : " + ex.Message, "확인");

                DisplayAlert("강제 종료", "앱에서 오류가 발생했습니다. 관리자에게 문의 하세요. 앱은 강제종료 됩니다.", "종료");
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }

        }

        private void AddDevicePanel()
        {
            for (var i = 0; i < ListSensor.Count; i++)
            {
                var devicePanel = new DevicePanel(this);
                DevicePanel.Add(devicePanel);
                MainStackLayout.Children.Add(devicePanel);
            }

            LoadPanel();
        }

        private void LoadPanel()
        {
            for(var i = 0; i < ListSensor.Count(); i++)
            {
                //devicePanel[i].DeviceNameChange(arrSensor[i]);
                DevicePanel[i].DeviceNameChange(ListSensor[i]);
                //devicePanel[i].TopicChange(arrSensor[i]);
                DevicePanel[i].TopicChange(ListSensor[i]);
                

                /* 밝기 제어 가능 LED의 밝기제어 기능 활성화 */
                if (ListSensor[i].Contains("Brightness"))
                {
                    DevicePanel[i].Visible_LEDAdjust();
                }
                else
                {
                    DevicePanel[i].Grid_Adjust();
                }

                switch (ListStatus[i])
                {
                    case "1":
                    {
                        string image;
                        if (ListType[i].ToLower().Contains("led"))
                        {
                            image = "G_One_Xamarin.image.led_on.png";
                        }

                        else
                        {
                            image = "G_One_Xamarin.image.power_strip_on.png";
                        }

                        DevicePanel[i].DeviceIconchange(image);
                        DevicePanel[i].DeviceButtonTextChange("끄기");
                        break;
                    }
                    case "0":
                    {
                        string image;
                        if (ListType[i].ToLower().Contains("led"))
                        {
                            image = "G_One_Xamarin.image.led_off.png";
                        }

                        else
                        {
                            image = "G_One_Xamarin.image.power_strip_off.png";
                        }

                        DevicePanel[i].DeviceIconchange(image);
                        DevicePanel[i].DeviceButtonTextChange("켜기");
                        break;
                    }
                }

            }
        }

        private async void Refresh_Refreshing(object sender, EventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            LoadDevice();

            await Task.Delay(1000);
            
            Refresh.IsRefreshing = false;
        }
        /* 모달 불러오는 메소드 */
        private async void AddDevice_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddDevicePage());
            
            LoadDevice();
        }

        private async void RemoveDevice_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RemoveDevicePage());
            
            LoadDevice();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            MqttPubSubTest.SendMqtt();
            MqttModule.MqttPub("iot/test", "TEST MQTTNET");
        }
    }
}

