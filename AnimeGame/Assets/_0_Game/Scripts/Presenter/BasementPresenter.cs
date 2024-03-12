using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasementPresenter : MonoBehaviour
{
    public static BasementPresenter Instance;

    [Space(20)] [Header("View")] [Space(10)]
    public UIView mapView;

    [Space(20)] [Header("Field")] [Space(10)]
    public GameObject mapCloseBtn;
    public int test;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
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
