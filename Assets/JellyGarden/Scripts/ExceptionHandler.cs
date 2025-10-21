using System;
using System.IO;
using UnityEngine;

public class ExceptionHandler : MonoBehaviour
{
    private string m_Writer;
    private int m_ExceptionCount = 0;
    void Awake()
    {
        Application.logMessageReceived += HandleException;
    }

    private void HandleException(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            m_ExceptionCount++;
            m_Writer = string.Format("{0}: {1}\n{2}\n{3}\n", type, condition, stackTrace, SystemInfo.deviceModel, SystemInfo.graphicsDeviceType);
            Debug.Log("Exception! ");
        }
    }

    void OnGUI()
    {
        GUILayout.Label(string.Format("Count: {0}", m_ExceptionCount));
        GUILayout.Label(string.Format("Exception: {0}", m_Writer));

        if (GUILayout.Button("Application Exception"))
        {
            throw new ApplicationException();
        }
        if (GUILayout.Button("Null Reference"))
        {
            GameObject go = null;
            Debug.Log(go.name);
        }
        if (GUILayout.Button("Float Divide By Zero"))
        {
            float x = 3.14f;
            float y = 0.0f;
            float z = x / y;
            Debug.Log(z.ToString());
        }
        if (GUILayout.Button("Integer Divide By Zero"))
        {
            int x = 42;
            int y = 0;
            int z = x / y;
            Debug.Log(z.ToString());
        }
        if (GUILayout.Button("Stack Overflow"))
        {
            OverflowStack(1, 2, 3);
        }
    }

    private int OverflowStack(int a, int b, int c)
    {
        return OverflowStack(c, b, a) + OverflowStack(b, c, a + 1);
    }
}