using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    // A variable we will update based the values we receive from the arduino controller, it keeps track of the last state
    int lastPositionChoice = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure that the arduino controller has valid values from the arduino (using .Available()) and that they consist of the 3 different values we were expecting
        if (ArduinoController.instance.Available() && ArduinoController.instance.lastArduinoValues.Length == 3)
        {
            // If the first of the three values sent by the arduino is 1, move the sphere to the left side of the screen
            if (ArduinoController.instance.lastArduinoValues[0] == 1)
            {
                this.gameObject.transform.position = new Vector3(-5, 0, 0);

                // Also update the last position choice if it wasn't this one (the first value == 1) originally
                // This also ensures that the same message is only sent to the arduino once when the state first changes
                if (lastPositionChoice != 0)
                {
                    // Send a message back to the arduino
                    // The arduino will use this to turn on a corresponding LED
                    ArduinoController.instance.WriteByte(0);
                    

                    lastPositionChoice = 0;
                }
            }
            // If the second of the three values sent by the arduino is 1, move the sphere to the middle of the screen
            else if (ArduinoController.instance.lastArduinoValues[1] == 1)
            {
                this.gameObject.transform.position = new Vector3(0, 0, 0);

                // Also update the last position choice if it wasn't this one (the second value == 1) originally
                // This also ensures that the same message is only sent to the arduino once when the state first changes
                if (lastPositionChoice != 1)
                {
                    // Send a message back to the arduino
                    // The arduino will use this to turn on a corresponding LED
                    ArduinoController.instance.WriteByte(1);
                    lastPositionChoice = 1;
                }
            }
            // If the third of the three values sent by the arduino is 1, move the sphere to the right side of the screen
            else if (ArduinoController.instance.lastArduinoValues[2] == 1)
            {
                this.gameObject.transform.position = new Vector3(5, 0, 0);

                // Also update the last position choice if it wasn't this one (the third value == 1) originally
                // This also ensures that the same message is only sent to the arduino once when the state first changes
                if (lastPositionChoice != 2)
                {
                    // Send a message back to the arduino
                    // The arduino will use this to turn on a corresponding LED
                    ArduinoController.instance.WriteByte(2);
                    lastPositionChoice = 2;
                }
            }
        }
    }
}
