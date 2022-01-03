using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private string building_ID;
    [SerializeField] private int current_lvl;
    [SerializeField] private List<GameObject> buildings_levels = new List<GameObject>();

    private void Start()
    {
        SelectBuilding();
    }

    private void SelectBuilding()
    {
        foreach (var buildingsLevel in buildings_levels)
        {
            buildingsLevel.SetActive(false);
        }
        
        buildings_levels[current_lvl].SetActive(true);
    }

    public void Upgrade()
    {
        current_lvl++;
        SelectBuilding();
    }
}
