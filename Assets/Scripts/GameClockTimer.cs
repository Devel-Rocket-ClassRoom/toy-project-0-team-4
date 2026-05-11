using UnityEngine;

public class GameClockTimer : MonoBehaviour
{
    [Header("전체 제한 시간")]
    [SerializeField]
    private float limitTime = 300f; // 5분 = 300초

    [Header("시계 회전축")]
    [SerializeField]
    private RectTransform clockHourPivot;

    [SerializeField]
    private RectTransform clockMinutePivot;

    [Header("미니게임 생성 매니저")]
    [SerializeField]
    private MiniGameSpawner miniGameSpawner;

    private float currentTime;
    private bool isRunning;
    private bool isFinished;

    private void Awake()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (!isRunning || isFinished)
        {
            return;
        }

        // Time.timeScale이 0이어도 전체 타이머 계산은 안전하게 하기 위해 unscaledDeltaTime 사용
        currentTime += Time.unscaledDeltaTime;

        float progress = currentTime / limitTime;

        if (progress >= 1f)
        {
            progress = 1f;
            isRunning = false;
            isFinished = true;

            UpdateClock(progress);

            if (miniGameSpawner != null)
            {
                // 여기서 일반 GameOver를 호출하지 않음
                // 점검시간 팝업을 최우선으로 띄우기 위해 MiniGameSpawner.ForceGameOver() 호출
                miniGameSpawner.ForceGameOver();
            }
            else
            {
                Debug.LogWarning("MiniGameSpawner가 GameClockTimer에 연결되지 않았습니다.");
            }

            return;
        }

        UpdateClock(progress);
    }

    private void UpdateClock(float progress)
    {
        // 시침: 제한 시간 동안 한 바퀴 회전
        float hourAngle = -360f * progress;

        // 분침: 시침이 한 바퀴 도는 동안 12바퀴 회전
        float minuteAngle = -360f * 12f * progress;

        if (clockHourPivot != null)
        {
            clockHourPivot.localRotation = Quaternion.Euler(0f, 0f, hourAngle);
        }

        if (clockMinutePivot != null)
        {
            clockMinutePivot.localRotation = Quaternion.Euler(0f, 0f, minuteAngle);
        }
    }

    public void StartTimer()
    {
        if (limitTime <= 0f)
        {
            Debug.LogWarning("Limit Time은 0보다 커야 합니다.");
            return;
        }

        currentTime = 0f;
        isRunning = true;
        isFinished = false;

        UpdateClock(0f);
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        if (isFinished)
        {
            return;
        }

        isRunning = true;
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        isRunning = false;
        isFinished = false;

        UpdateClock(0f);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float RemainingRatio => Mathf.Clamp01(1f - (currentTime / limitTime));
}
