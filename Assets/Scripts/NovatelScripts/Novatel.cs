﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//         N (z) (Lat)
//         ^ 
//         |
//  W ------------>  E (x) (Lon)
//         |
//         |
//         S


public class Novatel : MonoBehaviour {

	NovatelWrapper NovatelInterface;
	
	public bool ICD_Active = true;
	public string ICD_ConfigFile = "/home/robil/simConfigs/novatel.conf";

	public Vector3 LatLonAltPos0;
    public Vector3 LatLonAltPos, LatLonAltVel; 

	public float SensorPubFreq = 1;
    Rigidbody rb;
    Transform myref;

	public Text screnText;

	float Re = 6378.1f * 1000; // Earth radius in meters 
	float R2D = Mathf.Rad2Deg, D2R = Mathf.Deg2Rad;

	// Use this for initialization
	void Start () {
        myref = gameObject.transform;
        rb = GetComponent<Rigidbody>();

		float pubTimeInterval = 1/SensorPubFreq;
		InvokeRepeating("PosVelPub", 0.0f, pubTimeInterval);

		if(ICD_Active) {
			NovatelInterface = new NovatelWrapper(ICD_ConfigFile);
		}			
	}


	void Update() {
		if (screnText) {
			screnText.text = "NOVATEL : \n" +
							 "	Pos (Lat, Lon, Alt) [deg] = " + LatLonAltPos.ToString("N5") + "\n" +
							 "	Vel (North, East, Down) [m/sec] = " + LatLonAltVel.ToString("N2") + "\n";
		}
	}


	// Update is called once per frame
	void PosVelPub() {
		Vector3 pos = myref.position;
		float dist = Mathf.Sqrt(pos.x*pos.x + pos.z*pos.z);
		float brng = Mathf.Atan2(pos.x,pos.z);
		float hight = pos.y; 

		float lat0 = LatLonAltPos0.x;
		float lon0 = LatLonAltPos0.y;
		float alt0 = LatLonAltPos0.z;

		float lat = R2D*asin(sin(lat0*D2R)*cos(dist/Re)+cos(lat0*D2R)*sin(dist/Re)*cos(brng));
		float lon = lon0 + R2D*atan2(sin(brng)*sin(dist/Re)*cos(lat0*D2R),cos(dist/Re)-sin(lat0*D2R)*sin(lat*D2R));
		float alt = alt0 + hight;

		LatLonAltPos.x = lat;
		LatLonAltPos.y = lon;
		LatLonAltPos.z = alt;

		NovatelInterface.SetPosition(LatLonAltPos.x, LatLonAltPos.y, LatLonAltPos.z);
		NovatelInterface.SetTimeStamp(Time.fixedTime);
		NovatelInterface.SendBestPosData();

		Vector3 vel = rb.velocity;
		LatLonAltVel.x = vel.z;
		LatLonAltVel.y = vel.x;
		LatLonAltVel.z = vel.y;

		NovatelInterface.SetVelocities(LatLonAltVel.x , LatLonAltVel.y, LatLonAltVel.z);
		NovatelInterface.SetTimeStamp(Time.fixedTime);
		NovatelInterface.SendBestVelData();
	}


	float sin(float x) { return Mathf.Sin(x); }	
	float asin(float x) { return Mathf.Asin(x); }	
	float cos(float x) { return Mathf.Cos(x); }	
	float atan2(float y, float x) { return Mathf.Atan2(y,x); }	

}
