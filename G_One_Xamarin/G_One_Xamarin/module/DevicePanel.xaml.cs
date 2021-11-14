using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace G_One_Xamarin.module
{
    public partial class DevicePanel : ContentView
    {
        public readonly MainPage _mainPage;
        private readonly DeviceControl deviceControl = new DeviceControl();
        private string topic = string.Empty;

        public DevicePanel(MainPage mainPage)
        {
            InitializeComponent();
            _mainPage = mainPage;
        }

        /* 디바이스 이름 변경(지정) */

        public void DeviceNameChange(string text)
        {
            DeviceName.Text = text;
        }

        /* 디바이스 아이콘 변경(지정) */

        public void DeviceIconchange(string text)
        {
            DeviceIcon.Source = ImageSource.FromResource(text);
        }

        /* 디바이스 전원 버튼 이름 변경(지정) */

        public void DeviceButtonTextChange(string text)
        {
            ChangeDevicePower.Text = text;
        }

        /* 특정 기기에서만 밝기 제어 슬라이더가 나오게 하는 메소드 */

        public void Visible_LEDAdjust()
        {
            AdjustGrid.IsVisible = !AdjustGrid.IsVisible;
        }

        /* 밝기 제어 슬라이더가 필요없는 패널 사이즈 조절 테스트 메소드 */

        public void Grid_Adjust()
        {
            ControlGrid.RowDefinitions.RemoveAt(1); // AdjustGrid 의 부모 그리드를 이용해서 AdjustGrid 제거

            Grid.SetRow(ButtonGrid, 1); // Grid.SetRow 를 이용하여 ButtonGrid 의 Row 순번 변경
            
        }

        /* MQTT 통신용 Topic 만들기 */

        public void TopicChange(string text)
        {
            topic = "iot/" + text;
        }

        /* 버튼 클릭 이벤트 */

        private void ButtonClicked(object sender, EventArgs e)
        {
            if(ChangeDevicePower.Text == "끄기")
            {
                try
                {
                    deviceControl.StatusChange(1, DeviceName.Text.ToString(), topic);
                }

                catch (Exception ex)
                {
                    Application.Current.MainPage.DisplayAlert("DevicePanel Btn Error", "에러 내용 : " + ex.Message, "확인");
                }

                string imageSource = string.Empty;

                if (DeviceName.Text.ToString().ToLower().Contains("led"))
                {
                    imageSource = "G_One_Xamarin.image.led_off.png";
                }
                else
                {
                    imageSource = "G_One_Xamarin.image.power_strip_off.png";
                }
                DeviceIconchange(imageSource);

                DeviceButtonTextChange("켜기");
            }

            else if(ChangeDevicePower.Text == "켜기")
            {
                try
                {
                    deviceControl.StatusChange(0, DeviceName.Text.ToString(), topic);
                }

                catch (Exception ex)
                {
                    Application.Current.MainPage.DisplayAlert("DevicePanel Btn Error", "에러 내용 : " + ex.Message, "확인");
                }

                string imageSource = string.Empty;

                if (DeviceName.Text.ToString().ToLower().Contains("led"))
                {
                    imageSource = "G_One_Xamarin.image.led_on.png";
                }
                else
                {
                    imageSource = "G_One_Xamarin.image.power_strip_on.png";
                }
                DeviceIconchange(imageSource);

                DeviceButtonTextChange("끄기");
            }

            else
            {
                Application.Current.MainPage.DisplayAlert("DevicePanel Error", "어플리케이션 에러가 발견되었습니다.\n관리자에게 문의해주세요.", "확인");
            }
        }

    }
}

