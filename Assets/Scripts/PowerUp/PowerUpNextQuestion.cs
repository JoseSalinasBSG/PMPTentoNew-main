using Question;
using UnityEngine;
using UnityEngine.Events;

namespace PowerUp
{
    public class PowerUpNextQuestion : PowerUpListener
    {
        [SerializeField] private UnityEvent _onUsePowerUp;
        [SerializeField] private QuestionController _questionController;
        public void UsePowerUp()
        {
            if (Amount >0 && _questionController.GetCountSession > _questionController.CurrentIndex)
            {
                _onUsePowerUp?.Invoke();
            }
        }
        
    }
}