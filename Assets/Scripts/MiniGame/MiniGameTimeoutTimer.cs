using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameTimeoutTimer : MonoBehaviour
{
    [Header("미니게임 제한 시간")]
    [SerializeField]
    private float limitTime = 10f;

    private MiniGameSpawner miniGameSpawner;

    private TMP_Text tmpText;
    private Text uiText;

    private StageScreen stageScreen;

    private float currentTime;
    private bool isRunning;
    private bool isTimedOut;

    private void Awake()
    {
        // TextMeshPro 텍스트 자동 탐색
        tmpText = GetComponent<TMP_Text>();

        // 일반 UI Text 자동 탐색
        uiText = GetComponent<Text>();

        // 현재 미니게임의 StageScreen 자동 탐색
        stageScreen = GetComponentInParent<StageScreen>(true);

        // MiniGameSpawner가 비어 있으면 씬에서 자동 탐색
        if (miniGameSpawner == null)
        {
            miniGameSpawner = FindFirstObjectByType<MiniGameSpawner>();
        }
    }

    private void OnEnable()
    {
        ResetTimer();

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
        if (!isRunning)
            return;

        if (isTimedOut)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            UpdateTimerText();

            Timeout();
            return;
        }

        UpdateTimerText();
    }

    private void ResetTimer()
    {
        currentTime = limitTime;
        isRunning = true;
        isTimedOut = false;

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        SetText(seconds.ToString());
    }

    private void SetText(string text)
    {
        if (tmpText != null)
        {
            tmpText.text = text;
        }

        if (uiText != null)
        {
            uiText.text = text;
        }
    }

    private void Timeout()
    {
        if (isTimedOut)
            return;

        isTimedOut = true;
        isRunning = false;

        if (miniGameSpawner == null)
        {
            miniGameSpawner = FindFirstObjectByType<MiniGameSpawner>();
        }

        if (miniGameSpawner != null)
        {
            miniGameSpawner.ShowTimeoutPopup();
        }
        else
        {
            Debug.LogWarning("MiniGameSpawner를 찾지 못했습니다.");
        }
    }

    private void OnStageClear(int stageNumber)
    {
        // 미니게임이 클리어되면 개별 타이머 정지
        isRunning = false;
    }

    private void OnStageGameOver()
    {
        // 미니게임이 실패되면 개별 타이머 정지
        isRunning = false;
    }
}
