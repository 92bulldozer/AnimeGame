using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Instance;
    public List<TimelineAsset> timelineAssetList;

    public PlayableDirector pd;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
            Play(0);
        }
    }


    public void Play(int idx)
    {
        "Play".Log();
        pd.Play(timelineAssetList[idx]);
    }
}
