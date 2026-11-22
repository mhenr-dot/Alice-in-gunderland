using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private int maxHealth = 5;
    public int health;
    public Text showHealth;
    void Update()
    {
        showHealth.text = health.ToString();
    }
    private void Awake() => health = maxHealth;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log($"Player hit! Health: {health}/{maxHealth}");

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}