using UnityEngine;

public partial class MiniGamePopup
{
    /// <summary>
    /// 모든 팝업 끄기
    /// </summary>
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

        if (timeoutPopup != null)
        {
            timeoutPopup.SetActive(false);
        }

        if (meaningErrorPopup != null)
        {
            meaningErrorPopup.SetActive(false);
        }

        if (popupRoot != null)
        {
            popupRoot.SetActive(false);
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

        if (timeoutPopup != null)
        {
            timeoutPopup.SetActive(showTimeout);
        }

        if (meaningErrorPopup != null)
        {
            meaningErrorPopup.SetActive(showMeaningError);
        }
    }
}
