using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShahidActivator : MonoBehaviour {

	public ShahidWPController shahidTargetWP;

    public GameObject targetVehicle;
	
	

	public float activetionRadius;
	public float shahidVel = 3;
	bool wasActivated = false;

	public float targetVehicleDist;
    Transform myref;


	// Use this for initialization
	void Start () {
		myref = gameObject.transform;    
        shahidTargetWP = GetComponent<ShahidWPController>();

	}
	

	// Update is called once per frame
	void Update () 
	{
		Vector3 targetWP_local = myref.InverseTransformPoint(targetVehicle.transform.position);
        targetVehicleDist = targetWP_local.magnitude;

		if (! wasActivated)
		{
			if (targetVehicleDist < activetionRadius) 
			{
				Vector3 targetPos = targetVehicle.transform.position;
				Vector3 targetPredictedPose = targetPos; 
				
				shahidTargetWP.shhidTargetPoseAndVel = new Vector3(targetPredictedPose.x, targetPredictedPose.z, shahidVel); 	
				wasActivated = true;
			}
		}

	}
}
