using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Coffee.UIEffects;
using DG.Tweening;
using EJ;
using PathologicalGames;
using UnityEngine;
using UnityEngine.UI;

public class DropSlotUI : MonoBehaviour
{
    public Image icon;
    public Text countText;
    public Text expText;
    public UIGradient uiGradient;
    public Image glowEffectImg;
    public Sequence sequence;
    public Transform poolSpawner;
    public CanvasGroup cg;

    private WaitForSecondsRealtime ws;

    private void Awake()
    {
        sequence = DOTween.Sequence().Append(glowEffectImg.DOColor(new Color(0,0,0,0),0.3f).SetEase(Ease.InQuad)).SetAutoKill(false);
        ws = new WaitForSecondsRealtime(1.0f);
    }

 


    public void Init(string type,BigInteger amount,ref List<Color> uiColor, ref Transform poolSpawner)
    {
        this.poolSpawner = poolSpawner;
        
        switch (type)
        {
            case "Gold":
                icon.gameObject.SetActive(true);
                expText.gameObject.SetActive(false);
                uiGradient.offset = 0f;
                uiGradient.direction = UIGradient.Direction.Horizontal;
                uiGradient.color1 = uiColor[0];
                uiGradient.color2 = uiColor[1];
                countText.text = amount.ToUnitStringKR(1);
                break;
            case "Exp":
                icon.gameObject.SetActive(false);
                expText.gameObject.SetActive(true);
                uiGradient.offset = 0.329f;
                uiGradient.direction = UIGradient.Direction.Vertical;
                uiGradient.color1 = uiColor[0];
                uiGradient.color2 = uiColor[1];
                countText.text = amount.ToUnitStringKR(1);
                break;
        }
    }

    private void Disappear()
    {
        StartCoroutine(DisappearCor());
        
    }

    IEnumerator DisappearCor()
    {
        cg.alpha = 1;
        yield return ws;
        DOTween.To(() => cg.alpha, x => cg.alpha = x, 0, 0.2f);
        yield return ws;
        PoolManager.Pools["UI"].Despawn(transform,poolSpawner);
    }
    
    protected virtual void OnSpawned(SpawnPool pool)
    {
        sequence.Restart();
        Disappear();
    }

    protected virtual void OnDespawned(SpawnPool pool)
    {
       sequence.Rewind();
       
    }
}
