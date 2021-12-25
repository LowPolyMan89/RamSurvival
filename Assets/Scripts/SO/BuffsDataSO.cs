
using UnityEngine;
[CreateAssetMenu(fileName = "BuffsDataSO", menuName = "Data/Buffs/Create Buf Data", order = 4)]
public class BuffsDataSO : ScriptableObject
{
    public FunctionType FunctionType;
    public BuffType BuffType;
    public BuffModificator BuffModificator;
    public string BuffId;
    public float BuffTime;
    public float Value;
}
