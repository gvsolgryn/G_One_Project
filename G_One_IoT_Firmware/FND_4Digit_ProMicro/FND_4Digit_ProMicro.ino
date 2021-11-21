#include <Arduino.h>
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

int digit_select_pin[4]  = {2, 5, 6, A1};
int segment_pin[8]       = {3, 7, 15, 16, 10, 4, A0, 14};
int time_delay = 5;

#define _0 {HIGH, HIGH, HIGH, HIGH, HIGH, HIGH, LOW}
#define _1 {LOW, HIGH, HIGH, LOW, LOW, LOW, LOW}
#define _2 {HIGH, HIGH, LOW, HIGH, HIGH, LOW, HIGH}
#define _3 {HIGH, HIGH, HIGH, HIGH, LOW, LOW, HIGH}
#define _4 {LOW, HIGH, HIGH, LOW, LOW, HIGH, HIGH}
#define _5 {HIGH, LOW, HIGH, HIGH, LOW, HIGH, HIGH}
#define _6 {HIGH, LOW, HIGH, HIGH, HIGH, HIGH, HIGH}
#define _7 {HIGH, HIGH, HIGH, LOW, LOW, LOW, LOW}
#define _8 {HIGH, HIGH, HIGH, HIGH, HIGH, HIGH, HIGH}
#define _9 {HIGH, HIGH, HIGH, HIGH, LOW, HIGH, HIGH}

//byte digits_data[10] = {0xFC, 0x60, 0xDA, 0xF2, 0x66, 0xB6, 0xBE, 0xE4, 0xFE, 0xE6};

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

    Serial.begin(115200);
    Serial1.begin(115200);

    Serial.println("연결 성공");
    Serial1.println("연결 성공");
}

void loop() {
    // put your main code here, to run repeatedly:
    if(Serial1.available()){
        FNDSet();
        sevseg.refreshDisplay();
    }
}

void FNDSet(){
    if(Serial1.available()){
        int test = Serial1.read();
        sevseg.setNumberF(test, 1);
    }

    else if(Serial.available()){
        int test = Serial.read();
        sevseg.setNumberF(test, 1);
    }
}

void serialEvent(){
    char c = Serial.read();
    Serial.print("rx : ");
    Serial.println(c);
}
