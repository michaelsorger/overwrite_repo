using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

    /// <summary>
    /// The condition to move
    /// </summary>
    public bool condition;

    /// <summary>
    /// The position to move to
    /// </summary>
    public Vector3 moveTo;

    /// <summary>
    /// The speed with which to move
    /// </summary>
    public float speed;

	// Update is called once per frame
	void Update ()
    {
        if(condition)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, speed);
            if (transform.position == moveTo)
            {
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Ons the Trigger Enter
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && this.gameObject.tag != "Raiser")
        {
            Debug.Log("YOU HAVE DIED");
            Destroy(collider.gameObject);
        }
    }
}
