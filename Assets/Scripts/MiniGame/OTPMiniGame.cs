using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OTPMiniGame : MonoBehaviour
{
    [Header("OTP 표시 슬롯 (위 6개 박스, 왼쪽부터)")]
    [SerializeField] private TextMeshProUGUI[] otpSlots;

    [Header("입력 표시 슬롯 (아래 6개 박스, 왼쪽부터)")]
    [SerializeField] private TextMeshProUGUI[] inputSlots;

    [Header("타이머")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image timerFillImage;
    [SerializeField] private float timeLimit = 15f;

    [Header("숫자 버튼 (1~9, 인덱스0=1버튼 ~ 인덱스8=9버튼)")]
    [SerializeField] private Button[] numberButtons;

    [Header("취소(뒤로) / 확인 버튼")]
    [SerializeField] private Button backspaceButton;
    [SerializeField] private Button confirmButton;

    [Header("피드백")]
    [SerializeField] private GameObject wrongFeedbackObject;

    private string targetOTP;
    private string currentInput = "";
    private float remainingTime;
    private bool isRunning;

    private const int OTP_LENGTH = 6;
    private const float OTP_SHOW_DURATION = 5f;

    private static readonly WaitForSeconds WaitWrongFeedback = new(0.8f);

    void Awake()
    {
        AutoFindComponents();
    }

    private void AutoFindComponents()
    {
        if (otpSlots == null || otpSlots.Length == 0)
        {
            Transform otpKey = transform.Find("OTPKEY");
            if (otpKey != null)
                otpSlots = SortedTMPs(otpKey);
        }

        if (inputSlots == null || inputSlots.Length == 0)
        {
            Transform goChild = transform.Find("GameObject");
            if (goChild != null)
                inputSlots = SortedTMPs(goChild);
        }

        if (numberButtons == null || numberButtons.Length == 0)
        {
            Transform numPad = transform.Find("NumberPad");
            if (numPad != null)
                numberButtons = SortedButtons(numPad);
        }

        if (confirmButton == null || backspaceButton == null)
        {
            foreach (var btn in GetComponentsInChildren<Button>(true))
            {
                var label = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (label == null) continue;
                string txt = label.text.Trim();
                if (confirmButton == null && txt == "확인")
                    confirmButton = btn;
                else if (backspaceButton == null && (txt == "취소" || txt == "←" || txt == "⌫"))
                    backspaceButton = btn;
            }
        }
    }

    private TextMeshProUGUI[] SortedTMPs(Transform parent)
    {
        var list = new List<TextMeshProUGUI>(parent.GetComponentsInChildren<TextMeshProUGUI>(true));
        list.Sort((a, b) => a.rectTransform.anchoredPosition.x.CompareTo(b.rectTransform.anchoredPosition.x));
        return list.ToArray();
    }

    private Button[] SortedButtons(Transform parent)
    {
        var list = new List<Button>(parent.GetComponentsInChildren<Button>(true));
        list.Sort((a, b) =>
        {
            float ay = a.GetComponent<RectTransform>().anchoredPosition.y;
            float by = b.GetComponent<RectTransform>().anchoredPosition.y;
            if (Mathf.Abs(ay - by) > 5f)
                return by.CompareTo(ay);
            return a.GetComponent<RectTransform>().anchoredPosition.x
                    .CompareTo(b.GetComponent<RectTransform>().anchoredPosition.x);
        });
        return list.ToArray();
    }

    public void StartMiniGame()
    {
        targetOTP = GenerateOTP();
        currentInput = "";
        remainingTime = timeLimit;
        isRunning = false;

        ClearInputSlots();
        ShowOTPSlots();

        if (wrongFeedbackObject != null)
            wrongFeedbackObject.SetActive(false);

        BindButtons();
        StartCoroutine(OTPRevealThenStart());
    }

    private string GenerateOTP()
    {
        string otp = "";
        for (int i = 0; i < OTP_LENGTH; i++)
            otp += Random.Range(1, 10).ToString();
        return otp;
    }

    private IEnumerator OTPRevealThenStart()
    {
        yield return new WaitForSeconds(OTP_SHOW_DURATION);

        HideOTPSlots();
        isRunning = true;
        remainingTime = timeLimit;
        StartCoroutine(TimerCoroutine());
    }

    private void ShowOTPSlots()
    {
        if (otpSlots == null) return;
        for (int i = 0; i < otpSlots.Length; i++)
        {
            if (otpSlots[i] == null) continue;
            otpSlots[i].text = i < targetOTP.Length ? targetOTP[i].ToString() : "";
        }
    }

    private void HideOTPSlots()
    {
        if (otpSlots == null) return;
        foreach (var slot in otpSlots)
            if (slot != null) slot.text = "";
    }

    private void BindButtons()
    {
        if (numberButtons != null)
        {
            for (int i = 0; i < numberButtons.Length; i++)
            {
                if (numberButtons[i] == null) continue;
                int digit = i + 1;
                numberButtons[i].onClick.RemoveAllListeners();
                numberButtons[i].onClick.AddListener(() => OnNumberPressed(digit));
            }
        }

        if (backspaceButton != null)
        {
            backspaceButton.onClick.RemoveAllListeners();
            backspaceButton.onClick.AddListener(OnBackspace);
        }

        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnConfirm);
        }
    }

    private void OnNumberPressed(int digit)
    {
        if (!isRunning) return;
        if (currentInput.Length >= OTP_LENGTH) return;

        currentInput += digit.ToString();
        UpdateInputSlots();
    }

    private void OnBackspace()
    {
        if (!isRunning) return;
        if (currentInput.Length == 0) return;

        currentInput = currentInput[..^1];
        UpdateInputSlots();
    }

    private void OnConfirm()
    {
        if (!isRunning) return;
        if (currentInput.Length < OTP_LENGTH) return;

        isRunning = false;

        if (currentInput == targetOTP)
            MiniGameManager.NotifySuccess();
        else
            StartCoroutine(ShowWrongAndFail());
    }

    private IEnumerator ShowWrongAndFail()
    {
        if (wrongFeedbackObject != null)
            wrongFeedbackObject.SetActive(true);

        yield return WaitWrongFeedback;

        MiniGameManager.NotifyFail();
    }

    private IEnumerator TimerCoroutine()
    {
        while (remainingTime > 0f && isRunning)
        {
            yield return null;
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }

        if (isRunning)
        {
            isRunning = false;
            MiniGameManager.NotifyFail();
        }
    }

    private void UpdateInputSlots()
    {
        if (inputSlots == null) return;
        for (int i = 0; i < inputSlots.Length; i++)
        {
            if (inputSlots[i] == null) continue;
            inputSlots[i].text = i < currentInput.Length ? currentInput[i].ToString() : "";
        }
    }

    private void ClearInputSlots()
    {
        if (inputSlots == null) return;
        foreach (var slot in inputSlots)
            if (slot != null) slot.text = "";
    }

    private void UpdateTimerDisplay()
    {
        float clamped = Mathf.Max(remainingTime, 0f);
        if (timerText != null) timerText.text = clamped.ToString("F1");
        if (timerFillImage != null) timerFillImage.fillAmount = clamped / timeLimit;
    }
}