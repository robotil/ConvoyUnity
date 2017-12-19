using UnityEngine;
using System.Collections;

public class AntiRollBarConfigurableJoint : MonoBehaviour
{

    public Rigidbody SuspensionL, SuspensionR;

    public float AntiRoll = 5000, initheight;
    Rigidbody rb;
    Transform WheelR, WheelL;
    float wheelRadius = 0.5f; 
    // Use this for initialization
    void Start()
    {   
        WheelR = SuspensionR.transform;
        WheelL = SuspensionL.transform;

        wheelRadius = GetComponentInChildren<SphereCollider>().radius+0.1f;    // we asume no scale manipulations in parents

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // WheelHit hit;
        float travelL = 0;
        float travelR = 0;


        //bool groundedL = Physics.Raycast(WheelL.position, Vector3.down, wheelRadius);
        // if (groundedL)
            travelL =  WheelL.localPosition.y;

        //bool groundedR = Physics.Raycast(WheelR.position, Vector3.down, wheelRadius);
        // if (groundedR)
            travelR =  WheelR.localPosition.y;

        float antiRollForce = (travelL - travelR) * AntiRoll;

        // if (groundedL)
            SuspensionL.AddRelativeForce(Vector3.up * -antiRollForce);
        // if (groundedR)
            SuspensionR.AddRelativeForce(Vector3.up * antiRollForce);
    }
}
