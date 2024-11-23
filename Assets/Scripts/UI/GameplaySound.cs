using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CandyCoded.HapticFeedback;


public class GameplaySound : MonoBehaviour
{
    [SerializeField] AudioManager _audioManager;

    private void OnEnable()
    {
        if (!_audioManager)
        {
            _audioManager = GetComponent<AudioManager>();
        }
        UIEvents.PressLoginButton += UIEvents_PressLoginButton;
        UIEvents.PressVibrateButton += UIEvents_PressToVibrate;
        GameEvents.CorrectlyAnswered += GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered += GameEvents_IncorrectlyAnswered;
        GameEvents.GameLost += GameEvents_GameLost;
        GameEvents.GameWon += GameEvents_GameWon;
        UIEvents.ActivateSmoke += UIEvents_ActivateSmoke;
        UIEvents.ActivateExplosion += UIEvents_ActivateExplosion;
    } 


    private void OnDisable()
    {
        UIEvents.PressLoginButton -= UIEvents_PressLoginButton;
        UIEvents.PressVibrateButton -= UIEvents_PressToVibrate;
        GameEvents.CorrectlyAnswered -= GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered -= GameEvents_IncorrectlyAnswered;
        GameEvents.GameLost -= GameEvents_GameLost;
        GameEvents.GameWon -= GameEvents_GameWon;
    }

    private void UIEvents_PressLoginButton()
    {
        PlayPressedButtonSound();//reproducir sonido
    }

    private void UIEvents_PressToVibrate(int vibrateIndicator)
    {
        switch (vibrateIndicator)
        {
            case 0:
                return;                
            case 1:
                LightVibration();//vibrar
                break;
            case 2:
                MediumVibration();
                break;
            case 3:
                HeavyVibration();
                break;

            default:
                break;
        }

    }

    private void PlayPressedButtonSound()
    {
        _audioManager.PlaySFXAtPoint(_audioManager.AudioSettings.TapTouchSound, Vector3.zero, 0f, false);//reproduce sonido 
    }

    private void LightVibration()
    {
        //Debug.Log("Light Vibration performed");
        //HapticFeedback.LightFeedback();
    }
    private void MediumVibration()
    {
        //Debug.Log("Medium Vibration performed");
        //HapticFeedback.MediumFeedback();
    }
    private void HeavyVibration()
    {
        //Debug.Log("Heavy Vibration performed");
        //HapticFeedback.HeavyFeedback();
    }
    private void PlayCorrectAnswerSound()
    {
        _audioManager.PlaySFXAtPoint(_audioManager.AudioSettings.SuccessSound, Vector3.zero, 0f, false);
    }
    private void PlayIncorrectAnswerSound()
    {
        _audioManager.PlaySFXAtPoint(_audioManager.AudioSettings.FailSound, Vector3.zero, 0f, false);
    }
    private void PlayBombSmokeSound()
    {
        _audioManager.PlaySFXAtPoint(_audioManager.AudioSettings.BombSmokeSound, Vector3.zero, 0f, true);
    }
    private void PlayBombExplosionSound()
    {
        _audioManager.PlaySFXAtPoint(_audioManager.AudioSettings.BombExplosionSound, Vector3.zero, 0f, false);
    }
    private void PlayGameWonSound()
    {
        _audioManager.actualSound = AudioManager.ActualSound.gameWon;
        _audioManager.PlayMusic(_audioManager.AudioSettings.GameWonSound, Vector3.zero, 0f, false);
    }
    private void PlayLostSound()
    {
        _audioManager.actualSound = AudioManager.ActualSound.gameLost;
        _audioManager.PlayMusic(_audioManager.AudioSettings.GameLostSound, Vector3.zero, 0f, false);
    }

    public void PlayCategoryModeSound()
    {
        if (_audioManager.actualSound == AudioManager.ActualSound.categoryMode)
        {
            return;
        }
        _audioManager.actualSound = AudioManager.ActualSound.categoryMode;
        _audioManager.PlayMusic(_audioManager.AudioSettings.CategoryModeSound, Vector3.zero, 0, true);
    }

    public void PlayLearningModeSound()
    {
        if (_audioManager.actualSound == AudioManager.ActualSound.learningMode)
        {
            return;
        }
        _audioManager.actualSound = AudioManager.ActualSound.learningMode;
        _audioManager.PlayMusic(_audioManager.AudioSettings.LearningModeSound, Vector3.zero,0, true);
    }

    public void PlayVideoQuestionModeSound()
    {
        if (_audioManager.actualSound == AudioManager.ActualSound.videoQuestionMode)
        {
            return;
        }
        _audioManager.actualSound = AudioManager.ActualSound.videoQuestionMode;
        _audioManager.PlayMusic(_audioManager.AudioSettings.VideoQuestionModeSound, Vector3.zero, 0, true);
    }

    public void PlayMainMenuSound()
    {
        if (_audioManager.actualSound == AudioManager.ActualSound.main)
        {
            return;
        }
        Debug.Log("playmainmenu");
        _audioManager.actualSound = AudioManager.ActualSound.main;
        _audioManager.PlayMusic(_audioManager.AudioSettings.MainSound, Vector3.zero, 0, true);
    }

    public void PlaySurvivalChallengeSound()
    {
        if (_audioManager.actualSound == AudioManager.ActualSound.survivalChallenge)
        {
            return;
        }
        _audioManager.actualSound = AudioManager.ActualSound.survivalChallenge;
        _audioManager.PlayMusic(_audioManager.AudioSettings.SurvivalChallengeSound, Vector3.zero, 0, true);
    }

    public void PlayTrainingChallengeSound()
    {
        if (_audioManager.actualSound == AudioManager.ActualSound.trainingChallenge)
        {
            return;
        }
        _audioManager.actualSound = AudioManager.ActualSound.trainingChallenge;
        _audioManager.PlayMusic(_audioManager.AudioSettings.TrainingChallengeSound, Vector3.zero, 0, true);
    }

    public void PlayGlossaryChallengeSound()
    {
        if (_audioManager.actualSound == AudioManager.ActualSound.glossaryChallenge)
        {
            return;
        }
        _audioManager.actualSound = AudioManager.ActualSound.glossaryChallenge;
        _audioManager.PlayMusic(_audioManager.AudioSettings.GlossaryChallengeSound, Vector3.zero, 0, true);
    }
    private void GameEvents_IncorrectlyAnswered()
    {
        PlayIncorrectAnswerSound();
    }

    private void GameEvents_CorrectlyAnswered()
    {
        PlayCorrectAnswerSound();
    }
    private void UIEvents_ActivateExplosion()
    {
        PlayBombExplosionSound();
    }

    private void UIEvents_ActivateSmoke()
    {
        PlayBombSmokeSound();
    }
    private void GameEvents_GameWon()
    {
        PlayGameWonSound();
    }

    private void GameEvents_GameLost()
    {
        PlayLostSound();
    }
}
