using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using UnityEngine;
using UnityEngine.Events;

public class ChangeCameraTrigger : MonoBehaviour
{
    public GameObject newCamera;
    public bool isActive;
    public List<GameObject> activeObjectList;
    public UnityEvent triggerCallback;
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (newCamera != null)
            {
                PlayerPresenter.Instance.ChangeVirtualCamera(newCamera);
                triggerCallback?.Invoke();

                foreach (var activeObject in activeObjectList)
                {
                    if(isActive)
                        activeObject.SetActive(true);
                    else
                        activeObject.SetActive(false);
                }
                
            }
           
        }
    }
}