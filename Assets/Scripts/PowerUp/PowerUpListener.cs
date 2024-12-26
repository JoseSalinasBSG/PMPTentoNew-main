using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PowerUp
{
    public class PowerUpListener: MonoBehaviour
    {
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

        public void OnEventRaised()
        {
            _response?.Invoke();
        }
    }
}