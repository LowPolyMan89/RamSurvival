using System.Collections.Generic;

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