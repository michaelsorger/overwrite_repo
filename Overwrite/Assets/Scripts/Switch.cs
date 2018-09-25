using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Back and forth switch for objects on/off
/// </summary>
public class Switch : MonoBehaviour {


    /// <summary>
    /// Game objects being affected by switch
    /// </summary>
    public GameObject[] objs;

    /// <summary>
    /// Switch on/off
    /// </summary>
    public bool swtch;

    private bool trigger;

	// Update is called once per frame
	void GetCondition () {

        if (swtch)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < objs.Length; i++)
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

    void Update()
    {
        if (trigger)
        {
            if (Input.GetKeyDown("e"))
            {
                swtch = !swtch;
            }
            GetCondition();
        }
    }
}
