using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace G_One.Module
{
    class DeviceControl
    {

        DB_Module db = new DB_Module();
        MQTT_Module mqtt = new MQTT_Module();

        List<DevicePanel> devicePanel = MainWindow.devicePanel;

        public readonly List<string> listSensor = new List<string>();
        public readonly List<string> listStatus = new List<string>();
        public readonly List<string> listType = new List<string>();

        public void LedValueChange(string name, string topic, string value)
        {
            try
            {
                string sql = "UPDATE sensor_status SET led_value = @led_value, last_use = now() WHERE sensor = @sensorName";
                db.Execute(sql, new[] { "@led_value", "@sensorName" }, new[] { value, name });
                mqtt.Publish(topic, value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("LedValueChange 쪽 에러 : " + ex.Message);
            }
        }

        public void StatusChange(int status, string name, string topic)
        {
            try
            {
                string sql = "UPDATE sensor_status SET status = @sensorStatus, last_use = now() WHERE sensor = @sensorName";
                db.Execute(sql, new[] { "@sensorStatus", "@sensorName" }, new[] { status.ToString(), name });
                mqtt.Publish(topic, status.ToString());
                IconChange(status, name);
            }
            catch (Exception ex)
            {
                MessageBox.Show("StatusChange 쪽 에러 : " + ex.Message);
            }
        }

        public void IconChange(int id, string name)
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
        }

    }
}