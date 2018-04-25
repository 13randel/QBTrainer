#include <RoboClaw.h>
#include <SoftwareSerial.h>

#include "FastLED.h"

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
  
  attachInterrupt(digitalPinToInterrupt(2), Test, HIGH);*/
  pinMode(5, INPUT);
  pinMode(6, INPUT);

  pinMode(7, OUTPUT);
  pinMode(8, OUTPUT);
  
  while( !Serial) ;
  FastLED.addLeds<WS2811, 9, RGB>(&leds[3], NUM_LEDS_PER_STRIP * NUM_STRIPS);
  roboclaw.begin(38400);
  roboclaw.ForwardM1(address, 0);
  for(int i = 0; i < NUM_LEDS_PER_STRIP; i++){
    leds[i] = CRGB::Black;
    FastLED.show();
  }
}
unsigned int n_leds = 0;
unsigned int dst_speed = 0;
//Interrupt handler for the sensor kill switch and reset.
void Test(){
  Serial.println("Triggered");
}
void SensorTrigger(){
  uint8_t status;
  bool valid;

  Serial.println("Trigger");
  
  roboclaw.ForwardM1(address, 0);
  dst_speed = 35;
  //Read the current encoder position, and move the motor backward at slow speeds
  //Until the encoder position is the same as the first encoder position
  encoderPos2 = roboclaw.ReadEncM1(address, &status, &valid);
  roboclaw.ForwardM1(address, dst_speed);
  while(encoderPos2 != encoderPos1){
    encoderPos2 = roboclaw.ReadEncM1(address, &status, &valid);
  }
  //This will need to be changed if the remote is going to be used. 
  //If only the touchscreen pi is used this will be fine
  roboclaw.ForwardM1(address, 0);
  dst_speed = 0;
  encoderPos1 = roboclaw.ReadEncM1(address, &status, &valid);
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
  double led_per_s = (((double)dst_speed / 127.0) * 300.0) / 2.34;
  unsigned int delay_millis = ((double)n_leds / (double)led_per_s) ;
  Serial.println(dst_speed);
  Serial.println(led_per_s);
  Serial.println(delay_millis);

  delay_millis = 0;
  /*
  for (int i = 0; i >= NUM_LEDS_PER_STRIP + 2*LED_RUNNER_SIZE; i += 1) { 
      leds[i] = CRGB::Black;  
  }
  */
  FastLED.show();
  leds[n_leds-1] = CRGB::White;
  leds[n_leds-2] = CRGB::White;
  leds[n_leds-3] = CRGB::White;
  leds[n_leds-4] = CRGB::White;
  leds[n_leds-5] = CRGB::White;
  FastLED.show();
  for(int i = n_leds-6; i>=0; i--){
    leds[i] = CRGB::White;
    FastLED.show();
    delay(2*delay_millis);
    leds[i+6] = CRGB::Black;
    FastLED.show();
    
  }/*
  for (int i = n_leds - 1; i >= 0; i -= 1) {
    for (int j = 0; j < LED_RUNNER_SIZE; j += 1) {
      leds[i + j] = CRGB::White;
    }
    FastLED.show();
    delay(2 * delay_millis);
    for (int j = 0; j < LED_RUNNER_SIZE; j += 1) {
      leds[i + j] = CRGB::Black;
    }
  }*/
}

unsigned long long switch_toggle_time = 0;

void loop() {
  // if (digitalRead(5) == HIGH || digitalRead(6) == HIGH)
    // SensorTrigger();

  n_leds = 500;
  dst_speed = 60;
  runLEDs();
  

  
  // using Serial.available() makes checking serial so much faster
  // this is neccessary to make the acceleration smooth
  if (Serial.available()) {
    dst_speed = readUnsignedUntil('|');
    if(dst_speed == 200) SensorTrigger();
    //give this a different end char to prevent issues
    n_leds = readUnsignedUntil('|');
    // char newline = Serial.read();
    if (dst_speed) {
      if (T == 0.0) T = 0.00001;
      // calculate rate so that we approach dst_speed in T seconds
      // 0.01 is just a small number so that we don't divide by zero
      k = -1.0  * log(((float)dst_speed) / 0.01) / T;
      //runLEDs();
      start = millis();
      //delay
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
  */
}
