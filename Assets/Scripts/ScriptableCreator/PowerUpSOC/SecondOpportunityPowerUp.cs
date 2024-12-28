using UnityEngine;

namespace ScriptableCreator.PowerUpSOC
{
    [CreateAssetMenu(menuName = "Power Up/SecondaOpportunityPowerUp", fileName = "SecondaOpportunityPowerUp")]
    public class SecondOpportunityPowerUp : PowerUpSO
    {
        public override void AddPowerUpToUser(ScriptableObjectUser scriptableObjectUser, int amount)
        {
            scriptableObjectUser.userInfo.user.detail.secondChance += amount;
        }
        public override int GetAmount(ScriptableObjectUser scriptableObjectUser)
        {
            return scriptableObjectUser.userInfo.user.detail.secondChance;
        }
    }
}