#include "SevSeg.h"

SevSeg sevseg;

/*
    1 E         : 10 
    2 D         : 16 
    3 DP        : 14
    4 C         : 15
    5 G         : A0
    6 DIG.4     : A1
    7 B         : 7
    8 DIG.3     : 6
    9 DIG.2     : 5
    10 F        : 4
    11 A        : 3
    12 DIG.1    : 2
    RX          : 1
    TX          : 0
*/



String inputString = "";
bool stringComplete = false;

String serial1_inputString = "";
bool serial1Complete = false;

void setup() {
  // put your setup code here, to run once:
    /*for(int i = 0; i < 4; i++){
        pinMode(digit_select_pin[i], OUTPUT);
        digitalWrite(digit_select_pin[i], HIGH);
    }

    for(int i = 0; i < 8; i++){
        pinMode(segment_pin[i], OUTPUT);
    }*/

    byte numDigits = 4;
    byte digitPins[] = {2, 5, 6, A1};
    byte segmentPins[] = {3, 7, 15, 16, 10, 4, A0, 14};
    bool resistorsOnSegments = true; // 'false' means resistors are on digit pins
    byte hardwareConfig = COMMON_ANODE; // See README.md for options
    bool updateWithDelays = false; // Default 'false' is Recommended
    bool leadingZeros = false; // Use 'true' if you'd like to keep the leading zeros
    bool disableDecPoint = false; // Use 'true' if your decimal point doesn't exist or isn't connected. Then, you only need to specify 7 segmentPins[]

    sevseg.begin(hardwareConfig, numDigits, digitPins, segmentPins, resistorsOnSegments, updateWithDelays, leadingZeros, disableDecPoint);
    sevseg.blank();
    
    Serial.begin(115200);
    Serial1.begin(115200);
    while(!Serial1){}

    inputString.reserve(200);
    serial1_inputString.reserve(200);
}

void loop() {
    while(Serial.available()){
    char inChar = (char)Serial.read();
    inputString += inChar;

    if(inChar == '\n'){
      stringComplete = true;
    }
  }

  if (stringComplete) {
    sevseg.blank();
    Serial.println(inputString);
    

    inputString.toFloat();
    Serial.print("toFloat() : ");
    Serial.println(inputString.toFloat());

    if(inputString.toFloat()) sevseg.setNumberF(inputString.toFloat(), 1);

    // clear the string:
    inputString = "";
    
    stringComplete = false;
  }


  while(Serial1.available()){
    char inChar = (char)Serial1.read();
    serial1_inputString += inChar;

    if(inChar == '\n'){
      serial1Complete = true;
    }
  }

  if (serial1Complete) {
    sevseg.blank();

    if(serial1_inputString.toFloat()) sevseg.setNumberF(serial1_inputString.toFloat(), 1);\
    Serial.print("SomeSerial : ");
    Serial.println(serial1_inputString); 
    
    // clear the string:
    serial1_inputString = "";
    
    serial1Complete = false;
  }
  
  sevseg.refreshDisplay();
}
