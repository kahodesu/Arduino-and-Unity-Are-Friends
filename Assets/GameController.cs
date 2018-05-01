using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public ArduinoConnector arduinoConnector; //create an instance of object ArduinoConnector called arduinoConnector
    public GameObject sphere;

    float timeLeft = 0;
    int var;
    bool read = false;
    byte counter = 0;

    // Use this for initialization
    void Start () {
        arduinoConnector = GetComponent<ArduinoConnector>(); //to access properties of ArduinoConnector script
        arduinoConnector.Open(); //open serial connection
        arduinoConnector.ReadByteFromArduinoAsync(arduinoRead); //read from Arduino
    }

    // Update is called once per frame
    void Update () {
        if (read)
        {
            if (var > 0)
            {
                sphere.transform.localScale = new Vector3((float)(var / 255.0f), (float)(var / 255.0f), (float)(var / 255.0f));
            }
            arduinoConnector.WriteByteToArduinoAsync(counter); //send 255 to Arduino	
            counter++;
            if (counter > 255)
            {
                counter = 0;
            }

            read = false;

            arduinoConnector.ReadByteFromArduinoAsync(arduinoRead); //read from Arduino
        }
    }

    void arduinoRead(byte data)
    {
        var = data;
        read = true;
    }
}
