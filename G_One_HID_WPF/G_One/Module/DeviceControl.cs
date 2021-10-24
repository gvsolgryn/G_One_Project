using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace G_One.Module
{
    internal class DeviceControl
    {
        readonly static DB_Module db = new DB_Module();
        readonly static MQTT_Module mqtt = new MQTT_Module();

        public static void StatusChange(int status, string name, string topic)
        {
            MessageBox.Show(status + name + topic);
            try
            {
                string sql = "UPDATE sensor_status SET status = @sensorStatus, last_use = now() WHERE sensor = @sensorName";
                db.Execute(sql, new[] { "@sensorStatus", "@sensorName" }, new[] { status.ToString(), name });
                mqtt.Publish(topic, status.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void IconChange(int id, string name)
        {
            if (name.ToLower().Contains("led"))
            {
                name = "led";
            }

            if (id == 1)
            {
                string iconImagePath = name.ToLower() + "_on";
                //MainWindow.devicePanal.DeviceIconChange(iconImagePath);

            }
            else if (id == 0)
            {
                string iconImagePath = name.ToLower() + "_off";
                //MainWindow.devicePanal.DeviceIconChange(iconImagePath);
            }
        }

    }
}
