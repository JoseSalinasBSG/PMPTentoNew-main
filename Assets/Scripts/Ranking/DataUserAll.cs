using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataUserAll", menuName = "ScriptableObjects/DataUserAll")]
public class DataUserAll : ScriptableObject
{
    [Serializable]
    public class DataUsers
    {
        public int id;
        // public int position;
        public string userName;
        public int totalExperience;        
        public AvatarUsers avatar;
        public string urlAvatarUser;
        public Sprite spriteAvatarUser;
        public int position;
 
    }
    [Serializable]
    public class AvatarUsers
    {
        public int idAvatar;
        public int idAlumno;
        public string nombres;
        public string topC;
        public string accessories;
        public string hair_Color;
        public string facial_Hair;
        public string facial_Hair_Color;
        public string clothes;
        public string clothes_Color;
        public string eyes;
        public string eyesbrow;
        public string mouth;
        public string skin;        
    }

    public List<DataUsers> Users;

    private void OnEnable()
    {
        Users.Clear();
    }

    private void Reset()
    {
        Users.Clear();
    }
}
