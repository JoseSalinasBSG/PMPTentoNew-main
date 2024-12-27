using UnityEngine;

namespace ScriptableCreator.PowerUpSOC
{
    [CreateAssetMenu(menuName = "Power Up/TrueOptionPowerUp", fileName = "TrueOptionPowerUp")]
    public class TrueOptionPowerUp : PowerUpSO
    {
        public override void AddPowerUpToUser(ScriptableObjectUser scriptableObjectUser, int amount)
        {
            scriptableObjectUser.userInfo.user.detail.findCorrectAnswer += amount;
        }
        public override int GetAmount(ScriptableObjectUser scriptableObjectUser)
        {
            return scriptableObjectUser.userInfo.user.detail.findCorrectAnswer;
        }
        public override string GetName()
        {
            return "Opción verdadera";
        }
    }
}