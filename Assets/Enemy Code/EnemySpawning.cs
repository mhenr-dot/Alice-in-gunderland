using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;
    
    [Header("Spawn Settings")]
    public Transform player; // player location
    public float spawnRadius = 12f; // spawn distance

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
                // random direction
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                
                // find distance
                Vector2 spawnPosition = (Vector2)player.position + (randomDirection * spawnRadius);

                // spawn enemy
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}