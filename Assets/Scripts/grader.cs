using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;



public class grader : MonoBehaviour {


public float MinDist, distResolution;

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

		Debug.Log("grader::Start colliderOffset="+colliderOffset.ToString()+" Height="+graderCollider.height.ToString());

		Debug.Log("grader::Start Tag="+CollisionTag+" MinDist="+MinDist.ToString());
	}
	

    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag(CollisionTag) )
		  {
			Debug.Log("Grader:OnTriggerEnter:Check collision with "+other.gameObject.name);
			MinDist = graderCollider.radius;
			Debug.Log("Grader:graderCollider.radius: "+graderCollider.radius + "-distResolution:"+distResolution );
			if ( graderCollider.radius >= distResolution )
				{
					graderCollider.radius -= distResolution;
				}
		  }
		MinDistToCollisionTxt.text = "Min Dist: " + MinDist.ToString("F2");  
		//Debug.Log("Grader:OnTriggerEnter:"+MinDistToCollisionTxt.text);
		
    }


#if !UNITY_EDITOR
	private void OnApplicationQuit() {
		string[] args = System.Environment.GetCommandLineArgs ();
	
		string scenFolderURI = "/home/robil/SmARTest/work_space/scenario_1";
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
		string filename = scenFolderURI + "/record";
		//EZReplayManager.get.stop();
		//EZReplayManager.get.saveToFile(filename);
		Debug.Log (filename + " has been saved... ");
	}
#endif	
}
