using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AnimeGame;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DarkTonic.MasterAudio;
using DG.Tweening;
using EJ;
using FishNet;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using I2.Loc;
using INab.BetterFog.URP;
using MoreMountains.Feedbacks;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ELanguage
{
    KOREAN=0,
    ENGLISH
}

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Space(20)] [Header("Field")] [Space(10)]
    private NetworkManager _networkManager;
    public bool isDiscovered;
    public PlayerPresenter playerPresenter;
    public List<GameObject> playerPrefabList;
    public GameObject firstVirtualCamera;
    public Color inSideAmbientColor;
    public List<Color> outSideAmbientColor;
    

    [Space(20)] [Header("Players")] [Space(10)]
    public List<PlayerPresenter> playerList;

    public List<Transform> playerSpawnTransformList;

    [Space(20)] [Header("MMF")] [Space(10)]
    public MMF_Player ca;
    public MMF_Player caReverse;
    public MMF_Player jumpScareCamera;
    public MMF_Player MMF_CameraShake;

    [Space(20)] [Header("Volume")] [Space(10)]
    public Volume volume;

    public BetterFogVolume betterFogVolume;
    public float mentality;

    private VolumeProfile _volumeProfile;

    [Space(20)] [Header("Input")] [Space(10)]
    private Player _player;

    [Space(20)] [Header("UI")] [Space(10)] 
    public List<RectTransform> rebuildList;

    [Space(20)] [Header("Camera")] [Space(10)]
    public CinemachineBrain cb;
    public List<CinemachineVirtualCamera> virtualCameraList;


    [Space(20)] [Header("AStar")] [Space(10)]
    public AstarPath astarPath;
    public GameObject enemyPrefab;
    public GameObject pickupObjectTest;
    
    

    private static readonly int PlaneHeight = Shader.PropertyToID("_PlaneHeight");


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

    private void Start()
    {
        //DecreaseMentality().Forget();
    }

 

    

    private void Init()
    {
        _player = ReInput.players.GetPlayer(0);
        LanguageInit();
        // SetPlayer();
        // SetupAllVirtualCamera();
        
    }

    public void LanguageInit()
    {
        //ES3.Save("Language","ENGLISH");
        LocalizationManager.CurrentLanguage = ES3.Load<string>("Language","English",true);
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     "GameManager 0".Log();
        //     ScanUpdate();
        // }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            "GameManager 1".Log();
            //SpawnEnemy();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            "GameManager 5".Log();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            "GameManager 5".Log();
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            "GameManagerZ".Log();
            //Discovered();
            LocalizationManager.CurrentLanguage = ELanguage.KOREAN.ToString() ;
            ES3.Save<string>("Language","KOREAN");
            UIForceRebuild();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            "GameManagerX".Log();
            LocalizationManager.CurrentLanguage = ELanguage.ENGLISH.ToString() ;
            ES3.Save("Language","ENGLISH");
            UIForceRebuild();
            //NotDiscovered();

        }

       
    }

    public void UIForceRebuild()
    {
        StartCoroutine(UIForceRebuildCor());

    }

    public IEnumerator UIForceRebuildCor()
    {
        foreach (var VARIABLE in rebuildList)
            LayoutRebuilder.ForceRebuildLayoutImmediate(VARIABLE);
        
        yield return ExtensionMethods.CoroutineHelper.WaitForSeconds(0.1f);
        
        foreach (var VARIABLE in rebuildList)
            LayoutRebuilder.ForceRebuildLayoutImmediate(VARIABLE);
        
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

    public void SetPlayer(PlayerPresenter _playerPresenter)
    {
        // GameObject playerObj = Instantiate(playerPrefabList[Random.Range(0, playerPrefabList.Count)], new Vector3(3,-0.5f,5.8f),
        //     Quaternion.Euler(0, -90, 0));
        // GameObject playerObj = Instantiate(playerPrefabList[1], new Vector3(24,-0.5f,11f),
        //     Quaternion.Euler(0, -90, 0));
        // playerPresenter = playerObj.transform.GetChild(2).GetComponent<PlayerPresenter>();
        
        playerPresenter = _playerPresenter;
    }

    public void AddVirtualCamera(CinemachineVirtualCamera cvc)
    {
        virtualCameraList.Add(cvc);
    }

    public void SetupAllVirtualCamera()
    {
        foreach (var cinemachineVirtualCamera in virtualCameraList)
        {
            cinemachineVirtualCamera.Follow = playerPresenter.transform;
            cinemachineVirtualCamera.LookAt = playerPresenter.transform;
        }
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

    public void ChangeAmbientLight(bool isInside)
    {
        switch (isInside)
        {
            case true:
                // RenderSettings.ambientMode = AmbientMode.Flat;
                // RenderSettings.ambientSkyColor = inSideAmbientColor;
                break;
            case false:
                RenderSettings.ambientMode = AmbientMode.Trilight;
                RenderSettings.ambientSkyColor = outSideAmbientColor[0];
                RenderSettings.ambientEquatorColor = outSideAmbientColor[1];
                RenderSettings.ambientGroundColor = outSideAmbientColor[2];
                break;
        }
    }

    public async UniTaskVoid DecreaseMentality()
    {
        float heightDensity = 0;
        while (true)
        {

            
           
            heightDensity += Time.deltaTime * 0.1f;
            mentality += Time.deltaTime;
            
            _volumeProfile.TryGet(out betterFogVolume);
            if (betterFogVolume)
            {
                betterFogVolume._Height.value = mentality;

            }
            
            await UniTask.Yield();

            if (mentality > 35)
            {
                "게임오버".Log(EColor.RED);
                break;
            }
        }
    }

    public void ScanUpdate()
    {
        Debug.Log("UpdateScan");
        astarPath.Scan();
    }

    public void SpawnEnemy()
    {
        if (!HasAuthority)
            return;
       
        
        try
        {
            GameObject enemyObj = Instantiate(enemyPrefab, new Vector3(-6, -0.5f, 6), Quaternion.identity);
            ServerManager.Spawn(enemyObj);
        }
        catch (Exception e)
        {
            
        }
        
       

    }


   

   
   
    
    
}
