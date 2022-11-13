using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    [Header("Data need for populate a level")]
    [Tooltip("Map Scriptable object stores data about a level")]
    [SerializeField] private MapSOScript mapSo;
    [Tooltip("current level player is playing")]
    [SerializeField] private int level;

    [Tooltip("list contains all map for every level")]
    [SerializeField] private List<GameObject> gridLevelsList;
    // when determine the level, need to know what grid level is in grid level list
    private GameObject gridLevel;

    [Tooltip("The gate is place player exit and continue go to next level")] 
    [SerializeField] private GameObject gatePrefab;
    
    //Reference to soft block tilemap of grid
    private Tilemap tilemapSoftBlock;

    //Reference to hard block tilemap of grid
    private Tilemap tilemapHardBlock;

    //Reference to ground tilemap of grid
  //  private Tilemap tilemapGround;

    //width and height of all the map
    private const int TotalX = 13;
    private const int TotalY = 11;
    

    [Tooltip("Determine the chance can spawn a soft block on free cells")] 
    [SerializeField] private float chanceSpawnSoftBlockEachCell; 
    [Tooltip("Determine the chance can spawn a hard block on free cells")] 
    [SerializeField] private float chanceSpawnHardBlockEachCell; 

    //All the hard blocks have been spawn will be store in this list
  //  private List<Vector2> hardBlockSpawn;

    // list stores the position that is not taken
    private List<Vector2> freeCells;


    //list stores the position that is soft block
    private List<Vector2> softBlockPosList;

    //list stores the position that can spawn enemy
    private List<Vector2> positionForEnemySpawnList;
    
    [Tooltip("Store all enemies in current level")] 
    [SerializeField] private List<GameObject> enemyList;
    [Tooltip("Store all power ups in current level")] 
    [SerializeField] private List<GameObject> powerUpList;
    
    [Tooltip("Store soft block tile in current level")] 
    [SerializeField] private TileBase softBlockTile;
    [Tooltip("Store hard block tile in current level")]
    [SerializeField] private TileBase hardBlockTile;
    
    
    [Tooltip("when enemies spawned, it is parent transform of them in current level")]
    [SerializeField] private GameObject enemyContainer;

    [SerializeField] private GameObject playerPrefab;
    
    [Header("For checking valid map or not")]
    [Tooltip("Node contain data for checking valid map")]
    [SerializeField] private GameObject nodePrefab;
    //grid Node for contain all node in the grid level
    private GameObject[,]  gridNode;
    [Tooltip("All node is spawned will be set parents transform of its")]
    [SerializeField] private Transform nodeContainer;
    //Contains all node that connected with player
    private HashSet<GameObject> connectedNodeList;
    //position of player when start playing
     private int startX,startY;
    private void Awake()
    {
        instance = this;
        level = PlayerPrefs.GetInt("Level");
    }
    private void  GetGridLevel()
    {
        var instantGrid = Instantiate(gridLevelsList[level/2]);
        gridLevel = instantGrid;
        gridLevel.SetActive(true);
    }

   
    // Start is called before the first frame update
    private void Start()
    {
        if(level>=12) Initiate.Fade("Victory", Color.white, 1.0f);
        GetGridLevel();
        if (mapSo.levelList[level].isBoss) return;
        InitialSetup();
        PopulateMap();
        PopulateNode();
        CheckMapIsValid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            StartCoroutine(GameManager.instance.MoveToNextLevel());
        }
    }

    public void PlaceObjectOnMap()
    {
        if (mapSo.levelList[level].isBoss)
        {
            PlacePlayer();
            SpawnBoss();
            return;
        }
        PlacePlayer();
        PlaceGate(); 
        PlaceEnemy();
        PlacePowerUp();
    }
    
    private void PlacePlayer()
    {
        Instantiate(playerPrefab, Vector2.zero, quaternion.identity);
    }
    

   private void InitialSetup()
    {
        startX = 0;
        startY = 0;
        connectedNodeList = new HashSet<GameObject>();
        tilemapSoftBlock = gridLevel.transform.Find("SoftBlocks").GetComponent<Tilemap>();
        tilemapHardBlock = gridLevel.transform.Find("HardBlocks").GetComponent<Tilemap>();
        softBlockPosList = new List<Vector2>();
        freeCells = new List<Vector2>();
        enemyList = new List<GameObject>();
        positionForEnemySpawnList = new List<Vector2>();
        powerUpList = mapSo.levelList[level].powerUpList;
        softBlockTile = mapSo.levelList[level].softBlock;
        hardBlockTile = mapSo.levelList[level].hardBlock;
        gridNode = new GameObject[TotalX, TotalY];
    }

   private void SpawnBoss()
   {
       var boss = Instantiate(mapSo.levelList[level].enemyList[0], new Vector2(6,9),
           quaternion.identity);
       boss.transform.SetParent(enemyContainer.transform);
   }
     private void PopulateMap()
    {
        
        for(int x = 0; x < TotalX; x++) 
        {
            for(int y = 0; y < TotalY; y++)
            {
                if(x<2 && y<2) continue;
                Vector3Int cell;
                if (x % 2 == 0)
                {
                    if (CanSpawnSoftBlock(chanceSpawnSoftBlockEachCell))
                    {
                        cell = tilemapSoftBlock.WorldToCell(new Vector2(x,y));
                        tilemapSoftBlock.SetTile(cell, softBlockTile);
                        softBlockPosList.Add(new Vector2(x, y));
                        freeCells.Add(new Vector2(x, y));
                    }
                    else if(CanSpawnHardBlock(chanceSpawnHardBlockEachCell))
                    {                      
                            cell = tilemapHardBlock.WorldToCell(new Vector2(x, y));
                            tilemapHardBlock.SetTile(cell, hardBlockTile);
                    }
                    else
                    {
                        freeCells.Add(new Vector2(x, y));
                        positionForEnemySpawnList.Add(new Vector2(x, y));
                    }
                }
                else
                {
                    if (y % 2 == 0)
                    {
                       
                        if (CanSpawnSoftBlock(chanceSpawnSoftBlockEachCell))
                        {
                            cell = tilemapSoftBlock.WorldToCell(new Vector2(x, y));
                            tilemapSoftBlock.SetTile(cell, softBlockTile);
                            softBlockPosList.Add(new Vector2(x, y));
                            freeCells.Add(new Vector2(x, y));
                        }
                        else if (CanSpawnHardBlock(chanceSpawnHardBlockEachCell))
                        {       
                                cell = tilemapHardBlock.WorldToCell(new Vector2(x, y));
                                tilemapHardBlock.SetTile(cell, hardBlockTile);
                        }
                        else
                        {
                            freeCells.Add(new Vector2(x, y));
                            positionForEnemySpawnList.Add(new Vector2(x, y));
                        }
                    }
                }
            } 
        }
        AddFreeCellsInSafeZone();
    }
    private void PlacePowerUp()
    {
        foreach (var powerUp in powerUpList)
        {
            DecidePutToEnemyOrBlock(powerUp);
        }
    }

    private void DecidePutToEnemyOrBlock(GameObject powerUp)
    {
        var random = Random.Range(1, 3);
        if (random == 1)
        {
            PutPowerUpToEnemy(powerUp);
        }
        else if(random == 2)
        {
            PutPowerUpToSoftBlock(powerUp);
        }
    }

    private void PutPowerUpToSoftBlock(GameObject powerUp)
    {
        var index = Random.Range(0, softBlockPosList.Count);
        Instantiate(powerUp,softBlockPosList[index],quaternion.identity);
    }

    private void PutPowerUpToEnemy(GameObject powerUp)
    {
        var index = Random.Range(0, enemyList.Count);
        enemyList[index].GetComponent<EnemyHealth>().SetPowerUp(powerUp);
        //after add power up to enemy then remove enemy from list to ensure that not duplicate power up in enemy
        enemyList.RemoveAt(index);
    }
    

    private void PlaceEnemy()
    {
        var totalEnemy = mapSo.levelList[level].enemyList.Count;
        for(var i = 0; i < totalEnemy; i++)
        {
           var randomIndex = Random.Range(0, positionForEnemySpawnList.Count);
           var enemy=  Instantiate(mapSo.levelList[level].enemyList[i], positionForEnemySpawnList[randomIndex], Quaternion.identity);
           enemy.transform.SetParent(enemyContainer.transform);
           enemy.name = $"enemy_{i.ToString()}";
           enemy.GetComponent<EnemyHealth>().ReferenceEnemyContainer(enemyContainer);
           enemyList.Add(enemy);
           positionForEnemySpawnList.RemoveAt(randomIndex);
        }
        enemyContainer.GetComponent<EnemyContainerScript>().GetTotalEnemy(totalEnemy);
    }
    
    public GameObject GetRandomEnemy()
    {
        var index = Random.Range(0, mapSo.levelList[level].enemyList.Count);
        return mapSo.levelList[level].enemyList[index];
    }
    
   
    private void PlaceGate()
    {
        var pos = Random.Range(0, softBlockPosList.Count);
        while(softBlockPosList[pos].x<=2|| softBlockPosList[pos].y <= 2)
        {
             pos = Random.Range(0, softBlockPosList.Count);
        } 
        Instantiate(gatePrefab, softBlockPosList[pos], Quaternion.identity);
        softBlockPosList.RemoveAt(pos);
    }
    
    private bool CanSpawnSoftBlock(float chanceSpawn)
    {
        return (Random.Range(0f, 1f) <= chanceSpawn);
    }
    private bool CanSpawnHardBlock(float chanceSpawn)
    {
        return (Random.Range(0f, 1f) <= chanceSpawn);
    }
   
    
    private void PopulateNode()
    {
       // AddFreeCellsInSafeZone();
        foreach (var pos in freeCells)
        {
            GameObject node = Instantiate(nodePrefab, pos, quaternion.identity);
           
            var position = node.transform.position;
            int x =(int) position.x;
            int y =(int) position.y;
            gridNode[x, y] = node;
            var nodeScript  = node.GetComponent<Node>();
            nodeScript.visited = -1;
            nodeScript.x = x;
            nodeScript.y = y;
            node.transform.SetParent(nodeContainer);
        }
        gridNode[startX, startY].GetComponent<Node>().visited = 0;
       //start node always is connected so add it to the connected node list
       connectedNodeList.Add(gridNode[0, 0]);
    }
    //this function add 3 cells that in safe zone always connected in free cell for calculate map is valid or not
    private void AddFreeCellsInSafeZone()
    {
        freeCells.Add(new Vector2(0,0));
        freeCells.Add(new Vector2(1,0));
        freeCells.Add(new Vector2(0,1));
    }
    //after determine map is valid we remove cells in safe zone to place enemies, power ups and gate
    private void RemoveFreeCellsInSafeZone()
    {
        freeCells.RemoveRange(freeCells.Count-4,3);
    }
    
    private void CheckMapIsValid()
    {
        for (var step = 1; step < freeCells.Count; step++)
        {
            foreach (var node in gridNode)
            {
                if (node == null) continue;
                Node nodeScript = node.GetComponent<Node>();
                if (nodeScript.visited == step - 1)
                {
                    Test4Direction(nodeScript.x, nodeScript.y, step);
                }
            }
        }
        //for visualize connected node
        //ChangeColorPath(Color.red);
        
        //if connected node 
        if (connectedNodeList.Count == freeCells.Count)
        {
            nodeContainer.gameObject.SetActive(false);
            RemoveFreeCellsInSafeZone();
        }
        else
        {
            SceneManager.LoadScene(1);
        }
        
        
    }
  
    private void Test4Direction(int x, int y, int step)
    {
        if(TestDirection(x,y,-1,1)) SetVisited(x,y+1,step);
        if(TestDirection(x,y,-1,2)) SetVisited(x+1,y,step);
        if(TestDirection(x,y,-1,3)) SetVisited(x,y-1,step);
        if(TestDirection(x,y,-1,4)) SetVisited(x-1,y,step);
    }
    private bool TestDirection(int x, int y, int step, int direction)
    {
        //1 is up, 2 is right, 3 is down, 4 is left
        switch (direction)
        {
            case 1:
                if (y + 1 < TotalY && gridNode[x, y + 1] && gridNode[x, y + 1].GetComponent<Node>().visited == step)
                {
                    return true;
                }
                return false;
            case 2:
                if (x + 1 < TotalX && gridNode[x + 1, y] && gridNode[x + 1, y].GetComponent<Node>().visited == step)
                {
                    return true;
                }
                return false;
            case 3:
                if (y - 1 >-1  && gridNode[x, y-1] && gridNode[x, y-1].GetComponent<Node>().visited == step)
                {
                    return true;
                }
                return false;
            case 4:
                if (x - 1 > -1 && gridNode[x - 1, y] && gridNode[x - 1, y].GetComponent<Node>().visited == step)
                {
                    return true;
                }
                return false;     
        }

        return false;
    }

  
    private void SetVisited(int x,int y,int step)
    {
        if (gridNode[x, y])
        {
            gridNode[x, y].GetComponent<Node>().visited = step;
            connectedNodeList.Add(gridNode[x, y]);
        }
    }
    private void ChangeColorPath(Color newColor)
    {
        foreach (var node in connectedNodeList)
        {
            node.GetComponent<SpriteRenderer>().color = newColor;
        }
    }
    

    public GameObject GetEnemyContainer()
    {
        return enemyContainer;
    }


}
