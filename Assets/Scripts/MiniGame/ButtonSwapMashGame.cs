using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSwapMashGame : MonoBehaviour
{
    [Header("UI 참조 (Inspector에서 연결)")]
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;
    [SerializeField] private Image gaugeFill;

    [Header("게이지 설정")]
    [SerializeField] private float fillPerClick = 0.05f;
    [SerializeField] private float decayRate = 0.05f;

    [Header("스왑 설정")]
    [SerializeField] private float swapIntervalMin = 1.5f;
    [SerializeField] private float swapIntervalMax = 3.5f;

    [Header("스왑 이펙트")]
    [SerializeField] private float punchScale = 1.4f;
    [SerializeField] private float punchDuration = 0.25f;

    private float gauge;
    private bool gameActive;
    private float swapTimer;
    private RectTransform agreeRect;
    private RectTransform disagreeRect;

    public void StartMiniGame()
    {
        gauge = 0f;
        gameActive = true;

        if (agreeButton == null)
        {
            Transform t = transform.Find("버튼/동의");
            if (t != null) agreeButton = t.GetComponent<Button>();
        }
        if (disagreeButton == null)
        {
            Transform t = transform.Find("버튼/거절");
            if (t != null) disagreeButton = t.GetComponent<Button>();
        }
        if (gaugeFill == null)
        {
            Transform t = transform.Find("Image/Image");
            if (t != null) gaugeFill = t.GetComponent<Image>();
        }

        if (agreeButton == null)    { Debug.LogError("[ButtonSwapMash] agreeButton 미연결"); return; }
        if (disagreeButton == null) { Debug.LogError("[ButtonSwapMash] disagreeButton 미연결"); return; }
        if (gaugeFill == null)      { Debug.LogError("[ButtonSwapMash] gaugeFill 미연결"); return; }

        gaugeFill.type = Image.Type.Filled;
        gaugeFill.fillMethod = Image.FillMethod.Vertical;
        gaugeFill.fillOrigin = (int)Image.OriginVertical.Bottom;
        gaugeFill.fillAmount = 0f;

        agreeButton.gameObject.SetActive(true);
        disagreeButton.gameObject.SetActive(true);

        agreeRect    = agreeButton.GetComponent<RectTransform>();
        disagreeRect = disagreeButton.GetComponent<RectTransform>();

        agreeButton.onClick.AddListener(OnAgreeClicked);
        disagreeButton.onClick.AddListener(OnDisagreeClicked);

        ResetSwapTimer();
    }

    private void OnAgreeClicked()
    {
        if (!gameActive) return;

        gauge = Mathf.Min(gauge + fillPerClick, 1f);
        gaugeFill.fillAmount = gauge;

        if (gauge >= 1f)
        {
            gameActive = false;
            MiniGameManager.NotifySuccess();
        }
    }

    private void OnDisagreeClicked()
    {
        if (!gameActive) return;
        gameActive = false;
        MiniGameManager.NotifyFail();
    }

    private void Update()
    {
        if (!gameActive) return;

        if (decayRate > 0f && gauge > 0f)
        {
            gauge = Mathf.Max(0f, gauge - decayRate * Time.deltaTime);
            gaugeFill.fillAmount = gauge;
        }

        swapTimer -= Time.deltaTime;
        if (swapTimer <= 0f)
        {
            SwapButtonPositions();
            ResetSwapTimer();
        }
    }

    private void SwapButtonPositions()
    {
        Vector2 agreePos    = agreeRect.localPosition;
        Vector2 disagreePos = disagreeRect.localPosition;

        agreeRect.localPosition    = disagreePos;
        disagreeRect.localPosition = agreePos;

        StartCoroutine(PunchScale(agreeRect));
        StartCoroutine(PunchScale(disagreeRect));
    }

    private IEnumerator PunchScale(RectTransform target)
    {
        Vector3 original = target.localScale;
        Vector3 big = original * punchScale;
        float half = punchDuration * 0.5f;

        // 커지기
        for (float t = 0f; t < half; t += Time.deltaTime)
        {
            target.localScale = Vector3.Lerp(original, big, t / half);
            yield return null;
        }

        // 돌아오기
        for (float t = 0f; t < half; t += Time.deltaTime)
        {
            target.localScale = Vector3.Lerp(big, original, t / half);
            yield return null;
        }

        target.localScale = original;
    }

    private void ResetSwapTimer()
    {
        swapTimer = Random.Range(swapIntervalMin, swapIntervalMax);
    }
}
