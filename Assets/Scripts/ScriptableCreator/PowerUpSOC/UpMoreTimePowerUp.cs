using UnityEngine;

namespace ScriptableCreator.PowerUpSOC
{
    [CreateAssetMenu(menuName = "Power Up/UpMoreTimePowerUp", fileName = "UpMoreTimePowerUp")]
    public class UpMoreTimePowerUp : PowerUpSO
    {
        public override void AddPowerUpToUser(ScriptableObjectUser scriptableObjectUser, int amount)
        {
            scriptableObjectUser.userInfo.user.detail.moreTime += amount;
        }
        public override int GetAmount(ScriptableObjectUser scriptableObjectUser)
        {
            return scriptableObjectUser.userInfo.user.detail.moreTime;
        }
    }
}