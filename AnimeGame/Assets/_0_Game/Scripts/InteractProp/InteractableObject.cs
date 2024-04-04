using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using DarkTonic.MasterAudio;
using DG.Tweening;
using Doozy.Engine;
using EJ;
using EPOOutline;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour,IInteract
{

    [Space(20)] [Header("Field")] [Space(10)]
    public int itemID = -1;
    public string getItemSfx;
    public bool canInteract = true;
    public Vector3 panelOffset;
    public Transform interactCenterPosition;
    public Vector3 centerPositionOffset;
    public bool isInteracted { get; set; }
    public bool affectOutlinable = false;
    public bool useReverseInteract = false;
    public bool isReverse = false;
    public EInteractText eInteractText;
    public EInteractText eInteractReverseText;
    public UnityEvent interactEvent;

    private Outlinable outlinable;
    
    public virtual void Awake()
    {
        canInteract = true;
        Init();
    }
    public virtual void Start()
    {
        if (!affectOutlinable)
            return;
        
        outlinable = GetComponent<Outlinable>();
        outlinable.enabled = false;
    }
    
#if UNITY_EDITOR
    [ContextMenu("AutoInit/AutoSetupBox")]
    void InitItemData()
    {
        getItemSfx = "GetItem";
        eInteractText = EInteractText.Interact_PickUp;
        eInteractReverseText = EInteractText.Interact_PickUp;
        affectOutlinable = true;
        
        if (interactEvent == null)
            interactEvent = new UnityEvent();
 
        var targetInfo = UnityEvent.GetValidMethodInfo(this, nameof(GetItem), new Type[0]);
        UnityAction methodDelegate = Delegate.CreateDelegate(typeof(UnityAction), this, targetInfo) as UnityAction;
        UnityEventTools.AddPersistentListener(interactEvent, methodDelegate);
        
        
        panelOffset = new Vector3(0, 0.5f, 0);
        gameObject.layer = LayerMask.NameToLayer("InteractableObject");
        GameObject childObject = new GameObject("InteractCenterPosition");
        childObject.transform.parent = transform;
        childObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        childObject.layer = LayerMask.NameToLayer("InteractableObject");
        
        Outlinable outlinable = gameObject.AddComponent<Outlinable>();
        outlinable.RenderStyle = RenderStyle.FrontBack;
        outlinable.BackParameters.Enabled = false;
        outlinable.FrontParameters.Enabled = true;
        outlinable.FrontParameters.Color = new Color32(58, 58, 58,255);
        outlinable.FrontParameters.DilateShift = 0.65f;
        gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<PhysicsSound>();
        gameObject.AddComponent<BoxCollider>();
      
    }
#endif
    
    
    public virtual void Init()
    {
        try
        {
            interactCenterPosition = transform.GetChild(0).GetComponent<Transform>();

        }
        catch (Exception e)
        {
            interactCenterPosition = transform;
        }
        interactCenterPosition.localPosition += centerPositionOffset;
    }
    
    public void EnableOutline()
    {
        if (affectOutlinable)
        {
            outlinable.enabled = true;
        }
    }

    public void DisableOutline()
    {
        if (affectOutlinable)
        {
            outlinable.enabled = false;
        }
    }
    
    public virtual void Interact()
    {
        //"interact".Log();
        if (isInteracted)
            return;
        
        interactEvent?.Invoke();
        
    }

    public virtual void ShowInteractPanel()
    {
        //"ShowInteractPanel".Log();
        if (useReverseInteract)
        {
            if (isReverse)
                InteractPresenter.Instance.ShowInteractPanel(interactCenterPosition,panelOffset,eInteractReverseText);
            else
                InteractPresenter.Instance.ShowInteractPanel(interactCenterPosition,panelOffset,eInteractText);
            
        }
        else
        {
            InteractPresenter.Instance.ShowInteractPanel(interactCenterPosition,panelOffset,eInteractText);

        }
    }

    public virtual void HideInteractPanel()
    {
        //"HideInteractPanel".Log();
        InteractPresenter.Instance.HideInteractPanel();
    }
    
    public void UpdateReverseText(bool _isReverse)
    {
        if (!_isReverse)
        {
            InteractPresenter.Instance.ShowInteractPanel(interactCenterPosition,panelOffset,eInteractText);
            isReverse=false;
            //"ReverseTrue".Log();
        }
        else
        {
            InteractPresenter.Instance.ShowInteractPanel(interactCenterPosition,panelOffset,eInteractReverseText);
            isReverse=true;
            //"ReverseFalse".Log();
        }
    }

    public void GetItem()
    {
        if (itemID == -1)
            return;
        
        InventoryManager.Instance.AddInventoryItem(itemID);
        DOVirtual.DelayedCall(0.4f, () =>
        {
            MasterAudio.PlaySound3DAtTransformAndForget(getItemSfx, transform);
            Destroy(gameObject);

        });

        if (eInteractText == EInteractText.Interact_PickUp)
        {
            PlayerPresenter.Instance.PickUp();
        }
        
    }
}
