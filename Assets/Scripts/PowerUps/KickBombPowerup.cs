using UnityEngine;

public class KickBombPowerup : BasePowerupBuff
{
    protected override void ApplyBuffEffect(Collider2D collision)
    {
        BombController.instance.kick = true;
        base.ApplyBuffEffect(collision);       
    }
}
