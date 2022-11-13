using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartMenu : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button instructionButton;
    public Button quitButton;
    public GameObject optionMenu;
    public GameObject instructionMenu;
    public GameObject instructionPage1;
    public GameObject instructionPage2;
    public Button exitOptionMenu;
    public Button exitInstructionMenu;
    public Button backInstructionMenu;
    public Button nextInstructionMenu;
    [SerializeField] private PlayerSOScript playerSo;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    private void Start()
    {
        PlayerPrefs.SetInt("Level",0);
        DefaultPlayerStat();
       // playButton.onClick.AddListener(PlayGame);
       // optionsButton.onClick.AddListener(DisplayOptionPanel);
       // instructionButton.onClick.AddListener(DisplayInstructionPanel);
       // quitButton.onClick.AddListener(QuitGame);
        exitOptionMenu.onClick.AddListener(HideOptionPanel);
        exitInstructionMenu.onClick.AddListener(HideInstructionPanel);
        backInstructionMenu.onClick.AddListener(DisplayPage1Instruction);
        nextInstructionMenu.onClick.AddListener(DisplayPage2Instruction);
    }
    
    // used in inspector event trigger
    // ReSharper disable once MemberCanBePrivate.Global
    public void PlayGame()
    {
        Initiate.Fade("GamePlay", Color.white, 1.0f);
        GameMusic.instance.StopPlayMusic();
        audioSource.PlayOneShot(audioClips[0]);
    }
    // // used in inspector event trigger
    // ReSharper disable once MemberCanBePrivate.Global
    public void DisplayOptionPanel()
    {
        optionMenu.transform.DOScale(Vector2.one, 0.5f);
        audioSource.Play();
    }

    // used in inspector event trigger
    // ReSharper disable once MemberCanBePrivate.Global
    public void DisplayInstructionPanel()
    {
        instructionMenu.transform.DOScale(Vector2.one, 0.5f);
        audioSource.Play();
    }
    // used in inspector event trigger
    // ReSharper disable once MemberCanBePrivate.Global
    public void QuitGame()
    {
        Application.Quit();
    }
    private void HideOptionPanel()
    {
        optionMenu.transform.DOScale(Vector2.zero, 0.5f);
        audioSource.Play();
    }
    private void HideInstructionPanel()
    {
        instructionMenu.transform.DOScale(Vector2.zero, 0.5f);
        audioSource.Play();
    }
    
    private void DisplayPage2Instruction()
    {
       instructionPage1.SetActive(false);
       instructionPage2.SetActive(true);
       audioSource.Play();
    }

    private void DisplayPage1Instruction()
    {
        instructionPage1.SetActive(true);
        instructionPage2.SetActive(false);
        audioSource.Play();
    }
    
    private void DefaultPlayerStat()
    {
        playerSo.health = 3;
        playerSo.moveSpeed = 3;
        playerSo.explosionRadius = 2;
        playerSo.totalBomb = 1;
        playerSo.isHaveKickPower = false;
        playerSo.isHaveRemotePower = false;
        playerSo.isHaveRemotePower = false;
        playerSo.isHaveBlockPass = false;
        playerSo.isHaveBombPass = false;
    }

    public void PlaySoundHoverButton()
    {
        if(!audioSource.isPlaying) audioSource.PlayOneShot(audioClips[1]);
    }
   
}
