using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelCreatorManager : MonoBehaviour {

    /// <summary>
    /// The scriptable object to hold this level's information
    /// Do: assets->create->LevelInfo, drag and drop created Scriptable Object
    /// corresponding to level name in here
    /// </summary>
    [SerializeField]
    private LevelInformation theLevelInformation;

	// Use this for initialization
	void Start ()
    {
        theLevelInformation = SerializeLevelIntoScriptObj(theLevelInformation);
    }

    /// <summary>
    /// Compacts an entire level into a nice scriptable object 
    /// TagList is the list of tags used in this level
    /// </summary>
    public static LevelInformation SerializeLevelIntoScriptObj(LevelInformation theLevelInfo)
    {
        if (theLevelInfo != null)
        {
            Debug.Log("Serializing " + theLevelInfo.name + " into scriptable object");
            Dictionary<string, List<string>> tagGameObjectMap = new Dictionary<string, List<string>>();
            Dictionary<string, string> theSwtichControlDict = new Dictionary<string, string>();

            if(theLevelInfo.tagList.Count < 1)
            {
                Debug.Log("Forgot to type tags in! Type the tags you used for each item to be serialized");
            }

            foreach (string t in theLevelInfo.tagList)
            {
                GameObject[] t_arr = GameObject.FindGameObjectsWithTag(t);
                switch(t)
                {
                    case "Switcher":
                        //Add switcher and its position, and what tag switcher references
                        //Serialized in form: [name:pos:rot:scale , tag I reference]
                        //For example: [Lever:0:0:0 , "Lever_0"}
                        foreach (GameObject l in t_arr)
                        {
                            PositionRotationScale prs = new PositionRotationScale();
                            prs.position = l.transform.position;
                            prs.rotation = l.transform.rotation;
                            prs.scaler = l.transform.localScale;
                            string key_string = l.name + ":" + prs.position.ToString() + ":" + prs.rotation.ToString() + ":" + prs.scaler.ToString();
                            string tagToRef = l.GetComponent<Switch>().objTag;
                            theSwtichControlDict.Add(key_string, tagToRef);
                        }
                        break;
                    case "Wall":
                    case "Lever_0":
                    case "Lever_1":
                    case "Lever_2":
                    case "Lever_3":                    
                    case "Enemy":
                    case "Spikes_0":
                    case "Spikes_1":
                        //Serialized in form = [tag , pos:rot:scale]
                        //For example: [Lever_0 , 5:5:5]
                        foreach (GameObject go in t_arr)
                        {
                            LevelCreatorManager.SimpleAddToTagGameObjectMap(tagGameObjectMap, t, go);
                        }
                        break;
                    default:
                        break;
                }
            }

            //Serialize simple walls and doors with name and position information
            theLevelInfo.tagGameObjectListJSON = JsonConvert.SerializeObject(tagGameObjectMap);
            Debug.Log(theLevelInfo.tagGameObjectListJSON);

            //Serialize levers with control information
            theLevelInfo.switcherControlJSON = JsonConvert.SerializeObject(theSwtichControlDict);

            //Only in level creation, don't set playerStartPosition again
            if(GameObject.FindGameObjectWithTag("PlayerStartPosition") != null)
            {
                theLevelInfo.playerStartPosition = GameObject.FindGameObjectWithTag("PlayerStartPosition").gameObject.transform.position;
            }

            #if UNITY_EDITOR
            //Save ScriptableObject when we are done adding data
            EditorUtility.SetDirty(theLevelInfo);
            AssetDatabase.SaveAssets();
            #endif
        }
        else
        {
            Debug.LogWarning("No levelInfo to build from!");
        }
        return theLevelInfo;
    }

    /// <summary>
    /// Adds simple gameobject, only requiring name and position/rotation/scale, to serialize for later instantiation
    /// </summary>
    /// <param name="tagMap"></param>
    /// <param name="tagKeyToAdd"></param>
    /// <param name="objToAdd"></param>
    public static void SimpleAddToTagGameObjectMap(Dictionary<string, List<string>> tagMap, string tagKeyToAdd, GameObject objToAdd)
    {
        PositionRotationScale prs = new PositionRotationScale();
        prs.position = objToAdd.transform.position;
        prs.rotation = objToAdd.transform.rotation;
        prs.scaler = objToAdd.transform.localScale;
        string val_string = prs.position.ToString() + ":" + prs.rotation.ToString() + ":" + prs.scaler.ToString();

        if (!tagMap.ContainsKey(tagKeyToAdd))
        {
            List<string> prsList = new List<string>();
            prsList.Add(val_string);
            tagMap.Add(tagKeyToAdd, prsList);
        }
        else
        {
            tagMap[tagKeyToAdd].Add(val_string);
        }
    }
}
