using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInfoPanel : MonoBehaviour
{
    [SerializeField] private Image itemimage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;

    private DatabaseManager databaseManager;

    private void Start()
    {
        databaseManager = DatabaseManager.Instance;
    }

    public void Init(ItemDataSO itemdata)
    {
        itemimage.sprite = itemdata.Sprite;
        itemNameText.text = DatabaseManager.Instance.Localization.GetLocalization(itemdata.ItemId);
        itemDescriptionText.text = DatabaseManager.Instance.Localization.GetLocalization(itemdata.DescriptionId);
    }
}
