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
    /// The other game object to manipulate
    /// </summary>
    public GameObject otherGameObject;

    /// <summary>
    /// The path follower associated with other game object
    /// </summary>
    private PathFollower pathFollower;
    
    void Start()
    {
        pathFollower = otherGameObject.GetComponentInChildren<PathFollower>();
    }

    /// <summary>
    /// Ons the Trigger Stay
    /// </summary>
    /// <param name="box"></param>
    void OnTriggerStay(Collider collider)
    {
        Debug.Log("KeyController OnTriggerStay");
        pathFollower.condition = true;
        theSmallInventory.keyList.Add(this.name);
        //gameObject.SetActive(false);
    }
}
