using System;
using UnityEngine;

public class SpecialCardBehaviour : MonoBehaviour, IInteractauble
{
    public SpecialCards specialCard;
    public Transform selectedSlot;
    public bool canSelect;

    private void Start()
    {
        GameEvents.current.DisplayInfo(specialCard.cardName, specialCard.description);
    }

    public void SelectCard()
    {
        if (canSelect)
        {
            specialCard.UseCard();
            GameEvents.current.RemoveSpecialCard(selectedSlot, gameObject);
        }
    }
}
