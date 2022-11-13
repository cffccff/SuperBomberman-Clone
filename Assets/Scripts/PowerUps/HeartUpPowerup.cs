using UnityEngine;
public class HeartUpPowerup : BasePowerupBuff
{
    protected override void ApplyBuffEffect(Collider2D collision)
    {
        collision.GetComponent<PlayerHealth>().Health += 1;
        base.ApplyBuffEffect(collision);
    }
}
