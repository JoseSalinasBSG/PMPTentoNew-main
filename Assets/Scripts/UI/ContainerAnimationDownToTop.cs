using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class ContainerAnimationDownToTop : MonoBehaviour
    {
        #region Variables

        [SerializeField] private float _height;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _duration;
        // [SerializeField] private float _delay;
        [SerializeField] private UnityEvent _onFinishTransition;
        [SerializeField] private bool _initInStart;
        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, 0);

            if (_initInStart)
            {
                StartTransition();
            }
        }

        

        #endregion

        #region Methods
        
        
        public void StartTransition()
        {
            StartCoroutine(IStartAnimation());
        }

        IEnumerator IStartAnimation()
        {
            var width = _rectTransform.sizeDelta.x;
            var currentTime = 0f;
            // while (currentTime <= _delay)
            // {
            //     currentTime += Time.deltaTime;
            //     yield return null;
            //
            // }
            // currentTime = 0;
            while (currentTime <= 1)
            {
                currentTime += Time.deltaTime / _duration;
                _rectTransform.sizeDelta = new Vector2(width, _height * _animationCurve.Evaluate(currentTime));
                yield return null;

            }
            _onFinishTransition?.Invoke();
        }
        
        #endregion

    }

}