using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StashSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;
    public GameObject highLightObject;
    public bool isEmpty = true;

    [ContextMenu("AutoInit/AutoGetChild")]
    void GetChild()
    {
       
        icon = transform.GetChild(0).GetComponent<Image>();
        amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        highLightObject = transform.GetChild(3).gameObject;
     
    }


    public void OnClick()
    {
        $"{gameObject.name}".Log();
    }
    
    
    public void HoverEnter()
    {
        highLightObject.SetActive(true);
       
    }
    
    public void HoverExit()
    {
        highLightObject.SetActive(false);
        
    }
}
