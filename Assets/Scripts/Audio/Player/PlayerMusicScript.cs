using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMusicScript : MonoBehaviour
{
    public static PlayerMusicScript instance;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        instance = this;
    }

    public void PlayMovementSound()
    {
        audioSource.PlayOneShot(audioClips[1]);
    }

    public void PlayKickBombSound()
    {
     
        audioSource.PlayOneShot(audioClips[2]);
    }
    public void PlayDeathSound()
    {
       
        audioSource.PlayOneShot(audioClips[3]);
    }

    
    
    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    public void StopPlayAnySound()
    {
        audioSource.Stop();
    }
}
