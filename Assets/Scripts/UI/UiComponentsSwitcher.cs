using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiComponentsSwitcher : MonoBehaviour
{
    [SerializeField] private List<Image> swichObject = new List<Image>();
    [SerializeField] private List<GameObject> swichObjectGameObjects = new List<GameObject>();
    private void OnEnable()
    {
        swichObject.ForEach((o => o.enabled = !o.enabled));
        foreach (var obj in swichObjectGameObjects)
        {
            obj.SetActive(false);
        }
    }

    private void OnDisable()
    {
        swichObject.ForEach((o => o.enabled = !o.enabled));
        foreach (var obj in swichObjectGameObjects)
        {
            obj.SetActive(true);
        }
    }
}
