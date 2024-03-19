using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using UnityEngine;

[Serializable]
public class ItemDicionary
{
    public int id;
    public string name;
}

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public List<ItemDicionary> itemDictionary;

    public Dictionary<int, string> _itemDictionary;
    
    
    //[health]


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
        
        Init();
    }

    private void Init()
    {
        _itemDictionary = new Dictionary<int, string>();
        foreach (var VARIABLE in itemDictionary)
        {
            _itemDictionary.Add(VARIABLE.id,VARIABLE.name);
        }

        foreach (var VARIABLE in _itemDictionary)
        {
            $"{VARIABLE.Key} {VARIABLE.Value}".Log();
        }

    }

    private void Start()
    {

    }
}
