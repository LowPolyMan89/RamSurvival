
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffsDatabase", menuName = "Data/Buffs/Create Buffs Database", order = 1)]
public class BuffsDatabase : ScriptableObject
{
    [SerializeField] private List<BuffsDataSO> allBuffs = new List<BuffsDataSO>();

    public Dictionary<string, BuffsDataSO> AllBuffs = new Dictionary<string, BuffsDataSO>();

    public bool Init()
    {
        foreach (var b in allBuffs)
        {
            AllBuffs.Add(b.BuffId, b);
        }

        return true;
    }
    
    public BuffsDataSO GetBuff(string buffId)
    {
        BuffsDataSO value;
        AllBuffs.TryGetValue(buffId, out value);
        return value;
    }
    
#if UNITY_EDITOR 
    public void RefreshDatabase()
    {
        allBuffs.Clear();
        AllBuffs.Clear();
        var guids = AssetDatabase.FindAssets("t:BuffsDataSO");
        foreach (string guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            BuffsDataSO so = AssetDatabase.LoadAssetAtPath<BuffsDataSO>(path);
            allBuffs.Add(so);
            AllBuffs.Add(so.BuffId, so);
            Debug.Log("Add item to data base: " + path);
        }
    }
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(BuffsDatabase))]
public class BuffsDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BuffsDatabase myScript = (BuffsDatabase)target;
        if(GUILayout.Button("Refresh Database"))
        {
            myScript.RefreshDatabase();
        }
    }
}
#endif
