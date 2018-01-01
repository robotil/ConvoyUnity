﻿using UnityEngine;
using System.Collections;
//This code is an Ackermann vehicle using Configurable joints for physcs and steering and rigidbody torque.
public class VehicleSteering : MonoBehaviour
{

    public bool ManualInput = true;
    public float VehicleWidth = 2, VehicleLength = 3, MaxSteering=0.4f;

    public float steeringCommand = 0; 
    

    Transform myref;
    public ConfigurableJoint[] rightWheelsSteer, leftWheelsSteer;



    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ManualInput)
        {
            steeringCommand = Input.GetAxis("Horizontal");
            steeringCommand = Mathf.Clamp(steeringCommand, -1, 1);

            steeringCommand = MaxSteering * Input.GetAxis("Horizontal");  

        }
        Apply(steeringCommand);
    }


    public void Apply(float SteerCommand)
    {
        float right_steer = 0, left_steer =0;
        float Len = VehicleLength; 
        float Wid = VehicleWidth;


        if (SteerCommand != 0)
        {
        if ( SteerCommand < 0 )  // turning left - left wheel is the iner one
            {
            float R = Len/Mathf.Atan(-SteerCommand);
            right_steer = Mathf.Atan(Len/(R-Wid/2)); 
            left_steer  = Mathf.Atan(Len/(R+Wid/2)); 
            }
        else // turning right - right wheel is the iner one
            {
            float R = Len/Mathf.Atan(SteerCommand);
            right_steer = -Mathf.Atan(Len/(R+Wid/2)); 
            left_steer  = -Mathf.Atan(Len/(R-Wid/2)); 
            }
        }

        for (int i=0; i<rightWheelsSteer.Length; i++)
            {
            rightWheelsSteer[i].targetRotation = Quaternion.Euler(Mathf.Rad2Deg * right_steer , 0, 0);
            leftWheelsSteer[i].targetRotation = Quaternion.Euler(Mathf.Rad2Deg * left_steer , 0, 0);
            }

    }





}
