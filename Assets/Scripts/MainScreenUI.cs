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

    private Vector2 clearPrefabSize = new Vector2(160f, 60f);

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

            // 이미 클리어한 스테이지
            if (isCleared)
            {
                ChangeToClearPrefab(stageButton);
            }
            // 아직 클리어하지 않았지만 열려있는 스테이지
            else if (isUnlocked)
            {
                ChangeToStartButton(stageButton);
            }
            // 아직 열리지 않은 스테이지
            else
            {
                ChangeToLockedStage(stageButton);
            }
        }
    }

    private bool IsStageUnlocked(int stageNumber)
    {
        // 1단계는 항상 시작 가능
        if (stageNumber <= 1)
        {
            return true;
        }

        // 2단계 이상은 이전 단계가 클리어되어야 열림
        return stageClearManager.IsStageCleared(stageNumber - 1);
    }

    private void ChangeToClearPrefab(StageButtonInfo stageButton)
    {
        if (stageButton.startButton == null)
        {
            Debug.LogWarning($"{stageButton.stageNumber} 스테이지 StartButton이 연결되지 않았습니다.");
            return;
        }

        // 시작 버튼 숨김
        stageButton.startButton.SetActive(false);

        // 이미 클리어 프리팹이 생성되어 있으면 위치/크기만 다시 보정
        if (stageButton.spawnedClearObject != null)
        {
            ApplyClearPrefabTransform(stageButton);
            stageButton.spawnedClearObject.SetActive(true);
            return;
        }

        if (stageButton.clearPrefab == null)
        {
            Debug.LogWarning($"{stageButton.stageNumber} 스테이지 ClearPrefab이 연결되지 않았습니다.");
            return;
        }

        Transform parent = stageButton.startButton.transform.parent;

        // 클리어 프리팹 생성
        stageButton.spawnedClearObject = Instantiate(stageButton.clearPrefab, parent, false);

        // 시작 버튼과 같은 순서에 배치
        stageButton.spawnedClearObject.transform.SetSiblingIndex(stageButton.startButton.transform.GetSiblingIndex());

        // 위치, 크기, Z값 적용
        ApplyClearPrefabTransform(stageButton);

        stageButton.spawnedClearObject.SetActive(true);
    }

    private void ApplyClearPrefabTransform(StageButtonInfo stageButton)
    {
        if (stageButton.startButton == null || stageButton.spawnedClearObject == null)
            return;

        RectTransform startRect = stageButton.startButton.GetComponent<RectTransform>();
        RectTransform clearRect = stageButton.spawnedClearObject.GetComponent<RectTransform>();

        // 클리어 프리팹 크기 고정
        clearRect.sizeDelta = clearPrefabSize;

        // Z 위치 -5 적용
        clearRect.localRotation = Quaternion.Euler(0f, 0f, -5f);

    }

    private void ChangeToStartButton(StageButtonInfo stageButton)
    {
        // 시작 버튼 표시
        if (stageButton.startButton != null)
        {
            stageButton.startButton.SetActive(true);
        }

        // 클리어 프리팹이 있으면 제거
        if (stageButton.spawnedClearObject != null)
        {
            Destroy(stageButton.spawnedClearObject);
            stageButton.spawnedClearObject = null;
        }
    }

    private void ChangeToLockedStage(StageButtonInfo stageButton)
    {
        // 아직 열리지 않은 스테이지는 시작 버튼 숨김
        if (stageButton.startButton != null)
        {
            stageButton.startButton.SetActive(false);
        }

        // 클리어 프리팹도 숨김/삭제
        if (stageButton.spawnedClearObject != null)
        {
            Destroy(stageButton.spawnedClearObject);
            stageButton.spawnedClearObject = null;
        }
    }
}