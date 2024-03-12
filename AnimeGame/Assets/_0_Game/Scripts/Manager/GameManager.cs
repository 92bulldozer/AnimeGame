using System;
using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using DarkTonic.MasterAudio;
using DG.Tweening;
using EJ;
using MoreMountains.Feedbacks;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Space(20)] [Header("Field")] [Space(10)]
    public Transform target;
    public bool isDiscovered;
    
    [Space(20)] [Header("MMF")] [Space(10)]
    public MMF_Player ca;
    public MMF_Player caReverse;
    public MMF_Player jumpScareCamera;

    [Space(20)] [Header("Input")] [Space(10)]
    private Player _player;
    
    
    
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
        _player = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            "GameManagerZ".Log();
            Discovered();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            "GameManagerX".Log();
            NotDiscovered();
        }
    }


    IEnumerator DelayForceCor(Ray ray, RaycastHit hit)
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 forceDirection = (hit.point - ray.origin).normalized;
        float force = 150;
        hit.transform.GetComponent<Rigidbody>().AddForce(forceDirection*force,ForceMode.Impulse);
    }

    public void NotDiscovered()
    {
        //ca.Direction = MMFeedbacks.Directions.BottomToTop;
        caReverse.PlayFeedbacks();
        MasterAudio.FadeOutOldBusVoices("HeatBeat",0.5f,0.5f);

    }

    public void Discovered()
    {
        //ca.Direction = MMFeedbacks.Directions.TopToBottom;
        ca.PlayFeedbacks();
        MasterAudio.PlaySound("HeatBeatLoop");
        MasterAudio.PlaySound("JumpScare");
        jumpScareCamera.PlayFeedbacks();

    }
    
    public void EnableInput()
    {
        foreach (var VARIABLE in _player.controllers.maps.GetAllMaps())
        {
            if (VARIABLE.categoryId == 0)
                VARIABLE.enabled = true;
        }
        
        foreach (var VARIABLE in _player.controllers.maps.GetAllMaps())
        {
            if (VARIABLE.categoryId == 1)
                VARIABLE.enabled = false;
        }
        
        EventSystem.current.SetSelectedGameObject(null);
      
    }

    public void DisableInput()
    {


        foreach (var VARIABLE in _player.controllers.maps.GetAllMaps())
        {
            if (VARIABLE.categoryId == 0)
                VARIABLE.enabled = false;
        }
      
        foreach (var VARIABLE in _player.controllers.maps.GetAllMaps())
        {
            if (VARIABLE.categoryId == 1)
                VARIABLE.enabled = true;
        }
    }
    
    
    
    
    
}
