using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUI : MonoBehaviour
{
    [SerializeField] private List<EquipUISlot> equipUISlots = new List<EquipUISlot>();

    public List<EquipUISlot> EquipUISlots => equipUISlots;
}
