using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clips;
    [Space]
    public float minTime;
    public float maxTime;
    
    private void Start()
    {
        StartCoroutine(PlaySound());
    }
    void PlayRandomSound()
    {
        var selectedSound = clips[Random.Range(0, clips.Length)];
        source.pitch = Random.Range(0.60f, 0.65f);
        source.clip = selectedSound;
        source.Play();
    }

    IEnumerator PlaySound()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            PlayRandomSound();
        }
    }
}
