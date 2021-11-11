using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData", order = 1)]
public class ItemDataSO : ScriptableObject
{
    public ItemType itemType;
    public EquipType equipType;
    public string ItemID;
    public string Info;
    public int Tier;
    public int MaxStack;
    public float Capacity;
    public Sprite ItemSprite;
    public GameObject Prefab;
}
