  using System.Collections.Generic;
  using UnityEngine;

  [CreateAssetMenu(fileName = "BuildingUpgradeSO", menuName = "Data/Buildings/BuildingUpgradeSO", order = 4)]
  public class BuildingUpgradeSO : ScriptableObject
  {
      public string BlueprintId;
      public List<ItemToUpgrade> ItemsToUpgrade = new List<ItemToUpgrade>();
      public float CraftTimeInSeconds;
      public float EnergyCost;
      public int Exp;
      
      [System.Serializable]
      public class ItemToUpgrade
      {
          public ItemDataSO item;
          public int ItemValue;
      }
  }
  
