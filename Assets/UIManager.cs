using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject PlayGameMenu;
    public GameObject WinGameMenu;
    public GameObject FailedGameMenu;
    public static UIManager Instance;
    public Text LimitTxt;
    public List<GameObject> stars;
    // Start is called before the first frame update
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
    void Start()
    {

    }

    public void ShowOrHideMenu(GameObject Menu)
    {
        Menu.SetActive(!Menu.activeSelf);
    }
    public void GoToMap()
    {
        LocalGameManager.Instance.Reset();
    }
    public void PlayGame()
    {
        ShowOrHideMenu(PlayGameMenu);
        PrepareGame();
        if (LocalGameManager.Instance.limitType == LocalGameManager.LimitType.time)
        {
            LocalGameManager.Instance.clock = false;
        }
    }

    private void PrepareGame()
    {
        LocalGameManager.Instance.init = false;
        LocalGameManager.Instance.paused = false;
        CardManager.Instance.IntializeCards();


    }

    public void NextLevel()
    {
        ShowOrHideMenu(WinGameMenu);
        OpenLevel.Instance.curruntLevel++;
        LocalGameManager.Instance.LoadLevel();
        ShowOrHideMenu(PlayGameMenu);
    }
    public void TryAgain(GameObject Menu)
    {
        ShowOrHideMenu(Menu);
        LocalGameManager.Instance.LoadLevel();
        ShowOrHideMenu(PlayGameMenu);
        // GameManager.Instance.clock = false;
    }
    public void Pause()
    {
        LocalGameManager.Instance.paused = true;
    }
    public void Resume()
    {
        LocalGameManager.Instance.clock = false;
        LocalGameManager.Instance.paused = false;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
