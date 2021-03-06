﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShahidActivator : MonoBehaviour {

	public ShahidWPController shahidTargetWP;

    public GameObject targetVehicle;
	
	

	public float activationRadius;
	public float shahidVel = 3;
	bool wasActivated = false;

	public float targetVehicleDist;
    Transform myref;


	// Use this for initialization
	void Start () {
		myref = gameObject.transform;    
        shahidTargetWP = GetComponent<ShahidWPController>();
		Debug.Log("ShahidActivator:targetVehicle="+targetVehicle.name);
	}
	

	// Update is called once per frame
	void Update () 
	{
		Vector3 targetWP_local = myref.InverseTransformPoint(targetVehicle.transform.position);
        targetVehicleDist = targetWP_local.magnitude;
        //Debug.Log("targetVehicleDist:"+targetVehicleDist.ToString());

		if (! wasActivated)
		{
			if (targetVehicleDist < activationRadius) 
			{
				Vector3 targetPos = targetVehicle.transform.position;
				Vector3 targetPredictedPose = targetPos; 
#if MOVINGSHAHID
				shahidTargetWP.shahidTargetPoseAndVel = new Vector3(targetPredictedPose.x, targetPredictedPose.z, shahidVel); 	
#endif
				wasActivated = true;
				Debug.Log("ShahidActivator:Activated at distance="+targetVehicleDist.ToString()+" Goal is: "+shahidTargetWP.shahidTargetPoseAndVel.ToString());
			}
		}

	}
}
