using UnityEngine;

public class OnClickButton : MonoBehaviour
{
    [Header("화면")]
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject mainScreen;

    [Header("미니게임 생성 매니저")]
    [SerializeField] private MiniGameSpawner miniGameSpawner;

    [Header("설정 팝업")]
    [SerializeField] private GameObject settingPopup;

    [Header("오디오 매니저")]
    [SerializeField] private AudioManager audioManager;

    [Header("전체 시계 타이머")]
    [SerializeField] private GameClockTimer gameClockTimer;

    public void OnClickMain()
    {
        if (audioManager != null)
        {
            audioManager.PlaySfx(3);
        }

        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(true);
        }

        if (gameClockTimer != null)
        {
            gameClockTimer.StartTimer();
        }
        else
        {
            Debug.LogWarning("GameClockTimer가 OnClickButton에 연결되지 않았습니다.");
        }

        MainScreenUI mainScreenUI = mainScreen.GetComponent<MainScreenUI>();

        if (mainScreenUI != null)
        {
            mainScreenUI.RefreshStageButtons();
        }
    }

    public void OnClickSetting()
    {
        if (settingPopup == null)
        {
            Debug.LogWarning("Setting Popup이 연결되지 않았습니다.");
            return;
        }

        settingPopup.SetActive(true);

        if (audioManager != null)
        {
            audioManager.PlaySfx(3);
        }
    }

    public void OnClickTitle()
    {
        if (audioManager != null)
        {
            audioManager.PlaySfx(1);
        }

        if (miniGameSpawner != null)
        {
            miniGameSpawner.ShowTitleScreen();
            return;
        }

        if (titleScreen != null)
        {
            titleScreen.SetActive(true);
        }

        if (mainScreen != null)
        {
            mainScreen.SetActive(false);
        }
    }

    public void OnClickCancle()
    {
        if (audioManager != null)
        {
            audioManager.PlaySfx(2); // Fail 효과음
        }

        if (miniGameSpawner == null)
        {
#if UNITY_2023_1_OR_NEWER
            miniGameSpawner = FindFirstObjectByType<MiniGameSpawner>();
#else
        miniGameSpawner = FindObjectOfType<MiniGameSpawner>();
#endif
        }

        if (miniGameSpawner != null)
        {
            miniGameSpawner.ExternalGameOver();
            return;
        }

        Debug.LogWarning("MiniGameSpawner가 연결되지 않았습니다.");
    }

    public void OnClickExitSetting()
    {
        if (audioManager != null)
        {
            audioManager.PlaySfx(1);
        }

        if (settingPopup != null)
        {
            settingPopup.SetActive(false);
        }
    }
}