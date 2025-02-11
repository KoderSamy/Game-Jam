using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float minLifetime = 2f;
    public float maxLifetime = 4f;

    private void Start()
    {
        float lifetime = Random.Range(minLifetime, maxLifetime);
        Destroy(gameObject, lifetime);
    }


    private void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.CompareTag("Shield") || !collision.gameObject.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}