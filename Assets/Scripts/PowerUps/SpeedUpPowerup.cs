using UnityEngine;

public class SpeedUpPowerup : BasePowerupBuff
{
    protected override void ApplyBuffEffect(Collider2D collision)
    {
        collision.GetComponent<PlayerMovement>().moveSpeed += 1;
        base.ApplyBuffEffect(collision);     
    }
}
