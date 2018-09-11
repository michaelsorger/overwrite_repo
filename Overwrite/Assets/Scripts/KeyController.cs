using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {

    /// <summary>
    /// The small inventory
    /// </summary>
    [SerializeField]
    private SmallInventory theSmallInventory;

    /// <summary>
    /// Ons the Trigger Enter
    /// </summary>
    /// <param name="box"></param>
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("KeyController OnTriggerEnter");
        theSmallInventory.keyList.Add(this.name);
        gameObject.SetActive(false);
    }
}
