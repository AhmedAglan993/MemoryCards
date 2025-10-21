using System;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    public static GameRules Instance;
    List<int> c = new List<int>();
    public int OnrawMatches;
    public float progress;


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

    public void CheckCards()
    {
        c.Clear();

        for (int i = 0; i < LocalGameManager.Instance.cards.Count; i++)
        {
            if (LocalGameManager.Instance.cards[i].GetComponent<Card>().state == 1)
            {
                c.Add(i);
                // Debug.Log(i);
            }
        }

        if (c.Count == 2)
        {
            CardComparison(c);
        }
    }

    private void CardComparison(List<int> c)
    {
        Card.DO_NOT = true;
        int x = 0;
        if (LocalGameManager.Instance.cards[c[0]].GetComponent<Card>().cardValue ==
            LocalGameManager.Instance.cards[c[1]].GetComponent<Card>().cardValue)
        {
            x = 2;
            Debug.Log(c.Count);
            CardManager.Instance.matches--;
            OnrawMatches++;
            progress += 1;
            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.destroy[1]);
            switch (OnrawMatches)
            {
                case 1:
                    LocalGameManager.Instance.Score += 60;
                    break;
                case 2:
                    LocalGameManager.Instance.Score += 100;
                    SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.explosion);
                    LocalGameManager.Instance.GratzWords[UnityEngine.Random.Range(0, LocalGameManager.Instance.GratzWords.Count)]
                        .SetActive(true);
                    break;
                case 3:
                    LocalGameManager.Instance.Score += 200;
                    SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.explosion);
                    LocalGameManager.Instance.GratzWords[UnityEngine.Random.Range(0, LocalGameManager.Instance.GratzWords.Count)]
                        .SetActive(true);
                    break;
                default:
                    LocalGameManager.Instance.Score += 200;
                    SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.explosion);
                    LocalGameManager.Instance.GratzWords[UnityEngine.Random.Range(0, LocalGameManager.Instance.GratzWords.Count)]
                        .SetActive(true);
                    break;
            }

            if (CardManager.Instance.matches == 0)
            {
                LocalGameManager.Instance.WinGame();
                //  IntializeCards();
            }
        }
        else
        {
            OnrawMatches = 0;
            Debug.Log("NotMatch");
            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.wrongMatch);
        }

        for (int i = 0; i < c.Count; i++)
        {
            LocalGameManager.Instance.cards[c[i]].GetComponent<Card>().state = x;
            LocalGameManager.Instance.cards[c[i]].GetComponent<Card>().FalseCheck(c);
        }

        if (LocalGameManager.Instance.limitType == LocalGameManager.LimitType.moves)
        {
            LocalGameManager.Instance.Limit--;
            UIManager.Instance.LimitTxt.text = "Moves: " + LocalGameManager.Instance.Limit.ToString();
            if (LocalGameManager.Instance.Limit < 2)
            {
                SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.alert);
            }
        }
    }
}