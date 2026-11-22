using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 3;
    
    public int enemiesKilled = 0;
    public Text showKillCount;
    public Text showHealth;
    
    private SpriteRenderer sprite;     // auto-found if left empty
    private float flashTime = 0.1f;

    public int health;
    private bool isDead;
    private Color originalColor;


    public void Start()
    {
        showHealth.text = health.ToString();
        showKillCount.text = enemiesKilled.ToString();
    }
    private void Awake()
    {
        health = maxHealth;
        if (sprite == null) sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null) originalColor = sprite.color;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0)
        {
            Die();
            return;
        }

        if (sprite != null)
        {
            StopAllCoroutines();
            StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        sprite.color = originalColor;
    }

    private void Die()
    {
        isDead = true;
        Destroy(gameObject);
        enemiesKilled++;
    }
}