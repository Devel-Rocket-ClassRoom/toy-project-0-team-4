using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonChange : MonoBehaviour
{
    [Header("버튼 프리팹")]
    public GameObject agreePrefab;
    public GameObject disagreePrefab;

    // 런타임에 생성된 버튼 인스턴스
    private Button agreeButton;
    private Button disagreeButton;

    [Header("미니게임1 - 역할 스왑 색상")]
    public Color agreeColor = new(0.2f, 0.8f, 0.2f);
    public Color disagreeColor = new(0.9f, 0.2f, 0.2f);

    [Header("미니게임1 - 버튼 시작 크기")]
    public float game1StartScale = 0.3f;

    [Header("미니게임2 - 동의하지않음 버튼 성장")]
    public float game2StartScale = 1.0f;
    public float game2MaxScale = 15.0f;
    public float game2GrowSpeed = 5.0f;

    private float currentScale;
    private RectTransform agreeRect;
    private RectTransform disagreeRect;

    [Header("이 프리팹이 담당할 게임 번호 (1, 2, 3)")]
    [SerializeField] private int gameIndex = 1;

    private enum MiniGameState { None, Game1, Game2, Game3 }
    private MiniGameState currentState = MiniGameState.None;

    void Awake()
    {
        // Canvas 밖에 있으면 매니저 역할 → 시각 컴포넌트 숨김
        if (GetComponentInParent<Canvas>() == null)
        {
            var img = GetComponent<Image>();
            var btn = GetComponent<Button>();
            var txt = GetComponentInChildren<TextMeshProUGUI>();
            if (img != null) img.enabled = false;
            if (btn != null) btn.enabled = false;
            if (txt != null) txt.enabled = false;
        }
    }

    void OnEnable()
    {
        if (gameIndex == 1) MiniGameManager.OnGame1Start += StartGame1;
        if (gameIndex == 2) MiniGameManager.OnGame2Start += StartGame2;
        if (gameIndex == 3) MiniGameManager.OnGame3Start += StartGame3;
    }

    void OnDisable()
    {
        if (gameIndex == 1) MiniGameManager.OnGame1Start -= StartGame1;
        if (gameIndex == 2) MiniGameManager.OnGame2Start -= StartGame2;
        if (gameIndex == 3) MiniGameManager.OnGame3Start -= StartGame3;
    }

    void ShowButtons()
    {
        if (agreeButton == null)
        {
            if (agreePrefab == null)    { Debug.LogError("[ButtonChange] agreePrefab이 비어있음"); return; }
            if (disagreePrefab == null) { Debug.LogError("[ButtonChange] disagreePrefab이 비어있음"); return; }

            GameObject agreeGO    = Instantiate(agreePrefab,    transform);
            GameObject disagreeGO = Instantiate(disagreePrefab, transform);

            // 생성된 버튼의 ButtonChange가 또 버튼을 만들지 못하도록 비활성화
            var agreeBC    = agreeGO.GetComponent<ButtonChange>();
            var disagreeBC = disagreeGO.GetComponent<ButtonChange>();
            if (agreeBC    != null) agreeBC.enabled    = false;
            if (disagreeBC != null) disagreeBC.enabled = false;

            agreeButton    = agreeGO.GetComponent<Button>();
            disagreeButton = disagreeGO.GetComponent<Button>();
            agreeRect      = agreeGO.GetComponent<RectTransform>();
            disagreeRect   = disagreeGO.GetComponent<RectTransform>();

            // 화면 중앙 기준으로 좌우 배치
            agreeRect.anchorMin    = agreeRect.anchorMax    = new Vector2(0.5f, 0.5f);
            disagreeRect.anchorMin = disagreeRect.anchorMax = new Vector2(0.5f, 0.5f);
            agreeRect.anchoredPosition    = new Vector2(-150f, 0f);
            disagreeRect.anchoredPosition = new Vector2( 150f, 0f);
        }

        agreeButton.gameObject.SetActive(true);
        disagreeButton.gameObject.SetActive(true);
    }

    // ─── 미니게임 1: 두 버튼 매우 작게 시작 ────────────────────────
    void StartGame1()
    {
        ShowButtons();
        currentState = MiniGameState.Game1;
        agreeRect.localScale    = Vector3.one * game1StartScale;
        disagreeRect.localScale = Vector3.one * game1StartScale;
        ResetColors();
        RegisterListeners(
            () => { currentState = MiniGameState.None; MiniGameManager.NotifySuccess(); },
            ()   => MiniGameManager.NotifyFail()
        );
    }

    // ─── 미니게임 2: 동의하지않음 버튼만 커짐 ───────────────────────
    void StartGame2()
    {
        ShowButtons();
        currentState = MiniGameState.Game2;
        currentScale = game2StartScale;
        agreeRect.localScale    = Vector3.one;
        disagreeRect.localScale = Vector3.one * game2StartScale;
        ResetColors();
        RegisterListeners(
            () => { currentState = MiniGameState.None; MiniGameManager.NotifySuccess(); },
            ()   => MiniGameManager.NotifyFail()
        );
    }

    // ─── 미니게임 3: 동의함=빨강, 동의하지않음=초록 ─────────────────
    void StartGame3()
    {
        ShowButtons();
        currentState = MiniGameState.Game3;
        agreeRect.localScale    = Vector3.one;
        disagreeRect.localScale = Vector3.one;
        SetButtonColor(agreeButton,    disagreeColor);
        SetButtonColor(disagreeButton, agreeColor);
        RegisterListeners(
            () => { currentState = MiniGameState.None; MiniGameManager.NotifySuccess(); },
            ()   => MiniGameManager.NotifyFail()
        );
    }

    // ─── Update: 스케일 애니메이션 ───────────────────────────────────
    void Update()
    {
        if (currentState == MiniGameState.Game2 && currentScale < game2MaxScale)
        {
            currentScale = Mathf.Min(currentScale + game2GrowSpeed * Time.deltaTime, game2MaxScale);
            disagreeRect.localScale = Vector3.one * currentScale;
        }
    }

    // ─── 공통 유틸 ───────────────────────────────────────────────────
    void RegisterListeners(UnityEngine.Events.UnityAction onAgree, UnityEngine.Events.UnityAction onDisagree)
    {
        agreeButton.onClick.RemoveAllListeners();
        disagreeButton.onClick.RemoveAllListeners();
        agreeButton.onClick.AddListener(onAgree);
        disagreeButton.onClick.AddListener(onDisagree);
    }

    void ResetColors()
    {
        SetButtonColor(agreeButton,    agreeColor);
        SetButtonColor(disagreeButton, disagreeColor);
    }

    void SetButtonColor(Button btn, Color color)
    {
        btn.interactable = true;
        ColorBlock cb = btn.colors;
        cb.normalColor    = color;
        cb.colorMultiplier = 1f;
        btn.colors = cb;
        if (btn.targetGraphic != null)
            btn.targetGraphic.color = color;
    }

}
