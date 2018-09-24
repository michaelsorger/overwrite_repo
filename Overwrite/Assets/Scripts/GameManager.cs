using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The level being manipulated
    /// </summary>
    public LevelToSave theLevelToSave;

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

    private int badCounter = 0;

    void Start()
    {
        //Loop through all the levels, and instantiate the level set by a string matching a script obj level name
        foreach(LevelInformation LvlInfo in theLevelBank)
        {
            if(LvlInfo.name.Equals(theLevelToPlay))
            {
                //Instantiate walls at there position and rotation
                foreach(PositionRotationScale wprs in LvlInfo.wallPRSList)
                {
                    GameObject newObject = GameObject.Instantiate(theWallPrefab, wprs.position, wprs.rotation);
                    newObject.transform.localScale = wprs.scaler;
                }
                
                //Instantiate doors at there position and rotation
                foreach(PositionRotationScale dt in LvlInfo.doorPRSList)
                {
                    GameObject newObject = GameObject.Instantiate(theDoorPrefab, dt.position, dt.rotation);
                    newObject.transform.localScale = dt.scaler;
                }

                //Deserialize JSON string for levers in scriptable object
                Dictionary<string, List<string>> switchControlDict = new Dictionary<string, List<string>>();
                switchControlDict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(LvlInfo.switchControlJSON);

                //Instantiate lever, and add references obtained from json serialized object
                foreach(KeyValuePair<string, List<string>> leverToRefs in switchControlDict)
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

                GameObject.Instantiate(thePlayerPrefab, LvlInfo.playerStartPosition, thePlayerPrefab.transform.rotation);

                theLevelToSave.savedLevelInformation = LvlInfo;
                theLevelToSave.changedLevelInformation = LvlInfo;
            }
        }
    }

    /// <summary>
    /// Takes a string in the form "boolean" and returns the boolean value
    /// </summary>
    /// <param name="sString"></param>
    /// <returns></returns>
    public bool getSwtch(string sString)
    {
        Debug.Log(sString);
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
    public Vector3 getVector3(string rString)
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
    public Quaternion getRotation3(string qString)
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

}

/// <summary>
/// ScriptObjects to manipulate during gameplay
/// </summary>
public struct LevelToSave
{
    public LevelInformation savedLevelInformation;
    public LevelInformation changedLevelInformation;
}
