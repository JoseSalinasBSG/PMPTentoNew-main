using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AchievementControlller : MonoBehaviour
{
    [SerializeField] private AchievementData achievementData;
    [SerializeField] public List<int> maxGoodStreakList;//numero maximo de respuestas consecutivas para alcanzar una racha
    [SerializeField] private UnityEvent<int> OnMaxGoodStreakReached;//evento publico en editor
    [SerializeField] private UnityEvent OnMaxGoodWithoutErrorsReached;//evento publico en editor
    [SerializeField] private ScriptableObjectUser _objectUser;
    [SerializeField] private string achievementOrigin;

    private void OnEnable()
    {
        GameEvents.OnGoodStreaked += GoodStreak;
        GameEvents.OnGoodWithoutErrors += GoodWithoutErrors;
    }

    private void OnDisable()
    {
        GameEvents.OnGoodStreaked -= GoodStreak;
        GameEvents.OnGoodWithoutErrors -= GoodWithoutErrors;
    }

    private void GoodStreak()//racha
    {
        achievementData.AddCounter(0);
    }

    private void GoodWithoutErrors()//racha ronda sin fallas
    {
        achievementData.AddCounter(1);
    }

    public void CheckMaxGoodStreak(int counter)//verificar 
    {
        achievementData.achievementListContainer.achievementList[0].ConsecutiveAnswer = counter;//se llena el campo Consecutive Answer en SO Achievement Data

        for (int i = 0; i < maxGoodStreakList.Count; i++)//recorriendo lista de tipos de racha: 4,6,8 y 10
        {
            if (counter == maxGoodStreakList[i])//si es igual al contador ConsecutiveAnser
            {
                GameEvents.OnGoodStreaked?.Invoke();
                OnMaxGoodStreakReached?.Invoke(maxGoodStreakList[i]);
                UpdateAchievementData(maxGoodStreakList[i], SetDateAchievement(), achievementOrigin);//a�ado al contador de rachas de achivement data a traves del metodo StreakCounter
            }

        }
    }
    private string SetDateAchievement()
    {
        DateTime lastAchievementDate = DateTime.Now;
        return lastAchievementDate.ToString();
    }

    public void CheckGoodWithoutErrors()
    {
        GameEvents.OnGoodWithoutErrors?.Invoke();
        OnMaxGoodWithoutErrorsReached?.Invoke();
    }

    public void UpdateAchievementData(int verifier, string date, string origin)
    {
        switch (verifier)
        {
            case 4:
                _objectUser.userInfo.user.achievements.streak4++;
                _objectUser.userInfo.user.achievements.streak4Date=date;
                _objectUser.userInfo.user.achievements.streak4Origin=origin;
                break;

            case 6:
                _objectUser.userInfo.user.achievements.streak6++;
                _objectUser.userInfo.user.achievements.streak6Date = date;
                _objectUser.userInfo.user.achievements.streak6Origin = origin;
                break;
            case 8:
                _objectUser.userInfo.user.achievements.streak8++;
                _objectUser.userInfo.user.achievements.streak8Date = date;
                _objectUser.userInfo.user.achievements.streak8Origin = origin;
                break;

            case 10:
                _objectUser.userInfo.user.achievements.streak10++;
                _objectUser.userInfo.user.achievements.streak10Date = date;
                _objectUser.userInfo.user.achievements.streak10Origin = origin;
                break;
        }
        GameEvents.RequestUpdateAchievements?.Invoke();
    }
}
