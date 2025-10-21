using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TheNextFlow.UnityPlugins;
using UnityEngine.EventSystems;

public class LevelsMap : MonoBehaviour
{
    public static LevelsMap _instance;
    private static IMapProgressManager _mapProgressManager = new PlayerPrefsMapProgressManager();
    public bool IsGenerated;

    public MapLevel MapLevelPrefab;
    public int Count = 10;

    public TranslationType TranslationType;

    public bool StarsEnabled;
    public StarsType StarsType;

    public bool ScrollingEnabled;
    public MapCamera MapCamera;

    public bool IsClickEnabled;
    public bool IsConfirmationEnabled;

    public void Awake()
    {
        _instance = this;
    }

    public void OnDestroy()
    {
        _instance = null;
    }

    public void OnEnable()
    {
        Reset();
    }

    public static List<MapLevel> GetMapLevels()
    {
        return FindObjectsOfType<MapLevel>().OrderBy(ml => ml.Number).ToList();
    }

    public void Reset()
    {
        print("update");
        UpdateMapLevels();
        IsClickEnabled = true;

    }

    private void UpdateMapLevels()
    {
        foreach (MapLevel mapLevel in GetMapLevels())
        {

            mapLevel.UpdateState(
                _mapProgressManager.LoadLevelStarsCount(mapLevel.Number),
                IsLevelLocked(mapLevel.Number));
        }
    }

    #region Events

    public static event EventHandler<LevelReachedEventArgs> LevelSelected;
    public static event EventHandler<LevelReachedEventArgs> LevelReached;

    #endregion

    #region Static API

    public static void CompleteLevel(int number)
    {
        CompleteLevelInternal(number, 1);
    }

    public static void CompleteLevel(int number, int starsCount)
    {
        CompleteLevelInternal(number, starsCount);
    }

    internal static void OnLevelSelected(int number)
    {
        if (EventSystem.current.IsPointerOverGameObject(-1))
            return;
        if (LevelSelected != null && !IsLevelLocked(number))  //need to fix in the map plugin
            LevelSelected(_instance, new LevelReachedEventArgs(number));

        if (!_instance.IsConfirmationEnabled)
            GoToLevel(number);
    }

    public static void GoToLevel(int number)
    {
        switch (_instance.TranslationType)
        {
            case TranslationType.Teleportation:
                _instance.TeleportToLevelInternal(number, false);
                break;
            case TranslationType.Walk:
                  _instance.WalkToLevelInternal(number);
                break;
        }
    }
    public static int sectionStartLevelNumber;
    public static int sectionEndLevelNumber;
    public static bool IsLevelLocked(int number)
    {
        if (number > 1 && _mapProgressManager.LoadLevelStarsCount(number - 1) == 0)
        {

            return true;
        }
        else
        {
            if (number >= 20)
            {
                sectionStartLevelNumber = number - 19;
                sectionEndLevelNumber = sectionStartLevelNumber + 20;
            }
            else
            {
                sectionStartLevelNumber = 1;
                sectionEndLevelNumber = 21;
            }
            int total = 0;
            int levelnumber = number;
            //int levelnumber = Camera.main.GetComponent<LevelManager>().currentLevel;
            if (levelnumber > 1 && (levelnumber - 1) % 20 == 0)
            {
                for (int i = sectionStartLevelNumber - 1; i < sectionEndLevelNumber; i++)
                {
                    total += PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", i));

                }
                if (total >= 45 && (levelnumber - 1) % 20 == 0)
                {
                    Debug.Log("New Section's been open");
                    return false;
                }
                else
                {
#if UNITY_ANDROID
                    if (Application.platform == RuntimePlatform.Android)
                        AndroidNativePopups.OpenToast("You must get 45 stars", AndroidNativePopups.ToastDuration.Short);

#endif
                    return true;
                }
            }


            return false;
        }

    }

    public static void OverrideMapProgressManager(IMapProgressManager mapProgressManager)
    {
        _mapProgressManager = mapProgressManager;
    }

    public static void ClearAllProgress()
    {
        _instance.ClearAllProgressInternal();
    }

    public static bool IsStarsEnabled()
    {
        return _instance.StarsEnabled;
    }

    public static bool GetIsClickEnabled()
    {
        return _instance.IsClickEnabled;
    }

    public static bool GetIsConfirmationEnabled()
    {
        return _instance.IsConfirmationEnabled;
    }

    #endregion

    private static void CompleteLevelInternal(int number, int starsCount)
    {
        if (IsLevelLocked(number))
        {
            Debug.Log(string.Format("Can't complete locked level {0}.", number));
        }
        else if (starsCount < 1 || starsCount > 3)
        {
            Debug.Log(string.Format("Can't complete level {0}. Invalid stars count {1}.", number, starsCount));
        }
        else
        {
            int curStarsCount = _mapProgressManager.LoadLevelStarsCount(number);
            int maxStarsCount = Mathf.Max(curStarsCount, starsCount);
            _mapProgressManager.SaveLevelStarsCount(number, maxStarsCount);
            Debug.Log("gggggggg");

            if (_instance != null)
                _instance.UpdateMapLevels();
        }
    }

    private void TeleportToLevelInternal(int number, bool isQuietly)
    {
        MapLevel mapLevel = GetLevel(number);
        if (mapLevel.IsLocked)
        {
            Debug.Log(string.Format("Can't jump to locked level number {0}.", number));
        }
        else
        {
            //  WaypointsMover.transform.position = mapLevel.PathPivot.transform.position;   //need to fix in the map plugin
            // CharacterLevel = mapLevel;
            if (!isQuietly)
                RaiseLevelReached(number);
        }
    }

    private void WalkToLevelInternal(int number)
    {
        MapLevel mapLevel = GetLevel(number);
        if (mapLevel.IsLocked)
        {
            Debug.Log(string.Format("Can't go to locked level number {0}.", number));
        }
        else
        {
            RaiseLevelReached(number);
            //WaypointsMover.Move(CharacterLevel.PathPivot, mapLevel.PathPivot,
            //    () =>
            //    {
            //        CharacterLevel = mapLevel;
            //    });
        }
    }

    private void RaiseLevelReached(int number)
    {
        MapLevel mapLevel = GetLevel(number);
        //if (!string.IsNullOrEmpty(mapLevel.SceneName))
        //    SceneManager.LoadScene(mapLevel.SceneName);

        LevelReached?.Invoke(this, new LevelReachedEventArgs(number));
        OpenLevel.Instance.curruntLevel = number;
        OpenLevel.Instance.OnLevelClicked();
    }

    public MapLevel GetLevel(int number)
    {
        return GetMapLevels().SingleOrDefault(ml => ml.Number == number);
    }

    private void ClearAllProgressInternal()
    {
        foreach (MapLevel mapLevel in GetMapLevels())
        {
            _mapProgressManager.ClearLevelProgress(mapLevel.Number);

        }
        Reset();
        //  PlayerPrefs.DeleteAll();
        int stars = PlayerPrefs.GetInt(string.Format("Level.{0:000}.StarsCount", PlayerPrefs.GetInt("OpenLevel")), 0);
        Debug.Log(stars);

    }

    public void SetStarsEnabled(bool bEnabled)
    {
        StarsEnabled = bEnabled;
        int starsCount = 0;
        foreach (MapLevel mapLevel in GetMapLevels())
        {
            mapLevel.UpdateStars(starsCount);
            starsCount = (starsCount + 1) % 4;
           // mapLevel.StarsHoster.gameObject.SetActive(bEnabled);
            mapLevel.SolidStarsHoster.gameObject.SetActive(bEnabled);
        }
    }

    public void SetStarsType(StarsType starsType)
    {
        StarsType = starsType;
        foreach (MapLevel mapLevel in GetMapLevels())
            mapLevel.UpdateStarsType(starsType);
    }

}
