using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShahidWPController : MonoBehaviour {

	public ShahidController shahid;

	[Tooltip("the WP pose, and velocity ( x->WPx_coordinate , y->WPz_coordinate , z->WP_Velocity)")]
    public Vector3 targetPoseAndVel;
	public float targetRadius = 1;
    float targetDiss, targetAzi; 
    public float P_diss=1, P_azimuth=1;

    float LinVelCmd, AngVelCmd;

    public GameObject targetWP_Mark;

    Transform myref;



	
    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
        
        shahid = GetComponent<ShahidController>();

        targetWP_Mark = Instantiate(targetWP_Mark); // creating a private copy
    }
	

	// Update is called once per frame
    void Update()
    {
        Vector3 targetWP_global = new Vector3(targetPoseAndVel.x,0,targetPoseAndVel.y) + 1000*Vector3.up;

        RaycastHit hit;
        Physics.Raycast(targetWP_global,-Vector3.up, out hit);
        targetWP_global = hit.point;


        targetWP_Mark.transform.position = targetWP_global;


        Vector3 targetWP_local = myref.InverseTransformPoint(targetWP_Mark.transform.position);

        targetDiss = targetWP_local.magnitude;
		if (targetDiss < targetRadius)
			targetDiss = 0;	
        LinVelCmd = P_diss * targetDiss;


        LinVelCmd = Mathf.Min(LinVelCmd, targetPoseAndVel.z);


        targetAzi = Mathf.Atan2(targetWP_local.x , targetWP_local.z);

        if ( targetAzi > Mathf.PI) 
        	targetAzi = targetAzi - 2*Mathf.PI;
        else if ( targetAzi < -Mathf.PI)
        	targetAzi = targetAzi + 2*Mathf.PI;

        AngVelCmd = P_azimuth * targetAzi;

        shahid.WalkingCommand = LinVelCmd;
        shahid.TurningCommand = AngVelCmd;
    }
}
 