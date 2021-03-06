﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//         North (z) (Lat)
//         ^ 
//         |
//  W ------------>  East (x) (Lon)
//         |
//         |
//         S


public class Tiltan : MonoBehaviour {

	TiltanWrapper TiltanInterface;

	public bool ICD_Active = true;
	public string ICD_ConfigFile = "/home/robil/simConfigs/tiltan.conf";

	public Vector3 LatLonAltPos0;
    public Vector3 LatLonAltPos, NorthEastDownVel, AzimuthPitchRoll_mils, AzimuthPitchRollVel_mils;

	public float timeStamp;

	public float DistanceTraveled = 0;

	public bool MothionDetected = false;
	Vector3 prev_pose;


	public short GpsFom, NumOfSatelites; 
	public float HorizontalErrorCEP, VerticalErrorPE;
	public Vector3 LatLonAltPosErrorSIGMA, NorthEastDownVelErrorSIGMA, AzimuthPitchRollErrorSIGMA_mils;


	public float DataPubFreq = 50, StatusPubFreq = 1;
    Rigidbody rb;
    Transform myref;


	float Re = 6378.1f * 1000.0f; // Earth radius in meters 
	float R2D = Mathf.Rad2Deg, D2R = Mathf.Deg2Rad;
	float D2M = 6400.0f/360.0f, R2M = 6400.0f/(360.0f*Mathf.Deg2Rad);  // conversion to Mils

	public Text screnText;

	// Use this for initialization
	void Start () {
        myref = gameObject.transform;
        rb = GetComponent<Rigidbody>();
		prev_pose = myref.position;


		InvokeRepeating("TiltanDataPub", 0.0f, 1/DataPubFreq);
		InvokeRepeating("TiltanStatusPub", 0.0f, 1/StatusPubFreq);
		
		if (ICD_Active) {
			TiltanInterface = new TiltanWrapper(ICD_ConfigFile);
		}
	}


	void Update() {
		if (screnText) {
			screnText.text = "TILTAN : \n" +
							 "	Pos (Lat, Lon, Alt) [deg] = " + LatLonAltPos.ToString("N5") + "\n" +
							 "	Vel (North, East, Down) [m/sec] = " + NorthEastDownVel.ToString("N2") + "\n" +
							 "	Or (Azim, Pitch, Roll) [mils] = " + AzimuthPitchRoll_mils.ToString("N2") + "\n" + 	
							 "	OrVel (Azim, Pitch, Roll) [mils/sec] = " + AzimuthPitchRollVel_mils.ToString("N2") + "\n";
		}
	}

	
	void TiltanDataPub() {
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

		LatLonAltPos.x = lat;  // Lat in degs
		LatLonAltPos.y = lon;  // Lon in degs
		LatLonAltPos.z = alt;  // Alt in meters

		// velocity in meters/sec
		Vector3 vel = 1.0f*rb.velocity;

		NorthEastDownVel.x = 1.0f*vel.z;    
		NorthEastDownVel.y = 1.0f*vel.x;	   
		NorthEastDownVel.z = -1.0f*vel.y;    
		

		// Time in secs
        timeStamp = (float)Time.fixedTime;


		// angles in Mils
		Vector3 ang = myref.eulerAngles;
		AzimuthPitchRoll_mils.x = ang.y * D2M;
		AzimuthPitchRoll_mils.y = angShift(ang.x) * D2M;
		AzimuthPitchRoll_mils.z = angShift(ang.z) * D2M;


		// Azimuth Rate in Mils/sec
		Vector3 angVel = rb.angularVelocity;
		AzimuthPitchRollVel_mils.x = -angVel.y * R2M;

		
		float dissDiff = (pos - prev_pose).magnitude;
		if (dissDiff > 0.01)   // we asume sensor sensativity of 1cm
			{
			DistanceTraveled += dissDiff;
			prev_pose = pos;
			}


		float velMag = vel.magnitude;
		if (velMag > 0.01)
			MothionDetected = true;
		else
			MothionDetected = false;	

		if (ICD_Active && TiltanInterface!=null) {
			TiltanInterface.SetPose(LatLonAltPos.x, LatLonAltPos.y, LatLonAltPos.z);
			TiltanInterface.SetVelocity(NorthEastDownVel.x , NorthEastDownVel.y, NorthEastDownVel.z);   
			TiltanInterface.SetTimeStamps(timeStamp,timeStamp);   
			TiltanInterface.SetOrientation(AzimuthPitchRoll_mils.x,AzimuthPitchRoll_mils.y,AzimuthPitchRoll_mils.z);   
			TiltanInterface.SetAzimuthRate(AzimuthPitchRollVel_mils.x);   
			TiltanInterface.SetDistances(DistanceTraveled,DistanceTraveled);
			TiltanInterface.SetMotionDetected(MothionDetected);	
			TiltanInterface.SendNavigationData();
		}
	}


	void TiltanStatusPub() 
	{
		GpsFom = 1;   // as the   sqrt(Xerr^2 + Yerr^2 + Zerr^2) < 25meters
		NumOfSatelites = 5;
		
		HorizontalErrorCEP = 10; // Meters
		VerticalErrorPE = 10; // Meters
		LatLonAltPosErrorSIGMA = new Vector3(1,1,3); // in Meters , one sigma
		
		NorthEastDownVelErrorSIGMA = new Vector3(1,1,1); // in Meters , one sigma

		AzimuthPitchRollErrorSIGMA_mils = new Vector3((0.5f*D2M),1*D2M,1*D2M); // in Mills, one sigma

		if (ICD_Active && TiltanInterface!=null) {
			TiltanInterface.SetInternalGpsFields(GpsFom, NumOfSatelites);
			TiltanInterface.SetDirectionErrors(HorizontalErrorCEP, VerticalErrorPE, LatLonAltPosErrorSIGMA.x,LatLonAltPosErrorSIGMA.y,LatLonAltPosErrorSIGMA.z);
			TiltanInterface.SetVelocityErrors(NorthEastDownVelErrorSIGMA.x,NorthEastDownVelErrorSIGMA.y,NorthEastDownVelErrorSIGMA.z);
			TiltanInterface.SetOrientationErrors(AzimuthPitchRollErrorSIGMA_mils.x,AzimuthPitchRollErrorSIGMA_mils.y,AzimuthPitchRollErrorSIGMA_mils.z);
			TiltanInterface.SetTimeStamps(Time.fixedTime, Time.fixedTime);

			TiltanInterface.SendInternalGPSData();
			TiltanInterface.SendErrorEstimationData();
			TiltanInterface.SendStatusMsgData();
		}

	}


	float angShift(float ang)   // invert the direction of ang, ad shift it from [0 , 360] to [-180 180]
	{
		if (ang > 180) 
			return(360 - ang);
		else
			return(-ang);
	}


	float sin(float x) { return Mathf.Sin(x); }	
	float asin(float x) { return Mathf.Asin(x); }	
	float cos(float x) { return Mathf.Cos(x); }	
	float atan2(float y, float x) { return Mathf.Atan2(y,x); }	

}

