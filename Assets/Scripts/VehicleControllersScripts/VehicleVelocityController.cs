using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class VehicleVelocityController : MonoBehaviour
{
    public Vehicle vehicleControl;

    public float targeLinearVel=0, P_lin=1;
    float curentLinearVel, ErrorLinVel; 
    public float targetAngularVel=0, P_ang=1;
    float curentAngularVel, ErrorAngVel;
    public float throttelCmd, SteerCmd;


    Rigidbody rb;
    Transform myref;


    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
        rb = GetComponent<Rigidbody>();
        
        vehicleControl = GetComponent<Vehicle>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        curentLinearVel =  myref.InverseTransformVector(rb.velocity).z;   
        curentAngularVel =  myref.InverseTransformVector(rb.angularVelocity).y;   


        ErrorLinVel = targeLinearVel - curentLinearVel;
        throttelCmd = P_lin * ErrorLinVel;
        if (throttelCmd >=0) 
            vehicleControl.throttleCommand = throttelCmd;
        else
            vehicleControl.BreakCommand = -throttelCmd;


        ErrorAngVel = targetAngularVel - curentAngularVel;
        SteerCmd = P_ang * ErrorAngVel;
        vehicleControl.steeringCommand = SteerCmd;

    }


}
