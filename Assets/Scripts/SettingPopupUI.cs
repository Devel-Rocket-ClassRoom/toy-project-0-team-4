using UnityEngine;
using UnityEngine.UI;

public class SettingPopupUI : MonoBehaviour
{
    [Header("오디오 매니저")]
    [SerializeField] private AudioManager audioManager;

    [Header("BGM")]
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Toggle bgmMuteToggle;

    [Header("효과음")]
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle sfxMuteToggle;

    private void OnEnable()
    {
        audioManager.PlaySfx(3);
        RefreshUI();

        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);

        if (bgmMuteToggle != null)
            bgmMuteToggle.onValueChanged.AddListener(OnBgmMuteChanged);

        if (sfxMuteToggle != null)
            sfxMuteToggle.onValueChanged.AddListener(OnSfxMuteChanged);
    }

    private void OnDisable()
    {
        if (bgmVolumeSlider != null)
            bgmVolumeSlider.onValueChanged.RemoveListener(OnBgmVolumeChanged);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);

        if (bgmMuteToggle != null)
            bgmMuteToggle.onValueChanged.RemoveListener(OnBgmMuteChanged);

        if (sfxMuteToggle != null)
            sfxMuteToggle.onValueChanged.RemoveListener(OnSfxMuteChanged);
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

    private void OnBgmMuteChanged(bool isMuted)
    {
        Debug.Log($"BGM 음소거 상태 변경: {isMuted}");

        if (audioManager != null)
            audioManager.ToggleBgmMute();
    }

    private void OnSfxMuteChanged(bool isMuted)
    {
        Debug.Log($"효과음 음소거 상태 변경: {isMuted}");

        if (audioManager != null)
            audioManager.ToggleSfxMute();
    }
}