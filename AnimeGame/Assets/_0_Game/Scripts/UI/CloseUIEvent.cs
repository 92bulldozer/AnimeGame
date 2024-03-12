using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using Doozy.Engine.UI;
using EJ;
using UnityEngine;

public class CloseUIEvent : MonoBehaviour
{
    public UIView uiView;
    
    private void Awake()
    {
        uiView = transform.parent.GetComponent<UIView>();
    }

  
    // Update is called once per frame
    void Update()
    {
        if (PlayerPresenter.Instance.player.GetButtonDown("UICancel") && uiView.IsVisible)
        {
            uiView.Hide();
        }
    }
}
