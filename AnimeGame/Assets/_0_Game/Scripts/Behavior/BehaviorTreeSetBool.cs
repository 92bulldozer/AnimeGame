using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class BehaviorTreeSetBool : MonoBehaviour
{
    public bool testCanFind { get; set; }
    
    public void BoolToggle()
    {
        if (testCanFind)
        {
            testCanFind = false;
        }
        else
        {
            testCanFind = true;
        }
    }
    
    
}
