using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//         N (z) (Lat)
//         ^ 
//         |
//  W ------------>  E (x) (Lon)
//         |
//         |
//         S


public class Tiltan : MonoBehaviour {

	DgpsWrapper DGPSinterface;

	//public string ICD_ConfigFile = "/home/robil/tiltan.conf";

	public Vector3 LatLonAltPos0;
    public Vector3 LatLonAltPos, LatLonAltVel, AzimuthPitchRoll_mils, AzimuthPitchRollVel_mils;

	public double timeStamp_sec;

	public float DistanceTraveled = 0;

	public bool MothionDetected = false;
	Vector3 prev_pose;

	public float HorizontalErrorCEP, VerticalErrorPE;
	public Vector3 LatLonAltPosErrorSIGMA, LatLonAltVelErrorSIGMA, AzimuthPitchRollErrorSIGMA_mils;


	public float DataPubFreq = 50, StatusPubFreq = 1;
    Rigidbody rb;
    Transform myref;


	float Re = 6378.1f * 1000; // Earth radius in meters 
	float R2D = Mathf.Rad2Deg, D2R = Mathf.Deg2Rad;
	float D2M = 6400/360, R2M = 6400/(360*Mathf.Deg2Rad);  // conversion to Mils

	// Use this for initialization
	void Start () {
        myref = gameObject.transform;
        rb = GetComponent<Rigidbody>();
		prev_pose = myref.position;


		InvokeRepeating("TiltanDataPub", 0.0f, 1/DataPubFreq);
		InvokeRepeating("TiltanStatusPub", 0.0f, 1/StatusPubFreq);

	//	DGPSinterface = new DgpsWrapper(ICD_ConfigFile);
	//	DGPSinterface.Run();
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

		//DGPSinterface.SetPosition(LatLonAltPos.x, LatLonAltPos.y, LatLonAltPos.z);

		// velocity in meters/sec
		Vector3 vel = rb.velocity;
		LatLonAltVel.x = vel.z;    
		LatLonAltVel.y = vel.x;	   
		LatLonAltVel.z = vel.y;    

		//DGPSinterface.SetVelocities(LatLonAltVel.x , LatLonAltVel.y, LatLonAltVel.z);   // velocity in meters/sec
		

		// Time in secs
        timeStamp_sec = Time.fixedTime;
		//DGPSinterface.SetTimeStamp((int)timeStamp);   
		//DGPSinterface.SendData();


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

	}


	void TiltanStatusPub() 
	{
		HorizontalErrorCEP = 10; // Meters
		VerticalErrorPE = 10; // Meters
		LatLonAltPosErrorSIGMA = new Vector3(1,1,3); // in Meters , one sigma
		LatLonAltVelErrorSIGMA = new Vector3(1,1,1); // in Meters , one sigma
		AzimuthPitchRollErrorSIGMA_mils = new Vector3((0.5f*D2M),1*D2M,1*D2M); // in Mills, one sigma
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

