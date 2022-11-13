using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] protected int maxHealth, currentHealth;
    private SpriteRenderer spriteRenderer;
    private readonly Material lostHpMaterial;
    private Material defaultMaterial;
    private Animator animator;
    [SerializeField] protected Collider2D coll2D;
    private Rigidbody2D rb;
    //contain powerUp if enemy have been assigned to;
    [SerializeField] private GameObject powerUp;
    [SerializeField] private GameObject enemyContainer;
    private Collider2D enemyHitPointCollider;
    private EnemyHealth enemyHealth;
    private EnemyMovement enemyMovement;
    private BoxCollider2D enemyCollider;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator =GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyHitPointCollider = gameObject.transform.GetChild(0).GetComponent<Collider2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
      
        currentHealth = maxHealth;
        defaultMaterial = spriteRenderer.material;
    }
    public IEnumerator EnemyInvincible()
    {
        enemyHitPointCollider.enabled = false;
        for (float i = 0; i < 4.5f; i += 0.1f)
        {
          
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.material = lostHpMaterial;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.material = defaultMaterial;
        }
        enemyHitPointCollider.enabled = true;
    }
    
    public void Hurt()
    {
        StartCoroutine(LostHealth());
    }
    protected virtual IEnumerator LostHealth()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            SFXManager.instance.PlayEnemyDeath();
            rb.bodyType = RigidbodyType2D.Static;
            enemyMovement.enabled = false;
            enemyHealth.enabled = false;
            for (float i = 0; i < 0.5f; i += 0.1f)
            {

                yield return new WaitForSeconds(0.05f);
                spriteRenderer.material = lostHpMaterial;
                yield return new WaitForSeconds(0.05f);
                spriteRenderer.material = defaultMaterial;
            }
            //disable enemy collider to prevent when on dead play still be harm by enemy
            enemyCollider.enabled = false;
            animator.SetTrigger("Death");
        }
        else
        {
            for (float i = 0; i < 0.5f; i += 0.1f)
            {

                yield return new WaitForSeconds(0.05f);
                spriteRenderer.material = lostHpMaterial;
                yield return new WaitForSeconds(0.05f);
                spriteRenderer.material = defaultMaterial;
            }
        }
       
       
    }
   //this function is attached at the end of death animator stage
    private void DestroyEnemy()
    {
        InstantiatePowerUp();
        enemyContainer.GetComponent<EnemyContainerScript>().DecreaseEnemyInScene();
        Destroy(gameObject);
    }

    public void SetPowerUp(GameObject newObject)
    {
        powerUp = newObject;
    }
    private void InstantiatePowerUp()
    {
        if (powerUp == null) return;
        //ensure when power up spawn its position will in center of cell
        var position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        Instantiate(powerUp,position,quaternion.identity);
        
    }

    public void ReferenceEnemyContainer(GameObject container)
    {
        enemyContainer = container;
    }
    
}
