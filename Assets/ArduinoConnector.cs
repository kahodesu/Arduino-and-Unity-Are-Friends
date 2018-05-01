/* Modded version of ArduinoConnector by Alan Zucconi
 * http://www.alanzucconi.com/?p=2979
 * changed data read and written to be a byte. 
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;


public class ArduinoConnector : MonoBehaviour {


    /* The serial port where the Arduino is connected. */
    [Tooltip("The serial port where the Arduino is connected")]
	public string port = "/dev/cu.usbmodem1411";
    /* The baudrate of the serial port. */
    [Tooltip("The baudrate of the serial port")]
    public int baudrate = 115200;
    private SerialPort stream;
    
	byte[] b = new byte[1]; //this is a byte array called b with one item.

    public void Open () {
        // Opens the serial port
        stream = new SerialPort(port, baudrate);
        stream.ReadTimeout = 50;
		//Debug.Log("opening...");
        stream.Open();
		
        //this.stream.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
    }
    

    public void WriteToArduino(byte message)
     {
       //  Debug.Log(message);
       	 b[0] = message; //stick byte data into byte array b
         stream.Write(b, 0, 1); //sends b with some other info.
         stream.BaseStream.Flush(); 
    }

    public int ReadFromArduino(int timeout)
    {
        stream.ReadTimeout = timeout;
        try
        {
			//Debug.Log("reading...");
            return stream.ReadByte(); //return whatever is read here
        }
        catch (TimeoutException)
        {            
            return 0;
        }
    }
    
    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            // A single read attempt
            try
            {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield return null;
            } else
                yield return new WaitForSeconds(0.05f);

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }

    public void Close()
    {
        stream.Close();
    }
}