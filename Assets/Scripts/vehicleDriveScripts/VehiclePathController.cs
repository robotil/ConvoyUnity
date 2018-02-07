﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class VehiclePathController : MonoBehaviour
{
    public VehicleWPController WPController;

  
    public List<Vector3> PathWPs_PosesAndVels=new List<Vector3>();
    public int curentWP;

    Vector3 curentTeagetWP_PoseAndVel;
    Vector3 curentVehicle_PoseAndVel;


    float DissToCurentWP;

    public float WP_ReachRadius;


    List<GameObject> PathWPs=new List<GameObject>();
    public GameObject PathWP_Mark;

    string VehicleName;


    Transform myref;
    Rigidbody rb;


    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
        rb = GetComponent<Rigidbody>();
        VehicleName = gameObject.name;

        WPController = GetComponent<VehicleWPController>();

        PathWPs_PosesAndVels.Add(new Vector3(myref.position.x, myref.position.z, 0));
        curentWP = 0;
        
        WP_ReachRadius = 5;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        pathConstraction();

        curentTeagetWP_PoseAndVel = PathWPs_PosesAndVels[curentWP];
        curentVehicle_PoseAndVel = new Vector3(myref.position.x, myref.position.z, myref.InverseTransformVector(rb.velocity).z);


        Vector3 curentTeagetWP_glob = PathWPs[curentWP].transform.position;
        Vector3 curentTeagetWP_loc = myref.InverseTransformPoint(curentTeagetWP_glob);
        DissToCurentWP = curentTeagetWP_loc.magnitude;

        if (DissToCurentWP < WP_ReachRadius)
        {
            if (curentWP < (PathWPs_PosesAndVels.Count-1) )
                 curentWP++;
        }

        WPController.targetPoseAndVel = PathWPs_PosesAndVels[curentWP];
    }
    

    void pathConstraction()
    {
            for (int WP_i=0 ; WP_i<PathWPs_PosesAndVels.Count ; WP_i++) 
            {
                if ( WP_i >= PathWPs.Count )
                    {
                        GameObject newWP_Object = Instantiate(PathWP_Mark);
                        newWP_Object.name = VehicleName + "_PathWP_" + WP_i.ToString();
                        PathWPs.Add(newWP_Object);
                    }

            terrainAttachment WPpos = PathWPs[WP_i].GetComponent<terrainAttachment>();
            WPpos.moveTo(new Vector3(PathWPs_PosesAndVels[WP_i].x,PathWPs_PosesAndVels[WP_i].y,0));
            }
    }



}
