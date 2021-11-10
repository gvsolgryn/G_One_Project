using G_One_Xamarin;
using G_One_Xamarin.module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace G_One.Module
{
    class DeviceControl
    {

        readonly static DB_Module db = new DB_Module();
        //readonly static MQTT_Module mqtt = new MQTT_Module();

        static readonly List<DevicePanel> devicePanel = MainPage.devicePanel;

        public static readonly List<string> listSensor = new List<string>();
        public static readonly List<string> listStatus = new List<string>();
        public static readonly List<string> listType = new List<string>();

        public void StatusChange(int status, string name, string topic)
        {
            try
            {
                string sql = "UPDATE sensor_status SET status = @sensorStatus, last_use = now() WHERE sensor = @sensorName";
                db.Execute(sql, new[] { "@sensorStatus", "@sensorName" }, new[] { status.ToString(), name });
                //mqtt.Publish(topic, status.ToString());
                //IconChange(status, name);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        /*public void IconChange(int id, string name)
        {
            int idx = devicePanel.FindIndex(x => x.DeviceName.Content.Equals(name));

            string iconImagePath = String.Empty;

            if (id == 1)
            {

                if (name.ToLower().Contains("led"))
                {
                    name = "led";
                }

                iconImagePath = name.ToLower() + "_on";
                devicePanel[idx].DeviceIconChange(iconImagePath);
                devicePanel[idx].DeviceButtonTextChange("끄기");
            }

            else if (id == 0)
            {
                if (name.ToLower().Contains("led"))
                {
                    name = "led";
                }

                iconImagePath = name.ToLower() + "_off";
                devicePanel[idx].DeviceIconChange(iconImagePath);
                devicePanel[idx].DeviceButtonTextChange("켜기");
            }
        }*/

    }
}