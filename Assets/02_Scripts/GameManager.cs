using System;
using Cysharp.Threading.Tasks;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Selected Cards")]
    [SerializeField] private CardBehaviour card_1;
    [SerializeField] private CardBehaviour card_2;

    [Space] 
    public Quaternion rotation;

    [Header("Cinemachine Cameras")] 
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera tableCam;

    [Header("Player Parameters")] 
    [SerializeField] private int _maxHearts;
    [SerializeField]  private int _hearts;

    [HideInInspector] public bool checking;
    public int specialOnDeck;
    public bool canPlay;

    private bool _wrongChoise;
    private int _pairedCards;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        GameEvents.current.OnCardSelected += AddCard;
        GameEvents.current.OnCardDeselected += RemoveCard;
        GameEvents.current.OnCardsReveal += RevealCards;
        GameEvents.current.OnHeal += HealPlayer;
        GameEvents.current.OnCheckLife += CheckWin;
        
        GameEvents.current.SetLife(_hearts);
    }
    private void OnDisable()
    {
        GameEvents.current.OnCardSelected -= AddCard;
        GameEvents.current.OnCardDeselected -= RemoveCard;
        GameEvents.current.OnCardsReveal -= RevealCards;
        GameEvents.current.OnHeal -= HealPlayer;
        GameEvents.current.OnCheckLife -= CheckWin;
    }
    void AddCard(CardBehaviour card)
    {
        if (card_1 == null)
        {
            card_1 = card;
        }
        else if (card_2 == null)
        {
            card_2 = card;
            CheckEqual();
        }
    }
    void RemoveCard(CardBehaviour card)
    {
        if (card == card_1)
        {
            card_1 = null;
        }
    }
    void ClearCards()
    {
        card_1.RestorePos();
        card_2.RestorePos();
        
        card_1 = null;
        card_2 = null;
    }
    void CheckSolved(CardBehaviour card)
    {
        card.solved = true;
    }
    void ZoomCamera()
    {
        checking = true;
        mainCam.m_Priority = 0;
        tableCam.m_Priority = 10;
    }
    void HealPlayer(int val)
    {
        if (_hearts < _maxHearts + 1)
        {
            _hearts += val;
            GameEvents.current.SetLife(_hearts);
        }
    }
    async void RevealCards(CardBehaviour[] cards)
    {
        ZoomCamera();
        foreach (var card in cards)
        {
            if (!card.solved)
            {
                card.SelectCard();
                LeanTween.rotate(card.transform.parent.gameObject, rotation.eulerAngles, 0.2f);
            }
        }
        GameEvents.current.PlaySound(2, 0.2f);
        await UniTask.Delay(4500);
        foreach (var card in cards)
        {
            if (!card.solved)
            {
                card.RestorePos();
                LeanTween.rotate(card.transform.parent.gameObject, new Vector3(0,0,0), 0.2f);
            }
        }
        GameEvents.current.PlaySound(2, 0.2f);
        RestoreCamera();
    }
    async void CheckEqual()
    {
        ZoomCamera();
        await UniTask.Delay(2000);
        LeanTween.rotate(card_1.transform.parent.gameObject, rotation.eulerAngles, 0.2f);
        LeanTween.rotate(card_2.transform.parent.gameObject, rotation.eulerAngles, 0.2f);
        GameEvents.current.PlaySound(2, 0.2f);
        
        await UniTask.Delay(2000);
        if (card_1.cardName == card_2.cardName)
        {
            Debug.Log("Son iguales");
            _pairedCards++;
            CheckSolved(card_1);
            CheckSolved(card_2);
        }
        else
        {
            GameEvents.current.SetLife(_hearts);
            LeanTween.rotate(card_1.transform.parent.gameObject, new Vector3(0,0,0), 0.2f);
            LeanTween.rotate(card_2.transform.parent.gameObject, new Vector3(0,0,0), 0.2f);
            _wrongChoise = true;
        }
        
        GameEvents.current.PlaySound(2, 0.2f);
        ClearCards();
        RestoreCamera();
        
        await UniTask.Delay(2000);
        if (_wrongChoise)
        {
            HealPlayer(-1);
            GameEvents.current.DamageEffect();
            _wrongChoise = false;
            await UniTask.Delay(4000);
        }

        if (specialOnDeck < 3)
        {
            GameEvents.current.GiveSpecial();
        }
        CheckWin();
    }

    void CheckWin()
    {
        if (_hearts <= 0)
        {
            canPlay = false;
            GameEvents.current.DisplayGameOver();
        }

        if (_pairedCards == 4)
        {
            canPlay = false;
            GameEvents.current.DisplayWin();
        }
    }
    async void RestoreCamera()
    {
        mainCam.m_Priority = 10;
        tableCam.m_Priority = 0;
        
        await UniTask.Delay(1000);
        checking = false;
    }
}
