using UnityEngine;
using UnityEngine.UI;

public class JumpButtonGame : MonoBehaviour
{
    [Header("버튼 프리팹")]
    public GameObject agreePrefab;
    public GameObject disagreePrefab;

    [Header("거절 버튼 위치")]
    public Vector2 disagree1Position = new(-250f, -40f);
    public Vector2 disagree2Position = new( 250f, -40f);

    [Header("동의 버튼 등장 딜레이")]
    public float minDelay = 0.5f;
    public float maxDelay = 3f;

    [Header("포물선 설정")]
    public float gravity       = 1500f;
    public float minVelocityY  = 2200f;
    public float maxVelocityY  = 2800f;
    public float maxVelocityX  = 300f;

    [Header("시작 X 범위")]
    public float spawnRangeX = 350f;

    private RectTransform agreeRect;
    private Vector2       velocity;
    private float         startY;
    private bool          jumping;
    private float         agreeTimer;
    private float         agreeDelay;

    void Awake()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public void StartMiniGame()
    {
        if (agreePrefab == null || disagreePrefab == null)
        {
            Debug.LogError($"[JumpButtonGame] 프리팹 미연결 on {gameObject.name}");
            return;
        }

        foreach (var layout in GetComponents<LayoutGroup>())
            layout.enabled = false;

        SpawnDisagreeButtons();

        jumping    = false;
        agreeTimer = 0f;
        agreeDelay = Random.Range(minDelay, maxDelay);
    }

    void SpawnDisagreeButtons()
    {
        SpawnButton(disagreePrefab, disagree1Position, MiniGameManager.NotifyFail);
        SpawnButton(disagreePrefab, disagree2Position, MiniGameManager.NotifyFail);
        Debug.Log("[JumpButtonGame] 거절 버튼 2개 생성됨");
    }

    void SpawnButton(GameObject prefab, Vector2 pos, UnityEngine.Events.UnityAction onClick)
    {
        var go = Instantiate(prefab, transform);
        if (go.TryGetComponent<ButtonChange>(out var bc)) bc.enabled = false;

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        go.SetActive(true);

        var btn = go.GetComponent<Button>();
        btn.onClick = new Button.ButtonClickedEvent();
        btn.onClick.AddListener(onClick);
    }

    void LaunchAgreeButton()
    {
        var canvas = GetComponentInParent<Canvas>();
        float halfH = canvas != null ? canvas.GetComponent<RectTransform>().rect.height * 0.5f : 400f;

        startY = -halfH - 60f;

        float spawnX = Random.Range(-spawnRangeX, spawnRangeX);

        var go = Instantiate(agreePrefab, transform);
        if (go.TryGetComponent<ButtonChange>(out var bc)) bc.enabled = false;

        agreeRect = go.GetComponent<RectTransform>();
        agreeRect.anchorMin = agreeRect.anchorMax = new Vector2(0.5f, 0.5f);
        agreeRect.anchoredPosition = new Vector2(spawnX, startY);
        go.SetActive(true);

        velocity = new Vector2(
            Random.Range(-maxVelocityX, maxVelocityX),
            Random.Range(minVelocityY, maxVelocityY)
        );

        var btn = go.GetComponent<Button>();
        btn.onClick = new Button.ButtonClickedEvent();
        btn.onClick.AddListener(() =>
        {
            jumping = false;
            MiniGameManager.NotifySuccess();
        });

        jumping = true;
    }

    void Update()
    {
        if (!jumping)
        {
            agreeTimer += Time.deltaTime;
            if (agreeTimer >= agreeDelay)
                LaunchAgreeButton();
            return;
        }

        if (agreeRect == null) return;

        velocity.y -= gravity * Time.deltaTime;

        var pos = agreeRect.anchoredPosition;
        pos    += velocity * Time.deltaTime;
        agreeRect.anchoredPosition = pos;

        if (pos.y < startY)
        {
            Destroy(agreeRect.gameObject);
            agreeRect = null;
            jumping   = false;
            MiniGameManager.NotifyFail();
        }
    }
}
