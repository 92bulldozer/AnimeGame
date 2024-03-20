#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Array2DEditor;
using EJ;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemWindow : EditorWindow
{
    public ItemEditorSlot[] consumeItemEditorSlotArray;
    public ItemEditorSlot[] equipmentItemEditorSlotArray;
    public ItemEditorSlot[] materialItemEditorSlotArray;
    public GameObject[] someThings;
    public Vector2 editorScrollPosition;
    
    static GUIStyle horizontalLine;
 

    public static ItemWindow _editorWindow;

    public int space;
    private Rect rect;

    ScriptableObject scriptableObj;
    SerializedObject serialObj;
    SerializedProperty serialProp;



    [MenuItem("Item/ItemWindow")]
    public static void ShowEditor()
    {
        _editorWindow = GetWindow<ItemWindow>();
        _editorWindow.titleContent = new GUIContent("ItemWindow");
        // Limit size of the window
        _editorWindow.minSize = new Vector2(450, 200);
        _editorWindow.maxSize = new Vector2(1920, 720);
    }

    void OnEnable()
    {
       
        serialObj = new SerializedObject(this);
        space = 5;
        UpdateItemSlot();
        
        horizontalLine = new GUIStyle();
        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset( 0, 0, 4, 4 );
        horizontalLine.fixedHeight = 1;

    }


    private void OnGUI()
    {
        editorScrollPosition = EditorGUILayout.BeginScrollView(editorScrollPosition);
        
       


        #region CreateAsset
        
        EditorGUILayout.BeginHorizontal ();
        
        //Create ConsumableItemData Asset
        if(GUILayout.Button("Consumable Item 생성",GUILayout.Height(50)))
        {
            Debug.Log("Create_ConsumableItemData");
            
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(ConsumableItemData).Name);
            
            ConsumableItemData consumableItemData = CreateInstance<ConsumableItemData>();
            consumableItemData.name = $"{guids.Length}_" + "consumableItem";
            consumableItemData.itemID = guids.Length;
            consumableItemData.itemType = EItemType.Consumption;
            string path = $"Assets/_0_Game/ItemSO/ConsumableItemData/{guids.Length}_" + "consumableItem.asset";
            AssetDatabase.CreateAsset(consumableItemData, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = consumableItemData;
            
            UpdateItemSlot();
            
        }
        
        
        //Create EquipmentItemData Asset
        if(GUILayout.Button("EquipmentItem 생성",GUILayout.Height(50)))
        {
            Debug.Log("Create_EquipmentItemData");
            
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(EquipmentItemData).Name);
            
            EquipmentItemData equipmentItemData = CreateInstance<EquipmentItemData>();
            equipmentItemData.name = $"{guids.Length+1000}_" + "equipmentItem";
            equipmentItemData.itemID = guids.Length+1000;
            equipmentItemData.itemType = EItemType.Equipment;
            string path = $"Assets/_0_Game/ItemSO/EquipmentItemData/{guids.Length+1000}_" + "equipmentItem.asset";
            AssetDatabase.CreateAsset(equipmentItemData, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = equipmentItemData;
            
            UpdateItemSlot();
            
        }
        
        
        //Create MaterialItemData Asset
        if(GUILayout.Button("Material Item 생성",GUILayout.Height(50)))
        {
            Debug.Log("Create_MaterialItemData");
            
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(MaterialItemData).Name);
            
            MaterialItemData materialItemData = CreateInstance<MaterialItemData>();
            materialItemData.name = $"{guids.Length+2000}_" + "materialItem";
            materialItemData.itemID = guids.Length+2000;
            materialItemData.itemType = EItemType.Material;
            string path = $"Assets/_0_Game/ItemSO/MaterialItemData/{guids.Length+2000}_" + "materialItem.asset";
            AssetDatabase.CreateAsset(materialItemData, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = materialItemData;
            
            
            UpdateItemSlot();
            
        }
        
        
        
        if(GUILayout.Button("UpdateItemSlot",GUILayout.Height(50)))
        {
            Debug.Log("UpdateItemSlot");

            UpdateItemSlot();
            

        }
        
        
        
        EditorGUILayout.EndHorizontal ();
        #endregion


       
        // serialProp = serialObj.FindProperty("someThings");
        // EditorGUILayout.PropertyField(serialProp, true);
        // serialObj.ApplyModifiedProperties();
        
        GUILayout.Space(5);
        HorizontalLine(Color.white);
        GUILayout.Space(5);

        serialProp = serialObj.FindProperty("consumeItemEditorSlotArray");
        EditorGUILayout.PropertyField(serialProp, true);
        serialObj.ApplyModifiedProperties();
        serialObj.UpdateIfRequiredOrScript();
        
        
        
        serialProp = serialObj.FindProperty("equipmentItemEditorSlotArray");
        EditorGUILayout.PropertyField(serialProp, true);
        serialObj.ApplyModifiedProperties();
        serialObj.UpdateIfRequiredOrScript();
        
        serialProp = serialObj.FindProperty("materialItemEditorSlotArray");
        EditorGUILayout.PropertyField(serialProp, true);
        serialObj.ApplyModifiedProperties();
        serialObj.UpdateIfRequiredOrScript();
        
        GUILayout.Space(5);
        HorizontalLine(Color.white);
        GUILayout.Space(5);
        
        
        UpdateRect();
        GUILayout.BeginArea(new Rect(space,rect.y +rect.height+space,position.width - space*2,100),GUI.skin.window);
        GUILayout.EndArea();


        EditorGUILayout.EndScrollView();
    }

    public void UpdateRect()
    {
        if (GUILayoutUtility.GetLastRect().size.magnitude < 2)
            return;
        
        rect = GUILayoutUtility.GetLastRect();
        
    }


    public void UpdateItemSlot()
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(ConsumableItemData).Name);
            List<ItemEditorSlot> tempItemEditorSlot = new List<ItemEditorSlot>();
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                ConsumableItemData citem =  (ConsumableItemData)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ConsumableItemData));
                ItemEditorSlot itemEditorSlot = new ItemEditorSlot(citem.name,citem.icon, citem.itemID, citem.itemType);
                tempItemEditorSlot.Add(itemEditorSlot);
                consumeItemEditorSlotArray = tempItemEditorSlot.ToArray();
            }
            
            string[] guids2 = AssetDatabase.FindAssets("t:" + typeof(EquipmentItemData).Name);
            List<ItemEditorSlot> tempItemEditorSlot2 = new List<ItemEditorSlot>();
            for (int i = 0; i < guids2.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids2[i]);
                EquipmentItemData citem =  (EquipmentItemData)AssetDatabase.LoadAssetAtPath(assetPath, typeof(EquipmentItemData));
                ItemEditorSlot itemEditorSlot = new ItemEditorSlot(citem.name,citem.icon, citem.itemID, citem.itemType);
                tempItemEditorSlot2.Add(itemEditorSlot);
                equipmentItemEditorSlotArray = tempItemEditorSlot2.ToArray();
            }
            
            string[] guids3 = AssetDatabase.FindAssets("t:" + typeof(MaterialItemData).Name);
            List<ItemEditorSlot> tempItemEditorSlot3 = new List<ItemEditorSlot>();
            for (int i = 0; i < guids3.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids3[i]);
                MaterialItemData citem =  (MaterialItemData)AssetDatabase.LoadAssetAtPath(assetPath, typeof(MaterialItemData));
                ItemEditorSlot itemEditorSlot = new ItemEditorSlot(citem.name,citem.icon, citem.itemID, citem.itemType);
                tempItemEditorSlot3.Add(itemEditorSlot);
                materialItemEditorSlotArray = tempItemEditorSlot3.ToArray();
            }
    }
    
    static void HorizontalLine ( Color color ) {
        var c = GUI.color;
        GUI.color = color;
        GUILayout.Box( GUIContent.none, horizontalLine );
        GUI.color = c;
    }

    private void Update()
    {
        Repaint();
    }


    [Serializable]
    public class ItemEditorSlot
    {
        public string name;
        public int itemID;
        public Sprite icon;
        public EItemType itemType;

        public ItemEditorSlot(string name,Sprite _icon ,int itemID, EItemType itemType)
        {
            this.name = name;
            this.itemID = itemID;
            this.icon = _icon;
            this.itemType = itemType;
        }
    }
}


#endif