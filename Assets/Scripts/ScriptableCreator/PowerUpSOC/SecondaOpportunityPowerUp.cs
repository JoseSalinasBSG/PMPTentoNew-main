using UnityEngine;

namespace ScriptableCreator.PowerUpSOC
{
    [CreateAssetMenu(menuName = "Power Up/SecondaOpportunityPowerUp", fileName = "SecondaOpportunityPowerUp")]
    public class SecondaOpportunityPowerUp : PowerUpSO
    {
        public override void Apply(ScriptableObjectUser scriptableObjectUser, int amount)
        {
            scriptableObjectUser.userInfo.user.detail.secondChance += amount;
        }
        public override int GetAmount(ScriptableObjectUser scriptableObjectUser)
        {
            return scriptableObjectUser.userInfo.user.detail.secondChance;
        }
        public override string GetName()
        {
            return "Segunda oportunidad";
        }
    }
}