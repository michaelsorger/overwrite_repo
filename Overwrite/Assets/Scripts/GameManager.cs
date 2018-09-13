using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    /// <summary>
    /// The list of Camera Focuses
    /// </summary>
    public List<Camera> CameraFocuses;

    /// <summary>
    /// The small inventory
    /// </summary>
    [SerializeField]
    private SmallInventory theSmallInventory;

    /// <summary>
    /// The small inventory
    /// </summary>
    [SerializeField]
    private PositionPoints positionPoints;

    /// <summary>
    /// The Camera Focus Index
    /// </summary>
    private int CameraFocusIndex;

	void Start ()
    {
        //Default to 2D person camera focus
        SetupCameras();

        //Clear theSmallInventory (for now)
        theSmallInventory.keyList.Clear();

        //Zero out the player position saves (always)
        positionPoints.positionPoint = Vector3.zero;
	}
	
	void Update ()
    {
        if(Input.GetButtonDown("ToggleCamera"))
        {
            UpdateCameraFocus();
        }	
	}

    /// <summary>
    /// Sets the default focus on camera's in scene
    /// </summary>
    private void SetupCameras()
    {
        if(CameraFocuses != null)
        {
            CameraFocusIndex = 0;
            CameraFocuses[CameraFocusIndex].enabled = true;
            CameraFocuses[CameraFocusIndex + 1].enabled = false;
        }
    }

    /// <summary>
    /// Updates the Camera Focus
    /// </summary>
    private void UpdateCameraFocus()
    {
        if(CameraFocuses != null)
        {
            CameraFocusIndex += 1;
            if (CameraFocusIndex > CameraFocuses.Count - 1)
            {
                CameraFocusIndex = 0;
            }
            for (int i = 0; i < CameraFocuses.Count; i++)
            {
                if (i == CameraFocusIndex)
                {
                    CameraFocuses[i].enabled = true;
                }
                else
                {
                    CameraFocuses[i].enabled = false;
                }
            }
        }
    }
}

/*
Instantiate only needed objects depending on inventory
foreach(GameObject obj in gameObjectsToSpawn)
{

if(theSmallInventory.keyList.Contains(obj.name))
{
//Just don't spawn any items in theSmallInventory
obj.SetActive(false);
}
}
*/
