using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDeactivate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string[] args = System.Environment.GetCommandLineArgs ();
	
		for (int i = 0; i < args.Length; i++) {
			if (args [i] == "-scenReplay") {
				string replay = args [i + 1];
				if(replay == "1")
				{
					gameObject.SetActive(false);
				}
				Debug.Log("Mode replay - No panel"); 
 			}
 		
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
