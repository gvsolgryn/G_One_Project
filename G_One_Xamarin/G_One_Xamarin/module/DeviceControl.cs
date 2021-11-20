using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Xamarin.Forms;
using M2Mqtt;

namespace G_One_Xamarin.module
{
    internal class DeviceControl
    {
        private static readonly DbModule Db = new DbModule();

        public static void StatusChange(int status, string name, string topic)
        {
            var client = new MqttClient("gone.gvsolgryn.de");
            
            try
            {
                client.Connect("G_One_Xamarin", "g_one", "g_one");
                
                const string sql = "UPDATE sensor_status SET status = @sensorStatus, last_use = now() WHERE sensor = @sensorName";
                
                Db.Execute(sql, new[] { "@sensorStatus", "@sensorName" }, new[] { status.ToString(), name });
                
                client.Publish(topic, Encoding.UTF8.GetBytes(status.ToString()));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }

            
        }

        public static async void LedValueChange(int value, string name, string topic)
        {
            var client = new MqttClient("gone.gvsolgryn.de");

            try
            {
                client.Connect("G_One_Xamarin", "g_one", "g_one");
                
                const string sql = "UPDATE sensor_status SET led_value = @led_value, last_use = now() WHERE sensor = @sensorName";
                
                Db.Execute(sql, new[] { "@led_value", "@sensorName" }, new[] { value.ToString(), name });
                
                client.Publish(topic, Encoding.UTF8.GetBytes(value.ToString()));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "OK");
            }
        }
    }
}