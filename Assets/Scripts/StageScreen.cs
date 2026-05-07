using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageScreen : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button clearButton;

    [Header("미니게임 내부 제목 텍스트")]
    [SerializeField] private TextMeshProUGUI titleText;

    public void SetTitle(string title)
    {
        if (titleText != null)
            titleText.text = title;
    }

    public event Action<int> OnStageClearButtonClicked;
    public event Action OnGameOver;

    private int stageNumber;
    private bool isFinished;

    private void Awake()
    {
        if (clearButton != null)
            clearButton.onClick.AddListener(OnClickClearButton);
    }

    private void OnEnable()
    {
        MiniGameManager.OnMiniGameSuccess += ClearStage;
        MiniGameManager.OnMiniGameFail    += GameOver;
    }

    private void OnDisable()
    {
        MiniGameManager.OnMiniGameSuccess -= ClearStage;
        MiniGameManager.OnMiniGameFail    -= GameOver;
    }

    private void OnDestroy()
    {
        if (clearButton != null)
            clearButton.onClick.RemoveListener(OnClickClearButton);
    }

    public void Init(int stageNumber)
    {
        this.stageNumber = stageNumber;
        isFinished = false;
    }

    private void OnClickClearButton()
    {
        ClearStage();
    }

    public void ClearStage()
    {
        if (isFinished)
            return;

        isFinished = true;

        Debug.Log($"{stageNumber} 스테이지 클리어");

        OnStageClearButtonClicked?.Invoke(stageNumber);
    }

    public void GameOver()
    {
        if (isFinished)
            return;

        isFinished = true;

        Debug.Log($"{stageNumber} 스테이지 게임오버");

        OnGameOver?.Invoke();
    }
}