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
        private static readonly MqttFactory Factory = new MqttFactory();

        private static readonly IMqttClient MqttClient = Factory.CreateMqttClient();
        
        public static string Topic = String.Empty;
        public static string Payload = String.Empty;
        
        public static async void MqttConnect()
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId("G_One_Xamarin_MQTTnet")
                .WithTcpServer("gone.gvsolgryn.de")
                .Build();

            await MqttClient.ConnectAsync(options, CancellationToken.None);
        }

        public static async void MqttPub(string topic, string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithPayload(payload)
                .WithTopic(topic)
                .Build();

            await MqttClient.PublishAsync(message, CancellationToken.None);
        }
        
        public static async void MqttSub(string topic)
        {
            var subOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(topic)
                .Build();

            await MqttClient.SubscribeAsync(subOptions);
        }

        public static void MqttGetPayload()
        {
            MqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Topic = e.ApplicationMessage.Topic.ToString();
                Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            });
        }

        public static async void MqttDisconnect()
        {
            await MqttClient.DisconnectAsync();
        }
    }
}