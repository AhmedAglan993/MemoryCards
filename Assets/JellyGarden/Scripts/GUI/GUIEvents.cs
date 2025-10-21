using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TheNextFlow.UnityPlugins;
using System.Collections.Generic;

public class GUIEvents : MonoBehaviour
{
    bool BackToMainScreenOn;
    public GameObject image;
    public bool inTrip;
    public GameObject facebookButton;
    public Image toturial;
    public Sprite pic1, pic2, pic3;
    public GameObject leftButton, rightButton;
    string url;
    bool onBackClicked;
    Dictionary<string, string> myContacts;
    private string m_Writer;

    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // SendDomain();
        }

        Application.logMessageReceived += HandleException;
    }
    void SendDomain()
    {
        var androidJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var jo = androidJC.GetStatic<AndroidJavaObject>("currentActivity");
        // Accessing the class to call a static method on it
        var jc = new AndroidJavaClass("com.Approcks.CandyMasry.debugSystem.UncaughtExceptionHandlerActivity");
        // Calling a Call method to which the current activity is passed
        string domain = "";
        if (PlayerPrefs.HasKey("primary_domain"))
        {
            domain = PlayerPrefs.GetString("primary_domain");
        }
        else
        {
            domain = "https://maafmiinf3vfuyktk1.mg-apis.com";
        }
        jc.CallStatic("CallSetDomain", jo, domain);
    }
    private void HandleException(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            m_Writer = string.Format("{0}: {1}\n{2}\n{3}\n", type, condition, stackTrace, SystemInfo.deviceModel, SystemInfo.graphicsDeviceType);
            Debug.Log("Exception! ");
        }
    }

    IEnumerator DownloadPopup()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int timeStamp = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        // long timeStamp = (long)System.DateTime.Now.TimeOfDay.TotalSeconds;
        Debug.Log(timeStamp);
        onBackClicked = false;
        // PlayerPrefs.DeleteKey("timeStamp");

        if (!PlayerPrefs.HasKey("timeStamp"))
        {
            Debug.Log("time Stamp Not Here");

            PlayerPrefs.SetInt("timeStamp", 0);
            PlayerPrefs2.SetBool("app_on_store", false);
        }
        if (PlayerPrefs.HasKey("timeStamp"))
        {
            Debug.Log("timeStampHere: " + PlayerPrefs.GetInt("timeStamp"));
            int timeToShow = timeStamp - PlayerPrefs.GetInt("timeStamp");
            Debug.Log("time to show: " + timeToShow);
            if (timeToShow >= (12 * 60 * 60))
            {
                POST3("https://maafmiinf3vfuyktk1.mg-apis.com/apis/v1/getGameEnvs", timeStamp);
            }
        }
        WWW wwwImage = new WWW(PlayerPrefs.GetString("message"));
        yield return wwwImage;

        transform.Find("Download").gameObject.transform.Find("Image").transform.Find("Message").GetComponent<Image>().sprite = Sprite.Create(wwwImage.texture, new Rect(0, 0, wwwImage.texture.width, wwwImage.texture.height), new Vector2(0, 0));

    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "main")
        {
            StartCoroutine(DownloadPopup());
        }
        myContacts = new Dictionary<string, string>();
        if (SceneManager.GetActiveScene().name == "game")
        {
            LevelManager.THIS.PassedLevels = 0;
            Debug.Log("Passed Levels" + LevelManager.THIS.PassedLevels);
        }

        if (SceneManager.GetActiveScene().name == "NewYork")
        {
            LevelManager.THIS.PassedLevels = 100;
            Debug.Log("Passed Levels" + LevelManager.THIS.PassedLevels);
        }

        if (SceneManager.GetActiveScene().name == "London")
        {
            LevelManager.THIS.PassedLevels = 200;
            Debug.Log("Passed Levels" + LevelManager.THIS.PassedLevels);
        }
        if (SceneManager.GetActiveScene().name == "Paris")
        {
            LevelManager.THIS.PassedLevels = 300;
            Debug.Log("Passed Levels" + LevelManager.THIS.PassedLevels);
        }
        if (SceneManager.GetActiveScene().name == "Rome")
        {
            LevelManager.THIS.PassedLevels = 400;
            Debug.Log("Passed Levels" + LevelManager.THIS.PassedLevels);
        }
        if (SceneManager.GetActiveScene().name == "Spain")
        {
            LevelManager.THIS.PassedLevels = 500;
            Debug.Log("Passed Levels" + LevelManager.THIS.PassedLevels);
        }
        Debug.Log(PlayerPrefs.GetInt("PassedLevels"));
    }
    public void Continue()
    {
        ShowDownloadPopup(false);
    }
    public void AppStore()
    {
        Application.OpenURL("market://details?id=com.approcks.masry");
    }
    void ShowDownloadPopup(bool show)
    {
        if (show)
        {
            transform.Find("LevelChooser").gameObject.SetActive(false);
            transform.Find("Download").gameObject.SetActive(true);

            //var systemLanguage = SystemLanguage.Arabic;
            Debug.Log(Application.systemLanguage.ToString());

        }
        else
        {
            transform.Find("LevelChooser").gameObject.SetActive(true);
            transform.Find("Download").gameObject.SetActive(false);
        }
    }
    void LeveLChooserButtons()
    {
        if (PlayerPrefs.GetInt("PassedLevels") >= 100)
        {
            Debug.Log("sdsdshdsjdsj");
            image.transform.Find("New York").gameObject.SetActive(false);
            image.transform.Find("New York open").gameObject.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("PassedLevels") >= 200)
        {
            Debug.Log("sdsdshdsjdsj");
            image.transform.Find("London").gameObject.SetActive(false);
            image.transform.Find("London Open").gameObject.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("PassedLevels") >= 300)
        {
            Debug.Log("sdsdshdsjdsj");
            image.transform.Find("Paris").gameObject.SetActive(false);
            image.transform.Find("Paris Open").gameObject.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("PassedLevels") >= 400)
        {
            Debug.Log("sdsdshdsjdsj");
            image.transform.Find("Rome").gameObject.SetActive(false);
            image.transform.Find("Rome Open").gameObject.gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("PassedLevels") >= 500)
        {
            Debug.Log(image.transform.Find("Egypt").transform.GetChild(0).name);
            image.transform.Find("Spain").gameObject.SetActive(false);
            image.transform.Find("Spain Open").gameObject.gameObject.SetActive(true);
        }
        if ((PlayerPrefs.GetInt("PassedLevels") >= 100 && PlayerPrefs.GetInt("PassedLevels") < 200))
        {
            image.transform.Find("Egypt").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("New York open").transform.GetChild(0).gameObject.SetActive(true);
            image.transform.Find("London Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Paris Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Rome Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Spain Open").transform.GetChild(0).gameObject.SetActive(false);
        }
        if ((PlayerPrefs.GetInt("PassedLevels") >= 200 && PlayerPrefs.GetInt("PassedLevels") < 300))
        {
            image.transform.Find("Egypt").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("New York open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("London Open").transform.GetChild(0).gameObject.SetActive(true);
            image.transform.Find("Paris Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Rome Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Spain Open").transform.GetChild(0).gameObject.SetActive(false);
        }
        if ((PlayerPrefs.GetInt("PassedLevels") >= 300 && PlayerPrefs.GetInt("PassedLevels") < 400))
        {
            image.transform.Find("Egypt").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("New York open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("London Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Paris Open").transform.GetChild(0).gameObject.SetActive(true);
            image.transform.Find("Rome Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Spain Open").transform.GetChild(0).gameObject.SetActive(false);
        }
        if ((PlayerPrefs.GetInt("PassedLevels") >= 400 && PlayerPrefs.GetInt("PassedLevels") < 500))
        {
            image.transform.Find("Egypt").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("New York open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("London Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Paris Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Rome Open").transform.GetChild(0).gameObject.SetActive(true);
            image.transform.Find("Spain Open").transform.GetChild(0).gameObject.SetActive(false);
        }
        if ((PlayerPrefs.GetInt("PassedLevels") >= 500) || (PlayerPrefs.GetInt("PassedLevelsOfline") >= 500))
        {

            image.transform.Find("Egypt").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("New York open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("London Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Paris Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Rome Open").transform.GetChild(0).gameObject.SetActive(false);
            image.transform.Find("Spain Open").transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    void Update()
    {

        if (name == "FaceBook" || name == "Share" || name == "FaceBookLogout")
        {
            if (!LevelManager.THIS.FacebookEnable)
                gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (transform.Find("Activate").gameObject.activeSelf == true)
            {
                if (!onBackClicked)
                {
                    transform.Find("Activate").gameObject.SetActive(false);
                    LevelsMap._instance.IsClickEnabled = true;

                    onBackClicked = true;
                }
            }

            else if (transform.Find("ActivateToturial").gameObject.activeSelf == true)
            {
                if (!onBackClicked)
                {
                    transform.Find("ActivateToturial").gameObject.SetActive(false);
                    transform.Find("Activate").gameObject.SetActive(true);
                    onBackClicked = true;
                }
            }
            if (SceneManager.GetActiveScene().name != "main")
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (GameObject.Find("CanvasGlobal").transform.Find("Promo Code Store").gameObject.activeSelf == true)
                    {
                        if (!onBackClicked)
                        {
                            GameObject.Find("CanvasGlobal").transform.Find("Promo Code Store").gameObject.SetActive(false);
                            LevelsMap._instance.IsClickEnabled = true;

                            onBackClicked = true;
                        }
                    }
                }


            }

            if (SceneManager.GetActiveScene().name == "main")
            {
                if (transform.Find("LevelChooser").gameObject.activeSelf == true)
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {

                        if (!onBackClicked)
                        {
                            transform.Find("LevelChooser").gameObject.SetActive(false);
                            // LevelsMap._instance.IsClickEnabled = true;

                            onBackClicked = true;
                        }
                    }
                }
            }


        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            onBackClicked = false;
            if (SceneManager.GetActiveScene().name != "main")
            {
                LevelsMap._instance.IsClickEnabled = true;
            }

        }
    }

    public void Settings()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);

        GameObject.Find("CanvasGlobal").transform.Find("Settings").gameObject.SetActive(true);
        LevelsMap._instance.IsClickEnabled = false;

    }
    public WWW POST(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
        return www;
    }
    public GameObject progress, buttonPrefab, list;
    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            Stories myObject = new Stories();
            myObject = JsonUtility.FromJson<Stories>(www.text);
            Debug.Log("WWW Ok!: " + www.text);
            bool success = myObject.success;
            List<int> trips = new List<int>();
            List<int> price = new List<int>();
            List<string> expired_at = new List<string>();
            List<string> code_id = new List<string>();
            List<int> max_users = new List<int>();
            List<int> count = new List<int>();
            List<int> percentage = new List<int>();
            List<GameObject> circles = new List<GameObject>();
            List<GameObject> prices = new List<GameObject>();
            List<GameObject> tripCount = new List<GameObject>();
            List<GameObject> Expired = new List<GameObject>();
            if (www.isDone)
            {
                progress.SetActive(false);
                for (int i = 0; i < myObject.codes.Count; i++)
                {
                    Debug.Log(i);
                    button.Add(Instantiate(buttonPrefab) as GameObject);
                    button[i].transform.SetParent(list.transform, false);
                    trips.Add(myObject.codes[i].trips);
                    price.Add(myObject.codes[i].coins);
                    count.Add(myObject.codes[i].count);
                    percentage.Add(myObject.codes[i].percentage);
                    code_id.Add(myObject.codes[i].code_id);
                    max_users.Add(myObject.codes[i].max_users);
                    expired_at.Add(myObject.codes[i].expired_at);
                    circles.Add(button[i].transform.GetChild(0).gameObject);
                    prices.Add(button[i].transform.GetChild(1).gameObject);
                    tripCount.Add(button[i].transform.GetChild(2).gameObject);
                    Expired.Add(button[i].transform.GetChild(3).gameObject);
                    tripCount[i].gameObject.GetComponent<Text>().text = trips[i] + "trips";
                    Expired[i].gameObject.GetComponent<Text>().text = "Expired at" + expired_at[i];
                    prices[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = "" + price[i];
                    circles[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = percentage[i] + "%";
                    Button b = prices[i].GetComponent<Button>();
                    AddBuyListener(b, code_id[i]);
                }
                if (success)
                {

                }
            }
        }
    }
    public WWW POST2(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest2(www));
        return www;
    }
    private IEnumerator WaitForRequest2(WWW www)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            Stories myObject = new Stories();
            myObject = JsonUtility.FromJson<Stories>(www.text);
            Debug.Log("WWW Ok!: " + www.text);
            int coins = myObject.coins;
            bool success = myObject.success;
            if (www.isDone)
            {
                if (success)
                {
                    PlayerPrefs.SetInt("Coins", coins);
#if UNITY_ANDROID
                    if (Application.platform == RuntimePlatform.Android)
                        AndroidNativePopups.OpenToast("You successfully bought this promo code", AndroidNativePopups.ToastDuration.Long);
# endif
                }
                else
                {
#if UNITY_ANDROID
                    if (Application.platform == RuntimePlatform.Android)
                        AndroidNativePopups.OpenToast("You don't have enough coins", AndroidNativePopups.ToastDuration.Long);
# endif

                }
            }
        }
    }
    public WWW POST3(string url, int timeStamp)
    {
        WWWForm form = new WWWForm();
        form.AddField("Nothing", 1);
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest3(www, timeStamp));
        return www;
    }
    private IEnumerator WaitForRequest3(WWW www, int timeStamp)
    {
        yield return www;
        // check for errors
        if (www.error == null)
        {
            Stories myObject = new Stories();
            myObject = JsonUtility.FromJson<Stories>(www.text);
            Debug.Log("WWW Ok!: " + www.text);
            bool success = myObject.success;
            bool app_on_store = myObject.app_on_store;
            string message = myObject.message;
            if (success)
            {
                PlayerPrefs.SetInt("timeStamp", timeStamp);
                PlayerPrefs2.SetBool("app_on_store", app_on_store);
                PlayerPrefs.SetString("message", "https://i.imgur.com/i5u7YUY.png");
            }
        }
    }
    List<GameObject> button = new List<GameObject>();
    public void ClosePromoCodeMenu()
    {
        for (int i = 0; i < list.transform.childCount; i++)
        {
            Destroy(button[i]);
        }
        button.Clear();
        progress.SetActive(true);
    }
    public void AddBuyListener(Button b, string code_id)
    {
        b.onClick.AddListener(() => BuyCodes(code_id));
    }
    public void BuyCodes(string code_id)
    {
        Dictionary<string, string> post = new Dictionary<string, string>();
        post.Add("user_id", PlayerPrefs.GetString("user_id"));
        post.Add("token", PlayerPrefs.GetString("token"));
        Debug.Log(code_id);
        post.Add("code_id", code_id);
        string url = PlayerPrefs.GetString("primary_domain") + "/apis/v5/buyCode";
        POST2(url, post);
    }
    public void Store()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet connection");
#if UNITY_ANDROID

            if (Application.platform == RuntimePlatform.Android)
                AndroidNativePopups.OpenToast("No internet connection", AndroidNativePopups.ToastDuration.Short);
#endif
        }

        else
        {
            Debug.Log("First");
            GameObject.Find("CanvasGlobal").transform.Find("Promo Code Store").gameObject.SetActive(true);
            Debug.Log("Second");
            LevelsMap._instance.IsClickEnabled = false;
            Debug.Log("Third");
            url = PlayerPrefs.GetString("primary_domain") + "/apis/v5/getStoreCodes";
            Debug.Log("URL: " + url);
            myContacts.Add("user_id", PlayerPrefs.GetString("user_id"));
            myContacts.Add("token", PlayerPrefs.GetString("token"));
            POST(url, myContacts);
            Debug.Log("ToServer");
            myContacts.Remove("user_id");
            myContacts.Remove("token");
        }
    }
    public void GoToRight()
    {
        leftButton.SetActive(true);
        if (toturial.GetComponent<Image>().sprite == pic1)
        {
            toturial.GetComponent<Image>().sprite = pic2;
        }
        else if (toturial.GetComponent<Image>().sprite == pic2)
        {
            toturial.GetComponent<Image>().sprite = pic3;
            rightButton.SetActive(false);

        }

    }
    public void GoToLeft()
    {
        rightButton.SetActive(true);


        if (toturial.GetComponent<Image>().sprite == pic2)
        {
            toturial.GetComponent<Image>().sprite = pic1;
            leftButton.SetActive(false);

        }
        else if (toturial.GetComponent<Image>().sprite == pic3)
        {
            toturial.GetComponent<Image>().sprite = pic2;

        }

    }
    public void Exit()
    {
        transform.Find("ActivateToturial").gameObject.SetActive(false);
        transform.Find("Activate").gameObject.SetActive(true);
    }
    public void ExitActivation()
    {
        transform.Find("Activate").gameObject.SetActive(false);
        LevelsMap._instance.IsClickEnabled = true;

    }
    public void Play()
    {
        SceneManager.LoadScene("game");
        //SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        //if (inTrip)
        //{
        //    transform.Find("DreamLevelChooser").gameObject.SetActive(true);
        //}
        //else
        //{
        //    if (SceneManager.GetActiveScene().name == "main")
        //    {
        //        if (!PlayerPrefs.HasKey("primary_domain") && PlayerPrefs2.GetBool("app_on_store"))
        //        {
        //            ShowDownloadPopup(true);
        //        }
        //        else
        //        {
        //            Debug.Log("app_on_store false");
        //            ShowDownloadPopup(false);
        //        }
        //        LeveLChooserButtons();
        //    }
        //}
    }
    public void PlayRegular()
    {
        inTrip = false;
        transform.Find("DreamLevelChooser").gameObject.SetActive(false);
        transform.Find("LevelChooser").gameObject.SetActive(true);

        // SceneManager.LoadScene("game");

    }
    public void BackToMainScreen()
    {
        BackToMainScreenOn = true;
        SceneManager.LoadScene("main");
        Debug.Log("Back ToMain Screen On: " + BackToMainScreenOn);
    }
    public void PlayCountry(string name)
    {
        transform.Find("LevelChooser").gameObject.SetActive(false);
        transform.Find("Loading").gameObject.SetActive(true);
        SceneManager.LoadScene(name);

    }
    public void DreamLevel()
    {

        SceneManager.LoadScene("DreamLevel");

    }
    public void Pause()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);

        if (LevelManager.THIS.gameStatus == GameState.Playing)
            GameObject.Find("CanvasGlobal").transform.Find("MenuPause").gameObject.SetActive(true);

    }

    public void FaceBookLogin()
    {
#if FACEBOOK

        FacebookManager.THIS.LogIN();
#endif
    }

    public void FaceBookLogout()
    {
#if FACEBOOK
        FacebookManager.THIS.LogOut();


#endif
    }

    public void Share()
    {
#if FACEBOOK

        // FacebookManager.THIS.Share();
#endif
    }

}
