using UnityEngine;

public class MiniGameSpawner : MonoBehaviour
{
    [Header("화면")]
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

        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }

        DestroyCurrentMiniGame();

        int randomIndex = Random.Range(0, miniGamePrefabs.Length);
        StageScreen prefab = miniGamePrefabs[randomIndex];

        currentMiniGame = Instantiate(prefab, miniGameParent);
        Debug.Log($"[Spawner] 생성됨: {currentMiniGame.name}, 부모: {currentMiniGame.transform.parent?.name}, activeInHierarchy: {currentMiniGame.gameObject.activeInHierarchy}");

        currentMiniGame.Init(currentStageNumber);
        Debug.Log($"[Spawner] Init 후 존재여부: {currentMiniGame != null}");

        currentMiniGame.OnStageClearButtonClicked += HandleStageClear;
        currentMiniGame.OnGameOver += HandleGameOver;

        var buttonChange = currentMiniGame.GetComponent<ButtonChange>();
        int gameIdx = buttonChange != null ? buttonChange.gameIndex : randomIndex + 1;
        if (MiniGameManager.Instance != null)
            MiniGameManager.Instance.StartGame(gameIdx);
        Debug.Log($"[Spawner] StartGame 후 존재여부: {currentMiniGame != null}");

        Debug.Log($"{currentStageNumber} 스테이지 시작 / 랜덤 미니게임: {prefab.name} (gameIndex: {randomIndex + 1})");
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
        Debug.Log("게임오버 - 미니게임 삭제");

        DestroyCurrentMiniGame();
        ShowMainScreen();
    }

    public void ShowMainScreen()
    {
        if (mainScreen != null)
        {
            mainScreen.SetActive(true);
        }

        if (mainScreenUI != null)
        {
            mainScreenUI.RefreshStageButtons();
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