#include <RoboClaw.h>

#include <SoftwareSerial.h>


// For mirroring strips, all the "special" stuff happens just in setup.  We
// just addLeds multiple times, once for each strip


int val;
int encoder0PinA = 7;
int encoder0PinB = 6;
int encoder0PinC = 8;
int encoder0Pos = 0;
int encoder0PinALast = LOW;
int n = LOW;
int i = HIGH;

const int StartSw = 8;
const int StopSw = 9;

int j = 0;
int k = 0;
SoftwareSerial serial(10, 11);
RoboClaw roboclaw(&serial, 10000);
int motorspeed = 0;
int motordelay = 0;
#define address 0x80


void setup() {
  // tell FastLED there's 60 NEOPIXEL leds on pin 8
  // put your setup code here, to run once:
  pinMode (StartSw, INPUT);
  pinMode (StopSw, INPUT);
  digitalWrite (StartSw, HIGH);
  digitalWrite (StopSw, HIGH);
  Serial.begin(57600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  roboclaw.begin(38400);
}

void loop() {
  n = digitalRead(encoder0PinC);
  //Serial.print (n);
  if (n == LOW && i != LOW) {
    encoder0Pos = encoder0Pos +1;
  }
  else
  {
  }
  i = n;
  Serial.print (encoder0Pos);
  Serial.print ("\n");
  char ch = (char)Serial.read();
    if (ch == 'n')
    {
      roboclaw.ForwardM1(address, 50);
    }
    else if (ch == 'f' || encoder0Pos == 1)
    {
      roboclaw.ForwardM1(address, 0);
    }
    
}
