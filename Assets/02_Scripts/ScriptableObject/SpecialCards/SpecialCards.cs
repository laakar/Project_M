using UnityEngine;
using Cysharp.Threading.Tasks;

public enum CardEffect
{
    Heal,
    Damage,
    Blind
}
[CreateAssetMenu(fileName = "Special Card", menuName = "Create Special Card")]
public class SpecialCards : CardsInfo
{
    public CardEffect cardEffect;
    
   async public void UseCard()
    {
        switch (cardEffect)
        {
            case CardEffect.Heal:
                GameEvents.current.Heal(1);
                GameEvents.current.HealEffect();
                Debug.Log("Se curo al jugador");
                break;
            case CardEffect.Damage:
                GameEvents.current.Heal(-1);
                GameEvents.current.DamageEffect();
                await UniTask.Delay(4000);
                GameEvents.current.GiveSpecial();
                GameEvents.current.CheckLife();
                break;
            case CardEffect.Blind:
                GameEvents.current.RevealCards(SpawnManager.instance.ReturnArray(SpawnManager.instance.cardsOnTable));
                GameEvents.current.FlashPlayer();
                Debug.Log("Se llamo al evento");
                break;
        }
    }
}
