using System.Collections;
using UnityEngine;

public partial class MiniGameSpawner
{
    private void ShowOTP()
    {
        DestroyCurrentMiniGame();

        gate.ResetAndHide();

        Time.timeScale = 1f;

        if (otpPrefab != null)
        {
            otpInstance = Instantiate(otpPrefab, miniGameParent, false);
            otpInstance.StartMiniGame();
        }
        else
        {
            Debug.LogWarning("OTP Prefab이 연결되지 않았습니다.");
        }

        MiniGameManager.OnMiniGameSuccess -= OnOTPSuccess;
        MiniGameManager.OnMiniGameSuccess += OnOTPSuccess;

        MiniGameManager.OnMiniGameFail -= OnOTPFail;
        MiniGameManager.OnMiniGameFail += OnOTPFail;

        Debug.Log("모든 스테이지 클리어 - OTP 미니게임 시작");
    }

    private void OnOTPSuccess()
    {
        UnsubscribeOTPEvents();

        DestroyOTPInstance();

        StartCoroutine(OTPSuccessSequence());
    }

    private void OnOTPFail()
    {
        UnsubscribeOTPEvents();

        DestroyOTPInstance();

        gate.ShowFail(gameClockTimer);

        Debug.Log("OTP 실패 - 실패 팝업 표시");
    }

    private IEnumerator OTPSuccessSequence()
    {
        GameObject successPopup = null;

        if (otpSuccessPopupPrefab != null)
        {
            successPopup = Instantiate(otpSuccessPopupPrefab, miniGameParent, false);
        }

        Time.timeScale = 0f;

        float timeRatio = gameClockTimer != null ? gameClockTimer.RemainingRatio : 1f;

        if (gameClockTimer != null)
        {
            gameClockTimer.PauseTimer();
        }

        yield return WaitOTPSuccess;

        if (successPopup != null)
        {
            Destroy(successPopup);
        }

        Time.timeScale = 1f;

        if (transferPopupPrefab != null)
        {
            TransferPopup transferPopup = Instantiate(transferPopupPrefab, miniGameParent, false);

            // 확인 버튼을 누르면 ShowTitleScreen() 실행
            transferPopup.Show(ShowTitleScreen, timeRatio);
        }
        else
        {
            Debug.LogWarning("TransferPopup Prefab이 연결되지 않았습니다.");
        }

        Debug.Log("OTP 성공 - 송금 팝업 표시");
    }

    private void UnsubscribeOTPEvents()
    {
        MiniGameManager.OnMiniGameSuccess -= OnOTPSuccess;
        MiniGameManager.OnMiniGameFail -= OnOTPFail;
    }
}
