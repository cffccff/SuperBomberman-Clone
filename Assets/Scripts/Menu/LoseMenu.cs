using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenu : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(ReloadLevel);
        quitButton.onClick.AddListener(Quit);
    }

    private void Quit()
    {
        GameManager.instance.ResumeGame();
        Initiate.Fade("StartMenu", Color.white, 1.0f);
    }

    private void ReloadLevel()
    {
        GameManager.instance.ResetScriptableObjectPlayer();
        GameManager.instance.ResumeGame();
        Initiate.Fade("GamePlay", Color.white, 1.0f);
    }
}
