using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LocalGameManager : MonoBehaviour
{
    public Sprite[] AllcardFaces;
    public List<GameObject> cards;
    public static LocalGameManager Instance;
    public bool init = true;
    public List<GameObject> GratzWords;
    public List<string> levelSquaresFile = new List<string>();

    public enum LimitType
    {
        moves,
        time
    };

    public Image Bar;
    public LimitType limitType;
    public int colorLimit;
    public int Limit;
    public bool clock;
    public int stars;
    public bool levelLoaded;

    bool gameOver;
    bool Warning;
    public int Score;


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

        GameObject.Find("Music").GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Music");
        SoundBase.Instance.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Sound");
        paused = false;
    }

    private void Start()
    {
        print("ssssss");
        init = true;
        CardManager.Instance.Clicked = false;
        clock = true;
        Warning = false;
    }

    void Update()
    {
        if (limitType == LimitType.time)
        {
            if (Limit > 0 && clock == false)
            {
                clock = true;
                StartCoroutine("Wait");
            }

            if (Limit < 10 && !Warning)
            {
                SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.timeOut);
                Warning = true;
            }
        }

        if (levelLoaded && Limit == 0 && CardManager.Instance.matches > 0)
        {
            if (!gameOver)
            {
                GameOver();
                gameOver = true;
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            if (!CardManager.Instance.Clicked)
            {
                GameRules.Instance.CheckCards();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            CardManager.Instance.Clicked = false;
        }
    }

    public void Reset()
    {
        UIManager.Instance.PlayGameMenu.SetActive(true);
        UIManager.Instance.WinGameMenu.SetActive(false);
        UIManager.Instance.FailedGameMenu.SetActive(false);
        UIManager.Instance.LimitTxt.text = "";
        OpenLevel.Instance.GameField.SetActive(false);
        OpenLevel.Instance.Map.SetActive(true);
        CardManager.Instance.cardFaces = new Sprite[0];
        cards = new List<GameObject>();
        levelSquaresFile = new List<string>();
        RandomListGenerator.Instance.UsedIndecies = new List<int>();
    }

    private void GameOver()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.gameOver[0]);
        StartCoroutine(WaitToReload(UIManager.Instance.FailedGameMenu));
        Debug.Log("Game Over");
    }

    public void WinGame()
    {
        stars = 3;
        LevelsMap.CompleteLevel(OpenLevel.Instance.curruntLevel, stars);
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.complete[0]);
        if (PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", OpenLevel.Instance.curruntLevel), 0) < stars)
        {
            PlayerPrefs.SetInt(string.Format("Level.{0:000}.StarsCount", OpenLevel.Instance.curruntLevel), stars);
        }
        PlayerPrefs.SetInt(string.Format("Level.{0:000}.Score", OpenLevel.Instance.curruntLevel), Score);
        if (Score > PlayerPrefs.GetInt("Score" + OpenLevel.Instance.curruntLevel))
        {
            PlayerPrefs.SetInt("Score" + OpenLevel.Instance.curruntLevel, Score);
        }
        StopCoroutine("Wait");
        StartCoroutine(WaitToReload(UIManager.Instance.WinGameMenu));
    }


   

    public void LoadLevel()
    {
        CardManager.Instance.cardFaces = new Sprite[0];
        cards = new List<GameObject>();
        levelSquaresFile = new List<string>();
        RandomListGenerator.Instance.UsedIndecies = new List<int>();
        LevelCreator.Instance.LoadDataFromLocal(OpenLevel.Instance.curruntLevel);
        clock = true;
        gameOver = false;
        Warning = false;
        Score = 0;
        GameRules.Instance.progress = 0;
        Bar.fillAmount = 0;
        GameRules.Instance.OnrawMatches = 0;
        if (limitType == LimitType.time)
        {
            //clock = false;
            Timer.Instance.UpdateTimer(Limit);
        }
        else
        {
            UIManager.Instance.LimitTxt.text = "Moves: " + Limit.ToString();
        }

        GridManager.Instance.CreateGrid();
    }
    public bool paused;
    IEnumerator Wait()
    {
        if (!paused)
        {
            Limit -= 1;
            Timer.Instance.UpdateTimer(Limit);
            yield return new WaitForSeconds(1);
            clock = false;
        }
    }

    IEnumerator WaitToReload(GameObject Menu)
    {
        yield return new WaitForSeconds(1);
        UIManager.Instance.ShowOrHideMenu(Menu);

        //LoadLevel();
    }
}