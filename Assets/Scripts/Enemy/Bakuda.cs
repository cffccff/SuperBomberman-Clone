using UnityEngine;
using DG.Tweening;
public class Bakuda : EnemyMovement
{
    [Tooltip("The time need to makes bakuda use its skill")]
    [SerializeField] int timerSkill;
    [Tooltip("The time is counted in fix Update, if this timer >= timerSkill then bakuda use its skill")]
    [SerializeField] float timer;
    [Tooltip("The bakuda skill is drop a black bomb like player but have zero bomb fuse time")]
    protected GameObject tempBomb;
    private Collider2D enemyHitPointCollider;
    protected override void Start()
    {
        enemyHitPointCollider = gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>();
        base.Start();
        timerSkill = Random.Range(4, 7);
    }
    protected override void FixedUpdate()
    {
       timer += Time.fixedDeltaTime;
       
        if (timer >= timerSkill)
        {

            BakudaExplode();


        }
        else
        {
            base.FixedUpdate();
        }

    }
    private void BakudaExplode()
    {
        var position = transform.position;
        transform.DOMove(new Vector2(Mathf.Round(position.x), Mathf.Round(position.y)), 0);
        animator.SetTrigger("Skill");
       
    }
    //attached with animation stage
    protected virtual void DisplayBakudaExplosion()
    {
        spriteRenderer.enabled = false;
        enemyHitPointCollider.enabled = false;
        tempBomb = Pool.instance.GetEnemyBlackBomb();
        tempBomb.transform.position = transform.position;
        tempBomb.SetActive(true);
    }
    //attached this function to its animation
    private void EnableSpriteRender()
    {
        spriteRenderer.enabled = true;
       
    }
    //attached this function to its animation
    private void ReturnMoveStage()
    {       
        animator.ResetTrigger("Skill");
        animator.SetTrigger("Move");      
        timer = 0;
        timerSkill = Random.Range(4, 7);
        enemyHitPointCollider.enabled = true;
    }
}
