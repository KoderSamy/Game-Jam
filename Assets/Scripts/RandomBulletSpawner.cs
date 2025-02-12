using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBulletSpawner : MonoBehaviour
{
    public GameObject[] bulletPrefabs; // Массив префабов шаров
    public float minSpawnRate = 0.2f;  // Минимальная частота
    public float maxSpawnRate = 0.7f;   // Максимальная частота
    public float bulletSpeed = 10f;
    public Transform spawnPoint;    // Точка выстрела

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnBullet();
            // Вычисляем случайную частоту появления
            float randomSpawnRate = Random.Range(minSpawnRate, maxSpawnRate);
            nextSpawnTime = Time.time + 1f / randomSpawnRate;
        }
    }

    void SpawnBullet()
    {
        // Выбираем случайный префаб из массива
        int randomIndex = Random.Range(0, bulletPrefabs.Length);
        GameObject bulletPrefab = bulletPrefabs[randomIndex];

        // Создаем пулю
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Задаем направление и скорость пули
        rb.velocity = -transform.forward * bulletSpeed;

        //Отключаем гравитацию чтобы летело прямо
        rb.useGravity = false;
    }
}