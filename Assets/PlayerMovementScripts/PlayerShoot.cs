using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public float bulletSpeed = .4f;

    public GameObject bulletPrefab;
    public Transform spwanPoint;


    // Update is called once per frame
    void Update()
    {
        InputShot();
    }
    void InputShot()
    {
         if(Keyboard.current == null){
            return;
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GameObject clone = Instantiate(bulletPrefab, spwanPoint.position,Quaternion.identity);
            Destroy(clone, 5f);
            clone.GetComponent<Rigidbody2D>().linearVelocity = (spwanPoint.right * bulletSpeed);
        }
    }

}
