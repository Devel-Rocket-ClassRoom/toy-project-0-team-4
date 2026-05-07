using System.Collections;
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

    [Header("OTP (5개 스테이지 전부 클리어 시 표시)")]
    [SerializeField] private OTPMiniGame otpPrefab;
    [SerializeField] private int totalStages = 5;

    [Header("OTP 성공 팝업")]
    [SerializeField] private GameObject otpSuccessPopupPrefab;

    [Header("송금 팝업")]
    [SerializeField] private TransferPopup transferPopupPrefab;

    private StageScreen currentMiniGame;
    private int currentStageNumber;
    private OTPMiniGame otpInstance;

    private static readonly WaitForSecondsRealtime WaitOTPSuccess = new(5f);

    private void Awake()
    {
        // 미니게임 중복 방지 랜덤 풀 초기화
        if (miniGamePool != null)
        {
            miniGamePool.ResetPool();
        }

        // 시작 시 모든 결과 팝업 숨김
        if (popupController != null)
        {
            popupController.ResetState();
            popupController.HideAll();
        }
    }

    public void StartStage(int stageNumber)
    {
        if (miniGamePool == null || !miniGamePool.HasPrefab())
        {
            Debug.LogWarning("미니게임 Prefab이 등록되어 있지 않습니다.");
            return;
        }

        // 이전 팝업 때문에 멈춰있을 수 있으므로 정상 시간으로 복구
        Time.timeScale = 1f;

        currentStageNumber = stageNumber;

        // 새 미니게임 시작 전에 팝업 상태 초기화
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

        // 기존 미니게임이 남아있으면 제거
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

        // 현재 스테이지 번호 전달
        currentMiniGame.Init(currentStageNumber);

        // BallController처럼 buttonHandler 필드를 가진 스크립트에 OnClickButton 자동 연결
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
        // 점검시간 팝업이 이미 떠 있으면 클리어 처리 무시
        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 클리어 이벤트 무시");
            return;
        }

        // 이미 성공/실패/타임아웃 팝업이 떠 있으면 중복 처리 방지
        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log($"{stageNumber} 스테이지 클리어 이벤트 받음");

        // 클리어 상태 저장
        if (stageClearManager != null)
        {
            stageClearManager.ClearStage(stageNumber);

            Debug.Log($"[OTP 체크] 클리어된 스테이지: {stageNumber} / totalStages: {totalStages} / AllCleared: {stageClearManager.AllCleared(totalStages)}");

            if (stageClearManager.AllCleared(totalStages))
            {
                ShowOTP();
                return;
            }
        }

        // 성공 팝업 요청
        popupController.ShowSuccess(gameClockTimer);
    }

    private void HandleGameOver()
    {
        // 점검시간 팝업이 이미 떠 있으면 실패 처리 무시
        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 실패 이벤트 무시");
            return;
        }

        // 이미 다른 결과 팝업이 떠 있으면 중복 처리 방지
        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log("미니게임 실패 이벤트 받음");

        // 실패 팝업 요청
        popupController.ShowFail(gameClockTimer);
    }

    private void ShowOTP()
    {
        // StageScreen을 먼저 제거해야 MiniGameManager 이벤트가 OTP에만 전달됨
        DestroyCurrentMiniGame();
        popupController.ResetState();
        popupController.HideAll();
        Time.timeScale = 1f;

        if (otpPrefab != null)
        {
            otpInstance = Instantiate(otpPrefab, miniGameParent, false);
            otpInstance.StartMiniGame();
        }

        MiniGameManager.OnMiniGameSuccess -= OnOTPSuccess;
        MiniGameManager.OnMiniGameSuccess += OnOTPSuccess;
        MiniGameManager.OnMiniGameFail    -= OnOTPFail;
        MiniGameManager.OnMiniGameFail    += OnOTPFail;

        Debug.Log("OTP 미니게임 표시 - 5개 스테이지 전부 클리어");
    }

    private void OnOTPSuccess()
    {
        MiniGameManager.OnMiniGameSuccess -= OnOTPSuccess;
        MiniGameManager.OnMiniGameFail    -= OnOTPFail;

        if (otpInstance != null) Destroy(otpInstance.gameObject);

        StartCoroutine(OTPSuccessSequence());
    }

    private IEnumerator OTPSuccessSequence()
    {
        GameObject successPopup = null;
        if (otpSuccessPopupPrefab != null)
            successPopup = Instantiate(otpSuccessPopupPrefab, miniGameParent, false);

        yield return WaitOTPSuccess;

        if (successPopup != null) Destroy(successPopup);
        Time.timeScale = 1f;

        if (transferPopupPrefab != null)
        {
            TransferPopup popup = Instantiate(transferPopupPrefab, miniGameParent, false);
            popup.Show(ReturnToTitle);
        }
        else
        {
            ReturnToTitle();
        }

        Debug.Log("OTP 성공");
    }

    private void ReturnToTitle()
    {
        MiniGameManager.OnMiniGameSuccess -= OnOTPSuccess;
        MiniGameManager.OnMiniGameFail    -= OnOTPFail;

        DestroyCurrentMiniGame();
        if (stageClearManager != null) stageClearManager.ResetAll();
        if (gameClockTimer != null) gameClockTimer.ResetTimer();
        if (titleScreen != null) titleScreen.SetActive(true);
        if (mainScreen != null) mainScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnOTPFail()
    {
        MiniGameManager.OnMiniGameSuccess -= OnOTPSuccess;
        MiniGameManager.OnMiniGameFail    -= OnOTPFail;

        if (otpInstance != null) Destroy(otpInstance.gameObject);
        popupController.ShowFail(gameClockTimer);

        Debug.Log("OTP 실패");
    }

    public void ExternalGameOver()
    {
        // BallController처럼 외부 스크립트가 OnClickCancle()을 통해 실패 요청할 때 사용

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
        // 전체 시계 타이머가 끝났을 때 호출됨
        // 일반 실패 팝업이 아니라 점검시간 팝업을 최우선으로 표시

        Debug.Log("전체 타이머 종료 - 점검시간 팝업 요청");

        popupController.ShowMaintenance(gameClockTimer);
    }

    public void ShowTimeoutPopup()
    {
        // 각 미니게임 개별 제한시간이 끝났을 때 호출됨

        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 장시간 응답 없음 팝업 무시");
            return;
        }

        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log("장시간 응답 없음 팝업 요청");

        popupController.ShowTimeout(gameClockTimer);
    }

    public void ConfirmSuccess()
    {
        // 성공 팝업 확인 버튼에서 호출
        // 미니게임 삭제 후 메인화면으로 돌아감
        // 전체 시계 타이머는 이어서 진행

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

        if (gameClockTimer != null)
        {
            gameClockTimer.ResumeTimer();
        }

        Debug.Log("성공 확인 - 메인화면으로 이동");
    }

    public void ConfirmFail()
    {
        // 실패 팝업 확인 버튼을 이 함수에 직접 연결해도 됨
        ShowTitleScreen();
    }

    public void EndByMaintenance()
    {
        // 점검시간 팝업 확인 버튼을 이 함수에 직접 연결해도 됨
        ShowTitleScreen();
    }

    public void ShowTitleScreen()
    {
        // 실패 / 점검 / 장시간 응답 없음 / 타이틀 복귀 공통 처리

        Time.timeScale = 1f;

        // 팝업 상태 초기화
        popupController.ResetState();
        popupController.HideAll();

        // 전체 시계 타이머 초기화
        if (gameClockTimer != null)
        {
            gameClockTimer.ResetTimer();
        }

        // 게임 실패 또는 종료 시 클리어 상태 초기화
        if (stageClearManager != null)
        {
            stageClearManager.ResetAll();
        }

        currentStageNumber = 0;

        // 현재 미니게임 삭제
        DestroyCurrentMiniGame();

        // 미니게임 랜덤 풀 초기화
        if (miniGamePool != null)
        {
            miniGamePool.ResetPool();
        }

        // 타이틀 화면으로 복귀
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

        Debug.Log("타이틀 화면 이동 - 전체 상태 초기화");
    }

    public void HideResultObjects()
    {
        // 외부에서 팝업만 숨기고 싶을 때 사용
        popupController.ResetState();
        popupController.HideAll();
    }

    public void DestroyCurrentMiniGame()
    {
        if (currentMiniGame == null)
        {
            return;
        }

        // 이벤트 연결 해제 후 삭제
        currentMiniGame.OnStageClearButtonClicked -= HandleStageClear;
        currentMiniGame.OnGameOver -= HandleGameOver;

        Destroy(currentMiniGame.gameObject);
        currentMiniGame = null;
    }
}