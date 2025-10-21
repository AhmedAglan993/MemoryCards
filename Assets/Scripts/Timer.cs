using System;
using UnityEngine;

public class Timer:MonoBehaviour
{
    public static Timer Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(this);
        }
    }

    public string UpdateTimer(int fullTime)
    {
        string timeFormate = "";
        int min = Mathf.FloorToInt(fullTime / 60);
        int sec = Mathf.FloorToInt(fullTime % 60);
        timeFormate = "Time: " + min.ToString("00") + ":" + sec.ToString("00");
        UIManager.Instance.LimitTxt.text = "Time: " + min.ToString("00") + ":" + sec.ToString("00");
        return timeFormate;
    }
}