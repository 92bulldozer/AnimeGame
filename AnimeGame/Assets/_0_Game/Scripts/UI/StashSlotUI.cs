using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using EJ;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StashSlotUI : MonoBehaviour
{
    public Image icon;
    public int itemID = -1;
    public TextMeshProUGUI amountText;
    public GameObject highLightObject;
    public ItemData itemData;
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
    
    public void HoverEnter()=>
        highLightObject.SetActive(true);
    public void HoverExit()=>
        highLightObject.SetActive(false);

    

    public void SetSlot(StashSlot stashSlot)
    {
        icon.gameObject.SetActive(true);
        amountText.gameObject.SetActive(true);
        isEmpty = false;
        itemData = stashSlot.itemData;
        icon.sprite = itemData.icon;
        itemID = itemData.itemID;
        amountText.text = stashSlot.amount.ToString();

    }

    public void SetSlotEmpty()
    {
        isEmpty = true;
        itemData = null;
        icon.sprite = null;
        itemID = -1;
        amountText.text = 0.ToString();
        icon.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
    }

    public void UpdateStashDetailPanel()
    {
        if (itemData == null)
        {
            BasementPresenter.Instance.UpdateDetailPanel(null);
            return;
        }
        
        BasementPresenter.Instance.UpdateDetailPanel(itemData);
    }
    
}
