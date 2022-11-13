using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public static BombController instance;
    [SerializeField] private LayerMask  layerPreventBombPlacement;
    private int _totalBomb;
    private int _currentNumberBomb;
    private int _explosionRadius;
    private GameObject kickedBomb;
    [SerializeField] private List<GameObject> listRemotedBomb;
    private bool haveRedBombPowerUp;
    private bool haveKickPowerUp;
    private bool haveRemoteControlPowerUp;
    [SerializeField] private PlayerSOScript playerSo;
    
    //properties


    public int currentNumberBomb
    {
        get => _currentNumberBomb;
        set
        {
            _currentNumberBomb = value;
        }
    }
    
    
    public int totalBomb
    {
        get => _totalBomb;
        set
        {
            _totalBomb = value;
            OnTotalBombChanged?.Invoke(_totalBomb);
        }
    }
    
    public int explosionRadius
    {
        get => _explosionRadius;
        set
        {
            _explosionRadius = value;
            OnExplosionRadiusChanged?.Invoke(_explosionRadius);
        }
    }
    
    public bool redBomb
    {
        get => haveRedBombPowerUp;
        set
        {
            haveRedBombPowerUp = value;
            OnRedBombPowerChanged?.Invoke(haveRedBombPowerUp);
        }
    }
    
    public bool kick
    {
        get => haveKickPowerUp;
        set
        {
            haveKickPowerUp = value;
            OnKickPowerChanged?.Invoke(haveKickPowerUp);
        }
    }
    
    public bool remoteControl
    {
        get => haveRemoteControlPowerUp;
        set
        {
            haveRemoteControlPowerUp = value;
            OnRemoteControlPowerChanged?.Invoke(haveRemoteControlPowerUp);
        }
    }
    
    //delegates and events
    public delegate void TotalBombChangeHandler(int number);
    public event TotalBombChangeHandler OnTotalBombChanged;  
    
    public delegate void ExplosionRadiusChangeHandler(int number);
    public event ExplosionRadiusChangeHandler OnExplosionRadiusChanged;  
    
    public delegate void RedBombPowerChangeHandler(bool state);
    public event RedBombPowerChangeHandler OnRedBombPowerChanged;  
    
    public delegate void KickPowerChangeHandler(bool state);
    public event KickPowerChangeHandler OnKickPowerChanged;  
    
    public delegate void RemoteControlPowerChangeHandler(bool state);
    public event RemoteControlPowerChangeHandler OnRemoteControlPowerChanged;  
    
    private void Awake()
    {
        instance = this;
        listRemotedBomb = new List<GameObject>();
        haveKickPowerUp = playerSo.isHaveKickPower;
        haveRedBombPowerUp = playerSo.isHaveRedBombPower;
        haveRemoteControlPowerUp = playerSo.isHaveRemotePower;
        totalBomb = playerSo.totalBomb;
        explosionRadius = playerSo.explosionRadius;
        currentNumberBomb = totalBomb;
        OnTotalBombChanged += Test;
    }

    private void Test(int number)
    {
        _currentNumberBomb += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 position = transform.position;
            position.x = Mathf.Round(position.x);
            //offset 
            position.y -= 0.36f;
            position.y = Mathf.Round(position.y);
            if (!Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, layerPreventBombPlacement) && currentNumberBomb>0)
            {
                PlaceTypeOfBomb(position);
            }
        }
        else if (Input.GetKeyDown(KeyCode.X)&& haveKickPowerUp)
        {
            kickedBomb.GetComponent<Bomb>().DisableBombCanMove();
        }
        else if(Input.GetKeyDown(KeyCode.Z) && haveRemoteControlPowerUp)
        {
            if (listRemotedBomb.Count>0)
            {
                var remoteBomb = listRemotedBomb[0];
                listRemotedBomb.Remove(remoteBomb);
                var bomb= remoteBomb.GetComponent<Bomb>();
                    if(bomb.IsBombIsTrigger()==true) return;
                    bomb.Explosion();
            }
            
        }
    }
    public void RemoveBombFromRemoteList(GameObject ob)
    {
        if (listRemotedBomb.Count <= 0) return;
        listRemotedBomb.Remove(ob);
    }
    
    private void PlaceTypeOfBomb(Vector2 position)
    {
        GameObject bomb;
        if (haveRedBombPowerUp && haveRemoteControlPowerUp)
        {
            bomb = Pool.instance.GetRedRemoteBomb();
            listRemotedBomb.Add(bomb);
        }
        else if (haveRemoteControlPowerUp)
        {
            bomb = Pool.instance.GetRemoteBomb();
            listRemotedBomb.Add(bomb);
        }
        else if (haveRedBombPowerUp)
        {
            bomb = Pool.instance.GetRedBomb();
        }
        else
        {
            bomb = Pool.instance.GetBomb();
        }
        BombMusicScript.instance.PlayBombPlacementSound();
        bomb.transform.position = position;
        bomb.GetComponent<Bomb>().SetupExplosionRadius(explosionRadius);
        bomb.SetActive(true);
    }
   
    
  
    
    // this function makes bomb move constantly with direction of player 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!haveKickPowerUp) return;
        if (!collision.gameObject.CompareTag("Bomb")) return;
        kickedBomb = collision.gameObject;
        if (!kickedBomb.GetComponent<Bomb>().IsBombCanBePushed()) return;
        PlayerMusicScript.instance.PlayKickBombSound();
        kickedBomb.GetComponent<Bomb>().SetBombDirection(PlayerMovement.instance.direction);
        kickedBomb.GetComponent<Bomb>().EnableBombCanMove();
    }

   
}
