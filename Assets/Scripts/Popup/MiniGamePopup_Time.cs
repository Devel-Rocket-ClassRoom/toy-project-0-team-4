using UnityEngine;

public partial class MiniGamePopup
{
    /// <summary>
    /// 미니게임과 전체 시계 타이머 정지
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
}