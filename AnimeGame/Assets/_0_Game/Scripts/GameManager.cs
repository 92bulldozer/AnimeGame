using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Camera")][Space(20)]
    public List<GameObject> cameraList;
 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            SetCameraTarget(0);
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
            SetCameraTarget(1);
        
        if(Input.GetKeyDown(KeyCode.Alpha3))
            SetCameraTarget(2);
        
        if(Input.GetKeyDown(KeyCode.Alpha4))
            SetCameraTarget(3);
        if(Input.GetKeyDown(KeyCode.Alpha5))
            SetCameraTarget(4);
    }
    
    
    public void SetCameraTarget(int cameraIdx)
    {
        for (int i = 0; i < cameraList.Count; i++)
        {
            if (i == cameraIdx)
                cameraList[i].SetActive(true);
            
            else
                cameraList[i].SetActive(false);

        }
    }
}
