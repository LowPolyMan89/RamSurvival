using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlotUI : MonoBehaviour
{
    public TMP_Text TimerText;
    public Image ItemImage;
    public Button GetButton;
    public string Item;
    public bool IsActive = false;
    public bool IsComplite = false;
    public float Time;
    public Crafter processCrafter;

    private void Start()
    {
        GetButton.onClick.AddListener(GetButtonClick);
    }

    public void Init(Sprite itemSprite, string item)
    {
        ItemImage.sprite = itemSprite;
        Item = item;
    }

    public void GetButtonClick()
    {
        if (processCrafter != null)
        {
            ItemImage.sprite = DatabaseManager.OtherData.emptySprite;
            GetButton.interactable = false;
            IsActive = false;
            IsComplite = false;
            processCrafter._craftController.CollectItem();
        }
    }
    
    private void Update()
    {
        if (IsActive && Time <= 0.5)
        {
            GetButton.interactable = true;
        }
        else
        {
            GetButton.interactable = false;
        }
    }
}
