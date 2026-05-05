using UnityEngine;

/// <summary>
/// 성공 / 실패 / 점검 팝업 관리
/// </summary>

[System.Serializable]
public class MiniGamePopup
{
    [Header("팝업 루트")]
    [SerializeField] private GameObject popupRoot;

    [Header("결과 팝업")]
    [SerializeField] private GameObject successPopup;
    [SerializeField] private GameObject failPopup;
    [SerializeField] private GameObject maintenancePopup;

    public bool IsResultOpen { get; private set; }
    public bool IsMaintenanceOpen { get; private set; }

    public void ResetState()
    {
        IsResultOpen = false;
        IsMaintenanceOpen = false;
    }

    public void ShowSuccess(GameClockTimer gameClockTimer)
    {
        if (IsMaintenanceOpen)
        {
            return;
        }

        IsResultOpen = true;

        // 성공 팝업 중에는 게임과 전체 타이머 정지
        Time.timeScale = 0f;

        if (gameClockTimer != null)
        {
            gameClockTimer.PauseTimer();
        }

        SetPopupState(
            showSuccess: true,
            showFail: false,
            showMaintenance: false
        );

        Debug.Log("성공 팝업 표시 - 전체 타이머 정지");
    }

    public void ShowFail(GameClockTimer gameClockTimer)
    {
        if (IsMaintenanceOpen)
        {
            return;
        }

        IsResultOpen = true;

        // 실패 팝업 중에는 게임과 전체 타이머 정지
        Time.timeScale = 0f;

        if (gameClockTimer != null)
        {
            gameClockTimer.PauseTimer();
        }

        SetPopupState(
            showSuccess: false,
            showFail: true,
            showMaintenance: false
        );

        Debug.Log("실패 팝업 표시 - 전체 타이머 정지");
    }

    public void ShowMaintenance(GameClockTimer gameClockTimer)
    {
        // 점검시간 팝업은 성공/실패보다 최우선
        if (IsMaintenanceOpen)
        {
            return;
        }

        IsMaintenanceOpen = true;
        IsResultOpen = true;

        Time.timeScale = 0f;

        if (gameClockTimer != null)
        {
            gameClockTimer.PauseTimer();
        }

        SetPopupState(
            showSuccess: false,
            showFail: false,
            showMaintenance: true
        );

        Debug.Log("점검시간 팝업 표시 - 최우선 처리");
    }

    public void HideAll()
    {
        if (successPopup != null)
        {
            successPopup.SetActive(false);
        }

        if (failPopup != null)
        {
            failPopup.SetActive(false);
        }

        if (maintenancePopup != null)
        {
            maintenancePopup.SetActive(false);
        }

        if (popupRoot != null)
        {
            popupRoot.SetActive(false);
        }
    }

    private void SetPopupState(bool showSuccess, bool showFail, bool showMaintenance)
    {
        if (popupRoot != null)
        {
            popupRoot.SetActive(true);
        }

        if (successPopup != null)
        {
            successPopup.SetActive(showSuccess);
        }

        if (failPopup != null)
        {
            failPopup.SetActive(showFail);
        }

        if (maintenancePopup != null)
        {
            maintenancePopup.SetActive(showMaintenance);
        }
    }
}