using UnityEngine;
using UnityEngine.UI;

public class HiddenAgreeGame : MonoBehaviour
{
    [Header("버튼 프리팹")]
    public GameObject agreePrefab;
    public GameObject disagreePrefab;

    [Header("동의 버튼 등장 딜레이")]
    public float minDelay = 1f;
    public float maxDelay = 4f;

    [Header("동의 버튼 불투명도 (0=투명, 1=불투명)")]
    [Range(0f, 1f)]
    public float agreeAlpha = 0.3f;

    [Header("동의 버튼 등장 범위 (anchoredPosition 최대값)")]
    public float spawnRangeX = 480f;
    public float spawnRangeY = 220f;

    private float agreeTimer;
    private float agreeDelay;
    private bool agreeSpawned;

    void Awake()
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
    }

    public void StartMiniGame()
    {
        if (TryGetComponent<VerticalLayoutGroup>(out var vl)) vl.enabled = false;

        SpawnDisagreeButton();

        agreeTimer   = 0f;
        agreeDelay   = Random.Range(minDelay, maxDelay);
        agreeSpawned = false;
    }

    void SpawnDisagreeButton()
    {
        var go = Instantiate(disagreePrefab, transform);
        if (go.TryGetComponent<ButtonChange>(out var bc)) bc.enabled = false;

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        go.SetActive(true);

        var btn = go.GetComponent<Button>();
        btn.onClick = new Button.ButtonClickedEvent();
        btn.onClick.AddListener(MiniGameManager.NotifyFail);
    }

    void SpawnAgreeButton()
    {
        var go = Instantiate(agreePrefab, transform);
        if (go.TryGetComponent<ButtonChange>(out var bc)) bc.enabled = false;

        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(
            Random.Range(-spawnRangeX, spawnRangeX),
            Random.Range(-spawnRangeY, spawnRangeY)
        );
        go.SetActive(true);

        // 불투명도 적용
        var img = go.GetComponent<Image>();
        if (img != null)
        {
            var c = img.color;
            c.a = agreeAlpha;
            img.color = c;
        }

        var btn = go.GetComponent<Button>();
        btn.onClick = new Button.ButtonClickedEvent();
        btn.onClick.AddListener(MiniGameManager.NotifySuccess);

        agreeSpawned = true;
    }

    void Update()
    {
        if (agreeSpawned) return;

        agreeTimer += Time.deltaTime;
        if (agreeTimer >= agreeDelay)
            SpawnAgreeButton();
    }
}
