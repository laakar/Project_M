using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Text Properties")] 
    [SerializeField] private TextMeshProUGUI displayLifeText;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardInfoText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI winText;
    
    [Header("Canvas Properites")]
    [SerializeField] private GameObject cardCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject[] buttons;
    void OnEnable()
    {
        GameEvents.current.OnSetLife += SetLife;
        GameEvents.current.OnDisplayCardInfo += DisplayInfo;
        GameEvents.current.OnDisplayGameOver += DisplayGameOver;
        GameEvents.current.OnWin += DisplayWin;
    }

    private void OnDisable()
    {
        GameEvents.current.OnSetLife -= SetLife;
        GameEvents.current.OnDisplayCardInfo -= DisplayInfo;
        GameEvents.current.OnDisplayGameOver -= DisplayGameOver;
        GameEvents.current.OnWin -= DisplayWin;
    }

    async void DisplayGameOver()
    {
        await UniTask.Delay(3000);
        GameEvents.current.PlaySound(4, .2f);
        GameEvents.current.StopSound();
        gameOverCanvas.SetActive(true);
        await UniTask.Delay(2000);
        gameOverText.gameObject.SetActive(true);
        await UniTask.Delay(2000);
        foreach (var var in buttons)
        {
            var.SetActive(true);
        }
    }

    async void DisplayWin()
    {
        await UniTask.Delay(3000);
        GameEvents.current.PlaySound(5, .2f);
        GameEvents.current.StopSound();
        gameOverCanvas.SetActive(true);
        await UniTask.Delay(2000);
        winText.gameObject.SetActive(true);
        await UniTask.Delay(3000);
        winText.gameObject.SetActive(false);
        await UniTask.Delay(2000);
        GameEvents.current.ChangeScene();
    }
    async void DisplayInfo(string text1, string text2)
    {
        cardCanvas.SetActive(true);
        if (!cardCanvas.activeInHierarchy) {return;}

        LeanTween.scale(cardCanvas, new Vector3(1, 1, 1), .2f);
        
        await UniTask.Delay(200);
        cardNameText.gameObject.SetActive(true);
        cardInfoText.gameObject.SetActive(true);
        
        cardNameText.text = text1;
        cardInfoText.text = text2;
        await UniTask.Delay(5000);
        cardNameText.gameObject.SetActive(false);
        cardInfoText.gameObject.SetActive(false);
        LeanTween.scale(cardCanvas, new Vector3(0, 0, 0), .2f).setOnComplete(() =>
        {
            cardCanvas.SetActive(false);
        });
    }
    void SetLife(int hearts)
    {
        displayLifeText.text = $"Fingers:{hearts}";
    }
}
