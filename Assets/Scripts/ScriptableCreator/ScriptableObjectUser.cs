using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class User
{
    public string id;
    public int idProveedor;
    public int cursos;
    public int idAlumno;
    public int idContacto;
    public string clave;
    public string userName;
    public string token;
    public bool tipoCarrera;
    public string correoEnc;
    public string telEnc;
    public string userAgent;
    public string userIp;
    public UserDetail detail;
    public UserAchievements achievements;
    public Excepcion excepcion;
}
[Serializable]
public class UserDetail
{
    public int idCaracteristicaGamificacion;
    public int idAlumno;
    public string usernameG;
    public int instructorID;
    public int totalCoins;
    public int totalExperience;
    public int discardOption;
    public int skipQuestion;
    public int findCorrectAnswer;
    public int moreTime;
    public int secondChance;
}
[Serializable]
public class UserAchievements
{
    public int id;
    public int idAlumno;//parametro obligatorio en endpoint
    public int streak4;
    public int streak6;
    public int streak8;
    public int streak10;
    public string streak4Date;
    public string streak6Date;
    public string streak8Date;
    public string streak10Date;
    public string streak4Origin;
    public string streak6Origin;
    public string streak8Origin;
    public string streak10Origin;
    public string usuario;//parametro obligatorio al parecer en endpoint
}
[Serializable]
public class Excepcion
{
    public bool excepcionGenerada;
    public string descripcionGeneral;
    // public string error;
}

[Serializable]
public class ItemState
{
    public int id;
    public List<DateTime> timesToRetrive = new List<DateTime>();
}
[Serializable]
public class LearningModeState
{
    public List<ItemState> ItemStates = new List<ItemState>();
}
[Serializable]
public class UserInfo
{
    public User user;
    public LearningModeState LearningModeState;
    public bool haveUser;
    public bool haveUsername;
    public bool haveInstructor;
    public int idInstructor;
    public bool haveAvatar;
    public string urlAvatar;
    public Sprite spriteAvatar;
}

[CreateAssetMenu(fileName = "User Data", menuName = "User data")]
public class ScriptableObjectUser : ScriptableObject
{
    public UserInfo userInfo;

    private void OnEnable()
    {
        GameEvents.UsernameChanged += GameEvents_NameChanged;
        GameEvents.NewUsername += GameEvents_NewUsername;
        GameEvents.NewInstuctorId += GameEvents_NewInstuctorId;
        GameEvents.RequestExperienceChange += GameEvents_RequestExperienceChange;
        GameEvents.RequestCoinsChange += GameEvents_RequestCoinsChange;
    }

    private void GameEvents_RequestExperienceChange(float obj)
    {
        // userInfo.totalExperience += obj;
        PlayerPrefs.SetFloat("TotalExperience", userInfo.user.detail.totalExperience);
        PlayerPrefs.Save();
        GameEvents.ExperienceChanged?.Invoke();
    }

    private void OnDisable()
    {
        GameEvents.UsernameChanged -= GameEvents_NameChanged;
        GameEvents.NewUsername -= GameEvents_NewUsername;
        GameEvents.NewInstuctorId -= GameEvents_NewInstuctorId;
        GameEvents.RequestExperienceChange -= GameEvents_RequestExperienceChange;
        GameEvents.RequestCoinsChange -= GameEvents_RequestCoinsChange;
    }

    private void GameEvents_RequestCoinsChange(float obj)
    {
        // userInfo.totalCoins += obj;
        PlayerPrefs.SetFloat("TotalCoins", userInfo.user.detail.totalCoins);
        PlayerPrefs.Save();
        GameEvents.CoinsChanged?.Invoke();
    }

    private void GameEvents_NewInstuctorId(int index)
    {
        userInfo.haveInstructor = true;
        userInfo.idInstructor = index;
        PlayerPrefs.SetInt("HaveInstructor", index);
        PlayerPrefs.Save();

        //GameEvents.InstructorSelected?.Invoke();
    }

    private void GameEvents_NameChanged(string username)
    {
        // userInfo.username = username;
        PlayerPrefs.SetString("Username",username);
        PlayerPrefs.Save();
    }
    private void GameEvents_NewUsername(string username)
    {
        userInfo.haveUsername = true;
        // userInfo.username = username;
        PlayerPrefs.SetString("Username",username);
        PlayerPrefs.SetInt("HaveUsername",1);
        PlayerPrefs.Save();
        //GameEvents.UsernameSelected?.Invoke();
    }

    public void AddCounter(int idTask)
    {
        // Encuentra el item existente
        var item = userInfo.LearningModeState.ItemStates.FirstOrDefault(x => x.id == idTask);

        if (item != null)
        {
            // Si el item existe, añade un nuevo tiempo
            item.timesToRetrive ??= new List<DateTime>();
            item.timesToRetrive.Add(DateTime.Now.AddSeconds(10));
        }
        else
        {
            // Si el item no existe, créalo y añádelo
            item = new ItemState
            {
                id = idTask,
                timesToRetrive = new List<DateTime> { DateTime.Now.AddSeconds(10) }
            };

            userInfo.LearningModeState.ItemStates.Add(item);
        }
    }

}
