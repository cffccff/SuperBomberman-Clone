using UnityEngine;
public class Boss2 : BaseBoss
{
    [SerializeField] int bulletsAmount = 10;
    [SerializeField] float startAngle = 90f, endAngle = 270f;
    
    private static readonly int Skill = Animator.StringToHash("Skill");
    private static readonly int Hurt = Animator.StringToHash("Hurt");

    private void Fire()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt")) return;
        var angleStep = (endAngle - startAngle) / bulletsAmount;
        var angle = startAngle;
        for (var i = 0; i < bulletsAmount + 1; i++)
        {
            var position = transform.position;
            var bulDirX = position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            var bulDirY = position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            var bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            var bulDir = (bulMoveVector - position).normalized;
            var bullet = Pool.instance.GetBullet();
            bullet.GetComponent<BulletScript>().SetMoveDirection(bulDir);
            bullet.transform.position = position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);
            angle += angleStep;


        }
        // SetMoveSpeedZero();

    }
  // this function attached with animator state Hurt
    protected void ChargePlayer()
    {
        moveSpeed = 3;
        animator.ResetTrigger(Hurt);
        animator.SetTrigger(Skill);
      
    }
    protected override void Stagger()
    {
        if (currentHealth > 0)
        {
            SetMoveSpeedZero();
            Fire();
            animator.ResetTrigger(Skill);
            animator.SetTrigger(Hurt);
        }
    }
    protected override void BossDeath()
    {
        base.BossDeath();
        gameObject.GetComponent<Boss2>().enabled = false;
       
    }
}
