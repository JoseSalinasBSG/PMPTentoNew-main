using ScriptableCreator.PowerUpSOC;
using UnityEngine;

namespace PowerUp
{
    public class PowerUpController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private ScriptableObjectUser _objectUser;
        [SerializeField] private PowerUpSO _powerUpSecondOportunity;
        [SerializeField] private PowerUpSO _powerUpTrueOption;
        [SerializeField] private PowerUpSO _powerUpDeleteOption;
        [SerializeField] private PowerUpSO _powerUpNextQuestion;
        [SerializeField] private PowerUpSO _powerUpMoreTime;
        [SerializeField] private PowerUpListener _powerUpSecondOportunityI;
        [SerializeField] private PowerUpListener _powerUpTrueOptionI;
        [SerializeField] private PowerUpListener _powerUpDeleteOptionI;
        [SerializeField] private PowerUpListener _powerUpNextQuestionI;
        [SerializeField] private PowerUpListener _powerUpMoreTimeI;

        private PowerUpListener _currentListener;
        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            UpdateAmounts();
            GameEvents.DetailChanged += GameEvents_DetailChanged;
        }

        private void UpdateAmounts()
        {
            var detail = _objectUser.userInfo.user.detail;
            UpdatePowerUpAmount(_powerUpSecondOportunityI, detail.secondChance);
            UpdatePowerUpAmount(_powerUpTrueOptionI, detail.findCorrectAnswer);
            UpdatePowerUpAmount(_powerUpDeleteOptionI, detail.discardOption);
            UpdatePowerUpAmount(_powerUpNextQuestionI, detail.skipQuestion);
            UpdatePowerUpAmount(_powerUpMoreTimeI, detail.increaseTime);
        }

        private void UpdatePowerUpAmount(PowerUpListener listener, int amount)
        {
            if (listener != null)
            {
                listener.Amount = amount;
            }
        }

        private void OnDisable()
        {
            GameEvents.DetailChanged -= GameEvents_DetailChanged;
        }
        private void GameEvents_DetailChanged()
        {
            if (_currentListener)
            {
                UpdateAmounts();
                _currentListener.OnEventRaised();
                _currentListener = null;
            }

        }

        #endregion

        #region Methods


        public void UsePowerUp(PowerUpListener listener, ref int detailAmount)
        {
            detailAmount--;
            _currentListener = listener;
            GameEvents.RequestUpdateDetail?.Invoke();
        }

        public void BuyPowerUp(PowerUpSO powerUp, int amount, string playerPrefsKey)
        {
            powerUp.amount += amount;
            PlayerPrefs.SetInt(playerPrefsKey, powerUp.amount);
            PlayerPrefs.Save();
            powerUp.Raise();
        }

        public void UseSecondOportunity() => UsePowerUp(_powerUpSecondOportunityI, ref _objectUser.userInfo.user.detail.secondChance);
        public void BuySecondOportunity(int amount) => BuyPowerUp(_powerUpSecondOportunity, amount, _powerUpSecondOportunity.nameInPlayerPrefs);

        public void UseTrueOption() => UsePowerUp(_powerUpTrueOptionI, ref _objectUser.userInfo.user.detail.findCorrectAnswer);
        public void BuyTrueOption(int amount) => BuyPowerUp(_powerUpTrueOption, amount, _powerUpTrueOption.nameInPlayerPrefs);

        public void UseDeleteOption() => UsePowerUp(_powerUpDeleteOptionI, ref _objectUser.userInfo.user.detail.discardOption);
        public void BuyDeleteOption(int amount) => BuyPowerUp(_powerUpDeleteOption, amount, _powerUpDeleteOption.nameInPlayerPrefs);

        public void UseNextQuestion() => UsePowerUp(_powerUpNextQuestionI, ref _objectUser.userInfo.user.detail.skipQuestion);
        public void BuyNextQuestion(int amount) => BuyPowerUp(_powerUpNextQuestion, amount, _powerUpNextQuestion.nameInPlayerPrefs);

        public void UseMoreTime() => UsePowerUp(_powerUpMoreTimeI, ref _objectUser.userInfo.user.detail.increaseTime);
        public void BuyMoreTime(int amount) => BuyPowerUp(_powerUpMoreTime, amount, _powerUpMoreTime.nameInPlayerPrefs);



        #endregion

    }

}