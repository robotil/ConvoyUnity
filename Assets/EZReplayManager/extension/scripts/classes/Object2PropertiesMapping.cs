using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

using Newtonsoft.Json;
using MyCollections;

/*
 * SoftRare - www.softrare.eu
 * This class is mapped to a single game object in EZReplayManager.cs map gOs2propMappings. It takes control of how to handle the scene if a recording is replayed and set back to normal game view.
 * You may only use and change this code if you purchased it in a legal way.
 * Please read readme-file, included in this package, to see further possibilities on how to use/execute this code. 
 */
[Serializable()]
[JsonObject(IsReference = true)]
public class Object2PropertiesMapping /*: ISerializable */{

    //saved states belonging to one game object
    [JsonConverter(typeof(OrderedDictionaryConverter<SavedState>))]
    public OrderedDictionary<int,SavedState> savedStates = new OrderedDictionary<int,SavedState>();
    //the game object this mapping class object is being created for
    [JsonIgnore]
    protected GameObject gameObject;
    //the clone belonging to the gameObject
    [JsonIgnore]
    protected GameObject gameObjectClone;
    //is it a parent game object?
    [JsonProperty("isP")]
    public bool isParentObj = false;
    //mapping of parent game object
    [JsonProperty("P")]
    public Object2PropertiesMapping parentMapping;
    //child no. parents have 0
    [JsonProperty("cno")]
    public int childNo;
    //prefab load path (for saving recordings)
    [JsonProperty("path")]
    public string prefabLoadPath = "";
    //last frame where changes where recognized
    [JsonProperty("last")]
    public int lastChangedFrame = -1;
    //frame that falls out of buffer and is not needed anymore
    //protected int mostRecentFrameOutOfRange = -1;
    //first frame where changes where recognized
    [JsonProperty("first")]
    public int firstChangedFrame = -1;
    //InstanceID of original gameObject
    [JsonIgnore]
    public int gameObjectInstanceID = -1;
    //name of original gameObject
    [JsonProperty("goname")]
    public string gameObjectName = "name_untraceable";
    //way of identifying children of gameobjects in game scene hierarchy
    [JsonProperty("mode")]
    public ChildIdentificationMode childIdentificationMode = ChildIdentificationMode.IDENTIFY_BY_ORDER;
  
    //serialization constructor
    /*protected Object2PropertiesMapping(SerializationInfo info,StreamingContext context) {
	    savedStates = (OrderedDictionary<int,SavedState>)info.GetValue("savedStates",typeof(OrderedDictionary<int,SavedState>));
	    isParentObj = info.GetBoolean("isParentObj");
		parentMapping = (Object2PropertiesMapping)info.GetValue("parentMapping",typeof(Object2PropertiesMapping));
		childNo = info.GetInt32("childNo");
		prefabLoadPath = info.GetString("prefabLoadPath");
		lastChangedFrame = info.GetInt32("lastChangedFrame");
		firstChangedFrame = info.GetInt32("firstChangedFrame");
		try {
			childIdentificationMode = (ChildIdentificationMode)info.GetValue("childIdentificationMode",typeof(ChildIdentificationMode));
		} catch (SerializationException) {
			//file was recorded using old version of this plugin
			childIdentificationMode = ChildIdentificationMode.IDENTIFY_BY_ORDER;
		}		
		try {
			gameObjectName = info.GetString("gameObjectName");
		} catch (SerializationException) {
			//file was recorded using old version of this plugin	
			childIdentificationMode = ChildIdentificationMode.IDENTIFY_BY_ORDER;
			gameObjectName = "name_untraceable";
		}
	}*/
	
	public Object2PropertiesMapping(GameObject go,bool isParent, Object2PropertiesMapping parentMapping, int childNo, string prefabLoadPath, ChildIdentificationMode childIdentificationMode) : this (go,isParent,parentMapping, childNo, prefabLoadPath) {
		this.childIdentificationMode = childIdentificationMode;
		
		if (isParentObj) { // if gameObject is a parent..
			//..instantiate mappings for all children too
			Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>() ;
			for(int i=0;i<allChildren.Length;i++) {
				GameObject child = allChildren[i].gameObject;
				
				if (!EZReplayManager.get.gOs2propMappings.ContainsKey(child)) {
					
					if (child!=gameObject)
						EZReplayManager.get.gOs2propMappings.Add(child, new Object2PropertiesMapping(child,false,this,i,"",childIdentificationMode));
					
				} else 
					if (EZReplayManager.showHints)
						Debug.Log("EZReplayManager HINT: GameObject '"+child+"' is already being recorded. Will not be marked for recording again.");				
				
			}
		}

    }	
	
	protected Object2PropertiesMapping(GameObject go,bool isParent, Object2PropertiesMapping parentMapping, int childNo, string prefabLoadPath) : this (go,isParent,parentMapping, childNo) {
		this.prefabLoadPath = prefabLoadPath;
	}	
	
	//as this is not derived from MonoBehaviour, we have a constructor
	protected Object2PropertiesMapping(GameObject go,bool isParent, Object2PropertiesMapping parentMapping, int childNo) {
		//setting instance variables
		this.gameObject = go;
		this.isParentObj = isParent;		
		this.parentMapping = parentMapping;
		this.childNo = childNo;
		this.gameObjectInstanceID=go.GetInstanceID();
		this.gameObjectName = go.name;
	

	}

    [JsonConstructor]
    protected Object2PropertiesMapping() {

    }

    public bool isParent() {
		return isParentObj;
	}
	
	public GameObject getGameObject() {
		return gameObject;
	}		
	
	public GameObject getGameObjectClone() {
		return gameObjectClone;
	}
	
	public int getLastChangedFrame() {
		return lastChangedFrame;	
	}
	
	public void setLastChangedFrame(int lastChangedFrame) {
		this.lastChangedFrame = lastChangedFrame;
	}
	
	//executed before each replay
	public void prepareObjectForReplay() {
		
		//spawn super object which gets all replay manager objects as children
		GameObject superParent = GameObject.Find(EZReplayManager.S_PARENT_NAME);
	
		//create super parent if has not happened. The super parent keeps the scene clean
		if (superParent == null) {
			superParent = new GameObject(EZReplayManager.S_PARENT_NAME);		
			superParent.transform.position = Vector3.zero;
			superParent.transform.rotation = Quaternion.identity;
			superParent.transform.localScale = Vector3.one;
		}
		
		if (isParentObj) { //if is a parent gameObject mapping 
			
			if (prefabLoadPath == "") {
				gameObjectClone = (GameObject)GameObject.Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
				
			} else {
				try {
					gameObjectClone = (GameObject)GameObject.Instantiate(Resources.Load(prefabLoadPath));
				} catch (ArgumentException) {

				}
			}
			
			gameObjectClone.transform.parent = superParent.transform;
		
		} else { // if is a child (can also be a parent in game scene hierachy but "EZReplayManager.mark4recording()" has not been called for this object specifically, so we handle it as a child
			
			GameObject myParentClone = parentMapping.getGameObjectClone();
			Transform[] allChildren = myParentClone.GetComponentsInChildren<Transform>(true) ;
			
			for(int i=0;i<allChildren.Length;i++) {
				GameObject child = allChildren[i].gameObject;
				//map child to order number or go-name
				if ((childIdentificationMode == ChildIdentificationMode.IDENTIFY_BY_ORDER && i == childNo) ||
					(childIdentificationMode == ChildIdentificationMode.IDENTIFY_BY_NAME && gameObjectName == child.name)) {
					gameObjectClone = child;
					break;
				}
			}			
			
			if (gameObjectClone == null) { //child was destroyed along the way while recording
				if (EZReplayManager.get.precacheGameobjects)
					gameObjectClone = (GameObject)GameObject.Instantiate(Resources.Load(EZReplayManager.get.generateCachePath(gameObjectName,"")));

			}
			
		}
		
		gameObjectClone.name = gameObjectInstanceID+"_"+gameObjectClone.GetInstanceID()+"_"+gameObjectClone.name;
		
		if (gameObjectInstanceID > -1) // can happen when file was loaded. obviously this doesn't work with loaded files yet.
			EZReplayManager.get.instanceIDtoGO.Add(gameObjectInstanceID,gameObject);
	
		// kill all unneccessary scripts on gameObjectClone
		Component[] allComps = gameObjectClone.GetComponentsInChildren<Component>(true);
		
		List<Component> componentsToKill = new List<Component>();
		foreach (Component comp in allComps) {
			
			//Exclude scripts and components from removal: (this is done to preserve basic functionality and renderers)
			if (comp != comp.GetComponent<Transform>() 
				&& comp != comp.GetComponent<MeshFilter>()
                && comp != comp.GetComponent<MeshRenderer>() 
				&& comp != comp.GetComponent<SkinnedMeshRenderer>() 
				&& comp != comp.GetComponent<Camera>()
				&& comp != comp.GetComponent<GUILayer>()
				&& comp != comp.GetComponent<AudioListener>()
				&& comp != comp.GetComponent<SpriteRenderer>()
				&& comp != comp.GetComponent("FlareLayer")
				) {

						bool found = false;
						// take exceptions from public array "EZReplayManager.componentsAndScriptsToKeepAtReplay"
						for (int i=0;i<EZReplayManager.get.componentsAndScriptsToKeepAtReplay.Count;i++) {
							if (comp == comp.GetComponent(EZReplayManager.get.componentsAndScriptsToKeepAtReplay[i])) {
								found = true;
								break;
							}
						}
						
						if (!found) {
							componentsToKill.Add(comp);
						}
				} 
		}	
		//uses multiple cycles to kill components which are required by others
		int cycles = 0;
		do {
			List<Component> componentsToKillNew = componentsToKill;
			for(int i=0;i<componentsToKill.Count;i++) {
				Component comp = componentsToKill[i];

				try {
					
					GameObject.DestroyImmediate(comp);
				} finally {
					if (comp == null) {
						componentsToKillNew.RemoveAt(i);
					} else { //change order
						componentsToKillNew.Remove(comp);
						componentsToKillNew.Add(comp);
					}
				}
			}
			
			componentsToKill = componentsToKillNew;
			cycles++;
		} while (componentsToKill.Count > 0 && cycles <= 10);
		
		EZR_Clone thisCloneScript = gameObjectClone.AddComponent<EZR_Clone>();
		thisCloneScript.origInstanceID = gameObjectInstanceID;
		thisCloneScript.cloneInstanceID = gameObjectClone.GetInstanceID();		
		
		if (EZReplayManager.get.autoDeactivateLiveObjectsOnReplay && gameObject != null) {	
			
			gameObject.SetActive( false );
		}

    }

    public int getMaxFrames() {
		int maxframes = 0;
		foreach(var stateEntry in savedStates) {        // less efficient but the loop is needed now, for simple reference to last frame we need savedStates to be an OrderedDictionary (which implements System.ISerializable for saving)
            if (stateEntry.Key > maxframes)
				maxframes = stateEntry.Key;
		}		
		return maxframes;
	}

    public int getLastKeyFrameBefore(int current) {

        int frame = -1;
        foreach (var stateEntry in savedStates.Reverse()) {
            frame = stateEntry.Key;
            if (frame < current) {

                break; // more efficient but not good, first we need savedStates to be an OrderedDictionary (which implements System.ISerializable for saving)
            }
        }

        return frame;
    }

    //executed just before stopping a replay
    public void resetObject() {
		
		
		GameObject superParent = GameObject.Find(EZReplayManager.S_PARENT_NAME);
		//destroy superParent if not yet done
		if (superParent != null)
			GameObject.Destroy(superParent);
		//clear clones list
		if (EZReplayManager.get.instanceIDtoGO.Count > 0)
			EZReplayManager.get.instanceIDtoGO.Clear();
		
		//reactivate gameObject
		if (gameObject != null && EZReplayManager.get.autoDeactivateLiveObjectsOnReplay) {
			if (savedStates.ContainsKey(lastChangedFrame) && lastChangedFrame > -1) {
				
					gameObject.SetActive( savedStates[lastChangedFrame].isActive );
				
			} else 
				gameObject.SetActive( true );
		}
		
	}
	
	// insert a new state at certain position
	public void insertStateAtPos(int recorderPosition) {
		
		SavedState newState = new SavedState(gameObject, this);	
		bool insertFrame = true;
		if (lastChangedFrame > -1) {

			if (savedStates.ContainsKey(lastChangedFrame) && !newState.isDifferentTo(savedStates[lastChangedFrame])) {
			
				insertFrame = false;
			}
		}
		try {
			if (insertFrame) {
                //Debug.Log("recorderPosition: " + recorderPosition);
                savedStates.Add(recorderPosition,newState);

                lastChangedFrame = recorderPosition;

                if (firstChangedFrame == -1) { //first run
                    firstChangedFrame = recorderPosition;
                }

            }
		} catch {
			if (EZReplayManager.showErrors)
				Debug.LogError("EZReplayManager ERROR: You probably already inserted at position '"+recorderPosition+"' for game object '"+gameObject+"'.");
		}
	}

    public int removeFramesOutOfBufferRange() {
        List<int> framesToDelete = new List<int>();

        int lowestFrame = getLastKeyFrameBefore(EZReplayManager.get.getLowestFrame());
        foreach (var stateEntry in savedStates) {
            int frame = stateEntry.Key;
            if (frame < lowestFrame) {
                framesToDelete.Add(frame);
                //Debug.Log("frameToDelete: " + frame);
            } else {
                break;
            }
        }

        foreach (var frame in framesToDelete) {
            savedStates.Remove(frame);
        }

        return framesToDelete.Count;
    }

    //synchronize gameObjectClone to a certain state at a certain  recorderPosition
    public void synchronizeProperties(int recorderPosition, bool noLerp) {

        bool lerp = false;
        if (gameObjectClone.activeInHierarchy)
            lerp = true;

        if (noLerp) {
            lerp = false;
        }

        if (firstChangedFrame >= 0 && recorderPosition >= firstChangedFrame) {
            if (recorderPosition < lastChangedFrame) {

                getStateAtPos(recorderPosition).synchronizeProperties(gameObjectClone, lerp);

            } else {
                getStateAtPos(lastChangedFrame).synchronizeProperties(gameObjectClone, lerp);
            }

        } else {


            try {
                savedStates[0].synchronizeProperties(gameObjectClone, lerp);
            } catch (KeyNotFoundException) {
                gameObjectClone.SetActive(false);
            }

        }


    }

    protected SavedState getStateAtPos(int recorderPosition) {
        try {
            return savedStates[recorderPosition];
        } catch (KeyNotFoundException) {
            return savedStates[getLastKeyFrameBefore(recorderPosition)];
        }
	}	
	
	public int getAmountStates() {
		return savedStates.Count;
	}
	
	public void clearStates() {
		savedStates.Clear();
	}
	
	/*public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
	{
		info.AddValue("savedStates", this.savedStates);
		info.AddValue("isParentObj", this.isParentObj);
		info.AddValue("parentMapping", this.parentMapping);
		info.AddValue("childNo", this.childNo);
		info.AddValue("prefabLoadPath", this.prefabLoadPath);
		info.AddValue("lastChangedFrame", this.lastChangedFrame);
		info.AddValue("firstChangedFrame", this.firstChangedFrame);
		info.AddValue("gameObjectName", this.gameObjectName);
		info.AddValue("childIdentificationMode", this.childIdentificationMode);
		//base.GetObjectData(info, context);
	}*/		
	
}