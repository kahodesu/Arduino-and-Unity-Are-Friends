////////////////////////////////////////////////////
//    BTJS Arduino Unity Continuous Bytes Example //
//    Kaho Abe 3/3/2018                           //
////////////////////////////////////////////////////
//    -- LED at PWM pin 9                         //
//  uses http://www.arduino.cc/en/Tutorial/Fade   //
////////////////////////////////////////////////////

int led = 9;           // the PWM pin the LED is attached to
int brightness = 0;    // how bright the LED is
int fadeAmount = 5;    // how many points to fade the LED by
int ledMsg =0;
int counter =0;
// the setup routine runs once when you press reset:
void setup() {
  // declare pin 9 to be an output:
  pinMode(led, OUTPUT);
  Serial.begin(115200);
}

// the loop routine runs over and over again forever:
void loop() {
Serial.write(counter);
counter++;
if(counter>255) {
counter =0;
}
  if (Serial.available()) { //If anything comes in Serial
    ledMsg = Serial.read(); //read it and save in ledMsg              
  }

    
  // set the brightness of pin 9:
  analogWrite(led, ledMsg);

  
  delay(10);
}
