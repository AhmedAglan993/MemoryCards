using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManeger : MonoBehaviour
{
    public void LoadSpecificScene(string SceneName)
    {
        SceneManager.LoadScene(1);
    }
}
