using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PositionPoints", menuName = "PositionPoints", order = 1)]
public class PositionPoints : ScriptableObject
{
    /// <summary>
    /// The save point (add more to save point here i.e multiple saves might have a list of points)
    /// </summary>
    public Vector3 positionPoint;
    public Quaternion rotationPoint;
}
