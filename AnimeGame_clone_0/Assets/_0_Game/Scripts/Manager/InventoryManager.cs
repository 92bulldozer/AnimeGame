
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Doozy.Engine;
using Doozy.Engine.UI;
using EJ;
using I2.Loc;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        public UIView inventoryView;
        public bool isInventory;
        public int testItemCount;
        private Player _player;
        public StringBuilder sbTitle;
        public StringBuilder sbDescription;
        public Image detailIcon;
        public Localize detailTitleLocalize;
        public Localize detailDescriptionLocalize;
        
        public List<StashSlotUI> inventorySlotUIList;
        
        public List<StashSlot> inventorySlotDataList;
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

        private void Start()
        {
            //TestDelay().Forget();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //AddEquipmentItem(0);
                "inventory 1".Log();
                //TestDelay().Forget();
                AddInventoryItem(0);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Save();
                "inventory 2".Log();
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Load();
                "inventory 3".Log();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AllClearInventoryItem();
                //AddEquipmentItem(1);
                "inventory 4".Log();
            }
            
            if (_player.GetButtonDown("Inventory"))
            {
                try
                {
                    
                    if(!isInventory)
                        ShowInventory();
                    else
                        HideInventory();
                }
                catch (Exception e)
                {
                    e.Log();
                }
                
            }
            
            
            ProcessCanvasInput();
        }

        private void Init()
        { 
            _player = ReInput.players.GetPlayer(0);
            sbTitle = new StringBuilder();
            sbDescription = new StringBuilder();
            
            inventorySlotDataList = new List<StashSlot>();
            equipmentStashSlotDataList = new List<StashSlot>();
            consumeStashSlotDataList = new List<StashSlot>();
            materialStashSlotDataList = new List<StashSlot>();

            for (int i = 0; i < 36; i++)
            {
                inventorySlotDataList.Add(new StashSlot());
                equipmentStashSlotDataList.Add(new StashSlot());
                consumeStashSlotDataList.Add(new StashSlot());
                materialStashSlotDataList.Add(new StashSlot());
            }
            
            Load();
            
            
        }
        
        
        public void ProcessCanvasInput()
        {
            if(isInventory)
                InventoryInput();

           
        }
        
        public void InventoryInput()
        {
            if (_player.GetNegativeButtonDown("UIHorizontal"))
            {
                if (_player.GetAxis("UIHorizontal") < 0)
                {
                    "Basement UI Left".Log();
                }
            }
            else if (_player.GetButtonDown("UIHorizontal"))
            {
                if (_player.GetAxis("UIHorizontal") > 0)
                {
                    "Basement UI Right".Log();
                }
            }

            if (_player.GetNegativeButtonDown("UIVertical"))
            {
                if (_player.GetAxis("UIVertical") < 0)
                {
                    "Basement UI Down".Log();
                }
            }
            else if (_player.GetButtonDown("UIVertical"))
            {
                if (_player.GetAxis("UIVertical") > 0)
                {
                    "Basement UI Up".Log();
                }
            }

            if (_player.GetButtonDown("Prev"))
            {
                "Basement UI Prev".Log();
            }

            if (_player.GetButtonDown("Next"))
            {
                "Basement UI Next".Log();
            }

            if (_player.GetButtonDown("UISubmit"))
            {
                //"Basement UI Submit".Log();
            }

            if (_player.GetButtonDown("UICancel"))
            {
                "Basement UI Cancel".Log();
            }
            
            
        }
        
        public void ShowInventory()
        {
            DOVirtual.DelayedCall(0.5f, () =>   isInventory = true);

            inventoryView.Show();
           
            HoverInventorySlotUI(0);
            "ShowInventory".Log();

        }

        public void HideInventory()
        {
            isInventory = false;
            inventoryView.Hide();
            "HideInventory".Log();

        }
        
        public void HoverInventorySlotUI(int idx)
        {
            for (int i = 0; i < inventorySlotUIList.Count; i++)
                if (i == idx)
                {
                    inventorySlotUIList[i].HoverEnter();
                    EventSystem.current.SetSelectedGameObject(inventorySlotUIList[0].gameObject);
                    break;
                }
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
        
        public void UpdateInventoryUI()
        {
            "UpdateInventorySlotUI".Log();
            for (int i = 0; i < inventorySlotUIList.Count; i++)
            {
                if (inventorySlotDataList[i].isEmpty)
                {
                    inventorySlotUIList[i].SetSlotEmpty();
                    continue;
                }

                try
                {
                    inventorySlotUIList[i].SetSlot(inventorySlotDataList[i]);
                }
                catch (Exception e)
                {
              
                }
          
            }
        }
        
        public void UpdateDetailPanel(ItemData itemData)
        {
            if (itemData == null)
            {
                sbTitle.Clear();
                sbTitle.Append("Item/NULL");
                sbDescription.Clear();
                sbDescription.Append("Item/NULL");
                detailTitleLocalize.Term = sbTitle.ToString();
                detailDescriptionLocalize.Term = sbDescription.ToString();
                detailIcon.gameObject.SetActive(false);
                return;
            }
        
            detailIcon.gameObject.SetActive(true);
            detailIcon.sprite = itemData.icon;
            sbTitle.Clear();
            sbTitle.Append("Item/");
            sbTitle.Append(itemData.name);
            sbDescription.Clear();
            sbDescription.Append("Item/");
            sbDescription.Append(itemData.description);
            detailTitleLocalize.Term = sbTitle.ToString();
            detailDescriptionLocalize.Term = sbDescription.ToString();
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
                   
                }
                
               
            }
        }
        
        public void AddInventoryItem(int itemID)
        {
            ItemData itemData = ItemDatabase.Instance.GetItem(itemID);
            foreach (var inventorySlotData in inventorySlotDataList)
            {
                if (inventorySlotData.isEmpty)
                {
                    inventorySlotData.SetStashSlot(itemData);
                    GameEventMessage.SendEvent("UpdateInventory");
                    break;
                }
                else
                {
                    if (inventorySlotData.itemData == itemData)
                    {
                        inventorySlotData.amount++;
                        inventorySlotData.isEmpty = false;
                        GameEventMessage.SendEvent("UpdateInventory");
                        break;
                    }
                   
                }
                
               
            }
        }

        public void AllClearInventoryItem()
        {
            foreach (var inventorySlotData in inventorySlotDataList)
            {
                inventorySlotData.SetEmpty();
            }
            GameEventMessage.SendEvent("UpdateInventory");
        }

        #region TestUniTask

        public async UniTaskVoid TestDelay()
        {
            Debug.Log("testDelayStart");
            //"testDelayStart".Log();
            
            AddItemCount().Forget();
            
            
            await UniTask.WaitUntil(()=>testItemCount==3);
            Debug.Log("<color=red>TestDelayEnd</color>");
            "rlahWL".Log(EColor.RED);
           

            
            //Debug.Log($"AddItemCount End  Result = {await AddItemCount()}");
        }

        public async UniTask<int> AddItemCount()
        {
            for (int i = 0; i < 10; i++)
            {
                testItemCount++;
                Debug.Log($"testItemCount = {testItemCount}");
                await UniTask.Delay(TimeSpan.FromSeconds(1));
            }

            return testItemCount;
        }
        
        #endregion


     
        
    }
}
