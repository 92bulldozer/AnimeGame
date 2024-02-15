using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void OnSelected()
    {
        text.colorGradientPreset = MainMenuPresenter.Instance.colorGradient;
    }

    public void OnDeSelected()
    {
        text.colorGradientPreset = null;

    }

    
}
