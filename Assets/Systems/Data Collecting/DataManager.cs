using System;
using UnityEngine;

public class DataManager
{
    #region Player Prefs Operations

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void SaveData(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static void SaveData(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public static void SaveData(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public static T LoadData<T>(string key)
    {
        if (typeof(T) == typeof(int))
        {
            int intValue = PlayerPrefs.GetInt(key);
            return (T)Convert.ChangeType(intValue, typeof(T));
        }
        else if (typeof(T) == typeof(float))
        {
            float floatValue = PlayerPrefs.GetFloat(key);
            return (T)Convert.ChangeType(floatValue, typeof(T));
        }
        else if (typeof(T) == typeof(string))
        {
            string stringValue = PlayerPrefs.GetString(key);
            return (T)Convert.ChangeType(stringValue, typeof(T));
        }
        else
        {
            throw new NotSupportedException("Unsupported data type");
        }
    }

    #endregion
}
