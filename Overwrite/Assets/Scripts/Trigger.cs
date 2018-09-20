using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// One time trigger for objects on/off
/// </summary>
public class Trigger : MonoBehaviour {

    /// <summary>
    /// Needed to check bool isSeen
    /// </summary>
    public EnemySight sight;

    /// <summary>
    /// Bool used if the objects being affected should start on
    /// then turn off when triggered
    /// </summary>
    public bool isOnStart;

    /// <summary>
    /// Game objects being affected by trigger
    /// </summary>
    public GameObject[] objs;

    private void Start()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].SetActive(isOnStart);
        }
    }

    private void Update()
    {
        if (sight.isSeen)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].SetActive(!isOnStart);
            }
        }
    }
}
