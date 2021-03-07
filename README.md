# ArduinoMVVMDashboard
A cross-platform AvaloniaUI framework app that connects to and controls samples on an Arduino Mega. Includes diagram and sketch.

## About

This is a pretty simple project that demonstrates some functionality that covers a lot of areas. It uses the standard HC-SR04 Ultrasonic sensor to report distance, which will increase the brightness of the LED the closer the sensor is to an object. Additionally, there is a direct 16x2 LCD hooked up (with contrast set manully to 75 to spare the need for a potentiometer).

The desktop application uses MVVM (with only bindings, no code-behind) to connect, report the distance, and send text to the LCD.

### Board Diagram

![ArduinoMonitor_bb](https://user-images.githubusercontent.com/16778828/110229295-62c34e80-7ed6-11eb-84c9-bcf18a295495.png)

### Application

<img width="286" alt="Screen Shot 2021-03-07 at 12 09 45 AM" src="https://user-images.githubusercontent.com/16778828/110229705-71f7cb80-7ed9-11eb-993b-1f3178aba126.png">
