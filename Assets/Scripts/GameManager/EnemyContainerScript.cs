using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainerScript : MonoBehaviour
{
    [SerializeField] private int totalEnemy;
    public void GetTotalEnemy(int number)
    {
        totalEnemy = number;
    }

    public void DecreaseEnemyInScene()
    {
        totalEnemy--;
        if (totalEnemy == 0)
        {
            Gate.instance.ActiveTheGate();
        }
    }

    public void IncreaseEnemyInScene()
    {
        totalEnemy++;
    }
  
}
