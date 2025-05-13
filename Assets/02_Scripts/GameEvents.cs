using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }

    public event Action<CardBehaviour> OnCardSelected;
    public void CardSelected(CardBehaviour card)
    {
        if (OnCardSelected != null)
        {
            OnCardSelected(card);
        }
    }
    
    public event Action<CardBehaviour> OnCardDeselected;
    public void CardDeselected(CardBehaviour card)
    {
        if (OnCardDeselected != null)
        {
            OnCardDeselected(card);
        }
    }

    public event Action<CardBehaviour[]> OnCardsReveal;
    public void RevealCards(CardBehaviour[] cards)
    {
        if (OnCardsReveal != null)
        {
            OnCardsReveal(cards);
        }
    }

    public event Action OnFlashActive;
    public void FlashPlayer()
    {
        if (OnFlashActive != null)
        {
            OnFlashActive();
        }
    }

    public event Action OnHealActive;
    public void HealEffect()
    {
        if (OnHealActive != null)
        {
            OnHealActive();
        }
    }

    public event Action OnDamageActive;

    public void DamageEffect()
    {
        if (OnDamageActive != null)
        {
            OnDamageActive();
        }
    }

    public event Action<Transform, GameObject> OnRemoveSpecial;
    public void RemoveSpecialCard(Transform slots, GameObject card)
    {
        if (OnRemoveSpecial != null)
        {
            OnRemoveSpecial(slots, card);
        }
    }
    public event Action<int, float> OnSoundPlay;
    public void PlaySound(int index, float volume)
    {
        if (OnSoundPlay != null)
        {
            OnSoundPlay(index, volume);
        }
    }

    public event Action OnSoundStop;

    public void StopSound()
    {
        if (OnSoundStop != null)
        {
            OnSoundStop();
        }
    }
    public event Action<int> OnSetLife;
    public void SetLife(int hearts)
    {
        if (OnSetLife != null)
        {
            OnSetLife(hearts);
        }
    }
    public event Action<int> OnHeal;
    public void Heal(int val)
    {
        if (OnHeal != null)
        {
            OnHeal(val);
        }
    }

    public event Action OnCheckLife;
    public void CheckLife()
    {
        if (OnCheckLife != null)
        {
            OnCheckLife();
        }
    }

    public event Action OnGiveSpecial;
    public void GiveSpecial()
    {
        if (OnGiveSpecial != null)
        {
            OnGiveSpecial();
        }
    }

    public event Action<string, string> OnDisplayCardInfo;
    public void DisplayInfo(string text1, string text2)
    {
        if (OnDisplayCardInfo != null)
        {
            OnDisplayCardInfo(text1, text2);
        }
    }

    public event Action OnDisplayGameOver;
    public void DisplayGameOver()
    {
        if(OnDisplayGameOver != null)
        {
            OnDisplayGameOver();
        }
    }

    public event Action OnWin;
    public void DisplayWin()
    {
        if (OnWin != null)
        {
            OnWin();
        }
    }

    public event Action OnChangeScene;

    public void ChangeScene()
    {
        if (OnChangeScene != null)
        {
            OnChangeScene();
        }
    }
}
