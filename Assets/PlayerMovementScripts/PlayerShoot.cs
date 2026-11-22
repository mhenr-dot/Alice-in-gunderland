using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour, IAmmoReceiver
{
    public float bulletSpeed = 15f;

    public float bulletAmount = 5;
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    private PlayerControl playerDirection;
    public float shotDelay = .5f;
    private bool ishooting = false;

    public Text showAmmo;

    void Start()
    {
        playerDirection = GetComponent<PlayerControl>();
    }
    void Update()
    {
        InputShot();
        showAmmo.text = bulletAmount.ToString();
    }

    public void AddAmmo(int amount)
    {
    bulletAmount += amount;
    }
    IEnumerator shotWait()
    {
        ishooting = true;
        bulletAmount--;
        GameObject clone = Instantiate(bulletPrefab, spawnPoint.position,Quaternion.identity);
        Destroy(clone, 5f);
        if (clone.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = playerDirection.facingDirection * bulletSpeed;
        }
        yield return new WaitForSeconds(shotDelay);
        ishooting = false;
        
    }
    void InputShot()
    {
         if(Keyboard.current == null){
            return;
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame && bulletAmount > 0 && !ishooting)
        {
            StartCoroutine(shotWait());
        }
    }


}
