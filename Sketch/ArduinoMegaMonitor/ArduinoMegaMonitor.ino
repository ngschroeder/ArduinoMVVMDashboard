// --------------------------------------------------------------------------------- //
// Arduino Mega 2560
// UltraSonic Sensor / LED / LCD / C# MVVM Demo
// Author: Nick Schroeder
// See github for details: https://github.com/ngschroeder/ArduinoMVVMDashboard
// -------------------------------------------------------------------------------- //
#include <LiquidCrystal.h> 

// Define the pins

#define echoPin 11
#define trigPin 12
#define ledPin 13

// Instantiate all variables

String incomingString;            // String received from app
String units = "cm";              // Unit of measurement (defaults to cm)
long duration;                    // Stores the microseconds returned from the sensor's echo
long cycle = 0;                   // Loop counter, just for fun and debugging
int distance;                     // The calculated distance from the object
int oldDistance;                  // The calculated distance from the previous update, more on that below
int brightness = 0;               // LED brightness, starts turned off
int contrast = 75;                // LED contrast, because I hate potentiometers
float brightnessFactor = 1.0;     // Some extra math you can do on the LED (255 can hurt your eyes)
bool sendLoop = false;            // Bool that turns sensor reporting loop on or off
bool tickCycles = true;           // Boot that turns the printing of the cycles to the LCD on or off

// Map the LCD

LiquidCrystal lcd(7, 8, 5, 4, 3, 2); 

// Set up

void setup() {
  // Sensor
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  pinMode(ledPin, OUTPUT);
  // Serial
  Serial.begin(9600);
  // LCD
  analogWrite(6,contrast);
  lcd.begin(16, 2);
  lcd.setCursor(0, 0);
  lcd.print("Loop cycles:");
}

// Main loop

void loop() {
  // Checks for sent data from app every cycle
  
  checkPort();  
  
  // If the cycle counter is still active, print it
  
  if(tickCycles){
    lcd.setCursor(0, 1);
    lcd.print(cycle); 
  }
  
  // If the distance sensor is turned on, run

  if (sendLoop) {
    digitalWrite(trigPin, LOW);
    delayMicroseconds(2);
    digitalWrite(trigPin, HIGH);
    delayMicroseconds(10);
    digitalWrite(trigPin, LOW);
    duration = pulseIn(echoPin, HIGH);

    // Do math based on the unit of measurement
    
    if(units == "cm"){
      distance = duration * 0.0343 / 2;
    }else{
      distance = duration * 0.0133 / 2;
    }
    
    // Calculate brightness, override with 1 if negative, and set
    
    brightness = (255 - (distance * 6)) * brightnessFactor;
    if(brightness < 0){
      brightness = 1;
    }
    analogWrite(ledPin, brightness);
    
    // If the distance is different than last cycle, send the update, otherwise don't.
    // This eliminates redundant data being sent back to the app and wasting serial port calls
    
    if(distance != oldDistance){
      Serial.print(distance);
      oldDistance = distance;
    }

    // An arbitrary delay so things don't crawl
    
    delay(200);
  }

  // Update the cycle counter
  
  cycle++;
}

// Check port function

void checkPort() {

  // While data is being sent...
  
  while (Serial.available())
  {
    // Read the data until a newline
    incomingString = Serial.readStringUntil('\n');
    
    // The start command was sent
    if (incomingString == "getDist") {
      sendLoop = true;
      oldDistance = -1;
    }

    // The stop command was sent
    if (incomingString == "stopDist") {
      sendLoop = false;
    }

    // Switch between cm and inches command was sent
    if (incomingString == "changeUnits") {
      if(units == "cm"){
        units = "in";
      }else{
        units = "cm";
      }
    }

    // LCD line 1 was sent
    if (incomingString.startsWith("lcd1")){
      incomingString.replace("lcd1:", "");
      lcd.setCursor(0, 0);
      lcd.print(incomingString);
      tickCycles = false;
    }
    
    // LCD line 1 was sent
    if (incomingString.startsWith("lcd2")){
      incomingString.replace("lcd2:", "");
      lcd.setCursor(0, 1);
      lcd.print(incomingString);
      tickCycles = false;
    }
  }

  // Clear the string for the next call
  incomingString = "";
}
