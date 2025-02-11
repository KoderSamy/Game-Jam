using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBullet : MonoBehaviour
{
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Уничтожаем пулю через lifetime секунд
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); // Уничтожаем пулю при столкновении
    }
}
