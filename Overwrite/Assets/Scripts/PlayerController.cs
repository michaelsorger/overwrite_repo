using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    /// <summary>
    /// The scriptable object representing save position points
    /// </summary>
    [SerializeField]
    private PositionPoints posPoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("SavePosition"))
        {
            posPoints.positionPoint = this.gameObject.transform.position;
            Debug.Log("Position Overwrited at " + posPoints.positionPoint);
        }
        else if(Input.GetButton("Teleport"))
        {
            if(posPoints.positionPoint != null)
            {
                this.gameObject.transform.position = posPoints.positionPoint;
            }
        }
	}
}
