using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTrigger : Entity
{
   [SerializeField] private Building building;
   [SerializeField] private BuildingUpgradeSO upgradeToNextLevel;
   public Crafter Crafter;

   public Building Building
   {
      get => building;
      set => building = value;
   }
}
