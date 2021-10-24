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
        static string path = string.Empty;

        public DevicePanel()
        {
            InitializeComponent();
        }

        public void TopicChange(string text)
        {
            topic = "iot/" + text;
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
                    //string sql = $"UPDATE sensor_status SET status = 0, last_use = now() WHERE sensor = '{DeviceName.Content}'";
                    //db.Execute(sql, new[] {string.Empty}, new[] {string.Empty});
                    DeviceControl.StatusChange(0, DeviceName.Content.ToString(), topic);
                    Accept.Name = "켜기";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if(Accept.Name == "켜기"){
                try
                {
                    //string sql = $"UPDATE sensor_status SET status = 1, last_use = now() WHERE sensor = '{DeviceName.Content}'";
                    //db.Execute(sql, new[] { string.Empty }, new[] { string.Empty });
                    DeviceControl.StatusChange(1, DeviceName.Content.ToString(), topic);
                    Accept.Name = "끄기";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
