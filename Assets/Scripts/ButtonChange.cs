using UnityEngine;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour
{
    [Header("버튼 프리팹")]
    public GameObject agreePrefab;
    public GameObject disagreePrefab;

    // 런타임에 생성된 버튼 인스턴스
    private Button agreeButton;
    private Button disagreeButton;

    [Header("미니게임3 - 이미지 스왑")]
    public Sprite agreeSprite;    // 동의 버튼에 원래 표시할 이미지
    public Sprite disagreeSprite; // 동의하지않음 버튼에 원래 표시할 이미지

    [Header("미니게임1 - 버튼 시작 크기")]
    public float game1StartScale = 0.3f;

    [Header("미니게임2 - 동의하지않음 버튼 성장")]
    public float game2StartScale = 1.0f;
    public float game2MaxScale = 15.0f;
    public float game2GrowSpeed = 5.0f;

    private float currentScale;
    private RectTransform agreeRect;
    private RectTransform disagreeRect;

    [Header("미니게임4 - 동의 버튼 도망")]
    public float game4EscapeRadius = 150f;
    public float game4EscapeSpeed = 400f;

    [Header("미니게임5 - 숨겨진 동의 버튼 찾기")]
    public int game5Columns    = 8;
    public int game5Rows       = 10;
    public int game5DisagreeCount = 5;
    public Vector2 game5CellSize    = new Vector2(60f, 40f);
    public Vector2 game5CellSpacing = new Vector2(4f,  4f);
    public Color game5NormalColor   = Color.white;
    public Color game5AgreeFound    = new Color(0.2f, 0.8f, 0.2f);
    public Color game5DisagreeFound = new Color(0.9f, 0.2f, 0.2f);

    private GameObject game5Grid;

    [Header("이 프리팹이 담당할 게임 번호 (1~5)")]
    public int gameIndex = 1;

    private enum MiniGameState { None, Game1, Game2, Game3, Game4, Game5 }
    private MiniGameState currentState = MiniGameState.None;

    void Awake()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if (gameIndex == 1) MiniGameManager.OnGame1Start += StartGame1;
        if (gameIndex == 2) MiniGameManager.OnGame2Start += StartGame2;
        if (gameIndex == 3) MiniGameManager.OnGame3Start += StartGame3;
        if (gameIndex == 4) MiniGameManager.OnGame4Start += StartGame4;
        if (gameIndex == 5) MiniGameManager.OnGame5Start += StartGame5;
    }

    void OnDisable()
    {
        if (gameIndex == 1) MiniGameManager.OnGame1Start -= StartGame1;
        if (gameIndex == 2) MiniGameManager.OnGame2Start -= StartGame2;
        if (gameIndex == 3) MiniGameManager.OnGame3Start -= StartGame3;
        if (gameIndex == 4) MiniGameManager.OnGame4Start -= StartGame4;
        if (gameIndex == 5) MiniGameManager.OnGame5Start -= StartGame5;
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
        RegisterListeners(
            () => { currentState = MiniGameState.None; MiniGameManager.NotifySuccess(); },
            ()   => MiniGameManager.NotifyFail()
        );
    }

    // ─── 미니게임 3: 동의/동의하지않음 버튼 이미지를 서로 교체 ────────
    void StartGame3()
    {
        ShowButtons();
        currentState = MiniGameState.Game3;
        agreeRect.localScale    = Vector3.one;
        disagreeRect.localScale = Vector3.one;
        
        // 이미지를 서로 바꿔서 어떤 버튼이 어느 역할인지 헷갈리게 함
        SetButtonImage(agreeButton,    disagreeSprite);
        SetButtonImage(disagreeButton, agreeSprite);
        // 이미지가 바뀌었으므로 역할도 반대로 등록
        RegisterListeners(
            () => MiniGameManager.NotifyFail(),
            () => { currentState = MiniGameState.None; MiniGameManager.NotifySuccess(); }
        );
    }

    // ─── 미니게임 4: 동의 버튼이 커서 반대 방향으로 도망 ─────────────
    void StartGame4()
    {
        ShowButtons();
        currentState = MiniGameState.Game4;
        agreeRect.localScale    = Vector3.one;
        disagreeRect.localScale = Vector3.one;
        RegisterListeners(
            () => { currentState = MiniGameState.None; MiniGameManager.NotifySuccess(); },
            ()   => MiniGameManager.NotifyFail()
        );
    }

    // ─── 미니게임 5: 그리드 셀 중 숨겨진 동의 버튼 찾기 ─────────────
    void StartGame5()
    {
        currentState = MiniGameState.Game5;

        // 그리드 컨테이너 생성
        game5Grid = new GameObject("Game5Grid");
        game5Grid.transform.SetParent(transform, false);
        var gridRect = game5Grid.AddComponent<RectTransform>();
        gridRect.anchorMin = Vector2.zero;
        gridRect.anchorMax = Vector2.one;
        gridRect.offsetMin = Vector2.zero;
        gridRect.offsetMax = Vector2.zero;

        var layout = game5Grid.AddComponent<GridLayoutGroup>();
        layout.cellSize        = game5CellSize;
        layout.spacing         = game5CellSpacing;
        layout.constraint      = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = game5Columns;
        layout.childAlignment  = TextAnchor.MiddleCenter;

        // 셀 타입 배열 생성 후 셔플 (Agree 1개, Disagree N개, 나머지 Empty)
        int total = game5Columns * game5Rows;
        var types = new int[total]; // 0=Empty, 1=Agree, 2=Disagree
        types[0] = 1;
        for (int i = 1; i <= Mathf.Min(game5DisagreeCount, total - 1); i++)
            types[i] = 2;
        for (int i = total - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (types[i], types[j]) = (types[j], types[i]);
        }

        for (int i = 0; i < total; i++)
        {
            int cellType = types[i];
            var cellGO = new GameObject($"Cell_{i}", typeof(RectTransform), typeof(Image), typeof(Button));
            cellGO.transform.SetParent(game5Grid.transform, false);

            var img = cellGO.GetComponent<Image>();
            img.color = game5NormalColor;

            // 셀 테두리 효과를 위해 Outline 추가
            var outline = cellGO.AddComponent<Outline>();
            outline.effectColor    = new Color(0.5f, 0.5f, 0.5f, 1f);
            outline.effectDistance = new Vector2(1f, -1f);

            var btn = cellGO.GetComponent<Button>();
            int clickCount = 0;

            btn.onClick.AddListener(() =>
            {
                clickCount++;
                if (clickCount < 3) return;

                img.color = Color.clear;
                btn.interactable = false;

                if (cellType == 0) return;

                var prefab = cellType == 1 ? agreePrefab : disagreePrefab;
                var realGO = Instantiate(prefab, cellGO.transform);
                var rt     = realGO.GetComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = rt.offsetMax = Vector2.zero;

                if (realGO.TryGetComponent<ButtonChange>(out var bc)) bc.enabled = false;

                var realBtn = realGO.GetComponent<Button>();
                realBtn.onClick = new Button.ButtonClickedEvent();
                if (cellType == 1)
                    realBtn.onClick.AddListener(() =>
                    {
                        currentState = MiniGameState.None;
                        MiniGameManager.NotifySuccess();
                    });
                else
                    realBtn.onClick.AddListener(() => MiniGameManager.NotifyFail());
            });
        }
    }

    // ─── Update: 스케일 애니메이션 / 버튼 도망 ──────────────────────
    void Update()
    {
        if (currentState == MiniGameState.Game2 && currentScale < game2MaxScale)
        {
            currentScale = Mathf.Min(currentScale + game2GrowSpeed * Time.deltaTime, game2MaxScale);
            disagreeRect.localScale = Vector3.one * currentScale;
        }

        if (currentState == MiniGameState.Game4 && agreeRect != null)
        {
            var parentRect = agreeRect.parent as RectTransform;
            if (parentRect == null) return;

            Canvas canvas = GetComponentInParent<Canvas>();
            Camera uiCamera = canvas != null ? canvas.worldCamera : null;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                Input.mousePosition,
                uiCamera,
                out Vector2 localMouse
            );

            Vector2 agreePos = agreeRect.anchoredPosition;
            Vector2 diff = agreePos - localMouse;

            if (diff.magnitude < game4EscapeRadius)
            {
                Vector2 escapeDir = Vector2.zero == diff ? Vector2.right : diff.normalized;
                agreePos += escapeDir * game4EscapeSpeed * Time.deltaTime;

                Vector2 half = parentRect.rect.size * 0.5f;
                agreePos.x = Mathf.Clamp(agreePos.x, -half.x, half.x);
                agreePos.y = Mathf.Clamp(agreePos.y, -half.y, half.y);

                agreeRect.anchoredPosition = agreePos;
            }
        }
    }

    // ─── 공통 유틸 ───────────────────────────────────────────────────
    void RegisterListeners(UnityEngine.Events.UnityAction onAgree, UnityEngine.Events.UnityAction onDisagree)
    {
        agreeButton.onClick    = new Button.ButtonClickedEvent();
        disagreeButton.onClick = new Button.ButtonClickedEvent();
        agreeButton.onClick.AddListener(onAgree);
        disagreeButton.onClick.AddListener(onDisagree);
    }

    void SetButtonImage(Button btn, Sprite sprite)
    {
        if (btn.targetGraphic is Image img)
            img.sprite = sprite;
    }

}
