using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Question
{
    public class OptionVP : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private QuestionInformationVP _questionInformationVp;
        [SerializeField] private Image _image;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _correctColor;
        [SerializeField] private Color _incorrectColor;
        [SerializeField] private EventTrigger _eventTrigger;
        
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

        public TextMeshProUGUI Label
        {
            get => _label;
            set => _label = value;
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
            if (_questionInformationVp.CompareResponse(_id))
            {
                _image.color = _correctColor;
            }
            else
            {
                _image.color = _incorrectColor;
            }
            _label.color = Color.white;
            if (!string.IsNullOrEmpty(_questionInformationVp.Restroalimentacion))
            {
                UIEvents.ShowResponseVP.Invoke(_questionInformationVp.Restroalimentacion);
            }
        }

        public void DisableOption()
        {
            _eventTrigger.enabled = false;
        }
        
        public void EnableOption()
        {
            _label.color = Color.white;
            _image.color = _defaultColor;
            _eventTrigger.enabled = true;
            ShowOption();
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