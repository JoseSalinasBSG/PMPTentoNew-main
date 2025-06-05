using System.Collections;
using System.Collections.Generic;
using Question;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PowerUp
{
    public class PowerUpDeleteOption : PowerUpListener
    {
        [SerializeField] private BombAnimation _bombAnimation;
        [SerializeField] private QuestionInformation _questionInformation;
        [SerializeField] private QuestionController _questionController;

        [SerializeField] private UnityEvent _onUsePowerUp;
        private Option _optionChoose;
        private int _numUses;
        public void ChooseOption()
        {
            do
            {
                _optionChoose = ChooseOption(Random.Range(0, 3));
                Debug.Log(" seleccion: " + _optionChoose.ID + " -> " +(_questionController.CompareResponse(_optionChoose.ID) || _optionChoose.IsDisable() ));
            } while (_questionController.CompareResponse(_optionChoose.ID) || _optionChoose.IsDisable());
            _numUses++;
            _bombAnimation.SetTarget(_optionChoose.transform);
            Debug.Log(" elegido: " + _optionChoose.ID + " " + _optionChoose.IsDisable());
        }
        public void DeleteOption(){
            _optionChoose.HideOption();
        }

        public void ResetUses()
        {
            _numUses = 0;
        }
        public void UsePowerUp()
        {
            if (_numUses <1 && Amount >0)
            {
                _onUsePowerUp?.Invoke();
            }
        }

        public Option ChooseOption(int id)
        {
            switch (id)
            {
                case 0:
                    return _questionInformation.Opt1;
                case 1:
                    return _questionInformation.Opt2;
                case 2:
                    return _questionInformation.Opt3;
                case 3:
                    return _questionInformation.Opt4;
            }

            return null;
        }
    }
}