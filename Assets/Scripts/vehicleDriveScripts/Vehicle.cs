﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vehicle : MonoBehaviour  
{

    public bool ManualInput = true;
    public bool BreakOnNullThrottle = true;
    public int gear=1;
    public float MaxTorque = 1000000;
    public float  MaxBreakingTorque = 1000;

    public float VehicleWidth = 2, VehicleLength = 3, MaxSteering=0.4f;


    [Tooltip("Assign the wheels you want motorized here with a rotation axis set as X.")]
    public Rigidbody[] Wheels; //Assign here the wheels you want motorized
    Transform myref;
    Rigidbody rb;


    public float throttleCommand=0, steeringCommand = 0, BreakCommand=0, ForwardVel;
    List<float> wheelRotationVel = new List<float>();
    List<float> wheelAppliedTrq = new List<float>();
    
    public ConfigurableJoint[] rightWheelsSteer, leftWheelsSteer;



    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
        rb = GetComponent<Rigidbody>();

        for (int i = 0; i < Wheels.Length; i++)
           {
            wheelRotationVel.Add(0);
            wheelAppliedTrq.Add(0);            
           } 
  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ManualInput)
        {
            throttleCommand = Input.GetAxis("Vertical") > 0 ? Input.GetAxis("Vertical") : 0;
            throttleCommand = Mathf.Clamp(throttleCommand, 0, 1);

            BreakCommand = Input.GetAxis("Vertical") < 0 ? -Input.GetAxis("Vertical") : 0;
            BreakCommand = Mathf.Clamp(BreakCommand, 0, 1);

            if (Input.GetButtonDown("GearDown")) gear--;
            if (Input.GetButtonDown("GearUp")) gear++;
            gear = Mathf.Clamp(gear, -1, 1);

            steeringCommand = Input.GetAxis("Horizontal");
            steeringCommand = Mathf.Clamp(steeringCommand, -1, 1);

            steeringCommand = MaxSteering * Input.GetAxis("Horizontal");  
        }

        if (BreakOnNullThrottle && throttleCommand == 0)
        {
            BreakCommand = 0.2f;
        }

        ApplyTrottleAndBreaks(throttleCommand,  BreakCommand);
        ApplySteering(steeringCommand);


        throttleCommand = 0; BreakCommand = 0;
    }


    public void ApplyTrottleAndBreaks(float Throttle, float Break)
    {
        for (int i = 0; i < Wheels.Length; i++)
           {
            if ( Break > 0.1f ) 
            {
                Wheels[i].angularDrag = Break * MaxBreakingTorque; //wheel lock
            }
            else
            {    
            wheelRotationVel[i] = Wheels[i].GetComponent<HingeJoint>().velocity;
            float wheelPower = MaxTorque * Throttle;
            if (Mathf.Abs(wheelRotationVel[i]) <= 0.1 )
                {
               wheelAppliedTrq[i] =  wheelPower * gear;
                }
            else
                {
                wheelAppliedTrq[i] = wheelPower / Mathf.Abs(wheelRotationVel[i]) * gear;      
                if (wheelRotationVel[i] > 0.1 )
                    {
                    wheelAppliedTrq[i] =  Mathf.Clamp(wheelAppliedTrq[i], -MaxBreakingTorque, wheelPower/wheelRotationVel[i]);
                    }
                else if (wheelRotationVel[i] < -0.1 )
                    {
                    wheelAppliedTrq[i] =  Mathf.Clamp(wheelAppliedTrq[i], wheelPower/wheelRotationVel[i], MaxTorque);
                    }
                }

            Wheels[i].angularDrag = 0; //remove wheel lock
            Wheels[i].AddRelativeTorque(new Vector3(wheelAppliedTrq[i], 0, 0), ForceMode.Force);
            }
        }
    }


    public void ApplySteering(float SteerCommand)
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





    public void toggleManual(bool manual)
    {
        ManualInput = manual;
    }
}
