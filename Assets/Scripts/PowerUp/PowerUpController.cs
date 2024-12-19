using System;
using System.Collections;
using System.Collections.Generic;
using DataStorage;
using ScriptableCreator;
using TMPro;
using UnityEngine;

namespace PowerUp
{
    public class PowerUpController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private ScriptableObjectUser _objectUser;
        [SerializeField] private ScripableObjectPowerUp _powerUpSecondOportunity;
        [SerializeField] private ScripableObjectPowerUp _powerUpTrueOption;
        [SerializeField] private ScripableObjectPowerUp _powerUpDeleteOption;
        [SerializeField] private ScripableObjectPowerUp _powerUpNextQuestion;
        [SerializeField] private ScripableObjectPowerUp _powerUpMoreTime;
        
        [SerializeField] private PowerUpListener _powerUpSecondOportunityI;
        [SerializeField] private PowerUpListener _powerUpTrueOptionI;
        [SerializeField] private PowerUpListener _powerUpDeleteOptionI;
        [SerializeField] private PowerUpListener _powerUpNextQuestionI;
        [SerializeField] private PowerUpListener _powerUpMoreTimeI;

        private PowerUpListener _currentListener;

        private DataStorageManager _dataStorageManager;
        #endregion

        #region Unity Methods

        private void Start()
        {
            _dataStorageManager = new DataStorageManager(new PlayerPrefsStorageAdapter());
        }

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
                _powerUpMoreTimeI.Amount = detail.increaseTime;
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
            // _powerUpSecondOportunity.amount--;
            _objectUser.userInfo.user.detail.secondChance--;
            // PlayerPrefs.SetInt("pu_secondOportunity", _powerUpSecondOportunity.amount);
            // PlayerPrefs.Save();
            _currentListener = _powerUpSecondOportunityI;
            GameEvents.RequestUpdateDetail?.Invoke();
            // _powerUpSecondOportunity.Raise();
        }

        public void BuySecondOportunity(int amount )
        {
            _powerUpSecondOportunity.amount += amount;
            //PlayerPrefs.SetInt("pu_secondOportunity", _powerUpSecondOportunity.amount);
            //PlayerPrefs.Save();
            _dataStorageManager.Save("pu_secondOportunity", _powerUpSecondOportunity.amount);
            _powerUpSecondOportunity.Raise();
        }
        
        public void UseTrueOption()
        {
            // _powerUpTrueOption.amount--;
            _objectUser.userInfo.user.detail.findCorrectAnswer--;
            // PlayerPrefs.SetInt("pu_trueOption", _powerUpTrueOption.amount);
            // PlayerPrefs.Save();
            _currentListener = _powerUpTrueOptionI;
            GameEvents.RequestUpdateDetail?.Invoke();
            // _powerUpTrueOption.Raise();
        }

        public void BuyTrueOption(int amount )
        {
            _powerUpTrueOption.amount += amount;
            //PlayerPrefs.SetInt("pu_trueOption", _powerUpTrueOption.amount);
            //PlayerPrefs.Save();
            _dataStorageManager.Save("pu_trueOption", _powerUpTrueOption.amount);
            _powerUpTrueOption.Raise();
        }
        
        public void UseDeleteOption()
        {
            // _powerUpDeleteOption.amount--;
            _objectUser.userInfo.user.detail.discardOption--;

            // PlayerPrefs.SetInt("pu_deleteOption", _powerUpDeleteOption.amount);
            // PlayerPrefs.Save();
            // _powerUpDeleteOption.Raise();
            
            _currentListener = _powerUpDeleteOptionI;
            GameEvents.RequestUpdateDetail?.Invoke();
        }

        public void BuyDeleteOption(int amount )
        {
            _powerUpDeleteOption.amount += amount;
            //PlayerPrefs.SetInt("pu_deleteOption", _powerUpDeleteOption.amount);
            //PlayerPrefs.Save();
            _dataStorageManager.Save("pu_deleteOption", _powerUpDeleteOption.amount);
            _powerUpDeleteOption.Raise();
        }
        
        public void UseNextQuestion()
        {
            // _powerUpNextQuestion.amount--;
            _objectUser.userInfo.user.detail.skipQuestion--;
            // PlayerPrefs.SetInt("pu_nextQuestion", _powerUpNextQuestion.amount);
            // PlayerPrefs.Save();
            // _powerUpNextQuestion.Raise();
            _currentListener = _powerUpNextQuestionI;
            GameEvents.RequestUpdateDetail?.Invoke();
        }

        public void BuyNextQuestion(int amount )
        {
            _powerUpNextQuestion.amount += amount;
            //PlayerPrefs.SetInt("pu_nextQuestion", _powerUpNextQuestion.amount);
            //PlayerPrefs.Save();
            _dataStorageManager.Save("pu_nextQuestion", _powerUpNextQuestion.amount);
            _powerUpNextQuestion.Raise();
        }
        
        public void UseMoreTime()
        {
            // _powerUpMoreTime.amount--;
            _objectUser.userInfo.user.detail.increaseTime--;
            // PlayerPrefs.SetInt("pu_moreTime", _powerUpMoreTime.amount);
            // PlayerPrefs.Save();
            // _powerUpMoreTime.Raise();
            
            _currentListener = _powerUpMoreTimeI;
            GameEvents.RequestUpdateDetail?.Invoke();
        }
        public void BuyMoreTime(int amount )
        {
            _powerUpMoreTime.amount += amount;
            //PlayerPrefs.SetInt("pu_moreTime", _powerUpMoreTime.amount);
            //PlayerPrefs.Save();
            _dataStorageManager.Save("pu_moreTime", _powerUpMoreTime.amount);
            _powerUpMoreTime.Raise();
        }

        
        #endregion

    }

}