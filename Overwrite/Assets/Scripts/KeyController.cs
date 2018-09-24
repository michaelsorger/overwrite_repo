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
    /// Ons the Trigger Stay
    /// </summary>
    /// <param name="box"></param>
    void OnTriggerStay(Collider collider)
    {
        theSmallInventory.keyList.Add(this.name);
        //gameObject.SetActive(false);
    }
}
