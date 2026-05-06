using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;

    [Header("효과음 5개")]
    [SerializeField] private AudioSource[] sfxSources;

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private bool bgmMuted = true;
    private bool sfxMuted = true;

    public float BgmVolume => bgmVolume;
    public float SfxVolume => sfxVolume;

    public bool BgmMuted => bgmMuted;
    public bool SfxMuted => sfxMuted;

    private void Awake()
    {
        LoadAudioSetting();

        ApplyBgmSetting();
        ApplySfxSetting();
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;

        ApplyBgmSetting();
        SaveAudioSetting();

        Debug.Log($"BGM 볼륨 설정: {bgmVolume}");
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;

        ApplySfxSetting();
        SaveAudioSetting();

        Debug.Log($"효과음 볼륨 설정: {sfxVolume}");
    }

    // Toggle이 아니라 Toggle 상태값을 그대로 받는 함수
    public void SetBgmMute(bool isMuted)
    {
        bgmMuted = isMuted;

        ApplyBgmSetting();
        SaveAudioSetting();

        Debug.Log($"BGM 음소거 설정: {bgmMuted}");
    }

    // Toggle이 아니라 Toggle 상태값을 그대로 받는 함수
    public void SetSfxMute(bool isMuted)
    {
        sfxMuted = isMuted;

        ApplySfxSetting();
        SaveAudioSetting();

        Debug.Log($"효과음 음소거 설정: {sfxMuted}");
    }

    public void ToggleBgmMute()
    {
        bgmMuted = !bgmMuted;

        ApplyBgmSetting();
        SaveAudioSetting();

        Debug.Log($"BGM 음소거 토글: {bgmMuted}");
    }

    public void ToggleSfxMute()
    {
        sfxMuted = !sfxMuted;

        ApplySfxSetting();
        SaveAudioSetting();

        Debug.Log($"효과음 음소거 토글: {sfxMuted}");
    }

    private void ApplyBgmSetting()
    {
        if (bgmSource == null)
        {
            Debug.LogWarning("BGM AudioSource가 연결되지 않았습니다.");
            return;
        }

        bgmSource.volume = bgmVolume;
        bgmSource.mute = bgmMuted;
    }

    private void ApplySfxSetting()
    {
        if (sfxSources == null)
        {
            return;
        }

        foreach (AudioSource sfx in sfxSources)
        {
            if (sfx == null)
            {
                continue;
            }

            sfx.volume = sfxVolume;
            sfx.mute = sfxMuted;
        }
    }

    public void PlaySfx(int index)
    {
        if (sfxSources == null)
        {
            return;
        }

        if (index < 0 || index >= sfxSources.Length)
        {
            Debug.LogWarning($"효과음 index 범위 초과: {index}");
            return;
        }

        AudioSource sfx = sfxSources[index];

        if (sfx == null)
        {
            Debug.LogWarning($"{index}번 효과음 AudioSource가 비어있습니다.");
            return;
        }

        // 재생 직전에 현재 설정을 다시 반영
        sfx.volume = sfxVolume;
        sfx.mute = sfxMuted;

        if (sfxMuted)
        {
            Debug.Log($"효과음 {index}번은 음소거 상태라 재생하지 않음");
            return;
        }

        sfx.Play();

        Debug.Log($"효과음 {index}번 재생 / 볼륨: {sfxVolume}");
    }

    private void SaveAudioSetting()
    {
        PlayerPrefs.SetFloat("BGM_VOLUME", bgmVolume);
        PlayerPrefs.SetFloat("SFX_VOLUME", sfxVolume);

        PlayerPrefs.SetInt("BGM_MUTED", bgmMuted ? 1 : 0);
        PlayerPrefs.SetInt("SFX_MUTED", sfxMuted ? 1 : 0);

        PlayerPrefs.Save();
    }

    private void LoadAudioSetting()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);

        bgmMuted = PlayerPrefs.GetInt("BGM_MUTED", 0) == 1;
        sfxMuted = PlayerPrefs.GetInt("SFX_MUTED", 0) == 1;
    }
}