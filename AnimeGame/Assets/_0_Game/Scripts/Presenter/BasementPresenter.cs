using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Engine.UI;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class MapUIData
{
    public GameObject[] unLockArray;
}

public class BasementPresenter : MonoBehaviour
{
    public static BasementPresenter Instance;

    [Space(20)] [Header("UI")] [Space(10)]
    public UIView mapView;
    public List<GameObject> keyboardUIList;
    public List<GameObject> joystickUIList;
    public List<Button> mapButtonList;
    public RectTransform mapSelectHighLight;
    public List<MapUIData> mapUnLockDataList;
    public GameObject destinationPanel;

    [Space(20)] [Header("Field")] [Space(10)]
    public GameObject mapCloseBtn;
    public int unLockLevel;
    public int currentMap;
    
    [Space(20)] [Header("Input")] [Space(10)]
    private Player _player;
    public ControllerType currentControllerType;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        
        Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UnLockMap();
        }
    }


    public void Init()
    {
        unLockLevel = 2;
        AddControllerChangeCallback();
        InitUnLockMap();
    }

    public void AddControllerChangeCallback()
    {
        ReInput.controllers.AddLastActiveControllerChangedDelegate(new ActiveControllerChangedDelegate(controller =>
        {
            currentControllerType = controller.type;
            switch (currentControllerType)
            {
                case ControllerType.Keyboard:
                    foreach (var keyboardUI in keyboardUIList)
                        keyboardUI.SetActive(true);
                    foreach (var joystickUI in joystickUIList)
                        joystickUI.SetActive(false);
                    break;
        
                case ControllerType.Joystick:
                    foreach (var keyboardUI in keyboardUIList)
                        keyboardUI.SetActive(false);
                    foreach (var joystickUI in joystickUIList)
                        joystickUI.SetActive(true);
                    break;
                
            }
            
            GameManager.Instance.UIForceRebuild();

        }));
        

    }



    public void OpenMap()
    {
        mapView.Show();
    }

    public void CloseMap()
    {
        mapView.Hide();
        
    }

    public void SetSelectedMapBtn()
    {
        EventSystem.current.SetSelectedGameObject(mapCloseBtn);

    }

   

   
    
    public void DisableMapButton()
    {
        foreach (var mapButton in mapButtonList)
        {
            mapButton.enabled = false;
            mapButton.interactable = false;
            mapButton.GetComponent<UIButton>().enabled = false;
        }
    }

    public void InitUnLockMap()
    {
        for (int i = 0; i < unLockLevel; i++)
        {
            mapButtonList[i].enabled = true;
            mapButtonList[i].interactable = true;
            mapButtonList[i].GetComponent<UIButton>().enabled = true;
            foreach (var VARIABLE in mapUnLockDataList[i].unLockArray)
            {
                VARIABLE.SetActive(false);
            }
        }
    }

    public void UnLockMap()
    {
        unLockLevel++;
        for (int i = 0; i < unLockLevel; i++)
        {
            mapButtonList[i].enabled = true;
            mapButtonList[i].interactable = true;
            mapButtonList[i].GetComponent<UIButton>().enabled = true;
            foreach (var VARIABLE in mapUnLockDataList[i].unLockArray)
            {
                VARIABLE.SetActive(false);
            }
        }
        destinationPanel.SetActive(false);
        destinationPanel.SetActive(true);
    }

    public void HoverMapHighlight(int idx)
    {
        mapSelectHighLight.transform.parent = mapButtonList[idx].transform;
        mapSelectHighLight.anchoredPosition = new Vector2(0, 0);
        mapSelectHighLight.SetAsFirstSibling();
    }

    public void SelectMap(int idx)
    {
        DisableMapButton();
        DOVirtual.DelayedCall(0.74f, () => { mapView.Hide(); });
        currentMap = idx;
    }
    
}
