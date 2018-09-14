using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPlatform : MonoBehaviour {

    /// <summary>
    /// The player
    /// </summary>
    public GameObject player;

    //When the player is on top of platform, set player as child of platform
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            player.transform.SetParent(transform);
        }
    }

    //When the player exits platform, set player as child of nothing
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player)
        {
            player.transform.SetParent(null);
        }
    }


}
