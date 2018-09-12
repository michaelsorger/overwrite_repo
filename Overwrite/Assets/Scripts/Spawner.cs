using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject prefab;
    public Vector3 startPos;
    public float spawnTime;

	void Start ()
    {
        InvokeRepeating("SpawnObject", 0, spawnTime);
	}
	
    private void SpawnObject()
    {
        GameObject obj = Instantiate(prefab, startPos, transform.rotation);
    }
}
