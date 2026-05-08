using UnityEngine;

public partial class MiniGamePopup
{
    /// <summary>
    /// 성공 팝업 표시
    /// </summary>
    public void ShowSuccess(GameClockTimer gameClockTimer)
    {
        if (IsMaintenanceOpen)
            return;

        if (IsResultOpen)
            return;

        IsResultOpen = true;

        PauseGameAndTimer(gameClockTimer);

        SetPopupState(
            showSuccess: true,
            showFail: false,
            showMaintenance: false,
            showTimeout: false,
            showMeaningError: false
        );
    }

    /// <summary>
    /// 실패 팝업 표시
    /// </summary>
    public void ShowFail(GameClockTimer gameClockTimer)
    {
        if (IsMaintenanceOpen)
            return;

        if (IsResultOpen)
            return;

        IsResultOpen = true;

        PauseGameAndTimer(gameClockTimer);

        SetPopupState(
            showSuccess: false,
            showFail: true,
            showMaintenance: false,
            showTimeout: false,
            showMeaningError: false
        );
    }

    /// <summary>
    /// 점검시간 팝업 표시
    /// 전체 타이머 종료 시 최우선으로 표시됨
    /// </summary>
    public void ShowMaintenance(GameClockTimer gameClockTimer)
    {
        if (IsMaintenanceOpen)
            return;

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
    }

    /// <summary>
    /// 장시간 응답 없음 팝업 표시
    /// </summary>
    public void ShowTimeout(GameClockTimer gameClockTimer)
    {
        if (IsMaintenanceOpen)
            return;

        if (IsResultOpen)
            return;

        IsResultOpen = true;

        PauseGameAndTimer(gameClockTimer);

        SetPopupState(
            showSuccess: false,
            showFail: false,
            showMaintenance: false,
            showTimeout: true,
            showMeaningError: false
        );
    }

    /// <summary>
    /// 의미 불명 팝업 표시
    /// </summary>
    public void ShowMeaningError(GameClockTimer gameClockTimer)
    {
        if (IsMaintenanceOpen)
            return;

        if (IsResultOpen)
            return;

        IsResultOpen = true;

        PauseGameAndTimer(gameClockTimer);

        SetPopupState(
            showSuccess: false,
            showFail: false,
            showMaintenance: false,
            showTimeout: false,
            showMeaningError: true
        );
    }
}