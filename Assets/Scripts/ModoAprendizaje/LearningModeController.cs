using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ModoAprendizaje;
using Question;
using ScriptableCreator;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlatformInformationToPlayerPrefs
{
    public List<int> _itemToPlayerPrefsList = new List<int>();
}
public class LearningModeController : MonoBehaviour
{
    private const string PREFS_INFO_LEARNING_MODE = "LearningModeInformation";

    [SerializeField] private DataToRegisterSO _registerExam;
    [SerializeField] private DomainsAndTaskSO _domainsAndTask;
    [SerializeField] private ScriptableObjectSettings _gameSettings;
    [SerializeField] private ScriptableObjectUser _userData;
    [SerializeField] private TextMeshProUGUI _DomainLabelInQuestions;
    [SerializeField] private TextMeshProUGUI _TaskLabelInQuestions;
    [SerializeField] private QuestionController _questionController;
    [SerializeField] private PMPService _pmpService;
    [SerializeField] private RewardItemController _rewardItemController;
    [SerializeField] private PlatformController _platformController;
    [SerializeField] private Image _platformMarkerPrefab;

    private float _numberOfConsecutiveQuestion;
    private PlatformInformationToPlayerPrefs _informationToPlayerPrefs = new PlatformInformationToPlayerPrefs();
    private float _experienceAccumulated = 0;
    private float _coinsAccumulated = 0;
    private Image _markerInstanciated;
    private List<PlatformItem> _platformItems = new List<PlatformItem>();
    private PlatformItem _currentPlatform;
    private PlatformItem.PlatformInformation _selectedPlatformInformation;
    private bool haveInformationStored;
    private void Awake()
    {
        _numberOfConsecutiveQuestion = -2;
        _pmpService.Service_GetDomainAndTasks();
    }

    private void Start()
    {
        FindObjectOfType<GameplaySound>().PlayLearningModeSound();
    }

    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey(PREFS_INFO_LEARNING_MODE))
        {
            haveInformationStored = false;
        }
        else
        {
            _informationToPlayerPrefs = JsonUtility.FromJson<PlatformInformationToPlayerPrefs>(PlayerPrefs.GetString(PREFS_INFO_LEARNING_MODE));
            haveInformationStored = true;
        }
        GameEvents.DomainsRetreived += GameEvents_DomainRetreived;
        GameEvents.TaskRetreived += GameEvents_TaskRetreived;
        GameEvents.GetNameExam += GameEvents_GetNameExam;
        GameEvents.CorrectlyAnswered += GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered += GameEvents_IncorrectlyAnswered;
        GameEvents.SendInformation += GameEvents_SendInformation;
        GameEvents.GameWon += GameEvents_GameWon;
        GameEvents.GameLost += GameEvents_GameWon;
    }

    private void GameEvents_SendInformation(PlatformItem.PlatformInformation platformInformation)
    {
        _selectedPlatformInformation = platformInformation;
    }

    private void GameEvents_GameWon()
    {
        _rewardItemController.AddCoins((int)_coinsAccumulated);
        _rewardItemController.AddExperience((int)_experienceAccumulated);
        UIEvents.ShowFinishView?.Invoke();
    }

    private void GameEvents_IncorrectlyAnswered()
    {
        _numberOfConsecutiveQuestion = -2;
    }

    private void GameEvents_CorrectlyAnswered()
    {
        _numberOfConsecutiveQuestion++;
        var clampConsecutive = Mathf.Clamp(_numberOfConsecutiveQuestion, 0, int.MaxValue);
        var exp =
            // Base experience
            _gameSettings.settingData.MAReward.baseExperience +
            // Bonus by consecutive question
            _gameSettings.settingData.MAReward.aditionalBonusExpConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.MAReward.aditionalBonusExpForAchievement * 0;
        // GameEvents.RequestExperienceChange?.Invoke(exp);

        _experienceAccumulated += exp;
        _userData.userInfo.user.detail.totalExperience += (int)exp;

        var coins =
            // Base coins
            _gameSettings.settingData.MAReward.baseCoins +
            // Bonus by consecutive question
            _gameSettings.settingData.MAReward.aditionalBonusCoinsConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.MAReward.aditionalBonusCoinsForAchievement * 0;

        _coinsAccumulated += coins;
        _userData.userInfo.user.detail.totalCoins += (int)coins;
        // GameEvents.RequestCoinsChange?.Invoke(coins);
        GameEvents.RequestUpdateDetail?.Invoke();
    }

    private void GameEvents_GetNameExam(string obj)
    {
        _pmpService.Service_GetExam();
    }

    private void GameEvents_TaskRetreived(List<Task> obj)
    {
        // _moveItems.SetData(obj);
    }

    private void OnDisable()
    {
        GameEvents.DomainsRetreived -= GameEvents_DomainRetreived;
        GameEvents.TaskRetreived -= GameEvents_TaskRetreived;
        GameEvents.GetNameExam -= GameEvents_GetNameExam;
        GameEvents.CorrectlyAnswered -= GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered -= GameEvents_IncorrectlyAnswered;
        GameEvents.GameWon -= GameEvents_GameWon;
        GameEvents.GameLost -= GameEvents_GameWon;
    }

    private void GameEvents_DomainRetreived(Domains obj)
    {
        // _platformController.SetInstances(obj.listaTarea.Length);
        var ss = -1;
        var counterDomain = 0;
        for (int i = 0; i < obj.listaTarea.Length; i++)
        {
            if (ss != obj.listaTarea[i].idSimuladorPmpDominio)
            {
                counterDomain++;
                //cambio de dominio
                ss = obj.listaTarea[i].idSimuladorPmpDominio;
                _platformController.CreatePlatformInformation(
                    $"Sección {counterDomain}",
                    obj.listaDominio.FirstOrDefault(x => x.id == obj.listaTarea[i].idSimuladorPmpDominio)?.nombre,
                    _domainsAndTask.DomainDetail.First(x => x.id == ss).description);
                i--;
            }
            else
            {
                PlatformItem.PlatformInformation s = new PlatformItem.PlatformInformation
                {
                    id = obj.listaTarea[i].id,
                    description = obj.listaTarea[i].nombre,
                    index = i + 1,
                    tittle = obj.listaTarea[i].nombre,
                    totalTasks = obj.listaTarea.Length,
                    idDomain = obj.listaTarea[i].idSimuladorPmpDominio
                };
                var item = _platformController.CreatePlatform();
                item.Information = s;
                item.Attempts =
                    _userData.userInfo.LearningModeState.ItemStates.Exists(x => x.id == obj.listaTarea[i].id)
                        ? (3 - _userData.userInfo.LearningModeState.ItemStates.FirstOrDefault(x =>
                            x.id == obj.listaTarea[i].id)!.timesToRetrive.Count)
                        : 3;

                Debug.Log("attemptes: " + item.Attempts);
                if (haveInformationStored)
                {
                    if (_informationToPlayerPrefs._itemToPlayerPrefsList.Exists(
                            x => x == item.Information.id)
                        )
                    {
                        item.EnablePlatform();
                        _currentPlatform = item;
                        if (!_markerInstanciated)
                        {
                            _markerInstanciated = Instantiate(_platformMarkerPrefab, _currentPlatform.transform);
                        }
                        else
                        {
                            _markerInstanciated.transform.parent = _currentPlatform.transform;
                        }
                        _markerInstanciated.transform.SetLocalPositionAndRotation(new Vector3(0, 4, 0), quaternion.identity);
                        // _markerInstanciated.transform.parent = item.transform;
                    }
                }
                _platformItems.Add(item);
            }
        }

        if (!haveInformationStored)
        {
            _currentPlatform = _platformItems[0];
            _currentPlatform.EnablePlatform();
            _markerInstanciated = Instantiate(_platformMarkerPrefab, _currentPlatform.transform);
            _informationToPlayerPrefs._itemToPlayerPrefsList.Add(_currentPlatform.Information.id);
            PlayerPrefs.SetString(PREFS_INFO_LEARNING_MODE, JsonUtility.ToJson(_informationToPlayerPrefs));
            PlayerPrefs.Save();
        }
    }

    [ContextMenu("nextPlatform")]
    public void EnableNextPlatform()
    {
        _currentPlatform = _platformItems[_currentPlatform.Information.index++];
        _currentPlatform.EnablePlatform();
        _informationToPlayerPrefs._itemToPlayerPrefsList.Add(_currentPlatform.Information.id);
        PlayerPrefs.SetString(PREFS_INFO_LEARNING_MODE, JsonUtility.ToJson(_informationToPlayerPrefs));
        PlayerPrefs.Save();
        if (!_markerInstanciated)
        {
            _markerInstanciated = Instantiate(_platformMarkerPrefab, _currentPlatform.transform);
        }
        else
        {
            _markerInstanciated.transform.parent = _currentPlatform.transform;
        }
        _markerInstanciated.transform.SetLocalPositionAndRotation(new Vector3(0, 4, 0), quaternion.identity);
    }
    public void GetTasks()
    {
        // _pmpService.Service_GetTask(int.Parse(_buttonDomainController.CurrentButton.IndexS));
    }

    public void GetQuestions()
    {
        var response = _pmpService.Service_GetDomainAndTaskNames(_selectedPlatformInformation.id);
        _DomainLabelInQuestions.text = $"Dominio: {response.Item1}";
        _TaskLabelInQuestions.text = $"Tarea: {response.Item2}";
        _registerExam.dataToRegisterExam.IdSimuladorPmpTarea = _selectedPlatformInformation.id;
        _registerExam.dataToRegisterExam.IdSimuladorPmpDominio = _selectedPlatformInformation.idDomain;
        // _pmpService.Service_GetQuestions(9682);
        UIEvents.ShowLoadingView?.Invoke();
        GameEvents.GetNameExam?.Invoke($"ModoAprendizaje-{_userData.userInfo.user.detail.usernameG}-{response.Item1}-{response.Item2}-{DateTime.Now.ToString(CultureInfo.CurrentCulture)}");
    }

    public void SendRetrieveAttempt()
    {
        _userData.AddCounter(_questionController.CurrentQuestion.idTask);
    }
}
