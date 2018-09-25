using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// User set level to play
    /// </summary>
    [SerializeField]
    private string theLevelToPlay;
    
    /// <summary>
    /// The list of valid sccriptable object levels
    /// </summary>
    [SerializeField]
    private List<LevelInformation> theLevelBank;

    /// <summary>
    /// The horizontal platform prefab
    /// </summary>
    [SerializeField]
    private GameObject theWallPrefab;

    /// <summary>
    /// The player character prefab
    /// </summary>
    [SerializeField]
    private GameObject thePlayerPrefab;

    /// <summary>
    /// The door prefab
    /// </summary>
    [SerializeField]
    private GameObject theDoorPrefab;

    /// <summary>
    /// The lever prefab
    /// </summary>
    [SerializeField]
    private GameObject theLeverPrefab;

    /// <summary>
    /// The script obj holding data to teleport to
    /// </summary>
    [SerializeField]
    private PositionPoints thePosPoints;

    private int badCounter = 0;

    void Start()
    {
        //Loop through all the levels, and instantiate the level set by a string matching a script obj level name
        foreach(LevelInformation LvlInfo in theLevelBank)
        {
            if(LvlInfo.name.Equals(theLevelToPlay))
            {
                DeserializeFromScriptObj(LvlInfo, theWallPrefab, theDoorPrefab, thePlayerPrefab, theLeverPrefab, thePosPoints);
                break;
            }
        }
    }

    public static void DeserializeFromScriptObj(LevelInformation theLevelInfo, GameObject theWallPrefab, GameObject theDoorPrefab,
        GameObject thePlayerPrefab, GameObject theLeverPrefab, PositionPoints thePosPoints)
    {
        //Deserialize JSON string for simple objects in scriptable object
        Dictionary<string, List<string>> tagGameObjMap = new Dictionary<string, List<string>>();
        tagGameObjMap = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(theLevelInfo.tagGameObjectListJSON);

        foreach (KeyValuePair<string, List<string>> tagToObjList in tagGameObjMap)
        {
            Debug.Log(tagToObjList.Key + " in DeserializeFromScriptObj");
            switch (tagToObjList.Key)
            {
                case "Wall":
                    foreach (string s in tagToObjList.Value)
                    {
                        InstantiateSimplePrefab(theWallPrefab, tagToObjList.Key, s);
                    }
                    break;
                case "Lever_0":
                case "Lever_1":
                case "Lever_2":
                case "Lever_3":
                    foreach (string s in tagToObjList.Value)
                    {
                        InstantiateSimplePrefab(theDoorPrefab, tagToObjList.Key, s);
                    }
                    break;
            }
        }

        //Deserialize JSON string for levers in scriptable object
        Dictionary<string, List<string>> switchControlDict = new Dictionary<string, List<string>>();
        switchControlDict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(theLevelInfo.switchControlJSON);

        //Instantiate lever, and add references obtained from json serialized object
        foreach (KeyValuePair<string, List<string>> leverToRefs in switchControlDict)
        {
            string[] words = leverToRefs.Key.Split(':');
            PositionRotationScale prs = new PositionRotationScale();
            prs.position = getVector3(words[0]);
            prs.rotation = getRotation3(words[1]);
            prs.scaler = getVector3(words[2]);
            GameObject newObject = GameObject.Instantiate(theLeverPrefab, prs.position, prs.rotation);
            newObject.transform.localScale = prs.scaler;
            newObject.GetComponent<Switch>().objTags = leverToRefs.Value;
            newObject.GetComponent<Switch>().swtch = getSwtch(words[3]);
            newObject.GetComponent<Switch>().UpdateSwitchList(false, newObject.GetComponent<Switch>().swtch);
        }

        if(GameObject.FindGameObjectWithTag("Player") == null)
            GameObject.Instantiate(thePlayerPrefab, theLevelInfo.playerStartPosition, thePlayerPrefab.transform.rotation);
        thePosPoints.positionPoint = theLevelInfo.playerStartPosition;
    }

    /// <summary>
    /// Takes a string in the form "boolean" and returns the boolean value
    /// </summary>
    /// <param name="sString"></param>
    /// <returns></returns>
    public static bool getSwtch(string sString)
    {
        bool ret;
        if(sString.Equals("true"))
        {
            ret = true;
        }
        else
        {
            ret = false;
        }
        return ret;
    }

    /// <summary>
    /// Takes a string in the format "(x,y,z)" and returns a quaternion value 
    /// </summary>
    /// <param name="rString"></param>
    /// <returns></returns>
    public static Vector3 getVector3(string rString)
    {
        string newString = "";
        char[] rCharArr = rString.ToCharArray();
        for (int i = 1; i < rCharArr.Length-1; i++)
        {
            newString = newString + rCharArr[i];
        }

        string[] temp = newString.Split(',');
        float x = float.Parse(temp[0]);
        float y = float.Parse(temp[1]);
        float z = float.Parse(temp[2]);
        Vector3 rValue = new Vector3(x, y, z);
        return rValue;
    }

    /// <summary>
    /// Takes a string in the format "(x,y,z,w)" and returns a quaternion value
    /// </summary>
    /// <param name="qString"></param>
    /// <returns></returns>
    public static Quaternion getRotation3(string qString)
    {
        string newString = "";
        char[] rCharArr = qString.ToCharArray();
        for (int i = 1; i < rCharArr.Length - 1; i++)
        {
            newString = newString + rCharArr[i];
        }

        string[] temp = newString.Split(',');
        float x = float.Parse(temp[0]);
        float y = float.Parse(temp[1]);
        float z = float.Parse(temp[2]);
        float w = float.Parse(temp[3]);
        Quaternion qValue = new Quaternion(x, y, z, w);
        return qValue;
    }

    /// <summary>
    /// A simple prefab is that which only needs to be instantiated via name and position/rotation/scalar
    /// </summary>
    /// <param name="thisSimpleValue"></param>
    public static void InstantiateSimplePrefab(GameObject theSimplePrefab, string thisKey, string thisSimpleValue)
    {
        string[] words = thisSimpleValue.Split(':');
        PositionRotationScale prs = new PositionRotationScale();
        prs.position = getVector3(words[0]);
        prs.rotation = getRotation3(words[1]);
        prs.scaler = getVector3(words[2]);
        GameObject newObject = GameObject.Instantiate(theSimplePrefab, prs.position, prs.rotation);
        newObject.transform.localScale = prs.scaler;
        newObject.tag = thisKey;
    }

}