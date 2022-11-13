using System.Collections;
using UnityEngine;

public class Boss3 : BaseBoss
{
    [SerializeField] private Vector2 lastVelocity;
    [SerializeField] private float moveSpeedRushing;
    [SerializeField] private int timeBounce;
    [SerializeField] private bool isRushing;
    [SerializeField] private float timerRush;
    private const float LimitTimeRush = 10f;
    private static readonly int Skill = Animator.StringToHash("Skill");
    private static readonly int Move = Animator.StringToHash("Move");

    protected override void Start()
    {
        base.Start();
        isRushing = false;
        timeBounce = 0;
        moveSpeedRushing = 100f;
    }

    private IEnumerator BossCharges()
    {
        isRushing = true;
       
        animator.SetTrigger(Skill);

        yield return new WaitForSeconds(2.1f);
        
        circleCollider.isTrigger = false;
        //add force that makes boss go to player with move speed rush
        rb.AddForce(((player.transform.position - transform.position).normalized) * (5 * moveSpeedRushing));
        timerRush = 0;
    }
    private void Update()
    {
        lastVelocity = rb.velocity;
    }
    protected override void FixedUpdate()
    {
        if (timerRush >= LimitTimeRush)
        {
            if (!isRushing)
            {
                StartCoroutine(BossCharges());
            }

            isRushing = true;
           
        }

        if (isRushing) return;
        base.FixedUpdate();
        timerRush += Time.fixedDeltaTime;

    }
    protected override void Stagger()
    {
        base.Stagger();
        isRushing = false;
        rb.velocity = Vector2.zero;
        StopAllCoroutinesBoss();
        circleCollider.isTrigger = true;
        timeBounce = 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("HardBlock"))
        {
           
            var speed = lastVelocity.magnitude;
           
            if (speed <= 4)
            {
                speed += 6;
            }
         
            var dir = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
           
            rb.velocity = dir * Mathf.Max(speed, 0f);
            timeBounce++;
        }
        if (timeBounce >= 4)
        {
            ResetCharge();
        }
    }
    private void ResetCharge()
    {
        timeBounce = 0;
        rb.velocity = Vector2.zero;
        isRushing = false;
        circleCollider.isTrigger = true;
       
        animator.SetTrigger(Move);
    }
    protected override void BossDeath()
    {
        base.BossDeath();
        gameObject.GetComponent<Boss3>().enabled = false;
       
    }
}
