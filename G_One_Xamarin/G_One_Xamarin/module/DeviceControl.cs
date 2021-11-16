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
    class DeviceControl
    {
        private static readonly DbModule Db = new DbModule();

        public static void StatusChange(int status, string name, string topic)
        {
            MqttClient Client = new MqttClient("gone.gvsolgryn.de");
            
            try
            {
                Client.Connect("G_One_Xamarin", "g_one", "g_one");
                
                const string sql = "UPDATE sensor_status SET status = @sensorStatus, last_use = now() WHERE sensor = @sensorName";
                
                Db.Execute(sql, new[] { "@sensorStatus", "@sensorName" }, new[] { status.ToString(), name });
                
                Client.Publish(topic, Encoding.UTF8.GetBytes(status.ToString()));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }

            
        }

        public static void LedValueChange(int value, string name, string topic)
        {
            
        }
    }
}