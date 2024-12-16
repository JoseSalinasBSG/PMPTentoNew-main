using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Question
{
    public class Option : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private QuestionController _questionController;
        [SerializeField] private Image _image;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _correctColor;
        [SerializeField] private Color _alternativaColor;//color de alternativa
        [SerializeField] private Color _incorrectColor;
        [SerializeField] private EventTrigger _eventTrigger;
        [SerializeField] private UnityEvent OnCorrectAnswered;
        [SerializeField] private UnityEvent OnIncorrectAnswered;
        [SerializeField] private UnityEvent OnOptionEnabled;
        
        private string _id;
        public string ID
        {
            get => _id;
            set => _id = value;
        }
        
        public EventTrigger EventTriggerT
        {
            get => _eventTrigger;
            set => _eventTrigger = value;
        }

        #endregion

        #region Unity Methods

        

        #endregion

        #region Methods

        public void SetData(string description, string id)
        {
            _label.text = description;
            _id = id;
        }

        public void CompareResponse()
        {
            if (_questionController.ValidateResponse(_id))
            {
                SetCorrectColor();
            }
            else
            {
                SetIncorrectColor();
            }
            _label.color = Color.white;
        }

        public void SetCorrectColor()
        {
            _image.color = _correctColor;
            _label.color = Color.white;
            OnCorrectAnswered?.Invoke();
        }

        public void SetIncorrectColor()
        {
            _image.color = _incorrectColor;
            _label.color = Color.white;
            OnIncorrectAnswered?.Invoke();
        }
        public void DisableOption()
        {
            _eventTrigger.enabled = false;
        }
        
        public void EnableOption()
        {
            _label.color = Color.white;
            _image.color = _alternativaColor;
            _eventTrigger.enabled = true;
            ShowOption();
            OnOptionEnabled?.Invoke();
        }

        public void HideOption()
        {
            gameObject.SetActive(false);
        }

        public void ShowOption()
        {
            gameObject.SetActive(true);
        }

        public bool IsDisable()
        {
            return !gameObject.activeInHierarchy;
        }
        #endregion
    }

}