using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TestASEControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        Material mat = mesh.material;
        float intensity = 1;
        DOTween.To(()=> intensity, x=>
        {
            intensity = x;
            mat.SetFloat("_Intensity",intensity);
            Debug.Log(intensity);

        }, 0, 2).SetLoops(-1,LoopType.Yoyo);
    }

}
