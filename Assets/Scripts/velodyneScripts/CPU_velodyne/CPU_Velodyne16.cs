//Written by Yossi Cohen <yossicohen2000@gmail.com>

using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System;

public class Velodyne16 : MonoBehaviour
{
    public float MaxRange = 70;
    public float HorFOV = 360, VerFOV = 20, StartVerticalAngle = -10;
          
          
    [Range(0.1f, 2f)]
    public float HorRes = 1f;
    float verRes;
    public int ScaningFreqHZ = 10;
    public int ScaningPlaines = 16;

    public bool DebugDraw = false,  DebugDrawDots = true;
    public float DrawTime = 0.1f;

    public bool InterpolateLocation = true;


    float physics_StepTime, horFOV_ScanTime;
    int ScanColumnsPerPhysicStep;
 
    float horCurrentAngle;
    Transform myref, SensorRotator, emitter;
    Rigidbody rb;
    private VelodyneWrapper vc;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        myref = transform;
        SensorRotator = myref.Find("Laser Sensor");
        emitter = SensorRotator.Find("Emitter");

        horCurrentAngle = 0;
        vc = new VelodyneWrapper(192, 168, 1, 77, 2368, 200, 37, 22, 10, 16);
        vc.Run();
    }

    // Update is called once per frame
    void FixedUpdate() {
        physics_StepTime =  Time.fixedDeltaTime;
        horFOV_ScanTime = 1.0f/ScaningFreqHZ;

        ScanColumnsPerPhysicStep = (int)( (HorFOV/HorRes) * (physics_StepTime/horFOV_ScanTime) );  

        verRes = VerFOV/(ScaningPlaines-1);


        if (horCurrentAngle > HorFOV) { //completed horizontal scan
            horCurrentAngle = 0; 
            SensorRotator.localEulerAngles = new Vector3(0, 0, 0);
        } 

        Vector3 ScannerVel = myref.InverseTransformVector(rb.velocity);
        Vector3 scanerPos = myref.position;
        Vector3 ScanerLinearStep = ScannerVel * physics_StepTime;
        int timeFixer = 0;
        for (int i = 0; i < ScanColumnsPerPhysicStep; i++) { // multiple horizontal scans in 1 physics step in order to achieve the full range in the desired rate
            if (InterpolateLocation) {
                  scanerPos = scanerPos + ScanerLinearStep * i/ScanColumnsPerPhysicStep;
            }

            if (horCurrentAngle > HorFOV) {
                horCurrentAngle = 0; 
                SensorRotator.localEulerAngles = new Vector3(0, 0, 0);
            }

            for (int j = ScaningPlaines - 1; j >= 0 ; j--) { //the lazer column 
                float verCurentAngle = (StartVerticalAngle + j * verRes);
                emitter.localEulerAngles = new Vector3(verCurentAngle, 0, 0);


                Vector3 shootLaserDir = (emitter.forward);
                RaycastHit hit;
                if (Physics.Raycast(scanerPos, shootLaserDir, out hit, MaxRange)) {
                    vc.SetChannel(hit.distance, 0);
                    if (DebugDraw) {
                        Debug.DrawLine(hit.point, hit.point + 0.1f * Vector3.up, Color.red, DrawTime, true);
                    }
                }
                else {
                     vc.SetChannel(0, 0);
                }
            }
            vc.SetAzimuth(horCurrentAngle);
            double timeStamp = Time.fixedTime * 1000000.0 + timeFixer++;
            vc.SetTimeStamp((int)timeStamp);
            vc.SendData();

            horCurrentAngle = horCurrentAngle + HorRes;
            SensorRotator.localEulerAngles = new Vector3(0, horCurrentAngle, 0);
        }

    }
}