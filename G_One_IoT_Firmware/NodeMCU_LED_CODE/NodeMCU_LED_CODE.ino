#include <IRsend.h>
#include <IRremoteESP8266.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <stdio.h>
#include <string.h>

// Update these with values suitable for your network.
const char* ssid = "gvtime";
const char* mqtt_server = "gone.gvsolgryn.de";

WiFiClient espClient;
PubSubClient client(espClient);
unsigned long lastMsg = 0;
#define MSG_BUFFER_SIZE	(50)
char msg[MSG_BUFFER_SIZE];
int value = 0;

const uint16_t kIrLed = 4;
IRsend irsend(kIrLed);

void setup_wifi() {

  delay(10);
  // We start by connecting to a WiFi network
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.mode(WIFI_STA);
  //WiFi.begin(ssid, password);
  WiFi.begin(ssid);

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
  char* led = "iot/LED";
  char* ledAdjust = "iot/LEDAdjust";
  Serial.println("");
  
  if (strcmp(topic, led) == 0){
    if (strcmp(topic, led) == 0 && (char)payload[0] == '1'){
      digitalWrite(BUILTIN_LED, LOW);
      irsend.sendNEC(0xFF807F);
    }
    if (strcmp(topic, led) == 0 && (char)payload[0] == '0'){
      digitalWrite(BUILTIN_LED, HIGH);
      irsend.sendNEC(0xFF00FF);
    }
  }

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
      client.subscribe("iot/LED");
      client.subscribe("iot/LEDAdjust");
      client.subscribe("LEDAdjust");
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
  pinMode(BUILTIN_LED, OUTPUT);     // Initialize the BUILTIN_LED pin as an output
  pinMode(D0, OUTPUT);
  pinMode(D8, OUTPUT);
  Serial.begin(9600);
  setup_wifi();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
  digitalWrite(D0, HIGH);
  digitalWrite(BUILTIN_LED, HIGH);
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
