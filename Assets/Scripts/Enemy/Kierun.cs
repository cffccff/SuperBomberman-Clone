using UnityEngine;

public class Kierun : EnemyMoveAimless
{
    [SerializeField] private int timerSkill = 3;
    [SerializeField] private float timer;
    protected override void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= timerSkill)
        {
           
            animator.SetTrigger("Skill");
           

        }
        else
        {
            base.FixedUpdate();
        }
      
    }
    private void ReturnNormalStage()
    {
        animator.ResetTrigger("Skill");
        animator.SetTrigger("Move");
        timer = 0;
    }
}
