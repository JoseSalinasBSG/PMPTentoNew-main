using UnityEngine;
using UnityEngine.UI;

public class AudioSettingView : MonoBehaviour
{
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _soundEffectVolumeSlider;

    private void OnEnable()
    {
        var res = Resources.Load<AudioSettingsSO>("AudioSettings_Data");
        _musicVolumeSlider.value = res.MusicVolume;
        _soundEffectVolumeSlider.value = res.SoundEffectsVolume;
        AudioEvents.MusicSliderSet += MusicVolumeSetHandler;
        AudioEvents.SFXSliderSet += SFXVolumeSetHandler;
        _musicVolumeSlider.onValueChanged.AddListener(MusicVolumeChangeHandler);
        _soundEffectVolumeSlider.onValueChanged.AddListener(SoundEffectVolumeChangeHandler);
    }

    private void OnDisable()
    {
        AudioEvents.MusicSliderSet -= MusicVolumeSetHandler;
        AudioEvents.SFXSliderSet -= SFXVolumeSetHandler;
        _musicVolumeSlider.onValueChanged.RemoveListener(MusicVolumeChangeHandler);
        _soundEffectVolumeSlider.onValueChanged.RemoveListener(SoundEffectVolumeChangeHandler);
    }

    private void SFXVolumeSetHandler(float value)
    {
        _soundEffectVolumeSlider.value = value;
    }

    private void MusicVolumeSetHandler(float value)
    {
        _musicVolumeSlider.value = value;
    }

    private void SoundEffectVolumeChangeHandler(float newValue)
    {
        AudioEvents.SFXSliderChanged?.Invoke(newValue);
    }

    private void MusicVolumeChangeHandler(float newValue)
    {
        AudioEvents.MusicSliderChanged?.Invoke(newValue);
    }
}
