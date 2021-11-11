using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipUISlot : MonoBehaviour
{
    [SerializeField] private EquipType slotEquipType;
    [SerializeField] private Image image;

    public EquipType SlotEquipType { get => slotEquipType; }
    public Image Image { get => image; }
}
