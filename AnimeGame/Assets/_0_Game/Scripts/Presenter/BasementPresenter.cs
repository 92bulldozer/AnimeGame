using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasementPresenter : MonoBehaviour
{
    public static BasementPresenter Instance;

    [Space(20)] [Header("UI")] [Space(10)]
    public UIView mapView;
    public List<GameObject> keyboardUIList;
    public List<GameObject> joystickUIList;

    [Space(20)] [Header("Field")] [Space(10)]
    public GameObject mapCloseBtn;
    public int test;
    
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
    


    public void Init()
    {
        AddControllerChangeCallback();
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
}
