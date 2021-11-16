using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiComponentsSwitcher : MonoBehaviour
{
    [SerializeField] private List<Image> swichObject = new List<Image>();
    
    private void OnEnable()
    {
        swichObject.ForEach((o => o.enabled = !o.enabled));
    }

    private void OnDisable()
    {
        swichObject.ForEach((o => o.enabled = !o.enabled));
    }
}
