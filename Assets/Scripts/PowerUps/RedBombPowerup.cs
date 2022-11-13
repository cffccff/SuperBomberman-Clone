using UnityEngine;

public class RedBombPowerup : BasePowerupBuff
{
    protected override void ApplyBuffEffect(Collider2D collision)
    {
        BombController.instance.redBomb = true;
        base.ApplyBuffEffect(collision);
    }
}
