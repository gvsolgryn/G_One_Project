using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_One.Module
{
    using uPLibrary.Networking.M2Mqtt;
    using uPLibrary.Networking.M2Mqtt.Messages;

    internal class MQTT_Module
    {
        public MqttClient Connect()
        {
            MqttClient client = new MqttClient("gone.gvsolgryn.de");

            _ = client.Connect("G_One_HID", "ID", "PW");

            client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(Client_MqttMsgPublishReceived);

            return client;
        }

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(Encoding.UTF8.GetString(e.Message));
        }


        public void Publish(string topic, string status)
        {
            MqttClient client = Connect();

            _ = client.Publish(topic, Encoding.UTF8.GetBytes(status));
        }

        public void Subscribe(string[] topic)
        {
            MqttClient client = Connect();

            _ = client.Subscribe(topic, Encoding.UTF8.GetBytes("0"));
        }
    }
}
