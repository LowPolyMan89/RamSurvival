using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Support
{
#if UNITY_EDITOR 
    [MenuItem("SupportTools/TestTime")]
    #endif
    public static void TestTime()
    {
        Debug.Log(ConvertTimeSecondsToString(3600));
    }

    public static string ConvertTimeSecondsToString(float value)
    {
        float totalSeconds = value;
        TimeSpan time = TimeSpan.FromSeconds(totalSeconds);

        string result = time.ToString("hh':'mm':'ss");
        // 00:03:48
        
        return result;
    }
}


