using UnityEngine;

/// <summary>
/// Charger enemy: chases normally, then when the player is inside Attack Range
/// it winds up (the telegraph), dashes in a straight line, and is vulnerable
/// while recovering. Set Attack Range to the distance it starts charging from (e.g. 5).
/// </summary>
public class ChargerEnemy : EnemyAI
{
    private enum Phase { Ready, Windup, Dash, Recover }

    [Header("Charge")]
    [SerializeField] private float windupTime = 0.8f;    // pause before the dash
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float recoverTime = 1.2f;   // the player's window to hit back
    [SerializeField] private float hitRadius = 0.7f;     // how close counts as "hit" during the dash

    private Phase phase = Phase.Ready;
    private float phaseTimer;
    private Vector2 dashDirection;
    private bool hasHit;

    protected override void UpdateChase(float distToPlayer)
    {
        phase = Phase.Ready;                 // always start a fresh charge when entering Attack
        base.UpdateChase(distToPlayer);
    }

    protected override void UpdateAttack(float distToPlayer)
    {
        switch (phase)
        {
            case Phase.Ready:
                phase = Phase.Windup;
                phaseTimer = windupTime;
                // TODO: telegraph here (flash the sprite red, play a snort sound)
                break;

            case Phase.Windup:
                Face(player.position);       // tracks the player until the dash starts
                phaseTimer -= Time.fixedDeltaTime;
                if (phaseTimer <= 0f)
                {
                    // Direction is locked in now, so the player can dodge sideways.
                    dashDirection = ((Vector2)player.position - rb.position).normalized;
                    phase = Phase.Dash;
                    phaseTimer = dashDuration;
                    hasHit = false;
                }
                break;

            case Phase.Dash:
                Facing = dashDirection;
                rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);

                if (!hasHit && distToPlayer <= hitRadius)
                {
                    DamagePlayer(attackDamage);
                    hasHit = true;           // one hit per charge
                }

                phaseTimer -= Time.fixedDeltaTime;
                if (phaseTimer <= 0f)
                {
                    phase = Phase.Recover;
                    phaseTimer = recoverTime;
                }
                break;

            case Phase.Recover:
                phaseTimer -= Time.fixedDeltaTime;
                if (phaseTimer <= 0f) CurrentState = State.Chase;
                break;
        }
    }
}