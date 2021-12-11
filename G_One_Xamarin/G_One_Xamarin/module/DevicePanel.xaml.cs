using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace G_One_Xamarin.module
{
    public partial class DevicePanel : ContentView
    {
        private readonly MainPage _mainPage;
        private readonly DeviceControl _deviceControl = new DeviceControl();
        private string _topic = string.Empty;

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
            _topic = "iot/" + text;
        }

        /* 버튼 클릭 이벤트 */
        
        private void ButtonClicked(object sender, EventArgs e)
        {
            switch (ChangeDevicePower.Text)
            {
                case "끄기":
                {
                    try
                    {
                        DeviceControl.StatusChange(0, DeviceName.Text.ToString(), _topic);
                    }

                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("DevicePanel Btn Error", "에러 내용 : " + ex.Message, "확인");
                    }

                    string imageSource;

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
                    break;
                }
                case "켜기":
                {
                    try
                    {
                        DeviceControl.StatusChange(1, DeviceName.Text.ToString(), _topic);
                    }

                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("DevicePanel Btn Error", "에러 내용 : " + ex.Message, "확인");
                    }

                    string imageSource;

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
                    break;
                }
                default:
                    Application.Current.MainPage.DisplayAlert("DevicePanel Error", "어플리케이션 에러가 발견되었습니다.\n관리자에게 문의해주세요.", "확인");
                    break;
            }
        }

        /* LED 밝기 변경 버튼 클릭 이벤트 */
        private async void ChangeLedValueBtn_OnClicked(object sender, EventArgs e)
        {
            Console.WriteLine(DeviceName);
            if (LedValue.UpperValue > 0)
            {
                if (ChangeDevicePower.Text.ToString() == "켜기")
                {
                    await Application.Current.MainPage.DisplayAlert("밝기 제어", "LED 전원을 먼저 켜주세요.", "확인");
                }
                else
                {
                    DeviceControl.LedValueChange(Convert.ToInt32(LedValue.UpperValue), DeviceName.Text, "iot/LEDAdjust");
                
                    await Application.Current.MainPage.DisplayAlert("밝기 제어", "LED 밝기 변경 완료", "확인");
                }
            }
            
            else if (LedValue.UpperValue <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("밝기 제어", "밝기를 0보다 더 높게 설정하세요.", "확인");
            }

            else
            {
                await Application.Current.MainPage.DisplayAlert("밝기 제어 실패", "밝기 제어 실패", "확인");
            }
        }
    }
}

