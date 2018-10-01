using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// Player has been seen recently
    /// </summary>
    bool playerSeen;

    /// <summary>
    /// Original position of enemy
    /// </summary>
    Vector3 ogPosition;

    /// <summary>
    /// Original look direction of enemy
    /// </summary>
    Vector3 ogLook;

    /// <summary>
    /// Speed at which enemy moves
    /// </summary>
    public float speed;

    /// <summary>
    /// Reference to the player
    /// </summary>
    public GameObject player;

    /// <summary>
    /// Needed to check bool isSeen
    /// </summary>
    public EnemySight sight;

    /// <summary>
    /// Bool if object will perform extra action after seeing player
    /// </summary>
    public bool doExtraAction;

    /// <summary>
    /// Location of extra action
    /// </summary>
    public Vector3 extraActionLocation;

	// Use this for initialization
	void Start () {
        ogPosition = transform.position;
        ogLook = transform.forward;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (sight.isSeen) //player is seen, moving towards player
        {
            Vector3 moveTowards = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, moveTowards, speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveTowards - transform.position), 0.1F);
            playerSeen = true;
        }
        else if (playerSeen && doExtraAction && transform.position != extraActionLocation) //player has been seen recently
        {
            transform.position = Vector3.MoveTowards(transform.position, extraActionLocation, speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(extraActionLocation - transform.position), 0.1F);
        }
        else if (transform.position != ogPosition) //enemy is moving towards original position
        {
            playerSeen = false;
            transform.position = Vector3.MoveTowards(transform.position, ogPosition, speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ogPosition - transform.position), 0.2F);
        }
        else //enemy is in original position
        {
            transform.position = Vector3.MoveTowards(transform.position, ogPosition, speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ogLook), 0.2F);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("YOU HAVE DIED");
            collider.transform.position = GameManager.StaticPositionPoints.positionPoint;
        }
    }
}
