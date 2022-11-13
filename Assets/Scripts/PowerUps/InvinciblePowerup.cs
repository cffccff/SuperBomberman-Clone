using UnityEngine;

public class InvinciblePowerup : BasePowerupBuff
{
    protected override void ApplyBuffEffect(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerHealth>().ApplyInvincibleToPlayer();
        base.ApplyBuffEffect(collision);
    }
}
