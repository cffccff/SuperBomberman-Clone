using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GenerateNote : MonoBehaviour
{
    [SerializeField] private int testNumber;
    [SerializeField] private List<GameObject> tempList;
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private bool findDistance = false;
    [SerializeField] private int totalX;
    [SerializeField] private int totalY;
    [SerializeField] private int startX,startY;
    [SerializeField] private int endX, endY;
    private GameObject[,]  gridNode;

    [SerializeField] private List<GameObject> path;
    [SerializeField] private HashSet<GameObject> nodeList;
    // Start is called before the first frame update
    void Start()
    {
        totalX = 13;
        totalY = 11;
      gridNode = new GameObject[totalX, totalY];
      tempList = new List<GameObject>();
      path =  new List<GameObject>();
      nodeList = new HashSet<GameObject>();
        PopulateNote();
    }

    private void PopulateNote()
    {
        for (int y = 0; y < totalY; y++)
        {
            if(y%2!=0) continue;
            for (int x = 0; x < totalX-1; x++)
            {
                
                    GameObject node = Instantiate(notePrefab, new Vector3(x, y), quaternion.identity);
                    gridNode[x,y] = node;
                    node.GetComponent<Node>().x = x;
                    node.GetComponent<Node>().y = y;
                
            }
        }

        for (int y = 0; y < totalY; y++)
        {
            int x = totalX - 1;
            GameObject node = Instantiate(notePrefab, new Vector3(x, y), quaternion.identity);
            gridNode[x,y] = node;
            node.GetComponent<Node>().x = x;
            node.GetComponent<Node>().y = y;
        }

        int testy1 = 3;
        int testy2 = 7;
        int testx = 0;
        GameObject node1 = Instantiate(notePrefab, new Vector3(testx, testy1), quaternion.identity);
        gridNode[testx,testy1] = node1;
        node1.GetComponent<Node>().x = testx;
        node1.GetComponent<Node>().y = testy1;
        
        GameObject node2 = Instantiate(notePrefab, new Vector3(testx, testy2), quaternion.identity);
        gridNode[testx,testy2] = node2;
        node2.GetComponent<Node>().x = testx;
        node2.GetComponent<Node>().y = testy2;

        gridNode[0, 2] = null;
        gridNode[0, 4] = null;
        gridNode[0, 6] = null;
        gridNode[0, 8] = null;
    }

    private void DeleteNote()
    {
        gridNode[0, 8] = null;
        gridNode[2, 7] = null;
        gridNode[2, 9] = null;
        gridNode[3, 8] = null;
        gridNode[10, 0] = null;
        gridNode[12, 0] = null;
        
      
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
            SceneManager.LoadScene(2);
        }

        if (findDistance)
        {
            SetDistance();
            //not use this function because it calculate the shortest part to the start's position
           // SetPath();
            findDistance = false;
            ChangeColorPath(Color.red);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            testNumber++;
          findDistance = true;

        }
        
    }

    private void ChangeColorPath(Color newColor)
    {
        foreach (var node in nodeList)
        {
            node.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    private void InitialSetup()
    {
        foreach (var node in gridNode)
        {
            if(node !=null) node.GetComponent<Node>().visited = -1;
        }

        gridNode[startX, startY].GetComponent<Node>().visited = 0;
        //DeleteRandom();
    }

    private void SetDistance()
    {
        InitialSetup();
        for (var step = 1; step <= testNumber; step++)
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
    }
  
    private void SetPath()
    {
        
        int step;
        var x = endX;
        var y = endY;
       
        path.Clear(); 
        if (gridNode[endX, endY] && gridNode[endX, endY].GetComponent<Node>().visited > 0)
        {
            path.Add(gridNode[x,y]);
            step = gridNode[x, y].GetComponent<Node>().visited - 1;
        }
        else
        {
            print("No way to reach the destination");
            return;
        }
       
        for (var i = step; step > -1; step--)
        {
            
            if (TestDirection(x, y, step, 1))
            {
                tempList.Add(gridNode[x,y+1]);

            }

            if (TestDirection(x, y, step, 2))
            {
                tempList.Add(gridNode[x+1,y]);
              
            }

            if (TestDirection(x, y, step, 3))
            {
                tempList.Add(gridNode[x,y-1]);
          
            }

            if (TestDirection(x, y, step, 4))
            {
                tempList.Add(gridNode[x-1,y]);
              
            }
            GameObject temp = FindClosest(gridNode[endX, endY].transform, tempList);
            path.Add(temp);
            x = temp.GetComponent<Node>().x;
            y = temp.GetComponent<Node>().y;
              tempList.Clear();
        }
    }

    private GameObject FindClosest(Transform targetLocation, List<GameObject> list)
    {
        float currentDistance = (totalX) * (totalY);
        var index = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(targetLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(targetLocation.position, list[i].transform.position);
                index = i;
            }
        }
        return list[index];
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
                if (y + 1 < totalY && gridNode[x, y + 1] && gridNode[x, y + 1].GetComponent<Node>().visited == step)
                {
                    return true;
                }
                return false;
            case 2:
                if (x + 1 < totalX && gridNode[x + 1, y] && gridNode[x + 1, y].GetComponent<Node>().visited == step)
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
            nodeList.Add(gridNode[x, y]);
        }
    }
}

