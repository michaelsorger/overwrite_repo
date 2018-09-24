using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    /// <summary>
    /// The scriptable object representing save position points
    /// </summary>
    [SerializeField]
    private PositionPoints posPoints;

    /// <summary>
    /// Ghost object used to show the player's saved position
    /// </summary>
    public GameObject saveGhost;

	// Use this for initialization
	void Start () {
        saveGhost = Instantiate(saveGhost, transform.position, transform.rotation);
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("SavePosition"))
        {
            posPoints.positionPoint = this.gameObject.transform.position;
            posPoints.rotationPoint = this.gameObject.transform.rotation;
            saveGhost.transform.position = gameObject.transform.position;
            saveGhost.transform.rotation = gameObject.transform.rotation;
            
        }
        else if(Input.GetButton("Teleport"))
        {
            if(posPoints.positionPoint != null)
            {
                this.gameObject.transform.position = posPoints.positionPoint;
                this.gameObject.transform.rotation = posPoints.rotationPoint;
            }
        }
	}
}
