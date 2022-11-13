using UnityEngine;

public class RemoteControlPowerup : BasePowerupBuff
{
    protected override void ApplyBuffEffect(Collider2D collision)
    {
        BombController.instance.remoteControl = true;
        base.ApplyBuffEffect(collision);
    }
}
