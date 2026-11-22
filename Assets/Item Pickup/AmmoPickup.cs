using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
[SerializeField] private int ammoAmount = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("1. Triggered by: " + other.name);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("2. FAILED: tag is '" + other.tag + "', not Player");
            return;
        }

        if (other.TryGetComponent(out IAmmoReceiver receiver))
        {
            Debug.Log("3. Found IAmmoReceiver, giving ammo");
            receiver.AddAmmo(ammoAmount);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("4. FAILED: no IAmmoReceiver on " + other.name);
        }
    }

}
