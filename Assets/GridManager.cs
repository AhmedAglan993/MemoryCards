using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public float x_Start, y_Start;
    public int ColumnLength;
    public int RowLength;
    public int x_Space, y_Space;
    public GameObject prefab;
    private bool levelLoaded;
    public static GridManager Instance;
    GameObject go;
    public GameObject prefab2;
    private int maxNum = 20;
    public List<int> randomList;
    float cellSizeX, cellSizeY;
    private RectOffset gridPadding;
    private RectTransform parent;
    public float spacing = 10;
    private GridLayoutGroup grid;
    Vector2 lastSize;

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

        cellSizeY = GetComponent<FlexibleGridLayout>().cellSize.y;
        cellSizeX = GetComponent<FlexibleGridLayout>().cellSize.x;
    }

    public void CreateGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        SpawnGrid(ColumnLength);
       // if (!init)
    }

    private void SpawnGrid(int column)
    {
        for (int i = 0; i < LocalGameManager.Instance.levelSquaresFile.Count; i++)
        {
            Vector3 position;
            position = new Vector3(x_Start + (x_Space * (i % column)), y_Start + (-y_Space * (i / column)));

            if (LocalGameManager.Instance.levelSquaresFile[i] == "1")
            {
                go = Instantiate(prefab, position, Quaternion.identity);
                LocalGameManager.Instance.cards.Add(go);
            }
            else
            {
                go = Instantiate(prefab2, position, Quaternion.identity);
            }

            go.transform.SetParent(transform, false);
        }

        CardManager.Instance.SetCardFacesList();
    }

    private void SpawnGrid(int column, int raws)
    {
        int numberOfCells = column * raws;
        for (int i = 0; i < numberOfCells; i++)
        {
            Vector3 position;
            position = new Vector3(x_Start + (x_Space * (i % column)), y_Start + (-y_Space * (i / column)));
            go = Instantiate(prefab, position, Quaternion.identity);
            LocalGameManager.Instance.cards.Add(go);
            go.transform.SetParent(transform, false);
        }

        CardManager.Instance.SetCardFacesList();
    }

    public int raw, column;

    public void CreateGrid(int colum)
    {
        SpawnGrid(column, raw);
    }
}