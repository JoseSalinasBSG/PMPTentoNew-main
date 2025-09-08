using System.Collections.Generic;
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

        [Header("Pop-up compra")]
        [SerializeField] private FadeUI _popupCompra;
        [SerializeField] private TextMeshProUGUI _messageCompra;
        [SerializeField] private Image _imageCompra;
        [SerializeField] private TextMeshProUGUI _amountLabel;
        [Header("Pop-up Confirmación Compra")]
        [SerializeField] private Image _iconPowerUpCC;
        [SerializeField] private TextMeshProUGUI _compraDetail;


        private bool areItemsInstanciated = false;
        private StoreItem _currentItem;
        public float CoinsFromUser => _user.userInfo.user.detail.totalCoins;
        private void OnEnable()
        {
            UpdatePowerUpTexts();
            SubscribeToGameEvents();

            if (areItemsInstanciated)
            {
                return;
            }

            InstantiateStoreItems();

            areItemsInstanciated = true;
        }

        private void SubscribeToGameEvents()
        {
            GameEvents.CoinsChanged += GameEvents_CoinsChanged;
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
                storeSection.SetData(powerUpConfig.storeSectionName, powerUpConfig.powerUpSprite, powerUpConfig.powerUpIconColor);

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
                    storeItem.SetData(this, costItem, i + 1, powerUpConfig.powerUpSprite, powerUpConfig.powerUpSO, powerUpConfig.powerUpIconColor, powerUpConfig.backgroundColor);
                }

            }
        }


        private void GameEvents_DetailChanged()
        {
            UpdatePowerUpTexts();

            GameEvents.CoinsChanged?.Invoke();
            GameEvents.ExperienceChanged?.Invoke();
        }

        private void GameEvents_CoinsChanged()
        {

        }

        public void SetPopUpsCompra(StoreItem storeItem)
        {
            _currentItem = storeItem;
            _imageCompra.sprite = _currentItem.SpriteFromImage;
            _messageCompra.text = $"{GetPowerUpName()}";
            _amountLabel.text = $"x{_currentItem.Amount} por ${_currentItem.Cost}";

            _iconPowerUpCC.sprite = _currentItem.SpriteFromImage;
            _compraDetail.text = $"Has comprado <b>{GetPowerUpName()} x{_currentItem.Amount}</b>";

            _popupCompra.gameObject.SetActive(true);
            _popupCompra.FadeInTransition();
        }

        public void BuyItem()
        {
            _user.AddPowerUp(_currentItem.PowerUp, _currentItem.Amount);
            _user.RemoveCoins((int)_currentItem.Cost);
            GameEvents.RequestUpdateDetail?.Invoke();
        }

        public string GetPowerUpName()
        {
            return _currentItem.PowerUp.GetName();
        }
    }
}