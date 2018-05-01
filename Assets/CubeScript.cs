using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
	public ArduinoConnector arduinoConnector; //create an instance of object ArduinoConnector called arduinoConnector
	public Renderer rend; //create an instance of object Renderer called rend
 	float timeLeft = 0;
	int var;
	bool yellow = false; //just have to make sure it starts the same as on the Arduino

	void Start()
	{		
		arduinoConnector = GetComponent<ArduinoConnector>(); //to access properties of ArduinoConnector script
		arduinoConnector.Open(); //open serial connection
    	rend = GetComponent<Renderer>(); //to access the renderer of the object
    	rend.material.SetColor("_Color",Color.blue); //set the color of the cube to BLUE
	}

	void Update()
	{
		
		var = arduinoConnector.ReadFromArduino(1); //read from Arduino
		
		if (var == 1) {	 //If 1 is being sent from Arduino - button is being pressed!
			Debug.Log("Button Pressed!");
        	rend.material.SetColor("_Color",Color.red); //turn the cube RED
        	timeLeft = .50f; //start the timer
		} 

		timeLeft -= Time.deltaTime; //counting down
		if ( timeLeft < 0 ) //if time has run out
     	{
        rend.material.SetColor("_Color",Color.blue); //change the cube back to BLUE
    	}	
	}
	
	void OnMouseDown() //if the cube is clicked
    {
       if (!yellow){
       		Debug.Log("Turn on LED!");
       		rend.material.SetColor("_Color",Color.yellow); //turn the cube YELLOW
       		arduinoConnector.WriteToArduino(255); //send 255 to Arduino
       		yellow = true;
		}
		else {
			Debug.Log("Turn LED off!");
       		rend.material.SetColor("_Color",Color.blue); //turn the cube YELLOW
       		arduinoConnector.WriteToArduino(255); //send 255 to Arduino
       		yellow = false;
		}
    }	
}
