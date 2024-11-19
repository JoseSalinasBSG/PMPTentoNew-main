using System;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioSetting", menuName = "AudioSettings")]
public class AudioSettingsSO : ScriptableObject
{
    
    private const float DEFAULT_MASTER_VOLUME = 1f;
    private const float DEFAULT_SFX_VOLUME = 1f;
    private const float DEFAULT_MUSIC_VOLUME = 1f;

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private float _masterVolume = DEFAULT_MASTER_VOLUME;
    [SerializeField] private float _soundEffectsVolume = DEFAULT_SFX_VOLUME;
    [SerializeField] private float _musicVolume = DEFAULT_MUSIC_VOLUME;

    [SerializeField] private bool _isMasterMuted;
    [SerializeField] private bool _isSoundEffectsMuted;
    [SerializeField] private bool _isMusicMuted;

    [SerializeField] private AudioClip _successSound;
    [SerializeField] private AudioClip _failSound;
    [SerializeField] private AudioClip _tapTouchSound;
    [Header("Music")]
    [SerializeField] private AudioClip _mainSound;
    [SerializeField] private AudioClip _categoryModeSound;
    [SerializeField] private AudioClip _learningModeSound;
    [SerializeField] private AudioClip _videoQuestionModeSound;
    [SerializeField] private AudioClip _survivalChallengeSound;
    [SerializeField] private AudioClip _trainingChallengeSound;
    [SerializeField] private AudioClip _glossaryChallengeSound;
    [SerializeField] private AudioClip _gameWonSound;
    [SerializeField] private AudioClip _gameLostSound;
    
    
    [Header("Navbar")] [SerializeField] private AudioClip _navbarTapTouchSound;
    [Header("Power-up")] [SerializeField] private AudioClip _bombSmokeSound;
    [SerializeField] private AudioClip _bombExplosionSound;

    public AudioMixer AudioMixer => _audioMixer;
    public AudioClip SuccessSound => _successSound;
    public AudioClip FailSound => _failSound;
    public AudioClip GameWonSound => _gameWonSound;
    public AudioClip GameLostSound => _gameLostSound;
    public AudioClip TapTouchSound => _tapTouchSound;

    public AudioClip BombExplosionSound => _bombExplosionSound;

    public AudioClip BombSmokeSound => _bombSmokeSound;

    public AudioClip MainSound => _mainSound;

    public AudioClip CategoryModeSound => _categoryModeSound;

    public AudioClip LearningModeSound => _learningModeSound;

    public AudioClip VideoQuestionModeSound => _videoQuestionModeSound;

    public AudioClip SurvivalChallengeSound => _survivalChallengeSound;

    public AudioClip TrainingChallengeSound => _trainingChallengeSound;

    public AudioClip GlossaryChallengeSound => _glossaryChallengeSound;

    public float MasterVolume
    {
        get => _masterVolume;
        set => _masterVolume = value;
    }

    public float SoundEffectsVolume
    {
        get => _soundEffectsVolume;
        set => _soundEffectsVolume = value;
    }

    public float MusicVolume
    {
        get => _musicVolume;
        set => _musicVolume = value;
    }

    public bool IsMasterMuted
    {
        get => _isMasterMuted;
        set => _isMasterMuted = value;
    }

    public bool IsSoundEffectsMuted
    {
        get => _isSoundEffectsMuted;
        set => _isSoundEffectsMuted = value;
    }

    public bool IsMusicMuted
    {
        get => _isMusicMuted;
        set => _isMusicMuted = value;
    }

    private void OnEnable()
    {
        Debug.Log("on enable in audiosettingsSO");
        AudioEvents.MasterVolumeChanged += AudioEvents_MasterVolumeChanged;
        AudioEvents.MusicVolumeChanged += AudioEvents_MusicVolumeChanged;
        AudioEvents.SFXVolumeChanged += AudioEvents_SFXVolumeChanged;
    }

    private void OnDisable()
    {
        Debug.Log("on disable in audiosettingsSO");
        AudioEvents.MasterVolumeChanged -= AudioEvents_MasterVolumeChanged;
        AudioEvents.MusicVolumeChanged -= AudioEvents_MusicVolumeChanged;
        AudioEvents.SFXVolumeChanged -= AudioEvents_SFXVolumeChanged;
    }

    private void AudioEvents_SFXVolumeChanged(float obj)
    {
        SoundEffectsVolume = obj;
        PlayerPrefs.SetFloat("SounEffectVolume", SoundEffectsVolume);
        PlayerPrefs.Save();

    }

    private void AudioEvents_MusicVolumeChanged(float obj)
    {
        MusicVolume = obj;
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.Save();

    }

    private void AudioEvents_MasterVolumeChanged(float obj)
    {
        MasterVolume = obj;
    }
    private void SaveSettingInformation()
    {
        PlayerPrefs.Save();
    }
}
