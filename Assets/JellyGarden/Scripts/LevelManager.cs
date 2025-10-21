using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SquareBlocks
{
    public SquareTypes block;
    public SquareTypes obstacle;

}

public enum GameState
{
    Map,
    PrepareGame,
    Playing,
    Highscore,
    GameOver,
    Pause,
    PreWinAnimations,
    Win,
    WaitForPopup,
    WaitAfterClose,
    BlockedGame,
    Tutorial,
    PreTutorial,
    WaitForPotion,
    PreFailed,
    RegenLevel
}
public class LevelManager : MonoBehaviour
{

    public static LevelManager THIS;
    public static LevelManager Instance;
    public GameObject itemPrefab;
    public GameObject squarePrefab;
    public Sprite squareSprite;
    public Sprite squareSprite1;
    public Sprite outline1;
    public Sprite outline2;
    public Sprite outline3;
    int count = 0;

    public GameObject blockPrefab;
    public GameObject wireBlockPrefab;
    public GameObject solidBlockPrefab;
    public GameObject undesroyableBlockPrefab;
    public GameObject thrivingBlockPrefab;
    public LifeShop lifeShop;
    public Transform GameField;
    public bool enableInApps;
    public int maxRows = 9;
    public int maxCols = 9;
    public float squareWidth = 1.2f;
    public float squareHeight = 1.2f;
    public Vector2 firstSquarePosition;
    public Square[] squaresArray;
    List<List<Item>> combinedItems = new List<List<Item>>();
    public Item lastDraggedItem;
    public Item lastSwitchedItem;
    public List<Item> destroyAnyway = new List<Item>();
    public GameObject popupScore;
    public int scoreForItem = 10;
    public int scoreForBlock = 100;
    public int scoreForWireBlock = 100;
    public int scoreForSolidBlock = 100;
    public int scoreForThrivingBlock = 100;
    public LIMIT limitType;
    public int Limit = 30;
    public int total_coins;
    public int level_coins;
    Dictionary<string, string> myContacts;
    Dictionary<int, Dictionary<string, int>> levels;
    Dictionary<string, int> stars_count;
    Dictionary<string, int> score;
    string access_token;
    bool success = false;
    string token;
    string primary_domain;
    string url;
    public int AllStars;
    public int TargetScore = 1000;
    public int currentLevel = 1;
    public int FailedCost;
    public int ExtraFailedMoves = 5;
    public int ExtraFailedSecs = 30;
    public List<GemProduct> gemsProducts = new List<GemProduct>();
    public string[] InAppIDs;
    public string GoogleLicenseKey;
    LineRenderer line;
    public bool thrivingBlockDestroyed;
    List<List<Item>> newCombines;
    private bool dragBlocked;
    public int BoostColorfullBomb;
    public int BoostPackage;
    public int BoostStriped;
    public bool BoostHandActivated;
    public bool BoostBombActivated;
    public bool BoostReplacingActivated;
    public BoostIcon emptyBoostIcon;
    public BoostIcon AvctivatedBoostView;
    public BoostIcon activatedBoost;

    public BoostIcon ActivatedBoost
    {
        get
        {
            if (activatedBoost == null)
            {
                //BoostIcon bi = new BoostIcon();
                //bi.type = BoostType.None;
                return emptyBoostIcon;
            }
            else
                return activatedBoost;
        }
        set
        {
            if (value == null)
            {
                if (activatedBoost != null && gameStatus == GameState.Playing)
                    InitScript.Instance.SpendBoost(activatedBoost.type);
                UnLockBoosts();
            }
            //        if (activatedBoost != null) return;
            activatedBoost = value;

            if (value != null)
            {
                LockBoosts();
            }

            if (activatedBoost != null)
            {
                if (activatedBoost.type == BoostType.ExtraMoves || activatedBoost.type == BoostType.ExtraTime)
                {
                    if (LevelManager.Instance.limitType == LIMIT.MOVES)
                        LevelManager.THIS.Limit += 5;
                    else
                        LevelManager.THIS.Limit += 30;

                    ActivatedBoost = null;
                }
            }
        }
    }

    SquareBlocks[] levelSquaresFile = new SquareBlocks[81];
    public int targetBlocks;

    public GameObject[] itemExplPool = new GameObject[20];
    public static int Score;
    public int stars;
    private int linePoint;
    public int star1;
    public int star2;
    public int star3;
    public bool showPopupScores;

    public GameObject stripesEffect;
    public GameObject star1Anim;
    public GameObject star2Anim;
    public GameObject star3Anim;
    public GameObject snowParticle;
    public Color[] scoresColors;
    public Color[] scoresColorsOutline;
    public int colorLimit;
    public int[] ingrCountTarget = new int[2];
    public int[] AnaMasryCountTarget = new int[6];
    public Ingredients[] ingrTarget = new Ingredients[2];
    public CollectItems[] collectItems = new CollectItems[2];
    public ANAMASRY[] trip = new ANAMASRY[6];
    public Sprite[] ingrediendSprites;
    public string[] targetDiscriptions;
    public GameObject ingrObject;
    public GameObject blocksObject;
    public GameObject scoreTargetObject;
    public GameObject AnaMasryTarget;
    private bool matchesGot;
    bool ingredientFly;
    public GameObject[] gratzWords;

    public GameObject Level;
    public GameObject Levelsmap;

    public BoostIcon[] InGameBoosts;
    public int passLevelCounter;

    public Target target;

    public int TargetBlocks
    {
        get
        {
            return targetBlocks;
        }
        set
        {
            if (targetBlocks < 0)
                targetBlocks = 0;
            targetBlocks = value;
        }
    }

    public bool DragBlocked
    {
        get
        {
            return dragBlocked;
        }
        set
        {
            if (value)
            {
                List<Item> items = GetItems();
                foreach (Item item in items)
                {
                    //if (item != null)
                    //    item.anim.SetBool("stop", true);
                }
            }
            else
            {
                //  StartCoroutine( StartIdleCor());
            }
            dragBlocked = value;
        }
    }

    private GameState GameStatus;
    public bool itemsHided;
    public int moveID;

    public int lastRandColor;
    public bool onlyFalling;
    public bool levelLoaded;
    public Hashtable countedSquares;
    public Sprite doubleBlock;
    public bool FacebookEnable;
    internal int latstMatchColor;
    public CombineManager combineManager;

    #region EVENTS

    public delegate void GameStateEvents();
    public static event GameStateEvents OnMapState;
    public static event GameStateEvents OnEnterGame;
    public static event GameStateEvents OnLevelLoaded;
    public static event GameStateEvents OnMenuPlay;
    public static event GameStateEvents OnMenuComplete;
    public static event GameStateEvents OnStartPlay;
    public static event GameStateEvents OnWin;
    public static event GameStateEvents OnLose;
    public int PassedLevels = 1;
    public GameState gameStatus
    {
        get
        {
            return GameStatus;
        }
        set
        {
            GameStatus = value;
            InitScript.Instance.CheckAdsEvents(value);

            if (value == GameState.PrepareGame)
            {
                passLevelCounter++;
                Debug.Log(passLevelCounter);
                MusicBase.Instance.GetComponent<AudioSource>().Stop();
                MusicBase.Instance.GetComponent<AudioSource>().loop = true;
                MusicBase.Instance.GetComponent<AudioSource>().clip = MusicBase.Instance.music[1];
                MusicBase.Instance.GetComponent<AudioSource>().Play();
                PrepareGame();
            }
            else if (value == GameState.WaitForPopup)
            {

                InitLevel();
                OnLevelLoaded();


            }
            else if (value == GameState.PreFailed)
            {
                GameObject.Find("CanvasGlobal").transform.Find("PreFailed").gameObject.SetActive(true);

            }
            else if (value == GameState.Map)
            {
                if (PlayerPrefs.GetInt("OpenLevelTest") <= 0)
                {
                    MusicBase.Instance.GetComponent<AudioSource>().Stop();
                    MusicBase.Instance.GetComponent<AudioSource>().loop = true;
                    MusicBase.Instance.GetComponent<AudioSource>().clip = MusicBase.Instance.music[0];
                    MusicBase.Instance.GetComponent<AudioSource>().Play();
                    EnableMap(true);
                    OnMapState();
                }
                else
                {
                    LevelManager.THIS.gameStatus = GameState.PrepareGame;
                    PlayerPrefs.SetInt("OpenLevelTest", 0);
                    PlayerPrefs.Save();
                }
                if (passLevelCounter > 0 && InitScript.Instance.ShowRateEvery > 0)
                {
                    if (passLevelCounter % InitScript.Instance.ShowRateEvery == 0 && InitScript.Instance.ShowRateEvery > 0 && PlayerPrefs.GetInt("Rated", 0) == 0)
                        InitScript.Instance.ShowRate();
                }
            }
            else if (value == GameState.Pause)
            {
                Time.timeScale = 0;

            }
            else if (value == GameState.Playing)
            {
                Time.timeScale = 1;
                StartCoroutine(AI.THIS.CheckPossibleCombines());
            }
            else if (value == GameState.GameOver)
            {
                GameObject.Find("CanvasGlobal").transform.Find("MenuFailed").gameObject.SetActive(true);
                OnLose();
            }
            else if (value == GameState.PreWinAnimations)
            {
                StartCoroutine(PreWinAnimationsCor());
            }
            else if (value == GameState.Win)
            {
                OnMenuComplete();
                if (currentLevel > PlayerPrefs.GetInt("PassedLevels"))
                {
                    PlayerPrefs.SetInt("PassedLevels", currentLevel);
                }
                CheckNewSection();
                //PlayerPrefs.SetInt("PassedLevels", PassedLevels);
                Debug.Log("Passed level: " + PlayerPrefs.GetInt("PassedLevels"));
                if (SceneManager.GetActiveScene().name != "DreamLevel")
                {
                    GameObject.Find("CanvasGlobal").transform.Find("MenuComplete").gameObject.SetActive(true);
                }
                else if (SceneManager.GetActiveScene().name == "DreamLevel")
                {
                    GameObject.Find("CanvasGlobal").transform.Find("PromoCodeWin").gameObject.SetActive(true);
                }
                SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.complete[1]);
                OnWin();
            }


        }
    }
    public WWW POST(string url, Dictionary<string, string> post, string stars)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in post)
        {
            //    Debug.Log();
            form.AddField(post_arg.Key, post_arg.Value);
        }
        //for (int i = 0; i < stars.Count; i++)
        //{
        //foreach (KeyValuePair<string, int> post_arg in stars[i])
        //{
        //print(post_arg.Key.ToString());
        //form.AddField(post_arg.Key.ToString(), post_arg.Value.ToString());
        //}
        //}
        Debug.Log("levels" + stars);
        form.AddField("levels", stars);
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
        return www;
    }
    public GameObject progress, coins;
    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            Stories myObject = new Stories();
            myObject = JsonUtility.FromJson<Stories>(www.text);
            Debug.Log("WWW Ok!: " + www.text);
            total_coins = myObject.total_coins;
            level_coins = myObject.level_coins;
            success = myObject.success;

            if (www.isDone)
            {
                Debug.Log("www.isDone");
                if (success)
                {
                    coins.SetActive(true);
                    progress.SetActive(false);
                    foreach (var item in player)
                    {
                        item.sent = true;
                    }
                }
            }
            PlayerPrefs.SetInt("Coins", total_coins);
            PlayerPrefs.SetInt("level_coins", level_coins);
            Debug.Log("loginparameters: " + myObject);
            Debug.Log("Total coins" + PlayerPrefs.GetInt("Coins"));
            Debug.Log("Level coins" + PlayerPrefs.GetInt("level_coins"));
        }
        else
        {
#if UNITY_ANDROID
            if (Application.platform == RuntimePlatform.Android)
                AndroidNativePopups.OpenToast("Error while connecting to server please try again", AndroidNativePopups.ToastDuration.Short);
#endif
            Debug.Log("Error: " + www.error);
        }
    }
    void CheckNewSection()
    {
        print("current section: " + ((currentLevel / 20) + 1) + " currunt level: " + currentLevel);
        if (currentLevel >= 20)
        {
            sectionStartLevelNumber = currentLevel - 19;
            sectionEndLevelNumber = sectionStartLevelNumber + 20;
        }
        else
        {
            sectionStartLevelNumber = 1;
            sectionEndLevelNumber = 21;
        }
        int total = 0;

        for (int i = sectionStartLevelNumber; i < sectionEndLevelNumber; i++)
        {
            total += PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", i));
        }
        Debug.Log("hahahahahahahahahahah");
        print("total: " + total);
        Debug.Log("section Start Level Number: " + sectionStartLevelNumber);
        Debug.Log("section End Level Number: " + sectionEndLevelNumber);

    }
    public void MenuPlayEvent()
    {
        OnMenuPlay();
    }


    #endregion

    void LockBoosts()
    {
        foreach (BoostIcon item in InGameBoosts)
        {
            if (item != ActivatedBoost)
                item.LockBoost();
        }
    }

    public void UnLockBoosts()
    {
        foreach (BoostIcon item in InGameBoosts)
        {
            item.UnLockBoost();
        }
    }


    public void LoadLevel()
    {
        Debug.Log("LoadLevel");
        currentLevel = PlayerPrefs.GetInt("OpenLevel"); //TargetHolder.level;
        if (currentLevel == 0)
            currentLevel = 1;
        if (currentLevel > 600)
        {
            gameStatus = GameState.Map;
        }
        LoadDataFromLocal(currentLevel);

    }

    public void EnableMap(bool enable)
    {
        if (enable)
        {
            float aspect = (float)Screen.height / (float)Screen.width;
            GetComponent<Camera>().orthographicSize = 5.3f;
            aspect = (float)Math.Round(aspect, 2);
            if (aspect == 1.6f)
                GetComponent<Camera>().orthographicSize = 6.25f;                    //16:10
            else if (aspect == 1.78f)
                GetComponent<Camera>().orthographicSize = 7f;    //16:9
            else if (aspect == 1.5f)
                GetComponent<Camera>().orthographicSize = 5.9f;                  //3:2
            else if (aspect == 1.33f)
                GetComponent<Camera>().orthographicSize = 5.25f;                  //4:3
            else if (aspect == 1.67f)
                GetComponent<Camera>().orthographicSize = 6.6f;                  //5:3
            else if (aspect == 1.25f)
                GetComponent<Camera>().orthographicSize = 4.9f;                  //5:4
            GetComponent<Camera>().GetComponent<MapCamera>().SetPosition(new Vector2(0, GetComponent<Camera>().transform.position.y));
        }
        else
        {
            InitScript.DateOfExit = DateTime.Now.ToString();  //1.4

            LevelManager.THIS.latstMatchColor = -1;

            GetComponent<Camera>().orthographicSize = 6.5f;
            GameObject.Find("CanvasGlobal").GetComponent<GraphicRaycaster>().enabled = false;
            GameObject.Find("CanvasGlobal").GetComponent<GraphicRaycaster>().enabled = true;
            Level.transform.Find("Canvas").GetComponent<GraphicRaycaster>().enabled = false;
            Level.transform.Find("Canvas").GetComponent<GraphicRaycaster>().enabled = true;

        }
        Camera.main.GetComponent<MapCamera>().enabled = enable;
        Levelsmap.SetActive(!enable);
        Levelsmap.SetActive(enable);
        Level.SetActive(!enable);

        if (enable)
            GameField.gameObject.SetActive(false);

        if (!enable)
            Camera.main.transform.position = new Vector3(0, 0, -10);
        foreach (Transform item in GameField.transform)
        {
            Destroy(item.gameObject);
        }
    }
    [Serializable]
    public class Dict
    {
        public int levelNumber;
        public int StarsCount;
        public int Score;
        public bool sent, finishedOnline;
    }
    public static class JsonHelper
    {
        public static List<T> FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Levels;
        }

        public static string ToJson<T>(List<T> array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Levels = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(List<T> array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Levels = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public List<T> Levels;
        }
    }

    // Use this for initialization
    List<Dict> player;
    int interval = 5;
    float nextTime = 0;
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
            m_Writer = string.Format("{0}: {1}\n{2}", type, condition, stackTrace);
            StartPackage();
            Debug.Log("Exception! ");
        }
    }

    void StartPackage()
    {
        var androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var jo = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        // Accessing the class to call a static method on it
        var jc = new AndroidJavaClass("com.Approcks.CandyMasry.debugSystem.UncaughtExceptionHandlerActivity");
        // Calling a Call method to which the current activity is passed
        jc.CallStatic("Call", jo, m_Writer);
    }

   
  
    List<int> pingList;
    void Start()
    {
#if FACEBOOK
        FacebookEnable = true;//1.6.2
        if (FacebookEnable)
            //  FacebookManager.THIS.CallFBInit();
#else
        FacebookEnable = false;

#endif
#if UNITY_INAPPS
		gameObject.AddComponent<UnityInAppsIntegration> ();
		enableInApps = true;//1.6.1
#else
        enableInApps = false;

#endif
        combineManager = new CombineManager();
        THIS = this;

       // Firebase.Analytics.FirebaseAnalytics.SetCurrentScreen(SceneManager.GetActiveScene().name, null);
        myContacts = new Dictionary<string, string>();
        levels = new Dictionary<int, Dictionary<string, int>>();
        stars_count = new Dictionary<string, int>();
        score = new Dictionary<string, int>();
        PlayerPrefs.DeleteKey("levelsData");
        //PlayerPrefs.DeleteAll();
        pingList = new List<int>();
        //StartCoroutine();
        //  StartCoroutine(CheckConnection());
        Debug.Log("Current level: " + currentLevel);
        if (PlayerPrefs.HasKey("levelsData"))
        {
            player = JsonHelper.FromJson<Dict>(PlayerPrefs.GetString("levelsData"));
        }
        else
        {
            player = new List<Dict>();
        }
        for (int i = 0; i < 100; i++)
        {
            Debug.Log("Played levels stars: " + PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", i + 1)));

        }

        Instance = this;
        if (!LevelManager.THIS.enableInApps)
            GameObject.Find("CanvasMap/Gems").gameObject.SetActive(true);//2.1.2

        gameStatus = GameState.Map;
        for (int i = 0; i < 20; i++)
        {
            itemExplPool[i] = Instantiate(Resources.Load("Prefabs/Effects/ItemExpl"), transform.position, Quaternion.identity) as GameObject;
            itemExplPool[i].GetComponent<SpriteRenderer>().enabled = false;

            // itemExplPool[i].SetActive(false);
        }
        if (SceneManager.GetActiveScene().name == "DreamLevel")
        {
            PlayerPrefs.SetInt("OpenLevel", UnityEngine.Random.Range(1, 10));
            PlayerPrefs.Save();
            LoadLevel();
            if (InitScript.DreamLivesCount > 0)
            {
                Debug.Log("Lives: " + InitScript.DreamLivesCount);
                InitScript.Instance.SpendDreamLives(1);
                gameStatus = GameState.PrepareGame;
            }
        }
        passLevelCounter = 0;
    }

    void InitLevel()
    {

        GenerateLevel();
        GenerateOutline();
        ReGenLevel();
        if (limitType == LIMIT.TIME)
        {
            StopCoroutine(TimeTick());
            StartCoroutine(TimeTick());
            Debug.Log("time limit");
        }
        InitTargets();
        GameField.gameObject.SetActive(true);

    }

    void InitTargets()
    {
        blocksObject.SetActive(false);
        ingrObject.SetActive(false);
        scoreTargetObject.SetActive(false);
        AnaMasryTarget.SetActive(false);
        GameObject ingr1 = ingrObject.transform.Find("Ingr1").gameObject;
        GameObject ingr2 = ingrObject.transform.Find("Ingr2").gameObject;
        //  GameObject ingr3 = ingrObject.transform.Find("Ingr3").gameObject;
        ingr1.SetActive(true);
        ingr2.SetActive(true);
        ingr1.GetComponent<RectTransform>().localPosition = new Vector3(-105.2f, ingr1.GetComponent<RectTransform>().localPosition.y, ingr1.GetComponent<RectTransform>().localPosition.z);
        ingr2.GetComponent<RectTransform>().localPosition = new Vector3(33.5f, ingr2.GetComponent<RectTransform>().localPosition.y, ingr2.GetComponent<RectTransform>().localPosition.z);


        if (ingrCountTarget[0] == 0 && ingrCountTarget[1] == 0)
            ingrObject.SetActive(false);
        if (ingrCountTarget[0] > 0 || ingrCountTarget[1] > 0)
        {
            blocksObject.SetActive(false);
            ingrObject.SetActive(true);
            ingr1 = ingrObject.transform.Find("Ingr1").gameObject;
            ingr2 = ingrObject.transform.Find("Ingr2").gameObject;
            if (target == Target.INGREDIENT)
            {
                if (ingrCountTarget[0] > 0 && ingrCountTarget[1] > 0 && ingrTarget[0] == ingrTarget[1])
                {
                    ingrCountTarget[0] += ingrCountTarget[1];
                    ingrCountTarget[1] = 0;
                    ingrTarget[1] = Ingredients.None;
                }
                Debug.Log("Ingredients");
                ingr1.GetComponent<Image>().sprite = ingrediendSprites[(int)ingrTarget[0]];
                ingr2.GetComponent<Image>().sprite = ingrediendSprites[(int)ingrTarget[1]];
            }

            else if (target == Target.COLLECT)
            {
                if (ingrCountTarget[0] > 0 && ingrCountTarget[1] > 0 && collectItems[0] == collectItems[1])
                {
                    ingrCountTarget[0] += ingrCountTarget[1];
                    ingrCountTarget[1] = 0;
                    collectItems[1] = CollectItems.None;
                }
                ingr1.GetComponent<Image>().sprite = ingrediendSprites[(int)collectItems[0] + 2];
                ingr2.GetComponent<Image>().sprite = ingrediendSprites[(int)collectItems[1] + 2];
            }

            if (ingrCountTarget[0] == 0 && ingrCountTarget[1] > 0)
            {
                ingr1.SetActive(false);
                ingr2.GetComponent<RectTransform>().localPosition = new Vector3(0, ingr2.GetComponent<RectTransform>().localPosition.y, ingr2.GetComponent<RectTransform>().localPosition.z);
            }
            else if (ingrCountTarget[0] > 0 && ingrCountTarget[1] == 0)
            {
                ingr2.SetActive(false);
                ingr1.GetComponent<RectTransform>().localPosition = new Vector3(0, ingr1.GetComponent<RectTransform>().localPosition.y, ingr1.GetComponent<RectTransform>().localPosition.z);
            }
        }
        else if (AnaMasryCountTarget[0] > 0 || AnaMasryCountTarget[1] > 0 || AnaMasryCountTarget[2] > 0 || AnaMasryCountTarget[3] > 0 || AnaMasryCountTarget[4] > 0 || AnaMasryCountTarget[5] > 0)
        {
            if (target == Target.AnaMasry)
            {
                Debug.Log("Anamasry");
                // ingrObject.SetActive(true);
                AnaMasryTarget.SetActive(true);
                scoreTargetObject.SetActive(false);
                ingr1.GetComponent<Image>().sprite = ingrediendSprites[(int)trip[5]];
                Debug.Log(ingrediendSprites[(int)trip[5]]);
                ingr2.GetComponent<Image>().sprite = ingrediendSprites[(int)trip[2]];
            }
        }
        if (targetBlocks > 0)
        {
            blocksObject.SetActive(true);
        }
        else if (ingrCountTarget[0] == 0 && ingrCountTarget[1] == 0 && AnaMasryCountTarget[0] == 0 && AnaMasryCountTarget[1] == 0 && AnaMasryCountTarget[2] == 0 && AnaMasryCountTarget[3] == 0 && AnaMasryCountTarget[4] == 0 && AnaMasryCountTarget[5] == 0)
        {
            Debug.Log("sssssssdsdsdsd");
            ingrObject.SetActive(false);
            blocksObject.SetActive(false);
            scoreTargetObject.SetActive(true);
        }
    }

    public void PrepareGame()
    {
        ActivatedBoost = null;
        Score = 0;
        stars = 0;
        moveID = 0;
        if (SceneManager.GetActiveScene().name != "DreamLevel")
        {

            progress.SetActive(true);
            coins.SetActive(false);
        }

        blocksObject.SetActive(false);
        ingrObject.SetActive(false);
        scoreTargetObject.SetActive(false);
        AnaMasryTarget.SetActive(false);
        star1Anim.SetActive(false);
        star2Anim.SetActive(false);
        star3Anim.SetActive(false);

        collectItems[0] = CollectItems.None;
        collectItems[1] = CollectItems.None;

        ingrTarget[0] = Ingredients.None;
        ingrTarget[1] = Ingredients.None;


        ingrCountTarget[0] = 0;
        ingrCountTarget[1] = 0;

        AnaMasryCountTarget[0] = 0;
        AnaMasryCountTarget[1] = 0;
        AnaMasryCountTarget[2] = 0;
        AnaMasryCountTarget[3] = 0;
        AnaMasryCountTarget[4] = 0;
        AnaMasryCountTarget[5] = 0;
        if (SceneManager.GetActiveScene().name == "DreamLevel")
        {
            GameObject[] ingr = new GameObject[8];
            GameObject[] lightLetters = new GameObject[8];
            ingr[0] = AnaMasryTarget.transform.Find("A-out").gameObject;
            ingr[1] = AnaMasryTarget.transform.Find("N-out").gameObject;
            ingr[2] = AnaMasryTarget.transform.Find("A-out2").gameObject;
            ingr[3] = AnaMasryTarget.transform.Find("M-out").gameObject;
            ingr[4] = AnaMasryTarget.transform.Find("A-out3").gameObject;
            ingr[5] = AnaMasryTarget.transform.Find("S-out").gameObject;
            ingr[6] = AnaMasryTarget.transform.Find("R-out").gameObject;
            ingr[7] = AnaMasryTarget.transform.Find("Y-out").gameObject;
            lightLetters[0] = AnaMasryTarget.transform.Find("A").gameObject;
            lightLetters[1] = AnaMasryTarget.transform.Find("N").gameObject;
            lightLetters[2] = AnaMasryTarget.transform.Find("A1").gameObject;
            lightLetters[3] = AnaMasryTarget.transform.Find("M").gameObject;
            lightLetters[4] = AnaMasryTarget.transform.Find("A2").gameObject;
            lightLetters[5] = AnaMasryTarget.transform.Find("S").gameObject;
            lightLetters[6] = AnaMasryTarget.transform.Find("R").gameObject;
            lightLetters[7] = AnaMasryTarget.transform.Find("Y").gameObject;
            ingr[0].SetActive(true);
            ingr[1].SetActive(true);
            ingr[2].SetActive(true);
            ingr[3].SetActive(true);
            ingr[4].SetActive(true);
            ingr[5].SetActive(true);
            ingr[6].SetActive(true);
            ingr[7].SetActive(true);
            lightLetters[0].SetActive(false);
            lightLetters[1].SetActive(false);
            lightLetters[2].SetActive(false);
            lightLetters[3].SetActive(false);
            lightLetters[4].SetActive(false);
            lightLetters[5].SetActive(false);
            lightLetters[6].SetActive(false);
            lightLetters[7].SetActive(false);
        }

        TargetBlocks = 0;
        EnableMap(false);


        GameField.transform.position = Vector3.zero;
        firstSquarePosition = GameField.transform.position;

        squaresArray = new Square[maxCols * maxRows];
        LoadLevel();
        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxCols; col++)
            {
                if (levelSquaresFile[row * maxCols + col].block == SquareTypes.BLOCK)
                    TargetBlocks++;
                else if (levelSquaresFile[row * maxCols + col].block == SquareTypes.DOUBLEBLOCK)
                    TargetBlocks += 2;
            }
        }
        //float getSize = maxCols - 9;
        //if (getSize < maxRows - 9)
        //    getSize = maxRows - 9;
        //if (getSize > 0)
        //    camera.orthographicSize = 6.5f + getSize * 0.5f;
        GameObject.Find("Canvas").transform.Find("PrePlay").gameObject.SetActive(true);
        if (limitType == LIMIT.MOVES)
        {
            InGameBoosts[0].gameObject.SetActive(true);
            InGameBoosts[1].gameObject.SetActive(false);
        }
        else
        {
            InGameBoosts[0].gameObject.SetActive(false);
            InGameBoosts[1].gameObject.SetActive(true);

        }
        OnEnterGame();
    }

    public void CheckCollectedTarget(GameObject _item)
    {
        for (int i = 0; i < 2; i++)
        {
            if (ingrCountTarget[i] > 0)
            {
                if (_item.GetComponent<Item>() != null)
                {
                    if (_item.GetComponent<Item>().currentType == ItemsTypes.NONE)
                    {
                        if (_item.GetComponent<Item>().color == (int)collectItems[i] - 1)
                        {
                            GameObject item = new GameObject();
                            item.transform.position = _item.transform.position;
                            item.transform.localScale = Vector3.one / 2f;
                            SpriteRenderer spr = item.AddComponent<SpriteRenderer>();
                            spr.sprite = _item.GetComponent<Item>().items[_item.GetComponent<Item>().color];
                            spr.sortingLayerName = "UI";
                            spr.sortingOrder = 1;

                            StartCoroutine(StartAnimateIngredient(item, i));
                        }
                    }
                    else if (_item.GetComponent<Item>().currentType == ItemsTypes.INGREDIENT)
                    {
                        if (_item.GetComponent<Item>().color == (int)ingrTarget[i] + 1000)
                        {
                            GameObject item = new GameObject();
                            item.transform.position = _item.transform.position;
                            item.transform.localScale = Vector3.one / 2f;
                            SpriteRenderer spr = item.AddComponent<SpriteRenderer>();
                            spr.sprite = _item.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                            spr.sortingLayerName = "UI";
                            spr.sortingOrder = 1;

                            StartCoroutine(StartAnimateIngredient(item, i));
                        }
                    }


                }
            }
        }


        if (targetBlocks > 0)
        {
            if (_item.GetComponent<Square>() != null)
            {
                GameObject item = new GameObject();
                item.transform.position = _item.transform.position;
                // item.transform.localScale = Vector3.one / 2f;
                SpriteRenderer spr = item.AddComponent<SpriteRenderer>();
                spr.sprite = _item.GetComponent<SpriteRenderer>().sprite;
                spr.sortingLayerName = "UI";
                spr.sortingOrder = 1;

                StartCoroutine(StartAnimateIngredient(item, 0));

            }
        }
        if (target == Target.AnaMasry)
        {
            for (int i = 0; i < 6; i++)
            {
                if (_item.GetComponent<Item>().currentType == ItemsTypes.ANAMASRY)
                {
                    if (_item.GetComponent<Item>().color == (int)trip[i] + 1000)
                    {
                        GameObject item = new GameObject();
                        item.transform.position = _item.transform.position;
                        item.transform.localScale = Vector3.one / 2f;
                        SpriteRenderer spr = item.AddComponent<SpriteRenderer>();
                        spr.sprite = _item.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        spr.sortingLayerName = "UI";
                        spr.sortingOrder = 1;
                        StartCoroutine(StartAnimateAnaMasry(item, i));
                    }
                }
            }

        }


    }

    public GameObject GetExplFromPool()
    {
        for (int i = 0; i < itemExplPool.Length; i++)
        {
            if (!itemExplPool[i].GetComponent<SpriteRenderer>().enabled)
            {
                // itemExplPool[i].SetActive(true);
                itemExplPool[i].GetComponent<SpriteRenderer>().enabled = true;
                StartCoroutine(HideDelayed(itemExplPool[i]));
                return itemExplPool[i];
            }

        }
        return null;
    }

    IEnumerator HideDelayed(GameObject gm)
    {
        yield return new WaitForSeconds(1f);
        gm.GetComponent<Animator>().SetTrigger("stop");
        gm.GetComponent<Animator>().SetInteger("color", 10);
        gm.GetComponent<SpriteRenderer>().enabled = false;
        //gm.SetActive(false);
    }

    IEnumerator StartAnimateIngredient(GameObject item, int i)
    {

        if (ingrCountTarget[i] > 0)
            ingrCountTarget[i]--;



        Debug.Log("Target Comleted Items: " + AnaMasryCountTarget.Length);
        ingredientFly = true;
        GameObject[] ingr = new GameObject[2];
        ingr[0] = ingrObject.transform.Find("Ingr1").gameObject;
        ingr[1] = ingrObject.transform.Find("Ingr2").gameObject;
        if (targetBlocks > 0)
        {
            ingr[0] = blocksObject.transform.gameObject;
            ingr[1] = blocksObject.transform.gameObject;
        }
        AnimationCurve curveX = new AnimationCurve(new Keyframe(0, item.transform.localPosition.x), new Keyframe(0.4f, ingr[i].transform.position.x));
        AnimationCurve curveY = new AnimationCurve(new Keyframe(0, item.transform.localPosition.y), new Keyframe(0.5f, ingr[i].transform.position.y));
        curveY.AddKey(0.2f, item.transform.localPosition.y + UnityEngine.Random.Range(-2, 0.5f));
        float startTime = Time.time;
        Vector3 startPos = item.transform.localPosition;
        float speed = UnityEngine.Random.Range(0.4f, 0.6f);
        float distCovered = 0;
        while (distCovered < 0.5f)
        {
            distCovered = (Time.time - startTime) * speed;
            item.transform.localPosition = new Vector3(curveX.Evaluate(distCovered), curveY.Evaluate(distCovered), 0);
            item.transform.Rotate(Vector3.back, Time.deltaTime * 1000);
            yield return new WaitForFixedUpdate();
        }
        //     SoundBase.Instance.audio.PlayOneShot(SoundBase.Instance.getStarIngr);
        Destroy(item);
        if (gameStatus == GameState.Playing && !IsIngredientFalling())//1.6.1
            CheckWinLose();
        ingredientFly = false;
    }
    IEnumerator StartAnimateAnaMasry(GameObject item, int i)
    {

        if (AnaMasryCountTarget[i] > 0)
        {
            AnaMasryCountTarget[i]--;
            Debug.Log("Target Comleted Items: " + i);
        }
        if (i == 0)
        {
            count++;
            //  yield return new WaitForSeconds(.5f);
            Debug.Log("Counted: " + count);
        }

        ingredientFly = true;
        GameObject[] ingr = new GameObject[8];
        GameObject[] lightLetters = new GameObject[8];
        ingr[0] = AnaMasryTarget.transform.Find("A-out").gameObject;
        ingr[1] = AnaMasryTarget.transform.Find("N-out").gameObject;
        ingr[2] = AnaMasryTarget.transform.Find("A-out2").gameObject;
        ingr[3] = AnaMasryTarget.transform.Find("M-out").gameObject;
        ingr[4] = AnaMasryTarget.transform.Find("A-out3").gameObject;
        ingr[5] = AnaMasryTarget.transform.Find("S-out").gameObject;
        ingr[6] = AnaMasryTarget.transform.Find("R-out").gameObject;
        ingr[7] = AnaMasryTarget.transform.Find("Y-out").gameObject;
        lightLetters[0] = AnaMasryTarget.transform.Find("A").gameObject;
        lightLetters[1] = AnaMasryTarget.transform.Find("N").gameObject;
        lightLetters[2] = AnaMasryTarget.transform.Find("A1").gameObject;
        lightLetters[3] = AnaMasryTarget.transform.Find("M").gameObject;
        lightLetters[4] = AnaMasryTarget.transform.Find("A2").gameObject;
        lightLetters[5] = AnaMasryTarget.transform.Find("S").gameObject;
        lightLetters[6] = AnaMasryTarget.transform.Find("R").gameObject;
        lightLetters[7] = AnaMasryTarget.transform.Find("Y").gameObject;

        if (targetBlocks > 0)
        {
            ingr[0] = blocksObject.transform.gameObject;
            ingr[1] = blocksObject.transform.gameObject;
        }
        AnimationCurve curveX = new AnimationCurve(new Keyframe(0, item.transform.localPosition.x), new Keyframe(0.4f, ingr[i].transform.position.x));
        AnimationCurve curveY = new AnimationCurve(new Keyframe(0, item.transform.localPosition.y), new Keyframe(0.5f, ingr[i].transform.position.y));
        curveY.AddKey(0.2f, item.transform.localPosition.y + UnityEngine.Random.Range(-2, 0.5f));
        float startTime = Time.time;
        Vector3 startPos = item.transform.localPosition;
        float speed = UnityEngine.Random.Range(0.4f, 0.6f);
        float distCovered = 0;
        while (distCovered < 0.5f)
        {
            distCovered = (Time.time - startTime) * speed;
            item.transform.localPosition = new Vector3(curveX.Evaluate(distCovered), curveY.Evaluate(distCovered), 0);
            item.transform.Rotate(Vector3.back, Time.deltaTime * 1000);
            yield return new WaitForFixedUpdate();
        }
        //     SoundBase.Instance.audio.PlayOneShot(SoundBase.Instance.getStarIngr);
        if (AnaMasryCountTarget[0] < 3)
        {
            if (count >= 1 && count < 2)
            {
                ingr[0].SetActive(false);
                lightLetters[0].SetActive(true);
            }
            if (count >= 2 && count < 3)
            {
                ingr[2].SetActive(false);
                lightLetters[2].SetActive(true);
            }
            if (count >= 3 && count < 4)
            {
                ingr[4].SetActive(false);
                lightLetters[4].SetActive(true);
            }

        }
        if (AnaMasryCountTarget[1] < 1)
        {
            ingr[1].SetActive(false);
            lightLetters[1].SetActive(true);
        }
        if (AnaMasryCountTarget[2] < 1)
        {
            ingr[3].SetActive(false);
            lightLetters[3].SetActive(true);
        }

        if (AnaMasryCountTarget[3] < 1)
        {
            ingr[5].SetActive(false);
            lightLetters[5].SetActive(true);
        }
        if (AnaMasryCountTarget[4] < 1)
        {
            ingr[6].SetActive(false);
            lightLetters[6].SetActive(true);
        }
        if (AnaMasryCountTarget[5] < 1)
        {
            ingr[7].SetActive(false);
            lightLetters[7].SetActive(true);
        }
        Destroy(item);
        if (gameStatus == GameState.Playing && !IsIngredientFalling())//1.6.1
            CheckWinLose();
        ingredientFly = false;
    }


    public void CheckWinLose()
    {
        //		print ("check win lose");
        if (Limit <= 0)
        {
            bool lose = false;
            Limit = 0;

            if (LevelManager.THIS.target == Target.BLOCKS && LevelManager.THIS.TargetBlocks > 0)
            {
                lose = true;
            }
            else if (LevelManager.THIS.target == Target.COLLECT && (LevelManager.THIS.ingrCountTarget[0] > 0 || LevelManager.THIS.ingrCountTarget[1] > 0))
            {
                lose = true;
            }
            else if (LevelManager.THIS.target == Target.INGREDIENT && (LevelManager.THIS.ingrCountTarget[0] > 0 || LevelManager.THIS.ingrCountTarget[1] > 0))
            {
                lose = true;
            }
            else if (LevelManager.THIS.target == Target.AnaMasry && (LevelManager.THIS.AnaMasryCountTarget[0] > 0 || LevelManager.THIS.AnaMasryCountTarget[1] > 0 || LevelManager.THIS.AnaMasryCountTarget[2] > 0 || LevelManager.THIS.AnaMasryCountTarget[3] > 0 || LevelManager.THIS.AnaMasryCountTarget[4] > 0 || LevelManager.THIS.AnaMasryCountTarget[5] > 0))
            {
                lose = true;
            }
            if (LevelManager.Score < LevelManager.THIS.star1)
            {
                lose = true;

            }
            if (lose)
                gameStatus = GameState.PreFailed;
            else if (LevelManager.Score >= LevelManager.THIS.star1 && LevelManager.THIS.target == Target.SCORE)
            {
                gameStatus = GameState.PreWinAnimations;

            }
            else if (LevelManager.Score >= LevelManager.THIS.star1 && LevelManager.THIS.target == Target.BLOCKS && LevelManager.THIS.TargetBlocks <= 0)
            {
                gameStatus = GameState.PreWinAnimations;

            }
            else if (LevelManager.Score >= LevelManager.THIS.star1 && LevelManager.THIS.target == Target.COLLECT && (LevelManager.THIS.ingrCountTarget[0] <= 0 && LevelManager.THIS.ingrCountTarget[1] <= 0))
            {
                gameStatus = GameState.PreWinAnimations;

            }
            else if (LevelManager.Score >= LevelManager.THIS.star1 && LevelManager.THIS.target == Target.INGREDIENT && (LevelManager.THIS.ingrCountTarget[0] <= 0 && LevelManager.THIS.ingrCountTarget[1] <= 0))
            {
                gameStatus = GameState.PreWinAnimations;

            }
            else if (LevelManager.Score >= LevelManager.THIS.star1 && LevelManager.THIS.target == Target.AnaMasry && (LevelManager.THIS.AnaMasryCountTarget[0] <= 0 && LevelManager.THIS.AnaMasryCountTarget[1] <= 0 && LevelManager.THIS.AnaMasryCountTarget[2] <= 0 && LevelManager.THIS.AnaMasryCountTarget[3] <= 0 && LevelManager.THIS.AnaMasryCountTarget[4] <= 0 && LevelManager.THIS.AnaMasryCountTarget[5] <= 0))
            {
                gameStatus = GameState.PreWinAnimations;

            }


        }
        else
        {
            bool win = false;

            if (LevelManager.THIS.target == Target.BLOCKS && LevelManager.THIS.TargetBlocks <= 0)
            {
                win = true;
            }
            else if (LevelManager.THIS.target == Target.COLLECT && (LevelManager.THIS.ingrCountTarget[0] <= 0 && LevelManager.THIS.ingrCountTarget[1] <= 0))
            {
                win = true;
            }
            else if (LevelManager.THIS.target == Target.INGREDIENT && (LevelManager.THIS.ingrCountTarget[0] <= 0 && LevelManager.THIS.ingrCountTarget[1] <= 0))
            {
                win = true;
                //			} else if (LevelManager.Score >= LevelManager.THIS.star1 && LevelManager.THIS.target == Target.SCORE) { // win if get 1 start and still have moves
                //				win = true;
            }
            else if (LevelManager.THIS.target == Target.AnaMasry && (LevelManager.THIS.AnaMasryCountTarget[0] <= 0 && LevelManager.THIS.AnaMasryCountTarget[1] <= 0 && LevelManager.THIS.AnaMasryCountTarget[2] <= 0 && LevelManager.THIS.AnaMasryCountTarget[3] <= 0 && LevelManager.THIS.AnaMasryCountTarget[4] <= 0 && LevelManager.THIS.AnaMasryCountTarget[5] <= 0))
            {
                win = true;
            }
            if (LevelManager.Score < LevelManager.THIS.star1)
            {
                win = false;

            }
            //else if (LevelManager.THIS.target == Target.SCORE && LevelManager.Score >= LevelManager.THIS.star1)
            //    win = true;
            if (win)
                gameStatus = GameState.PreWinAnimations;
        }
    }
    public IEnumerator PreWinAnimationsCor()
    {
        if (!InitScript.Instance.losingLifeEveryGame)
            InitScript.Instance.AddLife(1);
        GameObject.Find("Canvas").transform.Find("CompleteLabel").gameObject.SetActive(true);
        Debug.Log(PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", currentLevel), 0));
        Debug.Log(stars);
        yield return new WaitForSeconds(1);

        List<Item> items = GetRandomItems(limitType == LIMIT.MOVES ? Limit : 8);
        foreach (Item item in items)
        {
            if (limitType == LIMIT.MOVES)
                Limit--;
            item.NextType = (ItemsTypes)UnityEngine.Random.Range(1, 3);
            item.ChangeType();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.3f);
        while (GetAllExtaItems().Count > 0 && gameStatus != GameState.Win)
        { //1.6
            Item item = GetAllExtaItems()[0];
            item.DestroyItem();
            dragBlocked = true;
            yield return new WaitForSeconds(0.1f);
            FindMatches();
            yield return new WaitForSeconds(1f);

            //           GenerateNewItems();
            while (dragBlocked)
                yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1f);
        while (dragBlocked || GetMatches().Count > 0)
            yield return new WaitForSeconds(0.2f);

        GameObject.Find("Canvas").transform.Find("CompleteLabel").gameObject.SetActive(false);
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.complete[0]);

        GameObject.Find("Canvas").transform.Find("PreCompleteBanner").gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        GameObject.Find("Canvas").transform.Find("PreCompleteBanner").gameObject.SetActive(false);
        if (PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", currentLevel), 0) < stars)
        {
            Debug.Log(PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", currentLevel), 0));
            PlayerPrefs.SetInt(string.Format("Level.{0:000}.StarsCount", currentLevel), stars);

        }
        PlayerPrefs.SetInt(string.Format("Level.{0:000}.Score", currentLevel), Score);
        Debug.Log("level score" + PlayerPrefs.GetInt(string.Format("Level.{0:000}.Score", currentLevel)));

        if (Score > PlayerPrefs.GetInt("Score" + currentLevel))
        {
            PlayerPrefs.SetInt("Score" + currentLevel, Score);
        }

#if PLAYFAB || GAMESPARKS
        //NetworkManager.dataManager.SetPlayerScore(currentLevel, Score);
        //NetworkManager.dataManager.SetPlayerLevel(currentLevel + 1);
        //NetworkManager.dataManager.SetStars();
#endif
        if (SceneManager.GetActiveScene().name != "DreamLevel")
        {
            SendToServer(false);
        }
        Debug.Log(stars);

        gameStatus = GameState.Win;
    }
    public int sectionStartLevelNumber;
    public int sectionEndLevelNumber;
    public Hashtable level = new Hashtable();
    public class DuplicateKeyComparer<TKey>
                :
             IComparer<TKey> where TKey : IComparable
    {
        #region IComparer<TKey> Members

        public int Compare(TKey x, TKey y)
        {
            int result = x.CompareTo(y);

            if (result == 0)
                return 1;   // Handle equality as beeing greater
            else
                return result;
        }

        #endregion
    }
    SortedList<string, int> slist = new SortedList<string, int>(new DuplicateKeyComparer<string>());
    List<Dictionary<string, int>> sslist = new List<Dictionary<string, int>>();
    public void SendToServer(bool test)
    {
        int f = 0;
        int d = 10;
        //  int r = d / f;
        // Debug.Log("division: " + r);
        if (PlayerPrefs.HasKey("primary_domain") && PlayerPrefs.HasKey("user_id") && PlayerPrefs.HasKey("token"))
        {
            myContacts.Add("user_id", PlayerPrefs.GetString("user_id"));
            myContacts.Add("token", PlayerPrefs.GetString("token"));
        }
        List<Dict> didnonSent = new List<Dict>();
        string to_server = "";
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
#if UNITY_ANDROID
            if (Application.platform == RuntimePlatform.Android)
                AndroidNativePopups.OpenToast("You must be intrnet connected to get your coins", AndroidNativePopups.ToastDuration.Short);
#endif
            PlayerPrefs.SetInt("Coins", 0);
            progress.SetActive(false);
            coins.SetActive(true);
            Dict myDict = new Dict();
            string levelDataToJason = JsonHelper.ToJson(player, true); ;
            string levelsDataFromJson = "";
            Debug.Log("levelsData: " + levelDataToJason);
            if (PlayerPrefs.HasKey("levelsData"))
            {
                levelsDataFromJson = PlayerPrefs.GetString("levelsData");
                player = JsonHelper.FromJson<Dict>(levelDataToJason);
            }
            else
            {
                PlayerPrefs.SetString("levelsData", levelDataToJason);
                player = JsonHelper.FromJson<Dict>(PlayerPrefs.GetString("levelsData"));
            }
            Debug.Log("Player lenght" + player.Count);
            bool found = false;
            Debug.Log("Sabbagh: " + levelsDataFromJson);
            for (int i = 0; i < player.Count; i++)
            {
                if ((player[i].levelNumber == currentLevel || player[i].levelNumber == 0) && player[i].sent == false)
                {
                    Debug.Log("Array length: " + player.Count);
                    // player.Add(myDict);
                    player[i].levelNumber = currentLevel;
                    player[i].finishedOnline = false;
                    player[i].sent = false;
                    player[i].StarsCount = PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", currentLevel));
                    player[i].Score = PlayerPrefs.GetInt(string.Format("Level.{0:000}.Score", currentLevel));
                    Debug.Log("found");
                    found = true;
                    break;
                }
            }
            Debug.Log("Array length: " + player.Count);
            if (!found)
            {
                int i = player.Count;
                Debug.Log("i: " + i);
                Debug.Log("!found");
                //player = new Dict[i];
                player.Add(myDict);
                player[i].finishedOnline = false;
                Debug.Log("player.Length: " + player.Count);
                player[i].levelNumber = currentLevel;
                player[i].StarsCount = PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", currentLevel));
                player[i].Score = PlayerPrefs.GetInt(string.Format("Level.{0:000}.Score", currentLevel));
                player[i].sent = false;
                i++;
            }
            foreach (var item in player)
            {
                if (item.sent == false)
                {
                    didnonSent.Add(item);
                }
            }
            //Convert to Jason
            levelDataToJason = JsonHelper.ToJson(player, true);
            PlayerPrefs.SetString("levelsData", levelDataToJason);
            to_server = JsonHelper.ToJson(didnonSent, true);
            Debug.Log("Text: " + to_server);
            PlayerPrefs.SetString("to_server", to_server);
            myContacts.Remove("token");
            myContacts.Remove("user_id");
            GameStatus = GameState.Win;


        }
        else
        {
            //Change the type to debug or production 
            myContacts.Add("game_type", "debug");
            Dict myDict = new Dict();
            string levelDataToJason = JsonHelper.ToJson(player, true); ;
            string levelsDataFromJson = "";
            if (PlayerPrefs.HasKey("levelsData"))
            {
                levelsDataFromJson = PlayerPrefs.GetString("levelsData");
                player = JsonHelper.FromJson<Dict>(levelDataToJason);
            }
            else
            {
                PlayerPrefs.SetString("levelsData", levelDataToJason);
                player = JsonHelper.FromJson<Dict>(PlayerPrefs.GetString("levelsData"));
            }
            bool found = false;
            Debug.Log("Sabbagh: " + levelsDataFromJson);
            for (int i = 0; i < player.Count; i++)
            {
                if (player[i].levelNumber == currentLevel || player[i].levelNumber == 0)
                {
                    Debug.Log("Array length: " + player.Count);
                    player[i].levelNumber = currentLevel;
                    player[i].sent = false;
                    player[i].StarsCount = PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", currentLevel));
                    player[i].Score = PlayerPrefs.GetInt(string.Format("Level.{0:000}.Score", currentLevel));
                    Debug.Log("Player[i].score" + player[i].Score);

                    player[i].finishedOnline = true;
                    Debug.Log("found");
                    found = true;
                    break;
                }
            }
            Debug.Log("Array length: " + player.Count);
            if (!found)
            {
                int i = player.Count;
                Debug.Log("i: " + i);
                Debug.Log("!found");
                //player = new Dict[i];
                player.Add(myDict);
                Debug.Log("player.Length: " + player.Count);
                player[i].levelNumber = currentLevel;
                player[i].finishedOnline = true;
                player[i].StarsCount = PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", currentLevel));
                player[i].Score = PlayerPrefs.GetInt(string.Format("Level.{0:000}.Score", currentLevel));
                Debug.Log(player[i].Score);

                i++;
            }
            levelDataToJason = JsonHelper.ToJson(player, true);
            PlayerPrefs.SetString("levelsData", levelDataToJason);
            player = JsonHelper.FromJson<Dict>(PlayerPrefs.GetString("levelsData"));
            foreach (var item in player)
            {
                if (item.sent == false)
                {
                    didnonSent.Add(item);
                }
            }
            to_server = JsonHelper.ToJson(didnonSent, true);
            Debug.Log("Text: " + to_server);
            PlayerPrefs.SetString("to_server", to_server);
            Debug.Log("levels data: " + PlayerPrefs.GetString("levelsData"));
            string url = "";
            if (PlayerPrefs.HasKey("primary_domain"))
            {
                url = PlayerPrefs.GetString("primary_domain") + "/apis/v5/finishLevel";
                Debug.Log("URL: " + url);
                POST(url, myContacts, to_server);
                myContacts.Remove("user_id");
                myContacts.Remove("token");
                PlayerPrefs.DeleteKey("to_server");
                // PlayerPrefs.DeleteKey("to_server");
                Debug.Log("to server: " + PlayerPrefs.GetString("to_server"));
                Debug.Log(url);
            }
            else
            {
#if UNITY_ANDROID
                if (Application.platform == RuntimePlatform.Android)
                    AndroidNativePopups.OpenToast("You must be logged in to get your coins", AndroidNativePopups.ToastDuration.Short);
#endif
                PlayerPrefs.SetInt("Coins", 0);
                progress.SetActive(false);
                coins.SetActive(true);
                Debug.Log("Not Logged in");
            }
            myContacts.Remove("game_type");
            Debug.Log(stars);

        }
    }
    void Update()
    {
        //  StartCoroutine(PingUpdate());
        if (Time.time >= nextTime)
        {

            //do something here every interval seconds
            // PingUpdate();

            nextTime += interval;

        }
        //  AvctivatedBoostView = ActivatedBoost;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReGenLevel();
        }
        /* if (Input.GetKeyDown(KeyCode.W))
         {
             gameStatus = GameState.PreWinAnimations;
         }*/
        if (Input.GetKeyDown(KeyCode.L))
        {
            Limit = 1;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {   //save items state
            print("Saving items...");
            int[] items = new int[99];

            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxCols; col++)
                {
                    if (GetSquare(col, row, false) != null)
                    {
                        if (GetSquare(col, row, false).item != null)
                            items[row * maxCols + col] = GetSquare(col, row, false).item.color;
                    }
                    else
                        items[row * maxCols + col] = -1;

                }
            }
            LevelDebugger.SaveMap(items, maxCols, maxRows);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {   //load items state
            print("load items...");

            int[] items = new int[99];
            items = LevelDebugger.LoadMap(maxCols, maxRows);
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxCols; col++)
                {
                    if (items[row * maxCols + col] > -1)
                    {
                        if (GetSquare(col, row).item != null)
                            GetSquare(col, row).item.SetColor(items[row * maxCols + col]);
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (LevelManager.THIS.gameStatus == GameState.Playing)
                GameObject.Find("CanvasGlobal").transform.Find("MenuPause").gameObject.SetActive(true);
            else if (LevelManager.THIS.gameStatus == GameState.Map)
                Application.Quit();

        }


        if (LevelManager.THIS.gameStatus == GameState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnStartPlay();
                Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (hit != null)
                {
                    Item item = hit.gameObject.GetComponent<Item>();
                    if (!LevelManager.THIS.DragBlocked && LevelManager.THIS.gameStatus == GameState.Playing)
                    {
                        if (LevelManager.THIS.ActivatedBoost.type == BoostType.Bomb && item.currentType != ItemsTypes.BOMB && item.currentType != ItemsTypes.INGREDIENT && item.currentType != ItemsTypes.ANAMASRY)
                        {
                            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.boostBomb);
                            LevelManager.THIS.DragBlocked = true;
                            GameObject obj = Instantiate(Resources.Load("Prefabs/Effects/bomb"), item.transform.position, item.transform.rotation) as GameObject;
                            obj.GetComponent<SpriteRenderer>().sortingOrder = 4;
                            obj.GetComponent<BoostAnimation>().square = item.square;
                            LevelManager.THIS.ActivatedBoost = null;
                        }
                        else if (LevelManager.THIS.ActivatedBoost.type == BoostType.Random_color && item.currentType != ItemsTypes.BOMB)
                        {
                            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.boostColorReplace);
                            LevelManager.THIS.DragBlocked = true;
                            GameObject obj = Instantiate(Resources.Load("Prefabs/Effects/random_color_item"), item.transform.position, item.transform.rotation) as GameObject;
                            obj.GetComponent<BoostAnimation>().square = item.square;
                            obj.GetComponent<SpriteRenderer>().sortingOrder = 4;
                            LevelManager.THIS.ActivatedBoost = null;
                        }
                        else if (item.square.type != SquareTypes.WIREBLOCK)
                        {
                            item.dragThis = true;
                            item.mousePos = item.GetMousePosition();
                            item.deltaPos = Vector3.zero;
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (hit != null)
                {
                    Item item = hit.gameObject.GetComponent<Item>();
                    item.dragThis = false;
                    item.switchDirection = Vector3.zero;
                }
            }
        }
    }
    IEnumerator TimeTick()
    {
        while (true)
        {
            if (gameStatus == GameState.Playing)
            {
                if (limitType == LIMIT.TIME)
                {
                    Limit--;
                    Debug.Log(Limit);
                    if (IsAllItemsFallDown())
                        CheckWinLose();
                }
            }
            if (gameStatus == GameState.Map)
                yield break;
            yield return new WaitForSeconds(1);
        }
    }
    private void GenerateLevel()
    {
        bool chessColor = false;
        Vector3 fieldPos = new Vector3(-maxCols / 2.75f, maxRows / 2.75f, -10);
        for (int row = 0; row < maxRows; row++)
        {
            if (maxCols % 2 == 0)
                chessColor = !chessColor;
            for (int col = 0; col < maxCols; col++)
            {
                CreateSquare(col, row, chessColor);
                chessColor = !chessColor;
            }
        }
        AnimateField(fieldPos);
    }
    void AnimateField(Vector3 pos)
    {

        float yOffset = 0;
        if (target == Target.INGREDIENT)
            yOffset = 0.3f;
        Animation anim = GameField.GetComponent<Animation>();
        AnimationClip clip = new AnimationClip();
        AnimationCurve curveX = new AnimationCurve(new Keyframe(0, pos.x + 15), new Keyframe(0.7f, pos.x - 0.2f), new Keyframe(0.8f, pos.x));
        AnimationCurve curveY = new AnimationCurve(new Keyframe(0, pos.y + yOffset), new Keyframe(1, pos.y + yOffset));
#if UNITY_5
        clip.legacy = true;
#endif
        clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
        clip.AddEvent(new AnimationEvent() { time = 1, functionName = "EndAnimGamField" });
       // anim.AddClip(clip, "appear");
       // anim.Play("appear");
        GameField.transform.position = new Vector2(pos.x + 15, pos.y + yOffset);
    }

    void CreateSquare(int col, int row, bool chessColor = false)
    {
        GameObject square = null;
        square = Instantiate(squarePrefab, firstSquarePosition + new Vector2(col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
        if (chessColor)
        {
            square.GetComponent<SpriteRenderer>().sprite = squareSprite1;
        }
        square.transform.SetParent(GameField);
        square.transform.localPosition = firstSquarePosition + new Vector2(col * squareWidth, -row * squareHeight);
        squaresArray[row * maxCols + col] = square.GetComponent<Square>();
        square.GetComponent<Square>().row = row;
        square.GetComponent<Square>().col = col;
        square.GetComponent<Square>().type = SquareTypes.EMPTY;
        if (levelSquaresFile[row * maxCols + col].block == SquareTypes.EMPTY)
        {
            CreateObstacles(col, row, square, SquareTypes.NONE);
        }
        else if (levelSquaresFile[row * maxCols + col].block == SquareTypes.NONE)
        {
            square.GetComponent<SpriteRenderer>().enabled = false;
            square.GetComponent<Square>().type = SquareTypes.NONE;

        }
        else if (levelSquaresFile[row * maxCols + col].block == SquareTypes.BLOCK)
        {
            GameObject block = Instantiate(blockPrefab, firstSquarePosition + new Vector2(col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
            block.transform.SetParent(square.transform);
            block.transform.localPosition = new Vector3(0, 0, -0.01f);
            square.GetComponent<Square>().block.Add(block);
            square.GetComponent<Square>().type = SquareTypes.BLOCK;

            // TargetBlocks++;
            CreateObstacles(col, row, square, SquareTypes.NONE);
        }
        else if (levelSquaresFile[row * maxCols + col].block == SquareTypes.DOUBLEBLOCK)
        {
            GameObject block = Instantiate(blockPrefab, firstSquarePosition + new Vector2(col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
            block.transform.SetParent(square.transform);
            block.transform.localPosition = new Vector3(0, 0, -0.01f);
            square.GetComponent<Square>().block.Add(block);
            square.GetComponent<Square>().type = SquareTypes.BLOCK;

            //  TargetBlocks++;
            block = Instantiate(blockPrefab, firstSquarePosition + new Vector2(col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
            block.transform.SetParent(square.transform);
            block.transform.localPosition = new Vector3(0, 0, -0.01f);
            square.GetComponent<Square>().block.Add(block);
            square.GetComponent<Square>().type = SquareTypes.BLOCK;
            block.GetComponent<SpriteRenderer>().sprite = doubleBlock;
            block.GetComponent<SpriteRenderer>().sortingOrder = 1;
            //  TargetBlocks++;
            CreateObstacles(col, row, square, SquareTypes.NONE);
        }

    }

    void GenerateOutline()
    {
        int row = 0;
        int col = 0;
        for (row = 0; row < maxRows; row++)
        { //down
            SetOutline(col, row, 0);
        }
        row = maxRows - 1;
        for (col = 0; col < maxCols; col++)
        { //right
            SetOutline(col, row, 90);
        }
        col = maxCols - 1;
        for (row = maxRows - 1; row >= 0; row--)
        { //up
            SetOutline(col, row, 180);
        }
        row = 0;
        for (col = maxCols - 1; col >= 0; col--)
        { //left
            SetOutline(col, row, 270);
        }
        col = 0;
        for (row = 1; row < maxRows - 1; row++)
        {
            for (col = 1; col < maxCols - 1; col++)
            {
                //  if (GetSquare(col, row).type == SquareTypes.NONE)
                SetOutline(col, row, 0);
            }
        }
    }


    void SetOutline(int col, int row, float zRot)
    {
        Square square = GetSquare(col, row, true);
        if (square.type != SquareTypes.NONE)
        {
            if (row == 0 || col == 0 || col == maxCols - 1 || row == maxRows - 1)
            {
                GameObject outline = CreateOutline(square);
                SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                outline.transform.localRotation = Quaternion.Euler(0, 0, zRot);
                if (zRot == 0)
                    outline.transform.localPosition = Vector3.zero + Vector3.left * 0.425f;
                if (zRot == 90)
                    outline.transform.localPosition = Vector3.zero + Vector3.down * 0.425f;
                if (zRot == 180)
                    outline.transform.localPosition = Vector3.zero + Vector3.right * 0.425f;
                if (zRot == 270)
                    outline.transform.localPosition = Vector3.zero + Vector3.up * 0.425f;
                if (row == 0 && col == 0)
                {   //top left
                    spr.sprite = outline3;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    outline.transform.localPosition = Vector3.zero + Vector3.left * 0.015f + Vector3.up * 0.015f;
                }
                if (row == 0 && col == maxCols - 1)
                {   //top right
                    spr.sprite = outline3;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    outline.transform.localPosition = Vector3.zero + Vector3.right * 0.015f + Vector3.up * 0.015f;
                }
                if (row == maxRows - 1 && col == 0)
                {   //bottom left
                    spr.sprite = outline3;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    outline.transform.localPosition = Vector3.zero + Vector3.left * 0.015f + Vector3.down * 0.015f;
                }
                if (row == maxRows - 1 && col == maxCols - 1)
                {   //bottom right
                    spr.sprite = outline3;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    outline.transform.localPosition = Vector3.zero + Vector3.right * 0.015f + Vector3.down * 0.015f;
                }
            }
            else
            {
                //top left
                if (GetSquare(col - 1, row - 1, true).type == SquareTypes.NONE && GetSquare(col, row - 1, true).type == SquareTypes.NONE && GetSquare(col - 1, row, true).type == SquareTypes.NONE)
                {
                    GameObject outline = CreateOutline(square);
                    SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                    spr.sprite = outline3;
                    outline.transform.localPosition = Vector3.zero + Vector3.left * 0.015f + Vector3.up * 0.015f;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 180);
                }
                //top right
                if (GetSquare(col + 1, row - 1, true).type == SquareTypes.NONE && GetSquare(col, row - 1, true).type == SquareTypes.NONE && GetSquare(col + 1, row, true).type == SquareTypes.NONE)
                {
                    GameObject outline = CreateOutline(square);
                    SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                    spr.sprite = outline3;
                    outline.transform.localPosition = Vector3.zero + Vector3.right * 0.015f + Vector3.up * 0.015f;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
                //bottom left
                if (GetSquare(col - 1, row + 1, true).type == SquareTypes.NONE && GetSquare(col, row + 1, true).type == SquareTypes.NONE && GetSquare(col - 1, row, true).type == SquareTypes.NONE)
                {
                    GameObject outline = CreateOutline(square);
                    SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                    spr.sprite = outline3;
                    outline.transform.localPosition = Vector3.zero + Vector3.left * 0.015f + Vector3.down * 0.015f;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 270);
                }
                //bottom right
                if (GetSquare(col + 1, row + 1, true).type == SquareTypes.NONE && GetSquare(col, row + 1, true).type == SquareTypes.NONE && GetSquare(col + 1, row, true).type == SquareTypes.NONE)
                {
                    GameObject outline = CreateOutline(square);
                    SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                    spr.sprite = outline3;
                    outline.transform.localPosition = Vector3.zero + Vector3.right * 0.015f + Vector3.down * 0.015f;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }


            }
        }
        else
        {
            bool corner = false;
            if (GetSquare(col - 1, row, true).type != SquareTypes.NONE && GetSquare(col, row - 1, true).type != SquareTypes.NONE)
            {
                GameObject outline = CreateOutline(square);
                SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                spr.sprite = outline2;
                outline.transform.localPosition = Vector3.zero;
                outline.transform.localRotation = Quaternion.Euler(0, 0, 0);
                corner = true;
            }
            if (GetSquare(col + 1, row, true).type != SquareTypes.NONE && GetSquare(col, row + 1, true).type != SquareTypes.NONE)
            {
                GameObject outline = CreateOutline(square);
                SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                spr.sprite = outline2;
                outline.transform.localPosition = Vector3.zero;
                outline.transform.localRotation = Quaternion.Euler(0, 0, 180);
                corner = true;
            }
            if (GetSquare(col + 1, row, true).type != SquareTypes.NONE && GetSquare(col, row - 1, true).type != SquareTypes.NONE)
            {
                GameObject outline = CreateOutline(square);
                SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                spr.sprite = outline2;
                outline.transform.localPosition = Vector3.zero;
                outline.transform.localRotation = Quaternion.Euler(0, 0, 270);
                corner = true;
            }
            if (GetSquare(col - 1, row, true).type != SquareTypes.NONE && GetSquare(col, row + 1, true).type != SquareTypes.NONE)
            {
                GameObject outline = CreateOutline(square);
                SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                spr.sprite = outline2;
                outline.transform.localPosition = Vector3.zero;
                outline.transform.localRotation = Quaternion.Euler(0, 0, 90);
                corner = true;
            }


            if (!corner)
            {
                if (GetSquare(col, row - 1, true).type != SquareTypes.NONE)
                {
                    GameObject outline = CreateOutline(square);
                    SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                    outline.transform.localPosition = Vector3.zero + Vector3.up * 0.395f;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
                if (GetSquare(col, row + 1, true).type != SquareTypes.NONE)
                {
                    GameObject outline = CreateOutline(square);
                    SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                    outline.transform.localPosition = Vector3.zero + Vector3.down * 0.395f;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
                if (GetSquare(col - 1, row, true).type != SquareTypes.NONE)
                {
                    GameObject outline = CreateOutline(square);
                    SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                    outline.transform.localPosition = Vector3.zero + Vector3.left * 0.395f;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                if (GetSquare(col + 1, row, true).type != SquareTypes.NONE)
                {
                    GameObject outline = CreateOutline(square);
                    SpriteRenderer spr = outline.GetComponent<SpriteRenderer>();
                    outline.transform.localPosition = Vector3.zero + Vector3.right * 0.395f;
                    outline.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        //Vector3 pos = GameField.transform.TransformPoint((Vector3)firstSquarePosition + new Vector3(col * squareWidth - squareWidth / 2, -row * squareHeight, 10));
        //line.SetVertexCount(linePoint + 1);
        //line.SetPosition(linePoint++, pos);

    }

    GameObject CreateOutline(Square square)
    {
        GameObject outline = new GameObject();
        outline.name = "outline";
        outline.transform.SetParent(square.transform);
        outline.transform.localPosition = Vector3.zero;
        SpriteRenderer spr = outline.AddComponent<SpriteRenderer>();
        spr.sprite = outline1;
        spr.sortingOrder = 1;
        return outline;
    }

    void CreateObstacles(int col, int row, GameObject square, SquareTypes type)
    {
        if ((levelSquaresFile[row * maxCols + col].obstacle == SquareTypes.WIREBLOCK && type == SquareTypes.NONE) || type == SquareTypes.WIREBLOCK)
        {
            GameObject block = Instantiate(wireBlockPrefab, firstSquarePosition + new Vector2(col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
            block.transform.SetParent(square.transform);
            block.transform.localPosition = new Vector3(0, 0, -0.5f);
            square.GetComponent<Square>().block.Add(block);
            square.GetComponent<Square>().type = SquareTypes.WIREBLOCK;
            block.GetComponent<SpriteRenderer>().sortingOrder = 3;
            //   TargetBlocks++;
        }
        else if ((levelSquaresFile[row * maxCols + col].obstacle == SquareTypes.SOLIDBLOCK && type == SquareTypes.NONE) || type == SquareTypes.SOLIDBLOCK)
        {
            GameObject block = Instantiate(solidBlockPrefab, firstSquarePosition + new Vector2(col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
            block.transform.SetParent(square.transform);
            block.transform.localPosition = new Vector3(0, 0, -0.5f);
            square.GetComponent<Square>().block.Add(block);
            block.GetComponent<SpriteRenderer>().sortingOrder = 3;
            square.GetComponent<Square>().type = SquareTypes.SOLIDBLOCK;

            //  TargetBlocks++;
        }
        else if ((levelSquaresFile[row * maxCols + col].obstacle == SquareTypes.UNDESTROYABLE && type == SquareTypes.NONE) || type == SquareTypes.UNDESTROYABLE)
        {
            GameObject block = Instantiate(undesroyableBlockPrefab, firstSquarePosition + new Vector2(col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
            block.transform.SetParent(square.transform);
            block.transform.localPosition = new Vector3(0, 0, -0.5f);
            square.GetComponent<Square>().block.Add(block);
            square.GetComponent<Square>().type = SquareTypes.UNDESTROYABLE;

            //  TargetBlocks++;
        }
        else if ((levelSquaresFile[row * maxCols + col].obstacle == SquareTypes.THRIVING && type == SquareTypes.NONE) || type == SquareTypes.THRIVING)
        {
            GameObject block = Instantiate(thrivingBlockPrefab, firstSquarePosition + new Vector2(col * squareWidth, -row * squareHeight), Quaternion.identity) as GameObject;
            block.transform.SetParent(square.transform);
            block.transform.localPosition = new Vector3(0, 0, -0.5f);
            block.GetComponent<SpriteRenderer>().sortingOrder = 3;
            if (square.GetComponent<Square>().item != null)
                Destroy(square.GetComponent<Square>().item.gameObject);
            square.GetComponent<Square>().block.Add(block);
            square.GetComponent<Square>().type = SquareTypes.THRIVING;

            //   TargetBlocks++;
        }

    }

    void GenerateNewItems(bool falling = true)
    {
        for (int col = 0; col < maxCols; col++)
        {
            for (int row = maxRows - 1; row >= 0; row--)
            {
                if (GetSquare(col, row) != null)
                {
                    if (!GetSquare(col, row).IsNone() && GetSquare(col, row).CanGoInto() && GetSquare(col, row).item == null)
                    {
                        if ((GetSquare(col, row).item == null && !GetSquare(col, row).IsHaveSolidAbove()) || !falling)
                        {
                            GetSquare(col, row).GenItem(falling);
                        }
                    }
                }
            }
        }

    }

    public void NoMatches()
    {
        StartCoroutine(NoMatchesCor());
    }

    IEnumerator NoMatchesCor()
    {
        if (gameStatus == GameState.Playing)
        {
            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.noMatch);

            GameObject.Find("Canvas").transform.Find("NoMoreMatches").gameObject.SetActive(true);
            gameStatus = GameState.PrepareGame;
            yield return new WaitForSeconds(1);
            //  ReGenLevel();
        }
    }

    public void ReGenLevel()
    {
        itemsHided = false;
        DragBlocked = true;
        if (gameStatus != GameState.Playing && gameStatus != GameState.RegenLevel)
            DestroyItems();
        else if (gameStatus == GameState.RegenLevel)
            DestroyItems(true);
        OnLevelLoaded();
        StartCoroutine(RegenMatches());
        OnLevelLoaded();
    }

    IEnumerator RegenMatches(bool onlyFalling = false)
    {
        if (gameStatus == GameState.RegenLevel)
        {
            //while (!itemsHided)
            //{
            yield return new WaitForSeconds(0.5f);
            //}
        }
        if (!onlyFalling)
            GenerateNewItems(false);
        else
            LevelManager.THIS.onlyFalling = true;
        //   yield return new WaitForSeconds(1f);
        yield return new WaitForFixedUpdate();

        List<List<Item>> combs = GetMatches();
        //while (!matchesGot)
        //{
        //    yield return new WaitForFixedUpdate();

        //}
        //combs = newCombines;
        //matchesGot = false;
        do
        {
            foreach (List<Item> comb in combs)
            {
                int colorOffset = 0;
                foreach (Item item in comb)
                {
                    item.GenColor(item.color + colorOffset);
                    colorOffset++;
                }
            }
            combs = GetMatches();
            //while (!matchesGot)
            //{
            //    yield return new WaitForFixedUpdate();
            //}
            //combs = newCombines;
            //matchesGot = false;

            // yield return new WaitForFixedUpdate();
        } while (combs.Count > 0);
        yield return new WaitForFixedUpdate();
        SetPreBoosts();
        if (!onlyFalling)
            DragBlocked = false;
        LevelManager.THIS.onlyFalling = false;
        if (gameStatus == GameState.RegenLevel)
            gameStatus = GameState.Playing;
        //StartCoroutine(CheckFallingAtStart());
    }
    void SetPreBoosts()
    {
        if (BoostPackage > 0)
        {
            InitScript.Instance.SpendBoost(BoostType.Packages);
            foreach (Item item in GetRandomItems(BoostPackage))
            {
                item.NextType = ItemsTypes.PACKAGE;
                item.ChangeType();
                item.boost = true;
            }
            BoostPackage = 0;
        }
        if (BoostColorfullBomb > 0)
        {
            InitScript.Instance.SpendBoost(BoostType.Colorful_bomb);
            foreach (Item item in GetRandomItems(BoostColorfullBomb))
            {
                item.NextType = ItemsTypes.BOMB;
                item.ChangeType();
                item.boost = true;
            }
            BoostColorfullBomb = 0;
        }
        if (BoostStriped > 0)
        {
            InitScript.Instance.SpendBoost(BoostType.Stripes);
            foreach (Item item in GetRandomItems(BoostStriped))
            {
                item.NextType = (ItemsTypes)UnityEngine.Random.Range(1, 3);
                item.ChangeType();
                item.boost = true;
            }
            BoostStriped = 0;
        }
    }
    public List<Item> GetRandomItems(int count)
    {
        List<Item> list = new List<Item>();
        List<Item> list2 = new List<Item>();
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        if (items.Length < count)
            count = items.Length;
        foreach (GameObject item in items)
        {
            if (item.GetComponent<Item>().currentType == ItemsTypes.NONE && item.GetComponent<Item>().NextType == ItemsTypes.NONE && !item.GetComponent<Item>().destroying)
            {
                list.Add(item.GetComponent<Item>());
            }
        }

        while (list2.Count < count)
        {

            try
            {
                Item newItem = list[UnityEngine.Random.Range(0, list.Count)];
                if (list2.IndexOf(newItem) < 0)
                {
                    list2.Add(newItem);
                }
            }
            catch (Exception ex)
            {
                gameStatus = GameState.Win;
            }
        }
        return list2;
    }

    List<Item> GetAllExtaItems()
    {
        List<Item> list = new List<Item>();
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            if (item.GetComponent<Item>().currentType != ItemsTypes.NONE)
            {
                list.Add(item.GetComponent<Item>());
            }
        }

        return list;
    }

    public List<Item> GetIngredients(int i)
    {
        List<Item> list = new List<Item>();
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            if (item.GetComponent<Item>().currentType == ItemsTypes.INGREDIENT && item.GetComponent<Item>().color == 1000 + (int)LevelManager.THIS.ingrTarget[i])
            {
                list.Add(item.GetComponent<Item>());
            }
        }
        return list;
    }
    public List<Item> GetMasry(int i)
    {
        List<Item> list = new List<Item>();
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            if (item.GetComponent<Item>().currentType == ItemsTypes.ANAMASRY && item.GetComponent<Item>().color == 1000 + (int)LevelManager.THIS.trip[i])
            {
                list.Add(item.GetComponent<Item>());
            }
        }
        return list;
    }

    public void DestroyItems(bool withoutEffects = false)
    {

        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            if (item != null)
            {
                if (item.GetComponent<Item>().currentType != ItemsTypes.INGREDIENT && item.GetComponent<Item>().currentType == ItemsTypes.NONE && item.GetComponent<Item>().currentType == ItemsTypes.ANAMASRY)
                {
                    if (!withoutEffects)
                        item.GetComponent<Item>().DestroyItem();
                    else
                        item.GetComponent<Item>().SmoothDestroy();
                }
            }
        }

    }

    public IEnumerator FindMatchDelay()
    {
        yield return new WaitForSeconds(0.2f);
        LevelManager.THIS.FindMatches();

    }

    public void FindMatches()
    {
        StartCoroutine(FallingDown());
    }

    public List<List<Item>> GetMatches(FindSeparating separating = FindSeparating.NONE, int matches = 3)
    {
        newCombines = new List<List<Item>>();
        //       List<Item> countedSquares = new List<Item>();
        countedSquares = new Hashtable();
        countedSquares.Clear();
        for (int col = 0; col < maxCols; col++)
        {
            for (int row = 0; row < maxRows; row++)
            {
                if (GetSquare(col, row) != null)
                {
                    if (!countedSquares.ContainsValue(GetSquare(col, row).item))
                    {
                        List<Item> newCombine = GetSquare(col, row).FindMatchesAround(separating, matches, countedSquares);
                        if (newCombine.Count >= matches)
                            newCombines.Add(newCombine);
                    }
                }
            }
        }
        //print("global " + countedSquares.Count);
        //  Debug.Break();
        return newCombines;
    }

    IEnumerator GetMatchesCor(FindSeparating separating = FindSeparating.NONE, int matches = 3, bool Smooth = true)
    {
        Hashtable countedSquares = new Hashtable();
        for (int col = 0; col < maxCols; col++)
        {
            //if (Smooth)
            //                    yield return new WaitForFixedUpdate();
            for (int row = 0; row < maxRows; row++)
            {

                if (GetSquare(col, row) != null)
                {
                    if (!countedSquares.ContainsValue(GetSquare(col, row).item))
                    {
                        List<Item> newCombine = GetSquare(col, row).FindMatchesAround(separating, matches, countedSquares);
                        if (newCombine.Count >= matches)
                            newCombines.Add(newCombine);
                    }
                }
            }
        }
        matchesGot = true;
        yield return new WaitForFixedUpdate();

    }

    IEnumerator CheckFallingAtStart()
    {
        yield return new WaitForSeconds(0.5f);
        while (!IsAllItemsFallDown())
        {
            yield return new WaitForSeconds(0.1f);
        }
        FindMatches();
    }

    public bool CheckExtraPackage(List<List<Item>> rowItems)
    {
        //     print("set package");
        foreach (List<Item> items in rowItems)
        {
            foreach (Item item in items)
            {
                if (item.square.FindMatchesAround(FindSeparating.VERTICAL).Count > 2)
                {
                    if (LevelManager.THIS.lastDraggedItem == null)
                        LevelManager.THIS.lastDraggedItem = item;
                    LevelManager.THIS.latstMatchColor = item.color;
                    //           print(LevelManager.THIS.latstMatchColor);
                    return true;
                }
            }
        }
        return false;
    }


    IEnumerator FallingDown()
    {
        bool nearEmptySquareDetected = false;
        int combo = 0;
        AI.THIS.allowShowTip = false;
        List<Item> it = GetItems();
        for (int i = 0; i < it.Count; i++)
        {
            Item item = it[i];
            if (item != null)
            {
                //AI.THIS.StopAllCoroutines();
                item.anim.StopPlayback();
            }
        }
        //        combineManager.GetCombine();

        while (true)
        {

            //find matches
            yield return new WaitForSeconds(0.1f);

            combinedItems.Clear();
            combinedItems = combineManager.GetCombine(); //GetMatches();  //1.6
                                                         //StartCoroutine(GetMatchesCor());
                                                         //while (!matchesGot)
                                                         //    yield return new WaitForFixedUpdate();
                                                         //combinedItems = newCombines;
                                                         //matchesGot = false;
                                                         //   print(LevelManager.THIS.latstMatchColor);
                                                         //			if (LevelManager.THIS.CheckExtraPackage (GetMatches (FindSeparating.HORIZONTAL)) && lastSwitchedItem != null) {
                                                         //
                                                         //				if (LevelManager.THIS.latstMatchColor == lastDraggedItem.color && LevelManager.THIS.lastDraggedItem.NextType == ItemsTypes.NONE)
                                                         //					LevelManager.THIS.lastDraggedItem.NextType = ItemsTypes.PACKAGE;
                                                         //				else if (LevelManager.THIS.latstMatchColor == lastSwitchedItem.color && LevelManager.THIS.lastDraggedItem.NextType == ItemsTypes.NONE)
                                                         //					LevelManager.THIS.lastSwitchedItem.NextType = ItemsTypes.PACKAGE;
                                                         //				lastDraggedItem.ChangeType ();
                                                         //				lastSwitchedItem.ChangeType ();
                                                         //
                                                         //			}

            if (combinedItems.Count > 0)
                combo++;
            foreach (List<Item> desrtoyItems in combinedItems)
            {

                //				if (lastDraggedItem == null) {  //1.6
                //					if (desrtoyItems.Count == 4) {
                //						if (lastDraggedItem == null)
                //							lastDraggedItem = desrtoyItems [UnityEngine.Random.Range (0, desrtoyItems.Count)];
                //						lastDraggedItem.NextType = (ItemsTypes)UnityEngine.Random.Range (1, 3);
                //						//lastDraggedItem.ChangeType();
                //					}
                //					if (desrtoyItems.Count >= 5) {
                //						if (lastDraggedItem == null)
                //							lastDraggedItem = desrtoyItems [UnityEngine.Random.Range (0, desrtoyItems.Count)];
                //						lastDraggedItem.NextType = ItemsTypes.BOMB;
                //						//lastDraggedItem.ChangeType();
                //					}
                //
                //				}
                // if (desrtoyItems.Count > 0) PopupScore(scoreForItem * desrtoyItems.Count, desrtoyItems[(int)desrtoyItems.Count / 2].transform.position, Color.black);
                foreach (Item item in desrtoyItems)
                {//TODO items not destroy

                    if (item.currentType != ItemsTypes.NONE)
                        yield return new WaitForSeconds(0.1f);
                    item.DestroyItem(true);  //destroy items safely
                    if (item.currentType != ItemsTypes.NONE)
                    {
                        //while (!item.animationFinished)
                        //{
                        //    yield return new WaitForFixedUpdate();
                        //}
                    }

                }
            }

            foreach (Item item in destroyAnyway)
            {
                //  if(item.sprRenderer.enabled)
                item.DestroyItem(true, "", true);  //destroy items safely
                                                   //yield return new WaitForSeconds(0.2f);
            }
            //          if (destroyAnyway.Count > 0) PopupScore(scoreForItem * destroyAnyway.Count, destroyAnyway[(int)destroyAnyway.Count / 2].transform.position);
            destroyAnyway.Clear();

            if (lastDraggedItem != null)
            {
                //
                //				if (LevelManager.THIS.CheckExtraPackage (GetMatches (FindSeparating.HORIZONTAL))) {  //1.6
                //					if (LevelManager.THIS.latstMatchColor == lastDraggedItem.color)
                //						LevelManager.THIS.lastDraggedItem.NextType = ItemsTypes.PACKAGE;
                //					else if (LevelManager.THIS.latstMatchColor == lastSwitchedItem.color)
                //						LevelManager.THIS.lastSwitchedItem.NextType = ItemsTypes.PACKAGE;
                //					lastDraggedItem.ChangeType ();
                //					lastSwitchedItem.ChangeType ();
                //
                //				}
                if (lastDraggedItem.NextType != ItemsTypes.NONE)
                {
                    //lastDraggedItem.ChangeType();
                    yield return new WaitForSeconds(0.5f);

                }
                lastDraggedItem = null;
            }

            while (!IsAllDestoyFinished())
            {
                yield return new WaitForSeconds(0.1f);
            }

            //falling down
            for (int i = 0; i < 20; i++)
            {   //just for testing
                for (int col = 0; col < maxCols; col++)
                {
                    for (int row = maxRows - 1; row >= 0; row--)
                    {   //need to enumerate rows from bottom to top
                        if (GetSquare(col, row) != null)
                            GetSquare(col, row).FallOut();
                    }
                }
                // yield return new WaitForFixedUpdate();
            }
            if (!nearEmptySquareDetected)
                yield return new WaitForSeconds(0.2f);

            CheckIngredient();
            for (int col = 0; col < maxCols; col++)
            {
                for (int row = maxRows - 1; row >= 0; row--)
                {
                    if (GetSquare(col, row) != null)
                    {
                        if (!GetSquare(col, row).IsNone())
                        {
                            if (GetSquare(col, row).item != null)
                            {
                                GetSquare(col, row).item.StartFalling();
                                //if (row == maxRows - 1 && GetSquare(col, row).item.currentType == ItemsTypes.INGREDIENT)
                                //{
                                //    destroyAnyway.Add(GetSquare(col, row).item);
                                //}
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
            GenerateNewItems();
            StartCoroutine(RegenMatches(true));
            yield return new WaitForSeconds(0.1f);
            while (!IsAllItemsFallDown())
            {
                yield return new WaitForSeconds(0.1f);
            }

            //detect near empty squares to fall into
            nearEmptySquareDetected = false;

            for (int col = 0; col < maxCols; col++)
            {
                for (int row = maxRows - 1; row >= 0; row--)
                {
                    if (GetSquare(col, row) != null)
                    {
                        if (!GetSquare(col, row).IsNone())
                        {
                            if (GetSquare(col, row).item != null)
                            {
                                if (GetSquare(col, row).item.GetNearEmptySquares())
                                    nearEmptySquareDetected = true;

                            }
                        }
                    }
                    // if (nearEmptySquareDetected) break;
                }
                //   if (nearEmptySquareDetected) break;
            }
            //StartCoroutine(GetMatchesCor());
            //while (!matchesGot)
            //    yield return new WaitForFixedUpdate();
            //matchesGot = false;
            //CheckIngredient();
            while (!IsAllItemsFallDown())
            {//2.0
                yield return new WaitForSeconds(0.1f);
            }

            if (destroyAnyway.Count > 0)
                nearEmptySquareDetected = true;
            if (GetMatches().Count <= 0 && !nearEmptySquareDetected)
                break;
        }

        List<Item> item_ = GetItems();
        for (int i = 0; i < it.Count; i++)
        {
            Item item1 = item_[i];
            if (item1 != null)
            {
                if (item1 != item1.square.item)
                {
                    Destroy(item1.gameObject);
                }
            }
        }

        //thrive thriving blocks
        if (!thrivingBlockDestroyed)
        {
            bool thrivingBlockSelected = false;
            for (int col = 0; col < maxCols; col++)
            {
                if (thrivingBlockSelected)
                    break;
                for (int row = maxRows - 1; row >= 0; row--)
                {
                    if (thrivingBlockSelected)
                        break;
                    if (GetSquare(col, row) != null)
                    {
                        if (GetSquare(col, row).type == SquareTypes.THRIVING)
                        {
                            List<Square> sqList = GetSquare(col, row).GetAllNeghbors();
                            foreach (Square sq in sqList)
                            {
                                if (sq.CanGoInto() && UnityEngine.Random.Range(0, 1) == 0 && sq.type == SquareTypes.EMPTY)
                                {
                                    if (sq.item != null)
                                    {//1.6.1
                                        if (sq.item.currentType == ItemsTypes.NONE)
                                        {//1.6.1
                                         //GetSquare(col, row).GenThriveBlock(sq);
                                            CreateObstacles(sq.col, sq.row, sq.gameObject, SquareTypes.THRIVING);

                                            thrivingBlockSelected = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        thrivingBlockDestroyed = false;

        if (gameStatus == GameState.Playing && !ingredientFly)
            LevelManager.THIS.CheckWinLose();

        if (combo > 2 && gameStatus == GameState.Playing)
        {
            gratzWords[UnityEngine.Random.Range(0, gratzWords.Length)].SetActive(true);
            combo = 0;
        }
        LevelManager.THIS.latstMatchColor = -1;
        CheckItemsPositions();//1.6.1
        DragBlocked = false;

        if (gameStatus == GameState.Playing)
            StartCoroutine(AI.THIS.CheckPossibleCombines());



    }

    void CheckItemsPositions()
    {//1.6.1
        List<Item> items = GetItems();
        foreach (var item in items)
        {
            if (item)
                item.transform.position = item.square.transform.position + Vector3.back * 0.2f;
        }
    }


    public void DestroyDoubleBomb(int col)
    {
        StartCoroutine(DestroyDoubleBombCor(col));
        StartCoroutine(DestroyDoubleBombCorBack(col));
    }

    IEnumerator DestroyDoubleBombCor(int col)
    {
        for (int i = col; i < maxCols; i++)
        {
            List<Item> list = GetColumn(i);
            foreach (Item item in list)
            {
                if (item != null)
                    item.DestroyItem(true, "", true);
            }
            yield return new WaitForSeconds(0.3f);
            //GenerateNewItems();
            //yield return new WaitForSeconds(0.3f);
        }
        if (col <= maxCols - col - 1)
            FindMatches();
    }

    IEnumerator DestroyDoubleBombCorBack(int col)
    {
        for (int i = col - 1; i >= 0; i--)
        {
            List<Item> list = GetColumn(i);
            foreach (Item item in list)
            {
                if (item != null)
                    item.DestroyItem(true, "", true);
            }
            yield return new WaitForSeconds(0.3f);
            //GenerateNewItems();
            //yield return new WaitForSeconds(0.3f);
        }
        if (col > maxCols - col - 1)
            FindMatches();
    }


    public Square GetSquare(int col, int row, bool safe = false)
    {
        if (!safe)
        {
            if (row >= maxRows || col >= maxCols)
                return null;
            return squaresArray[row * maxCols + col];
        }
        else
        {
            row = Mathf.Clamp(row, 0, maxRows - 1);
            col = Mathf.Clamp(col, 0, maxCols - 1);
            return squaresArray[row * maxCols + col];
        }
    }

    bool IsAllDestoyFinished()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            Item itemComponent = item.GetComponent<Item>();
            if (itemComponent == null)
            {
                return false;
            }
            if (itemComponent.destroying && !itemComponent.animationFinished)
                return false;
        }
        return true;
    }

    bool IsIngredientFalling()
    {//1.6.1
        if (gameStatus == GameState.PreWinAnimations)
            return true;
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            Item itemComponent = item.GetComponent<Item>();
            if (itemComponent != null)
            {
                if (itemComponent.falling && itemComponent.currentType == ItemsTypes.INGREDIENT || itemComponent.falling && itemComponent.currentType == ItemsTypes.ANAMASRY)
                    return true;
            }
        }
        return false;

    }

    bool IsAllItemsFallDown()
    {
        if (gameStatus == GameState.PreWinAnimations)
            return true;
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            Item itemComponent = item.GetComponent<Item>();
            if (itemComponent == null)
            {
                return false;
            }
            if (itemComponent.falling)
                return false;
        }
        return true;
    }

    public Vector2 GetPosition(Item item)
    {
        return GetPosition(item.square);
    }

    public Vector2 GetPosition(Square square)
    {
        return new Vector2(square.col, square.row);
    }

    public List<Item> GetRow(int row)
    {
        List<Item> itemsList = new List<Item>();
        for (int col = 0; col < maxCols; col++)
        {
            itemsList.Add(GetSquare(col, row, true).item);
        }
        return itemsList;
    }

    public List<Item> GetColumn(int col)
    {
        List<Item> itemsList = new List<Item>();
        for (int row = 0; row < maxRows; row++)
        {
            itemsList.Add(GetSquare(col, row, true).item);
        }
        return itemsList;
    }

    public List<Square> GetColumnSquaresObstacles(int col)
    {
        List<Square> itemsList = new List<Square>();
        for (int row = 0; row < maxRows; row++)
        {
            if (GetSquare(col, row, true).IsHaveDestroybleObstacle())
                itemsList.Add(GetSquare(col, row, true));
        }
        return itemsList;
    }

    public List<Square> GetRowSquaresObstacles(int row)
    {
        List<Square> itemsList = new List<Square>();
        for (int col = 0; col < maxCols; col++)
        {
            if (GetSquare(col, row, true).IsHaveDestroybleObstacle())
                itemsList.Add(GetSquare(col, row, true));
        }
        return itemsList;
    }

    public List<Item> GetItemsAround(Square square)
    {
        int col = square.col;
        int row = square.row;
        List<Item> itemsList = new List<Item>();
        for (int r = row - 1; r <= row + 1; r++)
        {
            for (int c = col - 1; c <= col + 1; c++)
            {
                itemsList.Add(GetSquare(c, r, true).item);
            }
        }
        return itemsList;
    }

    public List<Item> GetItemsCross(Square square, List<Item> exceptList = null, int COLOR = -1)
    {
        if (exceptList == null)
            exceptList = new List<Item>();
        int c = square.col;
        int r = square.row;
        List<Item> itemsList = new List<Item>();
        Item item = null;
        item = GetSquare(c - 1, r, true).item;
        if (exceptList.IndexOf(item) <= -1)
        {
            if (item.color == COLOR || COLOR == -1)
                itemsList.Add(item);
        }
        item = GetSquare(c + 1, r, true).item;
        if (exceptList.IndexOf(item) <= -1)
        {
            if (item.color == COLOR || COLOR == -1)
                itemsList.Add(item);
        }
        item = GetSquare(c, r - 1, true).item;
        if (exceptList.IndexOf(item) <= -1)
        {
            if (item.color == COLOR || COLOR == -1)
                itemsList.Add(item);
        }
        item = GetSquare(c, r + 1, true).item;
        if (exceptList.IndexOf(item) <= -1)
        {
            if (item.color == COLOR || COLOR == -1)
                itemsList.Add(item);
        }

        return itemsList;
    }

    public List<Item> GetItems()
    {
        List<Item> itemsList = new List<Item>();
        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxCols; col++)
            {
                if (GetSquare(col, row) != null)
                    itemsList.Add(GetSquare(col, row, true).item);
            }
        }
        return itemsList;
    }

    public void SetTypeByColor(int p, ItemsTypes nextType)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            if (item.GetComponent<Item>().color == p)
            {
                if (nextType == ItemsTypes.HORIZONTAL_STRIPPED || nextType == ItemsTypes.VERTICAL_STRIPPED)
                    item.GetComponent<Item>().NextType = (ItemsTypes)UnityEngine.Random.Range(1, 3);
                else
                    item.GetComponent<Item>().NextType = nextType;

                item.GetComponent<Item>().ChangeType();
                if (nextType == ItemsTypes.NONE)
                    destroyAnyway.Add(item.GetComponent<Item>());
            }
        }
    }

    public void CheckIngredient()
    {
        int row = maxRows;
        List<Square> sqList = GetBottomRow();
        foreach (Square sq in sqList)
        {
            if (sq.item != null)
            {
                if (sq.item.currentType == ItemsTypes.INGREDIENT || sq.item.currentType == ItemsTypes.ANAMASRY)
                {
                    destroyAnyway.Add(sq.item);
                }
            }
        }
    }

    public List<Square> GetBottomRow()
    {
        List<Square> itemsList = new List<Square>();
        int listCounter = 0;
        for (int col = 0; col < maxCols; col++)
        {
            for (int row = maxRows - 1; row >= 0; row--)
            {
                Square square = GetSquare(col, row, true);
                if (square.type != SquareTypes.NONE)
                {
                    itemsList.Add(square);
                    listCounter++;
                    break;
                }
            }
        }
        return itemsList;
    }

    public void StrippedShow(GameObject obj, bool horrizontal)
    {
        GameObject effect = Instantiate(stripesEffect, obj.transform.position, Quaternion.identity) as GameObject;
        if (!horrizontal)
            effect.transform.Rotate(Vector3.back, 90);
        Destroy(effect, 1);
    }

    public void PopupScore(int value, Vector3 pos, int color)
    {
        Score += value;
        UpdateBar();
        CheckStars();
        if (showPopupScores)
        {
            Transform parent = GameObject.Find("CanvasScore").transform;
            GameObject poptxt = Instantiate(popupScore, pos, Quaternion.identity) as GameObject;
            poptxt.transform.GetComponentInChildren<Text>().text = "" + value;
            if (color <= scoresColors.Length - 1)
            {
                poptxt.transform.GetComponentInChildren<Text>().color = scoresColors[color];
                poptxt.transform.GetComponentInChildren<Outline>().effectColor = scoresColorsOutline[color];
            }
            poptxt.transform.SetParent(parent);
            //   poptxt.transform.position += Vector3.right * 1;
            poptxt.transform.localScale = Vector3.one / 1.5f;
            Destroy(poptxt, 0.3f);
        }
    }
    void UpdateBar()
    {
        ProgressBarScript.Instance.UpdateDisplay((float)Score * 100f / ((float)star1 / ((star1 * 100f / star3)) * 100f) / 100f);
    }

    void CheckStars()
    {
        if (Score >= star1 && stars <= 0)
        {
            stars = 1;
        }
        if (Score >= star2 && stars <= 1)
        {
            stars = 2;
        }
        if (Score >= star3 && stars <= 2)
        {
            stars = 3;
        }

        if (Score >= star1)
        {
            if (!star1Anim.activeSelf)
                SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.getStarIngr);
            star1Anim.SetActive(true);
        }
        if (Score >= star2)
        {
            if (!star2Anim.activeSelf)
                SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.getStarIngr);
            star2Anim.SetActive(true);
        }
        if (Score >= star3)
        {
            if (!star3Anim.activeSelf)
                SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.getStarIngr);
            star3Anim.SetActive(true);
        }
    }
    public void LoadDataFromLocal(int currentLevel)
    {
        levelLoaded = false;
        //Read data from text file
        TextAsset mapText = Resources.Load("Levels/" + currentLevel) as TextAsset;
        if (mapText == null)
        {
            mapText = Resources.Load("Levels/" + currentLevel) as TextAsset;
        }
        if (SceneManager.GetActiveScene().name == "DreamLevel")
        {
            mapText = Resources.Load("DreamLevels/" + currentLevel) as TextAsset;
        }
        ProcessGameDataFromString(mapText.text);
    }

    void ProcessGameDataFromString(string mapText)
    {
        string[] lines = mapText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int mapLine = 0;
        foreach (string line in lines)
        {
            //check if line is game mode line
            if (line.StartsWith("MODE"))
            {
                //Replace GM to get mode number, 
                string modeString = line.Replace("MODE", string.Empty).Trim();
                //then parse it to interger
                target = (Target)int.Parse(modeString);
                //Assign game mode
            }
            else if (line.StartsWith("SIZE "))
            {
                string blocksString = line.Replace("SIZE", string.Empty).Trim();
                string[] sizes = blocksString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                maxCols = int.Parse(sizes[0]);
                maxRows = int.Parse(sizes[1]);
                squaresArray = new Square[maxCols * maxRows];
                levelSquaresFile = new SquareBlocks[maxRows * maxCols];
                for (int i = 0; i < levelSquaresFile.Length; i++)
                {

                    SquareBlocks sqBlocks = new SquareBlocks();
                    sqBlocks.block = SquareTypes.EMPTY;
                    sqBlocks.obstacle = SquareTypes.NONE;

                    levelSquaresFile[i] = sqBlocks;
                }
            }
            else if (line.StartsWith("LIMIT"))
            {
                string blocksString = line.Replace("LIMIT", string.Empty).Trim();
                string[] sizes = blocksString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                limitType = (LIMIT)int.Parse(sizes[0]);
                Limit = int.Parse(sizes[1]);
            }
            else if (line.StartsWith("COLOR LIMIT "))
            {
                string blocksString = line.Replace("COLOR LIMIT", string.Empty).Trim();
                colorLimit = int.Parse(blocksString);
            }

            //check third line to get missions
            else if (line.StartsWith("STARS"))
            {
                string blocksString = line.Replace("STARS", string.Empty).Trim();
                string[] blocksNumbers = blocksString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                star1 = int.Parse(blocksNumbers[0]);
                star2 = int.Parse(blocksNumbers[1]);
                star3 = int.Parse(blocksNumbers[2]);
                if (ProgressBarScript.Instance != null)//2.1.2
                    ProgressBarScript.Instance.InitBar();//2.1.2
            }
            else if (line.StartsWith("COLLECT COUNT "))
            {
                string blocksString = line.Replace("COLLECT COUNT", string.Empty).Trim();
                string[] blocksNumbers = blocksString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (target == Target.AnaMasry)
                {
                    for (int i = 0; i < blocksNumbers.Length; i++)
                    {
                        Debug.Log("sssssss");
                        AnaMasryCountTarget[i] = int.Parse(blocksNumbers[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < blocksNumbers.Length; i++)
                    {
                        ingrCountTarget[i] = int.Parse(blocksNumbers[i]);
                    }


                }
            }

            else if (line.StartsWith("COLLECT ITEMS "))
            {
                string blocksString = line.Replace("COLLECT ITEMS", string.Empty).Trim();
                string[] blocksNumbers = blocksString.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < blocksNumbers.Length; i++)
                {
                    Debug.Log("Blocksnumber: " + blocksNumbers[i]);

                    if (target == Target.INGREDIENT)
                        ingrTarget[i] = (Ingredients)int.Parse(blocksNumbers[i]);
                    else if (target == Target.COLLECT)
                        collectItems[i] = (CollectItems)int.Parse(blocksNumbers[i]);
                    else if (target == Target.AnaMasry)
                    {
                        trip[i] = (ANAMASRY)int.Parse(blocksNumbers[i]);
                        Debug.Log(trip[i]);
                    }
                }
            }
            else
            { //Maps
              //Split lines again to get map numbers
                string[] st = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < st.Length; i++)
                {
                    levelSquaresFile[mapLine * maxCols + i].block = (SquareTypes)int.Parse(st[i][0].ToString());
                    levelSquaresFile[mapLine * maxCols + i].obstacle = (SquareTypes)int.Parse(st[i][1].ToString());
                }
                mapLine++;
            }
        }
        levelLoaded = true;
    }

}

[System.Serializable]
public class GemProduct
{
    public int count;
    public float price;
}