using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemView
{  
   public string ItemId;
   public int Count;
   
   public ItemView(string itemId, int count)
   {
      ItemId = itemId;
      Count = count;
   }
   
}
