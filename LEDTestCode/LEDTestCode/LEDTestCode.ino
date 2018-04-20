// ArrayOfLedArrays - see https://github.com/FastLED/FastLED/wiki/Multiple-Controller-Examples for more info on
// using multiple controllers.  In this example, we're going to set up three NEOPIXEL strips on three
// different pins, each strip getting its own CRGB array to be played with, only this time they're going
// to be all parts of an array of arrays.

#include "FastLED.h"

#define NUM_STRIPS 1
#define NUM_LEDS_PER_STRIP 100
CRGB leds[NUM_LEDS_PER_STRIP];

// For mirroring strips, all the "special" stuff happens just in setup.  We
// just addLeds multiple times, once for each strip
void setup() {
  // tell FastLED there's 60 NEOPIXEL leds on pin 10
  FastLED.addLeds<WS2811, 2, RGB>(leds, NUM_LEDS_PER_STRIP);
  Serial.begin(57600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  // tell FastLED there's 60 NEOPIXEL leds on pin 11
  //FastLED.addLeds<NEOPIXEL, 11>(leds[1], NUM_LEDS_PER_STRIP);

  // tell FastLED there's 60 NEOPIXEL leds on pin 12
  //FastLED.addLeds<NEOPIXEL, 12>(leds[2], NUM_LEDS_PER_STRIP);

}

void loop() {
  // This outer loop will go over each strip, one at a time
    // This inner loop will go over each led in the current strip, one at a time
    char ch = (char)Serial.read();
    Serial.write(ch);
    Serial.write("\n");
    
    if (ch == 'r')
    {
      Serial.write("ON");
    
    for(int i = 0; i < NUM_LEDS_PER_STRIP; i++) {
      leds[i] = CRGB::White;
      FastLED.show();
    }
    }
    else if( ch == 's')
    {
    delay(1000);
    for(int i = NUM_LEDS_PER_STRIP; i > 0; i--) {
      
      leds[i] = CRGB::Black;
      delay(25);
      FastLED.show();
    }
    }
}
