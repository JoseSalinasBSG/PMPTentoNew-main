using System;
using System.Globalization;
using System.Linq;
using Question;
using ScriptableCreator;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Esta clase gestiona la funcionalidad del Desafío de Glosario en el juego.
/// 
/// Actualizaciones recientes (por [Tu Jose Salinas], [17/12/2024]):
/// - Se incluyó la línea `_userData.userInfo.user.detail.totalCoins += (int)coins;`
///   para actualizar las monedas totales acumuladas del usuario.
/// 
/// Propósito de los cambios:
/// - Mejorar la funcionalidad al rastrear las monedas acumuladas por el usuario para
///   un mejor monitoreo del progreso.
/// 
/// Notas para desarrolladores futuros:
/// - Asegurarse de que `_userData` esté correctamente inicializado y sincronizado
///   con el sistema de datos del usuario del juego.
/// - Validar cualquier dependencia de `_userData` en los componentes relacionados.
/// </summary>

public class TrainingChallengeController : MonoBehaviour
{
    [SerializeField] private DataToRegisterSO _registerSo;
    [SerializeField] private DomainsAndTaskSO _domainsAndTask;
    [SerializeField] private DataToRegisterSO _registerExam;
    [SerializeField] private ScriptableObjectSettings _gameSettings;
    [SerializeField] private ScriptableObjectUser _userData;
    // [SerializeField] private TextMeshProUGUI _DomainLabelInQuestions;
    [SerializeField] private TextMeshProUGUI _TaskLabelInQuestions;
    [SerializeField] private QuestionController _questionController;
    [SerializeField] private PMPService _pmpService;
    [SerializeField] private RewardItemController _rewardItemController;

    private float _numberOfConsecutiveQuestion;//contador respuesta consecutiva
    private float _experienceAccumulated = 0;
    private float _coinsAccumulated = 0;
    private void Awake()
    {
        _numberOfConsecutiveQuestion = -2;
        _pmpService.Service_GetDomainAndTasks();
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.AudioSettings.TrainingChallengeSound, true);
    }

    private void GetQuestions()
    {
        // _pmpService.Service_GetQuestions(9682);
        UIEvents.ShowLoadingView?.Invoke();
        // GameEvents.GetNameExam?.Invoke(DateTime.Now.ToString(CultureInfo.CurrentCulture));
        var task = _domainsAndTask.DomainContainer.listaTarea[Random.Range(0, _domainsAndTask.DomainContainer.listaTarea.Length)];
        var response = _pmpService.Service_GetDomainAndTaskNames(task.id);
        // _DomainLabelInQuestions.text = response.Item1;
        _TaskLabelInQuestions.text = $"<b>Tarea:</b> {response.Item2}";//Titulo de Tarea, se pone tarea en negrita usando <b>
        _registerExam.dataToRegisterExam.IdSimuladorPmpTarea = task.id;
        _registerExam.dataToRegisterExam.IdSimuladorPmpDominio = _domainsAndTask.DomainContainer.listaDominio.FirstOrDefault(x => x.id == task.idSimuladorPmpDominio)!.id;
        GameEvents.GetNameExam?.Invoke($"ModoAprendizaje-{_userData.userInfo.user.detail.usernameG}-{task.id}-{task.idSimuladorPmpDominio}-{DateTime.Now.ToString(CultureInfo.CurrentCulture)}");
    }

    private void OnEnable()
    {
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


    private void GameEvents_GetNameExam(string obj)
    {
        _pmpService.Service_GetExam();
    }

    private void OnDisable()
    {
        GameEvents.CorrectlyAnswered -= GameEvents_CorrectlyAnswered;
        GameEvents.IncorrectlyAnswered -= GameEvents_IncorrectlyAnswered;
        GameEvents.GetNameExam -= GameEvents_GetNameExam;
        GameEvents.GameWon -= GameEvents_GameWon;
        GameEvents.GameLost -= GameEvents_GameWon;
        GameEvents.DomainsSaved -= GameEvents_DomainsSaved;

    }

    private void GameEvents_CorrectlyAnswered()//respuesta correcta
    {

        _numberOfConsecutiveQuestion++;//incrementa el numero de respuestas acertadas consecutivas
        var clampConsecutive = Mathf.Clamp(_numberOfConsecutiveQuestion, 0, int.MaxValue);//limita el valor de _numberOfConsecutiveQuestion

        //EXPERIENCIA
        var exp = //experiencia recibida
                  // Base experience
            _gameSettings.settingData.DSReward.baseExperience +
            // Bonus by consecutive question
            _gameSettings.settingData.DSReward.aditionalBonusExpConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.DSReward.aditionalBonusExpForAchievement * 0;
        GameEvents.RequestExperienceChange?.Invoke(exp);//Invoca evento de cambio de experiencia
        _experienceAccumulated += exp;//se actualiza el total de experiencia acumulada
        _userData.userInfo.user.detail.totalExperience += (int)exp;

        //MONEDAS
        var coins =//monedas recibidas
                   // Base coins
            _gameSettings.settingData.DSReward.baseCoins +
            // Bonus by consecutive question
            _gameSettings.settingData.DSReward.aditionalBonusCoinsConsecutiveQuestion * clampConsecutive +
            // Bonus by achievement
            _gameSettings.settingData.DSReward.aditionalBonusCoinsForAchievement * 0;
        _coinsAccumulated += coins;//se actualiza monedas acumuladas
        _userData.userInfo.user.detail.totalCoins += (int)coins;
        
        GameEvents.RequestUpdateDetail?.Invoke();
        GameEvents.RequestCoinsChange?.Invoke(coins);//Invoca evento para notificar el cambio en las monedas

        CheckNumberOfConsecutiveQuestion();//revisar numero de respuestas consecutivas
    }

    private void GameEvents_IncorrectlyAnswered()//si responde mal
    {
        _numberOfConsecutiveQuestion = -2;//se vuelve a setear a -2

    }

    private void GameEvents_GameWon()
    {
        _rewardItemController.AddCoins((int)_coinsAccumulated);
        _rewardItemController.AddExperience((int)_experienceAccumulated);
        UIEvents.ShowFinishView?.Invoke();
    }

    public void CheckNumberOfConsecutiveQuestion()
    {
        if (_numberOfConsecutiveQuestion >= 0)
        {//si la variable es 0
            GameEvents.OnGoodStreaked?.Invoke();//se dispara evento OnGoodStreaked
        }
    }
}
