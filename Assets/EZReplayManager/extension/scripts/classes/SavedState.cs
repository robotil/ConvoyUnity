using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;
using Newtonsoft.Json;

/*
 * SoftRare - www.softrare.eu
 * This class represents a state of a single object in one single frame. 
 * You may only use and change this code if you purchased it in a legal way.
 * Please read readme-file, included in this package, to see further possibilities on how to use/execute this code. 
 */

[Serializable()]
[JsonObject(MemberSerialization.OptIn, IsReference = true)]
public class SavedState /*: ISerializable*/ {
    //so far 5 state attributes are saved: position, rotation, scale and whether the gameobject is active
    [JsonProperty]
    public SerVector3 pos;
    [JsonProperty]
    public SerQuaternion rot;
    [JsonProperty]
    public SerVector3 scale;

    [JsonProperty("active")]
    public bool isActive = false;
    [JsonProperty("isP")]
    public bool isParent = false;

    /*//serialization constructor
    protected SavedState(SerializationInfo info,StreamingContext context) {

		this.pos = (SerVector3)info.GetValue("pos",typeof(SerVector3));
        this.scale = (SerVector3)info.GetValue("scale", typeof(SerVector3));
        this.rot = (SerQuaternion)info.GetValue("rot",typeof(SerQuaternion));	
		
		isActive = info.GetBoolean("active");
        isParent = info.GetBoolean("parent");
    }*/

    [JsonConstructor]
    protected SavedState() {

    }

    //as this is not derived from MonoBehaviour, we have a constructor
    public SavedState(GameObject go, Object2PropertiesMapping o2m) {

        if (o2m.isParent()) {
            isParent = true;
        } else {
            isParent = false;
        }

        if (go != null) {

            if (o2m.isParent()) {
                this.pos = new SerVector3(go.transform.position);
                this.scale = new SerVector3(go.transform.lossyScale);
                this.rot = new SerQuaternion(go.transform.rotation);
                isParent = true;
            } else {
                this.pos = new SerVector3(go.transform.localPosition);
                this.scale = new SerVector3(go.transform.localScale);
                this.rot = new SerQuaternion(go.transform.localRotation);
            }
            
			this.isActive = go.activeInHierarchy;
		} else {

            this.pos = new SerVector3(Vector3.zero);
            this.scale = new SerVector3(Vector3.one);
            this.rot = new SerQuaternion(Quaternion.identity);			
			this.isActive = false;	
		}
		
	}
	
	public Vector3 serVec3ToVec3(SerVector3 serVec3) {
		return new Vector3(serVec3.x,serVec3.y,serVec3.z);
	}
	
	public Quaternion serQuatToQuat(SerQuaternion serQuat) {
		return new Quaternion(serQuat.x,serQuat.y,serQuat.z,serQuat.w);
	}	
	
	public bool isDifferentTo(SavedState otherState) {
		bool changed = false;
		
		if (!changed && isActive != otherState.isActive)
			changed = true;			
		
		if (!changed && pos.isDifferentTo( otherState.pos) )
			changed = true;

        if (!changed && rot.isDifferentTo( otherState.rot) )
			changed = true;

        if (!changed && scale.isDifferentTo(otherState.scale))
            changed = true;

		return changed;
	}

    //source: http://answers.unity3d.com/questions/14279/make-an-object-move-from-point-a-to-point-b-then-b.html
    public IEnumerator MoveTo(Transform tf, Vector3 target, float time) {
        Vector3 start = Vector3.zero;
        if (isParent) {
            start = tf.position;
        } else {
            start = tf.localPosition;
        }

        float t = 0;
        time = EZReplayManager.get.playingInterval;

        while (t <= 1) {
            yield return null;
            t += Time.deltaTime / time;
            if (tf != null) {

                if (isParent) {
                    tf.position = Vector3.Slerp(start, target, t);
                } else {
                    tf.localPosition = Vector3.Slerp(start, target, t);
                }
            } else {
                break;
            }
        }

    }
    public IEnumerator RotateTo(Transform tf, Quaternion target, float time) {
        Quaternion start = Quaternion.identity;
        if (isParent) {
            start = tf.rotation;
        } else {
            start = tf.localRotation;
        }

            float t = 0;
        time = EZReplayManager.get.playingInterval;

        while (t <= 1) {
            yield return null;
            t += Time.deltaTime / time;
            if (tf != null) {
                tf.localRotation = Quaternion.Slerp(start, target, t);
            } else {
                break;
            }
        }

    }
    public IEnumerator ScaleTo(Transform tf, Vector3 target, float time) {
        Vector3 start = Vector3.zero;

        if (isParent) {
            start = tf.lossyScale;
        } else {
            start = tf.localScale;
        }

        float t = 0;
        time = EZReplayManager.get.playingInterval;

        while (t <= 1) {
            yield return null;
            t += Time.deltaTime / time;
            if (tf != null) {
                tf.localScale = Vector3.Slerp(start, target, t);
            } else {
                break;
            }
        }

    }

    //called to synchronize gameObjectClone of Object2PropertiesMapping back to this saved state
    public virtual void synchronizeProperties(GameObject go, bool lerp) {

        if (lerp) {

            if (isParent) {

                if (serVec3ToVec3(this.pos) != go.transform.position) {
                    EZReplayManager.get.StartCoroutine(MoveTo(go.transform, serVec3ToVec3(this.pos), 0f));
                }
                if (serQuatToQuat(this.rot) != go.transform.rotation) {
                    EZReplayManager.get.StartCoroutine(RotateTo(go.transform, serQuatToQuat(this.rot), 0f));
                }
                if (serVec3ToVec3(this.scale) != go.transform.lossyScale) {
                    EZReplayManager.get.StartCoroutine(ScaleTo(go.transform, serVec3ToVec3(this.scale), 0f));
                }

            } else {

                if (serVec3ToVec3(this.pos) != go.transform.localPosition) {
                    EZReplayManager.get.StartCoroutine(MoveTo(go.transform, serVec3ToVec3(this.pos), 0f));
                }
                if (serQuatToQuat(this.rot) != go.transform.localRotation) {
                    EZReplayManager.get.StartCoroutine(RotateTo(go.transform, serQuatToQuat(this.rot), 0f));
                }
                if (serVec3ToVec3(this.scale) != go.transform.localScale) {
                    EZReplayManager.get.StartCoroutine(ScaleTo(go.transform, serVec3ToVec3(this.scale), 0f));
                }

            }


        } else {
            if (isParent) {
                go.transform.position = serVec3ToVec3(this.pos);
                go.transform.rotation = serQuatToQuat(this.rot);
                go.transform.localScale = serVec3ToVec3(this.scale);
            } else {
                go.transform.localPosition = serVec3ToVec3(this.pos);
                go.transform.localRotation = serQuatToQuat(this.rot);
                go.transform.localScale = serVec3ToVec3(this.scale);
            }
        }

        go.SetActive(this.isActive);
	}
	
	/*
	public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		
		info.AddValue("pos", pos);
        info.AddValue("scale", scale);
        info.AddValue("rot", rot);		
		
		info.AddValue("active", isActive);
        info.AddValue("parent", isParent);
        //base.GetObjectData(info, context);
    }*/	
	
}