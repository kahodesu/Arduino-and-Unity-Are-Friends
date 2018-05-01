 ////////////////////////////////////////////////////////////////////////
//    BTJS Arduino Unity Example                                      //
//    Kaho Abe 3/3/2018                                               //
////////////////////////////////////////////////////////////////////////
//    -- push button at pin 2 similar to the Arduino BUTTON example   //
//    -- LED attached to pin 13, or use the builtin LED               //
////////////////////////////////////////////////////////////////////////


///////////////////////// THE VARIABLES /////////////////////////////// 
//button variables
const int buttonPin = 2;     // the number of the pushbutton pin
int buttonState = 0;         // variable for reading the pushbutton status     

//LED variables
int ledMsg;                 //where the 
int duration = 1000;        //duration of the timer that turns off LED after a certain time
long startTime;             //where the start time of the timer will be stored
long currentTime;           //where the current time is stored
boolean yellow = false;


///////////////////////// THE SETUP /////////////////////////////// 
// the setup function runs once in the beginning ok
void setup() {
  pinMode(LED_BUILTIN, OUTPUT); //output at builtin LED, or pin 13
  Serial.begin(115200);       //begins serial communication at baud rate 9600
}


/////////////////////// THE LOOP /////////////////////////////// 
// the loop function runs over and over again forever, ever, ever....
void loop() {
  read(); 
  send();
  delay(100);               //important to not overwhelm unity
}


//////////////////////// THE FUNCTIONS /////////////////////////////// 
void read(){              
  if (Serial.available()) { //If anything comes in Serial
    ledMsg = Serial.read(); //read it and save in ledMsg                     
    if (ledMsg ==255) {     //if the message received is 255    
        if (yellow == false) {
 digitalWrite(LED_BUILTIN, HIGH); //turn on the LED
        yellow = true;
        }
        else{

 digitalWrite(LED_BUILTIN, LOW); //turn on the LED
        yellow=false;
        }
     
      startTime = millis(); //start the timer
    }
  }
  checkTime();              //check the timer
}

void send(){
  pushButton();
}


void pushButton(){
//checks if button is pressed
  buttonState = digitalRead(buttonPin); //reads buttonPin
  //if button is pressed
  if (buttonState == HIGH) { //when button is being pressed
    Serial.write(1);          //writes in serial communications 1
  }
}


void checkTime() {            //function that checks timer
  currentTime = millis();     //finds what time it is
  // Serial.println(currentTime-startTime); //FOR DEBUG 
  if ((currentTime-startTime)<duration) {   //if it hasn't reached the end of the timer duration
    digitalWrite(LED_BUILTIN,HIGH); //turn the LED on
  }
  else {                    //otherwise turn LED off
    digitalWrite(LED_BUILTIN, LOW); 
  }
}
 
