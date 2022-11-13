using System.Collections;
using DG.Tweening;
using Febucci.UI;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool isPausePanelActive;
    [SerializeField] private TextAnimator txtHeart;
    [SerializeField] private TextAnimator txtBomb;
    [SerializeField] private TextAnimator txtFire;
    [SerializeField] private TextAnimator txtSpeed;
    [SerializeField] private TextAnimator txtRemote;
    [SerializeField] private TextAnimator txtKick;
    [SerializeField] private TextAnimator txtBombPass;
    [SerializeField] private TextAnimator txtBlockPass;
    [SerializeField] private TextAnimator txtRedBomb;
    [SerializeField] private PlayerSOScript playerSo;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject losePanel;

    [SerializeField] private GameObject levelIntroPanel;
    [SerializeField] private TextAnimatorPlayer levelText;

    [SerializeField] private Button closeButton;
    
    private bool isLostPanelActive;

    private const string OpenTag = "{size}";
    private const string EndTag = "{/size}";
    //for transition scene
    private string scene;
    private readonly Color loadToColor = Color.white;
    private int level;
    
   
    private void LoadGamePlayScene()
    {
        Initiate.Fade(scene, loadToColor, 1.0f);
    }
    private void Awake()
    {
      
        instance = this;
        scene = "GamePlay";
        isPausePanelActive = true;
        isLostPanelActive = false;
        level = PlayerPrefs.GetInt("Level");
    }

    private void Start()
    {
        Invoke(nameof(DisplayLevelIntro),1.5f);
       Invoke(nameof(SetupAfterScreenVisible),5f);
       closeButton.onClick.AddListener(TransitionPausePanel);
    }

    private void TransitionPausePanel()
    {
        pausePanel.transform.DOLocalMoveY(1500, 0.5f).SetUpdate(true).OnComplete(ClosePausePanel);
    }

    private void ClosePausePanel()
    {
        ResumeGame();
        isPausePanelActive = false;
    }
    
    private void DisplayLevelIntro()
    {
        GameMusic.instance.PlayStageIntro();
        var text = level + 1;
        levelText.ShowText(text.ToString());
        levelIntroPanel.transform.DOLocalMoveX(0, 1f);
        StartCoroutine(HideLevelIntro());
    }

    private IEnumerator HideLevelIntro()
    {
        yield return new WaitForSeconds(3f);
        levelIntroPanel.transform.DOLocalMoveX(1920, 1f);
        isPausePanelActive = false;
    }
    
    
    private void SetupAfterScreenVisible()
    {
        GameMusic.instance.PlayBattleMusic();
        MapManager.instance.PlaceObjectOnMap();
        DisplayHUD();
        SubscribeEvent();
       
    }

    private void DisplayHUD()
    {
        txtHeart.SetText(PlayerHealth.instance.Health.ToString(),false);
        txtKick.SetText(BombController.instance.kick==true? "ON": "OFF",false);
        txtFire.SetText(BombController.instance.explosionRadius.ToString(),false);
        txtRedBomb.SetText(BombController.instance.redBomb==true? "ON": "OFF",false);
        txtRemote.SetText(BombController.instance.remoteControl==true? "ON": "OFF",false);
        txtBomb.SetText(BombController.instance.totalBomb.ToString(),false);
        txtSpeed.SetText(PlayerMovement.instance.moveSpeed.ToString(),false);
        txtBlockPass.SetText(PlayerMovement.instance.blockPass==true? "ON": "OFF",false);
        txtBombPass.SetText(PlayerMovement.instance.bombPass==true? "ON": "OFF",false);
    }

    private void SubscribeEvent()
    {
        PlayerMovement.instance.OnMoveSpeedPowerChanged += UpdateSpeedPowerUI;
        PlayerMovement.instance.OnBombPassPowerChanged += UpdateBombPassPowerUI;
        PlayerMovement.instance.OnBlockPassPowerChanged += UpdateBlockPassPowerUI;

        BombController.instance.OnExplosionRadiusChanged += UpdateExplosionRadiusUI;
        BombController.instance.OnKickPowerChanged += UpdateKickUI;
        BombController.instance.OnTotalBombChanged += UpdateBombUI;
        BombController.instance.OnRedBombPowerChanged += UpdateRedBombUI;
        BombController.instance.OnRemoteControlPowerChanged += UpdateRemoteUI;

        PlayerHealth.instance.OnHealthChanged += UpdateHealthUI;
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&isLostPanelActive==false)
        {
            HandlePauseUI();
        }
    }

    private void HandlePauseUI()
    {
        if (isPausePanelActive == false)
        {
            PauseGame();
            NegateValuePausePanel();
            pausePanel.transform.DOLocalMoveY(0, 0.5f).SetUpdate(true);
        }
    }

    private void NegateValuePausePanel()
    {
        isPausePanelActive = !isPausePanelActive;
    }
    public void DisplayLosePanel()
    {
        isLostPanelActive = true;
        GameMusic.instance.PlayLose();
        losePanel.transform.DOLocalMoveY(-100, 1.5f).SetEase(Ease.OutBounce).SetUpdate(true);
        PauseGame();
    }
    
    private void PauseGame()
    {
        Time.timeScale = 0;
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
  
    
    public IEnumerator MoveToNextLevel()
    {
        
        level++;
        PlayerPrefs.SetInt("Level",level);
        PlayerMovement.instance.ReadyForNextLevel();
        GameMusic.instance.PlayVictory();
        Gate.instance.DisableTheGate();
        yield return new WaitForSeconds(3f);
        Pool.instance.ResetAllBomb();
        playerSo.health = PlayerHealth.instance.Health;
        playerSo.moveSpeed = PlayerMovement.instance.moveSpeed;
        playerSo.explosionRadius = BombController.instance.explosionRadius;
        playerSo.totalBomb = BombController.instance.totalBomb;
        playerSo.isHaveKickPower = BombController.instance.kick;
        playerSo.isHaveRemotePower = BombController.instance.remoteControl;
        playerSo.isHaveRedBombPower = BombController.instance.redBomb;
        playerSo.isHaveBlockPass = PlayerMovement.instance.blockPass;
        playerSo.isHaveBombPass = PlayerMovement.instance.bombPass;
        LoadGamePlayScene();
    }
  

    public void ResetPowerUps()
    {
        BombController.instance.kick = false;
       BombController.instance.remoteControl = false;
        BombController.instance.redBomb = false;
        PlayerMovement.instance.blockPass = false;
        PlayerMovement.instance.bombPass = false;
    }

    private void UpdateSpeedPowerUI(int moveSpeed)
    {
        txtSpeed.SetText($"{OpenTag}{moveSpeed.ToString()}{EndTag}",false);
    }

    private void UpdateBombUI(int totalBomb)
    {
        txtBomb.SetText($"{OpenTag}{totalBomb.ToString()}{EndTag}",false);
    }

    private void UpdateExplosionRadiusUI(int explosionRadius)
    {
        txtFire.SetText($"{OpenTag}{explosionRadius.ToString()}{EndTag}",false);
    }

    private void UpdateHealthUI(int currentHp)
    {
        txtHeart.SetText($"{OpenTag}{currentHp.ToString()}{EndTag}",false);
    }

    private void UpdateKickUI(bool haveKickPowerUp)
    {
        SetTextBool(haveKickPowerUp, txtKick);
    }

    private void UpdateRemoteUI(bool haveRemoteControlPowerUp)
    {
        SetTextBool(haveRemoteControlPowerUp, txtRemote);
    }

    private void UpdateBlockPassPowerUI(bool isHaveBlockPassPowerUp)
    {
        SetTextBool(isHaveBlockPassPowerUp, txtBlockPass);
    }

    private void UpdateBombPassPowerUI(bool isHaveBombPassPowerUp)
    {
        SetTextBool(isHaveBombPassPowerUp, txtBombPass);
    }

    private void UpdateRedBombUI(bool haveRedBombPowerUp)
    {
        SetTextBool(haveRedBombPowerUp, txtRedBomb);
    }
    
    private void SetTextBool(bool currentState, TextAnimator text)
    {
        //tag for text animation
        text.SetText(currentState == true ? $"{OpenTag}ON{EndTag}" : $"{OpenTag}OFF{EndTag}",false);
    }


    public void ResetScriptableObjectPlayer()
    {
        playerSo.isHaveKickPower = false;
        playerSo.isHaveRemotePower = false;
        playerSo.isHaveRemotePower = false;
        playerSo.isHaveBlockPass = false;
        playerSo.isHaveBombPass = false;
    }
}
