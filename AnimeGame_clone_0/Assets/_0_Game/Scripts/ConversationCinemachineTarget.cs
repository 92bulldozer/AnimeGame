using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationCinemachineTarget : MonoBehaviour
{
    public GameObject originCamera;
    public GameObject playerCamera;
    public GameObject targetCamera;
    
    

    public void FocusPlayer()
    {
        originCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
        targetCamera.gameObject.SetActive(false);
    }

    public void FocusTarget()
    {
        originCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(false);
        targetCamera.gameObject.SetActive(true);
    }

    public void FocusOrigin()
    {
        originCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        targetCamera.gameObject.SetActive(false);
    }

   

}
