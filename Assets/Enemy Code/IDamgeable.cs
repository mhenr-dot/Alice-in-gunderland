/// <summary>
/// Anything that can take damage (player, breakable props, other enemies).
/// Lets enemies hurt things without knowing what they are.
/// </summary>
public interface IDamageable
{
    void TakeDamage(int amount);
}