﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    /// <summary>
    /// speed of the character
    /// </summary>
    public float speed;

    //Update is called once per frame
    //Updates location once per frame according to input
    void Update()
    {
        //position of the character
        Vector3 pos = transform.position;

        if (Input.GetKey("a"))
        {
            pos.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += speed * Time.deltaTime;
        }

        if (Input.GetKey("w"))
        {
            pos.z += speed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= speed * Time.deltaTime;
        }

        transform.position = pos;
    }

}