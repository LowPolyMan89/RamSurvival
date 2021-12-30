using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Data/Data/ItemDataSO", order = 4)]
public class ItemDataSO : ScriptableObject
{
    public string ItemId;
    public string DescriptionId;
    public ItemType ItemType;
    public EquipType equipType;
    public WeaponType WeponType;
    public int ItemRare;
    public int Count;
    public Sprite Sprite;
    public List<Item.ItemStats> ItemStats = new List<Item.ItemStats>();
    public GameObject Prefab;
    [Header("Если редмет имеет прочность")]
    public bool IsDurabilityItem;
    public float ItemDurability;
    public float MaxItemDurability;
    [Header("Если редмет можно использовать")]
    public bool IsUsableItem;
    [Header("Собирается ли в кучки")]
    public bool IsStack = true;

    public List<BuffsDataSO> ItemsBuffs = new List<BuffsDataSO>();

    public float GetStat(string statName)
    {
        float value = 0;
        foreach(var s in ItemStats)
        {
            if(s.StatName == statName)
            {
                value = s.StatValue;
                return value;
            }
        }

        return value;
    }
}
