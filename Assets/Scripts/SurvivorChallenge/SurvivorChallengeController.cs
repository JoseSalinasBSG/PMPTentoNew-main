using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Question;
using ScriptableCreator;
using UnityEngine;
using Random = UnityEngine.Random;

public class SurvivorChallengeController : MonoBehaviour
{
    [SerializeField] private DataToRegisterSO _registerSo;
    [SerializeField] private DomainsAndTaskSO _domainsAndTask;
    [SerializeField] private DataToRegisterSO _registerExam;
    [SerializeField] private ScriptableObjectSettings _gameSettings;
    [SerializeField] private ScriptableObjectUser _userData;

    [SerializeField] private QuestionController _questionController;
    [SerializeField] private PMPService _pmpService;
    [SerializeField] private RewardItemController _rewardItemController;


    [SerializeField] private TimerDS _timer;
    [SerializeField] private float timeToQuestions;
    [SerializeField] private float extraTime;

    private float _currentTime;
    private bool _useTimer;
    private float _numberOfConsecutiveQuestion;
    private float _experienceAccumulated = 0;
    private float _coinsAccumulated = 0;
    public bool UseTimer
    {
        get => _useTimer;
        set => _useTimer = value;
    }

    private void Awake()
    {
        _numberOfConsecutiveQuestion = -2;
        _pmpService.Service_GetDomainAndTasks();

    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.AudioSettings.SurvivalChallengeSound, true);
        _timer.InitValue(timeToQuestions);
        _currentTime = 0;
    }

    private void Update()
    {
        if (!UseTimer)
        {   
            return;
        }
        _currentTime += Time.deltaTime / timeToQuestions;
        if (_currentTime >1)
        {
            UseTimer = false;
            GameEvents.GameLost?.Invoke();
            return;
        }
        _timer.SetValueTimer(1 - _currentTime);
    }

    private void OnEnable()
    {
        GameEvents.RecoveryTime += GameEvent_RecoveryTime;
        UIEvents.ShowQuestionView += UIEvents_ShowQuestionView;
        GameEvents.CorrectlyAnswered += GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered += GameEvents_IncorrectlyAnswered;
        GameEvents.GetNameExam += GameEvents_GetNameExam;
        GameEvents.GameWon += GameEvents_GameWon;
        GameEvents.GameLost += GameEvents_GameWon;
        GameEvents.DomainsSaved += GameEvents_DomainsSaved;
    }

    private void GameEvents_DomainsSaved(string obj)
    {
        GetQuestions();
    }

    private void OnDisable()
    {
        GameEvents.RecoveryTime -= GameEvent_RecoveryTime;
        UIEvents.ShowQuestionView -= UIEvents_ShowQuestionView;
        GameEvents.CorrectlyAnswered -= GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered -= GameEvents_IncorrectlyAnswered;
        GameEvents.GetNameExam -= GameEvents_GetNameExam;
        GameEvents.GameWon -= GameEvents_GameWon;
        GameEvents.GameLost -= GameEvents_GameWon;
        GameEvents.DomainsSaved -= GameEvents_DomainsSaved;

    }

    private void GameEvents_GameWon()
    {
        _rewardItemController.AddCoins((int)_coinsAccumulated);
        _rewardItemController.AddExperience((int)_experienceAccumulated);
        Debug.Log("GameEvent_GameWon");
        UIEvents.ShowFinishView?.Invoke();
    }
    private void GameEvents_GetNameExam(string obj)
    {
        _pmpService.Service_GetExam();
    }

    private void GameEvents_IncorrectlyAnswered()
    {
        _numberOfConsecutiveQuestion = -2;
        Debug.Log("GameEvents_IncorrectlyAnswered");

    }
    private void GameEvents_CorrectlyAnswered()
    {
        
        _numberOfConsecutiveQuestion++;
        var clampConsecutive = Mathf.Clamp(_numberOfConsecutiveQuestion, 0, int.MaxValue);
        var exp = 
            // Base experience
            _gameSettings.settingData.DSReward.baseExperience +
            // Bonus by consecutive question
            _gameSettings.settingData.DSReward.aditionalBonusExpConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.DSReward.aditionalBonusExpForAchievement * 0;
        // GameEvents.RequestExperienceChange?.Invoke(exp);
        _experienceAccumulated += exp;
        _userData.userInfo.user.detail.totalExperience += (int)exp;


        var coins =
            // Base coins
            _gameSettings.settingData.DSReward.baseCoins +
            // Bonus by consecutive question
            _gameSettings.settingData.DSReward.aditionalBonusCoinsConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.DSReward.aditionalBonusCoinsForAchievement * 0;
        _coinsAccumulated += coins;
        _userData.userInfo.user.detail.totalCoins += (int)coins;

        // GameEvents.RequestCoinsChange?.Invoke(coins);
        GameEvents.RequestUpdateDetail?.Invoke();
    }
    private void UIEvents_ShowQuestionView()
    {
        UseTimer = true;
    }

    private void GameEvent_RecoveryTime()
    {
        UIEvents_ShowQuestionView();
        _currentTime = 0;
    }
    public void GetQuestions()
    {

        // _pmpService.Service_GetQuestions(9682);
        UIEvents.ShowLoadingView?.Invoke();
        // GameEvents.GetNameExam?.Invoke(DateTime.Now.ToString(CultureInfo.CurrentCulture));
        var task = _domainsAndTask.DomainContainer.listaTarea[
            Random.Range(
                0,
                _domainsAndTask.DomainContainer.listaTarea.Length
            )
        ];
        _registerExam.dataToRegisterExam.IdSimuladorPmpTarea = task.id;
        _registerExam.dataToRegisterExam.IdSimuladorPmpDominio = _domainsAndTask.DomainContainer.listaDominio.FirstOrDefault( x => x.id == task.idSimuladorPmpDominio)!.id;
        GameEvents.GetNameExam?.Invoke($"DesafíoSupervivencia-{_userData.userInfo.user.detail.usernameG}-{task.id}-{task.idSimuladorPmpDominio}-{DateTime.Now.ToString(CultureInfo.CurrentCulture)}");

    }

    public void AddMoreTime()
    {
        _currentTime = Mathf.Clamp(_currentTime - extraTime / timeToQuestions, 0, 1);
    }

    public void StopTimer()
    {
        UseTimer = false;
    }
    // public void EnableNextPlatform()
    // {
    //     _currentPlatform = _platformItems[_currentPlatform.Information.index++];
    //     _currentPlatform.EnablePlatform();
    //     if (!_markerInstanciated)
    //     {
    //         _markerInstanciated = Instantiate(_platformMarkerPrefab, _currentPlatform.transform);
    //     }
    //     else
    //     {
    //         _markerInstanciated.transform.parent = _currentPlatform.transform;
    //     }
    //     _markerInstanciated.transform.SetLocalPositionAndRotation(new Vector3(0,4,0), quaternion.identity);
    // }
}
