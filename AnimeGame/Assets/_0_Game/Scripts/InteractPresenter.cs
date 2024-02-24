using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using EJ;
using UnityEngine;

public class InteractPresenter : MonoBehaviour
{
    public static InteractPresenter Instance;

    [Header("View")] [Space(10)] 
    public UIView view;

    [Header("UI")] [Space(10)] 
    public RectTransform panelTransform;
    public Canvas interactionCanvas;
    private Transform _targetTransform;

    [Header("Field")] [Space(10)] 
    public Vector3 panelOffset;
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

   
    
    void LateUpdate()
    {
        UpdateInteractPanelPosition();

    }


    public void ShowInteractPanel(Transform target,Vector3 offset)
    {
        view.Show();
        
        // 즉시 업데이트
        panelOffset = offset;
        _targetTransform = target;
        var vp = Camera.main.WorldToViewportPoint(_targetTransform.transform.position+ panelOffset);
        var sp = interactionCanvas.worldCamera.ViewportToScreenPoint(vp);
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(panelTransform, sp, interactionCanvas.worldCamera, out worldPoint);
        panelTransform.position = worldPoint ;
    }

    public void HideInteractPanel()
    {
        view.Hide();
        _targetTransform = null;
        panelOffset = Vector3.zero;
    }

   

    public void UpdateInteractPanelPosition()
    {
        if(view.IsVisible)
        {
            var vp = Camera.main.WorldToViewportPoint(_targetTransform.transform.position + panelOffset);
            var sp = interactionCanvas.worldCamera.ViewportToScreenPoint(vp);
            Vector3 worldPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(panelTransform, sp, interactionCanvas.worldCamera, out worldPoint);
            panelTransform.position = worldPoint ;
        }

    }
}
