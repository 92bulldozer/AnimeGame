using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("itemData", "loadItemData", "ItemDatas", "loadItemDatas")]
	public class ES3UserType_TestLog : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_TestLog() : base(typeof(TestLog)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (TestLog)obj;
			
			writer.WritePropertyByRef("itemData", instance.itemData);
			writer.WritePropertyByRef("loadItemData", instance.loadItemData);
			writer.WriteProperty("ItemDatas", instance.ItemDatas, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<ItemData>)));
			writer.WriteProperty("loadItemDatas", instance.loadItemDatas, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<ItemData>)));
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (TestLog)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "itemData":
						instance.itemData = reader.Read<ItemData>();
						break;
					case "loadItemData":
						instance.loadItemData = reader.Read<ItemData>();
						break;
					case "ItemDatas":
						instance.ItemDatas = reader.Read<System.Collections.Generic.List<ItemData>>();
						break;
					case "loadItemDatas":
						instance.loadItemDatas = reader.Read<System.Collections.Generic.List<ItemData>>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_TestLogArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_TestLogArray() : base(typeof(TestLog[]), ES3UserType_TestLog.Instance)
		{
			Instance = this;
		}
	}
}