using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScenTerminator : MonoBehaviour {

	
	public float ScenTimeMax;	
	public float ScenTimeLeft;

	public Text ScenTimeLeftText;
	
	// Use this for initialization
	void Start () {
		ScenTimeMax = 15;
	}
	
	// Update is called once per frame
	void Update () {

		ScenTimeLeft = ScenTimeMax - Time.time;

		ScenTimeLeftText.text = "Time Left : " + ScenTimeLeft.ToString("N2"); 

		if (ScenTimeLeft <= 0){
			Application.Quit();
			Debug.Log("Aplication Quit");
		}
		
	}
}
