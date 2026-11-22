using System.Collections;
using UnityEngine;

/// <summary>
/// Health for any enemy. Add it next to EnemyAI / RangedEnemy / ChargerEnemy.
/// Anything that implements IDamageable (like the player's attack) can hurt it.
/// </summary>
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;

    [Header("Hit Feedback")]
    [SerializeField] private SpriteRenderer sprite;     // auto-found if left empty
    [SerializeField] private float flashTime = 0.1f;

    private int health;
    private bool isDead;
    private Color originalColor;

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
    }
}