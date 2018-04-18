#include <RoboClaw.h>
#include <SoftwareSerial.h>
 
// Roboclaw is set to Serial Packet Mode
#define address 0x80
#define Kp 1.0
#define Ki 0.5
#define Kd 0.25
#define qpps 44000
      // this is usb cable from Arduino to computer
SoftwareSerial serial(10,11);
//SoftwareSerial serial2(0,1);
RoboClaw roboclaw(&serial, 10000);    // serial connection to RoboClaw
long avgSpeedM1, avgSpeedM2;
// alpha is used to filter the results
float alpha = .10; // .1 = data smoothing single pole filter setting.
unsigned int motorspeed = 0;

void setup() {
    Serial.begin(57600);
    // Serial.begin(9600);

    
    //roboclaw.SetM1VelocityPID(address,Kd,Kp,Ki,qpps);
    roboclaw.ResetEncoders(address);
    while( !Serial) ;
    roboclaw.begin(38400);
    roboclaw.ForwardM1(address, 0);
}

int readSpeedToUnits() {
  uint8_t status = 0;
  bool valid = true;
  long speed = abs(roboclaw.ReadSpeedM1(address, &status, &valid));
  speed = ((double)speed / 44000.0) * 127;
  if (speed >= 3)
    speed += 2;
  return speed;
}

unsigned int dst_speed = 0;
unsigned int speed = 0;
unsigned int accel = 30;

int getAccel() {
  float ratio = (float)speed / (float)dst_speed;
  if (ratio < 0.5)
    return 25;
  else if (ratio < 0.8)
    return 8;
  else return 2;
}

void loop() {
  Serial.println(accel);
  char buff[32] = { 0 };
  int n_bytes = (int)Serial.readBytesUntil('|', buff, 32);
  if (n_bytes > 0){
    String s(buff);
    dst_speed = s.toInt();
    Serial.println(dst_speed);
  }
 
    // unsigned long t = millis();
    int currentSpeed = readSpeedToUnits();
    if (currentSpeed < dst_speed) {
      roboclaw.ForwardM1(address, speed);
      // while (millis() - t < 100);
      if ((int)speed + accel > 127) {
        speed = 127;
      } else {
        speed += accel;
      }  
    } else if (currentSpeed > dst_speed) {
      speed = dst_speed;
      roboclaw.ForwardM1(address, speed);
    }
    accel = getAccel();
}
