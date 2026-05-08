using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransferPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button mainButton;

    private const float CountUpDuration = 2f;
    private Action onClose;
    private bool countingUp;

    public void Show(Action onClose, float timeRatio = 1f)
    {
        this.onClose = onClose;

        // 최소 1만원, 최대 10000만원, 남은 시간 비율로 고정 계산
        int targetMan = Mathf.RoundToInt(Mathf.Lerp(1, 1000, timeRatio));

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(Close);

        mainButton.onClick.RemoveAllListeners();
        mainButton.onClick.AddListener(Close);

        StartCoroutine(CountUp(targetMan));
    }

    private IEnumerator CountUp(int targetMan)
    {
        countingUp = true;

        float elapsed = 0f;
        while (elapsed < CountUpDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / CountUpDuration);
            int current = Mathf.RoundToInt(Mathf.Lerp(0, targetMan, t));
            amountText.text = $"{current:#,0}만원";
            yield return null;
        }

        amountText.text = $"{targetMan:#,0}만원";
        countingUp = false;
    }

    private void Close()
    {
        if (countingUp) return;
        onClose?.Invoke();
        Destroy(gameObject);
    }
}
