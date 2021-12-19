using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftBlueprintUi : MonoBehaviour
{
    public Image ItemImage;
    public TMP_Text ItemName;
    public Button SelectButton;
    public CraftBlueprint currentBlueprint;

    private void Start()
    {
        SelectButton.onClick.AddListener(ClickSelectButton);
    }

    public void Create(CraftBlueprint blueprint)
    {
        ItemDataSO itemdats = DatabaseManager.Instance.GetItemData(blueprint.BlueprintId);
        if(!itemdats) return;
        
        ItemImage.sprite = itemdats.Sprite;
        ItemName.text = DatabaseManager.Instance.Localization.GetLocalization(itemdats.ItemId);
        currentBlueprint = blueprint;
    }
    
    private void ClickSelectButton()
    {
        UIController.Instance.CrafterUi.BlueprintSelect(this, 1);
    }
}
