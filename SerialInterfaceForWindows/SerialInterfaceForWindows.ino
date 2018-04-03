#include <RoboClaw.h>

#include <SoftwareSerial.h>

#include "FastLED.h"

#define NUM_STRIPS 1
#define NUM_LEDS_PER_STRIP 50
CRGB leds[NUM_LEDS_PER_STRIP];

// For mirroring strips, all the "special" stuff happens just in setup.  We
// just addLeds multiple times, once for each strip
  
const int StartSw = 8;
const int StopSw = 9;
const int ButtonA= 7;
const int ButtonB = 6;
int i = 0;
int j = 0;
int k = 0;
SoftwareSerial serial(10, 11);
RoboClaw roboclaw(&serial, 10000);
int motorspeed = 0;
int motordelay = 0;
#define address 0x80

void LEDSTART(int timeDelay)
{
    for(int i = NUM_LEDS_PER_STRIP; i > 0; i--) {
      
      leds[i] = CRGB::Black;
      delay(timeDelay);
      FastLED.show();
    }
}

void LEDON()
{
  for(int i = 0; i < NUM_LEDS_PER_STRIP; i++) {
      leds[i] = CRGB::White;
      FastLED.show();
    }
}

void MotorSTART(int timeDelay, int motorSpeed)
{
  
      roboclaw.ForwardM1(address, motorSpeed);
      digitalWrite(LED_BUILTIN, HIGH);
      delay(timeDelay);
      digitalWrite(LED_BUILTIN, LOW);
      roboclaw.ForwardM1(address, 0);  
}

void setup() {
  // tell FastLED there's 60 NEOPIXEL leds on pin 8
  FastLED.addLeds<WS2811, 5, RGB>(leds, NUM_LEDS_PER_STRIP);
  pinMode(LED_BUILTIN, OUTPUT);
  // put your setup code here, to run once:
  pinMode (StartSw, INPUT);
  pinMode (StopSw, INPUT);
  digitalWrite (StartSw, HIGH);
  digitalWrite (StopSw, HIGH);
  Serial.begin(57600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  
  //attachInterrupt(0, increment, FALLING); // pin 2
  //Open roboclaw serial ports
  
  roboclaw.begin(38400);
  
}
void loop() {
  if (Serial.available())
  {
    //i = digitalRead(ButtonA);
    //j = digitalRead(ButtonB);

    char ch = (char)Serial.read();
    Serial.write(ch);
    Serial.write("\n");
    if (ch == 's')
    {
      int size = Serial.available();
      //Serial.flush(); 
      
      //Get Motor speed 
      char inputBuffer[10] = "";
      String inputString = "";
      Serial.readBytesUntil('m', inputBuffer, 4);
      for(int k=0; k<4; k++){
      inputString += inputBuffer[k];
      }
      motorspeed = inputString.toInt();
      Serial.println("Motor Speed:" + inputString);
      Serial.write("\n");
      
      inputString = "";
      //inputBuffer = "";
      Serial.readBytesUntil('l', inputBuffer, 6);
      for(int k=0; k<6; k++){
      inputString += inputBuffer[k];
      }
      motordelay = inputString.toInt();
      Serial.println("Motor Delay:" + inputString);
      Serial.write("\n");

      inputString = "";
      //inputBuffer = "";
      Serial.readBytesUntil('e', inputBuffer, 3);
      for(int k=0; k<3; k++){
      inputString += inputBuffer[k];
      }
      int LEDDelay = inputString.toInt();
      Serial.println("LED Delay:" + inputString);
      Serial.write("\n");
      LEDON();
      //LEDSTART(15);
      for(int i = NUM_LEDS_PER_STRIP-1; i >= 0; i--) {
      
      leds[i] = CRGB::Black;
      delay(50);
      FastLED.show();
    }
    Serial.write("done");
      roboclaw.ForwardM1(address, motorspeed);
      digitalWrite(LED_BUILTIN, HIGH);
      delay(motordelay);
      digitalWrite(LED_BUILTIN, LOW);
      roboclaw.ForwardM1(address, 0);
      Serial.write("done2");
      //MotorSTART(motordelay, motorspeed);
    }
    else if (ch == 'r')
    {
      for(int i = 0; i < NUM_LEDS_PER_STRIP; i++) {
      leds[i] = CRGB::White;
      FastLED.show();
      }
      Serial.write("ON");
    }
    else if (ch == 'o')
    {
      for(int i = 0; i < 50; i++) {
      leds[i] = CRGB::Black;
      FastLED.show();
      }
      Serial.write("OFF");
    }
    ch = ' ';
  }


}
