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
    public Vector3 ogPosition;

    /// <summary>
    /// Original look direction of enemy
    /// </summary>
    public Vector3 ogLook;

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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (sight.isSeen) //player is seen, moving towards player
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                new Vector3(transform.position.x, transform.position.y, player.transform.position.z), speed);
            playerSeen = true;
        }
        else if (playerSeen) //player has been seen recently
        {
            //doExtraAction (such as set off alarm)
            playerSeen = false;
        }
        else if (transform.position != ogPosition) //enemy is moving towards original position
        {
            transform.position = Vector3.MoveTowards(transform.position, ogPosition, speed);
            transform.rotation = Quaternion.LookRotation(new Vector3(ogPosition.x, 0, ogPosition.z));
        }
        else //enemy is in original position
        {
            transform.position = Vector3.MoveTowards(transform.position, ogPosition, speed);
            transform.rotation = Quaternion.LookRotation(ogLook);
        }
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Debug.Log("YOU HAVE DIED");
            Destroy(collider.gameObject);
        }
    }
}
