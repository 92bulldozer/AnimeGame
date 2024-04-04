using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Doozy.Engine.UI;
using EJ;
using I2.Loc;
using TMPro;
using UnityEngine;


public enum EInteractText
{
    Interact_Stash=0,
    Interact_PickUp,
    Interact_Open,
    Interact_Close,
    Interact_Map,
    Interact_Equipment,
    Interact_Vegetation,
    Interact_Exit,
    Interact_Hide,
    Interact_ComeOut
}

public class InteractPresenter : MonoBehaviour
{
    public static InteractPresenter Instance;

    [Header("View")] [Space(10)] 
    public UIView view;

    [Header("UI")] [Space(10)] 
    public RectTransform panelTransform;
    public Canvas interactionCanvas;
    public GameObject currentInteractObject;
    public Transform _targetTransform;
   

    [Header("Field")] [Space(10)] 
    public Vector3 panelOffset;

    public Localize localize;
    public StringBuilder sb;


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


    void Init()
    {
        sb = new StringBuilder();
        if (interactionCanvas.worldCamera == null)
            interactionCanvas.worldCamera = Camera.main;
    }

    private void Update()
    {
        UpdateInteractPanelPosition();
    }


    void LateUpdate()
    {
        //UpdateInteractPanelPosition();

    }


    public void ShowInteractPanel(Transform target,Vector3 offset,EInteractText eInteractText)
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
        SetInteractText(eInteractText);


        // if (currentInteractObject != newGameObject && currentInteractObject != null)
        // {
        //     currentInteractObject.GetComponent<InteractSender>().HidePreviousInteractPanel();
        // }
        //
        // currentInteractObject = newGameObject;
    }
    
    public void SetInteractText(EInteractText eInteractText)
    {
        sb.Clear();
        sb.Append("Interact/");
        sb.Append(eInteractText.ToString());
        //sb.ToString().Log();
        localize.Term = sb.ToString();
    }

    public void HideInteractPanel()
    {
        
        view.Hide();
    }

    public void ResetTarget()
    {
        _targetTransform = null;
        panelOffset = Vector3.zero;
        currentInteractObject = null;
    }

   

    public void UpdateInteractPanelPosition()
    {
        if(view.IsShowing || view.IsVisible || view.IsHiding)
        {
            try
            {
                var vp = Camera.main.WorldToViewportPoint(_targetTransform.transform.position + panelOffset);
                var sp = interactionCanvas.worldCamera.ViewportToScreenPoint(vp);
                Vector3 worldPoint;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(panelTransform, sp, interactionCanvas.worldCamera, out worldPoint);
                panelTransform.position = worldPoint ;
            }
            catch (Exception e)
            {
                HideInteractPanel();
                ResetTarget();
            }
        }
      

    }
}
