#include <RoboClaw.h>

#include <SoftwareSerial.h>

const int encoderA = 7;
const int encoderB = 6;
SoftwareSerial serial(10, 11);
RoboClaw roboclaw(&serial, 10000);
#define address 0x80
const int StartSw = 8;
const int StopSw = 9;
int i = 0;
int j = 0;
int k = 0;
void setup() {
  pinMode (encoderA, INPUT);
  pinMode (encoderB, INPUT);
  pinMode (StartSw, INPUT);
  pinMode (StopSw, INPUT);
  digitalWrite (StartSw, HIGH);
  digitalWrite (StopSw, HIGH);
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }

  //attachInterrupt(0, increment, FALLING); // pin 2
  //Open roboclaw serial ports

  roboclaw.begin(38400);
}

void loop() {
  // put your main code here, to run repeatedly:
  i = digitalRead(encoderA);
  j = digitalRead(encoderB);

  if (j == 1)
  {
    Serial.print("hit\n");
    roboclaw.ForwardM1(address, 50);
  }
  if (j == 0 && i == 1)
  {
    Serial.print("hit\n");
    roboclaw.ForwardM1(address, 0);
  }
  Serial.print(i);
  Serial.print(j);
  Serial.print("\n");
  delay(100);

}
