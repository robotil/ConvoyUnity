using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShahidWPController : MonoBehaviour {

	public ShahidController shahid;

	[Tooltip("the WP pose, and velocity ( x->WPx_coordinate , y->WPz_coordinate , z->WP_Velocity)")]
    public Vector3 shhidTargetPoseAndVel;
	public float targetRadius = 1;
    float targetDiss, targetAzi; 
    public float P_diss=1, P_azimuth=1;

    float LinVelCmd, AngVelCmd;

    public GameObject targetWP_Mark;
    terrainAttachment TargetMarkPos;

    Transform myref;



	
    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
        
        shahid = GetComponent<ShahidController>();
        string shahidName = gameObject.name;

        targetWP_Mark = Instantiate(targetWP_Mark); // creating a private copy
        targetWP_Mark.name = shahidName + "Target" ;

        TargetMarkPos = targetWP_Mark.GetComponent<terrainAttachment>();

        shhidTargetPoseAndVel = new Vector3(myref.position.x+targetRadius,myref.position.z+targetRadius,0);
    }
	

	// Update is called once per frame
    void Update()
    {

        TargetMarkPos.moveTo(new Vector3(shhidTargetPoseAndVel.x,shhidTargetPoseAndVel.y,0)); 

        Vector3 targetWP_local = myref.InverseTransformPoint(targetWP_Mark.transform.position);

        targetDiss = targetWP_local.magnitude;
		if (targetDiss < targetRadius)
			targetDiss = 0;	
        LinVelCmd = P_diss * targetDiss;


        LinVelCmd = Mathf.Min(LinVelCmd, shhidTargetPoseAndVel.z);


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
 