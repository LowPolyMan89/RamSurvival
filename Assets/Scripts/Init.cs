using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(Initilize());
    }

    private IEnumerator Initilize()
    {
        yield return new WaitForSeconds(1f);
        DatabaseManager.Instance.LoadItemsDatabase();
        yield return new WaitForSeconds(1f);
        gameObject.AddComponent<GameTime>();
        UIController.Instance.LoadingComplite();
    }
}
