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
            if (_powerUpSecondOportunityI)
            {
                _powerUpSecondOportunityI.Amount = detail.secondChance;                
            }

            if (_powerUpTrueOptionI)
            {
                _powerUpTrueOptionI.Amount = detail.findCorrectAnswer;
            }

            if (_powerUpDeleteOptionI)
            {
                _powerUpDeleteOptionI.Amount = detail.discardOption;
            }

            if (_powerUpNextQuestionI)
            {
                _powerUpNextQuestionI.Amount = detail.skipQuestion;
            }

            if (_powerUpMoreTimeI)
            {
                _powerUpMoreTimeI.Amount = detail.moreTime;
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

        public void UseSecondOportunity()
        {
            _objectUser.userInfo.user.detail.secondChance--;
            _currentListener = _powerUpSecondOportunityI;
            GameEvents.RequestUpdateDetail?.Invoke();
        }

        public void BuySecondOportunity(int amount )
        {
            _powerUpSecondOportunity.amount += amount;
            PlayerPrefs.SetInt("pu_secondOportunity", _powerUpSecondOportunity.amount);
            PlayerPrefs.Save();
            _powerUpSecondOportunity.Raise();
        }
        
        public void UseTrueOption()
        {
            _objectUser.userInfo.user.detail.findCorrectAnswer--;
            _currentListener = _powerUpTrueOptionI;
            GameEvents.RequestUpdateDetail?.Invoke();
        }

        public void BuyTrueOption(int amount )
        {
            _powerUpTrueOption.amount += amount;
            PlayerPrefs.SetInt("pu_trueOption", _powerUpTrueOption.amount);
            PlayerPrefs.Save();
            _powerUpTrueOption.Raise();
        }
        
        public void UseDeleteOption()
        {
            _objectUser.userInfo.user.detail.discardOption--;            
            _currentListener = _powerUpDeleteOptionI;
            GameEvents.RequestUpdateDetail?.Invoke();
        }

        public void BuyDeleteOption(int amount )
        {
            _powerUpDeleteOption.amount += amount;
            PlayerPrefs.SetInt("pu_deleteOption", _powerUpDeleteOption.amount);
            PlayerPrefs.Save();
            _powerUpDeleteOption.Raise();
        }
        
        public void UseNextQuestion()
        {
            _objectUser.userInfo.user.detail.skipQuestion--;
            _currentListener = _powerUpNextQuestionI;
            GameEvents.RequestUpdateDetail?.Invoke();
        }

        public void BuyNextQuestion(int amount )
        {
            _powerUpNextQuestion.amount += amount;
            PlayerPrefs.SetInt("pu_nextQuestion", _powerUpNextQuestion.amount);
            PlayerPrefs.Save();
            _powerUpNextQuestion.Raise();
        }
        
        public void UseMoreTime()
        {
            _objectUser.userInfo.user.detail.moreTime--;            
            _currentListener = _powerUpMoreTimeI;
            GameEvents.RequestUpdateDetail?.Invoke();
        }
        public void BuyMoreTime(int amount )
        {
            _powerUpMoreTime.amount += amount;
            PlayerPrefs.SetInt("pu_moreTime", _powerUpMoreTime.amount);
            PlayerPrefs.Save();
            _powerUpMoreTime.Raise();
        }

        
        #endregion

    }

}