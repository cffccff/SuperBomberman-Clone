using UnityEngine;

public class EnemyRedBomb : EnemyBlackBomb
{
    protected override void DisplayExplosion(Vector2 position, Vector2 direction, int length)
    {
        for (var i = 0; i < length; i++)
        {

            GameObject explosion;
            var startPoint = position +direction/2;
            position += direction;
            raycastHit2D = Physics2D.Raycast(startPoint, direction, 0.5f, bombSo.explosionAffectLayer);
            if (raycastHit2D)
            {
                if (raycastHit2D.collider.CompareTag("Bomb"))
                {
                    StartCoroutine(BombChainExplosion(raycastHit2D));
                }
                else if(raycastHit2D.collider.CompareTag("SoftBlock"))
                {
                    ClearSoftBlock(position);

                }
                else if (raycastHit2D.collider.CompareTag("HardBlock"))
                {
                    break;
                }
                else if (raycastHit2D.collider.CompareTag("Gate"))
                {
                    Gate.instance.SpawnEnemy();
                    break;
                }
            }


            //because all the explosion prefab is animated horizontally from left to right so if the explosion is vertical, we have to rotate it to display animation correctly
            Quaternion quaternion = SetDirectionAnimation(direction);
            //if the length is larger than 1 it means the explosion radius does not reach the limit yet, the animation will be displayed is mid explosion. Otherwise, the animation will be the end explosion
            if (i == length - 1)
            {
                explosion = Pool.instance.GetExplosionEnd();
                explosion.transform.position = position;
                explosion.transform.rotation = quaternion;


            }
            else
            {
                explosion = Pool.instance.GetExplosionMid();
                explosion.transform.position = position;
                explosion.transform.rotation = quaternion;


            }

            explosion.SetActive(true);
            explosion.GetComponent<Explosion>().belongToBomb = gameObject;

        }

    }
}
