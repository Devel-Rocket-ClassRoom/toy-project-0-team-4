using UnityEngine;
using UnityEngine.UI;

public class MovingMiniGame : MonoBehaviour
{
    [Header("버튼 프리팹")]
    public GameObject agreePrefab;
    public GameObject disagreePrefab;

    [Header("거절 버튼 위치 (anchoredPosition)")]
    public Vector2 disagree1Position = new(-250f, -40f);
    public Vector2 disagree2Position = new( 250f, -40f);

    [Header("동의 버튼 등장 딜레이")]
    public float minDelay = 1f;
    public float maxDelay = 4f;

    [Header("동의 버튼 이동 속도")]
    public float agreeSpeed = 1000f;

    private Button disagreeBtn1;
    private Button disagreeBtn2;
    private RectTransform agreeRect;

    private bool agreeMoving;
    private float agreeTimer;
    private float agreeDelay;
    private float agreeDirection; // +1 = 왼→오, -1 = 오→왼

    void Awake()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    private float halfWidth;

    public void StartMiniGame()
    {
        if (TryGetComponent<VerticalLayoutGroup>(out var vl)) vl.enabled = false;

        // Canvas 크기 기준으로 이동 범위 계산
        halfWidth = 600f;
        var canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            var cr = canvas.GetComponent<RectTransform>();
            if (cr != null && cr.rect.width > 0f) halfWidth = cr.rect.width * 0.5f;
        }
        Debug.Log($"[MiniGames] halfWidth={halfWidth}");

        SpawnDisagreeButtons();

        agreeMoving = false;
        agreeTimer  = 0f;
        agreeDelay  = Random.Range(minDelay, maxDelay);
    }

    void SpawnDisagreeButtons()
    {
        disagreeBtn1 = SpawnButton(disagreePrefab, disagree1Position);
        disagreeBtn2 = SpawnButton(disagreePrefab, disagree2Position);

        disagreeBtn1.onClick = new Button.ButtonClickedEvent();
        disagreeBtn2.onClick = new Button.ButtonClickedEvent();
        disagreeBtn1.onClick.AddListener(MiniGameManager.NotifyFail);
        disagreeBtn2.onClick.AddListener(MiniGameManager.NotifyFail);
    }

    Button SpawnButton(GameObject prefab, Vector2 pos)
    {
        var go = Instantiate(prefab, transform);
        if (go.TryGetComponent<ButtonChange>(out var bc)) bc.enabled = false;

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;

        go.SetActive(true);
        return go.GetComponent<Button>();
    }

    void SpawnAgreeButton()
    {
        if (agreeRect != null) return;

        agreeDirection = Random.value > 0.5f ? 1f : -1f;
        float startX   = agreeDirection > 0f ? -halfWidth - 100f : halfWidth + 100f;

        var go = Instantiate(agreePrefab, transform);
        if (go.TryGetComponent<ButtonChange>(out var bc)) bc.enabled = false;

        agreeRect = go.GetComponent<RectTransform>();
        agreeRect.anchorMin = agreeRect.anchorMax = new Vector2(0.5f, 0.5f);
        agreeRect.anchoredPosition = new Vector2(startX, 0f);
        go.SetActive(true);

        var btn = go.GetComponent<Button>();
        btn.onClick = new Button.ButtonClickedEvent();
        btn.onClick.AddListener(MiniGameManager.NotifySuccess);

        agreeMoving = true;
    }

    void Update()
    {
        if (!agreeMoving)
        {
            agreeTimer += Time.deltaTime;
            if (agreeTimer >= agreeDelay)
                SpawnAgreeButton();
            return;
        }

        if (agreeRect == null) return;

        var pos = agreeRect.anchoredPosition;
        pos.x  += agreeDirection * agreeSpeed * Time.deltaTime;
        agreeRect.anchoredPosition = pos;

        float edge = halfWidth + 150f;
        if (Mathf.Abs(pos.x) > edge)
        {
            Destroy(agreeRect.gameObject);
            agreeRect   = null;
            agreeMoving = false;
            MiniGameManager.NotifyFail();
        }
    }
}
