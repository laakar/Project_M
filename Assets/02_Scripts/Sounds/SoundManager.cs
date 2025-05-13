using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private AudioSource _source;
    public AudioSource[] _otherSources;
    public AudioClip[] clip;
    void Start()
    {
        GameEvents.current.OnSoundPlay += PlaySound;
        GameEvents.current.OnSoundStop += StopSound;
        _source = GetComponent<AudioSource>();
    }

    void PlaySound(int index, float volume)
    {
        _source.clip = clip[index];
        _source.volume = volume;
        _source.Play();
    }

    async void StopSound()
    {
        foreach (var var in _otherSources)
        {
            var.Stop();
            var.gameObject.SetActive(false);
        }
        await UniTask.Delay(3000);
        _source.Stop();
        _source.gameObject.SetActive(false);
    }
}
