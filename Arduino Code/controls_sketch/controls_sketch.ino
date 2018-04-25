#include <RoboClaw.h>
#include <SoftwareSerial.h>

#include <FastLED.h>

#define NUM_STRIPS 1
#define NUM_LEDS_PER_STRIP 500
#define LED_RUNNER_SIZE 8
CRGB leds[NUM_STRIPS * NUM_LEDS_PER_STRIP + 2*LED_RUNNER_SIZE];
 
// Roboclaw is set to Serial Packet Mode
#define address 0x80
// this is usb cable from Arduino to computer
SoftwareSerial serial(10,11);
RoboClaw roboclaw(&serial, 10000);    // serial connection to RoboClaw
int32_t encoderPos2 = 0;              //Readings for the encoder positions and resetting
int32_t encoderPos1 = 0;

void setup() {
  uint8_t status;
  bool valid;
  encoderPos1 = roboclaw.ReadEncM1(address, &status, &valid);
  Serial.begin(57600);
  roboclaw.ResetEncoders(address);
/*
  attachInterrupt(digitalPinToInterrupt(3), Test, HIGH);
 
  //attachInterrupt(digitalPinToInterrupt(2), SensorTrigger, CHANGE); */
  pinMode(5, INPUT);
  pinMode(6, INPUT);

  pinMode(7, OUTPUT);
  pinMode(8, OUTPUT);
  
  while( !Serial) ;
  FastLED.addLeds<WS2811, 9, RGB>(&leds[3], NUM_LEDS_PER_STRIP * NUM_STRIPS);
  roboclaw.begin(38400);
  roboclaw.ForwardM1(address, 0);
  FastLED.clear();
  leds[502] = CRGB::White;
  leds[501] = CRGB::White;
  leds[500] = CRGB::White;
  leds[499] = CRGB::White;
  leds[498] = CRGB::White;
  leds[497] = CRGB::White;
  FastLED.show();
}
unsigned int n_leds = 0;
unsigned int dst_speed = 0;

//Interrupt handler for the sensor kill switch and reset.
void SensorTrigger(){
  //detachInterrupt()
  uint8_t status;
  bool valid;
  
  roboclaw.BackwardM1(address, 0);
  dst_speed = 35;
  //Read the current encoder position, and move the motor backward at slow speeds
  //Until the encoder position is the same as the first encoder position
  encoderPos2 = roboclaw.ReadEncM1(address, &status, &valid);
  roboclaw.ForwardM1(address, dst_speed);
  while(encoderPos2 > encoderPos1){
    encoderPos2 = roboclaw.ReadEncM1(address, &status, &valid);
  }
  //This will need to be changed if the remote is going to be used. 
  //If only the touchscreen pi is used this will be fine
  roboclaw.ForwardM1(address, 0);
  dst_speed = 0;
  encoderPos1 = roboclaw.ReadEncM1(address, &status, &valid);
  //attachInterrupt(digitalPinToInterrupt(2), SensorTrigger, CHANGE)
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

unsigned int readUnsignedUntil(char delim) {
  char buff[64] = { 0 };
  int n_bytes = (int)Serial.readBytesUntil(delim, buff, 64);
  String s(buff);
  return s.toInt();
}

void runLEDs() {
  Serial.println(n_leds);
  double led_per_s = (((double)dst_speed / 127.0) * 300.0) / 2.34;
  unsigned int delay_millis = ((double)n_leds / (double)led_per_s) ;
  //Lights up LEDs in order starting from the end of the strip
  for(int i = 503-7; i>=503-n_leds; i--){
    leds[i] = CRGB::White;
    FastLED.show();
    delay(2.0*delay_millis);
    leds[i+6] = CRGB::Black;
    FastLED.show();
  }
}

unsigned long long switch_toggle_time = 0;

void loop() {
  // using Serial.available() makes checking serial so much faster
  // this is neccessary to make the acceleration smooth
  if (Serial.available()) {
    dst_speed = readUnsignedUntil('|');
    n_leds = readUnsignedUntil('|');
    //If stop button pressed or proximity sensor triggered,
    //Call reset runction
    if(dst_speed == 200 && n_leds == 0) {SensorTrigger();}
    if (dst_speed) {
      if (T == 0.0) T = 0.00001;
      // calculate rate so that we approach dst_speed in T seconds
      // 0.01 is just a small number so that we don't divide by zero
      k = -1.0  * log(((float)dst_speed) / 0.01) / T;         
      runLEDs();
      start = millis();
      FastLED.clear();
      FastLED.show();
      //delay
    }
  }
  now = millis();
  int currentSpeed = readSpeedToUnits();
  if (currentSpeed < dst_speed) {
    roboclaw.BackwardM1(address, speed);
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
    roboclaw.BackwardM1(address, speed);
  }
  accel = getAccel();
  
}
