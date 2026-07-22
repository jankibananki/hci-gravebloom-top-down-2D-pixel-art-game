using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;

    private const string MasterVolumeParameter = "MasterVolume";
    private const string MasterVolumePreference = "MasterVolumePreference";

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(
            MasterVolumePreference,
            0.8f
        );

        masterVolumeSlider.SetValueWithoutNotify(savedVolume);
        SetMasterVolume(savedVolume);

        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SetMasterVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);

        float decibels = Mathf.Log10(value) * 20f;

        audioMixer.SetFloat(MasterVolumeParameter, decibels);
        PlayerPrefs.SetFloat(MasterVolumePreference, value);
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
    }
}