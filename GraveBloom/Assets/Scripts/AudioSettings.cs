using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    private const string MasterVolumeParameter = "MasterVolume";
    private const string MusicVolumeParameter = "MusicVolume";
    private const string MasterVolumePreference = "MasterVolumePreference";
    private const string MusicVolumePreference = "MusicVolumePreference";

    private void Start()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat(
            MasterVolumePreference,
            0.8f
        );

        float savedMusicVolume = PlayerPrefs.GetFloat(
            MusicVolumePreference,
            0.8f
        );

        masterVolumeSlider.SetValueWithoutNotify(savedMasterVolume);
        musicVolumeSlider.SetValueWithoutNotify(savedMusicVolume);

        ApplyVolume(MasterVolumeParameter,savedMasterVolume);
        ApplyVolume(MusicVolumeParameter,savedMusicVolume);

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMasterVolume(float value)
    {
        ApplyVolume(MasterVolumeParameter,value);
        PlayerPrefs.SetFloat(MasterVolumeParameter,value);
    }

    public void SetMusicVolume (float value)
    {
        ApplyVolume(MusicVolumeParameter,value);
        PlayerPrefs.SetFloat(MusicVolumeParameter,value);
    }

    private void ApplyVolume(string parameterName, float value)
    {
        value = Mathf.Clamp(value,0.0001f, 1f);

        float decibels = Mathf.Log10(value)*20f;

        bool parameterFound = audioMixer.SetFloat(
            parameterName,
            decibels
        );

        if (!parameterFound)
        {
            Debug.LogWarning(
                $"Audio Mixer parameter '{parameterName} nije pronadjen!"
            );
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
        }
    }
}