using UnityEngine;

public class GamePlayUISound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    
    //use in canvas to play hover sound button
    public void PlaySoundHoverButton()
    {
        if(!audioSource.isPlaying) audioSource.PlayOneShot(audioClips[1]);
    }
}


