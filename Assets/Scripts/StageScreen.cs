using System;
using UnityEngine;
using UnityEngine.UI;

public class StageScreen : MonoBehaviour
{
    [Header("스테이지 정보")]
    [SerializeField] private int stageNumber;

    [Header("버튼")]
    [SerializeField] private Button clearButton;

    public event Action<int> OnStageClearButtonClicked;

    private void Awake()
    {
        clearButton.onClick.AddListener(OnClickClearButton);
    }

    private void OnDestroy()
    {
        clearButton.onClick.RemoveListener(OnClickClearButton);
    }

    private void OnClickClearButton()
    {
        Debug.Log($"클리어 버튼 눌림");

        OnStageClearButtonClicked?.Invoke(stageNumber);
    }
}