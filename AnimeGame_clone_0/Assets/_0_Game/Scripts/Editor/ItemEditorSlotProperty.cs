#if UNITY_EDITOR


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemWindow.ItemEditorSlot))]
public class ItemEditorSlotProperty : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        int space = 5;
        int propertyCount = 4;

        //var itemIDRect = new Rect(position.x, position.y, 30, position.height);
        var itemIDRect = new Rect(position.x, position.y, position.width / propertyCount, position.height);
        // var itemTypeRect = new Rect(position.x+35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + space + position.width / propertyCount, position.y, position.width / propertyCount, position.height);
        // var nameRect = new Rect(position.x+90, position.y, position.width-90, position.height);
        var iconRect = new Rect(position.x + space * 2 + position.width / propertyCount * 2, position.y, position.width / propertyCount, position.height);
        
        var itemTypeRect = new Rect(position.x + space * 3 + position.width / propertyCount * 3, position.y, position.width / propertyCount-15, position.height);

        EditorGUI.PropertyField(itemIDRect, property.FindPropertyRelative("itemID"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.PropertyField(iconRect, property.FindPropertyRelative("icon"), GUIContent.none);
        EditorGUI.PropertyField(itemTypeRect, property.FindPropertyRelative("itemType"), GUIContent.none);

        EditorGUI.indentLevel = indent;
    }
}
#endif