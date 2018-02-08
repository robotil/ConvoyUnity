using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GPU_Velodyne16 : MonoBehaviour
{
    VelodyneWrapper vel16ICDinterface;
    GPULidar Sensor;
    Camera depthCam;
    Texture2D RangesSamples;


    public float RotateFrequency = 10;
    public bool Rotate;
    public float AngularResolution = 0.2f, LowerAngle = -10, HigherAngle = 10;
    public int Channels = 16, SuperSample = 1;
    public float MeasurementRange = 120f, MinMeasurementRange = 0.2f,  MeasurementAccuracy = 0.02f;

    public bool sendDataOnICD;

    int ResWidth, ResHeight;
    float horizontalFOV, verticalFOV;
    int ColumnsPerPhysStep;
    float verticalAngularResolution;
    float cameraRotationAngle;

    public bool DrawLidar; // for digug
    public float drawSize = 0.1f, drawTime = 0.1f;
    public Color drawColor = Color.red;


    void Awake()
    {
        Sensor = GetComponentInChildren<GPULidar>();
        depthCam = Sensor.GetComponent<Camera>();

        // Calculation of FOV 
        verticalFOV = HigherAngle - LowerAngle;
        verticalAngularResolution = verticalFOV / (Channels - 1f);
        horizontalFOV = Time.fixedDeltaTime * 360 * RotateFrequency / SuperSample;

        // Calculation of the Camera Projection Mat 
        Matrix4x4 projMat = depthCam.projectionMatrix;
        float horizontalMval = 1 / (Mathf.Tan((horizontalFOV / 2) * Mathf.Deg2Rad));
        float verticalMval = 1 / (Mathf.Tan((verticalFOV / 2) * Mathf.Deg2Rad));
        projMat[0, 0] = horizontalMval;
        projMat[1, 1] = verticalMval;
        depthCam.projectionMatrix = projMat;

        // target Texture size calculation 
        ColumnsPerPhysStep = Mathf.RoundToInt(Time.fixedDeltaTime * 360 * RotateFrequency / AngularResolution) / SuperSample;
        ResWidth = ColumnsPerPhysStep;
        ResHeight = Channels;
        depthCam.targetTexture = new RenderTexture(ResWidth, ResHeight, 1, RenderTextureFormat.RFloat, RenderTextureReadWrite.Default);
        RangesSamples = new Texture2D(ResWidth, ResHeight, TextureFormat.RGBAFloat, false);

        depthCam.farClipPlane = MeasurementRange;
        depthCam.nearClipPlane = MinMeasurementRange;

        // initial direction of the depthCam scan window center
        cameraRotationAngle = horizontalFOV / 2;
        Sensor.transform.localEulerAngles = new Vector3(-(HigherAngle + LowerAngle)/2,  cameraRotationAngle , 0);

        // activtion of the ICD interface    
        if (sendDataOnICD)
        {
            vel16ICDinterface = new VelodyneWrapper("/home/robil/vlp.conf");
            vel16ICDinterface.Run();
        }
    }


    Color[] ranges;
    void FixedUpdate()
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        for (int s = 0; s < SuperSample; s++)
        {
            RenderTexture.active = depthCam.targetTexture; // When a RenderTexture becomes active its hardware rendering context is created
            depthCam.Render();
            RangesSamples.ReadPixels(new Rect(0, 0, ResWidth, ResHeight), 0, 0);  // copy a rectangular pixel area from the currently active RenderTexture 
            RangesSamples.Apply();
            ranges = RangesSamples.GetPixels();

            float hAng = -horizontalFOV / 2;       
            for (int i = 0; i < ResWidth; i++) //columns
            {   
                float vAng = HigherAngle;
                for (int j = 0; j < Channels; j++) //rows
                {
                    float range = (ranges[j * ResWidth + i].r * MeasurementRange) / (Mathf.Cos(hAng * Mathf.Deg2Rad) * Mathf.Cos(vAng * Mathf.Deg2Rad));

                    if (range >= MeasurementRange)
                        range = 0;                         

                    if (sendDataOnICD)
                        vel16ICDinterface.SetChannel(range, 0);

                    if (DrawLidar)
                    {
                        Vector3 rangePointPos = Sensor.transform.position + Quaternion.AngleAxis(vAng, Sensor.transform.right) * Quaternion.AngleAxis(hAng, Sensor.transform.up) * Sensor.transform.forward * range;  
                        Debug.DrawLine(rangePointPos, rangePointPos + Vector3.up * drawSize, drawColor, drawTime);
                    }                    
                    vAng = vAng - verticalAngularResolution;
                }
                hAng = hAng + AngularResolution;

                if (sendDataOnICD)
                {
                    float columnAng = Mathf.Repeat( -horizontalFOV/2  +  cameraRotationAngle +  i * AngularResolution , 360);
                    vel16ICDinterface.SetAzimuth(columnAng);
                    double timeStamp = Time.fixedTime * 1000000.0 + i;  
                    vel16ICDinterface.SetTimeStamp((int)timeStamp);
                    vel16ICDinterface.SendData();
                }
            }


            if (Rotate) 
                cameraRotationAngle += horizontalFOV;

            Sensor.transform.localEulerAngles = new Vector3(-(HigherAngle + LowerAngle) / 2, cameraRotationAngle, 0);
        }
        RenderTexture.active = currentActiveRT;
    }
}
