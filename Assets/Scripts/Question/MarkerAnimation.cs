using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Question
{
    public class MarkerAnimation : MonoBehaviour
    {
        #region Variables

        [SerializeField] private AnimationCurve _animationCurve;
        private float _currentTime;
        #endregion

        #region Unity Methods

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= 1)
            {
                _currentTime = 0;
            }

            var actualValue = _animationCurve.Evaluate(_currentTime);
            transform.localScale = new Vector3(actualValue, actualValue, 1);
        }

        #endregion

        #region Methods
        
        

        
        #endregion

    }

}