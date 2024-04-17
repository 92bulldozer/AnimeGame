using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HierachyCopy : MonoBehaviour
{
    public GameObject notifyImg;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI countText;
    public GameObject lockBG;
    public GameObject lockIcon;
    public GameObject activeImg;
    public GameObject activeText;
    
    [ContextMenu("AutoInit/AutoGetChild")]
    void GetChild()
    {
        notifyImg = transform.GetChild(2).gameObject;
        contentText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        countText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        lockBG = transform.GetChild(5).gameObject;
        lockIcon = transform.GetChild(6).gameObject;
        activeImg = transform.GetChild(7).gameObject;
        activeText = transform.GetChild(8).gameObject;
    }
}
