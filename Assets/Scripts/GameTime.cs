using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTime : MonoBehaviour
{
    public static GameTime Instance;
    public float TotalSessionTime = 0f;
    public string TotalSessionTimeString;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
        StartCoroutine(SecondRoutine());
    }

    private IEnumerator SecondRoutine()
    {
        yield return  new WaitForSeconds(1f);
        TotalSessionTime += 1;
        TotalSessionTimeString = Support.ConvertTimeSecondsToString(TotalSessionTime);
        EventManager.Instance.OnTimerSecond();
        StartCoroutine(SecondRoutine());
    }
}
