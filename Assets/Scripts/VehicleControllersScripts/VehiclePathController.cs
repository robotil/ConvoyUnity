using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class VehiclePathController : MonoBehaviour
{
    public VehicleWPController WPController;

  // Read the waypoints from unity interface by default. They will be changed if there is a scenario afterward
  [Tooltip("Insert list of WP for the path: pose, and velocity ( x->WPx_coordinate , y->WPz_coordinate , z->WP_Velocity)")]
    public List<Vector3> PathWPs_PosesAndVels=new List<Vector3>();
  [Tooltip("Position of current way point in the path")]
    public int currentWPRange;

    
  [Tooltip("WP considered as reach at this distance")]
    public float WP_ReachRadius;
    public GameObject PathWP_Mark;

    Vector3 curentTargetWP_PoseAndVel;
    Vector3 curentVehicle_PoseAndVel;
    float DissToCurrentWP;
    List<GameObject> PathWPs=new List<GameObject>();

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
        currentWPRange = 0;
        
        WP_ReachRadius = 5;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        pathConstraction();

        curentTargetWP_PoseAndVel = PathWPs_PosesAndVels[currentWPRange];
        curentVehicle_PoseAndVel = new Vector3(myref.position.x, myref.position.z, myref.InverseTransformVector(rb.velocity).z);


        Vector3 curentTeagetWP_glob = PathWPs[currentWPRange].transform.position;
        Vector3 curentTeagetWP_loc = myref.InverseTransformPoint(curentTeagetWP_glob);
        DissToCurrentWP = curentTeagetWP_loc.magnitude;

        if (DissToCurrentWP < WP_ReachRadius)
        {
            if (currentWPRange < (PathWPs_PosesAndVels.Count-1) )
                 currentWPRange++;
        }

        WPController.targetPoseAndVel = PathWPs_PosesAndVels[currentWPRange];
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
