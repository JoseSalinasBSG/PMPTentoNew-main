using System;
using ScriptableCreator;
using TMPro;
using UnityEngine;

namespace Store
{
    [Serializable]
    public class PowerUpConfig 
    {
        public string sectionNamePowerUp;
        public PowerUpSO scripableObjectPowerUp;
        public TextMeshProUGUI textPowerUp;
        public Sprite spritePowerUp;

        public void UpdateTextPowerUp(ScriptableObjectUser scriptableObjectUser)
        {
            textPowerUp.text = scriptableObjectUser.userInfo.user.detail.secondChance.ToString();
        }
    }
}