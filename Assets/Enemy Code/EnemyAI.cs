using UnityEngine;

/// <summary>
/// Base enemy for a top-down 2D game. Used as-is, it IS the melee enemy.
/// Other enemy types inherit from this and override the virtual methods
/// (UpdateChase, UpdateAttack, PerformAttack).
///
/// States: Patrol -> Chase -> Attack, then Return to its post if the player escapes.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Return }

    [Header("Target")]
    [SerializeField] protected Transform player;        // auto-found by "Player" tag if empty

    [Header("Movement")]
    [SerializeField] protected float patrolSpeed = 1.5f;
    [SerializeField] protected float chaseSpeed = 3f;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waypointReachedDistance = 0.1f;
    [SerializeField] private float waitAtWaypoint = 1f;

    [Header("Detection")]
    [SerializeField] protected float detectionRadius = 5f;
    [SerializeField] protected float loseSightRadius = 8f;
    [SerializeField] protected LayerMask obstacleMask;

    [Header("Attack")]
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected int attackDamage = 1;

    public State CurrentState { get; protected set; } = State.Patrol;
    public Vector2 Facing { get; protected set; } = Vector2.down;   // read this for animations

    protected Rigidbody2D rb;
    protected IDamageable playerDamageable;

    private Vector2 startPosition;
    private int waypointIndex;
    private float waitTimer;
    private float attackTimer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        startPosition = transform.position;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        if (player != null) player.TryGetComponent(out playerDamageable);
    }

    private void FixedUpdate()
    {
        attackTimer -= Time.fixedDeltaTime;

        // Player died / was destroyed: stand down.
        if (player == null)
        {
            if (CurrentState == State.Chase || CurrentState == State.Attack)
                CurrentState = State.Return;
        }

        float distToPlayer = player != null
            ? Vector2.Distance(rb.position, player.position)
            : Mathf.Infinity;

        switch (CurrentState)
        {
            case State.Patrol:
                DoPatrol();
                if (CanSeePlayer(distToPlayer)) CurrentState = State.Chase;
                break;

            case State.Chase:
                UpdateChase(distToPlayer);      // overridable
                break;

            case State.Attack:
                UpdateAttack(distToPlayer);     // overridable
                break;

            case State.Return:
                MoveToward(startPosition, patrolSpeed);
                if (CanSeePlayer(distToPlayer)) CurrentState = State.Chase;
                else if (Vector2.Distance(rb.position, startPosition) <= waypointReachedDistance)
                    CurrentState = State.Patrol;
                break;
        }
    }

    // ---------- Overridable behavior (melee defaults) ----------

    /// <summary>Runs every physics frame while chasing.</summary>
    protected virtual void UpdateChase(float distToPlayer)
    {
        MoveToward(player.position, chaseSpeed);
        if (distToPlayer <= attackRange) CurrentState = State.Attack;
        else if (distToPlayer > loseSightRadius) CurrentState = State.Return;
    }

    /// <summary>Runs every physics frame while in the Attack state.</summary>
    protected virtual void UpdateAttack(float distToPlayer)
    {
        Face(player.position);
        // 1.2x gives a little buffer so it doesn't flicker between Chase and Attack
        if (distToPlayer > attackRange * 1.2f) CurrentState = State.Chase;
        // Only actually hit when the player is inside the real attack range
        else if (distToPlayer <= attackRange) TryAttack();
    }

    /// <summary>What a single attack does. Melee: damage the player directly.</summary>
    protected virtual void PerformAttack()
    {
        DamagePlayer(attackDamage);
    }

    // ---------- Helpers subclasses can use ----------

    /// <summary>Respects attackCooldown, then calls PerformAttack().</summary>
    protected void TryAttack()
    {
        if (attackTimer > 0f) return;
        attackTimer = attackCooldown;
        PerformAttack();
        // TODO: trigger attack animation / sound here
    }

    protected void DamagePlayer(int amount)
    {
        if (playerDamageable != null) playerDamageable.TakeDamage(amount);
    }

    protected bool HasLineOfSight()
    {
        // Linecast returns a hit if a wall sits between the enemy and the player.
        return !Physics2D.Linecast(rb.position, player.position, obstacleMask);
    }

    protected bool CanSeePlayer(float distToPlayer)
    {
        if (distToPlayer > detectionRadius) return false;
        return HasLineOfSight();
    }

    protected void MoveToward(Vector2 target, float speed)
    {
        Face(target);
        Vector2 next = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);
    }

    protected void Face(Vector2 target)
    {
        Vector2 dir = target - rb.position;
        if (dir.sqrMagnitude > 0.0001f) Facing = dir.normalized;
    }

    // ---------- Patrol ----------

    private void DoPatrol()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            return;
        }

        Vector2 target = waypoints[waypointIndex].position;
        MoveToward(target, patrolSpeed);

        if (Vector2.Distance(rb.position, target) <= waypointReachedDistance)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            waitTimer = waitAtWaypoint;
        }
    }

    // ---------- Editor visuals (select the enemy in Scene view) ----------

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, loseSightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (waypoints == null) return;
        Gizmos.color = Color.cyan;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Transform next = waypoints[(i + 1) % waypoints.Length];
            if (next != null) Gizmos.DrawLine(waypoints[i].position, next.position);
        }
    }
}