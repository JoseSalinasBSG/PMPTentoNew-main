using System;
using ScriptableCreator;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PowerUp
{
    public class PowerUpListener: MonoBehaviour
    {
        // [SerializeField] protected ScripableObjectPowerUp _event;
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private UnityEvent _response;

        private int _amountValue;
        public int Amount
        {
            get => _amountValue;
            set
            {
                _amountValue = value;
                _amount.text = value.ToString();
            }
        }
        private void Start()
        {
            // UpdateValue();
        }

        public void UpdateValue()
        {
            _amount.text = Amount.ToString();
        }
        // private void OnEnable()
        // {
        //     _event.RegisterListener(this);
        // }
        //
        // private void OnDisable()
        // {
        //     _event.UnregisterListener(this);
        // }

        public void OnEventRaised()
        {
            _response?.Invoke();
        }
    }
}