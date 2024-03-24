
using System;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/Item Data")]
[Serializable]
public abstract class ItemData :ScriptableObject
{
    
    public int itemID;
    [ShowAssetPreview()][Label("")]
    public Sprite icon;
    public string name;
    [TextArea]
    public string description;
    public EItemType itemType;

}
