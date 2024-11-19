using Question;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace PowerUp
{
    public class PowerUpTrueOption : PowerUpListener
    {
        [SerializeField] private BombAnimation _starAnimation;
        [SerializeField] private Question.QuestionInformation _questionInformation;
        [SerializeField] private QuestionController _questionController;

        [SerializeField] private UnityEvent _onUsePowerUp;
        private Option _optionChoose;

        public void UsePowerUp()
        {
            if (Amount >0)
            {
                _onUsePowerUp?.Invoke();
            }
        }

        public void ChooseCorrectOption()
        {
            do
            {
                _optionChoose = ChooseOption(Random.Range(0, 4));
                Debug.Log(" seleccion: " + _optionChoose.ID + " -> " +(_questionController.CompareResponse(_optionChoose.ID) || _optionChoose.IsDisable() ));
            } while (!_questionController.CompareResponse(_optionChoose.ID) || _optionChoose.IsDisable());
            _starAnimation.SetTarget(_optionChoose.transform);
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
        
        public void SelectOption(){
            ExecuteEvents.Execute<IPointerClickHandler>(_optionChoose.gameObject,
                new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }
    }
}