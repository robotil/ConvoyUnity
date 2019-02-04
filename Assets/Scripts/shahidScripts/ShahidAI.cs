using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ShahidAI : MonoBehaviour
    {
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target; // the leading vehicale,  the target to aim for
        Transform myref;

        public float Speed = 1, StopThreshold = 1;

        bool MoveToTarget = true, Triggered = false;

        Vector3 targetWP = Vector3.zero; 

        private void Start()
        {
            myref = transform;
            character = GetComponent<ThirdPersonCharacter>();
            character.Move(Vector3.zero, false, false); 

            target = GameObject.Find("hmmwv").transform;
        } 

        private void FixedUpdate()
        {
            if ( (Triggered) && (target != null) )
            {
                Vector3 distToTarget = Vector3.Scale(targetWP - myref.position, new Vector3(1, 0, 1));
                Vector3 velocityCommand = distToTarget * Speed;
#if MOVINGSHAHID
                if (MoveToTarget)
                    character.Move(velocityCommand, false, false);
                else
                    character.Move(Vector3.zero, false, false);
#endif
                if (distToTarget.magnitude < StopThreshold)
                    MoveToTarget = false;
            } 
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!Triggered)
                targetWP = target.position;

            if (other.CompareTag("Vehicle")) 
                Triggered = true;
        }
    }
}
