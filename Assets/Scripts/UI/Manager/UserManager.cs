using ScriptableCreator.PowerUpSOC;
using UnityEngine;

// <summary>
// La clase UserManager se encarga de gestionar los datos del usuario, incluyendo la obtención de la información del usuario y su avatar.
// Escucha los eventos relacionados con la carga de los datos del usuario y actualiza los estados correspondientes, como la carga de los detalles del usuario y avatar.
// Además, gestiona las opciones de poder del usuario, como la oportunidad de segunda chance, tiempo adicional, opción de omitir preguntas, y encontrar la respuesta correcta.
// Los datos del usuario se cargan desde un servicio externo utilizando credenciales almacenadas en PlayerPrefs o desde un ScriptableObject local.
// </summary>

public class UserManager : MonoBehaviour
{
    [SerializeField] private UserService _userService;
    [SerializeField] private PowerUpSO _powerUpSecondOportunity;
    [SerializeField] private PowerUpSO _powerUpTrueOption;
    [SerializeField] private PowerUpSO _powerUpDeleteOption;
    [SerializeField] private PowerUpSO _powerUpNextQuestion;
    [SerializeField] private PowerUpSO _powerUpMoreTime;
    private ScriptableObjectUser _userSO;
    public bool EndFinishLoadData, EndFinishLoadAvatar;

    private void OnEnable()
    {
        EndFinishLoadData = false;
        Initialize();
        GameEvents.SuccessGetUser += GameEvent_SuccessGetUser;
        GameEvents.SuccessGetUserDetail += GameEvent_SuccessGetUserDetail;
        GameEvents.ErrorGetUser += GameEvents_ErrorGetUser;
        GameEvents.ErrorGetUserDetail += GameEvents_ErrorGetUserDetail;
        GameEvents.SuccessGetAvatar += GameEvents_SuccessGetAvatar;
        GameEvents.ErrorGetAvatar += GameEvents_ErrorGetAvatar;
    }

    private void GameEvents_ErrorGetAvatar()
    {
        EndFinishLoadAvatar = true;
    }

    private void GameEvents_SuccessGetAvatar()
    {
        EndFinishLoadAvatar = true;
    }

    private void OnDisable()
    {
        GameEvents.SuccessGetUser -= GameEvent_SuccessGetUser;
        GameEvents.SuccessGetUserDetail -= GameEvent_SuccessGetUserDetail;
        GameEvents.ErrorGetUser -= GameEvents_ErrorGetUser;
        GameEvents.ErrorGetUserDetail -= GameEvents_ErrorGetUserDetail;
        GameEvents.SuccessGetAvatar -= GameEvents_SuccessGetAvatar;
        GameEvents.ErrorGetAvatar -= GameEvents_ErrorGetAvatar;
    }

    private void GameEvents_ErrorGetUserDetail()
    {
        _userSO.userInfo.haveUsername = false;
        _userSO.userInfo.haveInstructor = false;
        EndFinishLoadData = true;
    }

    private void GameEvents_ErrorGetUser()
    {
        _userSO.userInfo.haveUsername = false;
        _userSO.userInfo.haveInstructor = false;
        EndFinishLoadData = true;
    }

    private void GameEvent_SuccessGetUserDetail()
    {
        if (_userSO.userInfo.user.detail.idCaracteristicaGamificacion == 0)
        {
            _userSO.userInfo.haveUsername = false;
            _userSO.userInfo.haveInstructor = false;
        }
        else if (_userSO.userInfo.user.detail.idCaracteristicaGamificacion == 1)
        {
            _userSO.userInfo.haveUsername = true;
            _userSO.userInfo.haveInstructor = true;
        }

        _powerUpDeleteOption.amount = _userSO.userInfo.user.detail.discardOption;
        _powerUpMoreTime.amount = _userSO.userInfo.user.detail.moreTime;
        _powerUpNextQuestion.amount = _userSO.userInfo.user.detail.skipQuestion;
        _powerUpSecondOportunity.amount = _userSO.userInfo.user.detail.secondChance;
        _powerUpTrueOption.amount = _userSO.userInfo.user.detail.findCorrectAnswer;
        EndFinishLoadData = true;
    }

    private void GameEvent_SuccessGetUser()
    {
        GameEvents.GetUserExam?.Invoke(_userSO.userInfo.user.userName);
        _userService.GetUserDetail(_userSO.userInfo.user.idAlumno);//obtiene datos user detail de endpoint
        _userService.GetUserAchievement(_userSO.userInfo.user.idAlumno);//obtiene datos user achievement de endpoint
    }

    void Initialize()
    {
        if (!_userSO)
        {
            _userSO = Resources.Load<ScriptableObjectUser>("User Data");
        }

        if ((PlayerPrefs.HasKey("username") && PlayerPrefs.GetString("username") != "{}") && (PlayerPrefs.HasKey("password") && PlayerPrefs.GetString("password") != "{}"))
        {
            _userService.GetUSer(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"));
        }
        else
        {
            _userSO.userInfo.user = new User();
            _userSO.userInfo.haveUsername = false;
            _userSO.userInfo.haveUser = false;
            _userSO.userInfo.haveAvatar = false;
            EndFinishLoadData = true;
            EndFinishLoadAvatar = true;
            return;
        }
    }
}
