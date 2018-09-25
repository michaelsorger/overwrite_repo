using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Condition on which object will be triggered
/// Triggers when interacted with
/// </summary>
public class ActivateOnInteract : MonoBehaviour
{

    /// <summary>
    /// Reference to a trigger
    /// </summary>
    public TriggersClass trigger;

    /// <summary>
    /// Bool checks if object in radius of object
    /// </summary>
    bool inRadius;

    void OnTriggerEnter()
    {
        inRadius = true;
    }
    void OnTriggerExit()
    {
        inRadius = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown("e") && inRadius)
        {
            trigger.trigger = !trigger.trigger;
        }
    }
}
