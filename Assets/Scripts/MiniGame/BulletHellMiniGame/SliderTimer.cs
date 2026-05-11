using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{
    [Header("미니게임 제한 시간")]
    [SerializeField]
    private float limitTime = 25f;

    [Header("진행 상황 슬라이더")]
    [SerializeField]
    private Slider timerSlider; // 슬라이더 컴포넌트 연결용

    [SerializeField]
    private LoadingText loadingText; // 인스펙터에서 연결

    private MiniGameSpawner miniGameSpawner;
    private TMP_Text tmpText;
    private Text uiText;
    private StageScreen stageScreen;

    private float currentTime;
    private bool isRunning;
    private bool isTimedOut;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        uiText = GetComponent<Text>();
        stageScreen = GetComponentInParent<StageScreen>(true);

        if (miniGameSpawner == null)
        {
            miniGameSpawner = FindFirstObjectByType<MiniGameSpawner>();
        }
    }

    private void OnEnable()
    {
        if (stageScreen != null)
        {
            stageScreen.OnStageClearButtonClicked += OnStageClear;
            stageScreen.OnGameOver += OnStageGameOver;
        }
    }

    private void OnDisable()
    {
        if (stageScreen != null)
        {
            stageScreen.OnStageClearButtonClicked -= OnStageClear;
            stageScreen.OnGameOver -= OnStageGameOver;
        }
    }

    private void Update()
    {
        if (!isRunning || isTimedOut)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            UpdateTimerUI(); // 텍스트와 슬라이더 모두 업데이트
            Timeout();
            return;
        }

        UpdateTimerUI();
    }

    public void StartTimer()
    {
        ResetTimer();
        if (loadingText != null)
            loadingText.StartPulsing();
    }

    private void ResetTimer()
    {
        currentTime = limitTime;
        isRunning = true;
        isTimedOut = false;

        // 슬라이더 초기화
        if (timerSlider != null)
        {
            timerSlider.maxValue = limitTime;
            timerSlider.value = 0f;
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        // 텍스트 업데이트
        int seconds = Mathf.CeilToInt(currentTime);
        SetText(seconds.ToString());

        // 슬라이더 업데이트
        if (timerSlider != null)
        {
            // 시간이 지날수록 슬라이더가 점점 차오르는 방식 (진행률)
            timerSlider.value = limitTime - currentTime;
        }
    }

    private void SetText(string text)
    {
        if (tmpText != null)
            tmpText.text = text;
        if (uiText != null)
            uiText.text = text;
    }

    private void Timeout()
    {
        if (isTimedOut)
            return;

        isTimedOut = true;
        isRunning = false;

        DragController playerCtrl = FindFirstObjectByType<DragController>();
        if (playerCtrl != null)
        {
            playerCtrl.StopFollowing();
        }

        if (loadingText != null)
            loadingText.StopPulsing();
        stageScreen.ClearStage();
    }

    public void StopTimer()
    {
        isRunning = false; // 시간을 멈춤
        if (loadingText != null)
            loadingText.StopPulsing();
    }

    private void OnStageClear(int stageNumber) => isRunning = false;

    private void OnStageGameOver() => isRunning = false;
}
