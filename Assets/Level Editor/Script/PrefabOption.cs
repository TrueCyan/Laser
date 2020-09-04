﻿#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[ExecuteInEditMode]
public class PrefabOption : MonoBehaviour
{
    public bool rotatable;
    public bool flippable;
    public bool showInEditor = true;
    public GameObject borderPrefab;
    public bool onlyOnFloor;


    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
            Destroy(this);
    }
}

#endif