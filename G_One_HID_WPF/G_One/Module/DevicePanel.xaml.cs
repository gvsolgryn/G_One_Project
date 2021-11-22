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
        string topic = string.Empty;

        public readonly MainWindow _parent;
        readonly DeviceControl deviceControl = new DeviceControl();

        public DevicePanel(MainWindow parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        public void TopicChange(string text)
        {
            topic = "iot/" + text;
        }

        public void Visible_LEDAdjust()
        {
            DeviceLedValueSliderGrid.Visibility = Visibility.Visible;
            DeviceLedValueChangeGrid.Visibility = Visibility.Visible;
        }

        private void DevicePanelLoaded(object sender, RoutedEventArgs e)
        {
            _parent.LoadPanel();
        }

        public void Update()
        {
            _parent.LoadPanel();
        }

        public void DeviceNameChange(string text)
        {
            DeviceName.Content = text;
        }

        public void DeviceInfoChange(string text)
        {
            DeviceInfo.Text = text;
        }

        public void DeviceIconChange(string text)
        {
            
            DeviceIcon.Source = new BitmapImage(new Uri($"/image/{text}.png", UriKind.RelativeOrAbsolute)); ;
        }

        public void DeviceButtonTextChange(string text)
        {
            Accept.Content = text;
            Accept.Name = text;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
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

        private void LEDValueChange_Click(object sender, RoutedEventArgs e)
        {
            string ledValue = LEDValueSlider.Value.ToString();
            deviceControl.LedValueChange(DeviceName.Content.ToString(), "iot/LEDAdjust", ledValue);
        }
    }
}
