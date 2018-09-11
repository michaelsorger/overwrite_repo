using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour {

    /// <summary>
    /// The key name required
    /// </summary>
    public string key_required;

    /// <summary>
    /// The small inventory
    /// </summary>
    [SerializeField]
    private SmallInventory theSmallInventory;

    /// <summary>
    /// Ons the trigger enter
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerEnter(Collider coll)
    {
        if(theSmallInventory.keyList.Contains(key_required))
        {
            theSmallInventory.keyList.Add(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
