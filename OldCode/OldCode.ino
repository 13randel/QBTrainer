//This code contains examples of how to control the Roboclaw with an Arduino
// It is not complete and not all things are implemented such as the interrupt


//See BareMinimum example for a list of library functions

//Includes required to use Roboclaw library
#include <SoftwareSerial.h>
#include <RoboClaw.h>

const int StartSw = 8;
const int StopSw = 9;
int k = 1;
int s=1;
int t=1;

//See limitations of Arduino SoftwareSerial
SoftwareSerial serial(10, 11);
RoboClaw roboclaw(&serial, 10000);

#define address 0x80

void setup() {
  pinMode (StartSw, INPUT);
  pinMode (StopSw, INPUT);
  digitalWrite (StartSw, HIGH);
  digitalWrite (StopSw, HIGH);
  
  //attachInterrupt(0, increment, FALLING); // pin 2
  //Open roboclaw serial ports
  
  roboclaw.begin(38400);
}

//void increment(){  //an interrupt routine meant to stay in a contiuous loop until power is turned off
//  do{
//  roboclaw.ForwardM1(address, 0); //stops the motor
//  }while (s==1);
//}

// Loop until start switch is pressed
void loop() {
  do {
    k = digitalRead(StartSw); 
  }while (k == 1);

//start Motor1 forward  0 = stop, 127 = full speed

//This accelerates the motor  make the delay as short as possible
  roboclaw.ForwardM1(address, 5);
  delay(200);
  roboclaw.ForwardM1(address, 10);
  delay(200);
  roboclaw.ForwardM1(address, 20);
  delay(200);
  roboclaw.ForwardM1(address, 30);
  delay(200);
  roboclaw.ForwardM1(address, 40);
  delay(200);
  roboclaw.ForwardM1(address, 50);
  delay(200);
  roboclaw.ForwardM1(address, 60);
  delay(200);
  roboclaw.ForwardM1(address, 70);
  delay(200);
  roboclaw.ForwardM1(address, 80);
  delay(200);
  roboclaw.ForwardM1(address, 90);
  delay(200);
  roboclaw.ForwardM1(address, 100);
  delay(200);
  roboclaw.ForwardM1(address, 120);
  delay(200);
  
  //idle here until optical limit sw is reached  
  do{
    s = digitalRead(StopSw); 
  }while (s==1);



//this de-accelerates the motor
  roboclaw.ForwardM1(address, 100);
  delay(200);
  roboclaw.ForwardM1(address, 80);
  delay(200);
  roboclaw.ForwardM1(address, 60);
  delay(200);
  roboclaw.ForwardM1(address, 40);
  delay(200);
  roboclaw.ForwardM1(address, 20);
  delay(200);
  roboclaw.ForwardM1(address, 0);


  //idle here until return switch is pressed 

  do {
    k = digitalRead(StartSw);
  
  }while (k == 1);



 //This accelerates the motor  make the delay as short as possible
  roboclaw.BackwardM1(address, 5);
  delay(200);
  roboclaw.BackwardM1(address, 10);
  delay(200);
  roboclaw.BackwardM1(address, 20);
  delay(200);
  roboclaw.BackwardM1(address, 30);
  delay(200);
  roboclaw.BackwardM1(address, 40);
  delay(200);
  roboclaw.BackwardM1(address, 50);
  delay(200);
  roboclaw.BackwardM1(address, 60);
  delay(200);
  roboclaw.BackwardM1(address, 70);
  delay(200);
  roboclaw.BackwardM1(address, 80);
  delay(200);
  roboclaw.BackwardM1(address, 90);
  delay(200);
  roboclaw.BackwardM1(address, 100);
  delay(200);
  roboclaw.BackwardM1(address, 120);
  delay(200);
  
  //idle here until optical limit sw is reached  
  do{
    s = digitalRead(StopSw); 
  }while (s==1);

  //this de-accelerates the motor
  roboclaw.BackwardM1(address, 100);
  delay(200);
  roboclaw.BackwardM1(address, 80);
  delay(200);
  roboclaw.BackwardM1(address, 60);
  delay(200);
  roboclaw.BackwardM1(address, 40);
  delay(200);
  roboclaw.BackwardM1(address, 20);
  delay(200);
  roboclaw.BackwardM1(address, 0);

}

