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
    public Transform CraftPanel;
    public CrafterRequiredPanel crafterRequiredPanel;
    public Transform crafterBlueprintPanel;
    public CraftInfoPanel craftInfoPanel;
    public CraftBlueprintUi CraftBlueprintUi;
    public List<CraftBlueprintUi> CraftBlueprintUis = new List<CraftBlueprintUi>();
    public List<CraftSlotUI> CraftSlotsUi = new List<CraftSlotUI>();
    public Crafter CurrentCrafter;
    public Inventory CurrentInventory;
    private CraftBlueprintUi currentBlueprint;
    private BlueprintItemsCollection blueprintItemsCollection;
    
    private void Start()
    {
        StartCraftButton.onClick.AddListener(StartCraftButtonClick);
    }

    private void OnEnable()
    {
        EventManager.Instance.OnTimerSecondAction += OneSecondTick;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnTimerSecondAction -= OneSecondTick;
    }

    private void Update()
    {
        if (CurrentCrafter != null)
        {
            if (CurrentCrafter._craftController.CraftProcesses.Count >= CurrentCrafter.Data.Levels[0].Slots)
            {
                StartCraftButton.interactable = false;
            }
        }
    }

    private void OneSecondTick()
    {
        if (CurrentCrafter._craftController.CraftProcesses.Count > 0)
        {
            for (int i = 0; i < CurrentCrafter._craftController.CraftProcesses.Count; i++)
            {
                CraftProcess proc = CurrentCrafter._craftController.CraftProcesses.ToArray()[i];
                CraftSlotsUi[i].Item = proc.OutputItem;
                CraftSlotsUi[i].ItemImage.sprite = DatabaseManager.GetItemData(proc.OutputItem).Sprite;
                CraftSlotsUi[i].TimerText.text = proc.CraftTimeMax - proc.CurrentTime <= 0 ? "READY" : Support.ConvertTimeSecondsToString(proc.CraftTimeMax - proc.CurrentTime);
                CraftSlotsUi[i].IsActive = true;
            }
            
            var p = CurrentCrafter._craftController.CraftProcesses[0];
            CraftSlotsUi[0].processCrafter = CurrentCrafter;
            float time = p.CraftTimeMax - p.CurrentTime;
            
            if (time > 0)
            {
                CraftSlotsUi[0].TimerText.text = Support.ConvertTimeSecondsToString(time);
            }
            else
            {
                CraftSlotsUi[0].TimerText.text ="";
            }
            
            CraftSlotsUi[0].Time = time;
  
        }
    }
    
    
    public void Refresh()
    {
        BlueprintSelect(currentBlueprint, 1);
    }
    
    public void Open(CraftSheme sheme, Inventory inventory, Crafter crafter)
    {
        CraftBlueprintUis.Clear();
        CurrentCrafter = crafter;
        CurrentInventory = inventory;
        for (int i = 0; i < crafterBlueprintPanel.childCount; i++)
        {
            Destroy(crafterBlueprintPanel.GetChild(i).gameObject, 0.1f);
        }
        CreateBlueprints(sheme);
        BlueprintSelect(CraftBlueprintUis[0], 1);
        CreateSlots();
        crafter.OpenCraft(sheme, inventory, crafter);
        OneSecondTick();
    }
    
    
    private void CreateSlots()
    {
        foreach (var slot in CraftSlotsUi)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < CurrentCrafter.Data.Levels[CurrentCrafter.Level - 1].Slots; i++)
        {
            CraftSlotsUi[i].gameObject.SetActive(true);
            CraftSlotUI slo = CraftSlotsUi[i];
            slo.Init(DatabaseManager.OtherData.emptySprite, null);
            slo.TimerText.text = "";
        }
    }
    
    private void StartCraftButtonClick()
    {
        CurrentCrafter.StartCraft(blueprintItemsCollection);
        OneSecondTick();
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

        blueprintItemsCollection.OutputItemId = craftBlueprintUi.currentBlueprint.BlueprintId;
        blueprintItemsCollection.OutputItemValue = craftitemcount;
        blueprintItemsCollection.Energy = energycost;
        blueprintItemsCollection.Time = time;
    }
    
    public class BlueprintItemsCollection
    {
        public float Time;
        public float Energy;
        public List<ItemsToCraft> Items = new List<ItemsToCraft>();
        public string OutputItemId;
        public int OutputItemValue;
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


    public void GetButtonClick()
    {
        Open(CurrentCrafter.Sheme, CurrentInventory, CurrentCrafter);
    }
}
