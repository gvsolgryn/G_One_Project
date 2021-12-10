using System;
using System.Collections.Generic;
using System.IO;
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

namespace G_One.Module
{
    /// <summary>
    /// DevicePanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DevicePanel : UserControl
    {
        /* 디바이스 패널에서 사용하는 값을 저장하는 변수들 초기화 */
        string topic = string.Empty;

        public readonly MainWindow _parent;
        readonly DeviceControl deviceControl = new DeviceControl();

        public DevicePanel(MainWindow parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        /// <summary>
        /// 토픽 이름을 iot/(기기이름) 으로 변경하는 메서드
        /// (토픽 값은 디바이스패널에 저장됨)
        /// </summary>
        /// <param name="text">기기 이름</param>
        public void TopicChange(string text)
        {
            topic = "iot/" + text;
        }

        /// <summary>
        /// LED Adjust를 보이게 해주는 메서드
        /// 기본상태 : Hidden
        /// </summary>
        public void Visible_LEDAdjust()
        {
            DeviceLedValueSliderGrid.Visibility = Visibility.Visible;
            DeviceLedValueChangeGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 디바이스 패널이 로드 되었을 때 실행되는 메서드
        /// </summary>
        private void DevicePanelLoaded(object sender, RoutedEventArgs e)
        {
            _parent.LoadPanel();
        }

        /// <summary>
        /// 디바이스 패널 내용들 업데이트 하는 메서드
        /// </summary>
        public void Update()
        {
            _parent.LoadPanel();
        }

        /// <summary>
        /// 디바이스 패널에 있는 기기 이름을 변경하는 메서드
        /// </summary>
        /// <param name="text">기기 이름</param>
        public void DeviceNameChange(string text)
        {
            DeviceName.Content = text;
        }

        /// <summary>
        /// 디바이스 패널에 있는 기기 정보를 변경하는 메서드
        /// </summary>
        /// <param name="text">기기 정보</param>
        public void DeviceInfoChange(string text)
        {
            DeviceInfo.Text = text;
        }

        /// <summary>
        /// 디바이스 패널에 있는 기기 아이콘을 변경하는 메서드
        /// </summary>
        /// <param name="text">기기 아이콘 이름</param>
        public void DeviceIconChange(string text)
        {
            
            DeviceIcon.Source = new BitmapImage(new Uri($"/image/{text}.png", UriKind.RelativeOrAbsolute)); ;
        }

        /// <summary>
        /// 디바이스 패널의 기기 상태 변경 버튼의 이름을 변경하는 메서드
        /// </summary>
        /// <param name="text">기기 상태</param>
        public void DeviceButtonTextChange(string text)
        {
            Accept.Content = text;
            Accept.Name = text;
        }

        /// <summary>
        /// 기기 변경 버튼을 눌렀을 때 사용되는 메서드
        /// </summary>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            /* 버튼의 이름으로 끌지 켤지 구분한 뒤 컨트롤에 값을 전송함 */
            if(Accept.Name == "끄기")
            {
                try
                {
                    deviceControl.StatusChange(0, DeviceName.Content.ToString(), topic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else if(Accept.Name == "켜기"){
                try
                {
                    deviceControl.StatusChange(1, DeviceName.Content.ToString(), topic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
                MessageBox.Show("에러");
            }
        }

        /// <summary>
        /// LED 밝기 조절 버튼을 눌렀을 때 사용되는 메서드
        /// </summary>
        private void LEDValueChange_Click(object sender, RoutedEventArgs e)
        {
            string ledValue = LEDValueSlider.Value.ToString();
            deviceControl.LedValueChange(DeviceName.Content.ToString(), "iot/LEDAdjust", ledValue);
        }
    }
}
