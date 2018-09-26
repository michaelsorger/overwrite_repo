﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    /// <summary>
    /// I suck and am running out of time
    /// References to all the prefabs needed to reinstantiate an entire level
    /// </summary>
    public GameObject theWallPrefab;
    public GameObject theDoorPrefab;
    public GameObject thePlayerPrefab;
    public GameObject theLeverPrefab;

    /// <summary>
    /// The scriptable object representing save position points
    /// </summary>
    [SerializeField]
    private PositionPoints posPoints;

    /// <summary>
    /// Ghost object used to show the player's saved position
    /// </summary>
    public GameObject saveGhost;

    public bool inCorotine = false;

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
        else if(Input.GetButtonDown("SaveWorld"))
        {
            GameManager.theTempLevel = LevelCreatorManager.SerializeLevelIntoScriptObj(GameManager.theTempLevel);
        }
        else if(Input.GetButton("LoadWorld"))
        {
            if(!inCorotine)
            {
                Debug.Log("PlayerController LoadWorld, StartCoroutine");
                StartCoroutine(LoadWorld());
            }

        }
	}

    /// <summary>
    /// Remove all removable objects from world before reinstantiation
    /// add more destroyobjectswithtags as needed
    /// Have to wait for destroy to finish before reinstantiation or else there is problems
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadWorld()
    {
        inCorotine = true;
        DestroyObjectsWithTag("Wall");
        DestroyObjectsWithTag("Lever");
        DestroyObjectsWithTag("Lever_0");
        DestroyObjectsWithTag("Lever_1");
        DestroyObjectsWithTag("Lever_2");
        DestroyObjectsWithTag("Lever_3");
        yield return new WaitForSeconds(1);
        GameManager.DeserializeFromScriptObj(GameManager.theTempLevel, theWallPrefab, theDoorPrefab, thePlayerPrefab, theLeverPrefab, posPoints);
        inCorotine = false;
        Debug.Log("World loaded");
    }

    /// <summary>
    /// Destroys all gameobjects with the given tag
    /// </summary>
    /// <param name="goTag"></param>
    void DestroyObjectsWithTag(string goTag)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(goTag);
        foreach(GameObject g in gos)
        {
            Destroy(g);
        }
    }
}
