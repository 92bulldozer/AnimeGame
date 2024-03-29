using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Engine.UI;
using EJ;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuPresenter : MonoBehaviour
{
    public static MainMenuPresenter Instance;
    
    [Header("View")] [Space(20)] 
    public UIView mainMenuView;
    public UIView mainMenuOption;
    public UIView creditView;



    [Header("UI")] [Space(20)] 
    public TextMeshProUGUI controlButtonText;
    public TextMeshProUGUI graphicButtonText;
    public TextMeshProUGUI soundButtonText;
    public TextMeshProUGUI languageButtonText;
    public List<TextMeshProUGUI> tabPanelTextList;
    public Scrollbar creditScrollbar;


    [Header("Field")] [Space(20)] 
    public bool isCreditMenu;

    public float creditTime;
    public TMP_ColorGradient colorGradient;

    public GameObject gameStartBtnObject;
    public GameObject optionMenuBtnObject;
    public GameObject controlButtonObject;
    public GameObject creditButtonObject;
    public GameObject exitBtnObject;
    
    public CanvasGroup controlPanelCG;
    public CanvasGroup graphicPanelCG;
    public CanvasGroup soundPanelCG;
    public CanvasGroup languagePanelCG;
    public List<CanvasGroup> cgList;
    public List<Sequence> sequenceList;

    public Sequence controlPanelSequence;
    public Sequence graphicPanelSequence;
    public Sequence soundPanelSequence;
    public Sequence languagePanelSequence;

    public GameObject controlPanelFirstGameObject;
    public GameObject graphicPanelFirstGameObject;
    public GameObject soundPanelFirstGameObject;
    public GameObject languagePanelFirstGameObject;

    
    private Player _player;
    private Sequence creditSequence;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

        cgList = new List<CanvasGroup>();
        cgList.Add(controlPanelCG);
        cgList.Add(graphicPanelCG);
        cgList.Add(soundPanelCG);
        cgList.Add(languagePanelCG);

        controlPanelSequence = DOTween.Sequence();
        controlPanelSequence.Append(DOTween.To(
            () => controlPanelCG.alpha, x => controlPanelCG.alpha = x, 1, 1)).SetAutoKill(false);
        
        graphicPanelSequence = DOTween.Sequence();
        graphicPanelSequence.Append(DOTween.To(
            () => graphicPanelCG.alpha, x => graphicPanelCG.alpha = x, 1, 1)).SetAutoKill(false);
        
        soundPanelSequence = DOTween.Sequence();
        soundPanelSequence.Append(DOTween.To(
            () => soundPanelCG.alpha, x => soundPanelCG.alpha = x, 1, 1)).SetAutoKill(false);
        
        languagePanelSequence = DOTween.Sequence();
        languagePanelSequence.Append(DOTween.To(
            () => languagePanelCG.alpha, x => languagePanelCG.alpha = x, 1, 1)).SetAutoKill(false);

        sequenceList = new List<Sequence>();
        sequenceList.Add(controlPanelSequence);
        sequenceList.Add(graphicPanelSequence);
        sequenceList.Add(soundPanelSequence);
        sequenceList.Add(languagePanelSequence);

        tabPanelTextList = new List<TextMeshProUGUI>();
        tabPanelTextList.Add(controlButtonText);
        tabPanelTextList.Add(graphicButtonText);
        tabPanelTextList.Add(soundButtonText);
        tabPanelTextList.Add(languageButtonText);
        
        EventSystem.current.SetSelectedGameObject(gameStartBtnObject);
        
        _player = ReInput.players.GetPlayer(0);
        creditSequence = DOTween.Sequence().SetAutoKill(false).Pause()
            .Append(DOTween.To(() => creditScrollbar.value, x => creditScrollbar.value = x, 1.0f, creditTime));

    }

    private void Update()
    {
        if (_player.GetAnyButton())
        {
            "AnyButtonDown".Log();
            
            if(isCreditMenu)
                CloseCreditMenu();
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif


    }

    public void OpenOptionMenu()
    {
        mainMenuView.Hide();
        mainMenuOption.Show();
        EventSystem.current.SetSelectedGameObject(controlButtonObject);
        
      
    }

    public void CloseOptionMenu()
    {
        mainMenuView.Show();
        mainMenuOption.Hide();
        EventSystem.current.SetSelectedGameObject(optionMenuBtnObject);
     
    }

    public void OpenCreditMenu()
    {
        DOVirtual.DelayedCall(0.5f,()=> isCreditMenu = true);
        creditScrollbar.value = 0;
       
        creditView.Show();
        mainMenuView.Hide();
        creditSequence.Restart();
    }

    public void CloseCreditMenu()
    {
        isCreditMenu = false;
        mainMenuView.Show();
        creditView.Hide();
        creditScrollbar.value = 0;
        EventSystem.current.SetSelectedGameObject(creditButtonObject);
      
    }
    
    

    public void SelectControlPanel()
    {
        foreach (var cg in cgList)
            cg.alpha = 0;
        foreach (var sequence in sequenceList)
            sequence.Rewind();
        foreach (var textMeshProUGUI in tabPanelTextList)
            textMeshProUGUI.alpha = 0.5f;
        
        controlButtonText.alpha = 1;
        controlPanelSequence.Restart();
    }
    
    public void SelectGraphicPanel()
    {
        foreach (var cg in cgList)
            cg.alpha = 0;
        foreach (var sequence in sequenceList)
            sequence.Rewind();
        foreach (var textMeshProUGUI in tabPanelTextList)
            textMeshProUGUI.alpha = 0.5f;
        
        graphicButtonText.alpha = 1;
        graphicPanelSequence.Restart();
    }
    
    public void SelectSoundPanel()
    {
        foreach (var cg in cgList)
            cg.alpha = 0;
        foreach (var sequence in sequenceList)
            sequence.Rewind();
        foreach (var textMeshProUGUI in tabPanelTextList)
            textMeshProUGUI.alpha = 0.5f;
        
        soundButtonText.alpha = 1;
        soundPanelSequence.Restart();
    }
    
    public void SelectLanguagePanel()
    {
        foreach (var cg in cgList)
            cg.alpha = 0;
        foreach (var sequence in sequenceList)
            sequence.Rewind();
        foreach (var textMeshProUGUI in tabPanelTextList)
            textMeshProUGUI.alpha = 0.5f;
        
        languageButtonText.alpha = 1;
        languagePanelSequence.Restart();
    }

    public void OnClickControlPanel()
    {
        foreach (var cg in cgList)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
        controlPanelCG.interactable = true;
        controlPanelCG.blocksRaycasts = true;
        controlButtonText.alpha = 1;
        EventSystem.current.SetSelectedGameObject(controlPanelFirstGameObject);
    }
    
    public void OnClickGraphicPanel()
    {
        foreach (var cg in cgList)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
        graphicPanelCG.interactable = true;
        graphicPanelCG.blocksRaycasts = true;
        graphicButtonText.alpha = 1;
        EventSystem.current.SetSelectedGameObject(graphicPanelFirstGameObject);
    }
    
    public void OnClickSoundPanel()
    {
        foreach (var cg in cgList)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
        soundPanelCG.interactable = true;
        soundPanelCG.blocksRaycasts = true;
        soundButtonText.alpha = 1;
        EventSystem.current.SetSelectedGameObject(soundPanelFirstGameObject);

    }
    
    public void OnClickLanguagePanel()
    {
        foreach (var cg in cgList)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
        languagePanelCG.interactable = true;
        languagePanelCG.blocksRaycasts = true;
        languageButtonText.alpha = 1;
        EventSystem.current.SetSelectedGameObject(languagePanelFirstGameObject);
    }
}
