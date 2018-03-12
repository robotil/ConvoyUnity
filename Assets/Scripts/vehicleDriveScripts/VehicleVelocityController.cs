using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class VehicleVelocityController : MonoBehaviour
{
    public VehicleThrottle vehicleThrot;
    public VehicleSteering vehicleSteer;


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
        
        vehicleThrot = GetComponent<VehicleThrottle>();
        vehicleSteer = GetComponent<VehicleSteering>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        curentLinearVel =  myref.InverseTransformVector(rb.velocity).z;   
        curentAngularVel =  myref.InverseTransformVector(rb.angularVelocity).y;   


        ErrorLinVel = targeLinearVel - curentLinearVel;
        throttelCmd = P_lin * ErrorLinVel;
        if (throttelCmd >=0) 
            vehicleThrot.throttleCommand = throttelCmd;
        else
            vehicleThrot.BreakCommand = -throttelCmd;



        ErrorAngVel = targetAngularVel - curentAngularVel;
        SteerCmd = P_ang * ErrorAngVel;
        vehicleSteer.steeringCommand = SteerCmd;

    }


}
