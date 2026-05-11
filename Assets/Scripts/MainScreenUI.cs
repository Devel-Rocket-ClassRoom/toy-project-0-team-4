using System;
using TMPro;
using UnityEngine;

public class MainScreenUI : MonoBehaviour
{
    [Serializable]
    public class StageButtonInfo
    {
        [Header("스테이지 번호")]
        public int stageNumber;

        [Header("메인화면에 표시되는 단계 Text")]
        public TMP_Text stageStepText;

        [Header("메인화면에 표시되는 제목 Text")]
        public TMP_Text stageTitleText;

        [Header("해당 스테이지 시작 버튼")]
        public GameObject startButton;

        [Header("생성할 클리어 프리팹")]
        public GameObject clearPrefab;

        [HideInInspector]
        public GameObject spawnedClearObject;
    }

    [Header("스테이지 클리어 매니저")]
    [SerializeField]
    private StageClearManager stageClearManager;

    [Header("메인화면 스테이지 버튼 목록")]
    [SerializeField]
    private StageButtonInfo[] stageButtons;

    [Header("클리어 프리팹 크기")]
    [SerializeField]
    private Vector2 clearPrefabSize = new Vector2(160f, 60f);

    [Header("클리어 프리팹 Z축 회전")]
    [SerializeField]
    private float clearPrefabRotationZ = -5f;

    [Header("단계와 제목 사이 구분자")]
    [SerializeField]
    private string titleSeparator = " ";

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
            bool isUnlocked = IsStageUnlocked(stageButton.stageNumber);

            if (isCleared)
            {
                ChangeToClearPrefab(stageButton);
            }
            else if (isUnlocked)
            {
                ChangeToStartButton(stageButton);
            }
            else
            {
                ChangeToLockedStage(stageButton);
            }
        }
    }

    public string GetStageTitle(int stageNumber)
    {
        foreach (StageButtonInfo stageButton in stageButtons)
        {
            if (stageButton.stageNumber != stageNumber)
                continue;

            string stepText = "";
            string titleText = "";

            if (stageButton.stageStepText != null)
            {
                stepText = stageButton.stageStepText.text.Trim();
            }

            if (stageButton.stageTitleText != null)
            {
                titleText = stageButton.stageTitleText.text.Trim();
            }

            if (string.IsNullOrEmpty(stepText))
            {
                stepText = $"{stageNumber}단계";
            }

            if (string.IsNullOrEmpty(titleText))
            {
                Debug.LogWarning($"{stageNumber} 스테이지 제목 Text가 비어있습니다.");
                return stepText;
            }

            string result = $"{stepText}{titleSeparator}{titleText}";

            return result;
        }

        Debug.LogWarning($"{stageNumber} 스테이지 정보를 찾지 못했습니다.");
        return $"{stageNumber}단계";
    }

    private bool IsStageUnlocked(int stageNumber)
    {
        if (stageNumber <= 1)
            return true;

        return stageClearManager.IsStageCleared(stageNumber - 1);
    }

    private void ChangeToClearPrefab(StageButtonInfo stageButton)
    {
        if (stageButton.startButton == null)
        {
            Debug.LogWarning(
                $"{stageButton.stageNumber} 스테이지 StartButton이 연결되지 않았습니다."
            );
            return;
        }

        stageButton.startButton.SetActive(false);

        if (stageButton.spawnedClearObject != null)
        {
            ApplyClearPrefabTransform(stageButton);
            stageButton.spawnedClearObject.SetActive(true);
            return;
        }

        if (stageButton.clearPrefab == null)
        {
            Debug.LogWarning(
                $"{stageButton.stageNumber} 스테이지 ClearPrefab이 연결되지 않았습니다."
            );
            return;
        }

        Transform parent = stageButton.startButton.transform.parent;

        stageButton.spawnedClearObject = Instantiate(stageButton.clearPrefab, parent, false);

        stageButton.spawnedClearObject.transform.SetSiblingIndex(
            stageButton.startButton.transform.GetSiblingIndex()
        );

        ApplyClearPrefabTransform(stageButton);

        stageButton.spawnedClearObject.SetActive(true);
    }

    private void ApplyClearPrefabTransform(StageButtonInfo stageButton)
    {
        if (stageButton.startButton == null || stageButton.spawnedClearObject == null)
            return;

        RectTransform startRect = stageButton.startButton.GetComponent<RectTransform>();
        RectTransform clearRect = stageButton.spawnedClearObject.GetComponent<RectTransform>();

        if (startRect == null || clearRect == null)
            return;

        clearRect.sizeDelta = clearPrefabSize;

        clearRect.localRotation = Quaternion.Euler(0f, 0f, clearPrefabRotationZ);
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

    private void ChangeToLockedStage(StageButtonInfo stageButton)
    {
        if (stageButton.startButton != null)
        {
            stageButton.startButton.SetActive(false);
        }

        if (stageButton.spawnedClearObject != null)
        {
            Destroy(stageButton.spawnedClearObject);
            stageButton.spawnedClearObject = null;
        }
    }
}
