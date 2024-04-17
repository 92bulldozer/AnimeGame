using System;
using System.Collections;
using System.Collections.Generic;
using EJ;
using UnityEngine;
using VRM;

[Serializable]
public class BlendShapeData
{
    
}

public class TestBlendShape : MonoBehaviour
{
    public SkinnedMeshRenderer skin;
    public int blendShapeCount;
    public List<BlendShapeClip> bscList;

    private void Awake()
    {
        blendShapeCount= skin.sharedMesh.blendShapeCount;
        blendShapeCount.Log();
        for (int i = 0; i < blendShapeCount; i++)
        {
            string blendShapeName = skin.sharedMesh.GetBlendShapeName(i);
            float blendShapeWeight = skin.GetBlendShapeWeight(i);
            Debug.Log("Blend Shape Name: " + blendShapeName + ", Weight: " + blendShapeWeight);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            
        }
    }

   
}
