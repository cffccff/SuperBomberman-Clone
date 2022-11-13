using UnityEngine;

public class RedBakuda : Bakuda
{
    protected override void DisplayBakudaExplosion()
    {
        spriteRenderer.enabled = false;
        gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        tempBomb = Pool.instance.GetEnemyRedBomb();
        tempBomb.transform.position = transform.position;
        tempBomb.SetActive(true);
    }

}
