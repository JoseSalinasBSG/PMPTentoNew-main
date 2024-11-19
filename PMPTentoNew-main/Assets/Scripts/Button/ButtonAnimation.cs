using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Button
{
    [RequireComponent(typeof(EventTrigger))]
    public class ButtonAnimation : MonoBehaviour
    {
        [SerializeField] private ScriptableObjectButton _scriptableObjectButton;
        [SerializeField] private Image _imageButton;
        [SerializeField] private EventTrigger _eventTrigger;
        public bool toSound=true;
        private int vibrateIndicator;


        private float _currentTime;
        private Vector3 _startScale;
        private Color _colorDisable = new Color(.8f , .8f, .8f, 1);
        private Color _colorDefault = Color.white;
        EventTrigger.Entry entry = new EventTrigger.Entry();

        public enum VibrationMode
        {
            None,
            LightVibration,
            MediumVibration,
            HeavyVibration,               
        }
        [SerializeField] VibrationMode vibrationMode;

        private void Start()
        {
            _startScale = transform.localScale;
            switch (vibrationMode)
            {
                case VibrationMode.None:
                    vibrateIndicator = 0;
                    break;
                case VibrationMode.LightVibration:
                    vibrateIndicator = 1;
                    break;
                case VibrationMode.MediumVibration:
                    vibrateIndicator = 2;
                    break;
                case VibrationMode.HeavyVibration:
                    vibrateIndicator = 3;
                    break;
                default:
                    break;
            }
        }

        private void OnEnable()
        {  
            
            entry.eventID = EventTriggerType.PointerDown;//establece tipo de evento a escuchar,
            entry.callback.AddListener(Call);//se a�ade a la lista de metodos que se ejecutaran cuando el evento a escuchar se dispare, cuando haya evento PointerDown se llamara a Call
            _eventTrigger.triggers.Add(entry);

            
        }

        private void Call(BaseEventData arg0)
        {
            if (toSound)
            {
                UIEvents.PressLoginButton?.Invoke();
            }
            UIEvents.PressVibrateButton?.Invoke(vibrateIndicator);
            // Debug.Log("call");
        }

        private void OnDisable()
        {
            
            entry.callback.RemoveListener(Call);
            _eventTrigger.triggers.Remove(entry);
            
        }
        

        public void StartAnimation()
        {
            _currentTime = 0;
            StopAllCoroutines();
            StartCoroutine(IStartAnimation());
        }
        public void StartInverseCustomAnimation()
        {
            _currentTime = 0;
            StopAllCoroutines();
            StartCoroutine(IStartInverseCustomAnimation());
        }
        public void StartInverseAnimation()
        {
            _currentTime = 0;
            StopAllCoroutines();
            StartCoroutine(IStartInverseAnimation());
        }

        public void EnableButton()
        {
            _eventTrigger.enabled = true;
            _imageButton.color = _colorDefault;
        }

        public void DisableButton()
        {
            _eventTrigger.enabled = false;
            _imageButton.color = _colorDisable;
        }
    



        IEnumerator IStartAnimation()
        {
            while (_currentTime <= 1)
            {
                _currentTime += Time.deltaTime / _scriptableObjectButton.TimeDuration;
                transform.localScale = _startScale * _scriptableObjectButton.AnimationCurve.Evaluate(_currentTime);
                yield return null;
            }
        }
        IEnumerator IStartInverseCustomAnimation()
        {
            while (_currentTime <= 1)
            {
                _currentTime += Time.deltaTime / _scriptableObjectButton.TimeDuration;
                transform.localScale = _startScale * _scriptableObjectButton.AnimationInverseCurve.Evaluate(1 - _currentTime);
                yield return null;
            }
        }
        IEnumerator IStartInverseAnimation()
        {
            while (_currentTime <= 1)
            {
                _currentTime += Time.deltaTime / _scriptableObjectButton.TimeDuration;
                transform.localScale = _startScale * _scriptableObjectButton.AnimationCurve.Evaluate(1 - _currentTime);
                yield return null;
            }
        }
    
    }
}
