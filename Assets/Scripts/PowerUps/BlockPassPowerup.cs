using UnityEngine;

public class BlockPassPowerup : BasePowerupBuff
{
    protected override void ApplyBuffEffect(Collider2D collision)
    {
        PlayerMovement.instance.blockPass = true;
        base.ApplyBuffEffect(collision);     
    }
}
