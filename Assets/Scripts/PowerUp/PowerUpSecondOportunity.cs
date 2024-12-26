using Question;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PowerUp
{
    public class PowerUpSecondOportunity : PowerUpListener
    {
        [SerializeField] private QuestionInformation _questionInformation;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _deactiveColor;
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private TextMeshProUGUI _labelButton;
        [SerializeField] private Image _imageButton;
        [SerializeField] private EventTrigger _eventTrigger;
        public void UseSecondOportunity()
        {
            _questionInformation.EnableOptions();
            if (Amount > 0)
            {
                _imageButton.color = _activeColor;
                _labelButton.color = _activeColor;
                _message.text = "¿Necesitas una segunda oportunidad? ¡Usa tu potenciador y da lo mejor de ti!";
                _eventTrigger.enabled = true;
            }
            else
            {
                _imageButton.color = _deactiveColor;
                _labelButton.color = _deactiveColor;
                _message.text = "Ya no te quedan más potenciadores para otra oportunidad";
                _eventTrigger.enabled = false;
            }
        }

        public void ComprovePower()
        {
            if (Amount > 0)
            {
                _imageButton.color = _activeColor;
                _labelButton.color = _activeColor;
                _message.text = "¿Necesitas una segunda oportunidad? ¡Usa tu potenciador y da lo mejor de ti!";
                _eventTrigger.enabled = true;
            }
            else
            {
                _imageButton.color = _deactiveColor;
                _labelButton.color = _deactiveColor;
                _message.text = "Ya no te quedan más potenciadores para otra oportunidad";
                _eventTrigger.enabled = false;
            }
        }
    }

}