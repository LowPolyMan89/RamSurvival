using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LocalizationCreator : MonoBehaviour
{
    [MenuItem("Tools/Localization/Create Draft Json")]
    public static void CreateDraftLocalizationJson()
    {
        LocalizationData Localization = new LocalizationData();
        LocalizationData.LocalizationObject object1 = new LocalizationData.LocalizationObject();
        object1.LocalizationObjectID = "item_name_id";
        object1.Local.Add("Русская локализация");
        object1.Local.Add("English localization");
        Localization.Locals.Add(object1);
        string json = JsonUtility.ToJson(Localization);
        
        //Get the path of the Game data folder
        var m_Path = Application.dataPath;

        //Output the Game data path to the console
        Debug.Log("dataPath : " + m_Path);

        if (File.Exists(m_Path + "/Localization.json"))
        {
            File.WriteAllText(m_Path +"/Localization.json", json);
        }
        else
        {
            File.Create(m_Path + "/Localization.json");
            File.WriteAllText(m_Path +"/Localization.json", json);
        }
    }
    
    [MenuItem("Tools/Localization/Test Localization Data")]
    public static void TestLocalizationJson()
    {

        LocalizationData localization;
        
        //Get the path of the Game data folder
        var m_Path = Application.dataPath;
        
        if (File.Exists(m_Path + "/Localization.json"))
        {
           localization = JsonUtility.FromJson<LocalizationData>( File.ReadAllText(m_Path +"/Localization.json"));
        }
        else
        {
            Debug.LogError("No Localization File!");
            return;
        }
        
        var loc = new Dictionary<string, List<string>>();

        foreach (var l in localization.Locals)
        {
            loc.Add(l.LocalizationObjectID, l.Local);
        }

        foreach (var v in loc)
        {
            print($@"{v.Key} contains:");
            foreach (var s in v.Value)
            {
                print($@"{s}");
            }
        }

    }
}

[System.Serializable]
public class LocalizationData
{
    public List<LocalizationObject> Locals = new List<LocalizationObject>();
    
    [System.Serializable]
    public class LocalizationObject
    {
        public string LocalizationObjectID;
        public List<string> Local = new List<string>();
    }
}
