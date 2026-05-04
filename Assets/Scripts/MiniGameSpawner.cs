using System.Collections.Generic;
using UnityEngine;

public class MiniGameSpawner : MonoBehaviour
{
    [Header("화면")]
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject mainScreen;

    [Header("미니게임이 생성될 부모")]
    [SerializeField] private Transform miniGameParent;

    [Header("미니게임 프리팹 목록")]
    [SerializeField] private StageScreen[] miniGamePrefabs;

    [Header("클리어 매니저")]
    [SerializeField] private StageClearManager stageClearManager;

    [Header("메인화면 UI")]
    [SerializeField] private MainScreenUI mainScreenUI;

    [Header("결과 팝업")]
    [SerializeField] private GameObject successResultObject;
    [SerializeField] private GameObject failResultObject;

    [Header("오디오 매니저")]
    [SerializeField] private AudioManager audioManager;

    private StageScreen currentMiniGame;
    private int currentStageNumber;

    private readonly List<int> availableMiniGameIndexes = new List<int>();

    private void Awake()
    {
        ResetMiniGamePool();
    }

    public void StartStage(int stageNumber)
    {
        if (miniGamePrefabs == null || miniGamePrefabs.Length == 0)
        {
            Debug.LogWarning("미니게임 Prefab이 등록되어 있지 않습니다.");
            return;
        }

        Time.timeScale = 1f;

        currentStageNumber = stageNumber;

        HideResultObjects();

        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }

        DestroyCurrentMiniGame();

        StageScreen prefab = GetRandomMiniGameWithoutDuplicate();

        if (prefab == null)
        {
            Debug.LogWarning("생성할 미니게임 Prefab이 없습니다.");
            return;
        }

        currentMiniGame = Instantiate(prefab, miniGameParent);

        Debug.Log($"[Spawner] 생성됨: {currentMiniGame.name}");

        currentMiniGame.Init(currentStageNumber);

        currentMiniGame.OnStageClearButtonClicked += HandleStageClear;
        currentMiniGame.OnGameOver += HandleGameOver;

        StartCreatedMiniGame();

        Debug.Log($"{currentStageNumber} 스테이지 시작 / 미니게임: {prefab.name}");
    }

    private StageScreen GetRandomMiniGameWithoutDuplicate()
    {
        if (availableMiniGameIndexes.Count == 0)
        {
            Debug.Log("모든 미니게임이 한 번씩 나왔습니다. 목록을 다시 초기화합니다.");
            ResetMiniGamePool();
        }

        if (availableMiniGameIndexes.Count == 0)
            return null;

        int randomListIndex = Random.Range(0, availableMiniGameIndexes.Count);
        int prefabIndex = availableMiniGameIndexes[randomListIndex];

        availableMiniGameIndexes.RemoveAt(randomListIndex);

        return miniGamePrefabs[prefabIndex];
    }

    private void ResetMiniGamePool()
    {
        availableMiniGameIndexes.Clear();

        if (miniGamePrefabs == null)
            return;

        for (int i = 0; i < miniGamePrefabs.Length; i++)
        {
            if (miniGamePrefabs[i] != null)
            {
                availableMiniGameIndexes.Add(i);
            }
        }

        Debug.Log($"미니게임 풀 초기화 완료 / 개수: {availableMiniGameIndexes.Count}");
    }

    private void StartCreatedMiniGame()
    {
        if (currentMiniGame == null)
            return;

        if (currentMiniGame.TryGetComponent<ButtonChange>(out var buttonChange))
        {
            buttonChange.StartMiniGame();
            return;
        }

        if (currentMiniGame.TryGetComponent<MovingMiniGame>(out var movingMiniGame))
        {
            movingMiniGame.StartMiniGame();
            return;
        }

        if (currentMiniGame.TryGetComponent<HiddenAgreeGame>(out var hiddenAgreeGame))
        {
            hiddenAgreeGame.StartMiniGame();
            return;
        }

        if (currentMiniGame.TryGetComponent<WallButtonGame>(out var wallButtonGame))
        {
            wallButtonGame.StartMiniGame();
            return;
        }

        // 만약 미니게임 스크립트가 Prefab 루트가 아니라 자식에 붙어있을 경우 대비
        ButtonChange childButtonChange = currentMiniGame.GetComponentInChildren<ButtonChange>(true);
        if (childButtonChange != null)
        {
            childButtonChange.StartMiniGame();
            return;
        }

        MovingMiniGame childMovingMiniGame = currentMiniGame.GetComponentInChildren<MovingMiniGame>(true);
        if (childMovingMiniGame != null)
        {
            childMovingMiniGame.StartMiniGame();
            return;
        }

        HiddenAgreeGame childHiddenAgreeGame = currentMiniGame.GetComponentInChildren<HiddenAgreeGame>(true);
        if (childHiddenAgreeGame != null)
        {
            childHiddenAgreeGame.StartMiniGame();
            return;
        }

        WallButtonGame childWallButtonGame = currentMiniGame.GetComponentInChildren<WallButtonGame>(true);
        if (childWallButtonGame != null)
        {
            childWallButtonGame.StartMiniGame();
            return;
        }
    }

    private void HandleStageClear(int stageNumber)
    {
        Debug.Log($"{stageNumber} 스테이지 클리어");

        if (stageClearManager != null)
        {
            stageClearManager.ClearStage(stageNumber);
        }

        ShowSuccessPopup();
    }

    private void HandleGameOver()
    {
        Debug.Log("게임오버");

        ShowFailPopup();
    }

    private void ShowSuccessPopup()
    {
        Time.timeScale = 0f;

        if (successResultObject != null)
        {
            successResultObject.SetActive(true);
        }

        if (failResultObject != null)
        {
            failResultObject.SetActive(false);
        }
    }

    private void ShowFailPopup()
    {
        Time.timeScale = 0f;

        if (successResultObject != null)
        {
            successResultObject.SetActive(false);
        }

        if (failResultObject != null)
        {
            failResultObject.SetActive(true);
        }
    }

    public void ConfirmSuccess()
    {
        if (audioManager != null)
        {
            audioManager.PlaySfx(0);
        }

        Time.timeScale = 1f;

        DestroyCurrentMiniGame();

        HideResultObjects();

        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(true);
        }

        if (mainScreenUI != null)
        {
            mainScreenUI.RefreshStageButtons();
        }

        Debug.Log("성공 확인 버튼 클릭 - 메인화면으로 이동");
    }

    public void ConfirmFail()
    {
        if (audioManager != null)
        {
            audioManager.PlaySfx(0);
        }

        Time.timeScale = 1f;

        if (stageClearManager != null)
        {
            stageClearManager.ResetAll();
        }

        currentStageNumber = 0;

        DestroyCurrentMiniGame();

        HideResultObjects();

        ResetMiniGamePool();

        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }

        if (mainScreenUI != null)
        {
            mainScreenUI.RefreshStageButtons();
        }

        Debug.Log("실패 확인 버튼 클릭 - 타이틀화면으로 이동");
    }

    public void ShowTitleScreen()
    {
        Time.timeScale = 1f;

        if (stageClearManager != null)
        {
            stageClearManager.ResetAll();
        }

        currentStageNumber = 0;

        DestroyCurrentMiniGame();

        HideResultObjects();

        ResetMiniGamePool();

        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }

        if (mainScreenUI != null)
        {
            mainScreenUI.RefreshStageButtons();
        }
    }

    public void HideResultObjects()
    {
        if (successResultObject != null)
        {
            successResultObject.SetActive(false);
        }

        if (failResultObject != null)
        {
            failResultObject.SetActive(false);
        }
    }

    public void DestroyCurrentMiniGame()
    {
        if (currentMiniGame == null)
            return;

        currentMiniGame.OnStageClearButtonClicked -= HandleStageClear;
        currentMiniGame.OnGameOver -= HandleGameOver;

        Destroy(currentMiniGame.gameObject);
        currentMiniGame = null;
    }
}