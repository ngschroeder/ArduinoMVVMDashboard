# Arduino MVVM Dashboard
A cross-platform AvaloniaUI framework app that connects to and controls samples on an Arduino Mega.

## About

This is a pretty simple project that demonstrates various functionality covering several areas. It uses the standard HC-SR04 Ultrasonic sensor to report distance, which will increase the brightness of the LED the closer the sensor is to an object. Additionally, there is a 16x2 LCD hooked directly to the Arduino (with contrast set manully to 75 to spare the need for a potentiometer).

The desktop application can connect, report the distance, and send text to the LCD.

## Board Diagram

![ArduinoMonitor_bb](https://user-images.githubusercontent.com/16778828/110229295-62c34e80-7ed6-11eb-84c9-bcf18a295495.png)

## Sketch

The sketch is included in ArduinoMVVMDashboard/Sketch/ArduinoMegaMonitor/ and is fully annotated.

## Application

The app uses [AvaloniaUI Framework](https://avaloniaui.net/)'s ReactiveUI implementation. This is my first foray into MVVM, so I'm sure I've committed some cardinal sins, but I'm very proud of the fact that there is no code-behind.

To demonstrate the true cross-platforminess of this solution, I did a fresh clone of this repo on both Mac and PC, dotnet restored, and ran.

<img height="300" src="https://user-images.githubusercontent.com/16778828/110404284-9922da00-804c-11eb-9562-6d9026ada218.png">  <img height="300" src="https://user-images.githubusercontent.com/16778828/110263728-8b565180-7f85-11eb-8269-1a198622c35c.png">
