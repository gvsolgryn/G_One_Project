using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using G_One_Xamarin.module;

namespace G_One_Xamarin
{
    public partial class MainPage : ContentPage
    {
        public static List<DevicePanel> devicePanel = new List<DevicePanel>();
        public static readonly List<string> listSensor = new List<string>();
        public static readonly List<string> listStatus = new List<string>();
        public static readonly List<string> listType = new List<string>();

        public MainPage()
        {
            InitializeComponent();

            StartAlert();
        }

        private async void StartAlert()
        {
            bool answer = await DisplayAlert("주의사항", "이 앱은 인터넷 환경에서만 작동이 됩니다. 인터넷이 연결 되어있는지 확인 후 실행해주세요.", "확인", "앱 종료");

            if (answer == false)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
            else if (answer == true)
            {
                LoadDevice();
            }
        }

        private void LoadDevice()
        {
            MainStackLayout.Children.Clear();

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
            for (int i = 0; i < listSensor.Count; i++)
            {
                var _devicePanel = new DevicePanel(this);
                devicePanel.Add(_devicePanel);
                MainStackLayout.Children.Add(_devicePanel);
            }
        }

    }
}

