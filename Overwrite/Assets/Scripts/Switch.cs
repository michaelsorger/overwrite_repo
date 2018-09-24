using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Back and forth switch for objects on/off
/// </summary>
public class Switch : MonoBehaviour {

    /// <summary>
    /// Game objects by tag being affected by switch
    /// </summary>
    public List<string> objTags;

    /// <summary>
    /// Switch on/off
    /// </summary>
    public bool swtch;

    /// <summary>
    /// List of objects to manipulate, defined in levelcreate scene
    /// </summary>
    [SerializeField]
    private List<GameObject> objs;

    private bool trigger;

    /// <summary>
    /// Sets objects referenced by this lever to be active or inactive
    /// </summary>
	void GetCondition () {

        if (swtch)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].SetActive(false);
            }
        }
	}

    void OnTriggerEnter()
    {
        trigger = true;
    }
    void OnTriggerExit()
    {
        trigger = false;
    }

    /// <summary>
    /// If we have access to objects, then add to objtags list on serialization
    /// If we dont have access to objects, then add to objs list on deserialization
    /// </summary>
    /// <param name="hasObjects"></param>
    /// <param name="sw"></param>
    public void UpdateSwitchList(bool hasObjects, bool sw)
    {
        if(hasObjects)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                objTags.Add(objs[i].tag);
            }
        }
        else
        {
            for (int i = 0; i < objTags.Count; i++)
            {
                while(GameObject.FindWithTag(objTags[i]) != null)
                {
                    GameObject go = GameObject.FindWithTag(objTags[i]);
                    objs.Add(go);
                    go.SetActive(!sw);
                }
            }
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].SetActive(sw);
            }
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            if (Input.GetKeyDown("e"))
            {
                swtch = !swtch;
                GetCondition();
            }
        }
    }
}
