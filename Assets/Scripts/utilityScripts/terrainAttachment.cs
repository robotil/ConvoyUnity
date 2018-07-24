using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainAttachment : MonoBehaviour {

	//public Vector3 XYHightCords;


	// Use this for initialization
	void Start () {
		//Set each object (gameObject) at its position
		Vector3 tempPose = gameObject.transform.position; // + (new Vector3(10,0,10));
		Debug.Log(gameObject.name+":"+ tempPose);
		moveTo(new Vector3(tempPose.x,tempPose.z,0.1f));
	}
	
	public void moveTo(Vector3 XYHightCords) {
		Vector3 objectPosGlobal = new Vector3(XYHightCords.x,100.0f,XYHightCords.y);

        RaycastHit hit;
		int layerMask = 1 << 8;
        Physics.Raycast(objectPosGlobal,-Vector3.up ,out hit , 1000.0f, layerMask);


	//	Debug.DrawLine(objectPosGlobal,objectPosGlobal+0.1f*Vector3.up , Color.blue, 100, false);
	//	Debug.DrawLine(objectPosGlobal, hit.point , Color.red, 100, false);
	//	Debug.Log(hit.collider.name);

        objectPosGlobal = hit.point;
		objectPosGlobal.y = objectPosGlobal.y + XYHightCords.z;

        gameObject.transform.position = objectPosGlobal;


	}
	


}
