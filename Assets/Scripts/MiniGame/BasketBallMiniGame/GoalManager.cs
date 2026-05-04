using UnityEngine;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour
{
    [Header("Goal UI Settings")]
    public Image[] goalIndicators; 
    public Color emptyColor = new Color(0, 0.5f, 0, 0.5f); 
    public Color filledColor = Color.green;              

    private int currentGoalCount = 0;

    void Start()
    {
        // 시작할 때 모든 슬롯을 비어있는 색으로 초기화
        foreach (var img in goalIndicators)
        {
            img.color = emptyColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BasketBall"))
        {
            Rigidbody2D ballRb = other.GetComponent<Rigidbody2D>();

            if (ballRb != null)
            {
                other.tag = "Untagged"; // 중복 충돌 방지
                GoalScored();
            }
        }
    }

    private void GoalScored()
    {
        if (currentGoalCount < goalIndicators.Length)
        {
            goalIndicators[goalIndicators.Length - 1 - currentGoalCount].color = filledColor;
            currentGoalCount++;

            if (currentGoalCount >= goalIndicators.Length)
            {
                OnComplete();
            }
        }
    }

    private void OnComplete()
    {
        // 동의 팝업 띄우는 코드
    }
}