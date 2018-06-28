using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;



public class grader : MonoBehaviour {


public float MinDist, dissResulution;

public Text MinDistToCollisionTxt;


SphereCollider graderCollider;

public Vector3 colliderOffset = new Vector3(0,0.85f,0);

public string CollisionTag = "GraderCollisionTag";


	// Use this for initialization
	void Start () {
		graderCollider = gameObject.AddComponent<SphereCollider>();
		graderCollider.center = colliderOffset;
		graderCollider.isTrigger = true;
		graderCollider.radius = MinDist;	
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
		string garadesFile = scenFolderURI + "/grades.txt";

		StreamWriter writer = new StreamWriter(garadesFile, true);
		writer.WriteLine("MinDist : " + MinDist);
        writer.Close();
	}
#endif	
}
