# ArduinoMVVMDashboard
A cross-platform AvaloniaUI framework app that connects to and controls samples on an Arduino Mega. Includes diagram and sketch.

## About

This is a pretty simple project that demonstrates some functionality that covers a lot of areas. It uses the standard HC-SR04 Ultrasonic sensor to report distance, which will increase the brightness of the LED the closer the sensor is to an object. Additionally, there is a direct 16x2 LCD hooked up (with contrast set manully to 75 to spare the need for a potentiometer).

The desktop application uses MVVM (with only bindings, no code-behind) to connect, report the distance, and send text to the LCD.

### Board Diagram

![ArduinoMonitor_bb](https://user-images.githubusercontent.com/16778828/110229295-62c34e80-7ed6-11eb-84c9-bcf18a295495.png)

### Application

![Mac Version](https://user-images.githubusercontent.com/16778828/110250290-8cb55900-7f48-11eb-9eda-c8fa81918c34.png) | ![Screen Shot 2021-03-07 at 8 39 42 PM](https://user-images.githubusercontent.com/16778828/110263728-8b565180-7f85-11eb-8269-1a198622c35c.png)
