using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{
    //the move speed of enemy
    [SerializeField] protected float moveSpeed = 2f;
    protected float defaultMoveSpeed;
    //layer mask that raycast detect obstacles in map include enemy, bomb, blocks
    [SerializeField] protected LayerMask obstaclesLayer;
    protected Animator animator;
    protected Rigidbody2D rb;
    //direction of enemy's movement
    [SerializeField] protected Vector2 direction;
    protected SpriteRenderer spriteRenderer;
    // position to cast raycast
    [SerializeField] protected Vector2 sensor;
    protected readonly List<Vector2> vector2FourDirection = new List<Vector2>();
    protected RaycastHit2D hit;
    protected bool chooseNewDirection;

    private readonly Material newMaterial;
    private Material defaultMaterial;

    public bool isDisableByBomb { get; set; }

    protected bool canMove;
    // create list that contains 4 directions
    private void PopulateList()
    {
        vector2FourDirection.Add(Vector2.up);
        vector2FourDirection.Add(Vector2.down);
        vector2FourDirection.Add(Vector2.left);
        vector2FourDirection.Add(Vector2.right);
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
    }
    protected virtual void Start()
    {
        defaultMoveSpeed = moveSpeed;
        PopulateList();
        //set default direction for enemy
        direction = Vector2.left;
        chooseNewDirection = true;
        canMove = true;
        isDisableByBomb = false;
        ChangeStateAnimator();

    }
    
    // function ensure that display sprite follow the current direction
    protected void ChangeStateAnimator()
    {
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        if (direction.x >= 1)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        animator.SetFloat("Speed", direction.sqrMagnitude);
    }
    protected virtual void FixedUpdate()
    {
        sensor = (Vector2)transform.position + (direction / 2f);
        CheckWayToMove();
        if (canMove == true)
        {
            rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));
        }
        
    }

    //when enemy is hit by a bomb then can not move
   private IEnumerator DisableEnemy()
    {
        canMove = false;
        for (float i = 0; i < 0.5f; i += 0.1f)
        {

            yield return new WaitForSeconds(0.05f);
            spriteRenderer.material = newMaterial;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.material = defaultMaterial;
        }
        canMove = true;
        isDisableByBomb = false;
    }

   public void DisableEnemyByHitBomb()
   {
       if (isDisableByBomb)
       {
           StartCoroutine(DisableEnemy());
       }
   }
   
    protected virtual void FindClearDirectionToMove()
    {
        chooseNewDirection = false;
        //set enemy speed to 0, for purpose ensure new diretion clear, if clear then speed is normal
        moveSpeed = 0;
        //random new direction
        direction = vector2FourDirection[Random.Range(0, 4)];
        //return the default enemy's speed
        Invoke(nameof(SetMoveSpeedDelay), 0f);
    }


    //function detect the obstacle then if have obstacles choose new direction that clear to move
    protected virtual void CheckWayToMove()
    {
        //condition check for boxcast, it ensures that when cast a box to detect way is clear or not, it will not return itself collider so that only check the way is blocks, bomb or other enemy
        if (direction == Vector2.up || direction == Vector2.down)
        {
            hit = Physics2D.BoxCast(sensor, new Vector2(0.9f, 0.008f), 0, direction, 0.005f, obstaclesLayer);
            //if hit then it mean next cell is have obstacle
            if (hit)
            {
                FindClearDirectionToMove();
            }
            //if not then the way is clear
            else
            {
               // DrawBoxCast2D(sensor, new Vector2(0.9f, 0.008f), 0, direction, 0.005f, Color.black);
                if (chooseNewDirection == false) 
                    ChangeStateAnimator();
                chooseNewDirection = true;
               
            }
        }
        else if(direction == Vector2.left || direction == Vector2.right)
        {
            hit = Physics2D.BoxCast(sensor, new Vector2(0.008f, 0.9f), 0, direction, 0.005f, obstaclesLayer);
            if (hit)
            {
                FindClearDirectionToMove();
            }
            else
            {
               // DrawBoxCast2D(sensor, new Vector2(0.008f, 0.9f), 0, direction, 0.005f, Color.black);
                if (chooseNewDirection == false) 
                    ChangeStateAnimator();
                chooseNewDirection = true;
            }
        }
    }
    protected void SetMoveSpeedDelay()
    {
        moveSpeed = defaultMoveSpeed;
    }
    //function ensure that new random direction will be not old direction
    public Vector2 RandomExcept(int min, int max, Vector2 newDirection)
    {
        var random = Random.Range(min, max);
        if (newDirection == vector2FourDirection[random]) random = (random + 1) % max;
        return vector2FourDirection[random];
    }

    






    //Draws just the box at where it is currently hitting.
    public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color)
    {
        origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
        DrawBox(origin, halfExtents, orientation, color);
    }

    //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
    public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
    {
        direction.Normalize();
        Box bottomBox = new Box(origin, halfExtents, orientation);
        Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

        Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color);
        Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color);
        Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color);
        Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color);
        Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color);
        Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color);
        Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color);
        Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color);

        DrawBox(bottomBox, color);
        DrawBox(topBox, color);
    }
    public static void DrawBoxCast(Vector2 origin, Vector2 halfExtents, Quaternion orientation, Vector2 direction, float distance, Color color)
    {
        float time = 0.5f;
        direction.Normalize();
        Box bottomBox = new Box(origin, halfExtents, orientation);
        Box topBox = new Box(origin + (direction * distance), halfExtents);

        Debug.DrawLine(bottomBox.backBottomLeft, topBox.backBottomLeft, color, time);
        Debug.DrawLine(bottomBox.backBottomRight, topBox.backBottomRight, color, time);
        Debug.DrawLine(bottomBox.backTopLeft, topBox.backTopLeft, color, time);
        Debug.DrawLine(bottomBox.backTopRight, topBox.backTopRight, color, time);
        Debug.DrawLine(bottomBox.frontTopLeft, topBox.frontTopLeft, color, time);
        Debug.DrawLine(bottomBox.frontTopRight, topBox.frontTopRight, color, time);
        Debug.DrawLine(bottomBox.frontBottomLeft, topBox.frontBottomLeft, color, time);
        Debug.DrawLine(bottomBox.frontBottomRight, topBox.frontBottomRight, color, time);

        DrawBox(bottomBox, color);
        DrawBox(topBox, color);
    }
    public static void DrawBoxCast2D(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, Color color)
    {
        Quaternion angle_z = Quaternion.Euler(0f, 0f, angle);
        DrawBoxCast(origin, size / 2f, angle_z, direction, distance, color);
    }

    public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
    {
        DrawBox(new Box(origin, halfExtents, orientation), color);
    }
    public static void DrawBox(Box box, Color color)
    {
        Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color);
        Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color);
        Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color);
        Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color);

        Debug.DrawLine(box.backTopLeft, box.backTopRight, color);
        Debug.DrawLine(box.backTopRight, box.backBottomRight, color);
        Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color);
        Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color);

        Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color);
        Debug.DrawLine(box.frontTopRight, box.backTopRight, color);
        Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color);
        Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color);
    }

    public struct Box
    {
        public Vector3 localFrontTopLeft { get; private set; }
        public Vector3 localFrontTopRight { get; private set; }
        public Vector3 localFrontBottomLeft { get; private set; }
        public Vector3 localFrontBottomRight { get; private set; }
        public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
        public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
        public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
        public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

        public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
        public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
        public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
        public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
        public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
        public Vector3 backTopRight { get { return localBackTopRight + origin; } }
        public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
        public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

        public Vector3 origin { get; private set; }

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
        {
            Rotate(orientation);
        }
        public Box(Vector3 origin, Vector3 halfExtents)
        {
            this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
            this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

            this.origin = origin;
        }


        public void Rotate(Quaternion orientation)
        {
            localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
            localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
            localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
            localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
        }
    }

    //This should work for all cast types
    static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
    {
        return origin + (direction.normalized * hitInfoDistance);
    }

    static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot;
        return pivot + rotation * direction;
    }

}
