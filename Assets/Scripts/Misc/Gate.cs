using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using DG.Tweening;
public class Gate : MonoBehaviour
{
    public static Gate instance; 
    private bool readySpawn;
    private bool isGateActive;
    private GameObject enemyContainer;
    private EnemyContainerScript enemyContainerScript;
    private void Awake()
    {
        instance = this;
        readySpawn = true;
        isGateActive = false;
        enemyContainer = MapManager.instance.GetEnemyContainer();
        enemyContainerScript = enemyContainer.GetComponent<EnemyContainerScript>();
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGateActive) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        collision.gameObject.transform.DOMove(this.gameObject.transform.position, 0f);
        StartCoroutine(GameManager.instance.MoveToNextLevel());
    }
    
    public void SpawnEnemy()
    {
        if (!readySpawn) return;
        var enemy= Instantiate(MapManager.instance.GetRandomEnemy(), transform.position, quaternion.identity);
        enemy.transform.SetParent(enemyContainer.transform);
        enemy.GetComponent<EnemyHealth>().ReferenceEnemyContainer(enemyContainer);
        enemyContainerScript.IncreaseEnemyInScene();
        var enemyHealth = enemy.GetComponent<EnemyHealth>();
        StartCoroutine(enemyHealth.EnemyInvincible());
        readySpawn = false;
        StartCoroutine(TimerSpawnEnemy());
    }
    private IEnumerator TimerSpawnEnemy()
    {
        yield return new WaitForSeconds(1f);
        readySpawn = true;
    }
    public void ActiveTheGate()
    {
        isGateActive = true;
    }

    public void DisableTheGate()
    {
        isGateActive = false;
    }
}
