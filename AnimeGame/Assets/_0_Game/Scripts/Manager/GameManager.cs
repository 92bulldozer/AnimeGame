using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using DG.Tweening;
using EJ;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Field")] [Space(20)] 
    public Transform target;
    public int layerMask;
    public bool isDiscovered;
    [Header("MMF")] [Space(20)] 
    public MMF_Player ca;
    
    
    
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


    private void Init()
    {
        layerMask = (-1) - ((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Interaction")));;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            "GameManagerZ".Log();
            DisCovered();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            "GameManagerX".Log();
            NotDisCovered();
        }
    }


    IEnumerator DelayForceCor(Ray ray, RaycastHit hit)
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 forceDirection = (hit.point - ray.origin).normalized;
        float force = 150;
        hit.transform.GetComponent<Rigidbody>().AddForce(forceDirection*force,ForceMode.Impulse);
    }

    public void NotDisCovered()
    {
        ca.Direction = MMFeedbacks.Directions.BottomToTop;
        ca.PlayFeedbacks();
    }

    public void DisCovered()
    {
        ca.Direction = MMFeedbacks.Directions.TopToBottom;
        ca.PlayFeedbacks();
    }
    
    
    
}
