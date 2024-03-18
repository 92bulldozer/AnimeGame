using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemDatabase : SerializedMonoBehaviour
{
    public static ItemDatabase Instance;

    public Dictionary<string, int> dic;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (var VARIABLE in dic)
        {
            Debug.Log( $"{VARIABLE.Key} {VARIABLE.Value}");
        }
    }
}
