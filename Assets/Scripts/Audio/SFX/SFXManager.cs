using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] private AudioSource audioSourceSfx;
    [SerializeField] private AudioClip[] audioClips;

    private void Awake()
    {
        instance = this;
    }

    public void PlayGetItemSound()
    {
        audioSourceSfx.PlayOneShot(audioClips[0]);
    }

    public void PlayEnemyDeath()
    {
        audioSourceSfx.PlayOneShot(audioClips[1]);
    }
}
