using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackpackData", menuName = "Data/BackpackData", order = 3)]
public class PlayerBackpackDataSO : ScriptableObject
{
    public int MaxSlots;
    public int MaxCapacity;
    public int Rare;
    public string NameId;
    public string info;

    public Sprite ItemSprite;
}
