using UnityEngine;
using UnityEngine.UI;

public class CheckboxAgreeMiniGame : MonoBehaviour
{
    [Header("체크박스 (Toggle 10개)")]
    [SerializeField] private Toggle[] checkboxes;

    [Header("버튼")]
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;

    [Header("거절 버튼 추적 설정")]
    [SerializeField] private float smoothTime = 0.15f;

    private const int TotalCount = 10;
    private const int ChaseThreshold = 5;

    private bool gameActive;
    private int checkedCount;
    private bool chasing;
    private RectTransform disagreeRect;
    private Canvas canvas;
    private Vector2 followVelocity;

    public void StartMiniGame()
    {
        gameActive = true;
        checkedCount = 0;
        chasing = false;
        followVelocity = Vector2.zero;

        AutoFindComponents();

        if (!Validate()) return;

        canvas = GetComponentInParent<Canvas>();
        disagreeRect = disagreeButton.GetComponent<RectTransform>();

        // 체크박스 초기화
        foreach (var toggle in checkboxes)
        {
            toggle.isOn = false;
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        // 동의 버튼 비활성화, 거절 버튼 활성화
        agreeButton.interactable = false;
        agreeButton.gameObject.SetActive(true);
        disagreeButton.gameObject.SetActive(true);

        agreeButton.onClick.RemoveAllListeners();
        agreeButton.onClick.AddListener(OnAgreeClicked);

        disagreeButton.onClick.RemoveAllListeners();
        disagreeButton.onClick.AddListener(OnDisagreeClicked);
    }

    private void AutoFindComponents()
    {
        if (checkboxes == null || checkboxes.Length == 0)
        {
            Transform checkboxParent = transform.Find("Checkboxes");
            if (checkboxParent != null)
                checkboxes = checkboxParent.GetComponentsInChildren<Toggle>(true);
        }

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
    }

    private bool Validate()
    {
        if (checkboxes == null || checkboxes.Length < TotalCount)
        {
            Debug.LogError($"[CheckboxAgree] Toggle이 {TotalCount}개 필요합니다. 현재: {checkboxes?.Length}");
            return false;
        }
        if (agreeButton == null)  { Debug.LogError("[CheckboxAgree] agreeButton 미연결");    return false; }
        if (disagreeButton == null) { Debug.LogError("[CheckboxAgree] disagreeButton 미연결"); return false; }
        return true;
    }

    private void OnToggleChanged(bool isOn)
    {
        if (!gameActive) return;

        checkedCount = 0;
        foreach (var toggle in checkboxes)
            if (toggle.isOn) checkedCount++;

        // 5개 체크 시 거절 버튼 마우스 추적 시작
        if (!chasing && checkedCount >= ChaseThreshold)
        {
            chasing = true;
            BringDisagreeToFront();
        }

        // 10개 모두 체크 시 동의 버튼 활성화
        agreeButton.interactable = checkedCount >= TotalCount;
    }

    private void OnAgreeClicked()
    {
        if (!gameActive) return;
        gameActive = false;
        MiniGameManager.NotifySuccess();
    }

    private void OnDisagreeClicked()
    {
        if (!gameActive) return;
        gameActive = false;
        MiniGameManager.NotifyFail();
    }

    private void BringDisagreeToFront()
    {
        var overrideCanvas = disagreeButton.gameObject.GetComponent<Canvas>();
        if (overrideCanvas == null)
        {
            overrideCanvas = disagreeButton.gameObject.AddComponent<Canvas>();
            disagreeButton.gameObject.AddComponent<GraphicRaycaster>();
        }
        overrideCanvas.overrideSorting = true;
        overrideCanvas.sortingOrder = 100;
    }

    private void Update()
    {
        if (!gameActive || !chasing || disagreeRect == null || canvas == null) return;

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
