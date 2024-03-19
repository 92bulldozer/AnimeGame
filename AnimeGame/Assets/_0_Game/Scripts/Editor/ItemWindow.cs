#if UNITY_EDITOR

using System;
using Array2DEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemWindow : EditorWindow
{
    public Ingredient ingredient;
    public GameObject[] someThings;
    public Vector2 editorScrollPosition;
    public Array2DSprite arr2;

    [MenuItem("Item/ItemWindow")]
    public static void ShowEditor()
    {
        EditorWindow window = GetWindow<ItemWindow>();
        window.titleContent = new GUIContent("ItemWindow");
        // Limit size of the window
        window.minSize = new Vector2(450, 200);
        window.maxSize = new Vector2(1920, 720);
    }

   

    private void OnGUI()
    {
        editorScrollPosition = EditorGUILayout.BeginScrollView(editorScrollPosition);
        // Color defaultContentColor = GUI.contentColor;
        //
        // GUI.contentColor = Color.red;
        // GUILayout.Label("Label");
        // GUILayout.TextField("");
        // GUI.contentColor = defaultContentColor;
        // GUILayout.Label("Label2");
       

        #region CreateAsset

        EditorGUILayout.BeginHorizontal ();
        
        //Create ConsumableItemData Asset
        if(GUILayout.Button("Create_ConsumableItemData",GUILayout.Height(50)))
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
            
        }
        
        
        //Create EquipmentItemData Asset
        if(GUILayout.Button("Create_EquipmentItemData",GUILayout.Height(50)))
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
            
        }
        
        
        //Create MaterialItemData Asset
        if(GUILayout.Button("Create_MaterialItemData",GUILayout.Height(50)))
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
            
        }
        

        
        EditorGUILayout.EndHorizontal ();
        #endregion

        
        
        
        ScriptableObject scriptableObj = this;
        SerializedObject serialObj = new SerializedObject (scriptableObj);
        SerializedProperty serialProp = serialObj.FindProperty ("someThings");
 
        EditorGUILayout.PropertyField (serialProp, true);
        serialObj.ApplyModifiedProperties ();
        
        
        ScriptableObject scriptableObj2 = this;
        SerializedObject serialObj2 = new SerializedObject (scriptableObj2);
        SerializedProperty serialProp2 = serialObj2.FindProperty ("arr2");
 
        EditorGUILayout.PropertyField (serialProp2, true);
        serialObj2.ApplyModifiedProperties ();
        
        ScriptableObject scriptableObj3 = this;
        SerializedObject serialObj3 = new SerializedObject (scriptableObj3);
        SerializedProperty serialProp3 = serialObj3.FindProperty ("ingredient");
 
        EditorGUILayout.PropertyField (serialProp3, true);
        serialObj3.ApplyModifiedProperties ();
        
        
        
        EditorGUILayout.EndScrollView();
    }

    [Serializable]
    public class Ingredient
    {
        public string name;
        public int amount;
        public EItemType unit;
    }

   
}


#endif