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

    private StageScreen currentMiniGame;
    private int currentStageNumber;

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

        // 게임 중에는 배경은 보이고, 메인 팝업은 숨김
        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

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

        if (currentMiniGame.TryGetComponent<ButtonChange>(out var buttonChange))
            buttonChange.StartMiniGame();
        else if (currentMiniGame.TryGetComponent<MovingMiniGame>(out var miniGames))
            miniGames.StartMiniGame();
        else if (currentMiniGame.TryGetComponent<HiddenAgreeGame>(out var hiddenGame))
            hiddenGame.StartMiniGame();

        Debug.Log($"{currentStageNumber} 스테이지 시작 / 랜덤 미니게임: {prefab.name}");
    }

    private void HandleStageClear(int stageNumber)
    {
        Debug.Log($"{stageNumber} 스테이지 클리어");

        if (stageClearManager != null)
            stageClearManager.ClearStage(stageNumber);

        ShowSuccessPopup();
    }

    public void ShowMainScreen()
    {
        if (mainScreen != null) mainScreen.SetActive(true);
        if (mainScreenUI != null) mainScreenUI.RefreshStageButtons();
    }

    private void HandleGameOver()
    {
        Debug.Log("게임오버");

        // 바로 타이틀로 가지 않음
        // 현재 미니게임 화면 위에 실패 팝업만 표시
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
        Time.timeScale = 1f;

        if (stageClearManager != null)
        {
            stageClearManager.ResetAll();
        }

        currentStageNumber = 0;

        DestroyCurrentMiniGame();

        HideResultObjects();

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