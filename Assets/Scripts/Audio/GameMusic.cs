using UnityEngine;
public class GameMusic : MonoBehaviour
{
    public static GameMusic instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioClip clipPlay;
   

    private void Awake()
    {
        //Singleton method
        if (instance == null)
        {
            //First run, set the instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        
        }
        else if (instance != this)
        {
            //Instance is not the same as the one we have, destroy old one, and reset to newest one
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
       
    }
    
    private void Start()
    {
        PlayMusicBackGround();
    }
    
    private void PlayMusicBackGround()
    {
        audioSource.Stop();
        clipPlay = audioClips[9];
        audioSource.clip = clipPlay;
        audioSource.Play();
        audioSource.loop = true;
    }
    public void PlayBattleMusic()
    {
        int level = PlayerPrefs.GetInt("Level");
        audioSource.Stop();
        //boss stage 
        if (level == 3 || level == 7 || level == 11)
        {
            clipPlay = audioClips[6];
        }
        //normal stage
        else
        {
            clipPlay = audioClips[level/2];
        }
       
        audioSource.clip = clipPlay;
        audioSource.Play();
        audioSource.loop = true;
    }

    public void PlayStageIntro()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(audioClips[10]);
    }
    
    public void PlayVictory()
    {
        audioSource.Stop();
        clipPlay = audioClips[7];
        audioSource.clip = clipPlay;
        audioSource.loop = false;
        audioSource.Play();
    }
    public void PlayLose()
    {
        audioSource.Stop();
        clipPlay = audioClips[8];
        audioSource.clip = clipPlay;
        audioSource.loop = false;
        audioSource.Play();
    }
 
    public void StopPlayMusic()
    {
        audioSource.Stop();
    }
   
}
