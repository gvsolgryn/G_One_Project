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
        public DevicePanel()
        {
            InitializeComponent();
        }

        private void DevicePanelLoaded(object sender, RoutedEventArgs e)
        {
        }

        public void DeviceNameChange(string text)
        {
            DeviceName.Content = text;
        }

        public void DeviceInfoChange(string text)
        {
            DeviceInfo.Content = text;
        }

        public void DeviceIconChange(string text)
        {
            DeviceIcon.Source = new BitmapImage(new Uri($"/image/{text}.png", UriKind.RelativeOrAbsolute)); ;
        }
    }
}
