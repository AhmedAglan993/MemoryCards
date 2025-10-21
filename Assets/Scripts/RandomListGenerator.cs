using System.Collections.Generic;
using UnityEngine;

public class RandomListGenerator : MonoBehaviour
{
    public static RandomListGenerator Instance;
    public List<int> UsedIndecies = new List<int>();

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

    public void GenerateRandomList()
    {
        for (int i = 0; i < CardManager.Instance.cardFaces.Length; i++)
        {
            int numToAdd = Random.Range(0, LocalGameManager.Instance.colorLimit);
            while (UsedIndecies.Contains(numToAdd))
            {
                numToAdd = Random.Range(0, LocalGameManager.Instance.colorLimit);
            }

            UsedIndecies.Add(numToAdd);
        }
    }
}