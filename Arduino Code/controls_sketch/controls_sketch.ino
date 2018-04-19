#include <RoboClaw.h>
#include <SoftwareSerial.h>
 
// Roboclaw is set to Serial Packet Mode
#define address 0x80
// this is usb cable from Arduino to computer
SoftwareSerial serial(10,11);
//SoftwareSerial serial2(0,1);
RoboClaw roboclaw(&serial, 10000);    // serial connection to RoboClaw
unsigned int motorspeed = 0;

void setup() {
    Serial.begin(57600);
    roboclaw.ResetEncoders(address);
    
    while( !Serial) ;
    roboclaw.begin(38400);
    roboclaw.ForwardM1(address, 0);
}

// read qpps from the encoder and translate that to a value between 0 and 127
int readSpeedToUnits() {
  uint8_t status = 0;
  bool valid = true;
  long speed = abs(roboclaw.ReadSpeedM1(address, &status, &valid));
  speed = ((double)speed / 44000.0) * 127;

  // it's consistently off by a little, so we do some correction
  if (speed >= 3)
    speed += 2;
  return speed;
}

unsigned int dst_speed = 0;
unsigned int speed = 0;
unsigned int accel = 30;

// time it takes to accelerate to dst_speed -- user set
float T = 2.0;

// decay rate -- program set
float k = 0.0;

// time vars -- program set
unsigned long long start = 0;
unsigned long long now = 0;

// use increasing exponential growth to calculate the acceleration
int getAccel() {
  double t = (double)(now - start) / 1000;
  return (dst_speed * (1 - exp(k*t)) - speed);
}

void loop() {
  // using Serial.available() makes checking serial so much faster
  // this is neccessary to make the acceleration smooth
  if (Serial.available()) {
    char buff[32] = { 0 };
    int n_bytes = (int)Serial.readBytesUntil('|', buff, 32);
    if (n_bytes > 0){
      String s(buff);
      dst_speed = s.toInt();

      if (dst_speed) {
        if (T == 0.0) T = 0.00001;
        // calculate rate so that we approach dst_speed in T seconds
        // 0.01 is just a small number so that we don't divide by zero
        k = -1.0  * log(((float)dst_speed) / 0.01) / T;
      }
      
      start = millis();
    }
  }
 
    now = millis();
    int currentSpeed = readSpeedToUnits();
    if (currentSpeed < dst_speed) {
      roboclaw.ForwardM1(address, speed);

      // make sure not to overflow
      if ((int)speed + accel > 127) {
        speed = 127;
      } else {
        speed += accel;
      } 
    } else if (currentSpeed > dst_speed) {
      // decreases in speed should be instant
      // this helps when we want to stop the motor
      speed = dst_speed;
      roboclaw.ForwardM1(address, speed);
    }
    accel = getAccel();
}
