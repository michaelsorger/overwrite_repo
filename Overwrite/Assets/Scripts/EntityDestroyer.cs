using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDestroyer : MonoBehaviour {

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player" || collider.tag == "Enemy")
        {
            Destroy(collider.transform.root.gameObject);
        }
    }
}
