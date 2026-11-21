using UnityEngine;

/// <summary>
/// Minimal player health so the enemy has something to hit.
/// Put on the Player object (tag it "Player").
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 5;
    private int health;

    private void Awake() => health = maxHealth;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"Player hit! Health: {health}/{maxHealth}");

        if (health <= 0)
        {
            // TODO: death animation / game over screen
            Destroy(gameObject);
        }
    }
}