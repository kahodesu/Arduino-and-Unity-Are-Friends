using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;


public class SphereScript : MonoBehaviour
{
	public Renderer rend; //create an instance of object Renderer called rend

	void Start()
	{		
    	rend = GetComponent<Renderer>(); //to access the renderer of the object
    	rend.material.SetColor("_Color",Color.white); //set the color of the cube to BLUE
    }

    void Update()
	{

    }
}
