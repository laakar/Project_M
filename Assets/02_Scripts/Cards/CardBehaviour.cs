using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CardBehaviour : MonoBehaviour, IInteractauble
{
    public CardsInfo cardInfo;

    public int iD;
    public string cardName;
    public string cardDescription;

    public bool selected;
    public bool solved;

    private void Start()
    {
        iD = cardInfo.cardId;
        cardName = cardInfo.cardName;
        cardDescription = cardInfo.description;
        gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = cardInfo.cardIcon;
    }
    public void SelectCard()
    {
        if (!GameManager.instance.checking && !solved)
        {
            selected = !selected;
            if (selected)
            {
                LeanTween.moveLocalY(transform.parent.gameObject, 0.1f, .2f);
                GameEvents.current.CardSelected(this);
            }
            else
            {
                LeanTween.moveLocalY(transform.parent.gameObject, 0.1f, .2f);
                GameEvents.current.CardDeselected(this);
            }
        }
    }
    public void RestorePos()
    {
        selected = false;
        LeanTween.moveLocalY(transform.parent.gameObject, 0f, .2f);
    }
}
