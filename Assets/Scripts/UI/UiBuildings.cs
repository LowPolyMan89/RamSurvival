
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
        public Inventory CurrentInventory;
        public GameObject RequireUiSlotPrefab;
        [SerializeField] private GameObject processPanel;
        [SerializeField] private Image processImage;
        [SerializeField] private Building currentBuilding;

        public Building CurrentBuilding => currentBuilding;

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
           
        }

        private void OneSecondTick()
        {
           
        }


        public void Refresh()
        {
            bool check = true;
            
            if(!currentBuilding) return;
            
            foreach (Transform child in CraftPanel) 
            {
                Destroy(child.gameObject);
            }
            
            foreach (var KeyValue in currentBuilding.Inventory.Items)
            {
                CraftReqSlot slot = Instantiate(RequireUiSlotPrefab).GetComponent<CraftReqSlot>();
                slot.transform.SetParent(CraftPanel);
                slot.transform.localPosition = Vector3.zero;
                slot.transform.localScale = Vector3.one;
                slot.gameObject.name = "building_cell";
                slot.Image.sprite = DatabaseManager.Instance.GetItemData(KeyValue.Key).Sprite;
                slot.Text.text = $@"{KeyValue.Value.Current} / {KeyValue.Value.Need}";
                
                if (KeyValue.Value.Current < KeyValue.Value.Need)
                {
                    check = false;
                }
            }

            StartCraftButton.interactable = check;
        }

        public void Open(Building triggerBuilding)
        {
            currentBuilding = triggerBuilding;
            Refresh();
        }
        
        private void StartCraftButtonClick()
        {
            
        }
        
      
        
    }
    
