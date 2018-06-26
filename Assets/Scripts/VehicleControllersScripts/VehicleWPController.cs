using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class VehicleWPController : MonoBehaviour
{
    public VehicleVelocityController VelController;
    terrainAttachment WPpos;


    [Tooltip("the WP pose, and velocity ( x->WPx_coordinate , y->WPz_coordinate , z->WP_Velocity)")]
    public Vector3 targetPoseAndVel;

    float targetDiss, targetAzi; 
    public float P_diss=1, P_azimuth=1;

    float LinVelCmd, AngVelCmd;


    public GameObject targetWP_Mark;
    GameObject TargetWP;

    Transform myref;



    // Use this for initialization
    void Start()
    {
       myref = gameObject.transform;
        
       VelController = GetComponent<VehicleVelocityController>();


       TargetWP = Instantiate(targetWP_Mark);
       TargetWP.name = gameObject.name + "TargetWP";
       
       //For displaying red signs on the terrain
       WPpos = TargetWP.GetComponent<terrainAttachment>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        WPpos.moveTo(new Vector3(targetPoseAndVel.x,targetPoseAndVel.y,0));

        Vector3 targetWP_local = myref.InverseTransformPoint(TargetWP.transform.position);

        targetDiss = targetWP_local.magnitude;
        LinVelCmd = P_diss * targetDiss;

        LinVelCmd = Mathf.Min(LinVelCmd, targetPoseAndVel.z);


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
