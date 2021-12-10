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
        /* DB,MQTT 모듈 사용 사전 준비 */
        DB_Module db = new DB_Module();
        MQTT_Module mqtt = new MQTT_Module();
        
        /* 리스트 배열에 메인 윈도우에 있는 디바이스 패널 저장 */
        List<DevicePanel> devicePanel = MainWindow.devicePanel;

        /// <summary>
        /// LED 값을 변경할 때 사용되는 메서드
        /// </summary>
        /// <param name="name">기기 이름 값</param>
        /// <param name="topic">MQTT 토픽 이름 값</param>
        /// <param name="value">기기 상태 값</param>
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

        /// <summary>
        /// 기기 상태 변경 메서드
        /// </summary>
        /// <param name="status">기기 상태 값</param>
        /// <param name="name">기기 이름 값</param>
        /// <param name="topic">MQTT 토픽 이름 값</param>
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

        /// <summary>
        /// 디바이스 패널 아이콘 변경 메서드
        /// </summary>
        /// <param name="id">디바이스 패널 인덱스</param>
        /// <param name="name">아이콘 이름</param>
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