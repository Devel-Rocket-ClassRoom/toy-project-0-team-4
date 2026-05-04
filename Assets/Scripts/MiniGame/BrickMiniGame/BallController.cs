using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private StageScreen stageScreen;

    private bool isGameStarted = false;

    [SerializeField] private float constantSpeed = 5f;
    [SerializeField] private float minHorizontalVelocity = 0.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;

        stageScreen = GetComponentInParent<StageScreen>();
    }

    private void FixedUpdate()
    {
        if (isGameStarted)
        {
            Vector2 currentVel = rb.linearVelocity;

            if (Mathf.Abs(currentVel.x) < minHorizontalVelocity)
            {
                float pushDirection = (transform.localPosition.x < 0) ? 1f : -1f;
                currentVel.x = pushDirection * minHorizontalVelocity;
            }

            rb.linearVelocity = currentVel.normalized * constantSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger 감지: {other.name}");

        if (other.CompareTag("DeadZone"))
        {
            Debug.Log("게임 오버");

            if (stageScreen == null)
            {
                stageScreen = GetComponentInParent<StageScreen>();
            }

            if (stageScreen != null)
            {
                stageScreen.GameOver();
            }
            else
            {
                Debug.LogWarning("BallController가 StageScreen을 찾지 못했습니다.");
            }
        }
    }
}