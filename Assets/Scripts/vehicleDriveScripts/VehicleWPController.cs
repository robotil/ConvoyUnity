using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class VehicleWPController : MonoBehaviour
{
    public VehicleVelocityController VelController;


    public float targetX_glob, targetZ_glob;
    public float targetX_loc, targetZ_loc;


    
    float targetY;

    float targetDiss, targetAzi; 
    public float P_diss=1, P_azimuth=1;
    //float curentPose, curentAzimuth;

    float LinVelCmd, AngVelCmd;


    public GameObject targetWP;

    Rigidbody rb;
    Transform myref;// target;


    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
        rb = GetComponent<Rigidbody>();
        
        VelController = GetComponent<VehicleVelocityController>();

        targetWP = GameObject.Find("targetWP");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetWP_global = new Vector3(targetX_glob,0,targetZ_glob) + 1000*Vector3.up;

        RaycastHit hit;
        Physics.Raycast(targetWP_global,-Vector3.up, out hit);
        targetWP_global = hit.point;

        targetY = targetWP_global.y;

        targetWP.transform.position = targetWP_global;

        // target.position = targetWP_global;


        Vector3 targetWP_local = myref.InverseTransformPoint(targetWP.transform.position);


        targetX_loc = targetWP_local.x;
        targetZ_loc = targetWP_local.z;


        targetDiss = targetWP_local.magnitude;
        LinVelCmd = P_diss * targetDiss;


        targetAzi = Mathf.Atan2(targetWP_local.x , targetWP_local.z);

        if ( targetAzi > Mathf.PI) 
        	targetAzi = targetAzi - 2*Mathf.PI;
        else if ( targetAzi < -Mathf.PI)
        	targetAzi = targetAzi + 2*Mathf.PI;

        AngVelCmd = P_azimuth * targetAzi;


        VelController.targeLinearVel = LinVelCmd;
        VelController.targetAngularVel = AngVelCmd;
    }


}
