using System.Collections.Generic;
using PowerUp;
using UnityEngine;

namespace ScriptableCreator
{
    [CreateAssetMenu(menuName = "Power Up", fileName = "Power Up")]
    public class ScripableObjectPowerUp : ScriptableObject
    {
        public int amount;
        public float unitCost;
        public float discount;
        public string nameInPlayerPrefs;

        private List<PowerUpListener> _listeners = new List<PowerUpListener>();

        public void Raise()
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(PowerUpListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnregisterListener(PowerUpListener listener)
        {
            _listeners.Remove(listener);
        }
    }

}