using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    [Tooltip("Reference to Scriptable object that contains data of black bomb")]
    public BombSOScript bombSo;
    [Tooltip("when a bomb exploded the fire will spread 4 directions with the range equal radius")]
    [SerializeField] protected int radius;
    [SerializeField] protected Tilemap softBlockTilemap;
    private CircleCollider2D circleCollider2;
    private SpriteRenderer spriteRenderer;
    private Vector2 position;
    protected RaycastHit2D raycastHit2D;
    private Rigidbody2D rb;
    [Tooltip("The status of the bomb that determine it can move or not")]
    [SerializeField] private bool canMove;
    [Tooltip("The status of the bomb that determine it can be pushed or not")]
    [SerializeField] private bool canBePushed;
    [Tooltip("When the bomb is moving this display the bomb's direction")]
    [SerializeField] private Vector2 direction;
    // bool indicate this bomb is triggered by other bomb or not
    private bool isTriggered;
    public void SetupExplosionRadius(int bombRadius)
    {
        radius = bombRadius;
    }
    protected virtual void OnDisable()
    {
        ResetToNormalBomb();
        CancelInvoke();
    }

    protected void ResetToNormalBomb()
    {
        circleCollider2.enabled = true;
        circleCollider2.isTrigger = true;
        spriteRenderer.enabled = true;
        canMove = false;
        canBePushed = false;
        isTriggered = false;
    }

    protected void GetTileMapIfNull()
    {
        if(softBlockTilemap==null)
        {
            softBlockTilemap = GameObject.FindWithTag("SoftBlock").GetComponent<Tilemap>();
        }
    }
    
    protected virtual void OnEnable()
    {
        GetTileMapIfNull();
        BombController.instance.currentNumberBomb -= 1;
        Invoke(nameof(Explosion), bombSo.bombFuseTime);
    }
    private void Awake()
    {
        circleCollider2 = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        canMove = false;
        canBePushed = false;
        isTriggered = false;
    }
    protected virtual void Start()
    {
        
    }
    
    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + direction * (bombSo.moveSpeed * Time.fixedDeltaTime));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var go = collision.gameObject;
        
        //this condition check if game object collision layer is in stopBombMove layer or not
        // if (stopBombPushedLayer == (stopBombPushedLayer | (1 << go.layer)))
        if (bombSo.stopBombPushedLayer != (bombSo.stopBombPushedLayer | (1 << go.layer))) return;
      
        if (go.CompareTag("Enemy") && canMove)
        {
            EnemyMovement movement = go.GetComponent<EnemyMovement>();
            movement.isDisableByBomb = true;
            movement.DisableEnemyByHitBomb();
        }
        canMove = false;
        var position1 = transform.position;
        position1 =new Vector2( Mathf.Round(position1.x), Mathf.Round(position1.y));
        transform.position = position1;
    }
    public void SetBombDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }
    public virtual void Explosion()
    {
        if (isTriggered == true) return;
        GetBombPosition();
        StartExplosion();
        BombMusicScript.instance.PlayBombExplosionSound();
        BombController.instance.currentNumberBomb += 1;
        DisplayExplosionOn4Direction();
        Invoke(nameof(DeActiveBomb), 1f);
    }
   
    protected void StartExplosion()
    {
        circleCollider2.enabled = false;
        spriteRenderer.enabled = false;
        var exp = Pool.instance.GetExplosionStart();
        exp.transform.position = position;
        exp.SetActive(true);
        exp.GetComponent<Explosion>().belongToBomb = gameObject;
    }
    protected void GetBombPosition()
    {
        position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
    }
    protected void DisplayExplosionOn4Direction()
    {
        DisplayExplosion(position, Vector2.up, radius);
        DisplayExplosion(position, Vector2.down, radius);
        DisplayExplosion(position, Vector2.left, radius);
        DisplayExplosion(position, Vector2.right, radius);

    }
    protected IEnumerator BombChainExplosion(RaycastHit2D raycastHit)
    {
        //if it is triggered by a bomb then can not be triggered anymore
        if (isTriggered == false)
        {
            isTriggered = true;
            Debug.Log("Enter Bomb Chain");
            BombController.instance.RemoveBombFromRemoteList(raycastHit.collider.gameObject);
            Bomb bomb = raycastHit.collider.gameObject.GetComponent<Bomb>();
            bomb.CancelInvoke();
            yield return new WaitForSeconds(0.05f); 
            BombMusicScript.instance.PlayBombExplosionSound();
            bomb.Explosion();
            Debug.Log($"current bomb is {BombController.instance.currentNumberBomb.ToString()}");
        }
        
    }

    public bool IsBombIsTrigger()
    {
        return isTriggered;
    }
    
    protected virtual void DisplayExplosion(Vector2 positionExplosion, Vector2 directionExplosion, int length)
    {
        for (var i = 0; i < length; i++)
        {
       
            GameObject explosion;
            //where should raycast start cast a ray
            var startPoint = positionExplosion +directionExplosion/2;
            positionExplosion += directionExplosion;
            raycastHit2D = Physics2D.Raycast(startPoint, directionExplosion, 0.5f, bombSo.explosionAffectLayer);
            
             // raycastHit2D = Physics2D.BoxCast(positionExplosion, Vector2.one, 0, directionExplosion, 0f,
             //     bombSo.explosionAffectLayer);
            if (raycastHit2D)
            {
                if (raycastHit2D.collider.CompareTag("Bomb"))
                {
                    StartCoroutine(BombChainExplosion(raycastHit2D));
                }
                else if (raycastHit2D.collider.CompareTag("HardBlock"))
                {
                    break;
                }
                else if (raycastHit2D.collider.CompareTag("SoftBlock"))
                {
                    ClearSoftBlock(positionExplosion);
                    break;
                }
                else if (raycastHit2D.collider.CompareTag("Gate"))
                {
                    Gate.instance.SpawnEnemy();
                    break;
                }
            }
        

            //because all the explosion prefab is animated horizontally from left to right so if the explosion is vertical, we have to rotate it to display animation correctly
            Quaternion quaternion = SetDirectionAnimation(directionExplosion);
            //if the length is larger than 1 it means the explosion radius does not reach the limit yet, the animation will be displayed is mid explosion. Otherwise, the animation will be the end explosion
            if (i == length - 1)
            {
                explosion = Pool.instance.GetExplosionEnd();
                explosion.transform.position = positionExplosion;
                explosion.transform.rotation = quaternion;


            }
            else
            {
                explosion = Pool.instance.GetExplosionMid();
                explosion.transform.position = positionExplosion;
                explosion.transform.rotation = quaternion;


            }

            explosion.SetActive(true);
            explosion.GetComponent<Explosion>().belongToBomb = gameObject;

        }

    }
    protected void DeActiveBomb()
    {
        gameObject.SetActive(false);
    }
    protected void ClearSoftBlock(Vector2 clearPosition)
    {
        var cell = softBlockTilemap.WorldToCell(clearPosition);
        var tile = softBlockTilemap.GetTile(cell);

        if (tile == null) return;
        for (var i = 0; i < bombSo.softBlocks.Count; i++)
        {
            if (bombSo.softBlocks[i] != tile) continue;
            Instantiate(bombSo.softBlocksDestroy[i], clearPosition, Quaternion.identity);
            softBlockTilemap.SetTile(cell, null);
        }
    }
    //function that determine angle of the explosion animation to display correctly depend on the direction
    protected Quaternion SetDirectionAnimation(Vector2 directionExplosion)
    {
        if (directionExplosion.Equals(Vector2.up))
        {
            return Quaternion.Euler(0f, 0f, 90f);
        }
        else if (directionExplosion.Equals(Vector2.down))
        {
            return Quaternion.Euler(0f, 0f, -90f);
        }
        else if (directionExplosion.Equals(Vector2.left))
        {
            return Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            return Quaternion.Euler(0f, 0f, 0f);
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //wth is that
      //  if (!collision.gameObject.CompareTag("BombCheck")) return;
        circleCollider2.isTrigger = false;
        canBePushed = true;
    }

    public void EnableBombCanMove()
    {
        canMove = true;
    }
    public void DisableBombCanMove()
    {
        canMove = false;
        var position1 = transform.position;
        position1 = new Vector2(Mathf.Round(position1.x), Mathf.Round(position1.y));
        transform.position = position1;
    }
    public bool IsBombCanBePushed()
    {
        return canBePushed;
    }
    
}
