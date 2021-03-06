﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object containing all of a level's information needed to instantiate an instance of it later
/// Make a copy of it later when the tagList is defined, other fields can be left blank
/// </summary>
[CreateAssetMenu(fileName = "Level_X", menuName = "LevelInfo", order = 1)]
public class LevelInformation : ScriptableObject
{
    public List<string> tagList;
    public string tagGameObjectListJSON;
    public string switcherControlJSON;
    public Vector3 playerStartPosition;
}

[System.Serializable]
public struct PositionRotationScale
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scaler;
}