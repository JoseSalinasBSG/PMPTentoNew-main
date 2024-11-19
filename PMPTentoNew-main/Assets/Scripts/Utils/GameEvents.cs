using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    // Username changed
    public static Action<string> UsernameChanged;
    // New username
    public static Action RequesNewUsername;
    public static Action WrongWhenNewUsername;
    public static Action<string> NewUsername;

    #region Login events

    public static Action<User> SuccessfulLogin;
    public static Action<string> ErrorLogin;
    public static Action<string> FailedLogin;
    public static Action UsernameSelected;
    public static Action<int> NewInstuctorId;
    public static Action InstructorSelected;
    public static Action SuccessGetUser;
    public static Action ErrorGetUser;
    public static Action SuccessGetUserDetail;
    public static Action ErrorGetUserDetail;
    public static Action SuccessGetAvatar;
    public static Action ErrorGetAvatar;

    #endregion

    #region Main menu events

    public static Action<int> InstructorChanged;
    public static Action RequestRanking;
    public static Action RankingRetrieved;

    #endregion

    #region Question and options events

    public static Action CorrectlyAnswered;
    public static Action IncorrectlyAnswered;
    public static Action GameWon;
    public static Action GameLost;

    public static Action<Domains> DomainsRetreived;
    public static Action<string> DomainsSaved;
    
    public static Action<List<Task>> TaskRetreived;
    public static Action<int> GetIdDomain;
    public static Action<int> GetIdTask;
    public static Action<string> GetNameExam;
    public static Action<string> GetUserExam;
    public static Action<ResponseOfRegisterExam> ExamCreated;
    public static Action<QuestionInformationExam> QuestionsRetrieved;
    public static Action<int> ExamObtained;
    public static Action QuestionReady;
  
    public static Action OnGoodStreaked;
    public static Action OnGoodWithoutErrors;
    public static Action<AchievementData.Achievement, UnityEngine.UI.Button> OnSetGiftsFromAchievement;
    
    
    #endregion

    #region Experience

    public static Action<float> RequestExperienceChange;
    public static Action ExperienceChanged;

    #endregion
    
    #region Coins

    public static Action<float> RequestCoinsChange;
    public static Action CoinsChanged;

    #endregion

    #region Learning mode

    public static Action<Vector3> ChosenPlatform;
    public static Action<PlatformItem.PlatformInformation> SendInformation;

    #endregion

    #region Notifications

    public static Action<int> RecoveryAttempt;
    public static Action<NotificationPanel> AddNotificationPanel;
    public static Action<BackgroundNotification> SendNotification;
    public static Action<BackgroundNotificationIos> SendNotificationIos;

    #endregion

    #region Survivor Challenge

    public static Action RecoveryTime;

    #endregion


    #region PowerUps

    public static Action RequestUpdateDetail;
    public static Action RequestUpdateAchievements;

    public static Action DetailChanged;

    #endregion
}
