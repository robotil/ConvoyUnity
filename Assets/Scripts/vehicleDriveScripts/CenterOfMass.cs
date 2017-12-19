using UnityEngine;
using System.Collections;

public class CenterOfMass : MonoBehaviour {
    public Transform CenterOfMassPos;
    
    Rigidbody rb;
    void Start() {
        
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CenterOfMassPos.localPosition;
    }
}
