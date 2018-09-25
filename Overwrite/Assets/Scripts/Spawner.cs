using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Constant spawner for objects
/// </summary>
public class Spawner : MonoBehaviour {

    /// <summary>
    /// The prefab to spawn
    /// </summary>
    public GameObject prefab;

    /// <summary>
    /// The position to spawn
    /// </summary>
    public Vector3 startPos;

    /// <summary>
    /// The rotation to spawn
    /// </summary>
    public Quaternion startRot;

    /// <summary>
    /// The rate at spawning
    /// </summary>
    public float spawnTime;

	void Start ()
    {
        InvokeRepeating("SpawnObject", 0, spawnTime);
	}
	
    /// <summary>
    /// Spawns the prefab
    /// </summary>
    private void SpawnObject()
    {
        if(prefab != null)
        {
            GameObject obj = Instantiate(prefab, startPos, startRot);
        }
        else
        {
            Debug.Log("Nothing to spawn");
            Debug.Log("CONGRATULATIONS!! YOU BEAT THIS LEVEL!!");
        }
    }
}
