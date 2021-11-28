using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Initilize();
    }

    private void Initilize()
    {
        DatabaseManager.LoadItemsDatabase();
    }
}
