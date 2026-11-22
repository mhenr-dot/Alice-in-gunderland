using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 3f;
    public float stopSpawningAfter = 100f;

    private float timer;
    private float totalTime;

    void Update()
    {
        totalTime += Time.deltaTime;
        if (totalTime < stopSpawningAfter)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnEnemy();
                timer = 0f;
            }
        }
    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[randomIndex];

        Instantiate(enemyPrefab, selectedPoint.position, Quaternion.identity);
    }
}