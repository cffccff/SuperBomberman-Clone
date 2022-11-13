using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    public Button optionsButton;
    public Button instructionButton;
    public Button exitButton;
    public GameObject optionMenu;
    public GameObject instructionMenu;
    public GameObject instructionPage1;
    public GameObject instructionPage2;
    public Button exitOptionMenu;
    public Button exitInstructionMenu;
    public Button backInstructionMenu;
    public Button nextInstructionMenu;
    
    private void Start()
    {
        optionsButton.onClick.AddListener(DisplayOptionPanel);
        instructionButton.onClick.AddListener(DisplayInstructionPanel);
        exitButton.onClick.AddListener(BackToStartMenu);
        exitOptionMenu.onClick.AddListener(HideOptionPanel);
        exitInstructionMenu.onClick.AddListener(HideInstructionPanel);
        backInstructionMenu.onClick.AddListener(DisplayPage1Instruction);
        nextInstructionMenu.onClick.AddListener(DisplayPage2Instruction);
    }
    
   
    private void DisplayOptionPanel()
    {
        optionMenu.transform.DOScale(Vector2.one, 0.5f).SetUpdate(true);
    }
    private void DisplayInstructionPanel()
    {
        instructionMenu.transform.DOScale(Vector2.one, 0.5f).SetUpdate(true);
    }
    private void BackToStartMenu()
    {
        Pool.instance.ResetAllBomb();
        GameManager.instance.ResumeGame();
        GameMusic.instance.StopPlayMusic();
        Initiate.Fade("StartMenu", Color.white, 1.0f);
    }
    private void HideOptionPanel()
    {
        optionMenu.transform.DOScale(Vector2.zero, 0.5f).SetUpdate(true);
    }
    private void HideInstructionPanel()
    {
        instructionMenu.transform.DOScale(Vector2.zero, 0.5f).SetUpdate(true);
    }
    
    private void DisplayPage2Instruction()
    {
       instructionPage1.SetActive(false);
       instructionPage2.SetActive(true);
    }

    private void DisplayPage1Instruction()
    {
        instructionPage1.SetActive(true);
        instructionPage2.SetActive(false);
    }

   
}
