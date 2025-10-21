using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif





public class AnimationManager : MonoBehaviour
{
    public bool PlayOnEnable = true;
    bool WaitForPickupFriends;
    public static int GlobalLevelCount = 0;
    bool WaitForAksFriends;
    System.Collections.Generic.Dictionary<string, string> parameters;
    private void Awake()
    {
        //  Application.logMessageReceived += ExceptionHandler.This.HandleException;
    }
    void OnEnable()
    {
        if (PlayOnEnable)
        {
            //SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.swish[0]);

            //if( !GetComponent<SequencePlayer>().sequenceArray[0].isPlaying )
            //    GetComponent<SequencePlayer>().Play();
        }
        if (name == "StartGameMenu")
        {
            for (int i = 1; i <= 3; i++)
            {
                transform.Find("Image").Find("Star" + i).gameObject.SetActive(false);
            }
            int stars = PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", OpenLevel.Instance.curruntLevel), 0);
            // Debug.Log(LevelManager.THIS.currentLevel);
            if (stars > 0 && stars <= 3)
            {
                Debug.Log("wtf");
                for (int i = 1; i <= stars; i++)
                {
                    transform.Find("Image").Find("Star" + i).gameObject.SetActive(true);
                }

            }
            else
            {
                for (int i = 1; i <= 3; i++)
                {
                    transform.Find("Image").Find("Star" + i).gameObject.SetActive(false);
                }

            }
        }

        if (name == "PrePlay")
        {
            // GameObject
        }
        if (name == "PreFailed")
        {
            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.gameOver[0]);
            transform.Find("Buy").GetComponent<Button>().interactable = true;
            transform.Find("Video").gameObject.SetActive(false);

            GetComponent<Animation>().Play();
        }

        if (name == "Settings" || name == "MenuPause")
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                transform.Find("Image/SoundOff").gameObject.SetActive(true);
                transform.Find("Image/SoundOn").gameObject.SetActive(false);
            }
            else
            {
                transform.Find("Image/SoundOn").gameObject.SetActive(true);
                transform.Find("Image/SoundOff").gameObject.SetActive(false);
            }
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                transform.Find("Image/MusicOn").gameObject.SetActive(false);
                transform.Find("Image/MusicOff").gameObject.SetActive(true);
            }
            else
            {
                transform.Find("Image/MusicOn").gameObject.SetActive(true);
                transform.Find("Image/MusicOff").gameObject.SetActive(false);
            }
        }

        if (name == "GemsShop")
        {
            for (int i = 1; i <= 4; i++)
            {
                transform.Find("Image/Pack" + i + "/Count").GetComponent<Text>().text = "" + LevelManager.THIS.gemsProducts[i - 1].count;
                transform.Find("Image/Pack" + i + "/Buy/Price").GetComponent<Text>().text = "" + LevelManager.THIS.gemsProducts[i - 1].price;
            }
        }
        if (name == "WinMenu")
        {
            for (int i = 1; i <= 3; i++)
            {
                transform.Find("Image").Find("Star" + i).gameObject.SetActive(false);
            }

        }
        if (transform.Find("Image/Video") != null)
        {
#if UNITY_ADS
			InitScript.Instance.rewardedVideoZone = "rewardedVideo";

			if (!InitScript.Instance.enableUnityAds || !InitScript.Instance.GetRewardedUnityAdsReady ())
				transform.Find ("Image/Video").gameObject.SetActive (false);
#else
            transform.Find("Image/Video").gameObject.SetActive(false);
#endif
        }

    }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (name == "MenuPlay" || name == "Settings" || name == "BoostInfo" || name == "GemsShop" || name == "LiveShop" || name == "BoostShop" || name == "Reward")
                CloseMenu();
        }
    }

    public void ShowAds()
    {
        if (name == "GemsShop")
            InitScript.Instance.currentReward = RewardedAdsType.GetGems;
        else if (name == "LiveShop")
            InitScript.Instance.currentReward = RewardedAdsType.GetLifes;
        else if (name == "PreFailed")
            InitScript.Instance.currentReward = RewardedAdsType.GetGoOn;
        InitScript.Instance.ShowRewardedAds();
        CloseMenu();
    }

    public void GoRate()
    {
        Application.OpenURL(InitScript.Instance.RateURL);
        PlayerPrefs.SetInt("Rated", 1);
        PlayerPrefs.Save();
    }

    void OnDisable()
    {
        if (transform.Find("Image/Video") != null)
        {
            transform.Find("Image/Video").gameObject.SetActive(true);
        }

        //if( PlayOnEnable )
        //{
        //    if( !GetComponent<SequencePlayer>().sequenceArray[0].isPlaying )
        //        GetComponent<SequencePlayer>().sequenceArray[0].Play
        //}
    }

    public void OnFinished()
    {
        Debug.Log("OnFinished");
        if (name == "WinMenu")
        {
            Debug.Log("starts");
            StartCoroutine(MenuComplete());
            StartCoroutine(MenuCompleteScoring());
        }
        if (name == "MenuPlay")
        {
            //InitScript.Instance.currentTarget = InitScript.Instance.targets[PlayerPrefs.GetInt("OpenLevel")];
            transform.Find("Image/Boost1").GetComponent<BoostIcon>().InitBoost();
            transform.Find("Image/Boost2").GetComponent<BoostIcon>().InitBoost();
            transform.Find("Image/Boost3").GetComponent<BoostIcon>().InitBoost();

        }
        if (name == "MenuPause")
        {
            //if (LevelManager.THIS.gameStatus == GameState.Playing)
            //    LevelManager.THIS.gameStatus = GameState.Pause;
        }

        if (name == "MenuFailed")
        {
            if (LevelManager.Score < LevelManager.THIS.star1)
            {
                TargetCheck(false, 2);
            }
            else
            {
                TargetCheck(true, 2);
            }
            if (LevelManager.THIS.target == Target.BLOCKS)
            {
                if (LevelManager.THIS.TargetBlocks > 0)
                {
                    TargetCheck(false, 1);
                }
                else
                {
                    TargetCheck(true, 1);
                }
            }
            else if (LevelManager.THIS.target == Target.INGREDIENT || LevelManager.THIS.target == Target.COLLECT)
            {
                if (LevelManager.THIS.ingrCountTarget[0] > 0 || LevelManager.THIS.ingrCountTarget[1] > 0)
                {
                    TargetCheck(false, 1);
                }
                else
                {
                    TargetCheck(true, 1);
                }
            }
            else if (LevelManager.THIS.target == Target.SCORE)
            {
                if (LevelManager.Score < LevelManager.THIS.star1)
                {
                    TargetCheck(false, 1);
                }
                else
                {
                    TargetCheck(true, 1);
                }
            }


        }
        if (name == "PrePlay")
        {
            CloseMenu();
            LevelManager.THIS.gameStatus = GameState.WaitForPopup;

        }
        if (name == "PreFailed")
        {
            if (LevelManager.THIS.Limit <= 0)
                LevelManager.THIS.gameStatus = GameState.GameOver;
            transform.Find("Video").gameObject.SetActive(false);

            CloseMenu();

        }

        if (name.Contains("gratzWord"))
            gameObject.SetActive(false);
        if (name == "NoMoreMatches")
            gameObject.SetActive(false);
        if (name == "CompleteLabel")
            gameObject.SetActive(false);

    }

    void TargetCheck(bool check, int n = 1)
    {
        Transform TargetCheck = transform.Find("Image/TargetCheck" + n);
        Transform TargetUnCheck = transform.Find("Image/TargetUnCheck" + n);
        TargetCheck.gameObject.SetActive(check);
        TargetUnCheck.gameObject.SetActive(!check);
    }

    public void WaitForGiveUp()
    {
        if (name == "PreFailed")
        {
            GetComponent<Animation>()["bannerFailed"].speed = 0;
#if UNITY_ADS

			if (InitScript.Instance.enableUnityAds) {

				if (InitScript.Instance.GetRewardedUnityAdsReady ()) {
					transform.Find ("Video").gameObject.SetActive (true);
				}
			}
#endif
        }
    }
    public void BuyCodes(int price)
    {
        if (price <= PlayerPrefs.GetInt("Coins"))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - price);
        }
    }
    IEnumerator MenuComplete()
    {
        for (int i = 1; i <= LocalGameManager.Instance.stars; i++)
        {
            Debug.Log("Stars::::: " + LocalGameManager.Instance.stars);
            //SoundBase.Instance.audio.PlayOneShot(SoundBase.Instance.scoringStar);
            transform.Find("Image").Find("Star" + i).gameObject.SetActive(true);
            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.star[i - 1]);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator MenuCompleteScoring()
    {
        Text scores = transform.Find("Image").Find("Score").GetComponent<Text>();
        for (int i = 0; i <= LocalGameManager.Instance.Score; i += 10)
        {
            scores.text = "" + i;
            // SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.scoring );
            yield return new WaitForSeconds(0.00001f);
        }
        scores.text = "" + LocalGameManager.Instance.Score;
    }

    public void Info()
    {
        GameObject.Find("CanvasGlobal").transform.Find("Tutorial").gameObject.SetActive(true);
        CloseMenu();
    }



    public void PlaySoundButton()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);

    }

    public IEnumerator Close()
    {
        yield return new WaitForSeconds(0.5f);
        LevelsMap._instance.IsClickEnabled = true;
    }

    public void CloseMenu()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        LevelsMap._instance.IsClickEnabled = true;
        if (gameObject.name == "MenuPreGameOver")
        {
            ShowGameOver();
        }
        if (gameObject.name == "MenuComplete")
        {
            if (SceneManager.GetActiveScene().name != "DreamLevel")
            {

                /* Debug.Log("Current Level After Increment: " + LevelManager.THIS.currentLevel);
                 Debug.Log("Target: " + LevelManager.THIS.target);
                 for (int i = 0; i < 50; i++)
                 {
                     LevelManager.THIS.currentLevel++;
                     LevelManager.THIS.target = CheckTarget(LevelManager.THIS.currentLevel);
                     if (LevelManager.THIS.target != Target.AnaMasry)
                     {
                         GlobalLevelCount = i;
                         break;
                     }

                 }*/
                LevelManager.THIS.currentLevel++;
                if (LevelsMap.IsLevelLocked(LevelManager.THIS.currentLevel))
                {
                    LevelManager.THIS.gameStatus = GameState.Map;
#if UNITY_ANDROID
                    if (Application.platform == RuntimePlatform.Android)
                        AndroidNativePopups.OpenToast("You must get 45 stars", AndroidNativePopups.ToastDuration.Short);

#endif 
                }
                else
                {
                    PlayerPrefs.SetInt("OpenLevel", LevelManager.THIS.currentLevel);
                    LevelManager.THIS.PrepareGame();
                    LevelManager.THIS.gameStatus = GameState.Map;
                    Debug.Log("Open new level");
                    GameObject.Find("CanvasGlobal").transform.Find("MenuPlay").gameObject.SetActive(true);
                    if (LevelManager.THIS.currentLevel == 101)
                    {
                        SceneManager.LoadScene("NewYork");
                    }
                    else if (LevelManager.THIS.currentLevel == 201)
                    {
                        SceneManager.LoadScene("London");
                    }
                    else if (LevelManager.THIS.currentLevel == 301)
                    {
                        SceneManager.LoadScene("Paris");
                    }
                    else if (LevelManager.THIS.currentLevel == 401)
                    {
                        SceneManager.LoadScene("Rome");
                    }
                    else if (LevelManager.THIS.currentLevel == 501)
                    {
                        SceneManager.LoadScene("Spain");
                    }
                    else if (LevelManager.THIS.currentLevel == 601)
                    {
                        SceneManager.LoadScene("game");
                    }
                    Debug.Log("Current Level: " + PlayerPrefs.GetInt("OpenLevel"));
                    Debug.Log(LevelManager.THIS.target);
                }
            }
        }
        if (gameObject.name == "MenuFailed")
        {
            if (SceneManager.GetActiveScene().name != "DreamLevel")
            {
                if (InitScript.lifes > 0)
                {
                    LevelManager.THIS.gameStatus = GameState.PrepareGame;
                }
                else
                {
                    if (LevelManager.THIS.currentLevel <= 100)
                    {
                        Debug.Log("Egypt map! ");
                        SceneManager.LoadScene("game");
                    }
                    else if (LevelManager.THIS.currentLevel > 100 && LevelManager.THIS.currentLevel <= 200)
                    {
                        SceneManager.LoadScene("NewYork");
                    }
                    else if (LevelManager.THIS.currentLevel > 200 && LevelManager.THIS.currentLevel <= 300)
                    {
                        SceneManager.LoadScene("London");
                    }
                    else if (LevelManager.THIS.currentLevel > 300 && LevelManager.THIS.currentLevel <= 400)
                    {
                        SceneManager.LoadScene("Paris");
                    }
                    else if (LevelManager.THIS.currentLevel > 400 && LevelManager.THIS.currentLevel <= 500)
                    {
                        SceneManager.LoadScene("Rome");
                    }
                    else if (LevelManager.THIS.currentLevel > 500 && LevelManager.THIS.currentLevel <= 600)
                    {
                        SceneManager.LoadScene("Spain");
                    }
                    LevelManager.THIS.gameStatus = GameState.Map;
                }
            }
            else if (SceneManager.GetActiveScene().name == "DreamLevel")
            {
                PlayerPrefs.SetInt("OpenLevel", UnityEngine.Random.Range(0, 9));
                PlayerPrefs.Save();
                LevelManager.THIS.LoadLevel();
                if (InitScript.lifes > 0)
                {
                    InitScript.Instance.SpendDreamLives(1);
                    LevelManager.THIS.gameStatus = GameState.PrepareGame;
                }
                else
                {

                }
            }
        }

        //if (SceneManager.GetActiveScene().name != "DreamLevel")
        //{
        //    if (LevelManager.Instance.gameStatus == GameState.Pause)
        //    {
        //        LevelManager.Instance.gameStatus = GameState.WaitAfterClose;

        //    }
        //}
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.swish[1]);

        gameObject.SetActive(false);
    }
    Target CheckTarget(int currentLevel)
    {
        TextAsset mapText = Resources.Load("Levels/" + currentLevel) as TextAsset;
        if (mapText == null)
        {
            mapText = Resources.Load("Levels/" + LevelManager.THIS.currentLevel) as TextAsset;
        }
        string[] lines = mapText.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int mapLine = 0;
        foreach (string line in lines)
        {
            //check if line is game mode line
            if (line.StartsWith("MODE"))
            {
                //Replace GM to get mode number, 
                string modeString = line.Replace("MODE", string.Empty).Trim();
                //then parse it to interger
                LevelManager.THIS.target = (Target)int.Parse(modeString);
                //Assign game mode
            }
        }
        return LevelManager.THIS.target;
    }
    public void BackToGame()
    {
        if (LevelManager.THIS.currentLevel <= 100)
        {
            SceneManager.LoadScene("game");
        }
        else if (LevelManager.THIS.currentLevel > 100 && LevelManager.THIS.currentLevel <= 200)
        {
            SceneManager.LoadScene("NewYork");
        }
    }
    public void SwishSound()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.swish[1]);

    }

    public void ShowInfo()
    {
        GameObject.Find("CanvasGlobal").transform.Find("BoostInfo").gameObject.SetActive(true);

    }

    public void Play()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        if (gameObject.name == "MenuPreGameOver")
        {
            if (InitScript.Gems >= 12)
            {
                InitScript.Instance.SpendGems(12);
                //                LevelData.LimitAmount += 12;
                LevelManager.Instance.gameStatus = GameState.WaitAfterClose;
                gameObject.SetActive(false);

            }
            else
            {
                BuyGems();
            }
        }
        else if (gameObject.name == "MenuFailed")
        {
            if (SceneManager.GetActiveScene().name != "DreamLevel")
            {
                LevelManager.Instance.gameStatus = GameState.Map;

            }
        }
        else if (gameObject.name == "MenuPlay")
        {
            if (SceneManager.GetActiveScene().name != "DreamLevel")
            {
                if (InitScript.lifes > 0)
                {
                    InitScript.Instance.SpendLife(1);
                    LevelManager.THIS.gameStatus = GameState.PrepareGame;
                    CloseMenu();
                    Debug.Log("Current level: " + LevelManager.THIS.currentLevel);
                    //Application.LoadLevel( "game" );
                }
                else
                {
                    LevelManager.THIS.gameStatus = GameState.Map;
                    CloseMenu();
                }
            }

            else
            {
                BuyLifeShop();
            }

        }
        else if (gameObject.name == "MenuPause")
        {
            CloseMenu();
            LevelManager.Instance.gameStatus = GameState.Playing;
        }
    }

    public void PlayTutorial()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        LevelManager.Instance.gameStatus = GameState.Playing;
        //    mainscript.Instance.dropDownTime = Time.time + 0.5f;
        //        CloseMenu();
    }

    public void BackToMap()
    {
        Time.timeScale = 1;
        LevelManager.THIS.gameStatus = GameState.Map;
        CloseMenu();
    }

    public void Next()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        CloseMenu();
    }

    public void BuyGems()
    {

        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        GameObject.Find("CanvasGlobal").transform.Find("GemsShop").gameObject.SetActive(true);
    }

    public void Buy(GameObject pack)
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        if (pack.name == "Pack1")
        {
            InitScript.waitedPurchaseGems = int.Parse(pack.transform.Find("Count").GetComponent<Text>().text.Replace("x ", ""));
#if UNITY_WEBPLAYER || UNITY_WEBGL
            InitScript.Instance.PurchaseSucceded();
            CloseMenu();
            return;
#endif
#if UNITY_INAPPS
			UnityInAppsIntegration.THIS.BuyProductID (LevelManager.THIS.InAppIDs [0]);
#endif
        }

        if (pack.name == "Pack2")
        {
            InitScript.waitedPurchaseGems = int.Parse(pack.transform.Find("Count").GetComponent<Text>().text.Replace("x ", ""));
#if UNITY_WEBPLAYER || UNITY_WEBGL
            InitScript.Instance.PurchaseSucceded();
            CloseMenu();
            return;
#endif
#if UNITY_INAPPS
			UnityInAppsIntegration.THIS.BuyProductID (LevelManager.THIS.InAppIDs [1]);
#endif
        }
        if (pack.name == "Pack3")
        {
            InitScript.waitedPurchaseGems = int.Parse(pack.transform.Find("Count").GetComponent<Text>().text.Replace("x ", ""));
#if UNITY_WEBPLAYER || UNITY_WEBGL
            InitScript.Instance.PurchaseSucceded();
            CloseMenu();
            return;
#endif
#if UNITY_INAPPS
			UnityInAppsIntegration.THIS.BuyProductID (LevelManager.THIS.InAppIDs [2]);
#endif
        }
        if (pack.name == "Pack4")
        {
            InitScript.waitedPurchaseGems = int.Parse(pack.transform.Find("Count").GetComponent<Text>().text.Replace("x ", ""));
#if UNITY_WEBPLAYER || UNITY_WEBGL
            InitScript.Instance.PurchaseSucceded();
            CloseMenu();
            return;
#endif
#if UNITY_INAPPS
			UnityInAppsIntegration.THIS.BuyProductID (LevelManager.THIS.InAppIDs [3]);
#endif
        }
        CloseMenu();

    }

    public void BuyLifeShop()
    {

        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        if (InitScript.lifes < InitScript.Instance.CapOfLife)
            GameObject.Find("CanvasGlobal").transform.Find("LiveShop").gameObject.SetActive(true);

    }

    public void BuyLife(GameObject button)
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        if (InitScript.Gems >= int.Parse(button.transform.Find("Price").GetComponent<Text>().text))
        {
            InitScript.Instance.SpendGems(int.Parse(button.transform.Find("Price").GetComponent<Text>().text));
            InitScript.Instance.RestoreLifes();
            CloseMenu();
        }
        else
        {
            GameObject.Find("CanvasGlobal").transform.Find("GemsShop").gameObject.SetActive(true);
        }

    }

    public void BuyFailed(GameObject button)
    {
        if (GetComponent<Animation>()["bannerFailed"].speed == 0)
        {
            if (InitScript.Gems >= int.Parse(button.transform.Find("Price").GetComponent<Text>().text))
            {
                InitScript.Instance.SpendGems(int.Parse(button.transform.Find("Price").GetComponent<Text>().text));
                button.GetComponent<Button>().interactable = false;
                GoOnFailed();
            }
            else
            {
                GameObject.Find("CanvasGlobal").transform.Find("GemsShop").gameObject.SetActive(true);
            }
        }
    }

    public void GoOnFailed()
    {
        if (LevelManager.THIS.limitType == LIMIT.MOVES)
            LevelManager.THIS.Limit += LevelManager.THIS.ExtraFailedMoves;
        else
            LevelManager.THIS.Limit += LevelManager.THIS.ExtraFailedSecs;
        GetComponent<Animation>()["bannerFailed"].speed = 1;
        LevelManager.THIS.gameStatus = GameState.Playing;
    }

    public void GiveUp()
    {
        LevelManager.THIS.gameStatus = GameState.Map;
        GetComponent<Animation>()["bannerFailed"].speed = 1;

    }

    void ShowGameOver()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.gameOver[1]);

        GameObject.Find("Canvas").transform.Find("MenuGameOver").gameObject.SetActive(true);
        gameObject.SetActive(false);

    }

    #region boosts

    public void BuyBoost(BoostType boostType, int price, int count)
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        if (InitScript.Gems >= price)
        {
            InitScript.Instance.SpendGems(price);
            InitScript.Instance.BuyBoost(boostType, price, count);
            //InitScript.Instance.SpendBoost(boostType);
            CloseMenu();
        }
        else
        {
            BuyGems();
        }
    }

    #endregion

    public void SoundOff(int volume)
    {
        SoundBase.Instance.GetComponent<AudioSource>().volume = volume;
    }
    public void Save()
    {
        PlayerPrefs.SetInt("Sound", (int)SoundBase.Instance.GetComponent<AudioSource>().volume);
        PlayerPrefs.SetInt("Music", (int)GameObject.Find("Music").GetComponent<AudioSource>().volume);
        PlayerPrefs.Save();
    }
    public void MusicOff(int volume)
    {
        GameObject.Find("Music").GetComponent<AudioSource>().volume = volume;
    }

}
