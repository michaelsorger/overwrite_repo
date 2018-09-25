using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

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
            Dictionary<string, List<string>> tagGameObjectMap = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> theSwtichControlDict = new Dictionary<string, List<string>>();

            foreach (string t in theLevelInfo.tagList)
            {
                GameObject[] t_arr = GameObject.FindGameObjectsWithTag(t);
                switch(t)
                {
                    case "Lever":
                        //Add levers, and reference to levers
                        foreach (GameObject l in t_arr)
                        {
                            PositionRotationScale prs = new PositionRotationScale();
                            prs.position = l.transform.position;
                            prs.rotation = l.transform.rotation;
                            prs.scaler = l.transform.localScale;
                            string key_string = prs.position.ToString() + ":" + prs.rotation.ToString() + ":" + prs.scaler.ToString() + ":" + l.GetComponent<Switch>().swtch.ToString().ToLower();
                            List<string> tagList = l.GetComponent<Switch>().objTags;
                            theSwtichControlDict.Add(key_string, tagList);
                        }
                        break;

                    case "Wall":
                    case "Lever_0":
                    case "Lever_1":
                    case "Lever_2":
                    case "Lever_3":
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

            //Serialize levers with control information
            theLevelInfo.switchControlJSON = JsonConvert.SerializeObject(theSwtichControlDict);

            //Save ScriptableObject when we are done adding data
            EditorUtility.SetDirty(theLevelInfo);
            AssetDatabase.SaveAssets();
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
            List<string> wallPrsList = new List<string>();
            wallPrsList.Add(val_string);
            tagMap.Add(tagKeyToAdd, wallPrsList);
        }
        else
        {
            tagMap[tagKeyToAdd].Add(val_string);
        }
    }
}
