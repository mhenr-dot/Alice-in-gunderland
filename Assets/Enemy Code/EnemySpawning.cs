using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;
    public Transform player; 
    public float spawnRadius = 12f; 

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (player != null)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                Vector2 spawnPosition = (Vector2)player.position + (randomDirection * spawnRadius);
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}