using UnityEngine;
using UnityEngine.UI;

public class ButtonMashMiniGame : MonoBehaviour
{
    [Header("UI 참조 (Inspector에서 연결)")]
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;
    [SerializeField] private Image gaugeFill; // 90도 회전된 채움 이미지

    [Header("게이지 설정")]
    [SerializeField] private float fillPerClick = 0.5f;
    [SerializeField] private float decayRate = 0.05f;
    [SerializeField] [Range(0f, 1f)] private float disagreeAppearAt = 0.4f;

    [Header("거절 버튼 추적 설정")]
    [SerializeField] private float smoothTime = 0.15f; // 낮을수록 빠름


    private float gauge;
    private bool gameActive;
    private bool disagreeVisible;
    private RectTransform disagreeRect;
    private Canvas canvas;
    private Vector2 followVelocity;

    public void StartMiniGame()
    {
        gauge = 0f;
        gameActive = true;
        disagreeVisible = false;

        // 자동 탐색
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

        if (agreeButton == null)    { Debug.LogError("[ButtonMash] agreeButton 미연결"); return; }
        if (disagreeButton == null) { Debug.LogError("[ButtonMash] disagreeButton 미연결"); return; }
        if (gaugeFill == null)      { Debug.LogError("[ButtonMash] gaugeFill 미연결"); return; }

        // 이미지가 90도 회전돼 있으므로 Vertical + Bottom = 화면 기준 왼→오
        gaugeFill.type = Image.Type.Filled;
        gaugeFill.fillMethod = Image.FillMethod.Vertical;
        gaugeFill.fillOrigin = (int)Image.OriginVertical.Bottom;
        gaugeFill.fillAmount = 0f;

        // 처음부터 동의 버튼 옆에 배치
        agreeButton.gameObject.SetActive(true);
        disagreeButton.gameObject.SetActive(true);

        disagreeButton.TryGetComponent(out disagreeRect);

        canvas    = GetComponentInParent<Canvas>();

        agreeButton.onClick.AddListener(OnAgreeClicked);
        disagreeButton.onClick.AddListener(OnDisagreeClicked);
    }

    private void OnAgreeClicked()
    {
        if (!gameActive) return;

        gauge = Mathf.Min(gauge + fillPerClick, 1f);
        gaugeFill.fillAmount = gauge;

        // 게이지 40% 도달 시 마우스 추적 시작
        if (!disagreeVisible && gauge >= disagreeAppearAt)
            disagreeVisible = true;

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

        // 시간에 따라 게이지 감소
        if (decayRate > 0f && gauge > 0f)
        {
            gauge = Mathf.Max(0f, gauge - decayRate * Time.deltaTime);
            gaugeFill.fillAmount = gauge;
        }

        // 거절 버튼 마우스 실시간 추적
        if (!disagreeVisible) return;

        Camera uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        RectTransform parentRect = disagreeRect.parent as RectTransform;
        if (parentRect == null) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect, Input.mousePosition, uiCam, out Vector2 localMouse))
        {
            Vector2 current = disagreeRect.localPosition;
            disagreeRect.localPosition = Vector2.SmoothDamp(current, localMouse, ref followVelocity, smoothTime);
        }
    }
}
