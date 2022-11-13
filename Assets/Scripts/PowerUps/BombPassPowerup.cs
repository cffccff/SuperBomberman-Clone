using UnityEngine;

public class BombPassPowerup : BasePowerupBuff
{
    protected override void ApplyBuffEffect(Collider2D collision)
    {
        PlayerMovement.instance.bombPass = true;
        base.ApplyBuffEffect(collision);
    }
}
