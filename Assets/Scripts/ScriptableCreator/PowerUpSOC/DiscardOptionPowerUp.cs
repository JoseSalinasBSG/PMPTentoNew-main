using UnityEngine;

namespace ScriptableCreator.PowerUpSOC
{
    [CreateAssetMenu(menuName = "Power Up/DiscardOptionPowerUp", fileName = "DiscardOptionPowerUp")]
    public class DiscardOptionPowerUp : PowerUpSO
    {
        public override void AddPowerUpToUser(ScriptableObjectUser scriptableObjectUser, int amount)
        {
            scriptableObjectUser.userInfo.user.detail.discardOption += amount;
        }

        public override int GetAmount(ScriptableObjectUser scriptableObjectUser)
        {
            return scriptableObjectUser.userInfo.user.detail.discardOption;
        }
    }

}