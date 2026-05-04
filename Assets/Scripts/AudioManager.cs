using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;

    [Header("효과음 5개")]
    [SerializeField] private AudioSource[] sfxSources;

    private float bgmVolume = 1f;
    private float sfxVolume = 1f;

    private bool bgmMuted = false;
    private bool sfxMuted = false;

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

        Debug.Log($"BGM 볼륨: {bgmVolume}");
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        ApplySfxSetting();
        SaveAudioSetting();

        Debug.Log($"효과음 볼륨: {sfxVolume}");
    }

    public void ToggleBgmMute()
    {
        bgmMuted = !bgmMuted;
        ApplyBgmSetting();
        SaveAudioSetting();

        Debug.Log($"BGM 음소거: {bgmMuted}");
    }

    public void ToggleSfxMute()
    {
        sfxMuted = !sfxMuted;
        ApplySfxSetting();
        SaveAudioSetting();

        Debug.Log($"효과음 음소거: {sfxMuted}");
    }

    private void ApplyBgmSetting()
    {
        if (bgmSource == null)
            return;

        bgmSource.volume = bgmVolume;
        bgmSource.mute = bgmMuted;
    }

    private void ApplySfxSetting()
    {
        if (sfxSources == null)
            return;

        foreach (AudioSource sfx in sfxSources)
        {
            if (sfx == null)
                continue;

            sfx.volume = sfxVolume;
            sfx.mute = sfxMuted;
        }
    }

    public void PlaySfx(int index)
    {
        if (sfxSources == null)
            return;

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

        sfx.volume = sfxVolume;
        sfx.mute = sfxMuted;

        if (sfxMuted)
        {
            Debug.Log($"효과음 {index}번 음소거 상태라 재생 안 함");
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