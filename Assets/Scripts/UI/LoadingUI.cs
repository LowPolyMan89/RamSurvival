using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private TMP_Text LoadingText;
    
    public void Start()
    {
        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        yield return new WaitForSeconds(0.5f);
        LoadingText.text = string.Concat(LoadingText.text, ">");
    }

    private void OnDisable()
    {
        StopCoroutine(Loading());
    }
}
