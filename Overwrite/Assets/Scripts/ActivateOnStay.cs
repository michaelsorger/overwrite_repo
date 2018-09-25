using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Condition on which object will be triggered
/// Keeps triggered when stood on
/// </summary>
public class ActivateOnStay : MonoBehaviour
{

    /// <summary>
    /// Reference to a trigger
    /// </summary>
    public TriggersClass trigger;

    /// <summary>
    /// Keeps trigger same val onStay
    /// </summary>
    bool keepTrigger;

    public void OnTriggerEnter(Collider collider)
    {
        keepTrigger = !trigger.trigger;
    }

    private void OnTriggerStay(Collider collider)
    {
        trigger.trigger = keepTrigger;
    }

    private void OnTriggerExit(Collider collider)
    {
        trigger.trigger = !keepTrigger;
    }
}
