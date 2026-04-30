using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGameStarted = false;
    [SerializeField] private float constantSpeed = 5f;
    [SerializeField] private float minHorizontalVelocity = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
    }

    void FixedUpdate()
    {
        if (isGameStarted)
        {
            Vector2 currentVel = rb.linearVelocity;

            // 1. X축 속도가 너무 작으면(수직으로만 움직이면) 강제로 밀어줌
            if (Mathf.Abs(currentVel.x) < minHorizontalVelocity)
            {
                float nudge = (currentVel.x >= 0) ? minHorizontalVelocity : -minHorizontalVelocity;
                currentVel.x = nudge;
            }

            rb.linearVelocity = currentVel.normalized * constantSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGameStarted && collision.gameObject.CompareTag("Paddle"))
        {
            isGameStarted = true;
            rb.gravityScale = 0f; 

            rb.linearVelocity = new Vector2(Random.Range(-2f, 2f), constantSpeed);
        }

        if (collision.gameObject.CompareTag("Brick"))
        {
            Destroy(collision.gameObject);
        }
    }
}