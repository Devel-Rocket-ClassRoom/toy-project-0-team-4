using UnityEngine;

public partial class MiniGameSpawner : MonoBehaviour
{
    [Header("화면")]
    [SerializeField]
    private GameObject titleScreen;

    [SerializeField]
    private GameObject mainScreen;

    [Header("미니게임이 생성될 부모")]
    [SerializeField]
    private Transform miniGameParent;

    [Header("미니게임 랜덤 풀")]
    [SerializeField]
    private MiniGamePool miniGamePool = new MiniGamePool();

    [Header("팝업 컨트롤러")]
    [SerializeField]
    private MiniGamePopup popupController = new MiniGamePopup();

    [Header("클리어 매니저")]
    [SerializeField]
    private StageClearManager stageClearManager;

    [Header("메인화면 UI")]
    [SerializeField]
    private MainScreenUI mainScreenUI;

    [Header("전체 시계 타이머")]
    [SerializeField]
    private GameClockTimer gameClockTimer;

    [Header("버튼 핸들러")]
    [SerializeField]
    private OnClickButton onClickButton;

    [Header("OTP")]
    [SerializeField]
    private OTPMiniGame otpPrefab;

    [SerializeField]
    private int totalStages = 5;

    [Header("OTP 성공 팝업")]
    [SerializeField]
    private GameObject otpSuccessPopupPrefab;

    [Header("송금 팝업")]
    [SerializeField]
    private TransferPopup transferPopupPrefab;

    private StageScreen currentMiniGame;
    private int currentStageNumber;
    private OTPMiniGame otpInstance;

    private PopupGate gate;

    private static readonly WaitForSecondsRealtime WaitOTPSuccess = new WaitForSecondsRealtime(5f);

    private void Awake()
    {
        gate = new PopupGate(popupController);

        if (miniGamePool != null)
        {
            miniGamePool.ResetAllPools();
        }

        gate.ResetAndHide();
    }

    public void StartStage(int stageNumber)
    {
        if (miniGamePool == null || !miniGamePool.HasStage(stageNumber))
        {
            Debug.LogWarning($"{stageNumber} 스테이지 미니게임 Prefab이 등록되어 있지 않습니다.");
            return;
        }

        Time.timeScale = 1f;

        currentStageNumber = stageNumber;

        gate.ResetAndHide();

        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }

        DestroyCurrentMiniGame();
        DestroyOTPInstance();

        StageScreen prefab = miniGamePool.GetRandomPrefabByStage(currentStageNumber);

        if (prefab == null)
        {
            Debug.LogWarning(
                $"{currentStageNumber} 스테이지에서 생성할 미니게임 Prefab이 없습니다."
            );
            return;
        }

        currentMiniGame = Instantiate(prefab, miniGameParent, false);

        currentMiniGame.Init(currentStageNumber);

        ApplyStageTitleToCurrentMiniGame(currentStageNumber);

        MiniGameRuntimeBinder.BindButtonHandler(currentMiniGame, onClickButton);

        currentMiniGame.OnStageClearButtonClicked += HandleStageClear;
        currentMiniGame.OnGameOver += HandleGameOver;

        MiniGameRuntimeBinder.TryStartMiniGame(currentMiniGame);

        Debug.Log($"{currentStageNumber} 스테이지 시작 / 생성된 미니게임: {prefab.name}");
    }
}
