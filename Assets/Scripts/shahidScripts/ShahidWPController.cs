using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShahidWPController : MonoBehaviour {

	public ShahidController shahid;

	[Tooltip("the WP pose, and velocity ( x->WPx_coordinate , y->WPz_coordinate , z->WP_Velocity)")]
    public Vector3 shahidTargetPoseAndVel;
	public float targetRadius = 1;
    float targetDist, targetAzi; 
    public float P_dist=1, P_azimuth=1;

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

        shahidTargetPoseAndVel = new Vector3(myref.position.x,myref.position.z,0);
        

        Debug.Log("ShahidWPController::Start - targetRadius is:"+targetRadius.ToString()+" shahidTargetPoseAndVel:"+shahidTargetPoseAndVel); //+myref.position);
    }
	

	// Update is called once per frame
    void Update()
    {
#if MOVINGSHAHID
        TargetMarkPos.moveTo(new Vector3(shahidTargetPoseAndVel.x+targetRadius, shahidTargetPoseAndVel.y+targetRadius, 0)); 
#endif
        Vector3 targetWP_local = myref.InverseTransformPoint(targetWP_Mark.transform.position);

        targetDist = targetWP_local.magnitude; //Length of the vector
        //Debug.Log("targetDist is:"+targetDist.ToString());

		if (targetDist < targetRadius){
           // Debug.Log("00000 targetDist is zero:"+targetDist.ToString());
        	targetDist = 0;	
        }
        LinVelCmd = P_dist * targetDist;

        LinVelCmd = Mathf.Min(LinVelCmd, shahidTargetPoseAndVel.z);

        targetAzi = Mathf.Atan2(targetWP_local.x , targetWP_local.z);

        if ( targetAzi > Mathf.PI) 
        	targetAzi = targetAzi - 2*Mathf.PI;
        else if ( targetAzi < -Mathf.PI)
        	targetAzi = targetAzi + 2*Mathf.PI;

        AngVelCmd = P_azimuth * targetAzi;

#if MOVINGSHAHID
        shahid.WalkingCommand = LinVelCmd;
        shahid.TurningCommand = AngVelCmd;
        Debug.Log("LinVelCmd is:"+LinVelCmd.ToString() + " and AngVelCmd is:" + AngVelCmd.ToString());
#endif
    }
}
 