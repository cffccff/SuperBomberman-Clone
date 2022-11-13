using UnityEngine;
using DG.Tweening;
public class EnemyFollowPlayer : EnemyMovement
{
    //boolean for flag number is integer or not
    private bool isInteger = false;
    // when cast the raycast for checking the object for chasing, we should add layer mask that contains only obstacles (soft block and hard block/bomb) because we need raycast through object like enemy, player, explosion
    // but cant though the obstacles 
    [SerializeField] LayerMask layerRaycastChaseObject;


    protected override void FixedUpdate()
    {
        sensor = (Vector2)transform.position + (direction / 2f);
        CheckPlayer();
        CheckWayToMove();
        if (canMove == true)
        {
            rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
        }
      
    }
    //this function ensure that when the enemy detect a player, enemy's position x / y will be integer depend on its direction 
    protected virtual void SetEnemyToTheLine(Vector2 currentDirection)
    {
        Vector3 transformPos;
        if (currentDirection == Vector2.up || currentDirection == Vector2.down)
        {
            //check the integer of position x
            isInteger = CheckNumberInteger(transform.position.x);
            //if x is not an integer so it mean the enemy's position is now align so we align pakupa
            if (!isInteger)
            {
                transformPos = transform.position;
              //  transform.position = new Vector2(Mathf.Round(transform.position.x), transform.position.y);
                transform.DOMove(new Vector2(Mathf.Round(transformPos.x), transformPos.y), 0.05f);
            }
        }
        else if (currentDirection == Vector2.left || currentDirection == Vector2.right)
        {
            //check the integer of position y
            isInteger = CheckNumberInteger(transform.position.y);
            //if y is not an integer so it mean the enemy's position is now align so we align pakupa
            if (!isInteger)
            {
                transformPos = transform.position;
              //  transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y));
                transform.DOMove(new Vector2(transformPos.x, Mathf.Round(transformPos.y)), 0.05f);
            }
        }
    }
    //function that check a number is integer or not
    private bool CheckNumberInteger(float number)
    {
        return Mathf.Abs(number % 1) <= (System.Double.Epsilon * 100);
    }

    //function that cast 4 direction raycast from enemy's direction to detect player 
    protected virtual void CheckPlayer()
    {
        //may cause a delay or bug need to check again
        var transformPos = transform.position;
        RaycastHit2D up = Physics2D.Raycast(transformPos, Vector2.up, 8, layerRaycastChaseObject);
        RaycastHit2D down = Physics2D.Raycast(transformPos, Vector2.down, 8, layerRaycastChaseObject);
        RaycastHit2D left = Physics2D.Raycast(transformPos, Vector2.left, 20, layerRaycastChaseObject);
        RaycastHit2D right = Physics2D.Raycast(transformPos, Vector2.right, 20, layerRaycastChaseObject);

        if (IsRayCastHitPlayer(up, Vector2.up))
        {
            SetNewDirectionWhenSeePlayer(Vector2.up);
            return;
        }
        if (IsRayCastHitPlayer(down, Vector2.down))
        {
            SetNewDirectionWhenSeePlayer(Vector2.down);
            return;
        }
        if (IsRayCastHitPlayer(left, Vector2.left))
        {
            SetNewDirectionWhenSeePlayer(Vector2.left);
            return;
        }
        if (IsRayCastHitPlayer(right, Vector2.right))
        {
            SetNewDirectionWhenSeePlayer(Vector2.right);
        }

    }
    //function that set new direction for enemy and after that ensure the enemy's position on the integer position
    protected virtual void SetNewDirectionWhenSeePlayer(Vector2 newDirection)
    {
        if (direction == newDirection) return;
        direction = newDirection;
        SetEnemyToTheLine(direction);

    }
    //function check if the raycast hit a enemy
    private bool IsRayCastHitPlayer(RaycastHit2D raycast, Vector2 newDirection)
    {
        if (raycast && raycast.collider.CompareTag("PlayerHitPoint"))
        {
            if (direction == Vector2.up || direction == Vector2.down)
            {
                //for visualize raycast
                Debug.DrawRay(sensor, newDirection * 8f, Color.red, 1f);

                return true;
            }
            else if (direction == Vector2.left || direction == Vector2.right)
            {
                //for visualize raycast
                Debug.DrawRay(sensor, newDirection * 20f, Color.red, 1f);

                return true;
            }

        }
        return false;
    }
}
