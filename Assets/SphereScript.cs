using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;


public class SphereScript : MonoBehaviour
{
	public ArduinoConnector arduinoConnector; //create an instance of object ArduinoConnector called arduinoConnector
	public Renderer rend; //create an instance of object Renderer called rend
 	float timeLeft = 0;
	int var;
	byte counter =0;

	void Start()
	{		
		arduinoConnector = GetComponent<ArduinoConnector>(); //to access properties of ArduinoConnector script
		arduinoConnector.Open(); //open serial connection
    	rend = GetComponent<Renderer>(); //to access the renderer of the object
    	rend.material.SetColor("_Color",Color.white); //set the color of the cube to BLUE
	}

	void Update()
	{
		
		var = arduinoConnector.ReadFromArduino(1); //read from Arduino
	
		if(var>0){
		Debug.Log("From Arduino: "+var);
		this.transform.localScale = new Vector3((float)(var/255.0f),(float)(var/255.0f),(float)(var/255.0f));
		}
		Debug.Log("Sending this: "+counter);
		arduinoConnector.WriteToArduino(counter); //send 255 to Arduino	
		counter++;
		if(counter>255){
			counter = 0;
		}
	}
}
