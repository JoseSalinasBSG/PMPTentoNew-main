using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioEvents
{
    // Notify when press button
    public static Action ButtonClicked;
    
    // Presenter --> View: update UI sliders
    public static Action<float> MasterSliderSet;
    public static Action<float> SFXSliderSet;
    public static Action<float> MusicSliderSet;
    
    // View -> Presenter: update UI sliders
    public static Action<float> MasterSliderChanged;
    public static Action<float> SFXSliderChanged;
    public static Action<float> MusicSliderChanged;
    
    //// Presenter -> Model: update volume settings
    public static Action<float> SFXVolumeChanged;
    public static Action<float> MusicVolumeChanged;
    public static Action<float> MasterVolumeChanged;
    
    // Model -> Presenter: model values changed (e.g. loading saved values)
    public static Action<float> ModelMasterVolumeChanged;
    public static Action<float> ModelSFXVolumeChanged;
    public static Action<float> ModelMusicVolumeChanged;
}
