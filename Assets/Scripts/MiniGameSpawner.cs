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

    private StageScreen currentMiniGame;
    private int currentStageNumber;

    public void StartStage(int stageNumber)
    {
        if (miniGamePrefabs == null || miniGamePrefabs.Length == 0)
        {
            Debug.LogWarning("미니게임 Prefab이 등록되어 있지 않습니다.");
            return;
        }

        currentStageNumber = stageNumber;

        if (titleScreen != null)
        {
            titleScreen.SetActive(false);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }

        DestroyCurrentMiniGame();

        int randomIndex = Random.Range(0, miniGamePrefabs.Length);
        StageScreen prefab = miniGamePrefabs[randomIndex];

        currentMiniGame = Instantiate(prefab, miniGameParent);
        currentMiniGame.Init(currentStageNumber);

        currentMiniGame.OnStageClearButtonClicked += HandleStageClear;
        currentMiniGame.OnGameOver += HandleGameOver;

        Debug.Log($"{currentStageNumber} 스테이지 시작");
    }

    private void HandleStageClear(int stageNumber)
    {
        if (stageClearManager != null)
        {
            stageClearManager.ClearStage(stageNumber);
        }

        DestroyCurrentMiniGame();

        ShowMainScreen();
    }

    private void HandleGameOver()
    {
        Debug.Log("게임오버 - 타이틀 화면으로 이동");

        if (stageClearManager != null)
        {
            stageClearManager.ResetAll();
        }

        currentStageNumber = 0;

        DestroyCurrentMiniGame();

        ShowTitleScreen();
    }

    private void ShowMainScreen()
    {
        if (titleScreen != null)
        {
            titleScreen.SetActive(false);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(true);
        }

        if (mainScreenUI != null)
        {
            mainScreenUI.RefreshStageButtons();
        }
    }

    private void ShowTitleScreen()
    {
        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }

        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
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