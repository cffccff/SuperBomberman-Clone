using UnityEngine;

public class BombUpPowerup : BasePowerupBuff
{
    protected override void ApplyBuffEffect(Collider2D collision)
    {
        collision.gameObject.transform.GetChild(1).GetComponent<BombController>().totalBomb += 1;
        base.ApplyBuffEffect(collision);              
    }
}
