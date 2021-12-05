using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class ItemUIElement : MonoBehaviour
{
    public ItemView Item;
    public Image Image;
    public Text CountText;
    [SerializeField] private Transform startRoot;
    public bool IsEqipped { get; set; }

    public void OnEnable()
    {
        StartCoroutine(CustomUpdate());
    }

    private void OnDisable()
    {
        StopCoroutine(CustomUpdate());
    }

    private IEnumerator CustomUpdate()
    {
        yield return new WaitForSeconds(0.1f);
        Image.sprite = DatabaseManager.GetItemData(Item.ItemId).Sprite;
        CountText.text = Item.Count.ToString();
        StartCoroutine(CustomUpdate());
    }
}
