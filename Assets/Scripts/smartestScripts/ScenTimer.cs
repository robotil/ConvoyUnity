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

	
	// Use this for initialization
	void Start () {
		LeaderController = Leader.GetComponent<VehiclePathController>();
		LeaderController.enabled = false;
		FolowerController = Folower.GetComponent<VehiclePathController>();
		FolowerController.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		LeaderMovementTimeLeft = LeaderMovementStart - Time.time;
		if (LeaderMovementTimeLeft <= 0){
			LeaderController.enabled = true;
			FolowerController.enabled = true;
		}


		ScenTimeLeft = ScenDuration - Time.time;

		displayText.text = "Time to Start : " + LeaderMovementTimeLeft.ToString("0.00"); 

#if !UNITY_EDITOR
		if (ScenTimeLeft <= 0){
			Application.Quit();
			Debug.Log("Aplication Quit");
		}
#endif
		
	}
}
