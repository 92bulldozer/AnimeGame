using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

public class MainMenuPresenter : MonoBehaviour
{
    public static MainMenuPresenter Instance;
    
    [Header("View")] [Space(20)] 
    public UIView mainMenuView;
    public UIView mainMenuOption;
    
    
    
    //[Header("UI")] [Space(20)] 

    [Header("Field")] [Space(20)] 
    public TMP_ColorGradient colorGradient;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
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
}
