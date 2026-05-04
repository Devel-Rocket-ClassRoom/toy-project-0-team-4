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

    public void OnClickMain()
    {
        audioManager.PlaySfx(3); // Open 효과음
        titleScreen.SetActive(true);
        mainScreen.SetActive(true);

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
            audioManager.PlaySfx(3); // Open 효과음
        }
        else
        {
            Debug.LogWarning("AudioManager가 연결되지 않았습니다.");
        }
    }

    public void OnClickTitle()
    {
        audioManager.PlaySfx(1); // Close 효과음
        if (miniGameSpawner != null)
        {
            miniGameSpawner.ShowTitleScreen();
            return;
        }

        titleScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    public void OnClickExitSetting()
    {
        audioManager.PlaySfx(1); // Close 효과음
        settingPopup.SetActive(false);
    }

}