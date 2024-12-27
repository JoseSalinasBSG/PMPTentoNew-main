using System;
using System.Collections.Generic;
using Button;
using PowerUp;
using ScriptableCreator.PowerUpSOC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
    ///<summary>
    /// Este controlador gestiona las interacciones con la tienda dentro del juego, incluyendo la visualización y compra de potenciadores. 
    /// Los potenciadores disponibles se definen mediante objetos ScriptableObject y se muestran en la interfaz de usuario con precios variables y descuentos aplicados. 
    /// El script también maneja el sistema de ruleta, habilitando o deshabilitando la opción de girar la ruleta dependiendo del tiempo transcurrido desde su último uso. 
    /// Cuando un jugador compra un potenciador, se actualizan las estadísticas del usuario (como monedas y potenciadores disponibles), y se gestiona un pop-up de confirmación de compra. 
    /// Además, el controlador maneja la visualización de un contador para el tiempo restante hasta que la ruleta esté disponible nuevamente.
    ///</summary>

    public class StoreController : MonoBehaviour
    {
        [SerializeField] private ScriptableObjectUser _user;

        [Header("Power ups scriptable objects")]
        [SerializeField] private List<PowerUpConfig> _powerUpConfigList;

        [Header("General")]
        [SerializeField] private Transform _GeneralContainer;
        [SerializeField] private StoreSection _storeSectionPrefab;
        [SerializeField] private StoreItem _storeItemPrefab;
        [SerializeField] private Transform _offset;

        [Header("Pop-up compra")]
        [SerializeField] private FadeUI _popupCompra;
        [SerializeField] private TextMeshProUGUI _messageCompra;
        [SerializeField] private Image _imageCompra;
        [SerializeField] private TextMeshProUGUI _amountLabel;

        [Header("Roulette")]
        [SerializeField] private ButtonAnimation _rouletteButton;
        [SerializeField] private ButtonAnimation _rouletteButtonUsed;
        [SerializeField] private TextMeshProUGUI _timeRemainingRoulette;

        private bool areItemsInstanciated = false;
        private StoreItem _currentItem;
        public float CoinsFromUser => _user.userInfo.user.detail.totalCoins;
        private void OnEnable()
        {
            HandleRouletteState();

            UpdatePowerUpTexts();
            SubscribeToGameEvents();

            if (areItemsInstanciated)
            {
                return;
            }

            InstantiateStoreItems();

            areItemsInstanciated = true;
        }

        private void HandleRouletteState()
        {
            if (PlayerPrefs.HasKey("UseRoulette") && PlayerPrefs.GetString("UseRoulette") != "{}")//verifica si fue usada la ruleta
            {
                DateTime lastUseTime = DateTime.Parse(PlayerPrefs.GetString("UseRoulette"));
                TimeSpan timeSinceLastUse = DateTime.Now - lastUseTime;
                TimeSpan timeRemaining = TimeSpan.FromHours(24) - timeSinceLastUse;

                if (timeSinceLastUse < TimeSpan.FromHours(24))//es igual a la fecha de hoy
                {//desactiva ruleta
                    _rouletteButton.gameObject.SetActive(false);
                    _rouletteButtonUsed.gameObject.SetActive(true);
                    _rouletteButton.GetComponent<PassScrollEvents>().enabled = false;
                }
                else
                {//activa ruleta
                    _rouletteButton.gameObject.SetActive(true);
                    _rouletteButtonUsed.gameObject.SetActive(false);
                    _rouletteButton.GetComponent<PassScrollEvents>().enabled = true;
                }
            }
        }

        private void SubscribeToGameEvents()
        {
            GameEvents.CoinsChanged += GameEvents_CoinsChanged;
            GameEvents.ExperienceChanged += GameEvents_ExperienceChanged;
            GameEvents.DetailChanged += GameEvents_DetailChanged;
        }

        private void UpdatePowerUpTexts()
        {
            foreach (var powerUpConfig in _powerUpConfigList)
            {
                powerUpConfig.UpdateTextPowerUp(_user);
            }
        }

        private void InstantiateStoreItems()
        {
            foreach (var powerUpConfig in _powerUpConfigList)
            {
                var storeSection = Instantiate(_storeSectionPrefab, _GeneralContainer);
                storeSection.SetData(powerUpConfig.storeSectionName);

                for (int i = 0; i < 3; i++)
                {
                    var storeItem = Instantiate(_storeItemPrefab, storeSection.Container);
                    float costItem = 0;
                    if (i == 0)
                    {
                        costItem = powerUpConfig.powerUpSO.unitCost;
                    }
                    else
                    {
                        costItem = (powerUpConfig.powerUpSO.unitCost * (i + 1) - powerUpConfig.powerUpSO.discount * (i + 1));
                    }
                    storeItem.SetData(this, costItem, i + 1, powerUpConfig.powerUpSprite, powerUpConfig.powerUpSO);
                }

            }
            Instantiate(_offset, _GeneralContainer);
        }

        private void Update()
        {
            if (PlayerPrefs.HasKey("UseRoulette") && PlayerPrefs.GetString("UseRoulette") != "{}")
            {
                DateTime lastUseTime = DateTime.Parse(PlayerPrefs.GetString("UseRoulette"));
                TimeSpan timeSinceLastUse = DateTime.Now - lastUseTime;
                TimeSpan timeRemaining = TimeSpan.FromHours(24) - timeSinceLastUse;
                _timeRemainingRoulette.text = "El giro de la ruleta estará \r\ndisponible nuevamente en: " + string.Format("{0:D2}:{1:D2}:{2:D2}", timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
            }
        }


        private void GameEvents_DetailChanged()
        {
            UpdatePowerUpTexts();

            GameEvents.CoinsChanged?.Invoke();
            GameEvents.ExperienceChanged?.Invoke();
        }

        private void GameEvents_ExperienceChanged()
        {

        }

        private void GameEvents_CoinsChanged()
        {

        }

        public void OpenPopUpCompra(StoreItem storeItem)
        {
            _currentItem = storeItem;
            _imageCompra.sprite = _currentItem.SpriteFromImage;
            string pot = _currentItem.Amount > 1 ? "potenciador" : "potenciadores";
            _messageCompra.text = $"Está a punto de comprar {_currentItem.Amount} {pot} para {GetPowerUpName()}";
            _amountLabel.text = $"x{_currentItem.Amount}";
            _popupCompra.gameObject.SetActive(true);
            _popupCompra.FadeInTransition();
        }

        public void BuyItem()
        {
            _currentItem.PowerUp.AddPowerUpToUser(_user, _currentItem.Amount);

            _user.userInfo.user.detail.totalCoins -= (int)_currentItem.Cost;
            GameEvents.RequestUpdateDetail?.Invoke();
        }

        public string GetPowerUpName()
        {
            return _currentItem.PowerUp.GetName();
        }
    }
}