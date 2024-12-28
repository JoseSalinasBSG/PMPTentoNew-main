using System.Collections.Generic;
using PowerUp;
using UnityEngine;

namespace ScriptableCreator.PowerUpSOC
{
    //[CreateAssetMenu(menuName = "Power Up", fileName = "Power Up")]
    public abstract class PowerUpSO : ScriptableObject
    {
        public int amount;
        public float unitCost;
        public float discount;
        public string nameInPlayerPrefs;
        public string nameToShowInStore;

        private List<PowerUpListener> _listeners = new List<PowerUpListener>();

        public virtual void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised();
            }
        }

        public virtual void RegisterListener(PowerUpListener listener)
        {
            _listeners.Add(listener);
        }

        public virtual void UnregisterListener(PowerUpListener listener)
        {
            _listeners.Remove(listener);
        }
        public virtual void AddCoinsToUser(ScriptableObjectUser scriptableObjectUser, int amount)
        {
            scriptableObjectUser.userInfo.user.detail.totalCoins += amount;
        }

        public abstract void AddPowerUpToUser(ScriptableObjectUser scriptableObjectUser, int amount);
        public abstract int GetAmount(ScriptableObjectUser scriptableObjectUser);

        public virtual string GetName()
        {
            return nameToShowInStore;
        }
    }

}