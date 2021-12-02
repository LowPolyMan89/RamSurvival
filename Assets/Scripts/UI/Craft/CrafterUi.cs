using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CrafterUi : MonoBehaviour
{
    public Button StartCraftButton;
    public CrafterRequiredPanel crafterRequiredPanel;
    public Transform crafterBlueprintPanel;
    public CraftInfoPanel craftInfoPanel;
    public CraftBlueprintUi CraftBlueprintUi;
    public List<CraftBlueprintUi> CraftBlueprintUis = new List<CraftBlueprintUi>();
    private void Start()
    {
        StartCraftButton.onClick.AddListener(StartCraftButtonClick);
    }

    public void Open(CraftSheme sheme)
    {
        CraftBlueprintUis.Clear();
        for (int i = 0; i < crafterBlueprintPanel.childCount; i++)
        {
            Destroy(crafterBlueprintPanel.GetChild(i).gameObject, 0.1f);
        }
        CreateBlueprints(sheme);
        BlueprintSelect(CraftBlueprintUis[0]);
    }
    
    private void StartCraftButtonClick()
    {
        
    }

    private void SelectCraftBlueprint()
    {
        
    }

    public void CreateBlueprints(CraftSheme sheme)
    {
        for (int i = 0; i < sheme.Blueprints.Count; i++)
        {
            CraftBlueprintUi element = Instantiate(CraftBlueprintUi, crafterBlueprintPanel, false);
            element.transform.localPosition = Vector3.zero;
            element.currentBlueprint = sheme.Blueprints[i];
            element.Create(sheme.Blueprints[i]);
            CraftBlueprintUis.Add(element);
        }
    }

    [System.Serializable]
    public class CraftInfoPanel
    {
        public TMP_Text ItemNameText;
        public Transform Panel;
        public Image ItemImage;
        public TMP_Text DescriptionText;
    }
    
    [System.Serializable]
    public class CrafterRequiredPanel
    {
        public Transform Panel;
        public List<CraftReqSlot> Slots = new List<CraftReqSlot>();
        public CraftReqSlot TimerSlot;
        public CraftReqSlot EnergySlot;
        public GameObject SlotPrefab;
        
        public void AddSlot(Sprite item, int needcount, int currentcount)
        {
            CraftReqSlot slot = Instantiate(SlotPrefab, Panel).GetComponent<CraftReqSlot>();
            slot.transform.localPosition = Vector3.zero;
            slot.Image.sprite = item;
            slot.Text.text = needcount.ToString() + "/" + currentcount.ToString();
            Slots.Add(slot);
        }
        
    }

    public void BlueprintSelect(CraftBlueprintUi craftBlueprintUi)
    {

        foreach (var slot in crafterRequiredPanel.Slots)
        {
            Destroy(slot.gameObject, 0.01f);
        }
        
        crafterRequiredPanel.Slots.Clear();
        
        print("Select Blueprint" + craftBlueprintUi.currentBlueprint.BlueprintId.ToLower());
        craftInfoPanel.ItemNameText.text = craftBlueprintUi.currentBlueprint.BlueprintId.ToLower();
        craftInfoPanel.DescriptionText.text = DatabaseManager.GetItemData(craftBlueprintUi.currentBlueprint.OutputItem.ItemId).DescriptionId;
        craftInfoPanel.ItemImage.sprite = DatabaseManager.GetItemData(craftBlueprintUi.currentBlueprint.OutputItem.ItemId).Sprite;

        crafterRequiredPanel.EnergySlot.Text.text = craftBlueprintUi.currentBlueprint.EnergyCost.ToString() + "/" + Player.Instance.PlayerStats.Energy;
        crafterRequiredPanel.TimerSlot.Text.text = Support.ConvertTimeSecondsToString(craftBlueprintUi.currentBlueprint.CraftTimeInSeconds);

        foreach (var reqitem in craftBlueprintUi.currentBlueprint.RequiredItems)
        {
            var item = DatabaseManager.GetItemData(reqitem.ItemId);
            crafterRequiredPanel.AddSlot(item.Sprite, reqitem.ItemValue, Player.Instance.PlayerInventory.GetContainsItemCount(reqitem.ItemId));
        }
    }
}
