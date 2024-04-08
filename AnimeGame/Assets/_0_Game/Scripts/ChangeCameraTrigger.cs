using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public enum ECinemachineBlendType
{
    CUT=0,
    EASE_OUT
}

public class ChangeCameraTrigger : MonoBehaviour
{
    public bool useTimeStop;
    public GameObject newCamera;
    public bool isActive;
    public List<GameObject> activeObjectList;
 
    public UnityEvent triggerCallback;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (newCamera != null && PlayerPresenter.Instance != null)
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

                if (useTimeStop)
                {
                    Time.timeScale = 0.1f;
                    DOVirtual.DelayedCall(1f, () => Time.timeScale = 1);
                }
               
            }
           
        }
    }

    
}