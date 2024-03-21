using System;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using DarkTonic.MasterAudio;
using DG.Tweening;
using Doozy.Engine.UI;
using EJ;
using EPOOutline;
using MarkupAttributes;
using Michsky.UI.Dark;
using Rewired;
using TMPro;
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
    Equipment = 0,
    Consume,
    Material
}

public enum EStashTab
{
    Equipment = 0,
    Consume,
    Material
}

public enum EBasementCanvas
{
    None = 0,
    Map,
    Equipment,
    Stash,
    Exit
}

[Serializable]
public class StashSlot
{
    public ItemData itemData;
    public bool isEmpty;
}


public class BasementPresenter : AnimeBehaviour
{
    public static BasementPresenter Instance;

    
    
    [TabScope("Tab Scope", "UI|UI_Map|UI_Equipment|UI_Stash|RewiredInput", box: true,20)]
  
    [Space(10)] //[Foldout("UI",true,10)]
    [Tab("./UI")]
    public UIView mapView;

    public UIView equipmentView;
    public UIView stashView;
    public UIView exitView;
    public List<GameObject> keyboardUIList;
    public List<GameObject> joystickUIList;

    //[Foldout("UI_Map",true,10)]
    [Tab("../UI_Map")]
     public int maxMap;
     public List<Button> mapButtonList;
     public List<ParticleImage> mapSelectEffectList;
     public List<Animator> mapBtnAnimatorList;
     public List<MapUIData> mapUnLockDataList;
     public GameObject destinationPanel;
     public RectTransform mapSelectHighLight;


   
    //[Foldout("UI_Equipment",true,10)]
    [Tab("../UI_Equipment")]
    public Color normalColor;
    public Color selectColor;
    public int currentEquipmentTabIdx;
    public List<Button> equipmentTabButtonList;
    public List<CanvasGroup> equipmentCGList;
    private List<Sequence> equipmentScrollSequenceList;

    
    //[Foldout("UI_Stash",true,10)]
    [Tab("../UI_Stash")]
    public int currentStashTabIdx;
    public List<Button> stashTabButtonList;
    public List<CanvasGroup> stashCGList;
    private List<Sequence> stashScrollSequenceList;
    public List<StashSlot> equipmentStashSlotDataList;
    public List<StashSlot> consumeStashSlotDataList;
    public List<StashSlot> materialStashSlotDataList;
    public List<StashSlotUI> equipmentStashSlotUIList;
    public List<StashSlotUI> consumeStashSlotUIList;
    public List<StashSlotUI> materialStashSlotUIList;
    public Image detailIcon;
    public TextMeshProUGUI detailTitleText;
    public TextMeshProUGUI detailDescriptionText;
    public UIDissolveEffect exitDissolve;
    public Sequence exitDissolveSequence;

    //[Foldout("RewiredInput",true,10)]
    [Tab("../RewiredInput")]
    public ControllerType currentControllerType;
    private Player _player;
    private int cameraLayer;
    
  
    //[Foldout("Field",true,10)]
    [Box("Field",true,10)]
    public Camera mainCamera;
    public int unLockLevel;
    public int currentMap;
    public GameObject vcamEquipment;
    public GameObject vcam1;
    public GameObject vcam2;
    public GameObject vcamStash;
    public Outliner cameraOutliner;
    public EBasementCanvas eBasementCanvas;

   
   

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        Init();
        Debug.Log("WWW");
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
        stashScrollSequenceList = new List<Sequence>();
        equipmentStashSlotDataList = new List<StashSlot>(36);
        consumeStashSlotDataList = new List<StashSlot>(36);
        materialStashSlotDataList = new List<StashSlot>(36);


        stashScrollSequenceList.Add(
            DOTween.Sequence().Append(stashCGList[0].DOFade(1, 1)
                .SetEase(Ease.OutQuad)).OnStart(() => stashCGList[0].alpha = 0).SetAutoKill(false)
        );

        stashScrollSequenceList.Add(
            DOTween.Sequence().Append(stashCGList[1].DOFade(1, 1)
                .SetEase(Ease.OutQuad)).OnStart(() => stashCGList[1].alpha = 0).SetAutoKill(false));

        stashScrollSequenceList.Add(
            DOTween.Sequence().Append(stashCGList[2].DOFade(1, 1)
                .SetEase(Ease.OutQuad)).OnStart(() => stashCGList[2].alpha = 0).SetAutoKill(false));


        exitDissolveSequence = DOTween.Sequence().Append(
            DOTween.To(() => exitDissolve.location, x => exitDissolve.location = x, 0, 1)
                .OnStart(() => exitDissolve.location = 1).SetEase(Ease.OutQuad).SetAutoKill(false)
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
            case EBasementCanvas.Stash:
                StashInput();
                break;

            case EBasementCanvas.Exit:
                ExitInput();
                break;
        }
    }

    public void MapInput()
    {
        if (_player.GetNegativeButtonDown("UIHorizontal"))
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
            PrevSelectMap();
        }

        if (_player.GetButtonDown("Next"))
        {
            "Basement UI Next".Log();
            NextSelectMap();
        }

        if (_player.GetButtonDown("UISubmit"))
        {
            "Basement UI Submit".Log();
            SelectMap(currentMap);
        }

        if (_player.GetButtonDown("UICancel"))
        {
            "Basement UI Cancel".Log();
        }
    }

    public void EquipmentInput()
    {
        if (_player.GetNegativeButtonDown("UIHorizontal"))
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

    public void StashInput()
    {
        if (_player.GetNegativeButtonDown("UIHorizontal"))
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
            PreviousStashTab();
        }

        if (_player.GetButtonDown("Next"))
        {
            "Basement UI Next".Log();
            NextStashTab();
        }

        if (_player.GetButtonDown("UISubmit"))
        {
            //"Basement UI Submit".Log();
        }

        if (_player.GetButtonDown("UICancel"))
        {
            "Basement UI Cancel".Log();
        }
    }

    public void ExitInput()
    {
        if (_player.GetNegativeButtonDown("UIHorizontal"))
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
        DOVirtual.DelayedCall(0.5f, () => eBasementCanvas = EBasementCanvas.Map);
        mapView.Show();

        DOVirtual.DelayedCall(0.5f, () =>
        {
            foreach (var animator in mapBtnAnimatorList)
            {
                animator.SetFloat("Speed", 1);
            }
        });
    }

    public void HideMap()
    {
        eBasementCanvas = EBasementCanvas.None;
        mapView.Hide();
        DOVirtual.DelayedCall(0.1f, () =>
        {
            foreach (var animator in mapBtnAnimatorList)
            {
                animator.SetFloat("Speed", 10);
            }
        });
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
        if (unLockLevel >= maxMap)
            return;

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
        currentMap = idx;
        EventSystem.current.SetSelectedGameObject(mapButtonList[idx].gameObject);

        mapSelectHighLight.transform.parent = mapButtonList[idx].transform;
        mapSelectHighLight.anchoredPosition = new Vector2(0, 0);
        mapSelectHighLight.SetAsFirstSibling();
    }

    public void MapUnHover()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }


    public void PrevSelectMap()
    {
        currentMap--;
        if (currentMap < 0)
        {
            currentMap = 0;
            return;
        }

        EventSystem.current.SetSelectedGameObject(mapButtonList[currentMap].gameObject);
        HoverMapHighlight(currentMap);
        MasterAudio.PlaySound("Hover");
    }

    public void NextSelectMap()
    {
        if (currentMap >= unLockLevel - 1)
            return;
        $"{currentMap} {unLockLevel - 1}".Log();

        currentMap++;
        if (currentMap > 3)
        {
            currentMap = 3;
            return;
        }

        EventSystem.current.SetSelectedGameObject(mapButtonList[currentMap].gameObject);
        HoverMapHighlight(currentMap);
        MasterAudio.PlaySound("Hover");
    }

    public void SetSelectedMapBtn()
    {
        EventSystem.current.SetSelectedGameObject(mapButtonList[currentMap].gameObject);
        HoverMapHighlight(currentMap);
    }

    public void SelectMap(int idx)
    {
        currentMap = idx;
        EventSystem.current.SetSelectedGameObject(mapButtonList[currentMap].gameObject);
        eBasementCanvas = EBasementCanvas.None;
        DisableMapButton();
        mapSelectEffectList[currentMap].Play();
        DOVirtual.DelayedCall(0.74f, () => { mapView.Hide(); });
        MasterAudio.PlaySound("Select");
        MasterAudio.PlaySound("Click");
        foreach (var animator in mapBtnAnimatorList)
            animator.SetFloat("Speed", 10);
    }

    #endregion


    #region Equipment

    public void ShowEquipment()
    {
        DOVirtual.DelayedCall(0.5f, () => eBasementCanvas = EBasementCanvas.Equipment);

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


    #region Stash

    public void ShowStash()
    {
        DOVirtual.DelayedCall(0.5f, () => eBasementCanvas = EBasementCanvas.Stash);

        stashView.Show();
        vcam2.SetActive(false);
        vcamStash.SetActive(true);
        currentStashTabIdx = 0;
        SelectStashTab(currentStashTabIdx);
        HoverEquipmentStashSlotUI(0);
    }

    public void HideStash()
    {
        eBasementCanvas = EBasementCanvas.None;
        stashView.Hide();
        vcam2.SetActive(true);
        vcamStash.SetActive(false);
    }

    public void SelectStashTab(int idx)
    {
        foreach (var button in stashTabButtonList)
            button.image.color = normalColor;

        for (int i = 0; i < stashTabButtonList.Count; i++)
            if (i == idx)
            {
                stashTabButtonList[idx].image.color = selectColor;
                currentStashTabIdx = idx;
                SetStashScrollViewPanel((EStashTab)idx);
                break;
            }
    }

    public void SetStashScrollViewPanel(EStashTab tab)
    {
        foreach (var cg in stashCGList)
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }

        foreach (var sequence in stashScrollSequenceList)
        {
            sequence.Rewind();
        }

        stashScrollSequenceList[(int)tab].Restart();
        stashCGList[(int)tab].interactable = true;
        stashCGList[(int)tab].blocksRaycasts = true;

        switch (tab)
        {
            case EStashTab.Equipment:
                HoverEquipmentStashSlotUI(0);
                break;
            case EStashTab.Consume:
                HoverConsumptionStashSlotUI(0);
                break;
            case EStashTab.Material:
                HoverMaterialStashSlotUI(0);
                break;
        }
    }

    public void HoverEquipmentStashSlotUI(int idx)
    {
        for (int i = 0; i < equipmentStashSlotUIList.Count; i++)
            if (i == idx)
            {
                equipmentStashSlotUIList[i].HoverEnter();
                EventSystem.current.SetSelectedGameObject(equipmentStashSlotUIList[0].gameObject);
                break;
            }
    }

    public void HoverConsumptionStashSlotUI(int idx)
    {
        for (int i = 0; i < consumeStashSlotUIList.Count; i++)
            if (i == idx)
            {
                consumeStashSlotUIList[i].HoverEnter();
                EventSystem.current.SetSelectedGameObject(consumeStashSlotUIList[0].gameObject);
                break;
            }
    }

    public void HoverMaterialStashSlotUI(int idx)
    {
        for (int i = 0; i < materialStashSlotUIList.Count; i++)
            if (i == idx)
            {
                materialStashSlotUIList[i].HoverEnter();
                EventSystem.current.SetSelectedGameObject(materialStashSlotUIList[0].gameObject);
                break;
            }
    }

    public void PreviousStashTab()
    {
        currentStashTabIdx--;
        if (currentStashTabIdx < 0)
        {
            currentStashTabIdx = 0;
            return;
        }

        SelectStashTab(currentStashTabIdx);
        MasterAudio.PlaySound("Click");
    }

    public void NextStashTab()
    {
        currentStashTabIdx++;
        if (currentStashTabIdx > 2)
        {
            currentStashTabIdx = 2;
            return;
        }

        SelectStashTab(currentStashTabIdx);
        MasterAudio.PlaySound("Click");
    }

    public void InitEquipmentStashInventory()
    {
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