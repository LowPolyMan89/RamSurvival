using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CraftBlueprint", menuName = "Data/Craft/CraftBlueprint", order = 4)]
public class CraftBlueprint : ScriptableObject
{
    public string BlueprintId;
    public List<Constrains> Constrainses = new List<Constrains>();
    public List<BlueprintItem> RequiredItems = new List<BlueprintItem>();
    public BlueprintItem OutputItem;
    public float CraftTimeInSeconds;
    public float EnergyCost;
}


[System.Serializable]
public class Constrains
{
    public ConstrainType Type;
    public int IntValue;
    public float FloatValue;
    public string StringValue;
}

[System.Serializable]
public class BlueprintItem
{
    public string ItemId;
    public int ItemValue;
}

public enum ConstrainType
{
    Level, Quest, None
}
