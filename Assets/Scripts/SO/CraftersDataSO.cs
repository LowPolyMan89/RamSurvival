using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftersDataSO", menuName = "Data/Craft/CraftersDataSO", order = 4)]
public class CraftersDataSO : ScriptableObject
{
    public List<CrafterLevels> Levels = new List<CrafterLevels>();

    [System.Serializable]
    public class CrafterLevels
    {
        public int Slots;
    }
}
