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
	void Update () {

		
		
		Vector3 targetWP_local = myref.InverseTransformPoint(targetVehicle.transform.position);

        targetVehicleDist = targetWP_local.magnitude;
		

		if (( targetVehicleDist < activetionRadius) && ( ! wasActivated ) )
		{
			Rigidbody target_rb = targetVehicle.GetComponent<Rigidbody>();


			float physics_StepTime =  Time.fixedDeltaTime;
			Vector3 targetVel = target_rb.velocity; 
        	Vector3 targetPos = targetVehicle.transform.position;

        	Vector3 targetPredictedPose = targetPos + targetVel * (activetionRadius/shahidVel);
			
			shahidTargetWP.targetPoseAndVel = new Vector3(targetPredictedPose.x, targetPredictedPose.z, shahidVel); 	

			wasActivated = true;
		}
	}
}
