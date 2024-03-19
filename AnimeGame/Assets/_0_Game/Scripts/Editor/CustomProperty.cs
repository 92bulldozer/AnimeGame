#if UNITY_EDITOR


using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemWindow.Ingredient))]
public class CustomProperty : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var amountRect = new Rect(position.x, position.y, 30, position.height);
        var uniRect = new Rect(position.x+35, position.y, 50, position.height);
        var nameRect = new Rect(position.x+90, position.y, position.width-90, position.height);

        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
        EditorGUI.PropertyField(uniRect, property.FindPropertyRelative("unit"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

        EditorGUI.indentLevel = indent;
    }
}
#endif
