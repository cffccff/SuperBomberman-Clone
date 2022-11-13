using DG.Tweening;
using UnityEngine;
public class Kuwagen : EnemyMovement
{

    // when enemy charging cant stop when detect player
    private bool isCharging;
    [SerializeField] private float moveSpeedCharging = 9f;
   
    //boolean for flag number is integer or not
    protected bool isInteger = false;
    // when cast the raycast for checking the object for chasing, we should add layer mask that contains only obstacles (soft block and hard block/bomb)
    // because we need raycast through object like enemy, player, explosion
    // but cant though the obstacles 
    [SerializeField] private LayerMask layerRaycastChaseObject;


   
   


    //function that cast 4 direction raycast from enemy's direction to detect player 
    protected virtual void CheckPlayer()
    {
        //may cause a delay or bug need to check again
        var transformPos = transform.position;
        RaycastHit2D up = Physics2D.Raycast(transformPos, Vector2.up, 11, layerRaycastChaseObject);
        RaycastHit2D down = Physics2D.Raycast(transformPos, Vector2.down, 11, layerRaycastChaseObject);
        RaycastHit2D left = Physics2D.Raycast(transformPos, Vector2.left, 13, layerRaycastChaseObject);
        RaycastHit2D right = Physics2D.Raycast(transformPos, Vector2.right, 13, layerRaycastChaseObject);

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

    private void SetNewDirectionWhenSeePlayer(Vector2 newDirection)
    {
        isCharging = true;
        canMove = false;
        direction = newDirection;
        SetEnemyToTheLine();
        ChangeStateAnimator();
        Invoke(nameof(ChangeMoveSpeedCharging),1);
    }

    private void ChangeMoveSpeedCharging()
    {
        moveSpeed = moveSpeedCharging;
        canMove = true;
    }
    
    private void SetEnemyToTheLine()
    {
        var position = transform.position;
        transform.DOMove(new Vector2(Mathf.Round(position.x), Mathf.Round(position.y)), 0.2f);
    }
    
    //function check if the raycast hit a enemy
    private bool IsRayCastHitPlayer(RaycastHit2D raycast, Vector2 newDirection)
    {
        if (raycast && raycast.collider.CompareTag("PlayerHitPoint"))
        {
            if (direction == Vector2.up || direction == Vector2.down)
            {
                //for visualize raycast
                Debug.DrawRay(transform.position, newDirection * 11f, Color.red, 1f);

                return true;
            }
            else if (direction == Vector2.left || direction == Vector2.right)
            {
                //for visualize raycast
                Debug.DrawRay(transform.position, newDirection * 13f, Color.red, 1f);

                return true;
            }

        }
        return false;
    }
    protected override void FixedUpdate()
    {
        sensor = (Vector2)transform.position + (direction / 2f);
        if (isCharging==false) CheckPlayer();
        
        if (canMove)
        {
            CheckWayToMove();
            rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
        }
    }
    protected override void CheckWayToMove()
    {
        //condition check for boxcast it ensure that when cast a box to detect way is clear or not, it will not return itself collider so that only check the way is blocks, bomb or other enemy
        if (direction == Vector2.up || direction == Vector2.down)
        {
            hit = Physics2D.BoxCast(sensor, new Vector2(0.95f, 0.008f), 0, direction, 0.005f, obstaclesLayer);
            //if hit then it mean next cell is have obstacle
            if (hit)
            {
                FindClearDirectionToMove();
                isCharging = false;
 
            }
            //if not then the way is clear
            else
            {
                // for visualize the raycast for checking obstacles that enemy cant move on
                // Debug.DrawRay(sensor, new Vector2(0.95f, 0.008f));
                if (chooseNewDirection == false)
                    ChangeStateAnimator();
                chooseNewDirection = true;

            }
        }
        else if (direction == Vector2.left || direction == Vector2.right)
        {
            hit = Physics2D.BoxCast(sensor, new Vector2(0.008f, 0.95f), 0, direction, 0.005f, obstaclesLayer);
            if (hit)
            {
                FindClearDirectionToMove();
                isCharging = false;

            }
            else
            {
                // for visualize the raycast for checking obstacles that enemy cant move on
                //  Debug.DrawRay(sensor, new Vector2(0.008f,0.95f));
                if (chooseNewDirection == false)
                    ChangeStateAnimator();
                chooseNewDirection = true;
            }
        }
    }


    // //function that set new direction for enemy and after that ensure the enemy's position on the integer position
    // private void SetNewDirectionWhenSeePlayer(Vector2 newDirection)
    // {
    //     if (direction != newDirection)
    //     {
    //         direction = newDirection;
    //         SetEnemyToTheLine(direction);
    //        
    //
    //
    //     }
    //     
    //     if (transform.position.x == Mathf.Round(transform.position.x) && transform.position.y == Mathf.Round(transform.position.y) && moveSpeed ==defaultMoveSpeed)
    //     {
    //         moveSpeed = 0;
    //         ChangeStateAnimator();
    //         Invoke(nameof(ChangeMoveSpeedCharging), 1f);
    //     }
    //            
    //     
    //
    // }
    //
    // //check again or this will be a bug
    // private void SetEnemyToTheLine(Vector2 direction)
    // {      
    //             transform.DOMove(new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), 0.05f);
    // }
    // private void ChangeMoveSpeedCharging()
    // {
    //     moveSpeed = moveSpeedCharging;
    //     charging = true;
    // }

}
