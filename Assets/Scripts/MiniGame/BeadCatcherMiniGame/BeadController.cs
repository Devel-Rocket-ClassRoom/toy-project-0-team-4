using UnityEngine;
using TMPro;

public class BeadController : MonoBehaviour
{
    [Header("이동 및 회전 설정")]
    public float rollSpeed = 2.5f;
    public float pushForce = 15f;   // 밀어주는 힘의 세기
    public Transform visualModel; // 회전할 자식 오브젝트 (이미지+텍스트)

    [Header("데이터")]
    public TextMeshProUGUI textDisplay;
    public char letter;

    private Rigidbody2D rb;
    private bool isRolling = false;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    public void SetLetter(char newLetter)
    {
        letter = newLetter;
        if (textDisplay != null) textDisplay.text = newLetter.ToString();
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

    void Update()
    {
        // 물리 속도에 맞춰 시각적 모델 회전 (굴러가는 연출)
        if (visualModel != null)
        {
            visualModel.Rotate(0, 0, -rollSpeed * 150f * Time.deltaTime);
        }
    }
}