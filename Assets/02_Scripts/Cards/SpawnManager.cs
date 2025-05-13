using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    
    [Header("Scriptable Objects")]
    public CardsInfo[] normalCardsSO;
    
    [Header("Prefab")]
    public GameObject cardPrefab;

    [Header("Spawn Paramters")] 
    public int rowCount;
    public int columnCount;
    public float spaceBetween;
    [Space]
    public Transform cardHolder;
    public Transform deckPos;
    public GameObject spawnPoint;
    public List<GameObject> pointsSpawned;
    public List<CardBehaviour> cardsOnTable;

    [Space] 
    public List<CardsInfo> carddPairs;
    
    //Grid Parameters
    private float _totalWidth;
    private float _totalHeight;
    private Vector3 _offset;
    private Vector3 _startPos;

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
        SetCards();
        SetGrid();
        SpawnPoints();
    }

    public CardBehaviour[] ReturnArray (List<CardBehaviour> spawnPoint)
    {
        List<CardBehaviour> tempList = new List<CardBehaviour>();
        
        foreach (var cardB in spawnPoint)
        {
            var revealCard = cardB.GetComponentInChildren<CardBehaviour>();
            if (revealCard != null)
            {
                tempList.Add(revealCard);
            }
        }
        return tempList.ToArray();
    }
    void SetCards()
    {
        carddPairs = new List<CardsInfo>();
        for (int i = 0; i < normalCardsSO.Length; i++)
        {
            carddPairs.Add(normalCardsSO[i]);
            carddPairs.Add(normalCardsSO[i]);
        }
        ShuffleCards(carddPairs);
    }
    void ShuffleCards(List<CardsInfo> cardList)
    {
        for (int i = cardList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            CardsInfo temp = cardList[i];
            cardList[i] = cardList[randomIndex];
            cardList[randomIndex] = temp;
        }
    }
    void SetGrid()
    {
        //Se calcula el tamaño de la grilla
        _totalWidth = (columnCount - 1) * spaceBetween;
        _totalHeight = (rowCount - 1) * spaceBetween;
        
        //Se calcula el offset para centrar
        _offset = new Vector3(-_totalWidth / 2, 0, -_totalHeight / 2);
        
        //Se guarda la posicion del transform
        _startPos = cardHolder.transform.position;
        
    }
    void SpawnPoints()
    {
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                Vector3 position = _startPos + _offset + new Vector3(col * spaceBetween, 0, row * spaceBetween);
                var point  = Instantiate(spawnPoint, position, Quaternion.identity, cardHolder);
                pointsSpawned.Add(point);
            }
        }
        SpawnMoveCards();
    }
    async UniTask MoveCardToPosition(Transform card, Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(card.position, targetPosition) > 0.01f)
        {
            card.position = Vector3.MoveTowards(card.position, targetPosition, speed * Time.deltaTime);
            await UniTask.Yield(); // Esperar al siguiente frame
        }

        // Asegurar que termine exactamente en la posición final
        card.position = targetPosition;
    }
    async void SpawnMoveCards()
    {
        for (int i = 0; i < carddPairs.Count; i++)
        {
            var card = Instantiate(cardPrefab, deckPos.position, cardPrefab.transform.rotation, cardHolder);
            if (card.GetComponentInChildren<CardBehaviour>() == null) return;
            var cBehaviour = card.GetComponentInChildren<CardBehaviour>();
            cBehaviour.cardInfo = carddPairs[i];
        
            await UniTask.Delay(100);
            
            cardsOnTable.Add(cBehaviour);
            await MoveCardToPosition(card.transform, pointsSpawned[i].transform.position, 15f);
            GameEvents.current.PlaySound(1, 0.2f);
        }

        GameManager.instance.canPlay = true;
    }

}
