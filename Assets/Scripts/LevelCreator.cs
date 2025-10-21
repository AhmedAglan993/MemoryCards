using System;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    public static LevelCreator Instance;

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

    public void LoadDataFromLocal(int currentLevel)
    {
        LocalGameManager.Instance.levelLoaded = false;
        //Read data from text file
        TextAsset mapText = Resources.Load("Levels/" + currentLevel) as TextAsset;
        if (mapText == null)
        {
            mapText = Resources.Load("Levels/" + currentLevel) as TextAsset;
        }

        ProcessGameDataFromString(mapText.text);
    }

    private void ProcessGameDataFromString(string mapText)
    {
        string[] lines = mapText.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

        int mapLine = 0;
        foreach (string line in lines)
        {
            //check if line is game mode line
            if (line.StartsWith("MODE"))
            {
                //Replace GM to get mode number, 
                string modeString = line.Replace("MODE", string.Empty).Trim();
                //then parse it to interger
                //Assign game mode
            }
            else if (line.StartsWith("SIZE "))
            {
                string blocksString = line.Replace("SIZE", string.Empty).Trim();
                string[] sizes = blocksString.Split(new string[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
                GridManager.Instance.ColumnLength = int.Parse(sizes[0]);
                GridManager.Instance.RowLength = int.Parse(sizes[1]);
            }
            else if (line.StartsWith("LIMIT"))
            {
                string blocksString = line.Replace("LIMIT", string.Empty).Trim();
                string[] sizes = blocksString.Split(new string[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
                LocalGameManager.Instance.limitType = (LocalGameManager.LimitType) int.Parse(sizes[0]);
                LocalGameManager.Instance.Limit = int.Parse(sizes[1]);
            }
            else if (line.StartsWith("COLOR LIMIT "))
            {
                string blocksString = line.Replace("COLOR LIMIT", string.Empty).Trim();
                LocalGameManager.Instance.colorLimit = int.Parse(blocksString);
            }

            //check third line to get missions
            else if (line.StartsWith("STARS"))
            {
                string blocksString = line.Replace("STARS", string.Empty).Trim();
                string[] blocksNumbers = blocksString.Split(new string[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
                //star1 = int.Parse(blocksNumbers[0]);
                //star2 = int.Parse(blocksNumbers[1]);
                //star3 = int.Parse(blocksNumbers[2]);
                if (ProgressBarScript.Instance != null) //2.1.2
                    ProgressBarScript.Instance.InitBar(); //2.1.2
            }
            else
            {
                //Maps
                //Split lines again to get map numbers
                string[] st = line.Split(new string[] {" "}, StringSplitOptions.None);
                for (int i = 0; i < st.Length; i++)
                {
                    LocalGameManager.Instance.levelSquaresFile.Add(st[i][0].ToString());
                    //levelSquaresFile[mapLine * ColumnLength + i] = (st[i][0].ToString());
                    Debug.Log("st: " + st[i]);
                }

                mapLine++;
            }
        }

        LocalGameManager.Instance.levelLoaded = true;
    }
}