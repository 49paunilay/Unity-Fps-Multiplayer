﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateSky : MonoBehaviour
{
    public float RotateSpeed=4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation",Time.time*RotateSpeed);
        
    }
}
