#include <IRsend.h>
#include <IRremoteESP8266.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <stdio.h>
#include <string.h>

// Update these with values suitable for your network.
const char* ssid = "iPhone";
const char* password = "11111111";
const char* mqtt_server = "gone.gvsolgryn.de";

WiFiClient espClient;
PubSubClient client(espClient);
unsigned long lastMsg = 0;
#define MSG_BUFFER_SIZE	(50)
char msg[MSG_BUFFER_SIZE];
int value = 0;

const uint16_t kIrLed = D2;
IRsend irsend(kIrLed);

void setup_wifi() {

  delay(10);
  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.mode(WIFI_STA);
  //WiFi.begin(ssid, password);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  randomSeed(micros());

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}

void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");

  char payloadString[length];
  
  for (int i = 0; i < length; i++) {
    Serial.print((char)payload[i]);
    payloadString[i] = (char)payload[i];
  }
  
  Serial.println("");
  char* led = "iot/Brightness_control_LED";
  char* generaLed = "iot/General_LED";
  char* ledAdjust = "iot/LEDAdjust";
  char* powerStrip = "iot/Power_Strip";
  Serial.println("");
  
  // Brightness_control_LED Control
  if (strcmp(topic, led) == 0){
    if (strcmp(topic, led) == 0 && (char)payload[0] == '1'){
      irsend.sendNEC(0xFF807F);
    }
    else if (strcmp(topic, led) == 0 && (char)payload[0] == '0'){
      irsend.sendNEC(0xFF00FF);
    }
  }
  // LED Brightness Adjust
  if (strcmp(topic, ledAdjust) == 0){
    if(strcmp((char *)payloadString, "25") == 0){
      irsend.sendNEC(0xFF50AF);
    }

    else if(strcmp((char *)payloadString, "50") == 0){
      irsend.sendNEC(0xFFA857);
    }

    else if(strcmp((char *)payloadString, "75") == 0){
      irsend.sendNEC(0xFFB04F);
    }

    else if(strcmp((char *)payloadString, "100") == 0){
      irsend.sendNEC(0xFF6897);
    }
  }
  // General LED Control
  if (strcmp(topic, generaLed) == 0){
    if (strcmp(topic, generaLed) == 0 && (char)payload[0] == '1'){
      digitalWrite(D0, 1);
    }
    else if (strcmp(topic, generaLed) == 0 && (char)payload[0] == '0'){
      digitalWrite(D0, 0);
    }
  }
  // Power Strip Control
  if (strcmp(topic, powerStrip) == 0){
    if (strcmp(topic, powerStrip) == 0 && (char)payload[0] == '1'){
      digitalWrite(D1, 1);
    }
    else if (strcmp(topic, powerStrip) == 0 && (char)payload[0] == '0'){
      digitalWrite(D1, 0);
    }
  }
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Create a random client ID
    String clientId = "ESP8266Client-";
    clientId += String(random(0xffff), HEX);
    // Attempt to connect
    if (client.connect("ESP8266Client-", "user", "1234")) {
      Serial.println("connected");
      // Once connected, publish an announcement...
      client.publish("outTopic", "hello world");
      // ... and resubscribe
      client.subscribe("iot/Brightness_control_LED");
      client.subscribe("iot/LEDAdjust");
      client.subscribe("iot/General_LED");
      client.subscribe("iot/Power_Strip");
      client.subscribe("iot/#");

      /*char* led = "iot/Brightness_control_LED";
  char* generaLed = "iot/General_LED";
  char* ledAdjust = "iot/LEDAdjust";
  char* powerStrip = "iot/Power_Strip";*/
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}

void setup() {
  irsend.begin();
  pinMode(D0, OUTPUT);
  pinMode(D1, OUTPUT);
  pinMode(D2, OUTPUT);
  Serial.begin(9600);
  setup_wifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
}

void loop() {

  if (!client.connected()) {
    reconnect();
  }
  client.loop();

  unsigned long now = millis();
  /* if (now - lastMsg > 2000) {
    lastMsg = now;
    ++value;
    snprintf (msg, MSG_BUFFER_SIZE, "hello world #%ld", value);
    Serial.print("Publish message: ");
    Serial.println(msg);
    client.publish("outTopic", msg);
  }*/
}
