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
        if(theLevelInformation != null)
        {
            //Arrays of objects to add into scriptable object, don't add duplicates
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
            GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
            GameObject[] levers = GameObject.FindGameObjectsWithTag("Lever");

            //Add walls to LevelInformation
            foreach (GameObject w in walls)
            {
                PositionRotationScale prs = new PositionRotationScale();
                prs.position = w.transform.position;
                prs.rotation = w.transform.rotation;
                prs.scaler = w.transform.localScale;

                if(!theLevelInformation.wallPRSList.Contains(prs))
                {
                    theLevelInformation.wallPRSList.Add(prs);
                }       
            }

            //Add doors to LevelInformation
            foreach (GameObject d in doors)
            {
                PositionRotationScale prs = new PositionRotationScale();
                prs.position = d.transform.position;
                prs.rotation = d.transform.rotation;
                prs.scaler = d.transform.localScale;

                if (!theLevelInformation.doorPRSList.Contains(prs))
                {
                    theLevelInformation.doorPRSList.Add(prs);
                }
            }

            Dictionary<string, List<string>> theSwtichControlDict = new Dictionary<string, List<string>>();

            //Add levers, and reference to levers
            foreach (GameObject l in levers)
            {
                PositionRotationScale prs = new PositionRotationScale();
                prs.position = l.transform.position;
                prs.rotation = l.transform.rotation;
                prs.scaler = l.transform.localScale;
                string key_string = prs.position.ToString() + ":" + prs.rotation.ToString() + ":" + prs.scaler.ToString() + ":" + l.GetComponent<Switch>().swtch.ToString().ToLower();
                List<string> tagList = l.GetComponent<Switch>().objTags;
                theSwtichControlDict.Add(key_string, tagList);
            }

            //Serialize levers with control information
            theLevelInformation.switchControlJSON = JsonConvert.SerializeObject(theSwtichControlDict);

            //Add player start position to LevelInformation (can be overwritten)
            theLevelInformation.playerStartPosition = GameObject.FindGameObjectWithTag("PlayerStartPosition").transform.position;

            //Save ScriptableObject when we are done adding data
            EditorUtility.SetDirty(theLevelInformation);
            AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.LogWarning("A LevelInfo scriptable object does not exist in the LevelCreatorManager");
        }
    }
}
