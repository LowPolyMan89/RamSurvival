using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CraftSheme", menuName = "Data/Craft/CraftSheme", order = 4)]

public class CraftSheme : ScriptableObject
{
    public List<CraftBlueprint> Blueprints = new List<CraftBlueprint>();

    public CraftBlueprint GetBlueprint(string itemid)
    {
        foreach (var b in Blueprints)
        {
            if (b.OutputItem.ItemId == itemid)
                return b;
        }

        return null;
    }
}
