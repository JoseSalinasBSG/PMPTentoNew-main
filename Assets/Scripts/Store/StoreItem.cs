using System;
using Button;
using ScriptableCreator.PowerUpSOC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Esta clase gestiona los elementos de la sección de la tienda.
/// 
/// Actualizaciones recientes (por Jose Salinas, [17/12/2024]):
/// - Se agregó el if->return para controlar el botón de compra.
/// 
/// Propósito de los cambios:
/// - La tienda permitia comprar items aun sin saldo de dinero.
/// 
/// </summary>


namespace Store
{
    public class StoreItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountLabel;
        [SerializeField] private TextMeshProUGUI _costLabel;
        [SerializeField] private PowerUpSO _powerUp;
        [SerializeField] private Image _image;
        [SerializeField] private ButtonAnimation _buttonAnimation;
        [SerializeField] private Image _backgroundImage;

        private StoreController _storeController;
        public Action<int> SendEvent;
        private float _cost;
        private int _amount;
        private Color _powerUpColor;
        public float Cost => _cost;
        public string NamePowerUp => _powerUp.nameInPlayerPrefs;
        public int Amount => _amount;
        public PowerUpSO PowerUp => _powerUp;
        public Sprite SpriteFromImage => _image.sprite;


        public void SetData(StoreController storeController, float cost, int amount, Sprite sprite, PowerUpSO powerUp, Color powerUpColor, Color backgroundColor)
        {
            _storeController = storeController;
            _cost = cost;
            _amount = amount;
            _powerUpColor = powerUpColor;
            _backgroundImage.color = backgroundColor;
            _buttonAnimation.SetDefaultColor(powerUpColor);            

            //PassScrollEvents passScroll = gameObject.GetComponent<PassScrollEvents>();
            if (_cost > storeController.CoinsFromUser)
            {
                //passScroll.enabled = false;
                _costLabel.color = Color.red;
                _buttonAnimation.DisableButton();
            }
            else
            {
                //passScroll.enabled = true;
                _costLabel.color = _powerUpColor;
                _buttonAnimation.EnableButton();
            }
            _costLabel.text = $"${_cost}";
            _amountLabel.text = $"x{_amount}";
            _image.sprite = sprite;
            _image.color = _powerUpColor;
            _powerUp = powerUp;
        }

        public void BuyItem()
        {
            if (_cost > _storeController.CoinsFromUser)
                return;

            _storeController.SetPopUpsCompra(this);
        }

        private void OnEnable()
        {
            GameEvents.CoinsChanged += GameEvents_CoinsChanged;
        }

        private void OnDisable()
        {
            GameEvents.CoinsChanged -= GameEvents_CoinsChanged;
        }

        private void GameEvents_CoinsChanged()
        {
            if (_cost > _storeController.CoinsFromUser)
            {
                _costLabel.color = Color.red;
                _buttonAnimation.DisableButton();
                
            }
            else
            {
                _costLabel.color = _powerUpColor;
                _buttonAnimation.EnableButton();
            }
        }
    }
}