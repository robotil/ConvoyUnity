﻿using UnityEngine;
using System.Collections;
//This code is an Ackermann vehicle using Configurable joints for physcs and steering and rigidbody torque.
public class VehicleSteering : MonoBehaviour
{

    public bool ManualInput = true;
    public float VehicleWidth = 2, VehicleLength = 3, MaxSteering=0.4f;

    public float steeringCommand = 0, left_steer = 0, right_steer = 0;
   

    Transform myref;
    Rigidbody rb;
    [Tooltip("Assign the steering configurable joints in a Left Wheel, Right Wheel, Left Wheel, Right wheel... manner With the main axis set as X")]
    public ConfigurableJoint[] steering; //Set steering joints as Left Wheel, Right Wheel, Left Wheel, Right wheel via the inspector

    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
        rb = GetComponent<Rigidbody>();
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


    public void Apply(float SteerCommand){

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

        for (int i = 0; i < steering.Length/2; i=+2)
            {
            steering[i].targetRotation = Quaternion.Euler(Mathf.Rad2Deg * right_steer , 0, 0);
            steering[i+1].targetRotation = Quaternion.Euler(Mathf.Rad2Deg * left_steer, 0, 0);
            }
    }





}
