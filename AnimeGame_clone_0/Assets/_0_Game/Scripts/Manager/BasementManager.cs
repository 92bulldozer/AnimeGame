using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementManager : MonoBehaviour
{
    public static BasementManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }


    private void Init()
    {
        Cursor.visible = false;
    }
}
