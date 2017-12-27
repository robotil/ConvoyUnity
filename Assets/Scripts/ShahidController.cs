using UnityEngine;
using System.Collections;

public class ShahidController : MonoBehaviour
{

    public bool ManualInput = true;

    public float TurningCommand = 0, WokingCommand = 0;

	public float MaxWokingSpeed = 1, MaxTurningSpeed = 1; 

	
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
            WokingCommand = Input.GetAxis("Vertical");
            WokingCommand = MaxWokingSpeed * Mathf.Clamp(WokingCommand, 0, 1);

            TurningCommand = Input.GetAxis("Horizontal");
            TurningCommand = MaxTurningSpeed * Mathf.Clamp(TurningCommand, -1, 1);
        }

		anim.SetFloat("woking",WokingCommand);
		anim.SetFloat("turning",TurningCommand);
    }







}
