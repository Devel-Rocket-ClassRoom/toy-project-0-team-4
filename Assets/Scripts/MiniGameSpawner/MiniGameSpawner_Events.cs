using UnityEngine;

public partial class MiniGameSpawner
{
    private void HandleStageClear(int stageNumber)
    {
        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 클리어 이벤트 무시");
            return;
        }

        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log($"{stageNumber} 스테이지 클리어 이벤트 받음");

        if (stageClearManager != null)
        {
            stageClearManager.ClearStage(stageNumber);

            // 모든 스테이지 클리어 시 OTP 미니게임 표시
            if (stageClearManager.AllCleared(totalStages))
            {
                ShowOTP();
                return;
            }
        }

        popupController.ShowSuccess(gameClockTimer);
    }

    private void HandleGameOver()
    {
        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 실패 이벤트 무시");
            return;
        }

        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log("미니게임 실패 이벤트 받음");

        popupController.ShowFail(gameClockTimer);
    }

    public void ExternalGameOver()
    {
        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 외부 게임오버 무시");
            return;
        }

        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log("외부 스크립트에서 게임오버 호출됨");

        popupController.ShowFail(gameClockTimer);
    }

    public void ForceGameOver()
    {
        Debug.Log("전체 타이머 종료 - 점검시간 팝업 요청");

        popupController.ShowMaintenance(gameClockTimer);
    }

    public void ShowTimeoutPopup()
    {
        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 장시간 응답 없음 팝업 무시");
            return;
        }

        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log("장시간 응답 없음 팝업 요청");

        popupController.ShowTimeout(gameClockTimer);
    }

    public void ShowMeaningErrorPopup()
    {
        if (popupController.IsMaintenanceOpen)
        {
            Debug.Log("점검시간 상태라 의미 불명 팝업 무시");
            return;
        }

        if (popupController.IsResultOpen)
        {
            return;
        }

        Debug.Log("의미 불명 팝업 요청");

        popupController.ShowMeaningError(gameClockTimer);
    }
}
