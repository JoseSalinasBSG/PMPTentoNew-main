using System;
using System.Collections;
using System.Collections.Generic;
using DataStorage;
using ScriptableCreator;
using UnityEngine;

public class IncorrectQuestionsController : MonoBehaviour
{
    [SerializeField] private IncorrectQuestionsSO _incorrectQuestions;

    private DataStorageManager _dataStorageManager;

    private void Start()
    {
    }

    private void OnEnable()
    {
        _dataStorageManager = new DataStorageManager(new PlayerPrefsStorageAdapter());
        //if (PlayerPrefs.HasKey("IncorrectQuestions"))
        if (_dataStorageManager.HasKey("IncorrectQuestions"))
        {
            //var stringQuestions = PlayerPrefs.GetString("IncorrectQuestions");
            var stringQuestions = _dataStorageManager.Load<string>("IncorrectQuestions");
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
