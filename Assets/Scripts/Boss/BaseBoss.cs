using System.Collections;
using UnityEngine;
using DG.Tweening;
public class BaseBoss : MonoBehaviour
{
    protected Rigidbody2D rb;
    [SerializeField] protected float moveSpeed;
    public GameObject player;
    protected Animator animator;
    [SerializeField] protected int maxHealth, currentHealth;
    protected CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;
    protected float defaultMoveSpeed;

    private float x, y;
    private Vector3 vector;
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Skill = Animator.StringToHash("Skill");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = gameObject.GetComponentInChildren<BoxCollider2D>();
    }
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        defaultMoveSpeed = moveSpeed;
    }
    protected virtual void FixedUpdate()
    {

        rb.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * moveSpeed));
    }
    // all function below trigger when the animator run specify stage

    //when boss in Boss stage then run this function
    protected virtual void StartExplosionDeath()
    {

        StartCoroutine(ExplosionDeath());
    }
    //create randomly explosion on boss's BossDeath position
    protected virtual IEnumerator ExplosionDeath()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            for (int i = 0; i <= 20; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
                x = Random.Range(-2, 2.1f);
                y = Random.Range(-2, 2.1f);
                vector = new Vector3(x, y, 0);
                GameObject explosionBossDeath = Pool.instance.GetExplosionDeath();
                vector += transform.position;
                explosionBossDeath.transform.position = vector;
                explosionBossDeath.SetActive(true);

            }
        }




    }
    //this function attached with Death animator state
    protected void StopAllCoroutinesBoss()
    {
        StopAllCoroutines();

    }
    //this function attached with Death animator state
    protected void HideBoss()
    {
        
        transform.DOMoveY(-10, 4).OnComplete(DestroyBoss);
    }

    private void DestroyBoss()
    {
        StartCoroutine(GameManager.instance.MoveToNextLevel());
       // Destroy(gameObject);
    }
    protected virtual void BossDeath()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
            StartExplosionDeath();
         
          //  animator.ResetTrigger("Move");
            animator.SetTrigger(Death);
           
    }
    protected virtual void Stagger()
    {
       
            SetMoveSpeedZero();
         //   animator.ResetTrigger("Skill");
            animator.SetTrigger(Hurt);
        
    }
    public virtual void BossHurt()
    {
        currentHealth--;
        if(currentHealth <= 0) BossDeath();

        if (currentHealth > 0) Stagger();
    }
    protected void SetMoveSpeedZero()
    {
        moveSpeed = 0;
    }
    protected virtual void ReturnMoveStage()
    {
        moveSpeed = defaultMoveSpeed;
        animator.SetTrigger(Move);
        animator.ResetTrigger(Skill);
    }

    
}
