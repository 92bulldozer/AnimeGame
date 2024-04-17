using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenuPresenter : MonoBehaviour
{
    public static EscMenuPresenter Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        HideCursor();
    }

    public void ShowEscMenu()
    {
        
    }

    public void HideEscMenu()
    {
        
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
