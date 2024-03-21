
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/Item Data")]
[Serializable]
public abstract class ItemData :ScriptableObject
{
    
    public int itemID;
    public Sprite icon;
    public string name;
    public EItemType itemType;

}
