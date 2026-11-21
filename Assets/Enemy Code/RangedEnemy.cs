using UnityEngine;

/// <summary>
/// Ranged enemy: walks until it has a clear shot, stops, backs away if you get
/// too close, and fires projectiles. Set Attack Range to how far it can shoot (e.g. 6).
/// </summary>
public class RangedEnemy : EnemyAI
{
    [Header("Ranged")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;            // optional; defaults to enemy center
    [SerializeField] private float projectileSpeed = 6f;
    [SerializeField] private float preferredDistance = 3f;   // backs away if the player is closer
    [SerializeField] private float retreatSpeed = 2f;

    protected override void UpdateChase(float distToPlayer)
    {
        if (distToPlayer > loseSightRadius)
        {
            CurrentState = State.Return;
            return;
        }

        // Only start shooting once there's nothing in the way.
        if (distToPlayer <= attackRange && HasLineOfSight())
        {
            CurrentState = State.Attack;
            return;
        }

        MoveToward(player.position, chaseSpeed);
    }

    protected override void UpdateAttack(float distToPlayer)
    {
        Face(player.position);

        if (distToPlayer > attackRange * 1.2f || !HasLineOfSight())
        {
            CurrentState = State.Chase;
            return;
        }

        if (distToPlayer < preferredDistance)
        {
            Vector2 away = (rb.position - (Vector2)player.position).normalized;
            rb.MovePosition(rb.position + away * retreatSpeed * Time.fixedDeltaTime);
        }

        TryAttack();   // respects attackCooldown, then calls PerformAttack()
    }

    protected override void PerformAttack()
    {
        Vector2 origin = firePoint != null ? (Vector2)firePoint.position : rb.position;
        Vector2 direction = ((Vector2)player.position - origin).normalized;

        Projectile shot = Instantiate(projectilePrefab, origin, Quaternion.identity);
        shot.Launch(direction, projectileSpeed, attackDamage);
    }
}