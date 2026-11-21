using UnityEngine;

/// <summary>
/// Simple projectile for ranged enemies.
/// Prefab setup: sprite + Rigidbody2D (Kinematic) + Collider2D with "Is Trigger" checked + this script.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private LayerMask obstacleMask;   // walls that destroy the projectile

    private Rigidbody2D rb;
    private Vector2 direction;
    private float speed;
    private int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    public void Launch(Vector2 dir, float projectileSpeed, int projectileDamage)
    {
        direction = dir;
        speed = projectileSpeed;
        damage = projectileDamage;

        // Point the sprite along its flight path (assumes the sprite faces right).
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifetime);   // cleanup if it never hits anything
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if ((obstacleMask.value & (1 << other.gameObject.layer)) != 0)
        {
            Destroy(gameObject);
        }
    }
}