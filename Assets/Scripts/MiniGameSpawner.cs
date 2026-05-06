using UnityEngine;

public class MiniGameSpawner : MonoBehaviour
{
    [Header("화면")]
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject mainScreen;

    [Header("미니게임이 생성될 부모")]
    [SerializeField] private Transform miniGameParent;

    [Header("미니게임 랜덤 풀")]
    [SerializeField] private MiniGamePool miniGamePool = new MiniGamePool();

    [Header("팝업 컨트롤러")]
    [SerializeField] private MiniGamePopup popupController = new MiniGamePopup();

    [Header("클리어 매니저")]
    [SerializeField] private StageClearManager stageClearManager;

    [Header("메인화면 UI")]
    [SerializeField] private MainScreenUI mainScreenUI;

    [Header("전체 시계 타이머")]
    [SerializeField] private GameClockTimer gameClockTimer;

    [Header("버튼 핸들러")]
    [SerializeField] private OnClickButton onClickButton;

    private StageScreen currentMiniGame;
    private int currentStageNumber;

    private void Awake()
    {
        miniGamePool.ResetPool();
        popupController.HideAll();
    }

    public void StartStage(int stageNumber)
    {
        if (!miniGamePool.HasPrefab())
        {
            Debug.LogWarning("미니게임 Prefab이 등록되어 있지 않습니다.");
            return;
        }

        // 새 미니게임 시작 시 시간 정상화
        Time.timeScale = 1f;

        currentStageNumber = stageNumber;

        // 결과 팝업 상태 초기화
        popupController.ResetState();
        popupController.HideAll();

        // 게임 중에는 타이틀 배경은 보이고, 메인 팝업은 숨김
        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }

        // 기존 미니게임 제거
        DestroyCurrentMiniGame();

        // 중복 없이 랜덤 미니게임 선택
        StageScreen prefab = miniGamePool.GetRandomPrefabWithoutDuplicate();

        if (prefab == null)
        {
            Debug.LogWarning("생성할 미니게임 Prefab이 없습니다.");
            return;
        }

        // 미니게임 생성
        currentMiniGame = Instantiate(prefab, miniGameParent, false);
        currentMiniGame.Init(currentStageNumber);

        // 기존 BallController 같은 외부 스크립트가 buttonHandler를 필요로 하면 자동 연결
        MiniGameRuntimeBinder.BindButtonHandler(currentMiniGame, onClickButton);

        // StageScreen 이벤트 연결
        currentMiniGame.OnStageClearButtonClicked += HandleStageClear;
        currentMiniGame.OnGameOver += HandleGameOver;

        // StartMiniGame() 메서드가 있는 미니게임이면 자동 실행
        MiniGameRuntimeBinder.TryStartMiniGame(currentMiniGame);

        Debug.Log($"{currentStageNumber} 스테이지 시작 / 생성된 미니게임: {prefab.name}");
    }

    private void HandleStageClear(int stageNumber)
    {
        // 점검시간 팝업이 떠 있으면 성공 이벤트 무시
        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 클리어 이벤트 무시");
            return;
        }

        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log($"{stageNumber} 스테이지 클리어 이벤트 받음");

        if (stageClearManager != null)
        {
            stageClearManager.ClearStage(stageNumber);
        }

        popupController.ShowSuccess(gameClockTimer);
    }

    private void HandleGameOver()
    {
        // 점검시간 팝업이 떠 있으면 실패 이벤트 무시
        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 실패 이벤트 무시");
            return;
        }

        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log("미니게임 실패 이벤트 받음");

        popupController.ShowFail(gameClockTimer);
    }

    public void ExternalGameOver()
    {
        // BallController가 buttonHandler.OnClickCancle()을 호출하면 여기로 들어옴

        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 외부 게임오버 무시");
            return;
        }

        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log("외부 스크립트에서 게임오버 호출됨");

        popupController.ShowFail(gameClockTimer);
    }

    public void ForceGameOver()
    {
        // 전체 타이머 종료 시 호출됨
        // 여기서는 currentMiniGame.GameOver()를 호출하지 않음
        // 점검시간 팝업이 실패 팝업보다 우선이기 때문

        Debug.Log("전체 타이머 종료 - 점검시간 팝업 우선 표시");

        popupController.ShowMaintenance(gameClockTimer);
    }

    public void ConfirmSuccess()
    {
        // 성공 팝업 확인 버튼용

        Time.timeScale = 1f;

        popupController.ResetState();

        DestroyCurrentMiniGame();

        popupController.HideAll();

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

        // 성공 후 메인화면에서는 전체 타이머 재개
        if (gameClockTimer != null)
        {
            gameClockTimer.ResumeTimer();
        }

        Debug.Log("성공 확인 - 메인화면으로 이동");
    }

    public void ConfirmFail()
    {
        // 실패 팝업 확인 버튼을 이 함수에 연결해도 됨
        ShowTitleScreen();
    }

    public void EndByMaintenance()
    {
        // 점검시간 팝업 확인 버튼을 이 함수에 연결해도 됨
        ShowTitleScreen();
    }

    public void ShowTitleScreen()
    {
        // 실패 / 점검 / 타이틀 복귀 공통 처리

        Time.timeScale = 1f;

        popupController.ResetState();

        if (gameClockTimer != null)
        {
            gameClockTimer.ResetTimer();
        }

        if (stageClearManager != null)
        {
            stageClearManager.ResetAll();
        }

        currentStageNumber = 0;

        DestroyCurrentMiniGame();

        popupController.HideAll();

        miniGamePool.ResetPool();

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

        Debug.Log("타이틀 화면 이동 - 전체 타이머 초기화");
    }

    public void HideResultObjects()
    {
        popupController.HideAll();
    }

    public void DestroyCurrentMiniGame()
    {
        if (currentMiniGame == null)
        {
            return;
        }

        currentMiniGame.OnStageClearButtonClicked -= HandleStageClear;
        currentMiniGame.OnGameOver -= HandleGameOver;

        Destroy(currentMiniGame.gameObject);
        currentMiniGame = null;
    }
}