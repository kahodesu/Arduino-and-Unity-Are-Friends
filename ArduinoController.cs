/*
  ArduinoController.cs
  A simple Unity and Arduino communication script (ArduinoController.cs).
  This script creates a Singleton instance ArduinoController that handles all communication to/from Arduino.
  To use, just attach this script to an empty game object and set the appropriate values in the editor for Port, Baudrate, and Delimiter.
  On the Arduino side, always use Serial.println() for communication as a string.
  This script will constantly read from the serial port asynchronously and update lastArduinoValues with the latest values it reads.
  If you are sending multiple values in one line, make sure you select and use a consistent delimiter character such as ',' to separate the values.

  Created by Eddie Melcer for the Foundations of Alternative Controller Games course, 3/1/21

  This example code is available under Creative Commons Attribution 4.0 International Public License.
*/

#region Includes
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using UnityEngine;
#endregion

public class ArduinoController : MonoBehaviour
{
    #region Public Variables
    // Allows other scripts to call functions and get values from ArduinoController without storing a reference to an instance directly
    public static ArduinoController instance = null;

    // The serial port where the Arduino is connected
    [Tooltip("The serial port where the Arduino is connected")]
    public string port = "COM3";

    // The baudrate of the serial port
    [Tooltip("The baudrate of the serial port")]
    public int baudrate = 9600;

    // The delimiter for parsing messages from the Arduino via serial
    [Tooltip("The delimiter for parsing messages from the Arduino")]
    public char delimiter = ',';

    // The number of milliseconds before a time-out occurs when a read operation does not finish on the serial port
    [Tooltip("The timeout in milliseconds for a read attempt from the serial port")]
    public int readTimeout = 15;

    // The number of milliseconds before a time-out occurs when a write operation does not finish on the serial port
    [Tooltip("The timeout in milliseconds for a write attempt to the serial port")]
    public int writeTimeout = 15;

    // Do we want all of the debugging messages printed to the console?
    [Tooltip("Write all debugging information to the console as the ArduinoController runs")]
    public bool verboseDebugging = false;

    // Stores the values read from serial
    public int[] lastArduinoValues;
    #endregion

    #region Private Variables
    // The serial port we will use for communication to/from the Arduino
    private SerialPort stream;

    // The byte array that we will use to send a single byte to Arduino if desired
    byte[] b = new byte[1];
    #endregion

    #region Unity Functions
    // Our singleton pattern to make sure there is only ever one ArduinoController instance
    void Awake()
    {
        // Check if there is already an instance of ArduinoController
        if (instance == null)
        {
            // If not, set the ArduinoController instance to this
            instance = this;
        }
        // If the instance already exists:
        else if (instance != this)
        {
            // Then destroy this game object with the duplicate instace of ArduinoController, this enforces our singleton pattern so there can only ever be one instance of ArduinoController
            Destroy(gameObject);
        }

        // Set ArduinoController to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // On start try to open our serial port for communication with the Arduino
        Open();
    }

    // Update is called once per frame
    void Update()
    {
        // Every frame try to asyncronously read from the serial port. Make sure that the read timeout is less than one frame
        ReadString(UpdateArduinoValues);
    }

    // On closing of the game, try to close the serial port if it hasn't already been closed
    void OnApplicationQuit()
    {
        if(verboseDebugging) Debug.Log("Closing application, now attempting to close Arduino serial port connection...");
        Close();
    }
    #endregion

    #region Serial Port Control Functions
    // This function attempts to set up serial port communication with specified settings
    public void Open()
    {
        // Opens the serial port
        stream = new SerialPort(port, baudrate);

        // Sets some important properties of the serial port
        stream.ReadTimeout = readTimeout;
        stream.WriteTimeout = writeTimeout;
        stream.DtrEnable = true;
        stream.RtsEnable = true;

        try
        {
            if(verboseDebugging) Debug.Log("Opening serial port stream for communication...");

            // Open the serial port for communication
            stream.Open();
            
            if(verboseDebugging) Debug.Log("Serial port successfully opened!");
        }
        catch (Exception ex)
        {
            Debug.Log("Error opening serial port: " + ex);
        }
    }

    // This function determines whether the controller has valid data to read in lastArduinoValues, i.e., if it is null or not
    public bool Available()
    {
        return lastArduinoValues != null;
    }

    // This function updates the last arduino values read via serial port with new values from a passed in string
    private void UpdateArduinoValues(string newValues)
    {
        // Get all of our updated values by splitting the string of new values by their delimiter
        string[] strValues = newValues.Trim().Split(delimiter);

        // Clear the lastArduinoValues array and update it with our new arduino values
        lastArduinoValues = new int[strValues.Length];
        for(int i = 0; i < strValues.Length; i++)
        {
            try
            {
                lastArduinoValues[i] = Int32.Parse(strValues[i]);
            }
            catch (FormatException ex)
            {
                if(verboseDebugging) Debug.Log("Error updating arduino values with new read values: " + ex);
            }
        }
    }

    // This is the private function wrapper to read a string from Arduino via serial port (asynchronously)
    private void ReadString(Action<string> successCallback)
    {
        StartCoroutine(AsynchronousReadString(successCallback));
    }

    // The coroutine function to handle writing a single byte to Arduino asynchronously
    private IEnumerator AsynchronousReadString(Action<string> successCallback)
    {
        // Set the stream timout
        stream.ReadTimeout = readTimeout;

        // Stick the data into a string
        string message = "";

        try
        {
            if (verboseDebugging) Debug.Log("Attempting to read a string from the serial port...");

            // Read the next available line from the serial port
            message = stream.ReadLine();

            if (verboseDebugging) Debug.Log("Successfully read " + message + " from the serial port!");

            // Callback the success function with our newly read line
            successCallback(message);
        }
        catch (Exception ex)
        {
            if(verboseDebugging) Debug.Log("Error reading from the serial port, however this could also simply be due to no data being available for reading: " + ex);

            // Flush the stream in case the buffers are full
            //stream.BaseStream.Flush();
        }

        yield return null;
    }

    // This is the public function wrapper to write a byte to Arduino via serial port (asynchronously)
    public void WriteByte(byte message)
    {
        StartCoroutine(AsynchronousWriteByte(message));
    }

    // The coroutine function to handle writing a single byte to Arduino asynchronously
    private IEnumerator AsynchronousWriteByte(byte message)
    {
        // Set the stream timout
        stream.WriteTimeout = writeTimeout;

        // Stick byte data into byte array b
        b[0] = message;

        // Sends b with some other info.
        try
        {
            if(verboseDebugging) Debug.Log("Attempting to write " + b[0].ToString() + " to the serial port...");

            // Write the byte array starting from 0 offset and a total length of 1 byte to the serial port
            stream.Write(b, 0, 1);
            
            if (verboseDebugging) Debug.Log(b[0].ToString() + " successfully written to the serial port!");
        }
        catch (Exception ex)
        {
            Debug.Log("Error writing " + b[0].ToString() + " to the serial port: " + ex);
        }

        yield return null;
    }

    // This is the public function wrapper to write a string to Arduino via serial port (asynchronously)
    public void WriteString(string message)
    {
        StartCoroutine(AsynchronousWriteString(message));
    }

    // The coroutine function to handle writing a string to Arduino via serial port asynchronously
    private IEnumerator AsynchronousWriteString(string message)
    {
        message += "\n";

        // Set the stream timout
        stream.WriteTimeout = writeTimeout;

        // Sends the string message
        try
        {
            if (verboseDebugging) Debug.Log("Attempting to write " + message + " to the serial port...");

            // Write the byte array starting from 0 offset and a total length of 1 byte to the serial port
            stream.WriteLine(message);

            if (verboseDebugging) Debug.Log(message + " successfully written to the serial port!");
        }
        catch (Exception ex)
        {
            Debug.Log("Error writing " + message + " to the serial port: " + ex);
        }

        yield return null;
    }

    // This function attempts to close the serial port communication
    public void Close()
    {
        try {
            if (verboseDebugging) Debug.Log("Closing serial port stream to stop communication...");

            // Close the serial port to stop communication
            stream.Close();

            if (verboseDebugging) Debug.Log("Serial port successfully closed!");
        }
        catch (Exception ex)
        {
           Debug.Log("Error closing serial port: " + ex);
        }
    }
    #endregion
}
