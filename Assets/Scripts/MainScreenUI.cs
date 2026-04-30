using System;
using System.Collections;
using UnityEngine;

public class MainScreenUI : MonoBehaviour
{
    [Serializable]
    public class StageButtonInfo
    {
        [Header("스테이지 번호")]
        public int stageNumber;

        [Header("확인하기 버튼")]
        public GameObject checkButton;

        [Header("클리어 아이콘")]
        public GameObject clearIcon;
    }

    [Header("스테이지 클리어 매니저")]
    [SerializeField] private StageClearManager stageClearManager;

    [Header("메인화면 스테이지 버튼 목록")]
    [SerializeField] private StageButtonInfo[] stageButtons;


    private void OnEnable()
    {
        StartCoroutine(RefreshNextFrame());
    }

    private IEnumerator RefreshNextFrame()
    {
        yield return null;

        RefreshStageButtons();
    }

    public void RefreshStageButtons()
    {
        if (stageClearManager == null)
        {
            Debug.LogWarning("StageClearManager가 연결되지 않았습니다.");
            return;
        }

        foreach (StageButtonInfo stageButton in stageButtons)
        {
            bool isCleared = stageClearManager.IsStageCleared(stageButton.stageNumber);

            if (stageButton.checkButton != null)
            {
                stageButton.checkButton.SetActive(!isCleared);
            }

            if (stageButton.clearIcon != null)
            {
                stageButton.clearIcon.SetActive(isCleared);
            }

            Debug.Log($"{stageButton.stageNumber}단계 클리어 상태: {isCleared}");
        }
    }
}