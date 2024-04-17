
using AnimeGame;
using Doozy.Engine;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.Events;
using MarkupAttributes;


public class CloseUIEvent : AnimeBehaviour
{

    public UIView uiView;
    public bool useMessage;
   
    [ShowIfGroup("Shown If Boolean", nameof(useMessage))]
    public string eventName;
    [HideIfGroup("Hidden If Bollean",nameof(useMessage))]
    public UnityEvent closeEvent;
   



    private void Awake()
    {
        uiView = transform.parent.GetComponent<UIView>();
    }

  
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.playerPresenter.player.GetButtonDown("UICancel") && uiView.IsVisible)
        {
            if (!useMessage)
                closeEvent?.Invoke();
            else
                GameEventMessage.SendEvent(eventName);
        }
    }
}
