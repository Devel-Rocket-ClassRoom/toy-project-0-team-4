using TMPro;
using UnityEngine;

public class BeadController : MonoBehaviour
{
    [Header("이동 및 회전 설정")]
    public float rollSpeed = 2.5f;
    public float pushForce = 15f;

    [Header("데이터")]
    public TextMeshProUGUI textDisplay;
    public char letter;

    private Rigidbody2D rb;
    private bool isRolling = false;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    public void SetLetter(char newLetter)
    {
        letter = newLetter;
        if (textDisplay != null)
            textDisplay.text = newLetter.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isRolling = true;
        }
    }

    void FixedUpdate()
    {
        if (isRolling)
        {
            if (rb.linearVelocity.x < rollSpeed)
            {
                rb.AddForce(Vector2.right * pushForce);
            }
        }
    }
}
