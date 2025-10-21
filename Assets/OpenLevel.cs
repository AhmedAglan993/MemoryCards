using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLevel : MonoBehaviour
{
    public static OpenLevel Instance;
    [SerializeField]
    public GameObject Map;
    [SerializeField]
    public GameObject GameField;
    public int curruntLevel;
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
    private void Start()
    {
        // LevelsMap.LevelSelected += OnLevelClicked;
        //OnLevelClicked();
    }

    public void OnLevelClicked()
    {
        // Debug.Log(LevelsMap._instance.GetLevel());
        if (Map!=null)
        {
            Map.SetActive(false);
        }
        GameField.SetActive(true);
        LocalGameManager.Instance.LoadLevel();
    }
}
