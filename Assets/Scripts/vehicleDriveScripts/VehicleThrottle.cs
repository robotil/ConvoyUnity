﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VehicleThrottle : MonoBehaviour  
{

    public bool ManualInput = true;
    public bool BreakOnNullThrottle = true;
    public int gear=1;
    public float MaxTorque = 1000000;
    public float  MaxBreakingTorque = 1000;




    [Tooltip("Assign the wheels you want motorized here with a rotation axis set as X.")]
    public Rigidbody[] Wheels; //Assign here the wheels you want motorized
    Transform myref;
    Rigidbody rb;


    public float throttleCommand=0, BreakCommand=0, ForwardVel;
    List<float> wheelRotationVel = new List<float>();
    List<float> wheelAppliedTrq = new List<float>();
    

    

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

        
        ForwardVel = myref.InverseTransformVector(rb.velocity).z;
        if (ManualInput)
        {

            throttleCommand = Input.GetAxis("Vertical") > 0 ? Input.GetAxis("Vertical") : 0;
            throttleCommand = Mathf.Clamp(throttleCommand, 0, 1);

            BreakCommand = Input.GetAxis("Vertical") < 0 ? -Input.GetAxis("Vertical") : 0;
            BreakCommand = Mathf.Clamp(BreakCommand, 0, 1);


            if (Input.GetButtonDown("GearDown")) gear--;
            if (Input.GetButtonDown("GearUp")) gear++;
            gear = Mathf.Clamp(gear, -1, 1);
        }

        if (BreakOnNullThrottle && throttleCommand == 0)
        {
            BreakCommand = 0.2f;
        }

        Apply(throttleCommand,  BreakCommand);
        throttleCommand = 0; BreakCommand = 0;
    }


    public void Apply(float Throttle, float Break)
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



    // public override void Drive(float T, float S)
    // {
    //     throttle = T;
    //     SteeringAngleCommand = Mathf.Clamp(S, -MaxSteering, MaxSteering);
    //     BreakPedal = T > 0 ? 0 : Mathf.Clamp(-T, 0, 1);
    //     steeringSpeed = MaxSteeringSpeed;
    // }
    // public override void Drive(float T, float S, float breaks, float steerSpeed)
    // {
    //     throttle = T;
    //     SteeringAngleCommand = Mathf.Clamp(S, -MaxSteering, MaxSteering);
    //     BreakPedal = breaks;
    //     steeringSpeed = Mathf.Clamp(steerSpeed, 0, MaxSteeringSpeed);
    // }


}
