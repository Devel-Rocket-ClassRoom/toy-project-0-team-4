using UnityEngine;

[System.Serializable]
public class MiniGamePopup
{
    [Header("팝업 루트")]
    [SerializeField] private GameObject popupRoot;

    [Header("팝업 오브젝트")]
    [SerializeField] private GameObject successPopup;
    [SerializeField] private GameObject failPopup;
    [SerializeField] private GameObject maintenancePopup;
    [SerializeField] private GameObject timeoutPopup;
    [SerializeField] private GameObject meaningErrorPopup;

    // 현재 결과 팝업이 떠 있는지 확인
    public bool IsResultOpen { get; private set; }

    // 점검시간 팝업은 최우선 팝업
    public bool IsMaintenanceOpen { get; private set; }

    /// <summary>
    /// 팝업 상태 초기화
    /// </summary>
    public void ResetState()
    {
        IsResultOpen = false;
        IsMaintenanceOpen = false;
    }

    /// <summary>
    /// 성공 팝업 표시
    /// </summary>
    public void ShowSuccess(GameClockTimer gameClockTimer)
    {
        // 점검시간 팝업이 떠 있으면 성공 팝업은 무시
        if (IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 성공 팝업 무시");
            return;
        }

        // 이미 다른 결과 팝업이 떠 있으면 중복 표시 방지
        if (IsResultOpen)
        {
            return;
        }

        IsResultOpen = true;

        PauseGameAndTimer(gameClockTimer);

        SetPopupState(
            showSuccess: true,
            showFail: false,
            showMaintenance: false,
            showTimeout: false,
            showMeaningError: false
        );

        Debug.Log("성공 팝업 표시");
    }

    /// <summary>
    /// 실패 팝업 표시
    /// </summary>
    public void ShowFail(GameClockTimer gameClockTimer)
    {
        // 점검시간 팝업이 떠 있으면 실패 팝업은 무시
        if (IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 실패 팝업 무시");
            return;
        }

        if (IsResultOpen)
        {
            return;
        }

        IsResultOpen = true;

        PauseGameAndTimer(gameClockTimer);

        SetPopupState(
            showSuccess: false,
            showFail: true,
            showMaintenance: false,
            showTimeout: false,
            showMeaningError: false
        );

        Debug.Log("실패 팝업 표시");
    }

    /// <summary>
    /// 점검시간 팝업 표시
    /// 전체 타이머 종료 시 최우선으로 표시됨
    /// </summary>
    public void ShowMaintenance(GameClockTimer gameClockTimer)
    {
        if (IsMaintenanceOpen)
        {
            return;
        }

        IsMaintenanceOpen = true;
        IsResultOpen = true;

        PauseGameAndTimer(gameClockTimer);

        SetPopupState(
            showSuccess: false,
            showFail: false,
            showMaintenance: true,
            showTimeout: false,
            showMeaningError: false
        );

        Debug.Log("점검시간 팝업 표시");
    }

    /// <summary>
    /// 장시간 응답 없음 팝업 표시
    /// </summary>
    public void ShowTimeout(GameClockTimer gameClockTimer)
    {
        // 점검시간 팝업이 떠 있으면 장시간 응답 없음은 무시
        if (IsMaintenanceOpen)
        {
            Debug.Log("점검시간 팝업이 우선이라 장시간 응답 없음 팝업 무시");
            return;
        }

        if (IsResultOpen)
        {
            return;
        }

        IsResultOpen = true;

        PauseGameAndTimer(gameClockTimer);

        SetPopupState(
            showSuccess: false,
            showFail: false,
            showMaintenance: false,
            showTimeout: true,
            showMeaningError: false
        );

        Debug.Log("장시간 응답 없음 팝업 표시");
    }

    /// <summary>
    /// 의미 불명 팝업 표시
    /// </summary>
    public void ShowMeaningError(GameClockTimer gameClockTimer)
    {
        // 점검시간 팝업이 떠 있으면 의미 불명 팝업은 무시
        if (IsMaintenanceOpen)
        {
            Debug.Log("점검시간 팝업이 우선이라 의미 불명 팝업 무시");
            return;
        }

        if (IsResultOpen)
        {
            return;
        }

        IsResultOpen = true;

        PauseGameAndTimer(gameClockTimer);

        SetPopupState(
            showSuccess: false,
            showFail: false,
            showMaintenance: false,
            showTimeout: false,
            showMeaningError: true
        );

        if (meaningErrorPopup != null)
        {
            meaningErrorPopup.SetActive(true);
        }

        Debug.Log("의미 불명 팝업 표시");
    }

    /// <summary>
    /// 모든 팝업 끄기
    /// </summary>
    public void HideAll()
    {
        successPopup.SetActive(false);

        failPopup.SetActive(false);

        maintenancePopup.SetActive(false);

        timeoutPopup.SetActive(false);

        popupRoot.SetActive(false);

        meaningErrorPopup.SetActive(false);
    }

    /// <summary>
    /// 게임과 전체 시계 타이머 정지
    /// </summary>
    private void PauseGameAndTimer(GameClockTimer gameClockTimer)
    {
        // 미니게임 정지
        Time.timeScale = 0f;

        // 전체 시계 타이머 정지
        if (gameClockTimer != null)
        {
            gameClockTimer.PauseTimer();
        }
    }

    /// <summary>
    /// 팝업 상태 변경
    /// </summary>
    private void SetPopupState(
        bool showSuccess,
        bool showFail,
        bool showMaintenance,
        bool showTimeout,
        bool showMeaningError
    )
    {

        popupRoot.SetActive(true);

        successPopup.SetActive(showSuccess);

        failPopup.SetActive(showFail);

        maintenancePopup.SetActive(showMaintenance);

        timeoutPopup.SetActive(showTimeout);

        meaningErrorPopup.SetActive(showMeaningError);

    }
}