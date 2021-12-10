using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Inventory, IInteractive<Chest>
{
    public Chest Use(Chest value)
    {
        UIController.Instance.OpenChestUi(value);
        return this;
    }
}
