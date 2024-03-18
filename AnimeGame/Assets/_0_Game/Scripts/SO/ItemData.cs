using System.Numerics;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public string itemDescription;
    public bool isStackable;
    public EItemType itemType;
    public Sprite iconSprite;
}
