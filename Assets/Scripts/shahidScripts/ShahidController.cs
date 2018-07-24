using UnityEngine;
using System.Collections;

public class ShahidController : MonoBehaviour
{

    public bool ManualInput = true;
    [Tooltip("Inserted by the mouse for manual input")]
    public float TurningCommand = 0, WalkingCommand = 0;

	public float MaxWalkingSpeed = 1, MaxTurningSpeed = 1; 

	
    Transform myref;
	Rigidbody rb;
	Animator anim;


    // Use this for initialization
    void Start()
    {
        myref = gameObject.transform;
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ManualInput)
        {
            WalkingCommand = Input.GetAxis("Vertical");
            WalkingCommand = MaxWalkingSpeed * Mathf.Clamp(WalkingCommand, 0, 1);

            TurningCommand = Input.GetAxis("Horizontal");
            TurningCommand = MaxTurningSpeed * Mathf.Clamp(TurningCommand, -1, 1);
        }

        //"Forward" and "Turn" are parameters of the animator controller: ThirdPersonAnimatorController
        // There are 4 more parameters: "Crouch" (boolean), "OnGround" (boolean), "Jump" (float), "JumLeg" (float)
        // You can access those parameters in the Unity3d Editor via the Animator states diagram
		anim.SetFloat("Forward",WalkingCommand);
		anim.SetFloat("Turn",TurningCommand);
        
    }

    public void toggleManual(bool manual)
    {
        ManualInput = manual;
    }

}
