using System;
using ScriptableCreator.PowerUpSOC;
using TMPro;
using UnityEngine;

namespace Store
{
    [Serializable]
    public class PowerUpConfig 
    {
        public string storeSectionName;
        public PowerUpSO powerUpSO;
        public TextMeshProUGUI powerUpText;
        public Sprite powerUpSprite;

        public void UpdateTextPowerUp(ScriptableObjectUser scriptableObjectUser)
        {
            powerUpText.text = powerUpSO.GetAmount(scriptableObjectUser).ToString();
        }
    }
}