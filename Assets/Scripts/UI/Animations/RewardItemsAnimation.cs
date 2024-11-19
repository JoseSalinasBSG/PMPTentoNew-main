using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Animations
{
    public class RewardItemsAnimation : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Vector3 _initPosition;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private float multiplier;
        [SerializeField] private float _velocity = 2;
        [SerializeField] private float _delay;

        private float _currentTime;
        
        #endregion

        #region Unity Methods

        private void Start()
        {
            StartAnimation();
        }

        #endregion

        #region Methods

        [ContextMenu("Start animation")]
        public void StartAnimation()
        {
            StopAllCoroutines();
            StartCoroutine(IStartAnimation());
        }
        IEnumerator IStartAnimation()
        {
            var yPosition = _initPosition.y;
            _currentTime = 0;
            while (_currentTime <= 1)
            {
                _currentTime += Time.deltaTime / _delay;
            }
            _currentTime = 0;
            while (_currentTime <= 1)
            {
                _currentTime += Time.deltaTime * _velocity ;
                transform.localPosition = new Vector3(transform.localPosition.x, yPosition + _animationCurve.Evaluate(_currentTime) * multiplier, 0);
                yield return null;
            }
        }

        
        #endregion

    }

}