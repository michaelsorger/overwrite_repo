using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour {

    /// <summary>
    /// The gameObject to directly follow
    /// </summary>
    public GameObject gameObjectToFollow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObjectToFollow.transform.position.z);
	}
}
