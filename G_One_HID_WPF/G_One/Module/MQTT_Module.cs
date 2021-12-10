using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_One.Module
{
    using uPLibrary.Networking.M2Mqtt;
    using uPLibrary.Networking.M2Mqtt.Messages;

    class MQTT_Module
    {
        /// <summary>
        /// MQTT 서버 연결 메서드
        /// </summary>
        /// <returns>MQTT 서버에 연결 된 클라이언트 값</returns>
        public MqttClient Connect()
        {
            MqttClient client = new MqttClient("gone.gvsolgryn.de");

            _ = client.Connect("G_One_HID", "ID", "PW");

            client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(Client_MqttMsgPublishReceived);

            return client;
        }

        /// <summary>
        /// MQTT 서버에 구독 된 토픽에 메세지가 입력되면
        /// 작동하는 메서드 (테스트용이라 사용 하지 않음)
        /// </summary>
        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(Encoding.UTF8.GetString(e.Message));
        }

        /// <summary>
        /// MQTT 서버에 값을 전송하는 메서드
        /// </summary>
        /// <param name="topic">토픽 값</param>
        /// <param name="status">기기 상태 값</param>
        public void Publish(string topic, string status)
        {
            MqttClient client = Connect();

            _ = client.Publish(topic, Encoding.UTF8.GetBytes(status));
        }

        /// <summary>
        /// MQTT 서버 특정 토픽 구독 메서드 (현재 사용 안함)
        /// </summary>
        /// <param name="topic">토픽 값</param>
        public void Subscribe(string[] topic)
        {
            MqttClient client = Connect();

            _ = client.Subscribe(topic, Encoding.UTF8.GetBytes("0"));
        }
    }
}
