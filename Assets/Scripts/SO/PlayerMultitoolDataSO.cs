using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultitoolData", menuName = "Data/MultitoolData", order = 4)]
public class PlayerMultitoolDataSO : ScriptableObject
{
    public float MiningRange;
    public float MiningDPS;
    public string NameId;
    public string info;
}
