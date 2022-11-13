using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Boss1 : BaseBoss
{
    [Tooltip("Start position of attack box of boss")]
   [SerializeField] private Transform attackArea;
    [SerializeField] private LayerMask playerHitPoint;
    [SerializeField] private bool isAttacking;
    [SerializeField] private float attackTimer;
    [SerializeField] private BoxCollider2D boxColliderAttackArea;
    [SerializeField] private List<Vector2> brickList = new List<Vector2>();
    [SerializeField] private Ease ease;
    [Tooltip("Range of attack box of boss")]
    [SerializeField] private Vector2 attackAreaTrigger = new Vector2(5, 3f);
    [SerializeField] private AudioSource audioSource;
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Skill = Animator.StringToHash("Skill");

    protected override void Start()
    {
        base.Start();
        isAttacking = false;
        attackTimer = 0;
    }

    //create a overlapBox to detect player, if it hit player do the attack function
   private void InAttackRange()
    {
        var hit2D = Physics2D.OverlapBox(attackArea.position, attackAreaTrigger, 0,playerHitPoint);
      //if over lap box hit player in this box then do attack else return
        if (!hit2D) return;
        isAttacking = true;
        attackTimer += Time.fixedDeltaTime;
        Attack();
    }
    //attached with skill animator stage to enable attack area's collider to detect player
    private void CheckAttackHitPlayer()
    {
        var hit2D = Physics2D.OverlapBox(attackArea.position, attackAreaTrigger, 0, playerHitPoint);
        if (hit2D)
        {
            boxColliderAttackArea.enabled = true;
        }
        audioSource.Play();
        DisplayBrick();
        Camera.main.DOShakePosition(0.2f, new Vector2(0,-0.5f), 0);
    }
    private void OnDrawGizmos()
    {
        //visualize that attack area
       Gizmos.DrawCube(attackArea.position, attackAreaTrigger);
    }
    //when boss attack it cant move 
    private void Attack()
    {
        moveSpeed = 0;
        animator.ResetTrigger("Move");
        animator.SetTrigger(Skill);
    }
  
    private void DisplayBrick()
    {
        for (var i = 0; i < brickList.Count; i++)
        {
            var brick = Pool.instance.GetBrick();
            var position = attackArea.position;
            brick.transform.position = position;
            brick.SetActive(true);

            brick.transform.DOJump(new Vector3(position.x - brickList[i].x, position.y - brickList[i].y), 2, 1, 0.4f).SetEase(ease).OnComplete(() => DisplaySmallBrick(brick, brickList));
        }
    }
    private void DisplaySmallBrick(GameObject ob, List<Vector2> list)
    {
        var temp = ob.name.Substring(ob.name.Length - 1);
        var index = int.Parse(temp) -1;
            ob.GetComponent<Brick>().ChangeSprite();
            var position = ob.transform.position;
            ob.transform.DOJump(new Vector3(position.x - list[index].x, position.y - list[index].y), 1f, 1, 0.4f).OnComplete(() => ob.GetComponent<Brick>().DisableGameObject()); 


    }
    protected override void FixedUpdate()
    {
        if (isAttacking == false || attackTimer == 0)
        {
            InAttackRange();
        }
     
        base.FixedUpdate();
       
    }
    //whenever boss want to move from a difference stage this function will help
    protected override void ReturnMoveStage()
    {
        // animator.ResetTrigger("Skill");
        animator.ResetTrigger("Hurt");
        animator.SetTrigger(Move);
        attackTimer = 0;
        isAttacking = false;
        moveSpeed = defaultMoveSpeed;
    }
    //this function attached to its animator stage
    private void DisableAttackAreaCollider()
    {
        boxColliderAttackArea.enabled = false;
    }
    protected override void BossDeath()
    {
        base.BossDeath();
        gameObject.GetComponent<Boss1>().enabled = false;
       
    }
    //this function attached to its animator stage
   private void ReturnMoveSpeedDefault()
    {
        
        moveSpeed = defaultMoveSpeed;
    }
}
