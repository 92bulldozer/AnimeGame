using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

[Serializable]
public class ItemDictionaryData
{
    public int id;
    public ItemData itemData;

    public ItemDictionaryData(int id, ItemData itemData)
    {
        this.id = id;
        this.itemData = itemData;
    }
}

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public List<ItemDictionaryData> itemDictionaryInspector;

    public Dictionary<int, ItemData> itemDictionary;

   


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
        itemDictionary = new Dictionary<int, ItemData>();
        foreach (var VARIABLE in itemDictionaryInspector)
            itemDictionary.Add(VARIABLE.id, VARIABLE.itemData);
        

        foreach (var VARIABLE in itemDictionary)
        {
            Debug.Log($"{VARIABLE.Key} {VARIABLE.Value.name}");
            //$"{VARIABLE.Key} {VARIABLE.Value}".Log();
        }
        
    }


#if UNITY_EDITOR
    [ContextMenu("AutoInit/Init_ItemData")]
    void InitItemData()
    {
        itemDictionaryInspector = new List<ItemDictionaryData>();
        
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(ConsumableItemData).Name);
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            ConsumableItemData citem =  (ConsumableItemData)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ConsumableItemData));
            itemDictionaryInspector.Add(new ItemDictionaryData(citem.itemID,citem));
        }
        
        guids = AssetDatabase.FindAssets("t:" + typeof(EquipmentItemData).Name);
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            EquipmentItemData eitem =  (EquipmentItemData)AssetDatabase.LoadAssetAtPath(assetPath, typeof(EquipmentItemData));
            itemDictionaryInspector.Add(new ItemDictionaryData(eitem.itemID,eitem));
        }
        
        guids = AssetDatabase.FindAssets("t:" + typeof(MaterialItemData).Name);
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            MaterialItemData eitem =  (MaterialItemData)AssetDatabase.LoadAssetAtPath(assetPath, typeof(MaterialItemData));
            itemDictionaryInspector.Add(new ItemDictionaryData(eitem.itemID,eitem));
        }
    }
#endif

    private void Start()
    {
    }

    public ItemData GetItem(int itemID)
    {
        if (itemDictionary.TryGetValue(itemID, out ItemData itemData))
        {
            return itemData;
        }

        return null;

    }
}