using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class VehicleFollowerPathController : MonoBehaviour
{
    public VehiclePathController PathController;

    public Transform leaderVehicle;
    public float followerSpeed;

    public float leaderPointsDisstances; 

    public bool folloActive; 

    Vector2 targetPrevPose; 

    // Use this for initialization
    void Start()
    {

    PathController = GetComponent<VehiclePathController>();
    targetPrevPose = new Vector2(0,0);
    leaderPointsDisstances = 5; 
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if ( folloActive )
            {   
                Vector2 targetCurentPose = new Vector2( leaderVehicle.position.x, leaderVehicle.position.z);
                float diss = Vector2.Distance(targetCurentPose , targetPrevPose);
                
                if ( diss > leaderPointsDisstances )
                {
                   Vector3 nextFollowPoseAndSpeed = new Vector3( leaderVehicle.position.x, leaderVehicle.position.z, followerSpeed);
                   PathController.PathWPs_PosesAndVels.Add(nextFollowPoseAndSpeed); 
                   targetPrevPose = targetCurentPose;
                }
            }   
    }

}
