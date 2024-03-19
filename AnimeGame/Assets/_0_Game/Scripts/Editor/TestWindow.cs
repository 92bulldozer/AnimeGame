using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    int intValue;
    float floatValue;
    Color colorValue;
    Gradient gradientValue = new Gradient();
    Rect rectValue;
    Vector3 vector3Value;
    Vector3Int vectorInt3Value;
    UnityEngine.Object objectValue;
    public ConsumableItemData consumeData;
    string passwordValue;
    string tagValue;
    private string textValue;
    ParticleSystemCollisionType enumValue;
    
    
    string[] stringArr = new string[] { "a", "b", "c", "d", "e" };
    int selectionValue;
    
    public Vector2 editorScrollPosition;
    public Vector2 scrollPosition;
    public string longString = "메세지";
    
    
    [MenuItem("Item/TestWindow")]
    public static void InitEditor()
    {
        EditorWindow window = GetWindow<TestWindow>("Test");
        
    }


    private void OnGUI()
    {
        editorScrollPosition = EditorGUILayout.BeginScrollView(editorScrollPosition);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        {
            GUILayout.Label(longString);
            if (GUILayout.Button("지우기"))
                longString = "";
        }
        GUILayout.EndScrollView();

        if (GUILayout.Button("메세지 추가"))
            longString += "\n헬로우 월드";
        
        EditorGUILayout.LabelField("label value");
        textValue = EditorGUILayout.TextField("label value",textValue);
        intValue = EditorGUILayout.IntField("int value", intValue);
        floatValue = EditorGUILayout.FloatField("Float value", floatValue);
        colorValue = EditorGUILayout.ColorField("Color value", colorValue);
        gradientValue = EditorGUILayout.GradientField("Gridient Value", gradientValue);
        rectValue = EditorGUILayout.RectField("rect value", rectValue);
        vector3Value = EditorGUILayout.Vector3Field("Vector3 value", vector3Value);
        vectorInt3Value = EditorGUILayout.Vector3IntField("vector3Int value", vectorInt3Value);
        objectValue = EditorGUILayout.ObjectField("object value", objectValue,typeof(UnityEngine.Object),false);
        //consumeData = EditorGUILayout.ObjectField("consumData", consumeData, typeof(ConsumableItemData), false);
        passwordValue = EditorGUILayout.PasswordField("password Value", passwordValue);
        tagValue = EditorGUILayout.TagField("tag Value", tagValue);
        EditorGUILayout.Space(30);
        enumValue = (UnityEngine.ParticleSystemCollisionType)EditorGUILayout.EnumFlagsField("Enum value", enumValue);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        
        
        // ToolBar
        selectionValue = GUILayout.Toolbar(selectionValue, stringArr);
        
        EditorGUILayout.EndScrollView();
    }
}
