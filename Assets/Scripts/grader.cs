﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;



public class grader : MonoBehaviour {


public float MinDist, dissResulution;

public Text MinDistToCollisionTxt;


CapsuleCollider graderCollider;

public Vector3 colliderOffset = new Vector3(0,0.85f,0);

string CollisionTag = "GraderCollisionTag";


	// Use this for initialization
	void Start () {
		graderCollider = gameObject.AddComponent<CapsuleCollider>();
		graderCollider.center = colliderOffset;
		graderCollider.isTrigger = true;
		graderCollider.radius = MinDist;

		CollisionTag = gameObject.tag;	

		Debug.Log("Tag="+CollisionTag);
	}
	

    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag(CollisionTag) )
		  {
			MinDist = graderCollider.radius;
			if ( graderCollider.radius >= dissResulution )
				{
					graderCollider.radius -= dissResulution;
				}
		  }
		MinDistToCollisionTxt.text = "Min Dist: " + MinDist.ToString("F2");  
		Debug.Log(MinDistToCollisionTxt.text);
    }


#if !UNITY_EDITOR
	private void OnApplicationQuit() {
		string[] args = System.Environment.GetCommandLineArgs ();
	
		string scenFolderURI = "";
		for (int i = 0; i < args.Length; i++) 
		{
			Debug.Log ("ARG " + i + ": " + args [i]);
			if (args [i] == "-scenfolder") {
				scenFolderURI = args [i + 1];
 			}
		}
		string gradesFile = scenFolderURI + "/grades.txt";

		StreamWriter writer = new StreamWriter(gradesFile, true);
		writer.WriteLine("MinDist : " + MinDist);
        writer.Close();
	}
#endif	
}
