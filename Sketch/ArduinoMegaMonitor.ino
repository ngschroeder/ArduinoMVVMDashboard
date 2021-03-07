// --------------------------------------------------------------------------------- //
// Arduino Mega 2560
// UltraSonic Sensor / LED / LCD / C# MVVM Demo
// Author: Nick Schroeder
// See github for details: https://github.com/ngschroeder/ArduinoMVVMDashboard
// -------------------------------------------------------------------------------- //
#include <LiquidCrystal.h> 

#define echoPin 11
#define trigPin 12
#define ledPin 13

String incomingString;
String units = "cm";
String printStr = "";
long duration;
long cycle = 0;
int distance;
int oldDistance;
int brightness = 0;
int contrast = 75;
float brightnessFactor = 0.80;
bool sendLoop = false;  
bool tickCycles = true;
LiquidCrystal lcd(7, 8, 5, 4, 3, 2);  

void setup() {
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  pinMode(ledPin, OUTPUT);
  Serial.begin(9600);
  analogWrite(6,contrast);
  lcd.begin(16, 2);
  lcd.setCursor(0, 0);
  lcd.print("Loop cycles:");
}

void loop() {
  checkPort();
  if(tickCycles){
    lcd.setCursor(0, 1);
    lcd.print(cycle); 
  }
  if (sendLoop) {  
    analogWrite(ledPin, brightness);
    digitalWrite(trigPin, LOW);
    delayMicroseconds(2);
    digitalWrite(trigPin, HIGH);
    delayMicroseconds(10);
    digitalWrite(trigPin, LOW);
    duration = pulseIn(echoPin, HIGH);
    if(units == "cm"){
      distance = duration * 0.0343 / 2;
    }else{
      distance = duration * 0.0133 / 2;
    }
    brightness = (255 - (distance * 3)) * brightnessFactor;
    if(brightness < 0){
      brightness = 1;
    }
    if(distance != oldDistance){
      Serial.print(distance);
      oldDistance = distance;
    }
    delay(200);
  }
  cycle++;
}

void checkPort() {
  while (Serial.available())
  {
    incomingString = Serial.readStringUntil('\n');
  }
  if (incomingString == "getDist") {
    sendLoop = true;
    oldDistance = -1;
  }
  if (incomingString == "stopDist") {
    sendLoop = false;
  }
  if (incomingString == "changeUnits") {
    if(units == "cm"){
      units = "in";
    }else{
      units = "cm";
    }
  }
  if (incomingString.startsWith("lcd1")){
    incomingString.replace("lcd1:", "");
    lcd.setCursor(0, 0);
    lcd.print(incomingString);
  }
  if (incomingString.startsWith("lcd2")){
    incomingString.replace("lcd2:", "");
    lcd.setCursor(0, 1);
    lcd.print(incomingString);
    tickCycles = false;
  }
  incomingString = "";
}
