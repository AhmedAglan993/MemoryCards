using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;
    private LocalGameManager _localGameManager;
    public int matches;
    public bool Clicked;
    public Sprite cardBack;
    public Sprite[] cardFaces;

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
      //  throw new NotImplementedException();
    }

    public CardManager(LocalGameManager localGameManager)
    {
        _localGameManager = localGameManager;
    }

    public Sprite GetCardBack()
    {
        return cardBack;
    }

    public Sprite GetCardFace(int i)
    {
        return cardFaces[i - 1];
    }

    public void IntializeCards()
    {
        print("rrrrrrrrrrrr");
        Clicked = false;
        matches = cardFaces.Length;
        for (int id = 0; id < 2; id++)
        {
            for (int i = 1; i < cardFaces.Length + 1; i++)
            {
                bool test = false;
                int choice = 0;
                while (!test)
                {
                    choice = Random.Range(0, LocalGameManager.Instance.cards.Count);
                    test = !(LocalGameManager.Instance.cards[choice].GetComponent<Card>().intialized);
                }

                LocalGameManager.Instance.cards[choice].GetComponent<Card>().cardValue = i;
                LocalGameManager.Instance.cards[choice].GetComponent<Card>().intialized = true;
            }
        }

        foreach (GameObject c in LocalGameManager.Instance.cards)
            c.GetComponent<Card>().SetUpGarphics();
        if (!LocalGameManager.Instance.init) LocalGameManager.Instance.init = true;
    }

    public void SetCardFacesList()
    {
        cardFaces = new Sprite[LocalGameManager.Instance.cards.Count / 2];
        RandomListGenerator.Instance.GenerateRandomList();
        for (int i = 0; i < cardFaces.Length; i++)
        {
            cardFaces[i] = LocalGameManager.Instance.AllcardFaces[(RandomListGenerator.Instance.UsedIndecies[i])];
        }
    }
}