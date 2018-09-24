using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Back and forth switch trigger for objects on/off
/// </summary>
public class TwoWayTrigger : TriggersClass {

    /// <summary>
    /// Bool used if the objects being affected should start on
    /// then turn off when triggered
    /// </summary>
    public bool[] isOnStartObjs;

    /// <summary>
    /// Bool used if the components being affected should start on
    /// then turn off when triggered
    /// </summary>
    public bool[] isOnStartComps;

    /// <summary>
    /// Game objects being affected by switch
    /// </summary>
    public GameObject[] objs;

    /// <summary>
    /// Components being affected by trigger
    /// </summary>
    public MonoBehaviour[] comps;

    /// <summary>
    /// Used to check if trigger bool has changed
    /// </summary>
    bool lastTrigger;

    private void DoSwitch()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].activeSelf)
                objs[i].SetActive(false);
            else
                objs[i].SetActive(true);
        }
        for (int i = 0; i < comps.Length; i++)
        {
            if (comps[i].isActiveAndEnabled)
                comps[i].enabled = false;
            else
                comps[i].enabled = true;
        }
    }

    private void Start()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].SetActive(isOnStartObjs[i]);
        }
        for (int i = 0; i < comps.Length; i++)
        {
            comps[i].enabled = isOnStartComps[i];
        }
    }

    void Update()
    {
        if (lastTrigger != trigger)
            DoSwitch();
        lastTrigger = trigger;
    }
}
