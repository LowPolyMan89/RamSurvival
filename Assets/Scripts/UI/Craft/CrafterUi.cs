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
    public Button PlusCraftButton;
    public Button MinusCraftButton;
    public CrafterRequiredPanel crafterRequiredPanel;
    public Transform crafterBlueprintPanel;
    public CraftInfoPanel craftInfoPanel;
    public CraftBlueprintUi CraftBlueprintUi;
    public List<CraftBlueprintUi> CraftBlueprintUis = new List<CraftBlueprintUi>();
    public TMP_Text countToCraftText;
    private CraftBlueprintUi currentBlueprint;
    private BlueprintItemsCollection blueprintItemsCollection;
    private int itemToCraftCount = 1;
    private void Start()
    {
        StartCraftButton.onClick.AddListener(StartCraftButtonClick);
        PlusCraftButton.onClick.AddListener(PlusButton);
        MinusCraftButton.onClick.AddListener(MinusButton);
        gameObject.SetActive(false);
    }

    private void PlusButton()
    {
        itemToCraftCount++;
        BlueprintSelect(currentBlueprint, itemToCraftCount);
    }
    
    private void MinusButton()
    {
        if(itemToCraftCount < 2) return;
        itemToCraftCount--;
        BlueprintSelect(currentBlueprint, itemToCraftCount);
    }
    
    public void Open(CraftSheme sheme)
    {
        CraftBlueprintUis.Clear();
        for (int i = 0; i < crafterBlueprintPanel.childCount; i++)
        {
            Destroy(crafterBlueprintPanel.GetChild(i).gameObject, 0.1f);
        }
        CreateBlueprints(sheme);
        BlueprintSelect(CraftBlueprintUis[0], 1);
        itemToCraftCount = 1;
        countToCraftText.text = itemToCraftCount.ToString();
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

    public void BlueprintSelect(CraftBlueprintUi craftBlueprintUi, int craftitemcount)
    {
        StartCraftButton.interactable = true;
        currentBlueprint = craftBlueprintUi;
        countToCraftText.text = craftitemcount.ToString();
        
        blueprintItemsCollection = new BlueprintItemsCollection();
        
        foreach (var slot in crafterRequiredPanel.Slots)
        {
            Destroy(slot.gameObject, 0.01f);
        }
        
        crafterRequiredPanel.Slots.Clear();

        float energycost = craftitemcount * craftBlueprintUi.currentBlueprint.EnergyCost;
        float playerenergy = Player.Instance.PlayerStats.Energy;
        float time = craftBlueprintUi.currentBlueprint.CraftTimeInSeconds * craftitemcount;

        print("Select Blueprint" + craftBlueprintUi.currentBlueprint.BlueprintId.ToLower());
        craftInfoPanel.ItemNameText.text = craftBlueprintUi.currentBlueprint.BlueprintId.ToLower();
        craftInfoPanel.DescriptionText.text = DatabaseManager.GetItemData(craftBlueprintUi.currentBlueprint.OutputItem.ItemId).DescriptionId;
        craftInfoPanel.ItemImage.sprite = DatabaseManager.GetItemData(craftBlueprintUi.currentBlueprint.OutputItem.ItemId).Sprite;

        crafterRequiredPanel.EnergySlot.Text.text = energycost.ToString() + "/" + Player.Instance.PlayerStats.Energy;
        crafterRequiredPanel.TimerSlot.Text.text = Support.ConvertTimeSecondsToString(time);

        if (energycost > playerenergy)
        {
            StartCraftButton.interactable = false;
        }
        
        foreach (var reqitem in craftBlueprintUi.currentBlueprint.RequiredItems)
        {
            var item = DatabaseManager.GetItemData(reqitem.ItemId);
            int itemcount = reqitem.ItemValue * craftitemcount;
            blueprintItemsCollection.Items.Add(new BlueprintItemsCollection.ItemsToCraft(Player.Instance.PlayerInventory.GetItem(reqitem.ItemId), itemcount));
            crafterRequiredPanel.AddSlot(item.Sprite, itemcount, Player.Instance.PlayerInventory.GetContainsItemCount(reqitem.ItemId));
            
            if (itemcount > Player.Instance.PlayerInventory.GetContainsItemCount(reqitem.ItemId))
            {
                StartCraftButton.interactable = false;
            }
        }
        
        blueprintItemsCollection.Energy = energycost;
        blueprintItemsCollection.Time = time;
    }
    
    private class BlueprintItemsCollection
    {
        public float Time;
        public float Energy;
        public List<ItemsToCraft> Items = new List<ItemsToCraft>();
        public struct ItemsToCraft
        {
            public ItemView Item;
            public int Count;
            public ItemsToCraft(ItemView item, int count)
            {
                Item = item;
                Count = count;
            }
        }
    }
}
