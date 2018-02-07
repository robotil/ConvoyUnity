using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainAttachment : MonoBehaviour {

	//public Vector3 XYHightCords;


	// Use this for initialization
	void Start () {
	}
	
	public void moveTo(Vector3 XYHightCords) {
		Vector3 objectPosGlobal = new Vector3(XYHightCords.x,0,XYHightCords.y) + 1000*Vector3.up;

        RaycastHit hit;
        Physics.Raycast(objectPosGlobal,-Vector3.up, out hit);
        objectPosGlobal = hit.point;
		objectPosGlobal.y = objectPosGlobal.y + XYHightCords.z;

        gameObject.transform.position = objectPosGlobal;

		
	}
	


}
