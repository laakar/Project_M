using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Normal Card", menuName = "Create Cards")]
public class CardsInfo : ScriptableObject
{
    [Header("Card Info")]
    public int cardId;
    public string cardName;
    public string description;
    public Texture2D cardIcon;
}
