using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

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
    public static GameObject StaticWallPrefab;

    /// <summary>
    /// The player character prefab
    /// </summary>
    [SerializeField]
    private GameObject thePlayerPrefab;
    public static GameObject StaticPlayerPrefab;

    /// <summary>
    /// The door prefab
    /// </summary>
    [SerializeField]
    private GameObject theDoorPrefab;
    public static GameObject StaticDoorPrefab;

    /// <summary>
    /// The lever prefab
    /// </summary>
    [SerializeField]
    private GameObject theLeverPrefab;
    public static GameObject StaticLeverPrefab;

    /// <summary>
    /// The enemy prefab
    /// </summary>
    [SerializeField]
    private GameObject theEnemyPrefab;
    public static GameObject StaticEnemyPrefab;

    /// <summary>
    /// The lever prefab
    /// </summary>
    [SerializeField]
    private GameObject theSpikePrefab;
    public static GameObject StaticSpikePrefab;

    /// <summary>
    /// The script obj holding data to teleport to
    /// </summary>
    [SerializeField]
    private PositionPoints thePosPoints;
    public static PositionPoints StaticPositionPoints;

    /// <summary>
    /// The save world button
    /// </summary>
    [SerializeField]
    private Button theSaveWorldButton;

    /// <summary>
    /// The load world button
    /// </summary>
    [SerializeField]
    private Button theLoadWorldButton;

    /// <summary>
    /// The save position button
    /// </summary>
    [SerializeField]
    private Button theSavePositionButton;

    /// <summary>
    /// The load position button
    /// </summary>
    [SerializeField]
    private Button theLoadPositionButton;

    /// <summary>
    /// The scriptable object to change, save, and load from
    /// other scripts can reference this if changes are needed
    /// </summary>
    public static LevelInformation theTempLevel;

    private static bool EnemySpawned;

    void Start()
    {
        //Assign static's so scripts can access properties when needed
        StaticWallPrefab = theWallPrefab;
        StaticPlayerPrefab = thePlayerPrefab;
        StaticDoorPrefab = theDoorPrefab;
        StaticLeverPrefab = theLeverPrefab;
        StaticEnemyPrefab = theEnemyPrefab;
        StaticPositionPoints = thePosPoints;
        StaticSpikePrefab = theSpikePrefab;

        EnemySpawned = false;

        //Loop through all the levels, and instantiate the level set by a string matching a script obj level name
        foreach (LevelInformation LvlInfo in theLevelBank)
        {
            if(LvlInfo.name.Equals(theLevelToPlay))
            {
                //Deserialize level
                DeserializeFromScriptObj(LvlInfo);

                //Update temp script obj with level
                theTempLevel = ScriptableObject.CreateInstance<LevelInformation>();
                theTempLevel.tagList = LvlInfo.tagList;
                theTempLevel.tagGameObjectListJSON = LvlInfo.tagGameObjectListJSON;
                theTempLevel.switcherControlJSON = LvlInfo.switcherControlJSON;
                theTempLevel.oneWaySwitcherControlJSON = LvlInfo.oneWaySwitcherControlJSON;
                theTempLevel.playerStartPosition = LvlInfo.playerStartPosition;

                //Update ui buttons to map to player
                theSaveWorldButton.GetComponent<Button>().onClick.AddListener(CallSaveWorld);
                theLoadWorldButton.GetComponent<Button>().onClick.AddListener(CallLoadWorld);
                theSavePositionButton.GetComponent<Button>().onClick.AddListener(CallSavePosition);
                theLoadPositionButton.GetComponent<Button>().onClick.AddListener(CallLoadPosition);

                break;
            }
        }
    }

    /// <summary>
    /// Ui button functionality for player
    /// </summary>
    private void CallSaveWorld()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SaveWorld();
    }
    private void CallLoadWorld()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().LoadWorldHelper();
    }
    private void CallSavePosition()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SavePosition();
    }
    private void CallLoadPosition()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().LoadPosition();
    }

    /// <summary>
    /// Deserializes the scriptable object and instantiates each object by its tag into the level
    /// </summary>
    /// <param name="theLevelInfo"></param>
    /// <param name="theWallPrefab"></param>
    /// <param name="theDoorPrefab"></param>
    /// <param name="thePlayerPrefab"></param>
    /// <param name="theLeverPrefab"></param>
    /// <param name="thePosPoints"></param>
    public static void DeserializeFromScriptObj(LevelInformation theLevelInfo)
    {
        if(theLevelInfo != null)
        {
            Debug.Log("Deserializing " + theLevelInfo.name + " scriptable object and instantiating the following:");

            //Instantiate only player
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (!go)
            {
                Debug.Log("The gameobject with tag = Player");
                GameObject.Instantiate(StaticPlayerPrefab, theLevelInfo.playerStartPosition, StaticPlayerPrefab.transform.rotation);
                StaticPositionPoints.positionPoint = theLevelInfo.playerStartPosition;
            }

            //Deserialize JSON string for simple objects in scriptable object
            Dictionary<string, List<string>> tagGameObjMap = new Dictionary<string, List<string>>();
            tagGameObjMap = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(theLevelInfo.tagGameObjectListJSON);

            //TagGameObjectList = Tag of prefab : list positions to instantiate the prefab
            if (!string.IsNullOrEmpty(theLevelInfo.tagGameObjectListJSON) || !string.IsNullOrEmpty(theLevelInfo.switcherControlJSON))
            {
                foreach (KeyValuePair<string, List<string>> tagToObjList in tagGameObjMap)
                {
                    Debug.Log("Each gameobject with tag = " + tagToObjList.Key);
                    switch (tagToObjList.Key)
                    {
                        case "Wall":
                            foreach (string s in tagToObjList.Value)
                            {
                                InstantiateSimplePrefab(StaticWallPrefab, tagToObjList.Key, s);
                            }
                            break;
                        case "Lever_0":
                        case "Lever_1":
                        case "Lever_2":
                        case "Lever_3":
                            foreach (string s in tagToObjList.Value)
                            {
                                InstantiateSimplePrefab(StaticDoorPrefab, tagToObjList.Key, s);
                            }
                            break;
                        case "Enemy":
                            GameObject enemyObjCheck = GameObject.FindGameObjectWithTag("Enemy");
                            if(!enemyObjCheck && EnemySpawned == false)
                            {
                                foreach (string s in tagToObjList.Value)
                                {
                                    GameObject enemyObj = InstantiateSimplePrefab(StaticEnemyPrefab, tagToObjList.Key, s);
                                    enemyObj.GetComponent<EnemyAI>().player = GameObject.FindGameObjectWithTag("Player");
                                }
                                EnemySpawned = true;
                            }
                            break;
                        case "Spikes_0":
                            foreach (string s in tagToObjList.Value)
                            {
                                InstantiateSimplePrefab(StaticSpikePrefab, tagToObjList.Key, s);
                            }
                            break;
                    }
                }

                //Deserialize JSON string for levers in scriptable object
                Dictionary<string, string> switcherControlDict = new Dictionary<string, string>();
                switcherControlDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(theLevelInfo.switcherControlJSON);

                //Instantiate Switcher, and add references obtained from json serialized object
                foreach (KeyValuePair<string, string> switchToRef in switcherControlDict)
                {
                    string[] words = switchToRef.Key.Split(':');
                    string switchName = words[0];
                    PositionRotationScale prs = new PositionRotationScale();
                    prs.position = getVector3(words[1]);
                    prs.rotation = getRotation3(words[2]);
                    prs.scaler = getVector3(words[3]);
                    InstantiateSwitchPrefab(switchName, prs.position, prs.rotation, prs.scaler, switchToRef.Value);
                }
            }
        }      
    }

    /// <summary>
    /// Instantiates a switch prefab at a position and rotation
    /// the switch prefab name (defined in serialization and pulled from json), determines what switch to instaniate
    /// add more cases for more switch prefabs
    /// </summary>
    /// <param name="thisSimpleValue"></param>
    public static void InstantiateSwitchPrefab(string theSwitchPrefabName, Vector3 pos, Quaternion rot, Vector3 scaler, string tagRef)
    {
        switch(theSwitchPrefabName)
        {
            case "Lever":
                GameObject newObj = GameObject.Instantiate(StaticLeverPrefab, pos, rot);
                newObj.transform.localScale = scaler;
                newObj.GetComponent<Switch>().swtch = true;
                newObj.GetComponent<Switch>().objTag = tagRef;
                newObj.GetComponent<Switch>().UpdateSwitchObjRefList();
                newObj.tag = "Switcher";
                newObj.name = "Lever";
                break;
        }
    }

    /// <summary>
    /// A simple prefab is that which only needs to be instantiated via name and position/rotation/scalar
    /// </summary>
    /// <param name="thisSimpleValue"></param>
    public static GameObject InstantiateSimplePrefab(GameObject theSimplePrefab, string thisKey, string thisSimpleValue)
    {
        string[] words = thisSimpleValue.Split(':');
        PositionRotationScale prs = new PositionRotationScale();
        prs.position = getVector3(words[0]);
        prs.rotation = getRotation3(words[1]);
        prs.scaler = getVector3(words[2]);
        GameObject newObject = GameObject.Instantiate(theSimplePrefab, prs.position, prs.rotation);
        newObject.transform.localScale = prs.scaler;
        newObject.tag = thisKey;
        return newObject;
    }

    /// <summary>
    /// Takes a string in the form "boolean" and returns the boolean value
    /// </summary>
    /// <param name="sString"></param>
    /// <returns></returns>
    public static bool getBool(string sString)
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

}