using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float pulseSpeed = 1.2f; // 깜빡이는 속도
    [SerializeField] private float minAlpha = 0.1f;    // 최소 투명도
    [SerializeField] private float maxAlpha = 1.0f;    // 최대 투명도

    private TextMeshProUGUI loadingText;
    private bool isPulsing = false;

    void Awake()
    {
        loadingText = GetComponent<TextMeshProUGUI>();
        // 시작 시에는 텍스트를 숨겨둡니다.
        SetAlpha(0);
    }

    void Update()
    {
        if (!isPulsing) return;

        // 시간에 따라 0~1 사이를 왕복하는 값 계산
        float pingPong = Mathf.PingPong(Time.time * pulseSpeed, 1.0f);
        // 설정한 최소/최대 알파 값 범위로 변환
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, pingPong);

        SetAlpha(alpha);
    }

    // 깜빡임 시작
    public void StartPulsing()
    {
        isPulsing = true;
        gameObject.SetActive(true);
    }

    // 깜빡임 중지 및 숨김
    public void StopPulsing()
    {
        isPulsing = false;
        SetAlpha(0);
        gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        if (loadingText != null)
        {
            Color c = loadingText.color;
            c.a = alpha;
            loadingText.color = c;
        }
    }
}