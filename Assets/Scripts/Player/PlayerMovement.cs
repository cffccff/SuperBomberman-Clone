using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;
    private Animator animator;
    private Rigidbody2D rb;
    public Vector2 direction;
    private SpriteRenderer spriteRenderer;
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int NextLevel = Animator.StringToHash("NextLevel");
    private int playerMoveSpeed;
    private bool isHaveBombPassPowerUp;
    private bool isHaveBlockPassPowerUp;
    public int moveSpeed
    {
        get => playerMoveSpeed;
        set {
            if (playerMoveSpeed == value) return;
            playerMoveSpeed = value;
            OnMoveSpeedPowerChanged?.Invoke(playerMoveSpeed);
        }
    }

    public bool bombPass
    {
        get => isHaveBombPassPowerUp;
        set {
            if (isHaveBombPassPowerUp == value) return;
            isHaveBombPassPowerUp = value;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bomb"),isHaveBombPassPowerUp);
            OnBombPassPowerChanged?.Invoke(isHaveBombPassPowerUp);
        }
    }
    
    public bool blockPass
    {
        get => isHaveBlockPassPowerUp;
        set {
            if (isHaveBlockPassPowerUp == value) return;
            isHaveBlockPassPowerUp = value;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("SoftBlock"),isHaveBlockPassPowerUp);
            OnBlockPassPowerChanged?.Invoke(isHaveBlockPassPowerUp);
        }
    }
    
    [SerializeField] private PlayerSOScript playerSo;
    private PlayerMovement playerMovement;
    
    
    // event
    public delegate void MoveSpeedChangeHandler(int speed);
    public event MoveSpeedChangeHandler OnMoveSpeedPowerChanged;  
    
    public delegate void BombPassChangeHandler(bool state);
    public event BombPassChangeHandler OnBombPassPowerChanged;  
    
    public delegate void BlockPassChangeHandler(bool state);
    public event BlockPassChangeHandler OnBlockPassPowerChanged;  
    private void Awake()
    {
        SetUpReference();
        instance = this;
        playerMoveSpeed = playerSo.moveSpeed;
        isHaveBombPassPowerUp = playerSo.isHaveBombPass;
        isHaveBlockPassPowerUp = playerSo.isHaveBlockPass;
    }

    private void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction.y = 0;
        }
        else
        {
            direction.x = 0;
        }
        if (direction.x != 0 || direction.y != 0)
        {
            if(PlayerMusicScript.instance.IsPlaying()==false) 
                PlayerMusicScript.instance.PlayMovementSound();
        } 
       
        SetAnimation();

    }
    private void FixedUpdate()
    {
        if (direction == Vector2.zero) PlayerMusicScript.instance.StopPlayAnySound();
        rb.MovePosition(rb.position + direction * (playerMoveSpeed * Time.fixedDeltaTime));

    }
    private void SetUpReference()
    {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void SetAnimation()
    {
        animator.SetFloat(Horizontal, direction.x);
        animator.SetFloat(Vertical, direction.y);
        //true form of this is if (direction.x >= 1) spriteRenderer.flipX = true;
       //                       else spriteRenderer.flipX = false;
        spriteRenderer.flipX = direction.x >= 1;
        animator.SetFloat(Speed, direction.sqrMagnitude);
    }
  
    public void ReadyForNextLevel()
    {
        BombController.instance.enabled = false;
        animator.SetTrigger(NextLevel);
        PlayerHealth.instance.MakePlayerInvincible();
        playerMovement.enabled = false;
    }
    
}
