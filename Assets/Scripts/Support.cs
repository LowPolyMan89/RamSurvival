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
    
    public static List<T> RemoveNull<T>(List<T> list)
    {
        List<T> newList = new List<T>();
        newList.AddRange(list);

        // Find Fist Null Element in O(n)
        var count = newList.Count;
        for (var i = 0; i < count; i++)
        {
            if (newList[i] == null)
            {
                // Current Position
                int newCount = i++;
                // Copy non-empty elements to current position in O(n)
                for (; i < count; i++)
                {
                    if (newList[i] != null)
                    {
                        newList[newCount++] = newList[i];
                    }
                }
                // Remove Extra Positions O(n)
                newList.RemoveRange(newCount, count - newCount);
                break;
            }
        }

        return newList;
    }
}


