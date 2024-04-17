#if UNITY_EDITOR
using System;

[Serializable]
public struct ItemSlot
{
    //public string itemID;
    public int ItemCount;
    public ItemData Item;
}
#endif
