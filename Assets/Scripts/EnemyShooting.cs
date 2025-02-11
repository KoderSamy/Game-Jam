using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;  //  Префаб пули
    public float bulletSpeed = 10f;
    public float fireRate = 1f;
    public Transform firePoint; //  Точка выстрела


    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = -transform.forward * bulletSpeed; //  Стреляем назад
        rb.useGravity = false; //  Отключаем гравитацию для пули, если нужно

    }
}