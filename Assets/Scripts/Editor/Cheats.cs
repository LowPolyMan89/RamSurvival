
    using UnityEditor;
    using UnityEngine;

    public class Cheats : Editor
    {
        [MenuItem("Cheats/Player/Add 100 XP")]
        public static void AddPlayerExp()
        {
            EventManager.Instance.OnPlayerGetXP(100);
        }
        
        [MenuItem("Cheats/Player/Add 100 HP")]
        public static void AddPlayerHP()
        {
            EventManager.Instance.OnPlayerGetHP(100);
        }
        
        [MenuItem("Cheats/Player/Remove 100 HP")]
        public static void RemovePlayerHP()
        {
            EventManager.Instance.OnPlayerGetHP(-100);
        }
    }
