using System;
using System.Collections;
using System.Collections.Generic;
using DataStorage;
using ScriptableCreator;
using UnityEngine;
using UnityEngine.Serialization;

public class UserManager : MonoBehaviour
{
    private ScriptableObjectUser _userSO;
    [SerializeField] private UserService _userService;
    [SerializeField] private ScripableObjectPowerUp _powerUpSecondOportunity;
    [SerializeField] private ScripableObjectPowerUp _powerUpTrueOption;
    [SerializeField] private ScripableObjectPowerUp _powerUpDeleteOption;
    [SerializeField] private ScripableObjectPowerUp _powerUpNextQuestion;
    [SerializeField] private ScripableObjectPowerUp _powerUpMoreTime;

    public bool EndFinishLoadData, EndFinishLoadAvatar;

    private DataStorageManager _storeManager;

    private void Start()
    {
    }

    private void OnEnable()
    {
        EndFinishLoadData = false;
        Initialize();
        // InitPowerUpData();
        GameEvents.SuccessGetUser += GameEvent_SuccessGetUser;
        GameEvents.SuccessGetUserDetail += GameEvent_SuccessGetUserDetail;
        GameEvents.ErrorGetUser += GameEvents_ErrorGetUser;
        GameEvents.ErrorGetUserDetail += GameEvents_ErrorGetUserDetail;
        GameEvents.SuccessGetAvatar += GameEvents_SuccessGetAvatar;
        GameEvents.ErrorGetAvatar += GameEvents_ErrorGetAvatar;
    }

    private void GameEvents_ErrorGetAvatar()
    {
        EndFinishLoadData = true;
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
        Debug.Log("error get user detail");

        _userSO.userInfo.haveUsername = false;
        _userSO.userInfo.haveInstructor = false;
        EndFinishLoadData = true;
    }

    private void GameEvents_ErrorGetUser()
    {
        Debug.Log("error get user");

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
        _powerUpMoreTime.amount = _userSO.userInfo.user.detail.increaseTime;
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
        _storeManager = new DataStorageManager(new PlayerPrefsStorageAdapter());

        if (!_userSO)
        {
            _userSO = Resources.Load<ScriptableObjectUser>("User Data");
        }

        //if (PlayerPrefs.HasKey("username") && PlayerPrefs.HasKey("password"))
        if (_storeManager.HasKey("username") && _storeManager.HasKey("password"))
        {
            //_userService.GetUSer(PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"));
            //Debug.LogError("username: " + _storeManager.Load<string>("username") + " password: " + _storeManager.Load<string>("password"));
            _userService.GetUSer(_storeManager.Load<string>("username"), _storeManager.Load<string>("password"));
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

        // if (PlayerPrefs.HasKey("UserInfo") )
        // {
        //     _userSO.userInfo.haveUser= true;
        //     _userSO.userInfo.user = JsonUtility.FromJson<User>(PlayerPrefs.GetString("UserInfo"));
        //     GameEvents.GetUserExam?.Invoke(_userSO.userInfo.user.userName);
        // }
        // else
        // {
        //     _userSO.userInfo.haveUser = false;
        //     _userSO.userInfo.user = new User();
        // }
        // if (PlayerPrefs.HasKey("HaveUsername") && PlayerPrefs.HasKey("Username"))
        // {
        //     _userSO.userInfo.haveUsername = true;
        //     _userSO.userInfo.username = PlayerPrefs.GetString("Username");
        // }
        // else
        // {
        //     _userSO.userInfo.haveUsername = false;
        //     _userSO.userInfo.username = "";
        // }
        //
        // if (PlayerPrefs.HasKey("HaveInstructor"))
        // {
        //     _userSO.userInfo.haveInstructor = true;
        //     _userSO.userInfo.idInstructor = PlayerPrefs.GetInt("HaveInstructor");
        // }
        // else
        // {
        //     _userSO.userInfo.haveInstructor = false;
        //     _userSO.userInfo.idInstructor = -1;
        // }
        // if (PlayerPrefs.HasKey("TotalExperience"))
        // {
        //     _userSO.userInfo.totalExperience = PlayerPrefs.GetFloat("TotalExperience");
        // }
        // else
        // {
        //     _userSO.userInfo.totalExperience = 100;
        // }
        // if (PlayerPrefs.HasKey("TotalCoins"))
        // {
        //     _userSO.userInfo.totalCoins = PlayerPrefs.GetFloat("TotalCoins");
        // }
        // else
        // {
        //     _userSO.userInfo.totalCoins = 100;
        // }
        // Debug.Log("terminado 1");
        // EndFinishLoadData = true;
    }

    // private void InitPowerUpData()
    // {
    //     if (PlayerPrefs.HasKey("pu_secondOportunity"))
    //     {
    //         _powerUpSecondOportunity.amount = PlayerPrefs.GetInt("pu_secondOportunity");
    //     }
    //     else
    //     {
    //         _powerUpSecondOportunity.amount = 5;
    //         PlayerPrefs.SetInt("pu_secondOportunity", _powerUpSecondOportunity.amount);
    //         PlayerPrefs.Save();
    //     }
    //     if (PlayerPrefs.HasKey("pu_trueOption"))
    //     {
    //         _powerUpTrueOption.amount = PlayerPrefs.GetInt("pu_trueOption");
    //     }
    //     else
    //     {
    //         _powerUpTrueOption.amount = 5;
    //         PlayerPrefs.SetInt("pu_trueOption", _powerUpTrueOption.amount);
    //         PlayerPrefs.Save();
    //     }
    //     if (PlayerPrefs.HasKey("pu_deleteOption"))
    //     {
    //         _powerUpDeleteOption.amount = PlayerPrefs.GetInt("pu_deleteOption");
    //     }
    //     else
    //     {
    //         _powerUpDeleteOption.amount = 5;
    //         PlayerPrefs.SetInt("pu_deleteOption", _powerUpDeleteOption.amount);
    //         PlayerPrefs.Save();
    //     }
    //     if (PlayerPrefs.HasKey("pu_nextQuestion"))
    //     {
    //         _powerUpNextQuestion.amount = PlayerPrefs.GetInt("pu_nextQuestion");
    //     }
    //     else
    //     {
    //         _powerUpNextQuestion.amount = 5;
    //         PlayerPrefs.SetInt("pu_nextQuestion", _powerUpNextQuestion.amount);
    //         PlayerPrefs.Save();
    //     }
    //     if (PlayerPrefs.HasKey("pu_moreTime"))
    //     {
    //         _powerUpMoreTime.amount = PlayerPrefs.GetInt("pu_moreTime");
    //     }
    //     else
    //     {
    //         _powerUpMoreTime.amount = 5;
    //         PlayerPrefs.SetInt("pu_moreTime", _powerUpMoreTime.amount);
    //         PlayerPrefs.Save();
    //     }
    // }
}
