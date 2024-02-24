using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraTrigger : MonoBehaviour
{
    public GameObject prevCamera;
    public GameObject newCamera;
    public bool isReverse;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isReverse)
            {
                isReverse = true;
                prevCamera.SetActive(false);
                newCamera.SetActive(true);
            }
            else
            {
                isReverse = false;
                prevCamera.SetActive(true);
                newCamera.SetActive(false);
            }
            
        }
    }
}
