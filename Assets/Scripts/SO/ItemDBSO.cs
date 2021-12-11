using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Data/Data/ItemDatabase", order = 4)]
public class ItemDBSO : ScriptableObject
{
    public List<ItemDataSO> ItemsData = new List<ItemDataSO>();

    public void RefreshDatabase()
    {
        ItemsData.Clear();
        
        var guids = AssetDatabase.FindAssets("t:ItemDataSO");
        foreach (string guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            ItemsData.Add(AssetDatabase.LoadAssetAtPath<ItemDataSO>(path));
            Debug.Log("Add item to data base: " + path);
        }
    }
}

[CustomEditor(typeof(ItemDBSO))]
public class ItemDbsoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemDBSO myScript = (ItemDBSO)target;
        if(GUILayout.Button("Refresh Database"))
        {
            myScript.RefreshDatabase();
        }
    }
}
