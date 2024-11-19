using System;
using UnityEngine;

public class SettingsPresenter : MonoBehaviour
{
    private AudioSettingsSO _audioSettingsSO;
    private ScriptableObjectUser _userSO;
    void Initialize()
    {
        _audioSettingsSO = Resources.Load<AudioSettingsSO>("AudioSettings_Data");
        _userSO = Resources.Load<ScriptableObjectUser>("User Data");
    }
    private void OnEnable()
    {
        Initialize();
        
        // Listen for events from the View / UI
        AudioEvents.MasterSliderChanged += AudioEvents_MasterSliderChanged;
        AudioEvents.SFXSliderChanged += AudioEvents_SFXSliderChanged;
        AudioEvents.MusicSliderChanged += AudioEvents_MusicSliderChanged;
        
        // Listen for events from the model
        AudioEvents.ModelMasterVolumeChanged += AudioEvents_ModelMasterVolumeChanged;
        AudioEvents.ModelMusicVolumeChanged += AudioEvents_ModelMusicVolumeChanged;
        AudioEvents.ModelSFXVolumeChanged += AudioEvents_ModelSFXVolumeChanged;
        

    }
    private void OnDisable()
    {
        AudioEvents.MasterSliderChanged -= AudioEvents_MasterSliderChanged;
        AudioEvents.SFXSliderChanged -= AudioEvents_SFXSliderChanged;
        AudioEvents.MusicSliderChanged -= AudioEvents_MusicSliderChanged;
        
        AudioEvents.ModelMasterVolumeChanged -= AudioEvents_ModelMasterVolumeChanged;
        AudioEvents.ModelMusicVolumeChanged -= AudioEvents_ModelMusicVolumeChanged;
        AudioEvents.ModelSFXVolumeChanged -= AudioEvents_ModelSFXVolumeChanged;
    }

    // View event handlers
    public void AudioEvents_MasterSliderChanged(float sliderValue)
    {
        float volume = sliderValue / 100f;
        AudioEvents.MasterVolumeChanged?.Invoke(volume);
    }

    public void AudioEvents_SFXSliderChanged(float sliderValue)
    {
        float volume = sliderValue / 100f;
        // Debug.Log(volume);
        AudioEvents.SFXVolumeChanged?.Invoke(volume);
    }

    public void AudioEvents_MusicSliderChanged(float sliderValue)
    {
        float volume = sliderValue / 100f;
        AudioEvents.MusicVolumeChanged?.Invoke(volume);
    }
    
    
    // Model event handlers (response if Model data externally modified,
    // e.g. loading preferences from disk)
    private void AudioEvents_ModelMasterVolumeChanged(float volume)
    {
        // Process the master volume change from the Model
        AudioEvents.MasterSliderSet?.Invoke(volume);
    }

    private void AudioEvents_ModelSFXVolumeChanged(float volume)
    {
        // Process the SFX volume change from the Model
        AudioEvents.SFXSliderSet?.Invoke(volume);
    }

    private void AudioEvents_ModelMusicVolumeChanged(float volume)
    {
        // Process the music volume change from the Model
        AudioEvents.MusicSliderSet?.Invoke(volume);
    }
}
