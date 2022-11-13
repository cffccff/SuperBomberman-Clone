using DG.Tweening;
using UnityEngine;

public class Pakupa : EnemyMovement
{
    //boolean for flag number is integer or not
    private bool isInteger = false;
    // when cast the raycast for checking the bomb, we should add layer mask that contains only obstacles (soft block and hard block) and bomb because we need raycast through object like enemy, player, explosion
    // but cant though the obstacles 
    [SerializeField] LayerMask layerRaycast;
  
    protected override void FixedUpdate()
    {
        sensor = (Vector2)transform.position + (direction / 2f);    
        CheckBomb();
        CheckWayToMove();
        if (canMove == true)
        {
            rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
        }    
    }
    //this function ensure that when the pakupa detect a bomb, pakupa's position x / y will be integer depend on its direction 
    private void SetEnemyToTheLine(Vector2 currentDirection)
    {
        Vector3 transformPos;
        if (currentDirection == Vector2.up || currentDirection == Vector2.down)
        {
            //check the integer of position x
            isInteger = CheckNumberInteger(transform.position.x);
            //if x is not an integer so it mean the pakupa's position is now align so we align pakupa
            if (isInteger) return;
            transformPos = transform.position;
            transform.DOMove(new Vector2(Mathf.Round(transformPos.x), transformPos.y), 0.2f);
        }
        else if (currentDirection == Vector2.left || currentDirection == Vector2.right)
        {
            //check the integer of position y
            isInteger = CheckNumberInteger(transform.position.y);
            //if y is not an integer so it mean the pakupa's position is now align so we align pakupa
            if (isInteger) return;
            transformPos = transform.position;
            transform.DOMove(new Vector2(transformPos.x, Mathf.Round(transformPos.y)), 0.2f);
        }
    }
    //function that check a number is integer or not
    private bool CheckNumberInteger(float number)
    {
        return Mathf.Abs(number % 1) <= (System.Double.Epsilon * 100);
    }
 
    //function that cast 4 direction raycast from pakupa's direction to detect bomb 
    private void CheckBomb()
    {
        //may cause a delay or bug need to check again
        var transformPos = transform.position;
        RaycastHit2D up = Physics2D.Raycast(transformPos, Vector2.up, 8, layerRaycast);
        RaycastHit2D down = Physics2D.Raycast(transformPos, Vector2.down, 8, layerRaycast);
        RaycastHit2D left = Physics2D.Raycast(transformPos, Vector2.left, 20, layerRaycast);
        RaycastHit2D right = Physics2D.Raycast(transformPos, Vector2.right, 20, layerRaycast);
        //this condition check ensure that when pakupa chase a bomb in direction up and down, the next detected bomb will be priority is the bomb in up/down direction
        if (direction == Vector2.up || direction == Vector2.down)
        {
            //if one of the 4 direction raycast detect a bomb, set pakupa's direction = the raycast's direction and then return (not check other raycast)
            if (IsRayCastHitBomb(up, Vector2.up))
            {
                SetNewDirectionWhenSeeBomb(Vector2.up);
                return;
            }
            if (IsRayCastHitBomb(down, Vector2.down))
            {
                SetNewDirectionWhenSeeBomb(Vector2.down);
                return;
            }
            if (IsRayCastHitBomb(left, Vector2.left))
            {
                SetNewDirectionWhenSeeBomb(Vector2.left);
                return;
            }
            if (IsRayCastHitBomb(right, Vector2.right))
            {
                SetNewDirectionWhenSeeBomb(Vector2.right);
            }

            //For visualize the raycast
            //if (!IsRayCastHitBomb(up, Vector2.up))
            //{
            //    Debug.DrawRay(transform.position, vector2s[0] * 8f, Color.green, 0.1f);
            //}
            //if (!IsRayCastHitBomb(down, Vector2.down))
            //{
            //    Debug.DrawRay(transform.position, vector2s[1] * 8f, Color.green, 0.1f);
            //}
            //if (!IsRayCastHitBomb(left, Vector2.left))
            //{
            //    Debug.DrawRay(transform.position, vector2s[2] * 20f, Color.green, 0.1f);
            //}
            //if (!IsRayCastHitBomb(right, Vector2.right))
            //{
            //    Debug.DrawRay(transform.position, vector2s[3] * 20f, Color.green, 0.1f);
            //}
        }
        //this condition check ensure that when pakupa chase a bomb in diretion left/right, the next detected bomb will be priority is the bomb in left/right direction
        else if (direction == Vector2.left || direction == Vector2.right)
        {
            if (IsRayCastHitBomb(left, Vector2.left))
            {
                SetNewDirectionWhenSeeBomb(Vector2.left);
                return;
            }
            if (IsRayCastHitBomb(right, Vector2.right))
            {
                SetNewDirectionWhenSeeBomb(Vector2.right);
                return;
            }
            if (IsRayCastHitBomb(up, Vector2.up))
            {
                SetNewDirectionWhenSeeBomb(Vector2.up);
                return;
            }
            if (IsRayCastHitBomb(down, Vector2.down))
            {
                SetNewDirectionWhenSeeBomb(Vector2.down);
            }

            //For visualize the raycast
            //if (!IsRayCastHitBomb(left, Vector2.left))
            //{
            //    Debug.DrawRay(transform.position, vector2s[2] * 20f, Color.green, 0.1f);
            //}
            //if (!IsRayCastHitBomb(right, Vector2.right))
            //{
            //    Debug.DrawRay(transform.position, vector2s[3] * 20f, Color.green, 0.1f);
            //}
            //if (!IsRayCastHitBomb(up, Vector2.up))
            //{
            //    Debug.DrawRay(transform.position, vector2s[0] * 8f, Color.green, 0.1f);
            //}
            //if (!IsRayCastHitBomb(down, Vector2.down))
            //{
            //    Debug.DrawRay(transform.position, vector2s[1] * 8f, Color.green, 0.1f);
            //}
        }
    }
    //function that set new direction for pakupa and after that ensure the pakupa's position on the integer position
    private void SetNewDirectionWhenSeeBomb(Vector2 newDirection)
    {
        if(direction != newDirection)
        {
            direction = newDirection;
            SetEnemyToTheLine(direction);
            ChangeStateAnimator();
        }
        
    }
    //function check if the raycast hit a bomb
    private bool IsRayCastHitBomb(RaycastHit2D raycast, Vector2 newDirection)
    {
        if(raycast && raycast.collider.CompareTag("Bomb"))
        {
            if(direction == Vector2.up || direction == Vector2.down)
            {
                //for visualize raycast
               // Debug.DrawRay(sensor, newDirection * 8f, Color.red, 1f);

                return true;
            } 
            if(direction == Vector2.left || direction == Vector2.right)
            {
                //for visualize raycast
               // Debug.DrawRay(sensor, newDirection * 20f, Color.red, 1f);

                return true;
            }
            
        }
        return false;
    }
    // when pakupa collide with bomb's collider it will make that bomb disappear
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            collision.gameObject.SetActive(false);
            BombController.instance.currentNumberBomb += 1;
        }
    }
}
