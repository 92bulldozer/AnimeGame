using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using DG.Tweening;
using Doozy.Engine.UI;
using EJ;
using EPOOutline;
using Michsky.UI.Dark;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class MapUIData
{
    public GameObject[] unLockArray;
}

public enum EEquipmentTab
{
    Equipment=0,
    Consume,
    Material
}

public enum EBasementCanvas
{
    None=0,
    Map,
    Equipment,
    Inventory,
    Exit
}

public class BasementPresenter : MonoBehaviour
{
    public static BasementPresenter Instance;

    [Space(20)] [Header("UI")] [Space(10)]
    public UIView mapView;
    public UIView equipmentView;
    public UIView inventoryView;
    public UIView exitView;
    public List<GameObject> keyboardUIList;
    public List<GameObject> joystickUIList;
    [Space(20)] [Header("UI_Map")] [Space(10)]
    public List<Button> mapButtonList;
    public List<MapUIData> mapUnLockDataList;
    public GameObject destinationPanel;
    public RectTransform mapSelectHighLight;

    [Space(20)] [Header("UI_Equipment")] [Space(10)]
    public Color normalColor;
    public Color selectColor;
    public int currentEquipmentTabIdx;
    public List<Button> equipmentTabButtonList;
    public List<CanvasGroup> equipmentCGList;
    private List<Sequence> equipmentScrollSequenceList;

    [Space(20)] [Header("UI_Inventory")] [Space(10)]
    [Space(20)] [Header("UI_Exit")] [Space(10)]
    public UIDissolveEffect exitDissolve;
    public Sequence exitDissolveSequence;

    [Space(20)] [Header("Field")] [Space(10)]
    public Camera mainCamera;
    public GameObject mapCloseBtn;
    public int unLockLevel;
    public int currentMap;
    public GameObject vcamEquipment;
    public GameObject vcam1;
    public GameObject vcam2;
    public GameObject vcamInventory;
    public Outliner cameraOutliner;
    public EBasementCanvas eBasementCanvas;
   
    
    
    
    [Space(20)] [Header("Input")] [Space(10)]
    private Player _player;
    private int cameraLayer;
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

        
        
        ProcessCanvasInput();        
    }


    public void Init()
    {
        Cursor.visible = false;
        mainCamera = Camera.main;
        _player = ReInput.players.GetPlayer(0);
        unLockLevel = 2;
        AddControllerChangeCallback();
        InitUnLockMap();
        equipmentScrollSequenceList = new List<Sequence>();
        
        equipmentScrollSequenceList.Add(
            DOTween.Sequence().Append(equipmentCGList[0].DOFade(1,1) 
                    .SetEase(Ease.OutQuad)).OnStart(()=>equipmentCGList[0].alpha=0).SetAutoKill(false)
            );
        
        equipmentScrollSequenceList.Add(
            DOTween.Sequence().Append(equipmentCGList[1].DOFade(1,1) 
                .SetEase(Ease.OutQuad)).OnStart(()=>equipmentCGList[1].alpha=0).SetAutoKill(false));
        
        equipmentScrollSequenceList.Add(
            DOTween.Sequence().Append(equipmentCGList[2].DOFade(1,1) 
                .SetEase(Ease.OutQuad)).OnStart(()=>equipmentCGList[2].alpha=0).SetAutoKill(false));

        exitDissolveSequence = DOTween.Sequence().SetAutoKill(false).Append(
            DOTween.To(() => exitDissolve.location, x => exitDissolve.location = x, 0, 1)
                .OnStart(()=>exitDissolve.location=1).SetEase(Ease.OutQuad)
        );

    }

    public void AddControllerChangeCallback()
    {
        ReInput.controllers.AddLastActiveControllerChangedDelegate(controller =>
        {
            currentControllerType = controller.type;
            switch (currentControllerType)
            {
                case ControllerType.Keyboard:
                    foreach (var keyboardUI in keyboardUIList)
                        keyboardUI.SetActive(true);
                    foreach (var joystickUI in joystickUIList)
                        joystickUI.SetActive(false);
                    EscMenuPresenter.Instance.ShowCursor();
                    break;
        
                case ControllerType.Joystick:
                    foreach (var keyboardUI in keyboardUIList)
                        keyboardUI.SetActive(false);
                    foreach (var joystickUI in joystickUIList)
                        joystickUI.SetActive(true);
                    EscMenuPresenter.Instance.HideCursor();
                    break;
                
            }
            
            GameManager.Instance.UIForceRebuild();

        });
        

    }



    public void ProcessCanvasInput()
    {
        switch (eBasementCanvas)
        {
            case EBasementCanvas.None:
                break;
            case EBasementCanvas.Map:
                MapInput();
                break;
            case EBasementCanvas.Equipment:
                EquipmentInput();
                break;
         
            case EBasementCanvas.Exit:
                ExitInput();
                break;
            
        }
    }

    public void MapInput()
    {
      
        if(_player.GetNegativeButtonDown("UIHorizontal"))
        {
            if (_player.GetAxis("UIHorizontal") < 0)
            {
                "Basement UI Left".Log();
            }
        }
        else if (_player.GetButtonDown("UIHorizontal"))
        {
            if (_player.GetAxis("UIHorizontal") > 0)
            {
                "Basement UI Right".Log();
            }
          
        }

        if (_player.GetNegativeButtonDown("UIVertical"))
        {
            if (_player.GetAxis("UIVertical") < 0)
            {
                "Basement UI Down".Log();
            }
        }
        else if (_player.GetButtonDown("UIVertical"))
        {
            if (_player.GetAxis("UIVertical") > 0)
            {
                "Basement UI Up".Log();
            }
           
        }
        
        if (_player.GetButtonDown("Prev"))
        {
            "Basement UI Prev".Log();
            PreviousEquipmentTab();
        }
        
        if (_player.GetButtonDown("Next"))
        {
            "Basement UI Next".Log();
            NextEquipmentTab();
        }
        
        if (_player.GetButtonDown("UISubmit"))
        {
            "Basement UI Submit".Log();
        }
        if (_player.GetButtonDown("UICancel"))
        {
            "Basement UI Cancel".Log();
        }
    }
    public void EquipmentInput()
    {
       
        
        if(_player.GetNegativeButtonDown("UIHorizontal"))
        {
            if (_player.GetAxis("UIHorizontal") < 0)
            {
                "Basement UI Left".Log();
            }
        }
        else if (_player.GetButtonDown("UIHorizontal"))
        {
            if (_player.GetAxis("UIHorizontal") > 0)
            {
                "Basement UI Right".Log();
            }
          
        }

        if (_player.GetNegativeButtonDown("UIVertical"))
        {
            if (_player.GetAxis("UIVertical") < 0)
            {
                "Basement UI Down".Log();
            }
        }
        else if (_player.GetButtonDown("UIVertical"))
        {
            if (_player.GetAxis("UIVertical") > 0)
            {
                "Basement UI Up".Log();
            }
           
        }
        
        if (_player.GetButtonDown("Prev"))
        {
            "Basement UI Prev".Log();
            PreviousEquipmentTab();
        }
        
        if (_player.GetButtonDown("Next"))
        {
            "Basement UI Next".Log();
            NextEquipmentTab();
        }
        
        if (_player.GetButtonDown("UISubmit"))
        {
            "Basement UI Submit".Log();
        }
        if (_player.GetButtonDown("UICancel"))
        {
            "Basement UI Cancel".Log();
        }
    }
    
    public void ExitInput()
    {
       
        if(_player.GetNegativeButtonDown("UIHorizontal"))
        {
            if (_player.GetAxis("UIHorizontal") < 0)
            {
                "Basement UI Left".Log();
            }
        }
        else if (_player.GetButtonDown("UIHorizontal"))
        {
            if (_player.GetAxis("UIHorizontal") > 0)
            {
                "Basement UI Right".Log();
            }
          
        }

        if (_player.GetNegativeButtonDown("UIVertical"))
        {
            if (_player.GetAxis("UIVertical") < 0)
            {
                "Basement UI Down".Log();
            }
        }
        else if (_player.GetButtonDown("UIVertical"))
        {
            if (_player.GetAxis("UIVertical") > 0)
            {
                "Basement UI Up".Log();
            }
           
        }
        
        if (_player.GetButtonDown("Prev"))
        {
            "Basement UI Prev".Log();
            PreviousEquipmentTab();
        }
        
        if (_player.GetButtonDown("Next"))
        {
            "Basement UI Next".Log();
            NextEquipmentTab();
        }
        
        if (_player.GetButtonDown("UISubmit"))
        {
            "Basement UI Submit".Log();
        }
        if (_player.GetButtonDown("UICancel"))
        {
            "Basement UI Cancel".Log();
        }
    }

   



    #region Map
    
    public void ShowMap()
    {
        eBasementCanvas = EBasementCanvas.Map;
        mapView.Show();
    }

    public void HideMap()
    {
        eBasementCanvas = EBasementCanvas.None;
        mapView.Hide();
        
    }
    
    public void DisableMapButton()
    {
        foreach (var mapButton in mapButtonList)
        {
            mapButton.interactable = false;
            //mapButton.GetComponent<UIButton>().enabled = false;
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
    
    public void SetSelectedMapBtn()
    {
        EventSystem.current.SetSelectedGameObject(mapCloseBtn);

    }

    public void SelectMap(int idx)
    {
        DisableMapButton();
        DOVirtual.DelayedCall(0.74f, () => { mapView.Hide(); });
        currentMap = idx;
    }
    
    
    #endregion



    #region Equipment

    public void ShowEquipment()
    {
       
        DOVirtual.DelayedCall(0.5f,()=> eBasementCanvas = EBasementCanvas.Equipment);
        
        equipmentView.Show();
        EventSystem.current.SetSelectedGameObject(equipmentTabButtonList[0].gameObject);
        vcam2.SetActive(false);
        vcamEquipment.SetActive(true);
        currentEquipmentTabIdx = 0;
        SelectEquipmentTab(currentEquipmentTabIdx);
    }
    
    public void HideEquipment()
    {
        eBasementCanvas = EBasementCanvas.None;
        equipmentView.Hide();
        vcam2.SetActive(true);
        vcamEquipment.SetActive(false);
    }

    public void SelectEquipmentTab(int idx)
    {
        foreach (var button in equipmentTabButtonList)
            button.image.color = normalColor;
        
        for (int i = 0; i < equipmentTabButtonList.Count; i++)
            if (i == idx)
            {
                equipmentTabButtonList[idx].image.color = selectColor;
                currentEquipmentTabIdx = idx;
                SetEquipmentScrollViewPanel((EEquipmentTab)idx);
                break;
            }
    }

    public void SetEquipmentScrollViewPanel(EEquipmentTab tab)
    {
        foreach (var cg in equipmentCGList)
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }

        foreach (var sequence in equipmentScrollSequenceList)
        {
            sequence.Rewind();
        }

        equipmentScrollSequenceList[(int)tab].Restart();
        equipmentCGList[(int)tab].interactable = true;
        equipmentCGList[(int)tab].blocksRaycasts = true;
    }

    public void PreviousEquipmentTab()
    {
        currentEquipmentTabIdx--;
        if (currentEquipmentTabIdx < 0)
        {
            currentEquipmentTabIdx = 0;
            return;
        }
        SelectEquipmentTab(currentEquipmentTabIdx);
        MasterAudio.PlaySound("Click");
    }
    
    public void NextEquipmentTab()
    {
        currentEquipmentTabIdx++;
        if (currentEquipmentTabIdx > 2)
        {
            currentEquipmentTabIdx = 2;
            return;
        }
        SelectEquipmentTab(currentEquipmentTabIdx);
        MasterAudio.PlaySound("Click");
    }

    #endregion


    #region Inventory

    public void ShowInventory()
    {

        eBasementCanvas = EBasementCanvas.Inventory;
        
        inventoryView.Show();
        vcam2.SetActive(false);
        vcamInventory.SetActive(true);
      
    }
    
    public void HideInventory()
    {
        eBasementCanvas = EBasementCanvas.None;
        inventoryView.Hide();
        vcam2.SetActive(true);
        vcamInventory.SetActive(false);
    }
    

    #endregion


    #region Exit
    public void ShowExit()
    {
        eBasementCanvas = EBasementCanvas.Exit;
        exitView.Show();
        exitDissolveSequence.Restart();
    }

    public void HideExit()
    {
        eBasementCanvas = EBasementCanvas.None;
        "Exit".Log();
        exitView.Hide();
        MasterAudio.PlaySound("Click");
    }
    

    #endregion

   
    
}
