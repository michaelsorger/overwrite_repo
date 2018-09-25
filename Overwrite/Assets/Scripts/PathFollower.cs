using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

    /// <summary>
    /// The condition to move
    /// </summary>
    public bool condition;

    /// <summary>
    /// Determines if deleted once reaching destination
    /// </summary>
    public bool toDelete;

    /// <summary>
    /// Determines if goes back to original position
    /// </summary>
    public bool toBack;

    /// <summary>
    /// Related to toBack, amount of time before object starts going back
    /// </summary>
    public int timeToBack;

    /// <summary>
    /// Initial position of object
    /// </summary>
    Vector3 ogPosition;

    /// <summary>
    /// The position to move to
    /// </summary>
    public Vector3 moveTo;

    /// <summary>
    /// The speed with which to move
    /// </summary>
    public float speed;

    /// <summary>
    /// Controls when the objects starts going back to position
    /// </summary>
    private bool startBack;


    private void Start()
    {
        ogPosition = transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        if (startBack == true)
        {
            condition = false;
            transform.position = Vector3.MoveTowards(transform.position, ogPosition, speed);
            if (transform.position == ogPosition)
            {
                startBack = false;
            }
        }

        if (condition)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, speed);
            if (transform.position == moveTo && toDelete == true)
            {
                Destroy(this.gameObject);
            }
            if (transform.position == moveTo && toBack == true)
            {
                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeToBack);
        startBack = true;
        condition = false;
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
