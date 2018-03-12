﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScenTimer : MonoBehaviour {




	public float ScenDuration = 600;
	public float LeaderMovementStart = 30;	

	public float ScenTimeLeft, LeaderMovementTimeLeft;

	public Text ScenTimeLeftText;

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
		ScenTimeLeftText.text = "Time Left : " + ScenTimeLeft.ToString("N2"); 

		#if !UNITY_EDITOR
		if (ScenTimeLeft <= 0){
			Application.Quit();
			Debug.Log("Aplication Quit");
		}
		#endif
		
	}
}
