using UnityEngine;

public partial class MiniGameSpawner
{
    public void ConfirmSuccess()
    {
        Time.timeScale = 1f;

        popupController.ResetState();

        DestroyCurrentMiniGame();

        popupController.HideAll();

        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(true);
        }

        if (mainScreenUI != null)
        {
            mainScreenUI.RefreshStageButtons();
        }

        if (gameClockTimer != null)
        {
            gameClockTimer.ResumeTimer();
        }

        Debug.Log("성공 확인 - 메인화면으로 이동");
    }

    public void ConfirmFail()
    {
        ShowTitleScreen();
    }

    public void EndByMaintenance()
    {
        ShowTitleScreen();
    }

    public void ShowTitleScreen()
    {
        Time.timeScale = 1f;

        StopAllCoroutines();

        UnsubscribeOTPEvents();

        popupController.ResetState();
        popupController.HideAll();

        if (gameClockTimer != null)
        {
            gameClockTimer.ResetTimer();
        }

        if (stageClearManager != null)
        {
            stageClearManager.ResetAll();
        }

        currentStageNumber = 0;

        DestroyCurrentMiniGame();
        DestroyOTPInstance();

        if (miniGamePool != null)
        {
            miniGamePool.ResetAllPools();
        }

        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }

        if (mainScreenUI != null)
        {
            mainScreenUI.RefreshStageButtons();
        }

        Debug.Log("타이틀 화면 이동 - 전체 상태 초기화");
    }

    public void HideResultObjects()
    {
        popupController.ResetState();
        popupController.HideAll();
    }
}