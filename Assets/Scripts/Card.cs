using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Card : MonoBehaviour
{
    public static bool DO_NOT = false;
    [SerializeField] private int _state;
    [SerializeField] private int _cardValue;
    [SerializeField] private bool _intialized = false;
    private Sprite _cardBack;
    private Sprite _cardFace;
    private GameObject manager;
    private readonly CardManager _cardManager;

    public CardManager CardManager
    {
        get { return _cardManager; }
    }

    private void Awake()
    {
        intialized = false;
    }

    void Start()
    {
        _state = 1;
        manager = GameObject.FindGameObjectWithTag("Manager");
    }

    public void SetUpGarphics()
    {
        _cardBack = CardManager.Instance.GetCardBack();
        _cardFace = CardManager.Instance.GetCardFace(cardValue);
        FlipCard();
        Debug.Log("Start");
    }

    public void PlaySound()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.swish[0]);
    }

    public void FlipCard()
    {
        Debug.Log("Flip");
        if (_state == 0)
        {
            _state = 1;
        }
        else if (_state == 1)
        {
            _state = 0;
        }

        if (_state == 0 && !DO_NOT)
        {
            GetComponent<Image>().sprite = _cardBack;
        }
        else if (_state == 1 && !DO_NOT)
        {
            GetComponent<Animator>().Play("Base Layer.FlipCard");
            GetComponent<Button>().interactable = false;
            StartCoroutine(WaitForAnimation());
            //GetComponent<Image>().sprite = _cardFace;
        }
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.08f);
        SetCardFace();
    }

    public void SetCardFace()
    {
        GetComponent<Image>().sprite = _cardFace;
    }

    public int cardValue
    {
        get { return _cardValue; }
        set { _cardValue = value; }
    }

    public int state
    {
        get { return _state; }
        set { _state = value; }
    }

    public bool intialized
    {
        get { return _intialized; }
        set { _intialized = value; }
    }

    public void FalseCheck(List<int> c)
    {
        StartCoroutine(Pause(c));
    }

    IEnumerator Pause(List<int> c)
    {
        yield return new WaitForSeconds(.5f);
        if (state == 0)
        {
            GetComponent<Button>().interactable = true;
            GetComponent<Image>().sprite = _cardBack;
            // c.Clear();
        }
        else if (state == 1)
        {
            GetComponent<Image>().sprite = _cardFace;
        }
        else
        {
            GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

        GetComponent<Animator>().Play("Base Layer.FlipCard");
        DO_NOT = false;
    }
}