using UnityEngine;

public class FireUpPowerup : BasePowerupBuff
{
    protected override void ApplyBuffEffect(Collider2D collision)
    {
        collision.gameObject.transform.GetChild(1).GetComponent<BombController>().explosionRadius += 1;
        base.ApplyBuffEffect(collision);
    }
}
