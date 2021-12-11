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
        /* 메인페이지 에서 사용할 전역 변수들 초기화 */
        private static readonly List<DevicePanel> DevicePanel = new List<DevicePanel>();
        private static readonly List<string> ListSensor = new List<string>();
        private static readonly List<string> ListStatus = new List<string>();
        private static readonly List<string> ListType = new List<string>();

        /// <summary>
        /// 메인 페이지의 UI 가 로딩 되며 시작되는 메서드 (Main 메서드와 같음)
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            StartAlert();
            
            MqttModule.MqttConnect();
        }

        /// <summary>
        /// 어플 시작 시 팝업 알림을 나타나게 해주는 메서드
        /// </summary>
        private async void StartAlert()
        {
            var answer = await DisplayAlert("주의사항", "이 앱은 인터넷 환경에서만 작동이 됩니다.\n인터넷이 연결 되어있는지 확인 후 실행해주세요.", "확인", "앱 종료");

            /* 선택 값이 취소면 앱을 종료 */
            if (answer == false)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
            else if (true)
            {
                LoadDevice();
            }
        }

        /// <summary>
        /// DB 데이터를 받아오며 전에 있던 데이터들을 초기화하는 메서드
        /// </summary>
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

        /// <summary>
        /// 화면에 기기 목록을 나타나게 해주는 메서드
        /// </summary>
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

        /// <summary>
        /// 목록을 초기화 하며, 디바이스 패널의 이름 및 설명 등 을 수정하는 메서드
        /// </summary>
        private void LoadPanel()
        {
            for(var i = 0; i < ListSensor.Count(); i++)
            {
                DevicePanel[i].DeviceNameChange(ListSensor[i]);
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

        /// <summary>
        /// 새로고침 시 사용되는 메서드
        /// </summary>
        private async void Refresh_Refreshing(object sender, EventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            LoadDevice();

            await Task.Delay(1000);
            
            Refresh.IsRefreshing = false;
        }
        
        /// <summary>
        /// 기기 추가 버튼 클릭 이벤트 메서드
        /// </summary>
        private async void AddDevice_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddDevicePage());
            
            LoadDevice();
        }

        /// <summary>
        /// 기기 삭제 버튼 클릭 이벤트 메서드
        /// </summary>
        private async void RemoveDevice_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RemoveDevicePage());
            
            LoadDevice();
        }
    }
}

