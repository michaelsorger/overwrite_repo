using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour {

    /// <summary>
    /// Needed to check bool isSeen
    /// </summary>
    public EnemySight sight;

    private void Start()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }

    private void Update()
    {
        if (sight.isSeen)
        {
            GetComponent<Collider>().enabled = true;
            GetComponent<Renderer>().enabled = true;
        }
    }
}
