using UnityEngine;

public class GunAK : MonoBehaviour
{
    [SerializeField] private float BulletSpeedFor = 50f;
    [SerializeField] private GameObject BulletPrefab;
    private float ShootStopTimeFor = 0.1f;
    private float NextShootTimeFor = 0f;

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= NextShootTimeFor)
        {
            NextShootTimeFor = Time.time + ShootStopTimeFor;
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject Bullet = Instantiate(BulletPrefab, gameObject.transform.position, gameObject.transform.rotation);

        Rigidbody RigidbodyBullet = Bullet.GetComponent<Rigidbody>();
        if (RigidbodyBullet != null)
        {
            RigidbodyBullet.linearVelocity = gameObject.transform.forward * BulletSpeedFor;
        }

        Destroy(Bullet, 2f);
    }
}
