using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CrafterUi : MonoBehaviour
{
    public Button StartCraftButton;
    public CrafterRequiredPanel crafterRequiredPanel;
    public CraftInfoPanel craftInfoPanel;
    public List<Transform> blueprintsSlots = new List<Transform>();
    public CraftBlueprintUi CraftBlueprintUi;
    private void Start()
    {
        StartCraftButton.onClick.AddListener(StartCraftButtonClick);
    }

    public void Open(CraftSheme sheme)
    {
        for (int i = 0; i < blueprintsSlots.Count; i++)
        {
            if (blueprintsSlots[i].childCount > 0)
            {
                Destroy(blueprintsSlots[i].GetChild(0).gameObject);
            }
        }
        CreateBlueprints(sheme);
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
            CraftBlueprintUi element = Instantiate(CraftBlueprintUi, blueprintsSlots[i], false);
            element.transform.localPosition = Vector3.zero;
            element.currentBlueprint = sheme.Blueprints[i];
            element.Create(sheme.Blueprints[i]);
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
        public List<CrafterRequiredPanelSlot> Slots = new List<CrafterRequiredPanelSlot>();
        public CrafterRequiredPanelSlot TimerSlot;
        
        [System.Serializable]
        public class CrafterRequiredPanelSlot
        {
            public Image ReqItemImage;
            public TMP_Text ReqItemCountText;
        }
    }
}
