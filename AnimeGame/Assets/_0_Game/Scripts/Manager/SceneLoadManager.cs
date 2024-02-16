using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine;
using Doozy.Engine.SceneManagement;
using Doozy.Engine.UI;
using EJ;
using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance;

    public bool isCompleteTransition;
    public ParticleSystem transitionParticleSystem;
    private SceneLoader _sceneLoader;
    private WaitForSeconds sceneWs;
    private IEnumerator loadSceneIenumerator;
    private IEnumerator loadSceneDelayIenumerator;
    private IEnumerator preLoadSceneIenumerator;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject.transform.root.gameObject);
        }

        DontDestroyOnLoad(gameObject.transform.root.gameObject);
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
            TransitionBackward();
    }

    private void Init()
    {
        _sceneLoader = GetComponent<SceneLoader>();
        sceneWs = new WaitForSeconds(0.1f);
    }

    public void PreLoadSceneAsync()
    {
        if (preLoadSceneIenumerator != null)
            StopCoroutine(preLoadSceneIenumerator);

        preLoadSceneIenumerator = PreLoadSceneAsyncCor();
        StartCoroutine(preLoadSceneIenumerator);
    }

    IEnumerator PreLoadSceneAsyncCor()
    {
        while (true)
        {
            if (isCompleteTransition)
            {
                _sceneLoader.ActivateLoadedScene();
                yield break;
            }

            yield return sceneWs;
        }
    }

    public void LoadSceneAsync()
    {
        if (loadSceneIenumerator != null)
            StopCoroutine(loadSceneIenumerator);

        loadSceneIenumerator = LoadSceneAsyncCor();
        StartCoroutine(loadSceneIenumerator);
    }

    IEnumerator LoadSceneAsyncCor()
    {
        while (true)
        {
            if (isCompleteTransition)
            {
                _sceneLoader.LoadSceneAsync();
                yield break;
            }

            yield return sceneWs;
        }
    }

    public void LoadSceneAsync(string sceneName)
    {
        if (loadSceneIenumerator != null)
            StopCoroutine(loadSceneIenumerator);

        loadSceneIenumerator = LoadSceneAsyncCor(sceneName);
        StartCoroutine(loadSceneIenumerator);
    }
    
    

    IEnumerator LoadSceneAsyncCor(string sceneName)
    {
        GameEventMessage.SendEvent("FadeOut");
        while (true)
        {
            Debug.Log("씬트랜지션 대기중");
            if (isCompleteTransition)
            {
                Debug.Log("씬 트랜지션 완료");
                _sceneLoader.SceneName = sceneName;
                _sceneLoader.LoadSceneAsync();
                yield break;
            }

            yield return sceneWs;
        }
    }
    
    public void DelayLoadSceneAsync(string sceneName)
    {
        if (loadSceneDelayIenumerator != null)
            StopCoroutine(loadSceneDelayIenumerator);

        loadSceneDelayIenumerator = LoadSceneAsyncCor(sceneName,2);
        StartCoroutine(loadSceneDelayIenumerator);
    }
    
    IEnumerator LoadSceneAsyncCor(string sceneName,int delayTime)
    {
        GameEventMessage.SendEvent("FadeOut");
        yield return new WaitForSeconds(delayTime);
        
        
        while (true)
        {
            Debug.Log("씬트랜지션 대기중");
            
            if (isCompleteTransition)
            {
                Debug.Log("씬 트랜지션 완료");
                _sceneLoader.SceneName = sceneName;
                _sceneLoader.LoadSceneAsync();
                yield break;
            }

            yield return sceneWs;
            
        }
    }

    public void CompleteTransitionParam()
    {
        Debug.Log("CompleteTranstionParam");
        isCompleteTransition = true;
    }

    public void ResetTransitionParam()
    {
        isCompleteTransition = false;
    }

    public void SetSceneName(string sceneName)
    {
        Debug.Log("SetSceneName");
        _sceneLoader.SceneName = sceneName;
    }

    public void TransitionBackward()
    {
        transitionParticleSystem.Stop();
        ParticleSystem.MainModule mm = transitionParticleSystem.main;
        mm.simulationSpeed = 0.1f;
    }
}
