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

            // 수직 움직임 감지 (X축 속도가 매우 낮을 때)
            if (Mathf.Abs(currentVel.x) < minHorizontalVelocity)
            {
                float pushDirection = (transform.localPosition.x < 0) ? 1f : -1f;

                // 강제로 X축 속도를 부여하여 벽에서 떼어냄
                currentVel.x = pushDirection * minHorizontalVelocity;
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