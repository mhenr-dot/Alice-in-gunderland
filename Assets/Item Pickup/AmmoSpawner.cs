using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
     public GameObject ammoPrefab;
     public Vector2 areaMin = new Vector2(-5f, -5f);   
     public Vector2 areaMax = new Vector2(5f, 5f);    
     public float spawnInterval = 5f;

    private float timer;

    private void Start()
    { 
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn();
            timer = spawnInterval;
        }
    }

    private void Spawn()
    {
        Vector2 position = new Vector2(
            Random.Range(areaMin.x, areaMax.x),
            Random.Range(areaMin.y, areaMax.y));

        Instantiate(ammoPrefab, position, Quaternion.identity);
    }
}