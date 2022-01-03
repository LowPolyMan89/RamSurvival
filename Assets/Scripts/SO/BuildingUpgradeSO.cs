  using System.Collections.Generic;
  using UnityEngine;

  [CreateAssetMenu(fileName = "BuildingUpgradeSO", menuName = "Data/Buildings/BuildingUpgradeSO", order = 4)]
  public class BuildingUpgradeSO : ScriptableObject
  {
      public string BlueprintId;
      public List<Constrains> Constrainses = new List<Constrains>();
      public List<BlueprintItem> RequiredItems = new List<BlueprintItem>();
      public float CraftTimeInSeconds;
      public float EnergyCost;
      public int Exp;
  }
  
