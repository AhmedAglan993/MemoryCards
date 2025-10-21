using UnityEngine;
using System.Collections;

public class PlayerPrefs2
{
    public static void SetBool(string key, bool state)
    {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }
    public static bool GetBool(string key)
    {
        int value = PlayerPrefs.GetInt(key);

        if (value == 1)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    public static bool HasBool(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void DeleteBoolKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    public static void DeleteAllKeys()
    {
        PlayerPrefs.DeleteAll();
    }
}
