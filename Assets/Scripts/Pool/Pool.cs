using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public static Pool instance;
    [SerializeField] private GameObject  pooledBomb;
    [SerializeField] private GameObject pooledRedBomb;
    [SerializeField] private GameObject pooledRemoteBomb;
    [SerializeField] private GameObject pooledRedRemoteBomb;
    [SerializeField] private GameObject pooledExplosionStart;
    [SerializeField] private GameObject pooledExplosionMid;
    [SerializeField] private GameObject pooledExplosionEnd;
    [SerializeField] private GameObject pooledEnemyBlackBomb;
    [SerializeField] private GameObject pooledEnemyRedBomb;
    [SerializeField] private GameObject pooledEnemyBullet;
    [SerializeField] private GameObject pooledDestroyObjectAnimation;
    [SerializeField] private GameObject pooledBrick;
    private const bool NotEnoughObjectPool = true;
    [SerializeField] private List<GameObject> listBombs;
    [SerializeField] private List<GameObject> listRedBombs;
    [SerializeField] private List<GameObject> listRemoteBombs;
    [SerializeField] private List<GameObject> listRedRemoteBombs;
    [SerializeField] private List<GameObject> listExplosionStarts;
    [SerializeField] private List<GameObject> listExplosionMids;
    [SerializeField] private List<GameObject> listExplosionEnds;
    [SerializeField] private List<GameObject> listEnemyBlackBombs;
    [SerializeField] private List<GameObject> listEnemyRedBombs;
    [SerializeField] private List<GameObject> listEnemyBullets;
    [SerializeField] private List<GameObject> listDestroyObjectAnimation;
    [SerializeField] private List<GameObject> listBrick;
    [SerializeField] private Transform explosionContainer;
    [SerializeField] private Transform bombContainer;
    [SerializeField] private Transform redBombContainer;
    [SerializeField] private Transform remoteBombContainer;
    [SerializeField] private Transform redRemoteBombContainer;
    [SerializeField] private Transform enemyBlackBombContainer;
    [SerializeField] private Transform enemyRedBombContainer;
    [SerializeField] private Transform enemyBulletContainer;
    [SerializeField] private Transform destroyObjectAnimationContainer;
    [SerializeField] private Transform brickContainer;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
        listBombs = new List<GameObject>();
        listRemoteBombs = new List<GameObject>();
        listRedBombs = new List<GameObject>();
        listRedRemoteBombs = new List<GameObject>();
        listExplosionStarts = new List<GameObject>();
        listExplosionMids = new List<GameObject>();
        listExplosionEnds = new List<GameObject>();
        listEnemyBlackBombs = new List<GameObject>();
        listEnemyRedBombs = new List<GameObject>();
        listEnemyBullets = new List<GameObject>();
        listDestroyObjectAnimation = new List<GameObject>();
        listBrick = new List<GameObject>();
        PopulatedPool();
        DontDestroyOnLoad(gameObject);
    }

   

    private void AddObjectIntoList(List<GameObject> list, string obName, GameObject pooledGameObject, Transform container,int index)
    {
        var temp = Instantiate(pooledGameObject, container, true);
       
        temp.name = $"{obName}{index.ToString()}";
        list.Add(temp);
    }
    private void PopulatedPool()
    {
        var index =0;
        for(var i = 0; i <= 100; i++)
        {
            index++;
            if (i <= 5)
            {
                AddObjectIntoList(listExplosionStarts,"StartExp_" ,pooledExplosionStart, explosionContainer,index);
                AddObjectIntoList(listBombs, "Bomb_", pooledBomb, bombContainer, index);
                AddObjectIntoList(listRemoteBombs, "RemoteBomb_", pooledRemoteBomb, remoteBombContainer, index);
                AddObjectIntoList(listRedBombs, "RedBomb_", pooledRedBomb, redBombContainer, index);
                AddObjectIntoList(listRedRemoteBombs, "RedRemoteBomb_", pooledRedRemoteBomb, redRemoteBombContainer, index);
                AddObjectIntoList(listEnemyRedBombs, "EnemyRedBomb_", pooledEnemyRedBomb, enemyRedBombContainer, index);
                AddObjectIntoList(listEnemyBlackBombs, "EnemyBlackBomb_", pooledEnemyBlackBomb, enemyBlackBombContainer, index);
                AddObjectIntoList(listBrick, "Brick_", pooledBrick, brickContainer, index);
            }
            if (i <= 20)
            {
                AddObjectIntoList(listEnemyBullets, "Bullet_", pooledEnemyBullet, enemyBulletContainer, index);
                AddObjectIntoList(listDestroyObjectAnimation, "ExplosionDeath_", pooledDestroyObjectAnimation, destroyObjectAnimationContainer, index);
            }
            AddObjectIntoList(listExplosionMids, "MidExp_", pooledExplosionMid, explosionContainer, index);
            AddObjectIntoList(listExplosionEnds, "EndExp_", pooledExplosionEnd, explosionContainer, index);
        }
       
    }
    private GameObject GetGameObjectInList(List<GameObject> list,string obName ,GameObject pooledGameObject,Transform container)
    {
        if (list.Count > 0)
        {
            foreach (var t in list)
            {
                if (!t.activeInHierarchy)
                {
                    return t;
                }
            }
        }
        if (NotEnoughObjectPool)
        {
            var temp = Instantiate(pooledGameObject, container, true);
            temp.name = $"{obName} {((list.Count-1)).ToString()}";
            list.Add(temp);
            return temp;
        }
    }

   
    public GameObject GetBomb()
    {
      return GetGameObjectInList(listBombs, "Bomb_", pooledBomb, bombContainer);
    }
    public GameObject GetRemoteBomb()
    {
        return GetGameObjectInList(listRemoteBombs, "RemoteBomb_", pooledRemoteBomb, remoteBombContainer);
    }
    public GameObject GetRedBomb()
    {
        return GetGameObjectInList(listRedBombs, "RedBomb_", pooledRedBomb, redBombContainer);
    }
    public GameObject GetRedRemoteBomb()
    {
        return GetGameObjectInList(listRedRemoteBombs, "RedRemoteBomb_", pooledRedRemoteBomb, redRemoteBombContainer);
    }
    public GameObject GetEnemyRedBomb()
    {
        return GetGameObjectInList(listEnemyRedBombs, "EnemyRedBomb_", pooledEnemyRedBomb, enemyRedBombContainer);
    }
    public GameObject GetEnemyBlackBomb()
    {
        return GetGameObjectInList(listEnemyBlackBombs, "EnemyBlackBomb_", pooledEnemyBlackBomb, enemyBlackBombContainer);
    }
    public GameObject GetExplosionStart()
    {
        return GetGameObjectInList(listExplosionStarts, "StartExp_", pooledExplosionStart, explosionContainer);
    }
    public GameObject GetExplosionMid()
    {
        return GetGameObjectInList(listExplosionMids, "MidExp_", pooledExplosionMid, explosionContainer);
    }
    public GameObject GetExplosionEnd()
    {
        return GetGameObjectInList(listExplosionEnds, "EndExp_", pooledExplosionEnd, explosionContainer);
    }
    public GameObject GetBullet()
    {
        return GetGameObjectInList(listEnemyBullets, "Bullet_", pooledEnemyBullet, enemyBulletContainer);
    }
    public GameObject GetExplosionDeath()
    {
        return GetGameObjectInList(listDestroyObjectAnimation, "DestroyObjectAnimation_", pooledDestroyObjectAnimation, destroyObjectAnimationContainer);
    }
    public GameObject GetBrick()
    {
        return GetGameObjectInList(listBrick, "Brick_", pooledBrick, brickContainer);
    }

    //Reset all the bomb when quit to start menu
    public void ResetAllBomb()
    {
        ResetBombIn(listBombs);
        ResetBombIn(listRedBombs);
        ResetBombIn(listRemoteBombs);
        ResetBombIn(listRedRemoteBombs);
    }
    
    //Reset bomb in a list 
    private void ResetBombIn(List<GameObject> list)
    {
        foreach (var bomb in list)
        {
            bomb.SetActive(false);
        }
    }
}
