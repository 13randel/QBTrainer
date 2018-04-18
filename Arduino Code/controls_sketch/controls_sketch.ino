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

int dst_speed = 0;
int speed = 0;
int accel = 30;

int getAccel(int curSpeed) {
  if (curSpeed < 10)
    return 32;
  else if (curSpeed < 25)
    return 24;
  else if (curSpeed < 50)
    return 16;
  else if (curSpeed < 80)
    return 4;
  else return 1 ;
}

void loop() {
  int input = (int)Serial.read();
  if(input != 0){
  dst_speed = input;//(int)Serial.read();
  Serial.println(input);
  input = 0;
  }
 
    unsigned long t = millis();
    int currentSpeed = readSpeedToUnits();
    uint8_t status;
    bool valid;
    //Serial.println(abs(roboclaw.ReadSpeedM1(address, &status, &valid)));
    if (currentSpeed < dst_speed) {
      roboclaw.ForwardM1(address, speed);
      while (millis() - t < 250);
      if (speed >= 123)
        speed = 127;
      else if(speed <= dst_speed)
        speed += accel > 4 ? accel / 4 : 1;  
    }
    accel = getAccel(speed);
}
