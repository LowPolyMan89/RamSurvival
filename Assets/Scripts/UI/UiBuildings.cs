
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


    public class UiBuildings : MonoBehaviour
    {
        public Button StartCraftButton;
        public Transform CraftPanel;
        public CrafterRequiredPanel crafterRequiredPanel;
        public CraftInfoPanel craftInfoPanel;
        public Crafter CurrentCrafter;
        public Inventory CurrentInventory;
        private CraftBlueprintUi currentBlueprint;
        private BlueprintItemsCollection blueprintItemsCollection;
        [SerializeField] private GameObject processPanel;
        [SerializeField] private Image processImage;
        [SerializeField] private Building currentBuilding;
        
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
                CraftProcess proc = CurrentCrafter._craftController.CraftProcesses[0];
                processImage.fillAmount = proc.CurrentTime / proc.CraftTimeMax;
            }
        }


        public void Refresh()
        {
          
        }

        public void Open(CraftSheme sheme, Inventory inventory, Crafter crafter, Building building)
        {
            CurrentCrafter = crafter;
            CurrentInventory = inventory;
            currentBuilding = building;
            
            BlueprintSelect();
            crafter.OpenCraft(sheme, inventory, crafter);
            if (CurrentCrafter._craftController.CraftProcesses.Count > 0)
            {
                processPanel.SetActive(true);
            }
            else
            {
                processPanel.SetActive(false);
            }
            OneSecondTick();
        }
        

        private void StartCraftButtonClick()
        {
            processPanel.SetActive(true);
            CurrentCrafter._craftController.StartBuild(CurrentCrafter.Sheme.Blueprints[0], currentBuilding);
            OneSecondTick();
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

        public void BlueprintSelect()
        {
            StartCraftButton.interactable = true;
            blueprintItemsCollection = new BlueprintItemsCollection();

            foreach (var slot in crafterRequiredPanel.Slots)
            {
                Destroy(slot.gameObject, 0.01f);
            }

            crafterRequiredPanel.Slots.Clear();
            
            float energycost = CurrentCrafter.Sheme.Blueprints[0].EnergyCost;
            float playerenergy = Player.Instance.PlayerStats.CurrentEnergy;
            float time = CurrentCrafter.Sheme.Blueprints[0].CraftTimeInSeconds;
            int exp = CurrentCrafter.Sheme.Blueprints[0].Exp;
            print("Select Blueprint" + CurrentCrafter.Sheme.Blueprints[0].BlueprintId.ToLower());
            craftInfoPanel.ItemNameText.text =
                DatabaseManager.Instance.Localization.GetLocalization(CurrentCrafter.Sheme.Blueprints[0].BlueprintId);
            
            ItemDataSO itemdata = DatabaseManager
                .Instance.GetItemData(CurrentCrafter.Sheme.Blueprints[0].OutputItem.ItemId);

            if (itemdata)
            {
                craftInfoPanel.DescriptionText.text = DatabaseManager.Instance.Localization.GetLocalization(itemdata.DescriptionId);
                craftInfoPanel.ItemImage.sprite = itemdata.Sprite;
            }
            
            crafterRequiredPanel.EnergySlot.Text.text = energycost.ToString("0") + "/" +
                                                        Player.Instance.PlayerStats.CurrentEnergy.ToString("0");
            crafterRequiredPanel.TimerSlot.Text.text = Support.ConvertTimeSecondsToString(time);

            if (energycost > playerenergy)
            {
                StartCraftButton.interactable = false;
            }

            foreach (var reqitem in CurrentCrafter.Sheme.Blueprints[0].RequiredItems)
            {
                var item = DatabaseManager.Instance.GetItemData(reqitem.ItemId);
                int itemcount = reqitem.ItemValue;
                blueprintItemsCollection.Items.Add(
                    new BlueprintItemsCollection.ItemsToCraft(Player.Instance.PlayerInventory.GetItem(reqitem.ItemId),
                        itemcount));
                crafterRequiredPanel.AddSlot(item.Sprite, itemcount,
                    Player.Instance.PlayerInventory.GetContainsItemCount(reqitem.ItemId));

                if (itemcount > Player.Instance.PlayerInventory.GetContainsItemCount(reqitem.ItemId))
                {
                    StartCraftButton.interactable = false;
                }
            }

            blueprintItemsCollection.OutputItemId = CurrentCrafter.Sheme.Blueprints[0].BlueprintId;
            blueprintItemsCollection.Energy = energycost;
            blueprintItemsCollection.Time = time;
            blueprintItemsCollection.exp = exp;
        }

        public class BlueprintItemsCollection
        {
            public float Time;
            public float Energy;
            public List<ItemsToCraft> Items = new List<ItemsToCraft>();
            public string OutputItemId;
            public int OutputItemValue;
            public int exp;

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
    
