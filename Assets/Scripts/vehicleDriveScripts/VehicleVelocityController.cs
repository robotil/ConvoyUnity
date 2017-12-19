using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class VehicleVelocityController : MonoBehaviour
{
    public VehicleThrottle vehicleThrot;
    public VehicleSteering vehicleSteer;


    public float targeLinearVel, Plin;
    float curentLinearVel, ErrorLinVel, throttelCmd;
    public float targetAngularVel, Pang;
    float curentAngularVel, ErrorAngVel, SteerCmd;

    
    float maxSteerAng;

    Rigidbody rb;
    Transform myref;


    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
        rb = GetComponent<Rigidbody>();

        curentLinearVel =  rb.velocity.z; 
        curentAngularVel = rb.angularVelocity.y; 
        

        vehicleThrot = GetComponent<VehicleThrottle>();
        vehicleSteer = GetComponent<VehicleSteering>();

        maxSteerAng = vehicleSteer.MaxSteering;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        curentLinearVel =  myref.InverseTransformVector(rb.velocity).z;   
        curentAngularVel =  myref.InverseTransformVector(rb.angularVelocity).y;   


        ErrorLinVel = targeLinearVel - curentLinearVel;
        throttelCmd = Plin * ErrorLinVel;
        vehicleThrot.throttleCommand = throttelCmd;


        ErrorAngVel = targetAngularVel - curentAngularVel;
        SteerCmd = Pang * ErrorAngVel;
        vehicleSteer.steeringCommand = SteerCmd;

    }


}
