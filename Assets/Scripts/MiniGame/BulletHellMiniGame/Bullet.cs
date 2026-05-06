using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2f; 
    public float lifeTime = 5f; 

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}