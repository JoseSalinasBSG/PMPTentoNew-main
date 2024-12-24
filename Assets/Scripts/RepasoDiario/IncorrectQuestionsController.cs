using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableCreator;
using UnityEngine;

public class IncorrectQuestionsController : MonoBehaviour
{
    [SerializeField] private IncorrectQuestionsSO _incorrectQuestions;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("IncorrectQuestions"))
        {
            var stringQuestions = PlayerPrefs.GetString("IncorrectQuestions");
            _incorrectQuestions.questions = JsonUtility.FromJson<IncorrectQuestionsContainer>(stringQuestions);
            // AudioEvents_OnSFXVolumeChanged();
            Debug.Log(stringQuestions);
        }
        else
        {
            _incorrectQuestions.questions = new IncorrectQuestionsContainer();
        }
    }
}
