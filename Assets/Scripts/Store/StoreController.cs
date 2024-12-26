 using System;
using Button;
using ScriptableCreator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    [SerializeField] private ScriptableObjectUser _user;

    [Header("Power ups scriptable objects")]
    [SerializeField]
    private ScripableObjectPowerUp _powerUpSecondOportunity;
    private bool areItemsInstanciated = false;

    [SerializeField] private ScripableObjectPowerUp _powerUpTrueOption;
    [SerializeField] private ScripableObjectPowerUp _powerUpDeleteOption;
    [SerializeField] private ScripableObjectPowerUp _powerUpNextQuestion;
    [SerializeField] private ScripableObjectPowerUp _powerUpMoreTime;

    [Header("Power ups interfaces")]
    [SerializeField]
    private TextMeshProUGUI _powerUpSecondOportunityI;

    [SerializeField] private TextMeshProUGUI _powerUpTrueOptionI;
    [SerializeField] private TextMeshProUGUI _powerUpDeleteOptionI;
    [SerializeField] private TextMeshProUGUI _powerUpNextQuestionI;
    [SerializeField] private TextMeshProUGUI _powerUpMoreTimeI;

    [Header("Power ups scriptable objects")]
    [SerializeField]
    private Sprite _spriteSecondOportunity;

    [SerializeField] private Sprite _spriteTrueOption;
    [SerializeField] private Sprite _spriteDeleteOption;
    [SerializeField] private Sprite _spriteNextQuestion;
    [SerializeField] private Sprite _spriteMoreTime;

    [Header("General")][SerializeField] private Transform _GeneralContainer;
    [SerializeField] private StoreSection _storeSectionPrefab;
    [SerializeField] private StoreItem _storeItemPrefab;
    [SerializeField] private Transform _offset;
    //[SerializeField] private TextMeshProUGUI _totalCoinsLabel;
    //[SerializeField] private TextMeshProUGUI _totalExperienceLabel;
    //[SerializeField] private TextMeshProUGUI _usernameLabel;

    [Header("Pop-up compra")]
    [SerializeField]
    private FadeUI _popupCompra;

    [SerializeField] private TextMeshProUGUI _messageCompra;
    [SerializeField] private Image _imageCompra;
    [SerializeField] private TextMeshProUGUI _amountLabel;
    [Header("Roulette")]
    [SerializeField]
    private ButtonAnimation _rouletteButton;
    [SerializeField]
    private ButtonAnimation _rouletteButtonUsed;
    [SerializeField] private TextMeshProUGUI _timeRemainingRoulette;
    private StoreItem _currentItem;

    public float CoinsFromUser => _user.userInfo.user.detail.totalCoins;
    private void Update()
    {
        if (PlayerPrefs.HasKey("UseRoulette"))
        {
            DateTime lastUseTime = DateTime.Parse(PlayerPrefs.GetString("UseRoulette"));
            TimeSpan timeSinceLastUse = DateTime.Now - lastUseTime;
            TimeSpan timeRemaining = TimeSpan.FromHours(24) - timeSinceLastUse;
            _timeRemainingRoulette.text = "El giro de la ruleta estará \r\ndisponible nuevamente en: " + string.Format("{0:D2}:{1:D2}:{2:D2}", timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);
        }
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("UseRoulette"))//verifica si fue usada la ruleta
        {
            //Debug.Log(DateTime.Parse(PlayerPrefs.GetString("UseRoulette")));
            //var timeUsedRoulette = DateTime.Parse(PlayerPrefs.GetString("UseRoulette"));
            DateTime lastUseTime = DateTime.Parse(PlayerPrefs.GetString("UseRoulette"));
            TimeSpan timeSinceLastUse = DateTime.Now - lastUseTime;
            TimeSpan timeRemaining = TimeSpan.FromHours(24) - timeSinceLastUse;

            if (timeSinceLastUse < TimeSpan.FromHours(24))//es igual a la fecha de hoy
            {//desactiva ruleta
                _rouletteButton.gameObject.SetActive(false);
                _rouletteButtonUsed.gameObject.SetActive(true);
                //_timeRemainingRoulette.text = "El giro de la ruleta estará \r\ndisponible nuevamente en: " +timeRemaining;
                _rouletteButton.GetComponent<PassScrollEvents>().enabled = false;
            }
            else
            {//activa ruleta
                _rouletteButton.gameObject.SetActive(true);
                _rouletteButtonUsed.gameObject.SetActive(false);
                _rouletteButton.GetComponent<PassScrollEvents>().enabled = true;
            }
        }
        //_usernameLabel.text = _user.userInfo.user.detail.usernameG;
        //_totalCoinsLabel.text = _user.userInfo.user.detail.totalCoins.ToString();
        //_totalExperienceLabel.text = _user.userInfo.user.detail.totalExperience.ToString();

        _powerUpSecondOportunityI.text = _user.userInfo.user.detail.secondChance.ToString();
        _powerUpTrueOptionI.text = _user.userInfo.user.detail.findCorrectAnswer.ToString();
        _powerUpDeleteOptionI.text = _user.userInfo.user.detail.discardOption.ToString();
        _powerUpNextQuestionI.text = _user.userInfo.user.detail.skipQuestion.ToString();
        _powerUpMoreTimeI.text = _user.userInfo.user.detail.increaseTime.ToString();
        GameEvents.CoinsChanged += GameEvents_CoinsChanged;
        GameEvents.ExperienceChanged += GameEvents_ExperienceChanged;
        GameEvents.DetailChanged += GameEvents_DetailChanged;


        if (areItemsInstanciated)
        {
            return;
        }
        var itemInstantiaded = Instantiate(_storeSectionPrefab, _GeneralContainer);
        // Segunda oportunidad
        itemInstantiaded.SetData("Segunda oportunidad");
        for (int i = 0; i < 3; i++)
        {
            var storeItem = Instantiate(_storeItemPrefab, itemInstantiaded.Container);
            float costItem = 0;
            if (i == 0)
            {
                costItem = _powerUpSecondOportunity.unitCost;
            }
            else
            {
                costItem = (_powerUpSecondOportunity.unitCost * (i + 1) - _powerUpSecondOportunity.discount * (i + 1));
            }

            storeItem.SetData(this, costItem, i + 1, _spriteSecondOportunity, _powerUpSecondOportunity);
        }

        // Verdadera opcion

        itemInstantiaded = Instantiate(_storeSectionPrefab, _GeneralContainer);
        itemInstantiaded.SetData("Mostrar respuesta");
        for (int i = 0; i < 3; i++)
        {
            var storeItem = Instantiate(_storeItemPrefab, itemInstantiaded.Container);
            float costItem = 0;
            if (i == 0)
            {
                costItem = _powerUpTrueOption.unitCost;
            }
            else
            {
                costItem = (_powerUpTrueOption.unitCost * (i + 1) - _powerUpTrueOption.discount * (i + 1));
            }

            storeItem.SetData(this, costItem, i + 1, _spriteTrueOption, _powerUpTrueOption);
        }

        // Descartar alternativa

        itemInstantiaded = Instantiate(_storeSectionPrefab, _GeneralContainer);
        itemInstantiaded.SetData("Descartar alternativa");
        for (int i = 0; i < 3; i++)
        {
            var storeItem = Instantiate(_storeItemPrefab, itemInstantiaded.Container);
            float costItem = 0;
            if (i == 0)
            {
                costItem = _powerUpDeleteOption.unitCost;
            }
            else
            {
                costItem = (_powerUpDeleteOption.unitCost * (i + 1) - _powerUpDeleteOption.discount * (i + 1));
            }

            storeItem.SetData(this, costItem, i + 1, _spriteDeleteOption, _powerUpDeleteOption);
        }

        // Siguiente pregunta

        itemInstantiaded = Instantiate(_storeSectionPrefab, _GeneralContainer);
        itemInstantiaded.SetData("Saltar pregunta");
        for (int i = 0; i < 3; i++)
        {
            var storeItem = Instantiate(_storeItemPrefab, itemInstantiaded.Container);
            float costItem = 0;
            if (i == 0)
            {
                costItem = _powerUpNextQuestion.unitCost;
            }
            else
            {
                costItem = (_powerUpNextQuestion.unitCost * (i + 1) - _powerUpNextQuestion.discount * (i + 1));
            }

            storeItem.SetData(this, costItem, i + 1, _spriteNextQuestion, _powerUpNextQuestion);
        }

        // Mas Tiempo

        itemInstantiaded = Instantiate(_storeSectionPrefab, _GeneralContainer);
        itemInstantiaded.SetData("Aumento de tiempo");
        for (int i = 0; i < 3; i++)
        {
            var storeItem = Instantiate(_storeItemPrefab, itemInstantiaded.Container);
            float costItem = 0;
            if (i == 0)
            {
                costItem = _powerUpMoreTime.unitCost;
            }
            else
            {
                costItem = (_powerUpMoreTime.unitCost * (i + 1) - _powerUpMoreTime.discount * (i + 1));
            }

            storeItem.SetData(this, costItem, i + 1, _spriteMoreTime, _powerUpMoreTime);
        }

        Instantiate(_offset, _GeneralContainer);
        areItemsInstanciated = true;
    }

    private void GameEvents_DetailChanged()
    {
        _powerUpSecondOportunityI.text = _user.userInfo.user.detail.secondChance.ToString();
        _powerUpTrueOptionI.text = _user.userInfo.user.detail.findCorrectAnswer.ToString();
        _powerUpDeleteOptionI.text = _user.userInfo.user.detail.discardOption.ToString();
        _powerUpNextQuestionI.text = _user.userInfo.user.detail.skipQuestion.ToString();
        _powerUpMoreTimeI.text = _user.userInfo.user.detail.increaseTime.ToString();
        GameEvents.CoinsChanged?.Invoke();
        GameEvents.ExperienceChanged?.Invoke();
    }

    private void GameEvents_ExperienceChanged()
    {
        //_totalExperienceLabel.text = _user.userInfo.user.detail.totalExperience.ToString();
    }

    private void GameEvents_CoinsChanged()
    {
        //_totalCoinsLabel.text = _user.userInfo.user.detail.totalCoins.ToString();
        // _totalCoinsLabel.text = _user.userInfo.totalCoins.ToString();
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
        switch (_currentItem.NamePowerUp)
        {
            case "pu_deleteOption":
                _user.userInfo.user.detail.discardOption += _currentItem.Amount;
                break;
            case "pu_moreTime":
                _user.userInfo.user.detail.increaseTime += _currentItem.Amount;
                break;
            case "pu_nextQuestion":
                _user.userInfo.user.detail.skipQuestion += _currentItem.Amount;
                break;
            case "pu_secondOportunity":
                _user.userInfo.user.detail.secondChance += _currentItem.Amount;
                break;
            case "pu_trueOption":
                _user.userInfo.user.detail.findCorrectAnswer += _currentItem.Amount;
                break;
        }

        _user.userInfo.user.detail.totalCoins -= (int)_currentItem.Cost;
        GameEvents.RequestUpdateDetail?.Invoke();
        // _currentItem.PowerUp.amount += _currentItem.Amount;
        // PlayerPrefs.GetInt(_currentItem.PowerUp.nameInPlayerPrefs, _currentItem.PowerUp.amount);
        // PlayerPrefs.Save();
        // _currentItem.PowerUp.Raise();
    }

    public string GetPowerUpName()
    {
        switch (_currentItem.NamePowerUp)
        {
            case "pu_deleteOption":
                return "descartar alternativa";
            case "pu_moreTime":
                return "aumento de tiempo";
            case "pu_nextQuestion":
                return "saltar pregunta";
            case "pu_secondOportunity":
                return "segunda oportunidad";
            case "pu_trueOption":
                return "mostrar respuesta";
            default:
                return "";
        }
    }
}