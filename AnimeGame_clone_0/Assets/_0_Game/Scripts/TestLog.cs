using System;
using System.Collections.Generic;
using EJ;
using MarkupAttributes;
using UnityEngine;

[Serializable]
public class Testcl
{
    public int itemID;
}

public class TestLog : AnimeBehaviour
{
     public ItemData itemData;
     public ItemData loadItemData;
     public List<ItemData> ItemDatas;
     public List<ItemData> loadItemDatas;


     
    
     [TabScope("Tab Scope", "UI|UI_Map|UI_Equipment|UI_Stash", box: true,10)]
     // ./ shortcut opens a group on top of the current one,
     // ../ closes the topmost group and then opens a new one on top.
     [Tab("./UI")]
     public int six;
     public int seven;
     [Tab("../UI_Map")]
     public int eight;
     public int nine;
     [Tab("../UI_Equipment")]
     public int ten;
     public int eleven;
     [Tab("../UI_Stash")]
     public int ten2;
     public int eleven2;

     
     [Box("FirstGroup")]
     public int one;
     // To nest groups, just write their path.
     //[TitleGroup("First Group/Nested Group 1")]
     public int two;
     public int three;
     // Starting a group closes all groups untill path match.
     //[TitleGroup("First Group/Nested Group 2")]
     public int four;
     public int five;
   

    public void Log()
    {
        "TestLog".Log();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            TestSave();
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            TestLoad();
        }
    }


    private void Start()
    {
       
        //ES3.Save("myItemData",te);
        //ES3.Save("myItemDataList",Testcls.ToArray());
    }

    [ContextMenu("AutoInit/Save")]
    public void TestSave()
    {
        //ES3.Save("myItemData",itemData);
        //ES3.Save("myItemDataList",ItemDatas);
        //ES3.Save("myItemDataList",cuslist);
    }
    
    [ContextMenu("AutoInit/Load")]
    public void TestLoad()
    {
        //loadItemData = ES3.Load<ItemData>("myItemData");
        //loadItemDatas = ES3.Load<List<ItemData>>("myItemDataList");
        //Testcls = ES3.Load("myItemDataList",Testcls);
        //cuslist=ES3.Load<List<int>>("myItemDataList");
    }
}
