using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class VehicleVelocityController : MonoBehaviour
{
    public VehicleThrottle vehicleThrot;
    public VehicleSteering vehicleSteer;


    public float targeLinearVel=0, P_lin=1;
    float curentLinearVel, ErrorLinVel, throttelCmd;
    public float targetAngularVel=0, P_ang=1;
    float curentAngularVel, ErrorAngVel, SteerCmd;


    Rigidbody rb;
    Transform myref;


    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
        rb = GetComponent<Rigidbody>();

        // curentLinearVel =  rb.velocity.z; 
        // curentAngularVel = rb.angularVelocity.y; 
        
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
        vehicleThrot.throttleCommand = throttelCmd;


        ErrorAngVel = targetAngularVel - curentAngularVel;
        SteerCmd = P_ang * ErrorAngVel;
        vehicleSteer.steeringCommand = SteerCmd;

    }


}
