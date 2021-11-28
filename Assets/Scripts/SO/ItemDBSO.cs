using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Data/Data/ItemDatabase", order = 4)]
public class ItemDBSO : ScriptableObject
{
    public List<ItemDataSO> ItemsData = new List<ItemDataSO>();
}
