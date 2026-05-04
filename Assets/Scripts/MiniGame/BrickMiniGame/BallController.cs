using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;
    private StageScreen stageScreen;

    private bool isGameStarted = false;

    [SerializeField] private float constantSpeed = 5f;
    [SerializeField] private float minVelocity = 0.5f;
    [SerializeField] private float paddleInfluence = 0.05f;

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

            // 상하 무한 루프 방지
            if (Mathf.Abs(currentVel.x) < minVelocity)
            {
                float pushDirection = (transform.localPosition.x < 0) ? 1f : -1f;
                currentVel.x = pushDirection * minVelocity;
            }

            // 좌우 무한 루프 방지
            if (Mathf.Abs(currentVel.y) < minVelocity)
            {
                float pushDirection = (currentVel.y >= 0) ? 1f : -1f;
                currentVel.y = pushDirection * minVelocity;
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