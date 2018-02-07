using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Linq;






public class scenWorldGenerator : MonoBehaviour {


    public string SFVToLoad = "";
	XDocument file;

	public GameObject LeaderVehicleRef, FollowerVehicleRef, ShahidRef;
	GameObject LeaderVehicle, FollowerVehicle, Shahid;

	float MapSize = 250;

 	Vector2 LeaderPose;
	float LeaderAzimuth; 
	public VehiclePathController LarerPathController;


	
	// Use this for initialization
	void Start () {
		string fileNmae = "scen.SFV";
		SFVToLoad = Application.dataPath + "/SFVs/" + fileNmae;

		file = XDocument.Load(uri: SFVToLoad);
        var xmlText = System.IO.File.ReadAllText(SFVToLoad);
		Debug.Log(SFVToLoad);


		LeaderPositon();
		FollowerPosition();
		LeaderPath();

	}
	

    void LeaderPositon()
    {
        foreach (var vehicleXML in file.Descendants("platform_pose"))
        {
            Debug.Log(vehicleXML);
			
            LeaderPose = new Vector2(float.Parse(vehicleXML.Element("initial_platform_position_on_map_X_axis").Value),
			                                 float.Parse(vehicleXML.Element("initial_platform_position_on_map_Y_axis").Value)) * MapSize;
			LeaderAzimuth = float.Parse(vehicleXML.Element("initial_platform_azimut_on_map").Value);
			
			LeaderVehicle = Instantiate(LeaderVehicleRef);
			
			terrainAttachment LeaderVehiclePos = LeaderVehicle.GetComponent<terrainAttachment>();
			LeaderVehiclePos.moveTo(new Vector3(LeaderPose.x, LeaderPose.y, 1.0f));
			LeaderVehicle.transform.eulerAngles = new Vector3(0,LeaderAzimuth * Mathf.Rad2Deg,0);
        }
    }

	void FollowerPosition() {
		float FollowerDiss = 30; 
		Vector2 diffVec = new Vector2(Mathf.Sin(LeaderAzimuth), Mathf.Cos(LeaderAzimuth)) * FollowerDiss;
		Vector2 FollowerPose = LeaderPose - diffVec;

		float FollowerAzimuth = LeaderAzimuth;

		FollowerVehicle = Instantiate(FollowerVehicleRef);
		terrainAttachment FollowerVehiclePos = FollowerVehicle.GetComponent<terrainAttachment>();
	    FollowerVehiclePos.moveTo(new Vector3(FollowerPose.x, FollowerPose.y, 1.0f));
		FollowerVehiclePos.transform.eulerAngles = new Vector3(0,FollowerAzimuth * Mathf.Rad2Deg,0);

		VehicleFollowerPathController PathController = FollowerVehicle.GetComponent<VehicleFollowerPathController>(); 
		PathController.leaderVehicle = LeaderVehicle.transform;
	}


	void LeaderPath() 
	{
     		LarerPathController = LeaderVehicle.GetComponent<VehiclePathController>();

			Vector2 WP = LeaderPose;
            LarerPathController.PathWPs_PosesAndVels.Add(new Vector3( WP.x, WP.y, 5)); 

         	foreach (var element in file.Descendants("Path").Descendants("WayPoint"))
			{
				float WPdiss = float.Parse(element.Element("wp_i_relative_distance").Value);
				float WPang = float.Parse(element.Element("wp_i_relative_angle").Value);

				Vector2 vecToNextWP = new Vector2(Mathf.Sin(WPang),Mathf.Cos(WPang)) * WPdiss;
				WP = WP + vecToNextWP;
				LarerPathController.PathWPs_PosesAndVels.Add(new Vector3( WP.x, WP.y, 5)); 
			}
	}



	// Update is called once per frame
	void Update () {
		
	}
}
