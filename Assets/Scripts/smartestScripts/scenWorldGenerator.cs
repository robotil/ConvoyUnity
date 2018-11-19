using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Linq;



public class scenWorldGenerator : MonoBehaviour {


    public string scenFolderURI="";
    public string SFVToLoad = "";
	public string ReplayScenePath = "";
	public bool showReplay =false;

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
		SFVToLoad = "/home/robil/SmARTest/work_space/scenario_1/scen.SFV";
		loadInputArgs();

		file = XDocument.Load(uri: SFVToLoad);
        var xmlText = System.IO.File.ReadAllText(SFVToLoad);
		Debug.Log("scenWorldGenerator:"+SFVToLoad);

		if(showReplay)
		{
			Debug.Log("Start Replay"); 
			EZReplayManager.get.SendMessage("loadFromTextFile", ReplayScenePath,  SendMessageOptions.RequireReceiver);
		}
		else
		{  
			Debug.Log("Start Record"); 
			EZReplayManager.get.record();
			LeaderSetup();
			FollowerSetup();
			PathSetup();
			ShahidSetup(); 
		}
		
	}
	
	void loadInputArgs()
	{
		string[] args = System.Environment.GetCommandLineArgs ();
	
		for (int i = 0; i < args.Length; i++) {
			Debug.Log ("ARG " + i + ": " + args [i]);
			if (args [i] == "-scenfolder") {
				scenFolderURI = args [i + 1];
				SFVToLoad = scenFolderURI + "/scen.SFV";
				Debug.Log(SFVToLoad);
 			}
			if (args [i] == "-scenReplay") {
				string replay = args [i + 1];
				if(replay == "1")
				{
					showReplay = true;
					ReplayScenePath = scenFolderURI + "/record";
				}
				Debug.Log(ReplayScenePath); 
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
			Debug.Log("Leader position:"+LeaderPose.x.ToString()+","+ LeaderPose.y.ToString());
        }
    }

	void FollowerSetup() {
		float FollowerDiss = 20; //When inserted scenario doesn't exist, the distance between the two vehicle is 20 and the 
		                         // lock works better  .... it was 30; 
		Vector2 diffVec = new Vector2(Mathf.Sin(LeaderAzimuth), Mathf.Cos(LeaderAzimuth)) * FollowerDiss;
		Vector2 FollowerPose = LeaderPose - diffVec;

		float FollowerAzimuth = LeaderAzimuth;

		//FollowerVehicle = Instantiate(FollowerVehicleRef);
		terrainAttachment FollowerVehiclePos = FollowerVehicle.GetComponent<terrainAttachment>();
	    FollowerVehiclePos.moveTo(new Vector3(FollowerPose.x, FollowerPose.y, 0.0f));
		FollowerVehiclePos.transform.eulerAngles = new Vector3(0,FollowerAzimuth * Mathf.Rad2Deg,0);
        Debug.Log("Follower position:"+FollowerPose.x.ToString()+","+ FollowerPose.y.ToString());
		//FollowerVehicle = oshkosh
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
		//Note that if we have 3 obstacles in the XML file, then only the last coordinates are relevant!!!
        foreach (var shahidXML in file.Descendants("obstacles_on_path").Descendants("obstacle_on_path"))
        {		
            AlongPath = float.Parse(shahidXML.Element("obstacle_on_path_i_location_along_the_path").Value); //Percentage of the entire path length
			PerpePath = float.Parse(shahidXML.Element("obstacle_on_path_i_location_perpendicular_to_the_path").Value); //absolutely in meters
        }
        
        Debug.Log("ShahidSetup:AlongPath:"+AlongPath.ToString()+",PerpePath"+ PerpePath.ToString());
		float distFromStart = 0;
		Vector2 WPcurrent = LeaderPose;
		Vector2 WPnext = LeaderPose;

		float WPdist = 1, WPang=0;
		int WPid = 0;
    	foreach (var wpXML in file.Descendants("Path").Descendants("WayPoint"))
		{
			WPdist = float.Parse(wpXML.Element("wp_i_relative_distance").Value);
			WPang = float.Parse(wpXML.Element("wp_i_relative_angle").Value);
            WPid = int.Parse(wpXML.Attribute("ID").Value);

			Vector2 vecToNextWP = new Vector2(Mathf.Sin(WPang),Mathf.Cos(WPang)) * WPdist;
			WPnext = WPcurrent + vecToNextWP;
			distFromStart += WPdist;

			if (distFromStart > pathLength * AlongPath) {
				Debug.Log("ShahidSetup: WP ID="+WPid.ToString()+" distFromStart="+distFromStart.ToString()+" pathLength="+pathLength.ToString()+" AlongPath="+AlongPath.ToString());
				//The shahid should meet the oshkosh before this WP
				break; 
			} else {
				Debug.Log("ShahidSetup: ELSE WP ID="+WPid.ToString()+" distFromStart="+distFromStart.ToString()+" pathLength="+pathLength.ToString()+" AlongPath="+AlongPath.ToString());
			}
			WPcurrent = WPnext;
		}
        
		Debug.Log("ShahidSetup: WPcurrent="+WPcurrent.ToString()+" WPnext="+WPnext.ToString());
		float rem_alo = ((AlongPath - distFromStart/pathLength)*pathLength)/WPdist;
		Debug.Log("ShahidSetup: rem_alo="+rem_alo.ToString());
		float shahid_x = WPcurrent.x + rem_alo * (WPnext.x - WPcurrent.x) - PerpePath*(WPnext.y - WPcurrent.y)/WPdist;
		float shahid_y = WPcurrent.y + rem_alo * (WPnext.y - WPcurrent.y) + PerpePath*(WPnext.x - WPcurrent.x)/WPdist;
		Vector2 ShahidPose = new Vector2(shahid_x, shahid_y);

		//Vector2 shahidPose = Vector2.Lerp(WPcurrent,WPnext,rem_alo);

		terrainAttachment shahidPoseOnterrain = Shahid.GetComponent<terrainAttachment>();
		ShahidWPController shahidWpController = Shahid.GetComponent<ShahidWPController>();

		shahidPoseOnterrain.moveTo(new Vector3(ShahidPose.x, ShahidPose.y, 0.0f));
		shahidWpController.shahidTargetPoseAndVel = new Vector3(ShahidPose.x, ShahidPose.y, 0.0f);
        Debug.Log("ShahidSetup: Shahid position:"+ShahidPose.x.ToString()+","+ ShahidPose.y.ToString());

	}



	// Update is called once per frame
	void Update () {
		
	}
}
