using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Popups")] [SerializeField] private FadeUI _gameModesPopup;
    [SerializeField] private FadeUI _gameChallengesPopup;
    [SerializeField] private FadeUI _gameConfigurationPopup;
    [SerializeField] private FadeUI _chooseInstructorPopup;
    [SerializeField] private FadeUI _notificationPopup;
    
    
    // [Header("Views")]
    // [SerializeField]
    private void OnEnable()
    {
        UIEvents.SettingShow += UIEvent_SettingShow;
        UIEvents.GameModesShow += UIEvent_GameModesShow;
        UIEvents.GameChallengesShow += UIEvent_GameChallengesShow;
        UIEvents.ChooseInstructorShow += UIEvent_ChooseInstructorShow;
        UIEvents.NotificationShow += UIEvent_NotificationShow;
        
        UIEvents.SettingHide += UIEvent_SettingHide;
        UIEvents.GameModesHide += UIEvent_GameModesHide;
        UIEvents.GameChallengesHide += UIEvent_GameChallengesHide;
        UIEvents.ChooseInstructorHide += UIEvent_ChooseInstructorHide;
        UIEvents.NotificationHide += UIEvent_NotificationHide;
    }

    private void OnDisable()
    {
        UIEvents.SettingShow -= UIEvent_SettingShow;
        UIEvents.GameModesShow -= UIEvent_GameModesShow;
        UIEvents.GameChallengesShow -= UIEvent_GameChallengesShow;
        UIEvents.ChooseInstructorShow -= UIEvent_ChooseInstructorShow;
        UIEvents.NotificationShow -= UIEvent_NotificationShow;
        
        UIEvents.SettingHide -= UIEvent_SettingHide;
        UIEvents.GameModesHide -= UIEvent_GameModesHide;
        UIEvents.GameChallengesHide -= UIEvent_GameChallengesHide;
        UIEvents.ChooseInstructorHide -= UIEvent_ChooseInstructorHide;
        UIEvents.NotificationHide -= UIEvent_NotificationHide;
    }

    private void UIEvent_SettingShow()//metodo para mostrar configuraciones
    {
        _gameConfigurationPopup.gameObject.SetActive(true);
        _gameConfigurationPopup.FadeInTransition();
        var _audioSettingsSO = Resources.Load<AudioSettingsSO>("AudioSettings_Data");//carga datos de Resources
     

        // Update values to Sliders 
        float masterVolume = _audioSettingsSO.MasterVolume * 100f;
        float sfxVolume = _audioSettingsSO.SoundEffectsVolume * 100f;
        float musicVolume = _audioSettingsSO.MusicVolume * 100f;
        Debug.Log(musicVolume);
        Debug.Log(sfxVolume);
        //Notify the view of default values from the model
        AudioEvents.MasterSliderSet?.Invoke(masterVolume);
        AudioEvents.SFXSliderSet?.Invoke(sfxVolume);
        AudioEvents.MusicSliderSet?.Invoke(musicVolume);
    }
    private void UIEvent_GameModesShow()
    {
        _gameModesPopup.gameObject.SetActive(true);
        _gameModesPopup.FadeInTransition();
    }
    private void UIEvent_GameChallengesShow()
    {
        _gameChallengesPopup.gameObject.SetActive(true);
        _gameChallengesPopup.FadeInTransition();
    }
    private void UIEvent_ChooseInstructorShow()
    {
        _chooseInstructorPopup.gameObject.SetActive(true);
        _chooseInstructorPopup.FadeInTransition();
    }
    private void UIEvent_NotificationShow()
    {
        _notificationPopup.gameObject.SetActive(true);
        _notificationPopup.FadeInTransition();
    }
    
    
    private void UIEvent_SettingHide()
    {
        _gameConfigurationPopup.FadeOutTransition();
        
    }
    private void UIEvent_GameModesHide()
    {
        _gameModesPopup.FadeOutTransition();
    }
    private void UIEvent_GameChallengesHide()
    {
        _gameChallengesPopup.FadeOutTransition();
    }
    private void UIEvent_ChooseInstructorHide()
    {
        _chooseInstructorPopup.FadeOutTransition();
    }
    private void UIEvent_NotificationHide()
    {
        _notificationPopup.FadeOutTransition();
    }
    public void OpenConfigurationPopup()//abrir menu de configuracion, se ejecuta el metodo al presionar en icono config
    {
        UIEvents.SettingShow?.Invoke();//invocar evento de mostrar menu de configuracion
    }
    public void CloseConfigurationPopup()//cerrar menu de configuracion, se ejecuta el metodo al presionar en icono config
    {
        UIEvents.SettingHide?.Invoke();
     
    }
    public void OpenGameModesPopup()
    {
        UIEvents.GameModesShow?.Invoke();
    }
    public void CloseGameModesPopup()
    {
        UIEvents.GameModesHide?.Invoke();
    }
    public void OpenGameChanllengesPopup()
    {
        UIEvents.GameChallengesShow?.Invoke();
    }
    public void CloseGameChanllengesPopup()
    {
        UIEvents.GameChallengesHide?.Invoke();
    }
    public void OpenChooseInstructorPopup()
    {
        UIEvents.ChooseInstructorShow?.Invoke();
    }
    public void CloseChooseInstructorPopup()
    {
        UIEvents.ChooseInstructorHide?.Invoke();
    }
    public void OpenNotificationPopup()
    {
        UIEvents.NotificationShow?.Invoke();
    }
    public void CloseNotificationPopup()
    {
        UIEvents.NotificationHide?.Invoke();
    }
}
