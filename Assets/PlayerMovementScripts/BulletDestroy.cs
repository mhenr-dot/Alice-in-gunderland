using UnityEngine;

public class BulletDestroy : MonoBehaviour
{    void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable enemy = collision.gameObject.GetComponentInParent<IDamageable>();
        if (enemy != null){
            enemy.TakeDamage(1);
        }
        Destroy(gameObject);
    }

}
