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
    /// Switch on/off, when in levelcreate scene, convention is to make this true
    /// </summary>
    public bool swtch;

    /// <summary>
    /// List of objects to manipulate, defined in levelcreate scene
    /// </summary>
    [SerializeField]
    private List<GameObject> objs;

    private bool trigger;

    /// <summary>
    /// Sets objects referenced by this lever to be rotated open or closed
    /// (change me to be more generic later, inactive causes problems with serialization though)
    /// </summary>
	void GetCondition () {

        if (swtch)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                if(objs[i].transform.position.y < 0)
                {
                    objs[i].transform.position += Vector3.up;
                }
                else
                {
                    objs[i].transform.position += Vector3.down;
                }

            }
        }
        else
        {
            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i].transform.position.y < 0)
                {
                    objs[i].transform.position += Vector3.up;
                }
                else
                {
                    objs[i].transform.position += Vector3.down;
                }
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
    /// Update list of GameObject references using tags defined from serialization
    /// </summary>
    /// <param name="hasObjects"></param>
    /// <param name="sw"></param>
    public void UpdateSwitchObjRefList()
    {
        for (int i = 0; i < objTags.Count; i++)
        {   
            //set each found obj to false, otherwise infinite loop
            while(GameObject.FindWithTag(objTags[i]) != null)
            {
                GameObject go = GameObject.FindWithTag(objTags[i]);
                objs.Add(go);
                go.SetActive(false);
            }
        }

        //Make obj visible again
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].SetActive(true);
        }  
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            if (Input.GetKeyDown("e"))
            {
                GetCondition();
                swtch = !swtch;
            }
        }
    }
}
