using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

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

            SavePosition();

        }
        else if(Input.GetButton("Teleport"))
        {

            LoadPosition();

        }
        else if(Input.GetButtonDown("SaveWorld"))
        {
            SaveWorld();
        }
        else if(Input.GetButton("LoadWorld"))
        {
            LoadWorldHelper();
        }
	}

    public void SavePosition()
    {
        GameManager.StaticPositionPoints.positionPoint = this.gameObject.transform.position;
        GameManager.StaticPositionPoints.rotationPoint = this.gameObject.transform.rotation;
        saveGhost.transform.position = gameObject.transform.position;
        saveGhost.transform.rotation = gameObject.transform.rotation;
    }

    public void LoadPosition()
    {
        if(GameManager.StaticPositionPoints.positionPoint != null)
        {
            this.gameObject.transform.position = GameManager.StaticPositionPoints.positionPoint;
            this.gameObject.transform.rotation = GameManager.StaticPositionPoints.rotationPoint;
        }
    }


    public void SaveWorld()
    {
        GameManager.theTempLevel = LevelCreatorManager.SerializeLevelIntoScriptObj(GameManager.theTempLevel);
    }

    public void LoadWorldHelper()
    {
        if (!inCorotine)
        {
            Debug.Log("PlayerController LoadWorld, StartCoroutine");
            StartCoroutine(LoadWorld());
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
        DestroyObjectsWithTag("Switcher");
        DestroyObjectsWithTag("Lever_0");
        DestroyObjectsWithTag("Lever_1");
        DestroyObjectsWithTag("Lever_2");
        DestroyObjectsWithTag("Lever_3");
        DestroyObjectsWithTag("Spikes_0");
        StopEnemyMovement();
        yield return new WaitForSeconds(1f);
        GameManager.DeserializeFromScriptObj(GameManager.theTempLevel);
        yield return new WaitForSeconds(.5f);
        StartEnemyMovement();
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

    void StopEnemyMovement()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject go in gos)
        {
            go.GetComponent<EnemyAI>().speed = 0f;
        }
    }

    void StartEnemyMovement()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in gos)
        {
            go.GetComponent<EnemyAI>().speed = 3.5f;
        }
    }
}
