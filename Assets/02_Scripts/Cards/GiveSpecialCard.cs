using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

public class GiveSpecialCard : MonoBehaviour
{
    [Header("Scriptable Objects")] 
    public SpecialCards[] specialCardsOS;

    [Header("Spawn Properties")] 
    [SerializeField] private Transform[] slots;
    [SerializeField] private Transform initPos;
    [SerializeField] private GameObject cardPrefab;
    private void Start()
    {
        GameEvents.current.OnRemoveSpecial += RemoveCards;
        GameEvents.current.OnGiveSpecial += SpawnCard;
    }
    async void SpawnCard()
    {
        var card = Instantiate(cardPrefab, initPos.position, Quaternion.Euler(90,0,0));
        card.GetComponent<SpecialCardBehaviour>().specialCard = specialCardsOS[Random.Range(0, specialCardsOS.Length)];
        
        await UniTask.Delay(200);
        MoveCard(card.transform);
        GameManager.instance.specialOnDeck++;
    }
   async void MoveCard(Transform card)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount == 0)
            {
                card.GetComponent<SpecialCardBehaviour>().selectedSlot = slots[i].transform;
                LeanTween.move(card.gameObject, slots[i].position, .2f);
                card.SetParent(slots[i]);
                break;
            }
        }
        GameEvents.current.PlaySound(1, 0.2f);

        await UniTask.Delay(1000);
        card.GetComponent<SpecialCardBehaviour>().canSelect = true;
    }
    void RemoveCards(Transform slot, GameObject card)
    {
        slot.DetachChildren();
        Destroy(card);
        GameManager.instance.specialOnDeck--;
    }
}
