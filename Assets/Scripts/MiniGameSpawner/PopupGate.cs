using UnityEngine;

public class PopupGate
{
    private readonly MiniGamePopup popup;

    public PopupGate(MiniGamePopup popup)
    {
        this.popup = popup;
    }

    public bool IsMaintenanceOpen => popup != null && popup.IsMaintenanceOpen;

    public bool IsResultOpen => popup != null && popup.IsResultOpen;

    /// <summary>
    /// 점검 또는 결과 팝업이 열려 있으면 false. 점검 상태일 때만 로그를 남긴다.
    /// </summary>
    public bool TryEnter(string maintenanceLogMessage)
    {
        if (popup == null)
        {
            return true;
        }

        if (popup.IsMaintenanceOpen)
        {
            Debug.Log(maintenanceLogMessage);
            return false;
        }

        if (popup.IsResultOpen)
        {
            return false;
        }

        return true;
    }

    public void ResetAndHide()
    {
        if (popup == null)
        {
            return;
        }

        popup.ResetState();
        popup.HideAll();
    }

    public void ShowSuccess(GameClockTimer timer) => popup?.ShowSuccess(timer);

    public void ShowFail(GameClockTimer timer) => popup?.ShowFail(timer);

    public void ShowMaintenance(GameClockTimer timer) => popup?.ShowMaintenance(timer);

    public void ShowTimeout(GameClockTimer timer) => popup?.ShowTimeout(timer);

    public void ShowMeaningError(GameClockTimer timer) => popup?.ShowMeaningError(timer);
}
