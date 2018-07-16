using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Linq;



public class scenWorldGenerator : MonoBehaviour {


    public string SFVToLoad = "";
	XDocument file;

	//public GameObject LeaderVehicleRef, FollowerVehicleRef, ShahidRef;
	public GameObject LeaderVehicle, FollowerVehicle, Shahid;

	float MapSize = 250;

 	Vector2 LeaderPose;
	float pathLength = 0; 
	float LeaderAzimuth; 
	public VehiclePathController LeaderPathController;



	
	// Use this for initialization
	void Start () {
		SFVToLoad = "/home/robil/ws/src/SmARTest/work_space/scenario_10/scen.SFV";
		loadInputArgs();

		file = XDocument.Load(uri: SFVToLoad);
        var xmlText = System.IO.File.ReadAllText(SFVToLoad);
		Debug.Log(SFVToLoad);

		LeaderSetup();
		FollowerSetup();
		PathSetup();
		ShahidSetup();
	}
	

	void loadInputArgs()
	{
		string[] args = System.Environment.GetCommandLineArgs ();
	
		for (int i = 0; i < args.Length; i++) {
			Debug.Log ("ARG " + i + ": " + args [i]);
			if (args [i] == "-scenfolder") {
				string scenFolderURI = args [i + 1];
				SFVToLoad = scenFolderURI + "/scen.SFV";
				Debug.Log(SFVToLoad);
 			}
		}
	}




    void LeaderSetup()
    {
        foreach (var vehicleXML in file.Descendants("platform_pose"))
        {		
            LeaderPose = new Vector2(float.Parse(vehicleXML.Element("initial_platform_position_on_map_X_axis").Value),
			                                 float.Parse(vehicleXML.Element("initial_platform_position_on_map_Y_axis").Value));// * MapSize;
			LeaderAzimuth = float.Parse(vehicleXML.Element("initial_platform_azimut_on_map").Value);
			
			//LeaderVehicle = Instantiate(LeaderVehicleRef);
			
			terrainAttachment LeaderVehiclePos = LeaderVehicle.GetComponent<terrainAttachment>();
			LeaderVehiclePos.moveTo(new Vector3(LeaderPose.x, LeaderPose.y, 0.0f));
			LeaderVehicle.transform.eulerAngles = new Vector3(0,LeaderAzimuth * Mathf.Rad2Deg,0);
        }
    }

	void FollowerSetup() {
		float FollowerDiss = 30; 
		Vector2 diffVec = new Vector2(Mathf.Sin(LeaderAzimuth), Mathf.Cos(LeaderAzimuth)) * FollowerDiss;
		Vector2 FollowerPose = LeaderPose - diffVec;

		float FollowerAzimuth = LeaderAzimuth;

		//FollowerVehicle = Instantiate(FollowerVehicleRef);
		terrainAttachment FollowerVehiclePos = FollowerVehicle.GetComponent<terrainAttachment>();
	    FollowerVehiclePos.moveTo(new Vector3(FollowerPose.x, FollowerPose.y, 0.0f));
		FollowerVehiclePos.transform.eulerAngles = new Vector3(0,FollowerAzimuth * Mathf.Rad2Deg,0);

     	VehiclePathController FollowerPathController = FollowerVehicle.GetComponent<VehiclePathController>();
		FollowerPathController.PathWPs_PosesAndVels.Clear(); 

		VehicleFollowerPathController PathController = FollowerVehicle.GetComponent<VehicleFollowerPathController>(); 
		PathController.leaderVehicle = LeaderVehicle.transform;
	}


	void PathSetup() 
	{
     		LeaderPathController = LeaderVehicle.GetComponent<VehiclePathController>();
			LeaderPathController.PathWPs_PosesAndVels.Clear(); 

			Vector2 WP = LeaderPose;
            LeaderPathController.PathWPs_PosesAndVels.Add(new Vector3( WP.x, WP.y, 5)); 

         	foreach (var element in file.Descendants("Path").Descendants("WayPoint"))
			{
				float WPdiss = float.Parse(element.Element("wp_i_relative_distance").Value);
				float WPang = float.Parse(element.Element("wp_i_relative_angle").Value);
				float WPvel = float.Parse(element.Element("wp_i_velocity").Value);


				Vector2 vecToNextWP = new Vector2(Mathf.Sin(WPang),Mathf.Cos(WPang)) * WPdiss;
				WP = WP + vecToNextWP;
				LeaderPathController.PathWPs_PosesAndVels.Add(new Vector3( WP.x, WP.y, WPvel)); 

				pathLength+=WPdiss; // used in shahid positionning
			}
	}


	void ShahidSetup() 
	{
		float AlongPath = 0 , PerpePath = 0;
        foreach (var shahidXML in file.Descendants("obstacles_on_path").Descendants("obstacle_on_path"))
        {		
            AlongPath = float.Parse(shahidXML.Element("obstacle_on_path_i_location_along_the_path").Value); 
			PerpePath = float.Parse(shahidXML.Element("obstacle_on_path_i_location_perpendicular_to_the_path").Value); 
        }

		float dissFromStart = 0;
		Vector2 WPcurent = LeaderPose;
		Vector2 WPnext = LeaderPose;

		float WPdiss = 1, WPang=0;
    	foreach (var wpXML in file.Descendants("Path").Descendants("WayPoint"))
		{
			WPdiss = float.Parse(wpXML.Element("wp_i_relative_distance").Value);
			WPang = float.Parse(wpXML.Element("wp_i_relative_angle").Value);

			Vector2 vecToNextWP = new Vector2(Mathf.Sin(WPang),Mathf.Cos(WPang)) * WPdiss;
			WPnext = WPcurent + vecToNextWP;
			dissFromStart += WPdiss;

			if (dissFromStart > pathLength * AlongPath) {
				break; 
			}
			WPcurent = WPnext;
		}

		float rem_alo = ((AlongPath - dissFromStart/pathLength)*pathLength)/WPdiss;
		float shahid_x = WPcurent.x + rem_alo * (WPnext.x - WPcurent.x) - PerpePath*(WPnext.y - WPcurent.y)/WPdiss;
		float shahid_y = WPcurent.y + rem_alo * (WPnext.y - WPcurent.y) + PerpePath*(WPnext.x - WPcurent.x)/WPdiss;
		Vector2 shahidPose = new Vector2(shahid_x, shahid_y);

		//Vector2 shahidPose = Vector2.Lerp(WPcurent,WPnext,rem_alo);

		terrainAttachment shahidPoseOnterrain = Shahid.GetComponent<terrainAttachment>();
		ShahidWPController shahidWpController = Shahid.GetComponent<ShahidWPController>();

		shahidPoseOnterrain.moveTo(new Vector3(shahidPose.x, shahidPose.y, 0.0f));
		shahidWpController.shahidTargetPoseAndVel = new Vector3(shahidPose.x, shahidPose.y, 0.0f);
	}



	// Update is called once per frame
	void Update () {
		
	}
}
