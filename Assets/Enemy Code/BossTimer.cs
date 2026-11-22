using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossTimer : MonoBehaviour
{
    public float timer = 100f;
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public Text showTimer;

    private bool bossSpawned = false;
    void Update()
    {
        if (bossSpawned){
        return;
        }
        timer -= Time.deltaTime;
        showTimer.text = Mathf.CeilToInt(timer).ToString();
        if (timer <= 0f)
        {
            timer = 0f;
            showTimer.text = "0";
            SpawnBoss();
        }
    }

     void SpawnBoss()
    {
        bossSpawned = true;
        Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
    }
}