using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUpdater : MonoBehaviour
{
    Camera mainCamera;    

    void Awake()
    {
        mainCamera = Camera.main;        
    }

    void Update()
    {
        RaycastHit hit;
        var headPosition = mainCamera.transform.position;        
        var downDirection = new Vector3(0, -1, 0);

        if (Physics.Raycast(headPosition, downDirection, out hit))
        {
            var difference = headPosition - hit.point;
            var distanceInY = Mathf.Abs(difference.y);            

            gameObject.transform.localPosition = new Vector3(0, -0.5f * distanceInY, 0);
            gameObject.transform.localScale = new Vector3(1, distanceInY, 1);                                  
        }               
    }
}
