using UnityEngine;

namespace ScriptableCreator.PowerUpSOC
{
    [CreateAssetMenu(menuName = "Power Up/SkipQuesitonPowerUp", fileName = "SkipQuesitonPowerUp")]
    public class SkipQuesitonPowerUp : PowerUpSO
    {
        public override void AddPowerUpToUser(ScriptableObjectUser scriptableObjectUser, int amount)
        {
            scriptableObjectUser.userInfo.user.detail.skipQuestion += amount;
        }
        public override int GetAmount(ScriptableObjectUser scriptableObjectUser)
        {
            return scriptableObjectUser.userInfo.user.detail.skipQuestion;
        }
        public override string GetName()
        {
            return "Siguiente pregunta";
        }
    }
}