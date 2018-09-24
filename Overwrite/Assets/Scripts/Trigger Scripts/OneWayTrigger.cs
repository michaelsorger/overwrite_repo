using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One time trigger for objects on/off
/// </summary>
public class OneWayTrigger : TriggersClass {

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
    /// Game objects being affected by trigger
    /// </summary>
    public GameObject[] objs;

    /// <summary>
    /// Components being affected by trigger
    /// </summary>
    public MonoBehaviour[] comps;

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

    private void Update()
    {
        if (trigger)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].SetActive(!isOnStartObjs[i]);
            }
            for (int i = 0; i < comps.Length; i++)
            {
                comps[i].enabled = !isOnStartComps[i];
            }
        }
    }
}
