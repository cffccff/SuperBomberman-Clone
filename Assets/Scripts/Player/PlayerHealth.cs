using System.Collections;
using UnityEngine;
using DG.Tweening;
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Material invincibleMaterial;
    private Material defaultMaterial;
    //box collider of player's hit-point
    private BoxCollider2D hitPointBoxCollider;
    //transform hit-point
    private Transform hitPointTransform;
    private int playerHealth;
    private float timerInvincibleStart;
    private float timerInvinciblePowerUp;
    [SerializeField] private Ease ease;
    [SerializeField] private PlayerSOScript playerSo;
    
    public int Health
    {
        get => playerHealth;
        set {
            playerHealth = value;
            OnHealthChanged?.Invoke(playerHealth);
        }
    }
    
    //test event
    public delegate void HealthChangeHandler(int number);
    public event HealthChangeHandler OnHealthChanged;  

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitPointBoxCollider = gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>();
        hitPointTransform = gameObject.transform.GetChild(0);
        Health = playerSo.health;
    }

   
    // Start is called before the first frame update
    void Start()
    {
      
        defaultMaterial = spriteRenderer.material;
        timerInvincibleStart = 4.5f;
        timerInvinciblePowerUp = 13f;
        MakePlayerInvincible();
        StartCoroutine(Invincible(6));

    }
    
    //function ensures that play will not die from any harm
    public void MakePlayerInvincible()
    {
         hitPointTransform.tag = "PlayerInvincible";
         hitPointBoxCollider.enabled = false;
    }
    //this function attached to death animator stage
    public void DeathJumping()
    {
        Transform transform1;
        transform.DOJump(new Vector2((transform1 = transform).position.x,transform1.position.y),2,1,0.5f).SetEase(ease);
    }
    public void PlayerDeath()
    {
        Health -= 1;
        MakePlayerInvincible();
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.transform.GetChild(1).GetComponent<BombController>().enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("Death");
    }
    
    //run this function when end of death animation 
    private void ResetPlayer()
    {
        if (Health > 0)
        {
            gameObject.transform.position = Vector2.zero;
            animator.SetTrigger("Idle");
            rb.bodyType = RigidbodyType2D.Dynamic;
            gameObject.GetComponent<PlayerMovement>().enabled = true;
            gameObject.transform.GetChild(1).GetComponent<BombController>().enabled = true;
            StartCoroutine(Invincible(timerInvincibleStart));
        }
        else if (Health <= 0)
        {
            GameManager.instance.DisplayLosePanel();
        }
    }
    private IEnumerator Invincible(float timer)
    {
        for (float i = 0; i < timer; i += 0.1f)
        {

            yield return new WaitForSeconds(0.05f);
            spriteRenderer.material = invincibleMaterial;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.material = defaultMaterial;
        }
        hitPointBoxCollider.enabled = true;
        hitPointTransform.tag = "PlayerHitPoint";
    }
    
   

  
    public void ApplyInvincibleToPlayer()
    {
        MakePlayerInvincible();
        StartCoroutine(Invincible(timerInvinciblePowerUp));
    }
    
}
