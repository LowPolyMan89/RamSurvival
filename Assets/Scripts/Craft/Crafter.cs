using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour
{
    public CraftSheme Sheme;
    private CraftController _craftController;
    
    private void Start()
    {
        _craftController = new CraftController();
    }

    [ContextMenu("OpenCraft")]
    public void OpenCraft()
    {
        _craftController.OpenCraft(Sheme, Player.Instance.PlayerInventory);
    }
}
