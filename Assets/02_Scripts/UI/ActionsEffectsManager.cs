using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ActionsEffectsManager : MonoBehaviour
{
    [Header("Flash Properties")]
    public Image flashImage;
    public Light light;

    [Header("Heal Effect Parameters")]
    public Volume volume;
    [SerializeField] private float _finalBloom;
    [SerializeField] private float _finalExposure;
        
    private float _initBloomVal;
    private float _initExposure;

    [Header("Damage Effect Parameters")] 
    [SerializeField] private Color _initColor;
    [SerializeField] private Color _finalColor;

    private void Start()
    {
        GameEvents.current.OnFlashActive += FlashEffect;
        GameEvents.current.OnHealActive += HealEffect;
        GameEvents.current.OnDamageActive += DamagePlayer;
        
        if (volume.profile.TryGet(out Bloom bloom))
        {
            _initBloomVal = bloom.intensity.value;
        }
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            _initExposure = colorAdjustments.postExposure.value;
            _initColor = colorAdjustments.colorFilter.value;
        }
    }
    void FlashEffect()
    {
        GameEvents.current.PlaySound(0, 0.7f);
        LeanTween.value(gameObject, light.intensity, 10, 0.2f).setOnUpdate((float val) =>
        {
            light.intensity = val;
        });
        LeanTween.alpha(flashImage.rectTransform, 1f, 0.2f).setOnComplete(() =>
        {
            light.intensity = 0f;
            LeanTween.alpha(flashImage.rectTransform, 0f, 4.5f);
        });
    }
    void HealEffect()
    {
        if (volume.profile.TryGet(out Bloom bloom))
        {
            LeanTween.value(gameObject, _initBloomVal, _finalBloom, .2f).setOnUpdate((float val) =>
            {
                bloom.intensity.value = val;
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, _finalBloom, _initBloomVal, 2f).setOnUpdate((float val) =>
                {
                    bloom.intensity.value = val;
                });
            });
        }
    }
    void DamagePlayer()
    {
        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            GameEvents.current.PlaySound(3, .2f);
            LeanTween.value(gameObject, _initExposure, _finalExposure, 0.2f).setOnUpdate((float val) =>
            {
                colorAdjustments.postExposure.value = val;
            });
            LeanTween.value(gameObject, _initColor, _finalColor, 0.2f).setOnUpdate((Color color) =>
            {
                colorAdjustments.colorFilter.value = color;
            });
            LeanTween.value(gameObject, _finalExposure, _initExposure, 3f).setOnUpdate((float val) =>
            {
                colorAdjustments.postExposure.value = val;
            });
            LeanTween.value(gameObject, _finalColor, _initColor, 5f).setOnUpdate((Color color) =>
            {
                colorAdjustments.colorFilter.value = color;
            });
        }
    }
}
