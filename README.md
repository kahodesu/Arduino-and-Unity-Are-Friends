# Arduino-and-Unity-Are-Friends
This repository provides a script tool (**Ardunio Controller.cs**) for basic Unity/Arduino communication.

## Getting Started
Two steps are required in order to add Arduino communication capabilites to your Unity projects.
1. Add the **Ardunio Controller.cs** script to an empty game object in your Unity scene.
2. Change your Unity API Compatibility Level to .NET 4.x, see [Unity Documentation](https://docs.unity3d.com/2019.1/Documentation/Manual/dotnetProfileSupport.html) for details on how to do this.

## Arduino Controller Script Configuration Options
Upon successfully adding **Ardunio Controller.cs** script to an empty game object in your Unity scene, you will have the following options you can configure in the Inspector:
- **Port** - The serial port for communication with Arduino.
- **Baudrate** - The baudrate for communication with Arduino.
- **Delimiter** - The delimiter to parse all incoming Arduino messages with. If you send multiple values via Serial.println on the Arduino side, make sure that they are seperated using this delimiter.
- **Baudrate** - The baudrate for communication with Arduino.
- **Read Timeout** - How long to wait before reading again via serial for a new Arduino message. *Generally don't change this unless you know what you are doing.*
- **Write Timeout** - How long to wait before determining that the serial write failed. *Generally don't change this unless you know what you are doing.*
- **Verbose Debugging** - Will log much more detailed information to the Unity Console if checked.
- **Last Arduino Values** - An array containing the latest values sent from Arduino via serial and separated using the specified delimiter.

## Using the Arduino Controller
The Arduino Controller script uses a singleton programming pattern, so once it is added to an empty game object on the Unity scene then it can be accessed from any other script using
> ArduinoController.instance

*NOTE: make sure to use the Available() function to ensure that some data has been received before attempting to access lastArduinoValues.*

See **Unity_Arduino_Communicaiton_Example** for an example of the Unity and Arduino code/setup needed for bi-directional communication.

### Last Tested with Unity 2019.4.15f1
