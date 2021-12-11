using System;
using System.Linq;
using System.Text;
using System.Threading;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;

namespace G_One_Xamarin.module
{
    public class MqttModule
    {
        /* MQTT 서버 연결 */
        private static readonly MqttFactory Factory = new MqttFactory();

        private static readonly IMqttClient MqttClient = Factory.CreateMqttClient();
        
        public static string Topic = String.Empty;
        public static string Payload = String.Empty;
        
        /// <summary>
        /// MQTT 서버에 연결하는 메서드
        /// </summary>
        public static async void MqttConnect()
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId("G_One_Xamarin_MQTTnet")
                .WithTcpServer("gone.gvsolgryn.de")
                .Build();

            await MqttClient.ConnectAsync(options, CancellationToken.None);
        }

        /// <summary>
        /// MQTT 서버에 데이터를 전송하는 메서드
        /// </summary>
        /// <param name="topic">토픽</param>
        /// <param name="payload">전송 할 메세지</param>
        public static async void MqttPub(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithPayload(payload)
                .WithTopic(topic)
                .Build();

            await MqttClient.PublishAsync(message, CancellationToken.None);
        }

        /// <summary>
        /// MQTT 서버 연결 해제 메서드
        /// </summary>
        public static async void MqttDisconnect()
        {
            await MqttClient.DisconnectAsync();
        }
    }
}