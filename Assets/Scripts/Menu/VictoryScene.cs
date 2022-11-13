using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScene : MonoBehaviour
{
    [SerializeField] private Button returnButton;

    private void Awake()
    {
        returnButton.onClick.AddListener(ReturnStartMenu);
    }

    private void ReturnStartMenu()
    {
        Initiate.Fade("StartMenu", Color.white, 1.0f);
    }
    
    
}
