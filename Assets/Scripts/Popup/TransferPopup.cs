using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransferPopup : MonoBehaviour
{
    [Header("송금 금액 텍스트")]
    [SerializeField]
    private TextMeshProUGUI amountText;

    [Header("확인 버튼")]
    [SerializeField]
    private Button confirmButton;

    private const float CountUpDuration = 2f;

    // 팝업이 닫힐 때 실행할 함수
    private Action onClose;

    private bool countingUp;

    public void Show(Action onClose, float timeRatio = 1f)
    {
        this.onClose = onClose;

        // 최소 1만원, 최대 5000만원, 남은 시간 비율로 고정 계산
        int targetMan = Mathf.RoundToInt(Mathf.Lerp(1, 5000, timeRatio));

        if (confirmButton != null)
        {
            // Inspector에 연결된 OnClick을 지우고 코드에서 Close만 연결
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(Close);
        }
        else
        {
            Debug.LogWarning("TransferPopup: Confirm Button이 연결되지 않았습니다.");
        }

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

            if (amountText != null)
            {
                amountText.text = $"{current:#,0}만원을";
            }

            yield return null;
        }

        if (amountText != null)
        {
            amountText.text = $"{targetMan:#,0}만원을";
        }

        countingUp = false;
    }

    private void Close()
    {
        // 카운트업 중에는 확인 버튼 무시
        if (countingUp)
            return;

        // 타이틀 화면 이동 같은 외부 처리 실행
        onClose?.Invoke();

        Destroy(gameObject);
    }
}
