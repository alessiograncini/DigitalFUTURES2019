﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Billboards : MonoBehaviour
{

    public Camera cam;

    private void Awake()
    {
        cam= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.right);
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }
}



//private Transform camera;


//// Use this for initialization
//void Start()
//{

//    camera = Camera.main.transform;

//}

//// Update is called once per frame
//void Update()
//{
//    // Rotate the camera every frame so it keeps looking at the target
//    transform.LookAt(camera);
//}
 
 //}