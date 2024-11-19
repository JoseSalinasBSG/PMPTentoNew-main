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


    //private bool _hasReachedMaxGoodStreak;

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
                Debug.Log($"Alcanzo Racha de {maxGoodStreakList[i]} preguntas");
                GameEvents.OnGoodStreaked?.Invoke();
                OnMaxGoodStreakReached?.Invoke(maxGoodStreakList[i]);
                UpdateAchievementData(maxGoodStreakList[i], SetDateAchievement(), achievementOrigin);//añado al contador de rachas de achivement data a traves del metodo StreakCounter

                //_hasReachedMaxGoodStreak = true;
            }

        }
        /*
        if (counter >= maxGoodStreakList[0] && !_hasReachedMaxGoodStreak)//si counter alcanzo o supero maxGoodStreak
        {
            Debug.Log("Alcanzo Racha");
            GameEvents.OnGoodStreaked?.Invoke();
            OnMaxGoodStreakReached?.Invoke(maxGoodStreakList[0]);
            _hasReachedMaxGoodStreak = true;
            
        }*/

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
                //achievementListContainer.achievementList[0].Streak4Questions++;
                //achievementListContainer.achievementList[0].Streak4Date = date;
                //achievementListContainer.achievementList[0].lastOriginStreak4 = origin;
                _objectUser.userInfo.user.achievements.streak4++;
                _objectUser.userInfo.user.achievements.streak4Date=date;
                _objectUser.userInfo.user.achievements.streak4Origin=origin;

                Debug.Log("Se añadio logro racha 4 preguntas");
                break;

            case 6:
                _objectUser.userInfo.user.achievements.streak6++;
                _objectUser.userInfo.user.achievements.streak6Date = date;
                _objectUser.userInfo.user.achievements.streak6Origin = origin;
                Debug.Log("Se añadio logro racha 6 preguntas");
                break;
            case 8:
                _objectUser.userInfo.user.achievements.streak8++;
                _objectUser.userInfo.user.achievements.streak8Date = date;
                _objectUser.userInfo.user.achievements.streak8Origin = origin;
                Debug.Log("Se añadio logro racha 8 preguntas");
                break;

            case 10:
                _objectUser.userInfo.user.achievements.streak10++;
                _objectUser.userInfo.user.achievements.streak10Date = date;
                _objectUser.userInfo.user.achievements.streak10Origin = origin;
                Debug.Log("Se añadio logro racha 10 preguntas");
                break;
        }
        //SaveLocalData();
        GameEvents.RequestUpdateAchievements?.Invoke();
    }

}
