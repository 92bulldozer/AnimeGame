
using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine;
using EJ;
using UnityEngine;

namespace AnimeGame
{
    [Serializable]
    public class StashSlot
    {
        public ItemData itemData;
        public bool isEmpty;
        public int amount;

        public StashSlot(ItemData itemData=null, bool isEmpty=true ,int amount=0)
        {
            this.itemData = itemData;
            this.isEmpty = isEmpty;
            this.amount = amount;
        }

        public void SetStashSlot(ItemData itemData)
        {
            this.itemData = itemData;
            isEmpty = false;
            amount++;
        }

        public void SetEmpty()
        {
            itemData = null;
            isEmpty = true;
            amount=0;
        }
    }
    
    
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        
        
        
        
        
        public List<StashSlot> equipmentStashSlotDataList;
        public List<StashSlot> consumeStashSlotDataList;
        public List<StashSlot> materialStashSlotDataList;
        

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AddEquipmentItem(0);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Save();
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Load();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AddEquipmentItem(1);
            }
        }

        private void Init()
        {
            equipmentStashSlotDataList = new List<StashSlot>();
            consumeStashSlotDataList = new List<StashSlot>();
            materialStashSlotDataList = new List<StashSlot>();

            for (int i = 0; i < 36; i++)
            {
                equipmentStashSlotDataList.Add(new StashSlot());
                consumeStashSlotDataList.Add(new StashSlot());
                materialStashSlotDataList.Add(new StashSlot());
            }
            
            Load();
            
            
        }

        public void Save()
        {
            "InventorySave".Log();
            ES3AutoSaveMgr.Current.Save();
        }

        public void Load()
        {
            "InventoryLoad".Log();
            ES3AutoSaveMgr.Current.Load();
            GameEventMessage.SendEvent("UpdateInventory");
        }

        public void AddEquipmentItem(int itemID)
        {
            ItemData itemData = ItemDatabase.Instance.GetItem(itemID);
            foreach (var equipmentSlotData in equipmentStashSlotDataList)
            {
                if (equipmentSlotData.isEmpty)
                {
                    equipmentSlotData.SetStashSlot(itemData);
                    GameEventMessage.SendEvent("UpdateInventory");
                    break;
                }
                else
                {
                    if (equipmentSlotData.itemData == itemData)
                    {
                        equipmentSlotData.amount++;
                        equipmentSlotData.isEmpty = false;
                        GameEventMessage.SendEvent("UpdateInventory");
                        break;
                    }
                    else
                    {
                        
                    }
                }
                
               
                
               
            }
        }

     
        
    }
}
