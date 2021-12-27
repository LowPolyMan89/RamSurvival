
using System;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "BuffsDataSO", menuName = "Data/Buffs/Create Buf Data", order = 4)]
public class BuffsDataSO : ScriptableObject
{
    public FunctionType FunctionType;
    public BuffType BuffType;
    public BuffModificator BuffModificator;
    public ValueChangeType ValueChangeType;
    public ValueType ValueType;
    public StackType StackType;
    public bool IsStack;
    public bool IsHide;
    public string BuffId;
    public float BuffTime;
    public float Value;
    public Sprite BuffSprite;

    public void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BuffsDataSO))]
public class BuffsDataSOEditor : Editor
{
    private BuffsDataSO myScript;

    public override void OnInspectorGUI()
    {
        myScript = (BuffsDataSO)target;

        GUILayout.Label("Настройки баффа");
        GUILayout.Space(10f);
        myScript.BuffId = EditorGUILayout.TextField("Buff Id: ", myScript.BuffId);
        GUILayout.Space(10f);
        myScript.IsHide = EditorGUILayout.Toggle("Скрытый: ", myScript.IsHide);
        GUILayout.Space(10f);
        GUILayout.Label("Спрайт баффа");
        myScript.BuffSprite = (Sprite)EditorGUILayout.ObjectField(myScript.BuffSprite, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        GUILayout.Space(10f);
        myScript.BuffType = (BuffType)EditorGUILayout.EnumPopup("Перк или бафф?", myScript.BuffType);
        GUILayout.Space(10f);
        myScript.BuffModificator = (BuffModificator)EditorGUILayout.EnumPopup("На что влияет?", myScript.BuffModificator);
        GUILayout.Space(10f);
        myScript.ValueType = (ValueType)EditorGUILayout.EnumPopup("На какое значение?", myScript.ValueType);
        GUILayout.Space(10f);
        myScript.FunctionType = (FunctionType)EditorGUILayout.EnumPopup("Как влияет?", myScript.FunctionType);
        GUILayout.Space(10f);
        myScript.ValueChangeType = (ValueChangeType)EditorGUILayout.EnumPopup("Период влияния", myScript.ValueChangeType);
        GUILayout.Space(10f);
        myScript.Value = EditorGUILayout.FloatField("Значение: ", myScript.Value);
        
        if (myScript.BuffType == BuffType.Buff)
        {
            GUILayout.Space(10f);
            myScript.BuffTime = EditorGUILayout.FloatField("Время: ", myScript.BuffTime);
        }
        else
        {
            myScript.BuffTime = 0;
        }
        GUILayout.Space(10f);
        GUILayout.Label("Параметры накладывания одинаковых баффов");
        myScript.IsStack = EditorGUILayout.Toggle("Могут стакаться?: ", myScript.IsStack);
        GUILayout.Space(10f);
        if (myScript.IsStack)
        {
            GUILayout.Space(10f);
            myScript.StackType = (StackType)EditorGUILayout.EnumPopup("Как стакаются?", myScript.StackType);
        }
        if(GUILayout.Button("Save"))
        {
            myScript.Save();
        }
    }
}
#endif
