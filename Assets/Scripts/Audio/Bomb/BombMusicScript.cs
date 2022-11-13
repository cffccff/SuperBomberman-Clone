using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMusicScript : MonoBehaviour
{
    public static BombMusicScript instance;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        instance = this;
    }

    public void PlayBombPlacementSound()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }

    public void PlayBombExplosionSound()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }
}
