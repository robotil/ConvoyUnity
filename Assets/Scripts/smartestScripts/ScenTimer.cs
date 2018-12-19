using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScenTimer : MonoBehaviour {




	[Tooltip("Seconds")]
	public float ScenDuration = 600;

	[Tooltip("Number of seconds before the hummer starts leading.")]
	public float LeaderMovementStart = 30;	
    
	[Tooltip("Seconds")]
	public float ScenTimeLeft, LeaderMovementTimeLeft;

	public Text displayText;

    public GameObject Leader, Folower;
	VehiclePathController LeaderController, FolowerController;

	public bool showReplay =false;

	
	// Use this for initialization
	void Start () {
		LeaderController = Leader.GetComponent<VehiclePathController>();
		LeaderController.enabled = false;
		FolowerController = Folower.GetComponent<VehiclePathController>();
		FolowerController.enabled = false;
		string[] args = System.Environment.GetCommandLineArgs ();
	
		for (int i = 0; i < args.Length; i++) 
		{
			Debug.Log ("ARG " + i + ": " + args [i]);
			if (args [i] == "-scenDuration") {
				ScenDuration = float.Parse(args [i + 1]);
 			}
			if (args [i] == "-scenReplay") {
				string replay = args [i + 1];
				if(replay == "1")
				{
					showReplay = true;
				}
 			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		LeaderMovementTimeLeft = LeaderMovementStart - Time.time;
		if (LeaderMovementTimeLeft <= 0){
			LeaderController.enabled = true;
			FolowerController.enabled = true;
		}

        if (showReplay == true) {
			ScenTimeLeft = 1000;
		}
		else {
			ScenTimeLeft = ScenDuration - Time.time;

			displayText.text = "Time to Start : " + LeaderMovementTimeLeft.ToString("0.00"); 
			//Debug.Log("Time ScenTimeLeft: "+ ScenTimeLeft.ToString());
		}

#if !UNITY_EDITOR
		if (ScenTimeLeft <= 0){
			Application.Quit();
			Debug.Log("Time over "+ ScenDuration+ "seconds ...Aplication Quit");
		}
#endif
		
	}
}
