using System;
using System.Linq;
using System.Text;
using System.Threading;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using MQTTnet.Client.Receiving;

namespace G_One_Xamarin.module
{
    public class MqttPubSubTest
    {
        public static async void SendMqtt()
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("G_One_Xamarin_MQTTnet")
                .WithTcpServer("gone.gvsolgryn.de")
                .Build();

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("iot/TestMQTTnet")
                .WithPayload("MQTTnet Library Test Pub")
                .Build();

            await mqttClient.ConnectAsync(options, CancellationToken.None);

            await mqttClient.PublishAsync(message, CancellationToken.None);

            await mqttClient.DisconnectAsync();
        }
    }
}