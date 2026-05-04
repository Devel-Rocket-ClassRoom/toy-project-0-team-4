using System;
using UnityEngine;

public class MainScreenUI : MonoBehaviour
{
    [Serializable]
    public class StageButtonInfo
    {
        [Header("스테이지 번호")]
        public int stageNumber;

        [Header("해당 스테이지 시작 버튼")]
        public GameObject startButton;

        [Header("생성할 클리어 프리팹")]
        public GameObject clearPrefab;

        [HideInInspector]
        public GameObject spawnedClearObject;
    }

    [Header("스테이지 클리어 매니저")]
    [SerializeField] private StageClearManager stageClearManager;

    [Header("메인화면 스테이지 버튼 목록")]
    [SerializeField] private StageButtonInfo[] stageButtons;

    private void OnEnable()
    {
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

            if (isCleared)
            {
                ChangeToClearPrefab(stageButton);
            }
            else
            {
                ChangeToStartButton(stageButton);
            }
        }
    }

    private void ChangeToClearPrefab(StageButtonInfo stageButton)
    {
        if (stageButton.startButton != null)
        {
            stageButton.startButton.SetActive(false);
        }

        if (stageButton.spawnedClearObject != null)
            return;

        if (stageButton.clearPrefab == null)
        {
            Debug.LogWarning($"{stageButton.stageNumber} 스테이지 ClearPrefab이 연결되지 않았습니다.");
            return;
        }

        Transform parent = stageButton.startButton.transform.parent;

        stageButton.spawnedClearObject = Instantiate(
            stageButton.clearPrefab,
            parent
        );

        RectTransform startRect = stageButton.startButton.GetComponent<RectTransform>();
        RectTransform clearRect = stageButton.spawnedClearObject.GetComponent<RectTransform>();

        if (startRect != null && clearRect != null)
        {
            clearRect.anchorMin = startRect.anchorMin;
            clearRect.anchorMax = startRect.anchorMax;
            clearRect.pivot = startRect.pivot;
            clearRect.anchoredPosition = startRect.anchoredPosition;
            clearRect.sizeDelta = startRect.sizeDelta;
            clearRect.localScale = startRect.localScale;
            clearRect.localRotation = startRect.localRotation;
        }

        stageButton.spawnedClearObject.SetActive(true);
    }

    private void ChangeToStartButton(StageButtonInfo stageButton)
    {
        if (stageButton.startButton != null)
        {
            stageButton.startButton.SetActive(true);
        }

        if (stageButton.spawnedClearObject != null)
        {
            Destroy(stageButton.spawnedClearObject);
            stageButton.spawnedClearObject = null;
        }
    }
}