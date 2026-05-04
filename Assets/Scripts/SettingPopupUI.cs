using UnityEngine;
using UnityEngine.UI;

public class SettingPopupUI : MonoBehaviour
{
    [Header("오디오 매니저")]
    [SerializeField] private AudioManager audioManager;

    [Header("BGM")]
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Button bgmMuteButton;

    [Header("효과음")]
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button sfxMuteButton;

    private void OnEnable()
    {
        audioManager.PlaySfx(3);
        RefreshUI();

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);

        if (bgmMuteButton != null)
            bgmMuteButton.onClick.AddListener(OnClickBgmMute);

        if (sfxMuteButton != null)
            sfxMuteButton.onClick.AddListener(OnClickSfxMute);
    }

    private void OnDisable()
    {
        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.RemoveListener(OnBgmVolumeChanged);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);

        if (bgmMuteButton != null)
            bgmMuteButton.onClick.RemoveListener(OnClickBgmMute);

        if (sfxMuteButton != null)
            sfxMuteButton.onClick.RemoveListener(OnClickSfxMute);
    }

    private void RefreshUI()
    {
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager가 연결되지 않았습니다.");
            return;
        }

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.SetValueWithoutNotify(audioManager.BgmVolume);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.SetValueWithoutNotify(audioManager.SfxVolume);
    }

    private void OnBgmVolumeChanged(float value)
    {
        Debug.Log($"BGM 슬라이더 변경: {value}");

        if (audioManager != null)
            audioManager.SetBgmVolume(value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        Debug.Log($"효과음 슬라이더 변경: {value}");

        if (audioManager != null)
            audioManager.SetSfxVolume(value);
    }

    private void OnClickBgmMute()
    {
        Debug.Log("BGM 음소거 버튼 클릭");

        if (audioManager != null)
            audioManager.ToggleBgmMute();
    }

    private void OnClickSfxMute()
    {
        Debug.Log("효과음 음소거 버튼 클릭");

        if (audioManager != null)
            audioManager.ToggleSfxMute();
    }
}